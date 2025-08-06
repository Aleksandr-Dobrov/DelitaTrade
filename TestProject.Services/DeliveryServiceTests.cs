using DelitaTrade.Common.Enums;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Services;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using MockQueryable;
using Moq;
using System.Threading.Tasks;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;

namespace TestProject.Services
{
    public class DeliveryServiceTests
    {
        private Mock<IRepository> _repositoryMock;
        private Mock<IInvoiceInDayReportService> _invoiceInDayReportService;
        private IDeliveryService _deliveryService;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IRepository>();
            _invoiceInDayReportService = new Mock<IInvoiceInDayReportService>();
            _deliveryService = new DeliveryService(_repositoryMock.Object, _invoiceInDayReportService.Object);
        }

        [Test]
        public void AlwaysPass()
        {
            Assert.Pass();
        }

        [Test]
        public void TestGetByIdAsyncThrowUnauthorizedAccessExceptionWhenUserNotInRequiredRole()
        {
            int deliveryId = 1;

            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test User"
            };

            Assert.ThrowsAsync<UnauthorizedAccessException>(() => _deliveryService.GetByIdAsync(user, deliveryId));
        }

        [Test]
        public async Task TestGetByIdAsyncReturnNullWhenDeliveryNotExists()
        {
            int deliveryId = 1;

            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = [DriverRole]
            };

            _repositoryMock.Setup(r => r.AllReadonly<Delivery>())
                .Returns(new List<Delivery>().BuildMock());

            var result = await _deliveryService.GetByIdAsync(user, deliveryId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TestGetByIdAsyncReturnNullWhenUserIsDriverAndNotDeliveryOwner()
        {
            int deliveryId = 1;

            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = [DriverRole]
            };

            var employee = new DelitaUser()
            {
                Id = Guid.NewGuid(),
                Name = "Owner"
            };

            var trader = new Trader()
            {
                Id = 1,
                Name = "Test Trader"
            };

            var deliveryAddress = new CompanyObject()
            {
                Id = 1,
                Name = "Havi Logistic",
                Trader = trader,
                TraderId = trader.Id,
                
            };

            var dayReport = new DayReport()
            {
                Id = 1,
                IdentityUser = employee,
                IdentityUserId = employee.Id,
                Date = new DateTime(2025, 1, 1),
                Banknotes = new Dictionary<decimal, int>()
            };

            var existsDelivery = new Delivery()
            {
                Id = deliveryId,
                Employee = employee,
                EmployeeId = employee.Id,
                DeliveryAddress = deliveryAddress,
                DayReport = dayReport
            };

            _repositoryMock.Setup(r => r.AllReadonly<Delivery>())
                .Returns(new List<Delivery>() { existsDelivery }.BuildMock());

            var result = await _deliveryService.GetByIdAsync(user, deliveryId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TestGetByIdAsyncReturnDeliveryWhenUserIsOwner()
        {
            int deliveryId = 1;

            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test User",                
                Roles = [DriverRole]
            };

            var employee = new DelitaUser()
            {
                Id = user.Id,
                Name = user.Name,
            };

            var trader = new Trader()
            {
                Id = 1,
                Name = "Test Trader"
            };

            var deliveryAddress = new CompanyObject()
            {
                Id = 1,
                Name = "Havi Logistic",
                Trader = trader,
                TraderId = trader.Id,

            };

            var dayReport = new DayReport()
            {
                Id = 1,
                IdentityUser = employee,
                IdentityUserId = employee.Id,
                Date = new DateTime(2025, 1, 1),
                Banknotes = new Dictionary<decimal, int>()
            };

            var existsDelivery = new Delivery()
            {
                Id = deliveryId,
                Employee = employee,
                EmployeeId = employee.Id,
                DeliveryAddress = deliveryAddress,
                DayReport = dayReport
            };

            _repositoryMock.Setup(r => r.AllReadonly<Delivery>())
                .Returns(new List<Delivery>() { existsDelivery }.BuildMock());

            var result = await _deliveryService.GetByIdAsync(user, deliveryId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(deliveryId));
            Assert.That(result.CompanyObjectName, Is.EqualTo(deliveryAddress.Name));
            Assert.That(result.EmployeeName, Is.EqualTo(employee.Name));
        }

        [Test]
        public async Task TestGetByIdAsyncReturnDeliveryWhenUserIsInLogisticManagerRole()
        {
            int deliveryId = 1;

            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = [LogisticsManagerRole]
            };

            var employee = new DelitaUser()
            {
                Id = Guid.NewGuid(),
                Name = "Owner",
            };

            var trader = new Trader()
            {
                Id = 1,
                Name = "Test Trader"
            };

            var deliveryAddress = new CompanyObject()
            {
                Id = 1,
                Name = "Havi Logistic",
                Trader = trader,
                TraderId = trader.Id,

            };

            var dayReport = new DayReport()
            {
                Id = 1,
                IdentityUser = employee,
                IdentityUserId = employee.Id,
                Date = new DateTime(2025, 1, 1),
                Banknotes = new Dictionary<decimal, int>()
            };

            var existsDelivery = new Delivery()
            {
                Id = deliveryId,
                Employee = employee,
                EmployeeId = employee.Id,
                DeliveryAddress = deliveryAddress,
                DayReport = dayReport
            };

            _repositoryMock.Setup(r => r.AllReadonly<Delivery>())
                .Returns(new List<Delivery>() { existsDelivery }.BuildMock());

            var result = await _deliveryService.GetByIdAsync(user, deliveryId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(deliveryId));
            Assert.That(result.CompanyObjectName, Is.EqualTo(deliveryAddress.Name));
            Assert.That(result.EmployeeName, Is.EqualTo(employee.Name));
        }

        [Test]
        public async Task TestGetByIdAsyncReturnDeliveryWhenUserIsInAdminRole()
        {
            int deliveryId = 1;

            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Roles = [AdminRole]
            };

            var employee = new DelitaUser()
            {
                Id = Guid.NewGuid(),
                Name = "Owner",
            };

            var trader = new Trader()
            {
                Id = 1,
                Name = "Test Trader"
            };

            var deliveryAddress = new CompanyObject()
            {
                Id = 1,
                Name = "Havi Logistic",
                Trader = trader,
                TraderId = trader.Id,

            };

            var dayReport = new DayReport()
            {
                Id = 1,
                IdentityUser = employee,
                IdentityUserId = employee.Id,
                Date = new DateTime(2025, 1, 1),
                Banknotes = new Dictionary<decimal, int>()
            };

            var existsDelivery = new Delivery()
            {
                Id = deliveryId,
                Employee = employee,
                EmployeeId = employee.Id,
                DeliveryAddress = deliveryAddress,
                DayReport = dayReport
            };

            _repositoryMock.Setup(r => r.AllReadonly<Delivery>())
                .Returns(new List<Delivery>() { existsDelivery }.BuildMock());

            var result = await _deliveryService.GetByIdAsync(user, deliveryId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(deliveryId));
            Assert.That(result.CompanyObjectName, Is.EqualTo(deliveryAddress.Name));
            Assert.That(result.EmployeeName, Is.EqualTo(employee.Name));
        }

        [Test]
        public async Task TestGetVehicleIdFromDeliveryAsyncReturnNull()
        {
            int deliveryId = 1;

            var employee = new DelitaUser()
            {
                Id = Guid.NewGuid(),
                Name = "Owner",
            };

            var trader = new Trader()
            {
                Id = 1,
                Name = "Test Trader"
            };

            var deliveryAddress = new CompanyObject()
            {
                Id = 1,
                Name = "Havi Logistic",
                Trader = trader,
                TraderId = trader.Id,

            };

            var dayReport = new DayReport()
            {
                Id = 1,
                IdentityUser = employee,
                IdentityUserId = employee.Id,
                Date = new DateTime(2025, 1, 1),
                Banknotes = new Dictionary<decimal, int>()
            };

            int vehicleId = 1;

            var existsDelivery = new Delivery()
            {
                Id = deliveryId,
                Employee = employee,
                EmployeeId = employee.Id,
                VehicleId = vehicleId,
                DeliveryAddress = deliveryAddress,
                DayReport = dayReport
            };

            int notExistsDeliveryId = 2;

            _repositoryMock.Setup(r => r.AllReadonly<Delivery>())
                .Returns(new List<Delivery>() { existsDelivery }.BuildMock());

            var result = await _deliveryService.GetVehicleIdFromDeliveryAsync(notExistsDeliveryId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TestGetVehicleIdFromDeliverySuccess()
        {
            int deliveryId = 1;

            var employee = new DelitaUser()
            {
                Id = Guid.NewGuid(),
                Name = "Owner",
            };

            var trader = new Trader()
            {
                Id = 1,
                Name = "Test Trader"
            };

            var deliveryAddress = new CompanyObject()
            {
                Id = 1,
                Name = "Havi Logistic",
                Trader = trader,
                TraderId = trader.Id,

            };

            var dayReport = new DayReport()
            {
                Id = 1,
                IdentityUser = employee,
                IdentityUserId = employee.Id,
                Date = new DateTime(2025, 1, 1),
                Banknotes = new Dictionary<decimal, int>()
            };

            int vehicleId = 1;

            var existsDelivery = new Delivery()
            {
                Id = deliveryId,
                Employee = employee,
                EmployeeId = employee.Id,
                VehicleId = vehicleId,
                DeliveryAddress = deliveryAddress,
                DayReport = dayReport
            };

            _repositoryMock.Setup(r => r.AllReadonly<Delivery>())
                .Returns(new List<Delivery>() { existsDelivery }.BuildMock());

            var result = await _deliveryService.GetVehicleIdFromDeliveryAsync(deliveryId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(vehicleId));
        }

        [Test]
        public async Task TestGetDayReportIdAsyncReturnNull()
        {
            int deliveryId = 1;

            var employee = new DelitaUser()
            {
                Id = Guid.NewGuid(),
                Name = "Owner",
            };

            var trader = new Trader()
            {
                Id = 1,
                Name = "Test Trader"
            };

            var deliveryAddress = new CompanyObject()
            {
                Id = 1,
                Name = "Havi Logistic",
                Trader = trader,
                TraderId = trader.Id,

            };

            var dayReport = new DayReport()
            {
                Id = 1,
                IdentityUser = employee,
                IdentityUserId = employee.Id,
                Date = new DateTime(2025, 1, 1),
                Banknotes = new Dictionary<decimal, int>()
            };

            var existsDelivery = new Delivery()
            {
                Id = deliveryId,
                Employee = employee,
                EmployeeId = employee.Id,
                DeliveryAddress = deliveryAddress,
                DayReport = dayReport,
                DayReportId = deliveryId
            };

            int notExistsDeliveryId = 2;

            _repositoryMock.Setup(r => r.AllReadonly<Delivery>())
                .Returns(new List<Delivery>() { existsDelivery }.BuildMock());

            var result = await _deliveryService.GetDayReportIdAsync(notExistsDeliveryId);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public async Task TestGetDayReportIdSuccess()
        {
            int deliveryId = 1;

            var employee = new DelitaUser()
            {
                Id = Guid.NewGuid(),
                Name = "Owner",
            };

            var trader = new Trader()
            {
                Id = 1,
                Name = "Test Trader"
            };

            var deliveryAddress = new CompanyObject()
            {
                Id = 1,
                Name = "Havi Logistic",
                Trader = trader,
                TraderId = trader.Id,

            };

            var dayReport = new DayReport()
            {
                Id = 1,
                IdentityUser = employee,
                IdentityUserId = employee.Id,
                Date = new DateTime(2025, 1, 1),
                Banknotes = new Dictionary<decimal, int>()
            };

            var existsDelivery = new Delivery()
            {
                Id = deliveryId,
                Employee = employee,
                EmployeeId = employee.Id,
                DeliveryAddress = deliveryAddress,
                DayReport = dayReport,
                DayReportId = dayReport.Id
            };

            _repositoryMock.Setup(r => r.AllReadonly<Delivery>())
                .Returns(new List<Delivery>() { existsDelivery }.BuildMock());

            var result = await _deliveryService.GetDayReportIdAsync(deliveryId);

            Assert.That(result, Is.EqualTo(dayReport.Id));
        }

        [Test]
        public void TestCompleteAllThrowUnauthorizedAccessExceptionWhenUserIsNotInDriverRole()
        {
            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "user",
            };

            Assert.ThrowsAsync<UnauthorizedAccessException>(() => _deliveryService.CompleteAllAsync(user, 1));
        }

        [Test]
        public void TestCompleteAllThrowArgumentNullExceptionWhenDeliveryNotFound()
        {
            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "user",
                Roles = [DriverRole]
            };

            _repositoryMock.Setup(r => r.AllReadonly<Delivery>())
                .Returns(new List<Delivery>().BuildMock());

            Assert.ThrowsAsync<ArgumentNullException>(() => _deliveryService.CompleteAllAsync(user, 1));
        }

        [Test]
        public void TestCompleteAllThrowArgumentNullExceptionWhenUserIsNotOwner()
        {
            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "user",
                Roles = [DriverRole]
            };

            int deliveryId = 1;

            var employee = new DelitaUser()
            {
                Id = Guid.NewGuid(),
                Name = "Owner",
            };

            var trader = new Trader()
            {
                Id = 1,
                Name = "Test Trader"
            };

            var deliveryAddress = new CompanyObject()
            {
                Id = 1,
                Name = "Havi Logistic",
                Trader = trader,
                TraderId = trader.Id,

            };

            var dayReport = new DayReport()
            {
                Id = 1,
                IdentityUser = employee,
                IdentityUserId = employee.Id,
                Date = new DateTime(2025, 1, 1),
                Banknotes = new Dictionary<decimal, int>()
            };

            var existsDelivery = new Delivery()
            {
                Id = deliveryId,
                Employee = employee,
                EmployeeId = employee.Id,
                DeliveryAddress = deliveryAddress,
                DayReport = dayReport,
                DayReportId = dayReport.Id
            };

            _repositoryMock.Setup(r => r.AllReadonly<Delivery>())
                .Returns(new List<Delivery>() { existsDelivery }.BuildMock());

            Assert.ThrowsAsync<ArgumentNullException>(() => _deliveryService.CompleteAllAsync(user, deliveryId));
        }

        [Test]
        public async Task TestCompleteAllNoPaymentsSuccess()
        {
            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "user",
                Roles = [DriverRole]
            };

            int deliveryId = 1;

            var employee = new DelitaUser()
            {
                Id = user.Id,
                Name = user.Name,
            };

            var trader = new Trader()
            {
                Id = 1,
                Name = "Test Trader"
            };

            var deliveryAddress = new CompanyObject()
            {
                Id = 1,
                Name = "Havi Logistic",
                Trader = trader,
                TraderId = trader.Id,

            };

            var dayReport = new DayReport()
            {
                Id = 1,
                IdentityUser = employee,
                IdentityUserId = employee.Id,
                Date = new DateTime(2025, 1, 1),
                Banknotes = new Dictionary<decimal, int>()
            };

            var existsDelivery = new Delivery()
            {
                Id = deliveryId,
                Employee = employee,
                EmployeeId = employee.Id,
                DeliveryAddress = deliveryAddress,
                DayReport = dayReport,
                DayReportId = dayReport.Id
            };

            _repositoryMock.Setup(r => r.AllReadonly<Delivery>())
                .Returns(new List<Delivery>() { existsDelivery }.BuildMock());

            _invoiceInDayReportService.Setup(i => i.UpdateAsync(It.IsAny<InvoiceViewModel>()))
                .Returns(Task.CompletedTask);
            
            await _deliveryService.CompleteAllAsync(user, deliveryId);

            _invoiceInDayReportService.Verify(i => i.UpdateAsync(It.IsAny<InvoiceViewModel>()), Times.Never());
        }

        [Test]
        public async Task TestCompleteAllSuccess()
        {
            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "user",
                Roles = [DriverRole]
            };

            int deliveryId = 1;

            var employee = new DelitaUser()
            {
                Id = user.Id,
                Name = user.Name,
            };

            var trader = new Trader()
            {
                Id = 1,
                Name = "Test Trader"
            };

            var company = new Company()
            {
                Id = 1,
                Name = "Test Company"
            };

            var deliveryAddress = new CompanyObject()
            {
                Id = 1,
                Name = "Havi Logistic",
                Trader = trader,
                TraderId = trader.Id,
                CompanyId = company.Id,
                Company = company,
            };

            var dayReport = new DayReport()
            {
                Id = 1,
                IdentityUser = employee,
                IdentityUserId = employee.Id,
                Date = new DateTime(2025, 1, 1),
                Banknotes = new Dictionary<decimal, int>()
            };


            var invoice = new Invoice()
            {
                Id = 1,
                CompanyObject = deliveryAddress,
                CompanyObjectId = deliveryAddress.Id,
                Company = company,
                CompanyId = company.Id,
                Number = "1000000111"
            };

            var payment = new InvoiceInDayReport()
            {
                DayReport = dayReport,
                DayReportId = dayReport.Id,
                Invoice = invoice,
                PayMethod = PayMethod.Bank,
            };

            var existsDelivery = new Delivery()
            {
                Id = deliveryId,
                Employee = employee,
                EmployeeId = employee.Id,
                DeliveryAddress = deliveryAddress,
                DayReport = dayReport,
                DayReportId = dayReport.Id,
                Payments = [payment]
            };

            _repositoryMock.Setup(r => r.AllReadonly<Delivery>())
                .Returns(new List<Delivery>() { existsDelivery }.BuildMock());

            _invoiceInDayReportService.Setup(i => i.UpdateAsync(It.IsAny<InvoiceViewModel>()))
                .Returns(Task.CompletedTask);

            await _deliveryService.CompleteAllAsync(user, deliveryId);

            _invoiceInDayReportService.Verify(i => i.UpdateAsync(It.IsAny<InvoiceViewModel>()), Times.Once());
        }

        [Test]
        public async Task TestCompleteAllNotCorrectPayMethodSuccess()
        {
            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "user",
                Roles = [DriverRole]
            };

            int deliveryId = 1;

            var employee = new DelitaUser()
            {
                Id = user.Id,
                Name = user.Name,
            };

            var trader = new Trader()
            {
                Id = 1,
                Name = "Test Trader"
            };

            var company = new Company()
            {
                Id = 1,
                Name = "Test Company"
            };

            var deliveryAddress = new CompanyObject()
            {
                Id = 1,
                Name = "Havi Logistic",
                Trader = trader,
                TraderId = trader.Id,
                CompanyId = company.Id,
                Company = company,
            };

            var dayReport = new DayReport()
            {
                Id = 1,
                IdentityUser = employee,
                IdentityUserId = employee.Id,
                Date = new DateTime(2025, 1, 1),
                Banknotes = new Dictionary<decimal, int>()
            };


            var invoice = new Invoice()
            {
                Id = 1,
                CompanyObject = deliveryAddress,
                CompanyObjectId = deliveryAddress.Id,
                Company = company,
                CompanyId = company.Id,
                Number = "1000000111"
            };

            var payment = new InvoiceInDayReport()
            {
                DayReport = dayReport,
                DayReportId = dayReport.Id,
                Invoice = invoice,
                PayMethod = PayMethod.Expense,
            };

            var existsDelivery = new Delivery()
            {
                Id = deliveryId,
                Employee = employee,
                EmployeeId = employee.Id,
                DeliveryAddress = deliveryAddress,
                DayReport = dayReport,
                DayReportId = dayReport.Id,
                Payments = [payment]
            };

            _repositoryMock.Setup(r => r.AllReadonly<Delivery>())
                .Returns(new List<Delivery>() { existsDelivery }.BuildMock());

            _invoiceInDayReportService.Setup(i => i.UpdateAsync(It.IsAny<InvoiceViewModel>()))
                .Returns(Task.CompletedTask);

            await _deliveryService.CompleteAllAsync(user, deliveryId);

            _invoiceInDayReportService.Verify(i => i.UpdateAsync(It.IsAny<InvoiceViewModel>()), Times.Never());
        }

        [Test]
        public async Task TestIsCompletedAsyncReturnTrue()
        {
            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "user",
                Roles = [DriverRole]
            };

            int deliveryId = 1;

            var employee = new DelitaUser()
            {
                Id = user.Id,
                Name = user.Name,
            };

            var trader = new Trader()
            {
                Id = 1,
                Name = "Test Trader"
            };

            var company = new Company()
            {
                Id = 1,
                Name = "Test Company"
            };

            var deliveryAddress = new CompanyObject()
            {
                Id = 1,
                Name = "Havi Logistic",
                Trader = trader,
                TraderId = trader.Id,
                CompanyId = company.Id,
                Company = company,
            };

            var dayReport = new DayReport()
            {
                Id = 1,
                IdentityUser = employee,
                IdentityUserId = employee.Id,
                Date = new DateTime(2025, 1, 1),
                Banknotes = new Dictionary<decimal, int>()
            };


            var invoice = new Invoice()
            {
                Id = 1,
                CompanyObject = deliveryAddress,
                CompanyObjectId = deliveryAddress.Id,
                Company = company,
                CompanyId = company.Id,
                Number = "1000000111"
            };

            var payment = new InvoiceInDayReport()
            {
                DayReport = dayReport,
                DayReportId = dayReport.Id,
                Invoice = invoice,
                PayMethod = PayMethod.Bank,
                IsCompleted = true,
            };

            var secondInvoice = new Invoice()
            {
                Id = 1,
                CompanyObject = deliveryAddress,
                CompanyObjectId = deliveryAddress.Id,
                Company = company,
                CompanyId = company.Id,
                Number = "1000000112"
            };

            var secondPayment = new InvoiceInDayReport()
            {
                DayReport = dayReport,
                DayReportId = dayReport.Id,
                Invoice = secondInvoice,
                PayMethod = PayMethod.Bank,
                IsCompleted = true,
            };

            var existsDelivery = new Delivery()
            {
                Id = deliveryId,
                Employee = employee,
                EmployeeId = employee.Id,
                DeliveryAddress = deliveryAddress,
                DayReport = dayReport,
                DayReportId = dayReport.Id,
                Payments = [payment, secondPayment]
            };

            _repositoryMock.Setup(r => r.AllReadonly<Delivery>())
                .Returns(new List<Delivery>() { existsDelivery }.BuildMock());

            var result = await _deliveryService.IsCompleteAsync(user, deliveryId);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task TestIsCompletedAsyncReturnFalseWhenOnePaymentIsNotCompleted()
        {
            var user = new UserViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "user",
                Roles = [DriverRole]
            };

            int deliveryId = 1;

            var employee = new DelitaUser()
            {
                Id = user.Id,
                Name = user.Name,
            };

            var trader = new Trader()
            {
                Id = 1,
                Name = "Test Trader"
            };

            var company = new Company()
            {
                Id = 1,
                Name = "Test Company"
            };

            var deliveryAddress = new CompanyObject()
            {
                Id = 1,
                Name = "Havi Logistic",
                Trader = trader,
                TraderId = trader.Id,
                CompanyId = company.Id,
                Company = company,
            };

            var dayReport = new DayReport()
            {
                Id = 1,
                IdentityUser = employee,
                IdentityUserId = employee.Id,
                Date = new DateTime(2025, 1, 1),
                Banknotes = new Dictionary<decimal, int>()
            };


            var invoice = new Invoice()
            {
                Id = 1,
                CompanyObject = deliveryAddress,
                CompanyObjectId = deliveryAddress.Id,
                Company = company,
                CompanyId = company.Id,
                Number = "1000000111"
            };

            var payment = new InvoiceInDayReport()
            {
                DayReport = dayReport,
                DayReportId = dayReport.Id,
                Invoice = invoice,
                PayMethod = PayMethod.Bank,
                IsCompleted = true,
            };

            var secondInvoice = new Invoice()
            {
                Id = 1,
                CompanyObject = deliveryAddress,
                CompanyObjectId = deliveryAddress.Id,
                Company = company,
                CompanyId = company.Id,
                Number = "1000000112"
            };

            var secondPayment = new InvoiceInDayReport()
            {
                DayReport = dayReport,
                DayReportId = dayReport.Id,
                Invoice = secondInvoice,
                PayMethod = PayMethod.Bank,
                IsCompleted = false,
            };

            var existsDelivery = new Delivery()
            {
                Id = deliveryId,
                Employee = employee,
                EmployeeId = employee.Id,
                DeliveryAddress = deliveryAddress,
                DayReport = dayReport,
                DayReportId = dayReport.Id,
                Payments = [payment, secondPayment]
            };

            _repositoryMock.Setup(r => r.AllReadonly<Delivery>())
                .Returns(new List<Delivery>() { existsDelivery }.BuildMock());

            var result = await _deliveryService.IsCompleteAsync(user, deliveryId);

            Assert.That(result, Is.False);
        }
    }
}
