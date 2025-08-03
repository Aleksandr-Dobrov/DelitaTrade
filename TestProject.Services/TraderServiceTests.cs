using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Services;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using MockQueryable;
using Moq;

namespace TestProject.Services
{
    [TestFixture]
    public class TraderServiceTests
    {
        private Mock<IRepository> _repositoryMock;
        private ITraderService _traderService;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IRepository>();
            _traderService = new TraderService(_repositoryMock.Object);
        }

        [Test]
        public void AlwaysPass()
        {
            Assert.Pass();
        }

        [Test]
        public async Task TestThatGetAllReturnsEmptyCollection()
        {
            List<Trader> traders = new List<Trader>();
            var moqQueryable = traders.BuildMock();

            _repositoryMock.Setup(repo => repo.AllReadonly<Trader>())
                .Returns(moqQueryable);

            var result = await _traderService.GetAllAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.Zero);
        }

        [Test]
        public async Task TestThatGetAllReturnsCollectionWithOneTrader()
        {
            var trader = new Trader
            {
                Id = 1,
                Name = "Test Trader",
                PhoneNumber = "1234567890",
                IsActive = true
            };
            List<Trader> traders = new List<Trader> { trader };
            var moqQueryable = traders.BuildMock();
            _repositoryMock.Setup(repo => repo.AllReadonly<Trader>())
                .Returns(moqQueryable);
            var result = await _traderService.GetAllAsync();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo(trader.Id));
            Assert.That(result.First().Name, Is.EqualTo(trader.Name));
            Assert.That(result.First().PhoneNumber, Is.EqualTo(trader.PhoneNumber));
        }

        [Test]
        public async Task TestThatGetAllNotReturnsSoftDeleteTrader()
        {
            var trader = new Trader
            {
                Id = 1,
                Name = "Test Trader",
                PhoneNumber = "1234567890",
                IsActive = false
            };
            List<Trader> traders = new List<Trader> { trader };
            var moqQueryable = traders.BuildMock();
            _repositoryMock.Setup(repo => repo.AllReadonly<Trader>())
                .Returns(moqQueryable);
            var result = await _traderService.GetAllAsync();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.Zero);
        }

        [Test]
        public async Task TestThatCreateAsyncAddsNewTrader()
        {
            var traderViewModel = new TraderViewModel
            {
                Name = "New Trader",
                PhoneNumber = "0987654321"
            };

            var trader = new Trader
            {
                Id = 1,
                Name = traderViewModel.Name,
                PhoneNumber = traderViewModel.PhoneNumber,
                IsActive = true
            };

            _repositoryMock.Setup(repo => repo.AllReadonly<Trader>())
                .Returns(new List<Trader>().BuildMock());
            _repositoryMock.Setup(repo => repo.AddAsync(trader))
                .Returns(Task.CompletedTask);

            var result = await _traderService.CreateAsync(traderViewModel);
            Assert.That(result, Is.Zero);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Trader>()), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void TestThatCreateAsyncThrowsExceptionWhenTraderExists()
        {
            var traderViewModel = new TraderViewModel
            {
                Name = "Existing Trader",
                PhoneNumber = "1234567890"
            };
            var existingTrader = new Trader
            {
                Id = 1,
                Name = traderViewModel.Name,
                PhoneNumber = traderViewModel.PhoneNumber,
                IsActive = true
            };
            _repositoryMock.Setup(repo => repo.AllReadonly<Trader>())
                .Returns(new List<Trader> { existingTrader }.BuildMock());

            Assert.ThrowsAsync<ArgumentException>(() => _traderService.CreateAsync(traderViewModel));
        }
        
        [Test]
        public async Task TestThatUpdateAsyncUpdatesExistingTrader()
        {
            var traderViewModel = new TraderViewModel
            {
                Id = 1,
                Name = "Updated Trader",
                PhoneNumber = "1234567890"
            };
            var existingTrader = new Trader
            {
                Id = traderViewModel.Id,
                Name = "Old Trader",
                PhoneNumber = "0987654321",
                IsActive = true
            };
            _repositoryMock.Setup(repo => repo.GetByIdAsync<Trader>(traderViewModel.Id))
                .ReturnsAsync(existingTrader);
            _repositoryMock.Setup(repo => repo.SaveChangesAsync())
                .ReturnsAsync(1);

            await _traderService.UpdateAsync(traderViewModel);
            Assert.That(existingTrader.PhoneNumber, Is.EqualTo(traderViewModel.PhoneNumber));
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void TestThatUpdateAsyncThrowsExceptionWhenTraderNotFound()
        {
            var traderViewModel = new TraderViewModel
            {
                Id = 1,
                Name = "Non-existent Trader",
                PhoneNumber = "1234567890"
            };
            _repositoryMock.Setup(repo => repo.GetByIdAsync<Trader>(traderViewModel.Id))
                .ReturnsAsync((Trader?)null);
            Assert.ThrowsAsync<ArgumentNullException>(() => _traderService.UpdateAsync(traderViewModel));
        }

        [Test]
        public async Task TestThatDeleteSoftAsyncMarksTraderAsInactive()
        {
            var traderViewModel = new TraderViewModel
            {
                Id = 1,
                Name = "Trader to Delete",
                PhoneNumber = "1234567890"
            };
            var existingTrader = new Trader
            {
                Id = traderViewModel.Id,
                Name = traderViewModel.Name,
                PhoneNumber = traderViewModel.PhoneNumber,
                IsActive = true
            };
            _repositoryMock.Setup(repo => repo.GetByIdAsync<Trader>(traderViewModel.Id))
                .ReturnsAsync(existingTrader);
            _repositoryMock.Setup(repo => repo.SaveChangesAsync())
                .ReturnsAsync(1);
            await _traderService.DeleteSoftAsync(traderViewModel);
            Assert.That(existingTrader.IsActive, Is.False);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void TestThatDeleteSoftAsyncThrowsExceptionWhenTraderNotFound()
        {
            var traderViewModel = new TraderViewModel
            {
                Id = 1,
                Name = "Non-existent Trader",
                PhoneNumber = "1234567890"
            };
            _repositoryMock.Setup(repo => repo.GetByIdAsync<Trader>(traderViewModel.Id))
                .ReturnsAsync((Trader?)null);
            Assert.ThrowsAsync<ArgumentNullException>(() => _traderService.DeleteSoftAsync(traderViewModel));
        }

        [Test]
        public async Task TestThatGetByIdAsyncReturnsTrader()
        {
            var trader = new Trader
            {
                Id = 1,
                Name = "Test Trader",
                PhoneNumber = "1234567890",
                IsActive = true
            };
            _repositoryMock.Setup(repo => repo.AllReadonly<Trader>())
                .Returns(new List<Trader> { trader }.BuildMock());
            var result = await _traderService.GetByIdAsync(trader.Id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(trader.Id));
            Assert.That(result.Name, Is.EqualTo(trader.Name));
            Assert.That(result.PhoneNumber, Is.EqualTo(trader.PhoneNumber));
        }

        [Test]
        public void TestThatGetByIdAsyncThrowsExceptionWhenTraderNotFound()
        {
            int nonExistentId = 999;
            _repositoryMock.Setup(repo => repo.AllReadonly<Trader>())
                .Returns(new List<Trader>().BuildMock());
            Assert.ThrowsAsync<ArgumentNullException>(() => _traderService.GetByIdAsync(nonExistentId));
        }

        [Test]
        public void TestThatGetByIdAsyncNotReturnsSoftDeleteTrader()
        {
            var trader = new Trader
            {
                Id = 1,
                Name = "Test Trader",
                PhoneNumber = "1234567890",
                IsActive = false
            };
            _repositoryMock.Setup(repo => repo.AllReadonly<Trader>())
                .Returns(new List<Trader> { trader }.BuildMock());

            Assert.ThrowsAsync<ArgumentNullException>(() => _traderService.GetByIdAsync(trader.Id));
        }
    }
}
