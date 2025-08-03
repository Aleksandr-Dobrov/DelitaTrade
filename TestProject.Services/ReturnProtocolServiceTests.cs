using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Services;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using MockQueryable;
using Moq;
using System.Threading.Tasks;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace TestProject.Services
{
    [TestFixture]
    public class ReturnProtocolServiceTests
    {
        private Mock<IRepository> _repositoryMock;
        private IReturnProtocolService _returnProtocolService;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IRepository>(MockBehavior.Strict);
            _returnProtocolService = new ReturnProtocolService(_repositoryMock.Object);
        }

        [Test]
        public void AlwaysPass()
        {
            Assert.Pass();
        }

        [Test]
        public void TestGetSimpleFilteredAsyncReturnsEmptyListWhenNoProtocolsAndUserIsAdmin()
        {
            // Arrange
            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { AdminRole }
            };

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>().BuildMock());

            var result = _returnProtocolService.GetSimpleFilteredAsync(user, null, null, null, null);

            Assert.That(result.Result, Is.Empty);
            _repositoryMock.VerifyAll();
        }

        [Test]
        public async Task TestGetSimpleFilteredAsyncReturnsProtocolsWhenUserIsAdmin()
        {
            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { AdminRole }
            };

            var trader = new Trader
            {
                Id = 1,
                Name = "Trader 1",
                PhoneNumber = "1234567890"
            };

            var company = new Company
            {
                Id = 1,
                Name = "Company 1"
            };

            var companyObject = new CompanyObject
            {
                Id = 1,
                Name = "Company Object 1",
                Trader = trader,
                Company = company
            };

            var protocols = new List<ReturnProtocol>
            {
                new ()
                {
                    Id = 1,
                    Trader = trader,
                    Object = companyObject,
                    IdentityUser = new DelitaUser { Id = Guid.NewGuid() }
                },
                new ()
                {
                    Id = 2,
                    Trader = trader,
                    Object = companyObject,
                    IdentityUser = new DelitaUser { Id = Guid.NewGuid() }
                }
            };

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(protocols.BuildMock());

            var result = await _returnProtocolService.GetSimpleFilteredAsync(user, null, null, null, null);

            Assert.That(result.Count(), Is.EqualTo(2));
            _repositoryMock.VerifyAll();
        }

        [Test]
        public async Task TestGetSimpleFilteredAsyncReturnsEmptyListWhenUserNotHaveOwnsAndIsDriver()
        {
            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { DriverRole }
            };

            var trader = new Trader
            {
                Id = 1,
                Name = "Trader 1",
                PhoneNumber = "1234567890"
            };

            var company = new Company
            {
                Id = 1,
                Name = "Company 1"
            };

            var companyObject = new CompanyObject
            {
                Id = 1,
                Name = "Company Object 1",
                Trader = trader,
                Company = company
            };

            var protocols = new List<ReturnProtocol>
            {
                new ()
                {
                    Id = 1,
                    Trader = trader,
                    Object = companyObject,
                    IdentityUser = new DelitaUser { Id = Guid.NewGuid() }
                },
                new ()
                {
                    Id = 2,
                    Trader = trader,
                    Object = companyObject,
                    IdentityUser = new DelitaUser { Id = Guid.NewGuid() }
                }
            };

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(protocols.BuildMock());

            var result = await _returnProtocolService.GetSimpleFilteredAsync(user, null, null, null, null);

            Assert.That(result, Is.Empty);
            _repositoryMock.VerifyAll();
        }

        [Test]
        public async Task TestGetSimpleFilteredAsyncReturnsOwnsProtocolsWhenUserIsDriver()
        {
            // Arrange
            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { AdminRole }
            };

            var trader = new Trader
            {
                Id = 1,
                Name = "Trader 1",
                PhoneNumber = "1234567890"
            };

            var company = new Company
            {
                Id = 1,
                Name = "Company 1"
            };

            var companyObject = new CompanyObject
            {
                Id = 1,
                Name = "Company Object 1",
                Trader = trader,
                Company = company
            };

            var protocols = new List<ReturnProtocol>
            {
                new ()
                {
                    Id = 1,
                    Trader = trader,
                    Object = companyObject,
                    IdentityUser = new DelitaUser { Id = user.Id }
                },
                new ()
                {
                    Id = 2,
                    Trader = trader,
                    Object = companyObject,
                    IdentityUser = new DelitaUser { Id = user.Id }
                }
            };

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(protocols.BuildMock());

            var result = await _returnProtocolService.GetSimpleFilteredAsync(user, null, null, null, null);

            Assert.That(result.Count(), Is.EqualTo(2));
            _repositoryMock.VerifyAll();
        }

        [Test]
        public async Task TestGetSimpleFilteredAsyncReturnsProtocolsFilteredByDateInterval()
        {
            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { AdminRole }
            };

            var trader = new Trader
            {
                Id = 1,
                Name = "Trader 1",
                PhoneNumber = "1234567890"
            };

            var company = new Company
            {
                Id = 1,
                Name = "Company 1"
            };

            var companyObject = new CompanyObject
            {
                Id = 1,
                Name = "Company Object 1",
                Trader = trader,
                Company = company
            };

            var protocols = new List<ReturnProtocol>
            {
                new ()
                {
                    Id = 1,
                    Trader = trader,
                    Object = companyObject,
                    ReturnedDate = new DateTime(2023, 1, 1),
                    IdentityUser = new DelitaUser { Id = Guid.NewGuid() }
                },
                new ()
                {
                    Id = 2,
                    Trader = trader,
                    Object = companyObject,
                    ReturnedDate = new DateTime(2023, 2, 1),
                    IdentityUser = new DelitaUser { Id = Guid.NewGuid() }
                }
            };

            DateTime startDate = new DateTime(2023, 1, 15);
            DateTime endDate = new DateTime(2023, 2, 15);

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(protocols.BuildMock());

            var result = await _returnProtocolService.GetSimpleFilteredAsync(user, null, null, startDate, endDate);

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo(2)); // Only the second protocol should be returned
            _repositoryMock.VerifyAll();
        }

        [Test]
        public async Task TestGetSimpleFilteredAsyncReturnsProtocolsFilteredByObject()
        {
            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { AdminRole }
            };

            var trader = new Trader
            {
                Id = 1,
                Name = "Trader 1",
                PhoneNumber = "1234567890"
            };

            var company = new Company
            {
                Id = 1,
                Name = "Company 1"
            };

            var companyObject = new CompanyObject
            {
                Id = 1,
                Name = "Company Object 1",
                Trader = trader,
                Company = company
            };

            var secondCompanyObject = new CompanyObject
            {
                Id = 2,
                Name = "Find Object",
                Trader = trader,
                Company = company
            };

            var protocols = new List<ReturnProtocol>
            {
                new ()
                {
                    Id = 1,
                    Trader = trader,
                    Object = companyObject,
                    IdentityUser = new DelitaUser { Id = Guid.NewGuid() }
                },
                new ()
                {
                    Id = 2,
                    Trader = trader,
                    Object = secondCompanyObject,
                    IdentityUser = new DelitaUser { Id = Guid.NewGuid() }
                }
            };

            string searchObjectArg = "Find";

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(protocols.BuildMock());

            var result = await _returnProtocolService.GetSimpleFilteredAsync(user, null, searchObjectArg, null, null);

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo(2)); // Only the second protocol should be returned
            _repositoryMock.VerifyAll();
        }

        [Test]
        public async Task TestGetSimpleFilteredAsyncReturnsProtocolsFilteredByTraderName()
        {
            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { AdminRole }
            };

            var trader = new Trader
            {
                Id = 1,
                Name = "Trader 1",
                PhoneNumber = "1234567890"
            };

            var secondTrader = new Trader
            {
                Id = 1,
                Name = "Find Trader",
                PhoneNumber = "1234567890"
            };

            var company = new Company
            {
                Id = 1,
                Name = "Company 1"
            };

            var companyObject = new CompanyObject
            {
                Id = 1,
                Name = "Company Object 1",
                Trader = trader,
                Company = company
            };

            var secondCompanyObject = new CompanyObject
            {
                Id = 2,
                Name = "Find Object",
                Trader = secondTrader,
                Company = company
            };

            var protocols = new List<ReturnProtocol>
            {
                new ()
                {
                    Id = 1,
                    Trader = trader,
                    Object = companyObject,
                    IdentityUser = new DelitaUser { Id = Guid.NewGuid() }
                },
                new ()
                {
                    Id = 2,
                    Trader = secondTrader,
                    Object = secondCompanyObject,
                    IdentityUser = new DelitaUser { Id = Guid.NewGuid() }
                }
            };

            string searchTraderArg = "Find";

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(protocols.BuildMock());

            var result = await _returnProtocolService.GetSimpleFilteredAsync(user, searchTraderArg, null, null, null);

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo(2)); // Only the second protocol should be returned
            _repositoryMock.VerifyAll();
        }

        [Test]
        public async Task TestGetSimpleFilteredAsyncReturnsProtocolsFilteredByTraderNameAndDateInterval()
        {
            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { AdminRole }
            };

            var trader = new Trader
            {
                Id = 1,
                Name = "Trader 1",
                PhoneNumber = "1234567890"
            };

            var secondTrader = new Trader
            {
                Id = 1,
                Name = "Find Trader",
                PhoneNumber = "1234567890"
            };

            var company = new Company
            {
                Id = 1,
                Name = "Company 1"
            };

            var companyObject = new CompanyObject
            {
                Id = 1,
                Name = "Company Object 1",
                Trader = trader,
                Company = company
            };

            var secondCompanyObject = new CompanyObject
            {
                Id = 2,
                Name = "Find Object",
                Trader = secondTrader,
                Company = company
            };

            var protocols = new List<ReturnProtocol>
            {
                new ()
                {
                    Id = 1,
                    Trader = trader,
                    Object = companyObject,
                    ReturnedDate = new DateTime(2023, 2, 1),
                    IdentityUser = new DelitaUser { Id = Guid.NewGuid() }
                },
                new ()
                {
                    Id = 2,
                    Trader = secondTrader,
                    Object = secondCompanyObject,
                    ReturnedDate = new DateTime(2023, 1, 18),
                    IdentityUser = new DelitaUser { Id = Guid.NewGuid() }
                },
                new ()
                {
                    Id = 3,
                    Trader = secondTrader,
                    Object = secondCompanyObject,
                    ReturnedDate = new DateTime(2023, 3, 1),
                    IdentityUser = new DelitaUser { Id = Guid.NewGuid() }
                }
            };

            DateTime startDate = new DateTime(2023, 1, 15);
            DateTime endDate = new DateTime(2023, 2, 15);

            string searchTraderArg = "Find";

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(protocols.BuildMock());

            var result = await _returnProtocolService.GetSimpleFilteredAsync(user, searchTraderArg, null, startDate, endDate);

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo(2)); // Only the second protocol should be returned
            _repositoryMock.VerifyAll();
        }

        [Test]
        public async Task TestGetByIdAsyncReturnsNull()
        {

            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { AdminRole }
            };

            int protocolId = 1;

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>().BuildMock());

            var result = await _returnProtocolService.GetByIdAsync(user, protocolId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TestGetByIdAsyncReturnsExistsNotOwnProtocolWhenUserIsAdmin()
        {

            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { AdminRole }
            };

            var protocolUser = new DelitaUser
            {
                Id = Guid.NewGuid(),
                UserName = "Owner"
            };

            var trader = new Trader
            {
                Id = 1,
                Name = "Trader 1",
                PhoneNumber = "1234567890"
            };

            var company = new Company
            {
                Id = 1,
                Name = "Company 1"
            };

            var companyObject = new CompanyObject
            {
                Id = 1,
                Name = "Company Object 1",
                Trader = trader,
                Company = company
            };

            int protocolId = 1;

            var protocol = new ReturnProtocol
            {
                Id = protocolId,
                ReturnedDate = DateTime.Now,
                PayMethod = "Cash",
                Trader = trader,
                Object = companyObject,
                IdentityUserId = protocolUser.Id,
                IdentityUser = protocolUser
            };


            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { protocol }.BuildMock());

            var result = await _returnProtocolService.GetByIdAsync(user, protocolId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(protocolId));
        }

        [Test]
        public async Task TestGetByIdAsyncReturnsExistsNotOwnProtocolWhenUserIsWarehouseManager()
        {

            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { WarehouseManagerRole }
            };

            var protocolUser = new DelitaUser
            {
                Id = Guid.NewGuid(),
                UserName = "Owner"
            };

            var trader = new Trader
            {
                Id = 1,
                Name = "Trader 1",
                PhoneNumber = "1234567890"
            };

            var company = new Company
            {
                Id = 1,
                Name = "Company 1"
            };

            var companyObject = new CompanyObject
            {
                Id = 1,
                Name = "Company Object 1",
                Trader = trader,
                Company = company
            };

            int protocolId = 1;

            var protocol = new ReturnProtocol
            {
                Id = protocolId,
                ReturnedDate = DateTime.Now,
                PayMethod = "Cash",
                Trader = trader,
                Object = companyObject,
                IdentityUserId = protocolUser.Id,
                IdentityUser = protocolUser
            };


            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { protocol }.BuildMock());

            var result = await _returnProtocolService.GetByIdAsync(user, protocolId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(protocolId));
        }

        [Test]
        public async Task TestGetByIdAsyncReturnsNullWhenUserIsDriverAndProtocolIsNotOwn()
        {

            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { DriverRole }
            };

            var protocolUser = new DelitaUser
            {
                Id = Guid.NewGuid(),
                UserName = "Owner"
            };

            var trader = new Trader
            {
                Id = 1,
                Name = "Trader 1",
                PhoneNumber = "1234567890"
            };

            var company = new Company
            {
                Id = 1,
                Name = "Company 1"
            };

            var companyObject = new CompanyObject
            {
                Id = 1,
                Name = "Company Object 1",
                Trader = trader,
                Company = company
            };

            int protocolId = 1;

            var protocol = new ReturnProtocol
            {
                Id = protocolId,
                ReturnedDate = DateTime.Now,
                PayMethod = "Cash",
                Trader = trader,
                Object = companyObject,
                IdentityUserId = protocolUser.Id,
                IdentityUser = protocolUser
            };


            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { protocol }.BuildMock());

            var result = await _returnProtocolService.GetByIdAsync(user, protocolId);

            Assert.That(result, Is.Null);
        }
        [Test]
        public async Task TestGetByIdAsyncReturnsOwnExistsProtocolWhenUserIsDriver()
        {

            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Owner",
                Roles = new List<string> { DriverRole }
            };

            var protocolUser = new DelitaUser
            {
                Id = user.Id,
                UserName = user.Name
            };

            var trader = new Trader
            {
                Id = 1,
                Name = "Trader 1",
                PhoneNumber = "1234567890"
            };

            var company = new Company
            {
                Id = 1,
                Name = "Company 1"
            };

            var companyObject = new CompanyObject
            {
                Id = 1,
                Name = "Company Object 1",
                Trader = trader,
                Company = company
            };

            int protocolId = 1;

            var protocol = new ReturnProtocol
            {
                Id = protocolId,
                ReturnedDate = DateTime.Now,
                PayMethod = "Cash",
                Trader = trader,
                Object = companyObject,
                IdentityUserId = protocolUser.Id,
                IdentityUser = protocolUser
            };


            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { protocol }.BuildMock());

            var result = await _returnProtocolService.GetByIdAsync(user, protocolId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(protocolId));
        }

        [Test]
        public async Task TestGetEditableByIdAsyncReturnsNull()
        {

            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { AdminRole }
            };

            int protocolId = 1;

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>().BuildMock());

            var result = await _returnProtocolService.GetEditableByIdAsync(user, protocolId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TestGetEditableByIdAsyncReturnsExistsNotOwnProtocolWhenUserIsAdmin()
        {

            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { AdminRole }
            };

            var protocolUser = new DelitaUser
            {
                Id = Guid.NewGuid(),
                UserName = "Owner"
            };

            var trader = new Trader
            {
                Id = 1,
                Name = "Trader 1",
                PhoneNumber = "1234567890"
            };

            var company = new Company
            {
                Id = 1,
                Name = "Company 1"
            };

            var companyObject = new CompanyObject
            {
                Id = 1,
                Name = "Company Object 1",
                Trader = trader,
                Company = company
            };

            int protocolId = 1;

            var protocol = new ReturnProtocol
            {
                Id = protocolId,
                ReturnedDate = DateTime.Now,
                PayMethod = "Cash",
                Trader = trader,
                Object = companyObject,
                IdentityUserId = protocolUser.Id,
                IdentityUser = protocolUser
            };


            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { protocol }.BuildMock());

            var result = await _returnProtocolService.GetEditableByIdAsync(user, protocolId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(protocolId));
        }

        [Test]
        public async Task TestGetEditableByIdAsyncReturnsExistsNotOwnProtocolWhenUserIsWarehouseManager()
        {

            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { WarehouseManagerRole }
            };

            var protocolUser = new DelitaUser
            {
                Id = Guid.NewGuid(),
                UserName = "Owner"
            };

            var trader = new Trader
            {
                Id = 1,
                Name = "Trader 1",
                PhoneNumber = "1234567890"
            };

            var company = new Company
            {
                Id = 1,
                Name = "Company 1"
            };

            var companyObject = new CompanyObject
            {
                Id = 1,
                Name = "Company Object 1",
                Trader = trader,
                Company = company
            };

            int protocolId = 1;

            var protocol = new ReturnProtocol
            {
                Id = protocolId,
                ReturnedDate = DateTime.Now,
                PayMethod = "Cash",
                Trader = trader,
                Object = companyObject,
                IdentityUserId = protocolUser.Id,
                IdentityUser = protocolUser
            };


            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { protocol }.BuildMock());

            var result = await _returnProtocolService.GetEditableByIdAsync(user, protocolId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(protocolId));
        }

        [Test]
        public async Task TestGetEditableByIdAsyncReturnsNullWhenUserIsDriverAndProtocolIsNotOwn()
        {

            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { DriverRole }
            };

            var protocolUser = new DelitaUser
            {
                Id = Guid.NewGuid(),
                UserName = "Owner"
            };

            var trader = new Trader
            {
                Id = 1,
                Name = "Trader 1",
                PhoneNumber = "1234567890"
            };

            var company = new Company
            {
                Id = 1,
                Name = "Company 1"
            };

            var companyObject = new CompanyObject
            {
                Id = 1,
                Name = "Company Object 1",
                Trader = trader,
                Company = company
            };

            int protocolId = 1;

            var protocol = new ReturnProtocol
            {
                Id = protocolId,
                ReturnedDate = DateTime.Now,
                PayMethod = "Cash",
                Trader = trader,
                Object = companyObject,
                IdentityUserId = protocolUser.Id,
                IdentityUser = protocolUser
            };


            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { protocol }.BuildMock());

            var result = await _returnProtocolService.GetEditableByIdAsync(user, protocolId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TestGetEditableByIdAsyncReturnsOwnExistsProtocolWhenUserIsDriver()
        {

            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Owner",
                Roles = new List<string> { DriverRole }
            };

            var protocolUser = new DelitaUser
            {
                Id = user.Id,
                UserName = user.Name
            };

            var trader = new Trader
            {
                Id = 1,
                Name = "Trader 1",
                PhoneNumber = "1234567890"
            };

            var company = new Company
            {
                Id = 1,
                Name = "Company 1"
            };

            var companyObject = new CompanyObject
            {
                Id = 1,
                Name = "Company Object 1",
                Trader = trader,
                Company = company
            };

            int protocolId = 1;

            var protocol = new ReturnProtocol
            {
                Id = protocolId,
                ReturnedDate = DateTime.Now,
                PayMethod = "Cash",
                Trader = trader,
                Object = companyObject,
                IdentityUserId = protocolUser.Id,
                IdentityUser = protocolUser
            };


            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { protocol }.BuildMock());

            var result = await _returnProtocolService.GetEditableByIdAsync(user, protocolId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(protocolId));
        }

        [Test]
        public async Task TestCreateProtocolAsync()
        {
            var trader = new TraderViewModel
            {
                Id = 1,
                Name = "Test Trader",
                PhoneNumber = "1234567890"
            };

            var company = new CompanyViewModel
            {
                Id = 1,
                Name = "Test Company"
            };

            var companyObject = new CompanyObjectViewModel
            {
                Id = 1,
                Name = "Test Object",
                Trader = trader,
                Company = company
            };

            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { AdminRole }
            };

            var protocolViewModel = new ReturnProtocolViewModel
            {
                PayMethod = "Cash",
                CompanyObject = companyObject,
                Trader = trader,
                User = user,
            };
            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1);
            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<ReturnProtocol>()))
                .Returns(Task.CompletedTask);
            _repositoryMock.Setup(r => r.All<DelitaUser>())
                .Returns(new List<DelitaUser> { new DelitaUser { Id = user.Id } }.BuildMock());
            _repositoryMock.Setup(r => r.GetByIdAsync<Trader>(trader.Id))
                .ReturnsAsync(new Trader { Id = trader.Id, Name = trader.Name, PhoneNumber = trader.PhoneNumber });
            _repositoryMock.Setup(r => r.GetByIdAsync<CompanyObject>(companyObject.Id))
                .ReturnsAsync(new CompanyObject { Id = companyObject.Id, Name = companyObject.Name, TraderId = trader.Id, Trader = new Trader { Id = trader.Id, Name = trader.Name, PhoneNumber = trader.PhoneNumber }, CompanyId = company.Id });
            _repositoryMock.Setup(r => r.GetByIdAsync<Company>(company.Id))
                .ReturnsAsync(new Company { Id = company.Id, Name = company.Name });

            var result = await _returnProtocolService.CreateProtocolAsync(protocolViewModel);

            Assert.That(result, Is.Zero);
            _repositoryMock.Verify();
        }

        [Test]
        public void TestCreateProtocolAsyncThrowsInvalidOperationExceptionWhenUserNotFount()
        {
            var trader = new TraderViewModel
            {
                Id = 1,
                Name = "Test Trader",
                PhoneNumber = "1234567890"
            };

            var company = new CompanyViewModel
            {
                Id = 1,
                Name = "Test Company"
            };

            var companyObject = new CompanyObjectViewModel
            {
                Id = 1,
                Name = "Test Object",
                Trader = trader,
                Company = company
            };

            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { AdminRole }
            };

            var protocolViewModel = new ReturnProtocolViewModel
            {
                PayMethod = "Cash",
                CompanyObject = companyObject,
                Trader = trader,
                User = user,
            };

            _repositoryMock.Setup(r => r.All<DelitaUser>())
                .Returns(new List<DelitaUser>().BuildMock());

            Assert.ThrowsAsync<InvalidOperationException>(() => _returnProtocolService.CreateProtocolAsync(protocolViewModel));
        }

        [Test]
        public void TestCreateProtocolAsyncArgumentNullExceptionWhenTraderNotFount()
        {
            var trader = new TraderViewModel
            {
                Id = 1,
                Name = "Test Trader",
                PhoneNumber = "1234567890"
            };

            var company = new CompanyViewModel
            {
                Id = 1,
                Name = "Test Company"
            };

            var companyObject = new CompanyObjectViewModel
            {
                Id = 1,
                Name = "Test Object",
                Trader = trader,
                Company = company
            };

            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { AdminRole }
            };

            var protocolViewModel = new ReturnProtocolViewModel
            {
                PayMethod = "Cash",
                CompanyObject = companyObject,
                Trader = trader,
                User = user,
            };

            _repositoryMock.Setup(r => r.All<DelitaUser>())
                .Returns(new List<DelitaUser> { new DelitaUser { Id = user.Id } }.BuildMock());
            _repositoryMock.Setup(r => r.GetByIdAsync<Trader>(trader.Id))
               .ReturnsAsync((Trader?)null);


            Assert.ThrowsAsync<ArgumentNullException>(() => _returnProtocolService.CreateProtocolAsync(protocolViewModel));
        }

        [Test]
        public void TestCreateProtocolAsyncArgumentNullExceptionWhenCompanyNotFount()
        {
            var trader = new TraderViewModel
            {
                Id = 1,
                Name = "Test Trader",
                PhoneNumber = "1234567890"
            };

            var company = new CompanyViewModel
            {
                Id = 1,
                Name = "Test Company"
            };

            var companyObject = new CompanyObjectViewModel
            {
                Id = 1,
                Name = "Test Object",
                Trader = trader,
                Company = company
            };

            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { AdminRole }
            };

            var protocolViewModel = new ReturnProtocolViewModel
            {
                PayMethod = "Cash",
                CompanyObject = companyObject,
                Trader = trader,
                User = user,
            };

            _repositoryMock.Setup(r => r.All<DelitaUser>())
                .Returns(new List<DelitaUser> { new DelitaUser { Id = user.Id } }.BuildMock());
            _repositoryMock.Setup(r => r.GetByIdAsync<Trader>(trader.Id))
                .ReturnsAsync(new Trader { Id = trader.Id, Name = trader.Name, PhoneNumber = trader.PhoneNumber });
            _repositoryMock.Setup(r => r.GetByIdAsync<Company>(trader.Id))
               .ReturnsAsync((Company?)null);

            Assert.ThrowsAsync<ArgumentNullException>(() => _returnProtocolService.CreateProtocolAsync(protocolViewModel));
        }

        [Test]
        public void TestCreateProtocolAsyncArgumentNullExceptionWhenCompanyObjectNotFount()
        {
            var trader = new TraderViewModel
            {
                Id = 1,
                Name = "Test Trader",
                PhoneNumber = "1234567890"
            };

            var company = new CompanyViewModel
            {
                Id = 1,
                Name = "Test Company"
            };

            var companyObject = new CompanyObjectViewModel
            {
                Id = 1,
                Name = "Test Object",
                Trader = trader,
                Company = company
            };

            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { AdminRole }
            };

            var protocolViewModel = new ReturnProtocolViewModel
            {
                PayMethod = "Cash",
                CompanyObject = companyObject,
                Trader = trader,
                User = user,
            };

            _repositoryMock.Setup(r => r.All<DelitaUser>())
                .Returns(new List<DelitaUser> { new DelitaUser { Id = user.Id } }.BuildMock());
            _repositoryMock.Setup(r => r.GetByIdAsync<Trader>(trader.Id))
                .ReturnsAsync(new Trader { Id = trader.Id, Name = trader.Name, PhoneNumber = trader.PhoneNumber }); 
            _repositoryMock.Setup(r => r.GetByIdAsync<Company>(company.Id))
                .ReturnsAsync(new Company { Id = company.Id, Name = company.Name });
            _repositoryMock.Setup(r => r.GetByIdAsync<CompanyObject>(trader.Id))
               .ReturnsAsync((CompanyObject?)null);

            Assert.ThrowsAsync<ArgumentNullException>(() => _returnProtocolService.CreateProtocolAsync(protocolViewModel));
        }

        [Test]
        public async Task TestUpdateProtocolAsync()
        {
            var trader = new TraderViewModel
            {
                Id = 1,
                Name = "new Trader",
                PhoneNumber = "1234567890"
            };

            var company = new CompanyViewModel
            {
                Id = 1,
                Name = "new Company"
            };

            var companyObject = new CompanyObjectViewModel
            {
                Id = 1,
                Name = "new Object",
                Trader = trader,
                Company = company
            };

            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { DriverRole }
            };

            var returnedDate = new DateTime(2023, 10, 1);

            var protocolViewModel = new ReturnProtocolViewModel
            {
                Id = 1,
                PayMethod = "Cash",
                CompanyObject = companyObject,
                Trader = trader,
                User = user,
                ReturnedDate = returnedDate,
            };

            var existingProtocol = new ReturnProtocol
            {
                Id = protocolViewModel.Id,
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
                LastChanged = returnedDate,
                TraderId = 2,
                IdentityUserId = user.Id,
                IdentityUser = new DelitaUser { Id = user.Id, UserName = user.Name }
            };

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolViewModel.Id))
                .ReturnsAsync(existingProtocol);

            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1);
            _repositoryMock.Setup(r => r.All<DelitaUser>())
                .Returns(new List<DelitaUser> { new DelitaUser { Id = user.Id } }.BuildMock());
            _repositoryMock.Setup(r => r.GetByIdAsync<Trader>(trader.Id))
                .ReturnsAsync(new Trader { Id = trader.Id, Name = trader.Name, PhoneNumber = trader.PhoneNumber });
            _repositoryMock.Setup(r => r.GetByIdAsync<CompanyObject>(companyObject.Id))
                .ReturnsAsync(new CompanyObject { Id = companyObject.Id, Name = companyObject.Name, TraderId = trader.Id, Trader = new Trader { Id = trader.Id, Name = trader.Name, PhoneNumber = trader.PhoneNumber }, CompanyId = company.Id });
            _repositoryMock.Setup(r => r.GetByIdAsync<Company>(company.Id))
                .ReturnsAsync(new Company { Id = company.Id, Name = company.Name });

            await _returnProtocolService.UpdateProtocolAsync(protocolViewModel);

            Assert.That(existingProtocol.PayMethod, Is.EqualTo(protocolViewModel.PayMethod));
            Assert.That(existingProtocol.Object.Id, Is.EqualTo(companyObject.Id));
            Assert.That(existingProtocol.Object.Name, Is.EqualTo(companyObject.Name));
            Assert.That(existingProtocol.Object.TraderId, Is.EqualTo(trader.Id));
            Assert.That(existingProtocol.Object.Trader.Name, Is.EqualTo(trader.Name));
            Assert.That(existingProtocol.Object.CompanyId, Is.EqualTo(company.Id));
            Assert.That(existingProtocol.ReturnedDate, Is.EqualTo(protocolViewModel.ReturnedDate));
            Assert.That(existingProtocol.LastChanged, Is.Not.EqualTo(returnedDate));
        }

        [Test]
        public void TestUpdateProtocolAsyncArgumentNullExceptionWhenProtocolNotFount()
        {
            var trader = new TraderViewModel
            {
                Id = 1,
                Name = "Test Trader",
                PhoneNumber = "1234567890"
            };

            var company = new CompanyViewModel
            {
                Id = 1,
                Name = "Test Company"
            };

            var companyObject = new CompanyObjectViewModel
            {
                Id = 1,
                Name = "Test Object",
                Trader = trader,
                Company = company
            };

            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { AdminRole }
            };

            var protocolViewModel = new ReturnProtocolViewModel
            {
                Id = 1,
                PayMethod = "Cash",
                CompanyObject = companyObject,
                Trader = trader,
                User = user,
            };
                        
            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolViewModel.Id))
                .ReturnsAsync((ReturnProtocol?)null);

            Assert.ThrowsAsync<ArgumentNullException>(() => _returnProtocolService.UpdateProtocolAsync(protocolViewModel));
        }

        [Test]
        public void TestUpdateProtocolAsyncArgumentNullExceptionWhenTraderNotFount()
        {
            var trader = new TraderViewModel
            {
                Id = 1,
                Name = "Test Trader",
                PhoneNumber = "1234567890"
            };

            var company = new CompanyViewModel
            {
                Id = 1,
                Name = "Test Company"
            };

            var companyObject = new CompanyObjectViewModel
            {
                Id = 1,
                Name = "Test Object",
                Trader = trader,
                Company = company
            };

            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { AdminRole }
            };

            var protocolViewModel = new ReturnProtocolViewModel
            {
                PayMethod = "Cash",
                CompanyObject = companyObject,
                Trader = trader,
                User = user,
            };

            var existingProtocol = new ReturnProtocol
            {
                Id = protocolViewModel.Id,
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
                IdentityUserId = user.Id,
                IdentityUser = new DelitaUser { Id = user.Id, UserName = user.Name }
            };

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolViewModel.Id))
                .ReturnsAsync(existingProtocol);
            _repositoryMock.Setup(r => r.GetByIdAsync<Trader>(trader.Id))
                .ReturnsAsync((Trader?)null);

            Assert.ThrowsAsync<ArgumentNullException>(() => _returnProtocolService.UpdateProtocolAsync(protocolViewModel));
        }

        [Test]
        public void TestUpdateProtocolAsyncArgumentNullExceptionWhenCompanyNotFount()
        {
            var trader = new TraderViewModel
            {
                Id = 1,
                Name = "Test Trader",
                PhoneNumber = "1234567890"
            };

            var company = new CompanyViewModel
            {
                Id = 1,
                Name = "Test Company"
            };

            var companyObject = new CompanyObjectViewModel
            {
                Id = 1,
                Name = "Test Object",
                Trader = trader,
                Company = company
            };

            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { AdminRole }
            };

            var protocolViewModel = new ReturnProtocolViewModel
            {
                PayMethod = "Cash",
                CompanyObject = companyObject,
                Trader = trader,
                User = user,
            };

            var existingProtocol = new ReturnProtocol
            {
                Id = protocolViewModel.Id,
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
                IdentityUserId = user.Id,
                IdentityUser = new DelitaUser { Id = user.Id, UserName = user.Name }
            };

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolViewModel.Id))
                .ReturnsAsync(existingProtocol);
            _repositoryMock.Setup(r => r.GetByIdAsync<Trader>(trader.Id))
                .ReturnsAsync(new Trader { Id = trader.Id, Name = trader.Name, PhoneNumber = trader.PhoneNumber });
            _repositoryMock.Setup(r => r.GetByIdAsync<Company>(company.Id))
                .ReturnsAsync((Company?)null);

            Assert.ThrowsAsync<ArgumentNullException>(() => _returnProtocolService.UpdateProtocolAsync(protocolViewModel));
        }

        [Test]
        public void TestUpdateProtocolAsyncArgumentNullExceptionWhenCompanyObjectNotFount()
        {
            var trader = new TraderViewModel
            {
                Id = 1,
                Name = "Test Trader",
                PhoneNumber = "1234567890"
            };

            var company = new CompanyViewModel
            {
                Id = 1,
                Name = "Test Company"
            };

            var companyObject = new CompanyObjectViewModel
            {
                Id = 1,
                Name = "Test Object",
                Trader = trader,
                Company = company
            };

            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { AdminRole }
            };

            var protocolViewModel = new ReturnProtocolViewModel
            {
                PayMethod = "Cash",
                CompanyObject = companyObject,
                Trader = trader,
                User = user,
            };

            var existingProtocol = new ReturnProtocol
            {
                Id = protocolViewModel.Id,
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
                IdentityUserId = user.Id,
                IdentityUser = new DelitaUser { Id = user.Id, UserName = user.Name }
            };

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolViewModel.Id))
                .ReturnsAsync(existingProtocol);
            _repositoryMock.Setup(r => r.GetByIdAsync<Trader>(trader.Id))
                .ReturnsAsync(new Trader { Id = trader.Id, Name = trader.Name, PhoneNumber = trader.PhoneNumber });
            _repositoryMock.Setup(r => r.GetByIdAsync<Company>(company.Id))
                .ReturnsAsync(new Company { Id = company.Id, Name = company.Name });
            _repositoryMock.Setup(r => r.GetByIdAsync<CompanyObject>(trader.Id))
               .ReturnsAsync((CompanyObject?)null);

            Assert.ThrowsAsync<ArgumentNullException>(() => _returnProtocolService.UpdateProtocolAsync(protocolViewModel));
        }

        [Test]
        public void TestUpdateProtocolAsyncInvalidOperationExceptionWhenProtocolIsApproved()
        {
            var trader = new TraderViewModel
            {
                Id = 1,
                Name = "Test Trader",
                PhoneNumber = "1234567890"
            };

            var company = new CompanyViewModel
            {
                Id = 1,
                Name = "Test Company"
            };

            var companyObject = new CompanyObjectViewModel
            {
                Id = 1,
                Name = "Test Object",
                Trader = trader,
                Company = company
            };

            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = new List<string> { AdminRole }
            };

            var protocolViewModel = new ReturnProtocolViewModel
            {
                PayMethod = "Cash",
                CompanyObject = companyObject,
                Trader = trader,
                User = user,
            };

            var existingProtocol = new ReturnProtocol
            {
                Id = protocolViewModel.Id,
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
                IdentityUserId = user.Id,
                IdentityUser = new DelitaUser { Id = user.Id, UserName = user.Name },
                ApproverId = Guid.NewGuid(),
            };

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolViewModel.Id))
                .ReturnsAsync(existingProtocol);
            _repositoryMock.Setup(r => r.GetByIdAsync<Trader>(trader.Id))
                .ReturnsAsync(new Trader { Id = trader.Id, Name = trader.Name, PhoneNumber = trader.PhoneNumber });
            _repositoryMock.Setup(r => r.GetByIdAsync<Company>(company.Id))
                .ReturnsAsync(new Company { Id = company.Id, Name = company.Name });
            _repositoryMock.Setup(r => r.GetByIdAsync<CompanyObject>(companyObject.Id))
                .ReturnsAsync(new CompanyObject { Id = companyObject.Id, Name = companyObject.Name, TraderId = trader.Id, Trader = new Trader { Id = trader.Id, Name = trader.Name, PhoneNumber = trader.PhoneNumber }, CompanyId = company.Id });


            Assert.ThrowsAsync<InvalidOperationException>(() => _returnProtocolService.UpdateProtocolAsync(protocolViewModel));
        }

        [Test]
        public void TestApproveAsyncThrowUnauthorizedAccessExceptionWhenRoleIsNotWarehouseManager()
        {
            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Not warehouse manager",
                Roles = new List<string>() { DriverRole }
            };

            var approveModel = new ReturnProtocolApproveModel()
            {
                Approver = user                
            };

            Assert.ThrowsAsync<UnauthorizedAccessException>(() => _returnProtocolService.ApproveAsync(approveModel, user));
        }

        [Test]
        public void TestApproveAsyncThrowArgumentNullExceptionWhenProtocolNotExists()
        {
            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Not warehouse manager",
                Roles = new List<string>() { WarehouseManagerRole }
            };

            var approveModel = new ReturnProtocolApproveModel()
            {
                Id = 1,
                Approver = user
            };

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(approveModel.Id))
                .ReturnsAsync((ReturnProtocol?)null);

            Assert.ThrowsAsync<ArgumentNullException>(() => _returnProtocolService.ApproveAsync(approveModel, user));
        }

        [Test]
        public void TestApproveAsyncThrowInvalidOperationExceptionWhenProtocolIsChanged()
        {
            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Approver",
                Roles = new List<string>() { WarehouseManagerRole }
            };

            var newChange = new DateTime(2023, 10, 2);

            var approveModel = new ReturnProtocolApproveModel()
            {
                Id = 1,
                Approver = user,
                LastChange = newChange
            };

            var lastChange = new DateTime(2023, 10, 1);

            var existingProtocol = new ReturnProtocol
            {
                Id = approveModel.Id,
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
                IdentityUserId = user.Id,
                IdentityUser = new DelitaUser { Id = user.Id, UserName = user.Name }
            };

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(approveModel.Id))
                .ReturnsAsync(existingProtocol);

            Assert.ThrowsAsync<InvalidOperationException>(() => _returnProtocolService.ApproveAsync(approveModel, user));
        }

        [Test]
        public void TestApproveAsyncThrowUnauthorizedAccessExceptionWhenProtocolIsApproveByAnotherUser()
        {
            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Approver",
                Roles = new List<string>() { WarehouseManagerRole }
            };

            var newChange = new DateTime(2023, 10, 2);

            var approveModel = new ReturnProtocolApproveModel()
            {
                Id = 1,
                Approver = user,
                LastChange = newChange
            };

            var lastChange = new DateTime(2023, 10, 2);

            var existingProtocol = new ReturnProtocol
            {
                Id = approveModel.Id,
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
                IdentityUserId = user.Id,
                IdentityUser = new DelitaUser { Id = Guid.NewGuid(), UserName = "Owner" },
                ApproverId = Guid.NewGuid()
            };

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(approveModel.Id))
                .ReturnsAsync(existingProtocol);

            Assert.ThrowsAsync<UnauthorizedAccessException>(() => _returnProtocolService.ApproveAsync(approveModel, user));
        }

        [Test]
        public async Task TestApproveAsyncThatApprovedProtocol()
        {
            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Approver",
                Roles = new List<string>() { WarehouseManagerRole }
            };

            var newChange = new DateTime(2023, 10, 2);

            var approveModel = new ReturnProtocolApproveModel()
            {
                Id = 1,
                Approver = user,
                LastChange = newChange,
                ReturnedProducts =
                [
                    new ()
                    {
                        Id = 1,
                        IsScraped = true,
                        WarehouseDescription = "Is scraped"
                    }
                ]
            };

            var lastChange = new DateTime(2023, 10, 2);

            var existingProtocol = new ReturnProtocol
            {
                Id = approveModel.Id,
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
                IdentityUserId = user.Id,
                IdentityUser = new DelitaUser { Id = Guid.NewGuid(), UserName = "Owner" },
                ReturnedProducts = 
                [
                    new ()
                    {
                        Id = 1,
                        IsScrapped = false,
                    }
                ]
            };

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(approveModel.Id))
                .ReturnsAsync(existingProtocol);
            _repositoryMock.Setup(r => r.IncludeCollection(existingProtocol, p => p.ReturnedProducts))
                .Returns(Task.CompletedTask);
            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1);

            await _returnProtocolService.ApproveAsync(approveModel, user);

            Assert.That(existingProtocol.ApproverId, Is.EqualTo(user.Id));
            Assert.That(existingProtocol.LastChanged, Is.Not.EqualTo(approveModel.LastChange));
            Assert.That(existingProtocol.ReturnedProducts, Has.Count.EqualTo(approveModel.ReturnedProducts.Count()));
            Assert.That(existingProtocol.ReturnedProducts.First().IsScrapped, Is.EqualTo(approveModel.ReturnedProducts.First().IsScraped));
            Assert.That(existingProtocol.ReturnedProducts.First().WarehouseDescription, Is.EqualTo(approveModel.ReturnedProducts.First().WarehouseDescription));
        }

        [Test]
        public void TestApproveAsyncThrowArgumentNullExceptionWhenProductsIsNotEqual()
        {
            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Approver",
                Roles = new List<string>() { WarehouseManagerRole }
            };

            var newChange = new DateTime(2023, 10, 2);

            var approveModel = new ReturnProtocolApproveModel()
            {
                Id = 1,
                Approver = user,
                LastChange = newChange,
                ReturnedProducts =
                [
                    new ()
                    {
                        Id = 1,
                        IsScraped = true,
                        WarehouseDescription = "Is scraped"
                    }
                ]
            };

            var lastChange = new DateTime(2023, 10, 2);

            var existingProtocol = new ReturnProtocol
            {
                Id = approveModel.Id,
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
                IdentityUserId = user.Id,
                IdentityUser = new DelitaUser { Id = Guid.NewGuid(), UserName = "Owner" },
                ReturnedProducts =
                [
                    new ()
                    {
                        Id = 2,
                        IsScrapped = false,
                    }
                ]
            };

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(approveModel.Id))
                .ReturnsAsync(existingProtocol);
            _repositoryMock.Setup(r => r.IncludeCollection(existingProtocol, p => p.ReturnedProducts))
                .Returns(Task.CompletedTask);

            Assert.ThrowsAsync<ArgumentNullException>(() => _returnProtocolService.ApproveAsync(approveModel, user));
        }

        [Test]
        public async Task TestIsApproveReturnFalse()
        {
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
                IdentityUser = new DelitaUser { Id = Guid.NewGuid(), UserName = "Owner" },
            };

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { existingProtocol }.BuildMock());

            var result = await _returnProtocolService.IsApproved(protocolId);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task TestIsApproveReturnTrue()
        {
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
                IdentityUser = new DelitaUser { Id = Guid.NewGuid(), UserName = "Owner" },
                ApproverId = Guid.NewGuid()
            };

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>() { existingProtocol }.BuildMock());

            var result = await _returnProtocolService.IsApproved(protocolId);

            Assert.That(result, Is.True);
        }
        [Test]
        public async Task TestIsApproveReturnFalseWhenProtocolNotExists()
        {
            int protocolId = 1;
            

            _repositoryMock.Setup(r => r.AllReadonly<ReturnProtocol>())
                .Returns(new List<ReturnProtocol>().BuildMock());

            var result = await _returnProtocolService.IsApproved(protocolId);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task TestDeleteProtocolAsync()
        {
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
                IdentityUser = new DelitaUser { Id = Guid.NewGuid(), UserName = "Owner" }
            };
            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolId))
                .ReturnsAsync(existingProtocol);
            _repositoryMock.Setup(r => r.Remove(It.IsAny<ReturnProtocol>()));
            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1);

            await _returnProtocolService.DeleteProtocolAsync(protocolId);
            _repositoryMock.Verify(r => r.Remove(It.IsAny<ReturnProtocol>()), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void TestDeleteProtocolAsyncThrowArgumentNullExceptionWhenProtocolNotExists()
        {
            int protocolId = 1;

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolId))
                .ReturnsAsync((ReturnProtocol?)null);

            Assert.ThrowsAsync<ArgumentNullException>(() => _returnProtocolService.DeleteProtocolAsync(protocolId));
        }

        [Test]
        public void TestDeleteProtocolAsyncThrowInvalidOperationExceptionWhenProtocolIsApproved()
        {
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
                IdentityUser = new DelitaUser { Id = Guid.NewGuid(), UserName = "Owner" },
                ApproverId = Guid.NewGuid()
            };

            _repositoryMock.Setup(r => r.GetByIdAsync<ReturnProtocol>(protocolId))
                .ReturnsAsync(existingProtocol);

            Assert.ThrowsAsync<InvalidOperationException>(() => _returnProtocolService.DeleteProtocolAsync(protocolId));
        }
    }
}
