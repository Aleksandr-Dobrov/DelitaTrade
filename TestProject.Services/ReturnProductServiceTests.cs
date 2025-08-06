using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Services;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.IdentityModel.Tokens.Configuration;
using MockQueryable;
using Moq;
using System.Threading.Tasks;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace TestProject.Services
{
    [TestFixture]
    public class ReturnProductServiceTests
    {
        private Mock<IRepository> _repositoryMock;
        private Mock<IDescriptionCategoryService> _descriptionCategoryServiceMock;
        private IReturnProductService _returnProductService;

        [SetUp]
        public void Setup() 
        {
            _repositoryMock = new Mock<IRepository>();
            _descriptionCategoryServiceMock = new Mock<IDescriptionCategoryService>();
            _returnProductService = new ReturnProductService(_repositoryMock.Object, _descriptionCategoryServiceMock.Object);
        }

        [Test]
        public void AlwaysPass()
        {
            Assert.Pass();
        }

        [Test]
        public void TestAddProductAsyncThrowArgumentNullExceptionWhenProtocolNotFound()
        {
            var productToAdd = new ReturnedProductViewModel()
            {
                Batch = "123",
                Product = new ProductViewModel() 
                {
                    Name = "Test",
                    Unit = "kg"
                },
                ReturnProtocolId = 1,
                DescriptionCategory = new DescriptionCategoryViewModel() 
                {
                    Id = 1,
                    Name = "Test"
                }
            };

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(productToAdd.ReturnProtocolId))
                .ReturnsAsync((ReturnProtocol?)null);

            Assert.ThrowsAsync<ArgumentNullException>(() => _returnProductService.AddProductAsync(productToAdd, productToAdd.ReturnProtocolId));
        }

        [Test]
        public void TestAddProductAsyncThrowInvalidOperationExceptionWhenProtocolIsApproved()
        {
            int protocolId = 1;
            var productToAdd = new ReturnedProductViewModel()
            {
                Batch = "123",
                Product = new ProductViewModel()
                {
                    Name = "Test",
                    Unit = "kg"
                },
                ReturnProtocolId = protocolId,
                DescriptionCategory = new DescriptionCategoryViewModel()
                {
                    Id = 1,
                    Name = "Test"
                }
            };
            var existingProtocol = new ReturnProtocol
            {
                Id = protocolId,
                PayMethod = "OldPayMethod",
                Object = new CompanyObject
                {
                    Id = 2,
                    Name = "Old name",
                    TraderId = 2,
                    Trader = new Trader { Id = 2, Name = "Old Trader", PhoneNumber = "0987654321" },
                    CompanyId = 2,
                    Company = new Company { Id = 2, Name = "Old Company" }
                },
                ReturnedDate = DateTime.MinValue,
                TraderId = 2,
                IdentityUser = new DelitaUser { Id = Guid.NewGuid(), UserName = "Owner" },
                ApproverId = Guid.NewGuid(),
                Approver = new DelitaUser()
            };

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolId))
                .ReturnsAsync(existingProtocol);

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { existingProtocol }.BuildMock());
           

            Assert.ThrowsAsync<InvalidOperationException>(() => _returnProductService.AddProductAsync(productToAdd, productToAdd.ReturnProtocolId));
        }

        [Test]
        public void TestAddProductAsyncThrowArgumentNullExceptionWhenProductNotFound()
        {
            int protocolId = 1;
            var productToAdd = new ReturnedProductViewModel()
            {
                Batch = "123",
                Product = new ProductViewModel()
                {
                    Name = "Test",
                    Unit = "kg"
                },
                ReturnProtocolId = protocolId,
                DescriptionCategory = new DescriptionCategoryViewModel()
                {
                    Id = 1,
                    Name = "Test"
                }
            };
            var existingProtocol = new ReturnProtocol
            {
                Id = protocolId,
                PayMethod = "OldPayMethod",
                Object = new CompanyObject
                {
                    Id = 2,
                    Name = "Old name",
                    TraderId = 2,
                    Trader = new Trader { Id = 2, Name = "Old Trader", PhoneNumber = "0987654321" },
                    CompanyId = 2,
                    Company = new Company { Id = 2, Name = "Old Company" }
                },
                ReturnedDate = DateTime.MinValue,
                TraderId = 2,
                IdentityUser = new DelitaUser { Id = Guid.NewGuid(), UserName = "Owner" },
            };

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolId))
                .ReturnsAsync(existingProtocol);

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { existingProtocol }.BuildMock());

            _repositoryMock.Setup(r => r.All<Product>())
                .Returns(new List<Product>().BuildMock());


            Assert.ThrowsAsync<ArgumentNullException>(() => _returnProductService.AddProductAsync(productToAdd, productToAdd.ReturnProtocolId));
        }

        [Test]
        public void TestAddProductAsyncThrowArgumentNullExceptionWhenDescriptionCategoryNotFound()
        {
            int protocolId = 1;
            var productToAdd = new ReturnedProductViewModel()
            {
                Batch = "123",
                Product = new ProductViewModel()
                {
                    Name = "Test",
                    Unit = "kg"
                },
                ReturnProtocolId = protocolId,
                DescriptionCategory = new DescriptionCategoryViewModel()
                {
                    Id = 1,
                    Name = "Test"
                }
            };
            var existingProtocol = new ReturnProtocol
            {
                Id = protocolId,
                PayMethod = "OldPayMethod",
                Object = new CompanyObject
                {
                    Id = 2,
                    Name = "Old name",
                    TraderId = 2,
                    Trader = new Trader { Id = 2, Name = "Old Trader", PhoneNumber = "0987654321" },
                    CompanyId = 2,
                    Company = new Company { Id = 2, Name = "Old Company" }
                },
                ReturnedDate = DateTime.MinValue,
                TraderId = 2,
                IdentityUser = new DelitaUser { Id = Guid.NewGuid(), UserName = "Owner" },
            };

            var existingProduct = new Product()
            {
                Name = "Test",
                Unit = "kg"
            };

            int descriptionCategoryId = 1;

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolId))
                .ReturnsAsync(existingProtocol);

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { existingProtocol }.BuildMock());

            _repositoryMock.Setup(r => r.All<Product>())
                .Returns(new List<Product>() { existingProduct }.BuildMock());

            _repositoryMock.Setup(r => r.GetByIdAsync<DescriptionCategory>(descriptionCategoryId))
                .ReturnsAsync((DescriptionCategory?)null);

            Assert.ThrowsAsync<ArgumentNullException>(() => _returnProductService.AddProductAsync(productToAdd, productToAdd.ReturnProtocolId));
        }

        [Test]
        public async Task TestAddProductAsync()
        {
            int protocolId = 1;
            var productToAdd = new ReturnedProductViewModel()
            {
                Batch = "123",
                Product = new ProductViewModel()
                {
                    Name = "Test",
                    Unit = "kg"
                },
                ReturnProtocolId = protocolId,
                DescriptionCategory = new DescriptionCategoryViewModel()
                {
                    Id = 1,
                    Name = "Test"
                }
            };
            var lastChange = new DateTime(2025,1,1);

            var existingProtocol = new ReturnProtocol
            {
                Id = protocolId,
                PayMethod = "OldPayMethod",
                Object = new CompanyObject
                {
                    Id = 2,
                    Name = "Old name",
                    TraderId = 2,
                    Trader = new Trader { Id = 2, Name = "Old Trader", PhoneNumber = "0987654321" },
                    CompanyId = 2,
                    Company = new Company { Id = 2, Name = "Old Company" }
                },
                ReturnedDate = DateTime.MinValue,
                LastChanged = lastChange,
                TraderId = 2,
                IdentityUser = new DelitaUser { Id = Guid.NewGuid(), UserName = "Owner" },
            };

            var existingProduct = new Product()
            {
                Name = "Test",
                Unit = "kg"
            };

            int descriptionCategoryId = 1;

            var descriptionCategory = new DescriptionCategory()
            {
                Id = descriptionCategoryId,
                Name = "Test"
            };

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolId))
                .ReturnsAsync(existingProtocol);

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { existingProtocol }.BuildMock());

            _repositoryMock.Setup(r => r.All<Product>())
                .Returns(new List<Product>() { existingProduct }.BuildMock());

            _repositoryMock.Setup(r => r.GetByIdAsync<DescriptionCategory>(descriptionCategoryId))
                .ReturnsAsync(descriptionCategory);

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);
            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1);

            var result = await _returnProductService.AddProductAsync(productToAdd, protocolId);

            Assert.That(existingProtocol.LastChanged, Is.Not.EqualTo(lastChange));
            Assert.That(result, Is.EqualTo(0));

            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<ReturnedProduct>()), Times.Once);
        }

        [Test]
        public async Task TestAddProductAsyncWithUser()
        {
            int protocolId = 1;
            int descriptionCategoryId = 1;

            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test User"
            };

            var productToAdd = new ReturnedProductInputModel()
            {
                Batch = "123",
                ProductName = "Test",
                Unit = "kg",                
                ReturnProtocolId = protocolId,
                DescriptionCategoryId = descriptionCategoryId
            };

            var lastChange = new DateTime(2025, 1, 1);

            var existingProtocol = new ReturnProtocol
            {
                Id = protocolId,
                PayMethod = "OldPayMethod",
                Object = new CompanyObject
                {
                    Id = 2,
                    Name = "Old name",
                    TraderId = 2,
                    Trader = new Trader { Id = 2, Name = "Old Trader", PhoneNumber = "0987654321" },
                    CompanyId = 2,
                    Company = new Company { Id = 2, Name = "Old Company" }
                },
                ReturnedDate = DateTime.MinValue,
                LastChanged = lastChange,
                TraderId = 2,
                IdentityUser = new DelitaUser { Id = user.Id, UserName = user.Name },
                IdentityUserId = user.Id,
            };

            var existingProduct = new Product()
            {
                Name = "Test",
                Unit = "kg"
            };

            var descriptionCategoryViewModel = new DescriptionCategoryViewModel()
            {
                Id = descriptionCategoryId,
                Name = "Test"
            };

            var descriptionCategory = new DescriptionCategory()
            {
                Id = descriptionCategoryId,
                Name = "Test"
            };

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolId))
                .ReturnsAsync(existingProtocol);

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { existingProtocol }.BuildMock());

            _repositoryMock.Setup(r => r.All<Product>())
                .Returns(new List<Product>() { existingProduct }.BuildMock());

            _repositoryMock.Setup(r => r.GetByIdAsync<DescriptionCategory>(descriptionCategoryId))
                .ReturnsAsync(descriptionCategory);

            _descriptionCategoryServiceMock.Setup(r => r.GetByIdAsync(descriptionCategoryId))
                .ReturnsAsync(descriptionCategoryViewModel);

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);
            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1);

            var result = await _returnProductService.AddProductAsync(productToAdd, protocolId, user);

            Assert.That(existingProtocol.LastChanged, Is.Not.EqualTo(lastChange));
            Assert.That(result, Is.EqualTo(0));

            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<ReturnedProduct>()), Times.Once);
        }

        [Test]
        public void TestAddProductAsyncWithUserThrowUnauthorizedAccessException()
        {
            int protocolId = 1;
            int descriptionCategoryId = 1;

            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test User"
            };

            var productToAdd = new ReturnedProductInputModel()
            {
                Batch = "123",
                ProductName = "Test",
                Unit = "kg",
                ReturnProtocolId = protocolId,
                DescriptionCategoryId = descriptionCategoryId
            };

            var lastChange = new DateTime(2025, 1, 1);

            var existingProtocol = new ReturnProtocol
            {
                Id = protocolId,
                PayMethod = "OldPayMethod",
                Object = new CompanyObject
                {
                    Id = 2,
                    Name = "Old name",
                    TraderId = 2,
                    Trader = new Trader { Id = 2, Name = "Old Trader", PhoneNumber = "0987654321" },
                    CompanyId = 2,
                    Company = new Company { Id = 2, Name = "Old Company" }
                },
                ReturnedDate = DateTime.MinValue,
                LastChanged = lastChange,
                TraderId = 2,
                IdentityUser = new DelitaUser { Id = Guid.NewGuid(), UserName = user.Name },
                IdentityUserId = Guid.NewGuid(),
            };

            var existingProduct = new Product()
            {
                Name = "Test",
                Unit = "kg"
            };

            var descriptionCategoryViewModel = new DescriptionCategoryViewModel()
            {
                Id = descriptionCategoryId,
                Name = "Test"
            };

            var descriptionCategory = new DescriptionCategory()
            {
                Id = descriptionCategoryId,
                Name = "Test"
            };

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { existingProtocol }.BuildMock());
            
            Assert.ThrowsAsync<UnauthorizedAccessException>(() => _returnProductService.AddProductAsync(productToAdd, protocolId, user));
        }

        [Test]
        public async Task TestGetAllProductsAsyncReturnsAllFromProtocol()
        {
            var product = new Product()
            {
                Name = "Test",
                Unit = "kg"
            };

            var descriptionCategory = new DescriptionCategory()
            {
                Id = 1,
                Name = "No reason"
            };

            int protocolId = 1;

            int secondProtocolId = 2;

            List<ReturnedProduct> products =
            [
                new()
                {
                    Id = 1,
                    Product = product,
                    DescriptionCategory = descriptionCategory,
                    ReturnProtocolId = protocolId,
                    Batch = "124"
                },
                new(){
                    Id = 2,
                    Product = product,
                    DescriptionCategory = descriptionCategory,
                    ReturnProtocolId = protocolId,
                    Batch = "125"
                }, new()
                {
                    Id = 3,
                    Product = product,
                    DescriptionCategory = descriptionCategory,
                    ReturnProtocolId = secondProtocolId,
                    Batch = "126"
                },
                new(){
                    Id = 4,
                    Product = product,
                    DescriptionCategory = descriptionCategory,
                    ReturnProtocolId= secondProtocolId,
                    Batch = "127"
                }
            ];

            _repositoryMock.Setup(r => r.AllReadonly<ReturnedProduct>())
                .Returns(products.BuildMock());

            var result = await _returnProductService.GetAllProductsAsync(protocolId);

            IEnumerable<int> comparer = new List<int>() { 1, 2 };

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.Select(p => p.Id), Is.EqualTo(comparer));
            Assert.That(result.First().Product.Name, Is.EqualTo(product.Name));
            Assert.That(result.First().Product.Unit, Is.EqualTo(product.Unit));
            Assert.That(result.First().DescriptionCategory.Id, Is.EqualTo(descriptionCategory.Id));
            Assert.That(result.First().DescriptionCategory.Name, Is.EqualTo(descriptionCategory.Name));
        }

        [Test]
        public async Task TestGetAllProductsAsyncReturnsAllFromProtocolWithDescriptionNull()
        {
            var product = new Product()
            {
                Name = "Test",
                Unit = "kg"
            };

            var descriptionCategory = new DescriptionCategory()
            {
                Id = 1,
                Name = "No reason"
            };

            int protocolId = 1;

            List<ReturnedProduct> products =
            [
                new()
                {
                    Id = 1,
                    Product = product,
                    DescriptionCategory = descriptionCategory,
                    ReturnProtocolId = protocolId,
                    Batch = "124"
                },
                new(){
                    Id = 2,
                    Product = product,
                    DescriptionCategory = descriptionCategory,
                    ReturnProtocolId = protocolId,
                    Batch = "125"
                }
            ];

            _repositoryMock.Setup(r => r.AllReadonly<ReturnedProduct>())
                .Returns(products.BuildMock());

            var result = await _returnProductService.GetAllProductsAsync(protocolId);

           

            Assert.That(result, Is.Not.Null);
            Assert.That(result.First().Description, Is.Null);
        }

        [Test]
        public async Task TestGetAllProductsAsyncReturnsAllFromProtocolWithDescriptionNotNull()
        {
            var product = new Product()
            {
                Name = "Test",
                Unit = "kg"
            };

            var description = new ReturnedProductDescription()
            {
                Id = 1,
                Description = "Something is wrong"
            };

            var descriptionCategory = new DescriptionCategory()
            {
                Id = 1,
                Name = "No reason"
            };

            int protocolId = 1;

            List<ReturnedProduct> products =
            [
                new()
                {
                    Id = 1,
                    Product = product,
                    DescriptionCategory = descriptionCategory,
                    Description = description,
                    ReturnProtocolId = protocolId,
                    Batch = "124"
                },
                new(){
                    Id = 2,
                    Product = product,
                    DescriptionCategory = descriptionCategory,
                    Description = description,
                    ReturnProtocolId = protocolId,
                    Batch = "125"
                }
            ];

            _repositoryMock.Setup(r => r.AllReadonly<ReturnedProduct>())
                .Returns(products.BuildMock());

            var result = await _returnProductService.GetAllProductsAsync(protocolId);



            Assert.That(result, Is.Not.Null);
            Assert.That(result.First().Description, Is.Not.Null);
            Assert.That(result.First().Description.Id, Is.EqualTo(description.Id));
            Assert.That(result.First().Description.Description, Is.EqualTo(description.Description));
        }

        [Test]
        public async Task TestGetProductByIdAsyncReturnsProduct()
        {
            var product = new Product()
            {
                Name = "Test",
                Unit = "kg"
            };

            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test User"
            };

            var descriptionCategory = new DescriptionCategory()
            {
                Id = 1,
                Name = "No reason"
            };

            int protocolId = 1;

            var existingProtocol = new ReturnProtocol
            {
                Id = protocolId,
                PayMethod = "OldPayMethod",
                Object = new CompanyObject
                {
                    Id = 2,
                    Name = "Old name",
                    TraderId = 2,
                    Trader = new Trader { Id = 2, Name = "Old Trader", PhoneNumber = "0987654321" },
                    CompanyId = 2,
                    Company = new Company { Id = 2, Name = "Old Company" }
                },
                ReturnedDate = DateTime.MinValue,
                TraderId = 2,
                IdentityUser = new DelitaUser { Id = user.Id, UserName = user.Name },
                IdentityUserId = user.Id,
            };

            int productId = 1;

            var returnedProduct = new ReturnedProduct()
            {
                Id = productId,
                Product = product,
                DescriptionCategory = descriptionCategory,
                ReturnProtocolId = protocolId,
                ReturnProtocol = existingProtocol,
                Batch = "124"
            };

            _repositoryMock.Setup(r => r.AllReadonly<ReturnedProduct>())
                .Returns(new List<ReturnedProduct>(){ returnedProduct }.BuildMock());

            var result = await _returnProductService.GetProductByIdAsync(productId, user);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(productId));
            Assert.That(result.Product.Name, Is.EqualTo(product.Name));
            Assert.That(result.Description, Is.Null);
            Assert.That(result.DescriptionCategory.Name, Is.EqualTo(descriptionCategory.Name));  
        }

        [Test]
        public async Task TestGetProductByIdAsyncReturnsProductWithDescription()
        {
            var product = new Product()
            {
                Name = "Test",
                Unit = "kg"
            };

            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test User"
            };

            var description = new ReturnedProductDescription()
            {
                Id = 1,
                Description = "Something is wrong"
            };

            var descriptionCategory = new DescriptionCategory()
            {
                Id = 1,
                Name = "No reason"
            };

            int protocolId = 1;

            var existingProtocol = new ReturnProtocol
            {
                Id = protocolId,
                PayMethod = "OldPayMethod",
                Object = new CompanyObject
                {
                    Id = 2,
                    Name = "Old name",
                    TraderId = 2,
                    Trader = new Trader { Id = 2, Name = "Old Trader", PhoneNumber = "0987654321" },
                    CompanyId = 2,
                    Company = new Company { Id = 2, Name = "Old Company" }
                },
                ReturnedDate = DateTime.MinValue,
                TraderId = 2,
                IdentityUser = new DelitaUser { Id = user.Id, UserName = user.Name },
                IdentityUserId = user.Id,
            };

            int productId = 1;

            var returnedProduct = new ReturnedProduct()
            {
                Id = productId,
                Product = product,
                DescriptionCategory = descriptionCategory,
                Description = description,
                ReturnProtocolId = protocolId,
                ReturnProtocol = existingProtocol,
                Batch = "124"
            };

            _repositoryMock.Setup(r => r.AllReadonly<ReturnedProduct>())
                .Returns(new List<ReturnedProduct>() { returnedProduct }.BuildMock());

            var result = await _returnProductService.GetProductByIdAsync(productId, user);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(productId));
            Assert.That(result.Product.Name, Is.EqualTo(product.Name));
            Assert.That(result.Description, Is.Not.Null);
            Assert.That(result.Description.Description, Is.EqualTo(description.Description));
            Assert.That(result.DescriptionCategory.Name, Is.EqualTo(descriptionCategory.Name));
        }

        [Test]
        public async Task TestGetProductByIdAsyncReturnsNullWhenUserIsNotOwnerOfProtocol()
        {
            var product = new Product()
            {
                Name = "Test",
                Unit = "kg"
            };

            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test User"
            };

            var descriptionCategory = new DescriptionCategory()
            {
                Id = 1,
                Name = "No reason"
            };

            int protocolId = 1;

            var existingProtocol = new ReturnProtocol
            {
                Id = protocolId,
                PayMethod = "OldPayMethod",
                Object = new CompanyObject
                {
                    Id = 2,
                    Name = "Old name",
                    TraderId = 2,
                    Trader = new Trader { Id = 2, Name = "Old Trader", PhoneNumber = "0987654321" },
                    CompanyId = 2,
                    Company = new Company { Id = 2, Name = "Old Company" }
                },
                ReturnedDate = DateTime.MinValue,
                TraderId = 2,
                IdentityUser = new DelitaUser { Id = Guid.NewGuid(), UserName = "Other user" },
                IdentityUserId = Guid.NewGuid(),
            };

            int productId = 1;

            var returnedProduct = new ReturnedProduct()
            {
                Id = productId,
                Product = product,
                DescriptionCategory = descriptionCategory,
                ReturnProtocolId = protocolId,
                ReturnProtocol = existingProtocol,
                Batch = "124"
            };

            _repositoryMock.Setup(r => r.AllReadonly<ReturnedProduct>())
                .Returns(new List<ReturnedProduct>() { returnedProduct }.BuildMock());

            var result = await _returnProductService.GetProductByIdAsync(productId, user);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void TestUpdateProductAsyncThrowsNullArgumentExceptionWhenReturnProductNotFound()
        {
            var product = new ProductViewModel()
            {
                Name = "product",
                Unit = "kg"
            };

            var descriptionCategory = new DescriptionCategoryViewModel()
            {
                Id = 1,
                Name = "Description",
            };

            var returnProduct = new ReturnedProductViewModel()
            {
                Id = 1,
                Batch = "123",
                Product = product,
                DescriptionCategory = descriptionCategory,

            };

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnedProduct>(returnProduct.Id))
                .ReturnsAsync((ReturnedProduct?)null);

            Assert.ThrowsAsync<ArgumentNullException>(() => _returnProductService.UpdateProductAsync(returnProduct));               
        }

        [Test]
        public void TestUpdateProductAsyncThrowsNullArgumentExceptionWhenReturnProtocolNotFound()
        {
            var productViewModel = new ProductViewModel()
            {
                Name = "product",
                Unit = "kg"
            };

            var descriptionCategoryViewModel = new DescriptionCategoryViewModel()
            {
                Id = 1,
                Name = "Description",
            };

            var returnProduct = new ReturnedProductViewModel()
            {
                Id = 1,
                Batch = "123",
                Product = productViewModel,
                DescriptionCategory = descriptionCategoryViewModel,

            };

            var product = new Product()
            {
                Name = "Test",
                Unit = "kg"
            };

            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test User"
            };

            var description = new ReturnedProductDescription()
            {
                Id = 1,
                Description = "Something is wrong"
            };

            var descriptionCategory = new DescriptionCategory()
            {
                Id = 1,
                Name = "No reason"
            };
                        
            int protocolId = 1;

            var existingProtocol = new ReturnProtocol
            {
                Id = protocolId,
                PayMethod = "OldPayMethod",
                Object = new CompanyObject
                {
                    Id = 2,
                    Name = "Old name",
                    TraderId = 2,
                    Trader = new Trader { Id = 2, Name = "Old Trader", PhoneNumber = "0987654321" },
                    CompanyId = 2,
                    Company = new Company { Id = 2, Name = "Old Company" }
                },
                ReturnedDate = DateTime.MinValue,
                TraderId = 2,
                IdentityUser = new DelitaUser { Id = user.Id, UserName = user.Name },
                IdentityUserId = user.Id,
            };

            int productId = 1;

            var returnedProduct = new ReturnedProduct()
            {
                Id = productId,
                Product = product,
                DescriptionCategory = descriptionCategory,
                ReturnProtocolId = protocolId,
                ReturnProtocol = existingProtocol,
                Batch = "124"
            };

            existingProtocol.ReturnedProducts = [returnedProduct];

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnedProduct>(returnProduct.Id))
                .ReturnsAsync(returnedProduct);

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolId))
                .ReturnsAsync((ReturnProtocol?)null);

            Assert.ThrowsAsync<ArgumentNullException>(() => _returnProductService.UpdateProductAsync(returnProduct));
        }

        [Test]
        public void TestUpdateProductAsyncThrowsNullArgumentExceptionWhenDescriptionCategoryNotFound()
        {
            var productViewModel = new ProductViewModel()
            {
                Name = "product",
                Unit = "kg"
            };

            var descriptionCategoryViewModel = new DescriptionCategoryViewModel()
            {
                Id = 1,
                Name = "Description",
            };

            var returnProductInput = new ReturnedProductViewModel()
            {
                Id = 1,
                Batch = "123",
                Product = productViewModel,
                DescriptionCategory = descriptionCategoryViewModel,

            };

            var product = new Product()
            {
                Name = "Test",
                Unit = "kg"
            };

            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test User"
            };

            var description = new ReturnedProductDescription()
            {
                Id = 1,
                Description = "Something is wrong"
            };

            var descriptionCategory = new DescriptionCategory()
            {
                Id = 1,
                Name = "No reason"
            };

            int protocolId = 1;

            var existingProtocol = new ReturnProtocol
            {
                Id = protocolId,
                PayMethod = "OldPayMethod",
                Object = new CompanyObject
                {
                    Id = 2,
                    Name = "Old name",
                    TraderId = 2,
                    Trader = new Trader { Id = 2, Name = "Old Trader", PhoneNumber = "0987654321" },
                    CompanyId = 2,
                    Company = new Company { Id = 2, Name = "Old Company" }
                },
                ReturnedDate = DateTime.MinValue,
                TraderId = 2,
                IdentityUser = new DelitaUser { Id = user.Id, UserName = user.Name },
                IdentityUserId = user.Id,
            };

            int productId = 1;

            var returnedProduct = new ReturnedProduct()
            {
                Id = productId,
                Product = product,
                DescriptionCategory = descriptionCategory,
                ReturnProtocolId = protocolId,
                ReturnProtocol = existingProtocol,
                Batch = "124"
            };

            existingProtocol.ReturnedProducts = [returnedProduct];

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnedProduct>(returnProductInput.Id))
                .ReturnsAsync(returnedProduct);

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolId))
                .ReturnsAsync(existingProtocol);

            _repositoryMock.Setup(r => r.All<Product>())
                .Returns(new List<Product>() { product }.BuildMock());

            _repositoryMock.Setup(r => r.GetByIdAsync<DescriptionCategory>(descriptionCategory.Id))
                .ReturnsAsync((DescriptionCategory?)null);

            Assert.ThrowsAsync<ArgumentNullException>(() => _returnProductService.UpdateProductAsync(returnProductInput));
        }

        [Test]
        public async Task TestUpdateProductAsyncUpdateProduct()
        {
            int protocolId = 1;
            
            var productViewModel = new ProductViewModel()
            {
                Name = "new product",
                Unit = "kg"
            };

            var descriptionCategoryViewModel = new DescriptionCategoryViewModel()
            {
                Id = 1,
                Name = "New Category",
            };

            var descriptionViewModel = new ReturnedProductDescriptionViewModel()
            {
                Id = 1,
                Description = "It's ok"
            };

            var returnProductInput = new ReturnedProductViewModel()
            {
                Id = 1,
                Batch = "987",
                ReturnProtocolId = protocolId,
                Product = productViewModel,
                Quantity = 2,
                DescriptionCategory = descriptionCategoryViewModel,
                Description = descriptionViewModel,
                BestBefore = new DateTime(2025,1,1)                                
            };

            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test User"
            };

            var product = new Product()
            {
                Name = "existing product",
                Unit = "kg"
            };

            var description = new ReturnedProductDescription()
            {
                Id = 2,
                Description = "Something is wrong"
            };

            var descriptionCategory = new DescriptionCategory()
            {
                Id = 2,
                Name = "No reason"
            };

            var newProduct = new Product()
            {
                Name = productViewModel.Name,
                Unit = productViewModel.Unit
            };

            var newDescription = new ReturnedProductDescription()
            {
                Id = descriptionViewModel.Id,
                Description = descriptionViewModel.Description
            };

            var newDescriptionCategory = new DescriptionCategory()
            {
                Id = descriptionCategoryViewModel.Id,
                Name = descriptionCategoryViewModel.Name
            };


            var existingProtocol = new ReturnProtocol
            {
                Id = protocolId,
                PayMethod = "OldPayMethod",
                Object = new CompanyObject
                {
                    Id = 2,
                    Name = "Old name",
                    TraderId = 2,
                    Trader = new Trader { Id = 2, Name = "Old Trader", PhoneNumber = "0987654321" },
                    CompanyId = 2,
                    Company = new Company { Id = 2, Name = "Old Company" }
                },
                ReturnedDate = DateTime.MinValue,
                TraderId = 2,
                IdentityUser = new DelitaUser { Id = user.Id, UserName = user.Name },
                IdentityUserId = user.Id,
            };

            int productId = 1;

            var returnedProduct = new ReturnedProduct()
            {
                Id = productId,
                Product = product,
                DescriptionCategory = descriptionCategory,
                ReturnProtocolId = protocolId,
                ReturnProtocol = existingProtocol,
                Description = description,
                Batch = "124"
            };

            existingProtocol.ReturnedProducts = [returnedProduct];

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnedProduct>(returnProductInput.Id))
                .ReturnsAsync(returnedProduct);

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolId))
                .ReturnsAsync(existingProtocol);

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { existingProtocol }.BuildMock());

            _repositoryMock.Setup(r => r.All<Product>())
                .Returns(new List<Product>() { newProduct, product }.BuildMock());

            _repositoryMock.Setup(r => r.GetByIdAsync<DescriptionCategory>(returnProductInput.DescriptionCategory.Id))
                .ReturnsAsync(newDescriptionCategory);

            _repositoryMock.Setup(r => r.All<ReturnedProductDescription>())
                .Returns(new List<ReturnedProductDescription>() { newDescription }.BuildMock());

            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1);

            await _returnProductService.UpdateProductAsync(returnProductInput);

            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);

            Assert.That(returnedProduct.Product.Name, Is.EqualTo(returnProductInput.Product.Name));
            Assert.That(returnedProduct.Batch, Is.EqualTo(returnProductInput.Batch));
            Assert.That(returnedProduct.BestBefore, Is.EqualTo(returnProductInput.BestBefore));
            Assert.That(returnedProduct.DescriptionCategory.Name, Is.EqualTo(returnProductInput.DescriptionCategory.Name));
            Assert.That(returnedProduct.Description.Description, Is.EqualTo(returnProductInput.Description.Description));
            Assert.That(returnedProduct.Quantity, Is.EqualTo(returnProductInput.Quantity));
            Assert.That(existingProtocol.LastChanged, Is.Not.EqualTo(DateTime.MinValue));
        }

        [Test]
        public async Task TestUpdateProductAsyncUpdateProductSetDescriptionToNull()
        {
            int protocolId = 1;

            var productViewModel = new ProductViewModel()
            {
                Name = "new product",
                Unit = "kg"
            };

            var descriptionCategoryViewModel = new DescriptionCategoryViewModel()
            {
                Id = 1,
                Name = "New Category",
            };

            var returnProductInput = new ReturnedProductViewModel()
            {
                Id = 1,
                Batch = "987",
                ReturnProtocolId = protocolId,
                Product = productViewModel,
                Quantity = 2,
                DescriptionCategory = descriptionCategoryViewModel,
                BestBefore = new DateTime(2025, 1, 1)
            };

            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test User"
            };

            var product = new Product()
            {
                Name = "existing product",
                Unit = "kg"
            };

            var description = new ReturnedProductDescription()
            {
                Id = 2,
                Description = "Something is wrong"
            };

            var descriptionCategory = new DescriptionCategory()
            {
                Id = 2,
                Name = "No reason"
            };

            var newProduct = new Product()
            {
                Name = productViewModel.Name,
                Unit = productViewModel.Unit
            };

            var newDescriptionCategory = new DescriptionCategory()
            {
                Id = descriptionCategoryViewModel.Id,
                Name = descriptionCategoryViewModel.Name
            };


            var existingProtocol = new ReturnProtocol
            {
                Id = protocolId,
                PayMethod = "OldPayMethod",
                Object = new CompanyObject
                {
                    Id = 2,
                    Name = "Old name",
                    TraderId = 2,
                    Trader = new Trader { Id = 2, Name = "Old Trader", PhoneNumber = "0987654321" },
                    CompanyId = 2,
                    Company = new Company { Id = 2, Name = "Old Company" }
                },
                ReturnedDate = DateTime.MinValue,
                TraderId = 2,
                IdentityUser = new DelitaUser { Id = user.Id, UserName = user.Name },
                IdentityUserId = user.Id,
            };

            int productId = 1;

            var returnedProduct = new ReturnedProduct()
            {
                Id = productId,
                Product = product,
                DescriptionCategory = descriptionCategory,
                ReturnProtocolId = protocolId,
                ReturnProtocol = existingProtocol,
                Description = description,
                Batch = "124"
            };

            existingProtocol.ReturnedProducts = [returnedProduct];

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnedProduct>(returnProductInput.Id))
                .ReturnsAsync(returnedProduct);

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolId))
                .ReturnsAsync(existingProtocol);

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { existingProtocol }.BuildMock());

            _repositoryMock.Setup(r => r.All<Product>())
                .Returns(new List<Product>() { newProduct, product }.BuildMock());

            _repositoryMock.Setup(r => r.GetByIdAsync<DescriptionCategory>(returnProductInput.DescriptionCategory.Id))
                .ReturnsAsync(newDescriptionCategory);

            _repositoryMock.Setup(r => r.All<ReturnedProductDescription>())
                .Returns(new List<ReturnedProductDescription>().BuildMock());

            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1);

            await _returnProductService.UpdateProductAsync(returnProductInput);

            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);

            Assert.That(returnedProduct.Product.Name, Is.EqualTo(returnProductInput.Product.Name));
            Assert.That(returnedProduct.Batch, Is.EqualTo(returnProductInput.Batch));
            Assert.That(returnedProduct.BestBefore, Is.EqualTo(returnProductInput.BestBefore));
            Assert.That(returnedProduct.DescriptionCategory.Name, Is.EqualTo(returnProductInput.DescriptionCategory.Name));
            Assert.That(returnedProduct.Description, Is.Null);
            Assert.That(returnedProduct.Quantity, Is.EqualTo(returnProductInput.Quantity));
            Assert.That(existingProtocol.LastChanged, Is.Not.EqualTo(DateTime.MinValue));
        }

        [Test]
        public async Task TestUpdateProductAsyncUpdateProductSetValueToDescription()
        {
            int protocolId = 1;

            var productViewModel = new ProductViewModel()
            {
                Name = "new product",
                Unit = "kg"
            };

            var descriptionCategoryViewModel = new DescriptionCategoryViewModel()
            {
                Id = 1,
                Name = "New Category",
            };

            var descriptionViewModel = new ReturnedProductDescriptionViewModel()
            {
                Id = 1,
                Description = "It's ok"
            };

            var returnProductInput = new ReturnedProductViewModel()
            {
                Id = 1,
                Batch = "987",
                ReturnProtocolId = protocolId,
                Product = productViewModel,
                Quantity = 2,
                DescriptionCategory = descriptionCategoryViewModel,
                Description = descriptionViewModel,
                BestBefore = new DateTime(2025, 1, 1)
            };

            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test User"
            };

            var product = new Product()
            {
                Name = "existing product",
                Unit = "kg"
            };

            var descriptionCategory = new DescriptionCategory()
            {
                Id = 2,
                Name = "No reason"
            };

            var newProduct = new Product()
            {
                Name = productViewModel.Name,
                Unit = productViewModel.Unit
            };

            var newDescription = new ReturnedProductDescription()
            {
                Id = descriptionViewModel.Id,
                Description = descriptionViewModel.Description
            };

            var newDescriptionCategory = new DescriptionCategory()
            {
                Id = descriptionCategoryViewModel.Id,
                Name = descriptionCategoryViewModel.Name
            };


            var existingProtocol = new ReturnProtocol
            {
                Id = protocolId,
                PayMethod = "OldPayMethod",
                Object = new CompanyObject
                {
                    Id = 2,
                    Name = "Old name",
                    TraderId = 2,
                    Trader = new Trader { Id = 2, Name = "Old Trader", PhoneNumber = "0987654321" },
                    CompanyId = 2,
                    Company = new Company { Id = 2, Name = "Old Company" }
                },
                ReturnedDate = DateTime.MinValue,
                TraderId = 2,
                IdentityUser = new DelitaUser { Id = user.Id, UserName = user.Name },
                IdentityUserId = user.Id,
            };

            int productId = 1;

            var returnedProduct = new ReturnedProduct()
            {
                Id = productId,
                Product = product,
                DescriptionCategory = descriptionCategory,
                ReturnProtocolId = protocolId,
                ReturnProtocol = existingProtocol,
                Batch = "124"
            };

            existingProtocol.ReturnedProducts = [returnedProduct];

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnedProduct>(returnProductInput.Id))
                .ReturnsAsync(returnedProduct);

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolId))
                .ReturnsAsync(existingProtocol);

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { existingProtocol }.BuildMock());

            _repositoryMock.Setup(r => r.All<Product>())
                .Returns(new List<Product>() { newProduct, product }.BuildMock());

            _repositoryMock.Setup(r => r.GetByIdAsync<DescriptionCategory>(returnProductInput.DescriptionCategory.Id))
                .ReturnsAsync(newDescriptionCategory);

            _repositoryMock.Setup(r => r.All<ReturnedProductDescription>())
                .Returns(new List<ReturnedProductDescription>() { newDescription }.BuildMock());

            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1);

            await _returnProductService.UpdateProductAsync(returnProductInput);

            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);

            Assert.That(returnedProduct.Product.Name, Is.EqualTo(returnProductInput.Product.Name));
            Assert.That(returnedProduct.Batch, Is.EqualTo(returnProductInput.Batch));
            Assert.That(returnedProduct.BestBefore, Is.EqualTo(returnProductInput.BestBefore));
            Assert.That(returnedProduct.DescriptionCategory.Name, Is.EqualTo(returnProductInput.DescriptionCategory.Name));
            Assert.That(returnedProduct.Description!.Description, Is.EqualTo(returnProductInput.Description.Description));
            Assert.That(returnedProduct.Quantity, Is.EqualTo(returnProductInput.Quantity));
            Assert.That(existingProtocol.LastChanged, Is.Not.EqualTo(DateTime.MinValue));
        }

        [Test]
        public void TestUpdateProductAsyncThrowInvalidOperationExceptionWhenProtocolIsApproved()
        {
            int protocolId = 1;

            var productViewModel = new ProductViewModel()
            {
                Name = "new product",
                Unit = "kg"
            };

            var descriptionCategoryViewModel = new DescriptionCategoryViewModel()
            {
                Id = 1,
                Name = "New Category",
            };

            var descriptionViewModel = new ReturnedProductDescriptionViewModel()
            {
                Id = 1,
                Description = "It's ok"
            };

            var returnProductInput = new ReturnedProductViewModel()
            {
                Id = 1,
                Batch = "987",
                ReturnProtocolId = protocolId,
                Product = productViewModel,
                Quantity = 2,
                DescriptionCategory = descriptionCategoryViewModel,
                Description = descriptionViewModel,
                BestBefore = new DateTime(2025, 1, 1)
            };

            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test User"
            };

            var product = new Product()
            {
                Name = "existing product",
                Unit = "kg"
            };

            var description = new ReturnedProductDescription()
            {
                Id = 2,
                Description = "Something is wrong"
            };

            var descriptionCategory = new DescriptionCategory()
            {
                Id = 2,
                Name = "No reason"
            };

            var newProduct = new Product()
            {
                Name = productViewModel.Name,
                Unit = productViewModel.Unit
            };

            var newDescription = new ReturnedProductDescription()
            {
                Id = descriptionViewModel.Id,
                Description = descriptionViewModel.Description
            };

            var newDescriptionCategory = new DescriptionCategory()
            {
                Id = descriptionCategoryViewModel.Id,
                Name = descriptionCategoryViewModel.Name
            };


            var existingProtocol = new ReturnProtocol
            {
                Id = protocolId,
                PayMethod = "OldPayMethod",
                Object = new CompanyObject
                {
                    Id = 2,
                    Name = "Old name",
                    TraderId = 2,
                    Trader = new Trader { Id = 2, Name = "Old Trader", PhoneNumber = "0987654321" },
                    CompanyId = 2,
                    Company = new Company { Id = 2, Name = "Old Company" }
                },
                ReturnedDate = DateTime.MinValue,
                TraderId = 2,
                IdentityUser = new DelitaUser { Id = user.Id, UserName = user.Name },
                IdentityUserId = user.Id,
                ApproverId = Guid.NewGuid(),
                Approver = new DelitaUser()
            };

            int productId = 1;

            var returnedProduct = new ReturnedProduct()
            {
                Id = productId,
                Product = product,
                DescriptionCategory = descriptionCategory,
                ReturnProtocolId = protocolId,
                ReturnProtocol = existingProtocol,
                Description = description,
                Batch = "124"
            };

            existingProtocol.ReturnedProducts = [returnedProduct];

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnedProduct>(returnProductInput.Id))
                .ReturnsAsync(returnedProduct);

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolId))
                .ReturnsAsync(existingProtocol);

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { existingProtocol }.BuildMock());

            _repositoryMock.Setup(r => r.All<Product>())
                .Returns(new List<Product>() { newProduct, product }.BuildMock());

            _repositoryMock.Setup(r => r.GetByIdAsync<DescriptionCategory>(returnProductInput.DescriptionCategory.Id))
                .ReturnsAsync(newDescriptionCategory);

            _repositoryMock.Setup(r => r.All<ReturnedProductDescription>())
                .Returns(new List<ReturnedProductDescription>() { newDescription }.BuildMock());

            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1);

            Assert.ThrowsAsync<InvalidOperationException>(() => _returnProductService.UpdateProductAsync(returnProductInput));
        }

        [Test]
        public void TestDeleteProductAsyncThrowArgumentNullExceptionWhenProductToDeleteNotFound()
        {
            int productToDeleteId = 1;
            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnedProduct>(productToDeleteId))
                .ReturnsAsync((ReturnedProduct?)null);

            Assert.ThrowsAsync<ArgumentNullException>(() => _returnProductService.DeleteProductAsync(productToDeleteId));
        }

        [Test]
        public void TestDeleteProductAsyncThrowArgumentNullExceptionWhenProtocolNotFound()
        {
            int productToDeleteId = 1;
            int protocolId = 1;
           
            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test User"
            };

            var product = new Product()
            {
                Name = "existing product",
                Unit = "kg"
            };

            var descriptionCategory = new DescriptionCategory()
            {
                Id = 2,
                Name = "No reason"
            };

            var existingProtocol = new ReturnProtocol
            {
                Id = protocolId,
                PayMethod = "OldPayMethod",
                Object = new CompanyObject
                {
                    Id = 2,
                    Name = "Old name",
                    TraderId = 2,
                    Trader = new Trader { Id = 2, Name = "Old Trader", PhoneNumber = "0987654321" },
                    CompanyId = 2,
                    Company = new Company { Id = 2, Name = "Old Company" }
                },
                ReturnedDate = DateTime.MinValue,
                TraderId = 2,
                IdentityUser = new DelitaUser { Id = user.Id, UserName = user.Name },
                IdentityUserId = user.Id,
                ApproverId = Guid.NewGuid(),
                Approver = new DelitaUser()
            };

            var returnedProduct = new ReturnedProduct()
            {
                Id = productToDeleteId,
                Product = product,
                DescriptionCategory = descriptionCategory,
                ReturnProtocolId = protocolId,
                ReturnProtocol = existingProtocol,
                Batch = "124"
            };

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnedProduct>(productToDeleteId))
                .ReturnsAsync(returnedProduct);

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolId))
                .ReturnsAsync((ReturnProtocol?)null);            

            Assert.ThrowsAsync<ArgumentNullException>(() => _returnProductService.DeleteProductAsync(productToDeleteId));
        }

        [Test]
        public void TestDeleteProductAsyncThrowInvalidOperationExceptionWhenProtocolIsApproved()
        {
            int productToDeleteId = 1;
            int protocolId = 1;

            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test User"
            };

            var product = new Product()
            {
                Name = "existing product",
                Unit = "kg"
            };

            var descriptionCategory = new DescriptionCategory()
            {
                Id = 2,
                Name = "No reason"
            };

            var existingProtocol = new ReturnProtocol
            {
                Id = protocolId,
                PayMethod = "OldPayMethod",
                Object = new CompanyObject
                {
                    Id = 2,
                    Name = "Old name",
                    TraderId = 2,
                    Trader = new Trader { Id = 2, Name = "Old Trader", PhoneNumber = "0987654321" },
                    CompanyId = 2,
                    Company = new Company { Id = 2, Name = "Old Company" }
                },
                ReturnedDate = DateTime.MinValue,
                TraderId = 2,
                IdentityUser = new DelitaUser { Id = user.Id, UserName = user.Name },
                IdentityUserId = user.Id,
                ApproverId = Guid.NewGuid(),
                Approver = new DelitaUser()
            };

            var returnedProduct = new ReturnedProduct()
            {
                Id = productToDeleteId,
                Product = product,
                DescriptionCategory = descriptionCategory,
                ReturnProtocolId = protocolId,
                ReturnProtocol = existingProtocol,
                Batch = "124"
            };

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnedProduct>(productToDeleteId))
                .ReturnsAsync(returnedProduct);

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolId))
                .ReturnsAsync(existingProtocol);

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { existingProtocol }.BuildMock());

            Assert.ThrowsAsync<InvalidOperationException>(() => _returnProductService.DeleteProductAsync(productToDeleteId));
        }

        [Test]
        public async Task TestDeleteProductAsyncSuccess()
        {
            int productToDeleteId = 1;
            int protocolId = 1;

            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test User"
            };

            var product = new Product()
            {
                Name = "existing product",
                Unit = "kg"
            };

            var descriptionCategory = new DescriptionCategory()
            {
                Id = 2,
                Name = "No reason"
            };

            var existingProtocol = new ReturnProtocol
            {
                Id = protocolId,
                PayMethod = "OldPayMethod",
                Object = new CompanyObject
                {
                    Id = 2,
                    Name = "Old name",
                    TraderId = 2,
                    Trader = new Trader { Id = 2, Name = "Old Trader", PhoneNumber = "0987654321" },
                    CompanyId = 2,
                    Company = new Company { Id = 2, Name = "Old Company" }
                },
                ReturnedDate = DateTime.MinValue,
                TraderId = 2,
                IdentityUser = new DelitaUser { Id = user.Id, UserName = user.Name },
                IdentityUserId = user.Id,
            };

            var returnedProduct = new ReturnedProduct()
            {
                Id = productToDeleteId,
                Product = product,
                DescriptionCategory = descriptionCategory,
                ReturnProtocolId = protocolId,
                ReturnProtocol = existingProtocol,
                Batch = "124"
            };

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnedProduct>(productToDeleteId))
                .ReturnsAsync(returnedProduct);

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolId))
                .ReturnsAsync(existingProtocol);

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { existingProtocol }.BuildMock());

            _repositoryMock.Setup(r => r.Remove(It.IsAny<ReturnedProduct>()));

            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1);

            await _returnProductService.DeleteProductAsync(productToDeleteId);

            Assert.That(existingProtocol.LastChanged, Is.Not.EqualTo(DateTime.MinValue));
            _repositoryMock.Verify(r => r.Remove(It.IsAny<ReturnedProduct>()), Times.Once());
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void TestDeleteProductAsyncByUserThrowInvalidOperationExceptionWhenProtocolIsApproved()
        {
            int productToDeleteId = 1;
            int protocolId = 1;

            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test User"
            };

            var product = new Product()
            {
                Name = "existing product",
                Unit = "kg"
            };

            var descriptionCategory = new DescriptionCategory()
            {
                Id = 2,
                Name = "No reason"
            };

            var existingProtocol = new ReturnProtocol
            {
                Id = protocolId,
                PayMethod = "OldPayMethod",
                Object = new CompanyObject
                {
                    Id = 2,
                    Name = "Old name",
                    TraderId = 2,
                    Trader = new Trader { Id = 2, Name = "Old Trader", PhoneNumber = "0987654321" },
                    CompanyId = 2,
                    Company = new Company { Id = 2, Name = "Old Company" }
                },
                ReturnedDate = DateTime.MinValue,
                TraderId = 2,
                IdentityUser = new DelitaUser { Id = user.Id, UserName = user.Name },
                IdentityUserId = user.Id,
                ApproverId = Guid.NewGuid(),
                Approver = new DelitaUser()
            };

            var returnedProduct = new ReturnedProduct()
            {
                Id = productToDeleteId,
                Product = product,
                DescriptionCategory = descriptionCategory,
                ReturnProtocolId = protocolId,
                ReturnProtocol = existingProtocol,
                Batch = "124"
            };

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnedProduct>(productToDeleteId))
                .ReturnsAsync(returnedProduct);

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolId))
                .ReturnsAsync(existingProtocol);

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { existingProtocol }.BuildMock());

            Assert.ThrowsAsync<InvalidOperationException>(() => _returnProductService.DeleteProductAsync(productToDeleteId, user));
        }

        [Test]
        public void TestDeleteProductAsyncByUserThrowUnauthorizedAccessExceptionWhenDeleteNotOwnProduct()
        {
            int productToDeleteId = 1;
            int protocolId = 1;

            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test User"
            };

            var owner = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Owner"
            };

            var product = new Product()
            {
                Name = "existing product",
                Unit = "kg"
            };

            var descriptionCategory = new DescriptionCategory()
            {
                Id = 2,
                Name = "No reason"
            };

            var existingProtocol = new ReturnProtocol
            {
                Id = protocolId,
                PayMethod = "OldPayMethod",
                Object = new CompanyObject
                {
                    Id = 2,
                    Name = "Old name",
                    TraderId = 2,
                    Trader = new Trader { Id = 2, Name = "Old Trader", PhoneNumber = "0987654321" },
                    CompanyId = 2,
                    Company = new Company { Id = 2, Name = "Old Company" }
                },
                ReturnedDate = DateTime.MinValue,
                TraderId = 2,
                IdentityUser = new DelitaUser { Id = owner.Id, UserName = owner.Name },
                IdentityUserId = owner.Id,
            };

            var returnedProduct = new ReturnedProduct()
            {
                Id = productToDeleteId,
                Product = product,
                DescriptionCategory = descriptionCategory,
                ReturnProtocolId = protocolId,
                ReturnProtocol = existingProtocol,
                Batch = "124"
            };

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnedProduct>(productToDeleteId))
                .ReturnsAsync(returnedProduct);

            _repositoryMock.Setup(r => r.AllReadonly<ReturnedProduct>())
                .Returns(new List<ReturnedProduct>() { returnedProduct }.BuildMock());

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolId))
                .ReturnsAsync(existingProtocol);

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { existingProtocol }.BuildMock());

            Assert.ThrowsAsync<UnauthorizedAccessException>(() => _returnProductService.DeleteProductAsync(productToDeleteId, user));
        }

        [Test]
        public async Task TestDeleteProductAsyncByUserSuccess()
        {
            int productToDeleteId = 1;
            int protocolId = 1;

            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test User"
            };

            var product = new Product()
            {
                Name = "existing product",
                Unit = "kg"
            };

            var descriptionCategory = new DescriptionCategory()
            {
                Id = 2,
                Name = "No reason"
            };

            var existingProtocol = new ReturnProtocol
            {
                Id = protocolId,
                PayMethod = "OldPayMethod",
                Object = new CompanyObject
                {
                    Id = 2,
                    Name = "Old name",
                    TraderId = 2,
                    Trader = new Trader { Id = 2, Name = "Old Trader", PhoneNumber = "0987654321" },
                    CompanyId = 2,
                    Company = new Company { Id = 2, Name = "Old Company" }
                },
                ReturnedDate = DateTime.MinValue,
                TraderId = 2,
                IdentityUser = new DelitaUser { Id = user.Id, UserName = user.Name },
                IdentityUserId = user.Id,
            };

            var returnedProduct = new ReturnedProduct()
            {
                Id = productToDeleteId,
                Product = product,
                DescriptionCategory = descriptionCategory,
                ReturnProtocolId = protocolId,
                ReturnProtocol = existingProtocol,
                Batch = "124"
            };

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnedProduct>(productToDeleteId))
                .ReturnsAsync(returnedProduct);
            
            _repositoryMock.Setup(r => r.AllReadonly<ReturnedProduct>())
               .Returns(new List<ReturnedProduct>() { returnedProduct }.BuildMock());

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolId))
                .ReturnsAsync(existingProtocol);

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { existingProtocol }.BuildMock());

            _repositoryMock.Setup(r => r.Remove(It.IsAny<ReturnedProduct>()));

            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1);

            await _returnProductService.DeleteProductAsync(productToDeleteId, user);

            Assert.That(existingProtocol.LastChanged, Is.Not.EqualTo(DateTime.MinValue));
            _repositoryMock.Verify(r => r.Remove(It.IsAny<ReturnedProduct>()), Times.Once());
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
