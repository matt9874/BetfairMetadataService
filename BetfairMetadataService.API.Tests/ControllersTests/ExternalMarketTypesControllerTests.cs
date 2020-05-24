using BetfairMetadataService.API.Controllers;
using BetfairMetadataService.DataAccess.Interfaces.Repositories;
using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain.External;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetfairMetadataService.API.Tests.ControllersTests
{
    [TestClass]
    public class ExternalMarketTypesControllerTests
    {
        private Mock<IExternalEventTypesRepository> _mockEventTypesRepository;
        private Mock<IExternalCompetitionsRepository> _mockCompetitionsRepository;
        private Mock<IExternalMarketTypesRepository> _mockMarketTypesRepository;
        private ExternalMarketTypesController _controller;
        private Mock<IReader<DataProvider, int>> _mockDataProviderReader;

        [TestInitialize]
        public void TestInit()
        {
            _mockDataProviderReader = new Mock<IReader<DataProvider, int>>();
            _mockMarketTypesRepository = new Mock<IExternalMarketTypesRepository>();
            Func<int, IExternalMarketTypesRepository> _marketTypesRepositoryFactory = n => _mockMarketTypesRepository.Object;
            _mockEventTypesRepository = new Mock<IExternalEventTypesRepository>();
            Func<int, IExternalEventTypesRepository> eventTypesRepositoryFactory = n => _mockEventTypesRepository.Object;
            _mockCompetitionsRepository = new Mock<IExternalCompetitionsRepository>();
            Func<int, IExternalCompetitionsRepository> competitionsRepositoryFactory = n => _mockCompetitionsRepository.Object;
            _controller = new ExternalMarketTypesController(_mockDataProviderReader.Object, eventTypesRepositoryFactory,
                competitionsRepositoryFactory, _marketTypesRepositoryFactory);
        }

        private void SetupNonNullParentObjects()
        {
            _mockDataProviderReader.Setup(dpr => dpr.Read(It.IsAny<int>())).Returns(Task.FromResult(new DataProvider()));
            _mockEventTypesRepository.Setup(dpr => dpr.GetEventType(It.IsAny<string>())).Returns(Task.FromResult(new EventType()));
            _mockCompetitionsRepository.Setup(dpr => dpr.GetCompetition(It.IsAny<string>())).Returns(Task.FromResult(new Competition()));
        }

        [TestMethod]
        public async Task GetMarketTypesByCompetitionId_RepositoryThrowsArgumentException_ThrowsArgumentException()
        {
            SetupNonNullParentObjects();
            _mockMarketTypesRepository.Setup(r => r.GetMarketTypesByCompetitionId(It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException());

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _controller.GetMarketTypesByCompetition(1, "1", "1"));
        }

        [TestMethod]
        public async Task GetMarketTypesByCompetitionId_RepositoryReturnsNull_ThrowsException()
        {
            SetupNonNullParentObjects();
            _mockMarketTypesRepository.Setup(r => r.GetMarketTypesByCompetitionId(It.IsAny<string>()))
                .ReturnsAsync((IEnumerable<MarketType>)null);

            await Assert.ThrowsExceptionAsync<Exception>(async () => await _controller.GetMarketTypesByCompetition(1, "1", "1"));
        }

        [TestMethod]
        public async Task GetMarketTypesByCompetitionId_RepositoryReturnsEmpty_ReturnsOk()
        {
            SetupNonNullParentObjects();
            _mockMarketTypesRepository.Setup(r => r.GetMarketTypesByCompetitionId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[0]);
            var result = await _controller.GetMarketTypesByCompetition(1, "1", "1");
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetMarketTypesByCompetitionId_RepositoryReturnsEmpty_EmptyIEnumerable()
        {
            SetupNonNullParentObjects();
            _mockMarketTypesRepository.Setup(r => r.GetMarketTypesByCompetitionId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[0]);
            var okResult = (OkObjectResult)await _controller.GetMarketTypesByCompetition(1, "1", "1");
            Assert.IsFalse(((IEnumerable<MarketType>)okResult.Value).Any());
        }

        [TestMethod]
        public async Task GetMarketTypesByCompetitionId_RepositoryReturnsOne_ReturnsOk()
        {
            SetupNonNullParentObjects();
            _mockMarketTypesRepository.Setup(r => r.GetMarketTypesByCompetitionId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[] {
                    new MarketType() {Name="WinDrawLose" }
                });
            var result = await _controller.GetMarketTypesByCompetition(1, "1", "1");
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetMarketTypesByCompetitionId_RepositoryReturnsOne_IEnumerableOfCountOne()
        {
            SetupNonNullParentObjects();
            _mockMarketTypesRepository.Setup(r => r.GetMarketTypesByCompetitionId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[] {
                    new MarketType() {Name="WinDrawLose" }
                });
            var okResult = (OkObjectResult)await _controller.GetMarketTypesByCompetition(1, "1", "1");
            Assert.AreEqual(1, ((IEnumerable<MarketType>)okResult.Value).Count());
        }

        [TestMethod]
        public async Task GetMarketTypesByCompetitionId_RepositoryReturnsTwo_ReturnsOk()
        {
            SetupNonNullParentObjects();
            _mockMarketTypesRepository.Setup(r => r.GetMarketTypesByCompetitionId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[] {
                    new MarketType() {Name="WinDrawLose" },
                    new MarketType() {Name="FirstGoalScorer" }
                });
            var result = await _controller.GetMarketTypesByCompetition(1, "1", "1");
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetMarketTypesByCompetitionId_RepositoryReturnsOne_IEnumerableOfCountTwo()
        {
            SetupNonNullParentObjects();
            _mockMarketTypesRepository.Setup(r => r.GetMarketTypesByCompetitionId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[] {
                    new MarketType() {Name="WinDrawLose" },
                    new MarketType() {Name="FirstGoalScorer" }
                });
            var okResult = (OkObjectResult)await _controller.GetMarketTypesByCompetition(1, "1", "1");
            Assert.AreEqual(2, ((IEnumerable<MarketType>)okResult.Value).Count());
        }

        [TestMethod]
        public async Task GetMarketTypesByEventTypeId_RepositoryThrowsArgumentException_ThrowsArgumentException()
        {
            SetupNonNullParentObjects();
            _mockMarketTypesRepository.Setup(r => r.GetMarketTypesByEventTypeId(It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException());

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _controller.GetMarketTypesByEventType(1, "1"));
        }

        [TestMethod]
        public async Task GetMarketTypesByEventTypeId_RepositoryReturnsNull_ThrowsException()
        {
            SetupNonNullParentObjects();
            _mockMarketTypesRepository.Setup(r => r.GetMarketTypesByEventTypeId(It.IsAny<string>()))
                .ReturnsAsync((IEnumerable<MarketType>)null);

            await Assert.ThrowsExceptionAsync<Exception>(async () => await _controller.GetMarketTypesByEventType(1, "1"));
        }

        [TestMethod]
        public async Task GetMarketTypesByEventTypeId_RepositoryReturnsEmpty_ReturnsOk()
        {
            SetupNonNullParentObjects();
            _mockMarketTypesRepository.Setup(r => r.GetMarketTypesByEventTypeId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[0]);
            var result = await _controller.GetMarketTypesByEventType(1, "1");
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetMarketTypesByEventTypeId_RepositoryReturnsEmpty_EmptyIEnumerable()
        {
            SetupNonNullParentObjects();
            _mockMarketTypesRepository.Setup(r => r.GetMarketTypesByEventTypeId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[0]);
            var okResult = (OkObjectResult)await _controller.GetMarketTypesByEventType(1, "1");
            Assert.IsFalse(((IEnumerable<MarketType>)okResult.Value).Any());
        }

        [TestMethod]
        public async Task GetMarketTypesByEventTypeId_RepositoryReturnsOne_ReturnsOk()
        {
            SetupNonNullParentObjects();
            _mockMarketTypesRepository.Setup(r => r.GetMarketTypesByEventTypeId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[] {
                    new MarketType() {Name="WinDrawLose" }
                });
            var result = await _controller.GetMarketTypesByEventType(1, "1");
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetMarketTypesByEventTypeId_RepositoryReturnsOne_IEnumerableOfCountOne()
        {
            SetupNonNullParentObjects();
            _mockMarketTypesRepository.Setup(r => r.GetMarketTypesByEventTypeId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[] {
                    new MarketType() {Name="WinDrawLose" }
                });
            var okResult = (OkObjectResult)await _controller.GetMarketTypesByEventType(1, "1");
            Assert.AreEqual(1, ((IEnumerable<MarketType>)okResult.Value).Count());
        }

        [TestMethod]
        public async Task GetMarketTypesByEventTypeId_RepositoryReturnsTwo_ReturnsOk()
        {
            SetupNonNullParentObjects();
            _mockMarketTypesRepository.Setup(r => r.GetMarketTypesByEventTypeId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[] {
                    new MarketType() {Name="WinDrawLose" },
                    new MarketType() {Name="FirstGoalScorer" }
                });
            var result = await _controller.GetMarketTypesByEventType(1, "1");
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetMarketTypesByEventTypeId_RepositoryReturnsOne_IEnumerableOfCountTwo()
        {
            SetupNonNullParentObjects();
            _mockMarketTypesRepository.Setup(r => r.GetMarketTypesByEventTypeId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[] {
                    new MarketType() {Name="WinDrawLose" },
                    new MarketType() {Name="FirstGoalScorer" }
                });
            var okResult = (OkObjectResult)await _controller.GetMarketTypesByEventType(1, "1");
            Assert.AreEqual(2, ((IEnumerable<MarketType>)okResult.Value).Count());
        }
    }
}
