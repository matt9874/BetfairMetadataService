using AutoMapper;
using BetfairMetadataService.API.AutomapperProfiles;
using BetfairMetadataService.API.Controllers;
using BetfairMetadataService.API.Models.External;
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
        private Func<int, IMarketTypesService> _marketTypesServiceFactory;
        private Mock<IMarketTypesService> _marketTypesService;
        private ExternalMarketTypesController _controller;
        private Mock<IReader<DataProvider, int>> _mockDataProviderReader;
        private Mock<IReader<EventType, string>> _mockEventTypeReader;
        private Mock<IReader<Competition, string>> _mockCompetitionReader;

        [TestInitialize]
        public void TestInit()
        {
            _mockDataProviderReader = new Mock<IReader<DataProvider, int>>();
            _mockEventTypeReader = new Mock<IReader<EventType, string>>();
            _mockCompetitionReader = new Mock<IReader<Competition, string>>();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new ExternalDtosProfile()));
            _marketTypesService = new Mock<IMarketTypesService>();
            _marketTypesServiceFactory = n => _marketTypesService.Object;
            _controller = new ExternalMarketTypesController(_mockDataProviderReader.Object, _mockEventTypeReader.Object,
                _mockCompetitionReader.Object, _marketTypesServiceFactory);
        }

        private void SetupNonNullParentObjects()
        {
            _mockDataProviderReader.Setup(dpr => dpr.Read(It.IsAny<int>())).Returns(Task.FromResult(new DataProvider()));
            _mockEventTypeReader.Setup(etr => etr.Read(It.IsAny<string>())).Returns(Task.FromResult(new EventType()));
            _mockCompetitionReader.Setup(cr => cr.Read(It.IsAny<string>())).Returns(Task.FromResult(new Competition()));
        }

        [TestMethod]
        public async Task GetMarketTypesByCompetitionId_ServiceThrowsArgumentException_ThrowsArgumentException()
        {
            SetupNonNullParentObjects();
            _marketTypesService.Setup(r => r.GetMarketTypesByCompetitionId(It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException());

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _controller.GetMarketTypesByCompetition(1, "1"));
        }

        [TestMethod]
        public async Task GetMarketTypesByCompetitionId_ServiceReturnsNull_ThrowsException()
        {
            SetupNonNullParentObjects();
            _marketTypesService.Setup(r => r.GetMarketTypesByCompetitionId(It.IsAny<string>()))
                .ReturnsAsync((IEnumerable<MarketType>)null);

            await Assert.ThrowsExceptionAsync<Exception>(async () => await _controller.GetMarketTypesByCompetition(1, "1"));
        }

        [TestMethod]
        public async Task GetMarketTypesByCompetitionId_ServiceReturnsEmpty_ReturnsOk()
        {
            SetupNonNullParentObjects();
            _marketTypesService.Setup(r => r.GetMarketTypesByCompetitionId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[0]);
            var result = await _controller.GetMarketTypesByCompetition(1,"1");
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetMarketTypesByCompetitionId_ServiceReturnsEmpty_EmptyIEnumerable()
        {
            SetupNonNullParentObjects();
            _marketTypesService.Setup(r => r.GetMarketTypesByCompetitionId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[0]);
            var okResult = (OkObjectResult)await _controller.GetMarketTypesByCompetition(1, "1");
            Assert.IsFalse(((IEnumerable<MarketType>)okResult.Value).Any());
        }

        [TestMethod]
        public async Task GetMarketTypesByCompetitionId_ServiceReturnsOne_ReturnsOk()
        {
            SetupNonNullParentObjects();
            _marketTypesService.Setup(r => r.GetMarketTypesByCompetitionId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[] {
                    new MarketType() {Name="WinDrawLose" }
                });
            var result = await _controller.GetMarketTypesByCompetition(1, "1");
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetMarketTypesByCompetitionId_ServiceReturnsOne_IEnumerableOfCountOne()
        {
            SetupNonNullParentObjects();
            _marketTypesService.Setup(r => r.GetMarketTypesByCompetitionId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[] {
                    new MarketType() {Name="WinDrawLose" }
                });
            var okResult = (OkObjectResult)await _controller.GetMarketTypesByCompetition(1, "1");
            Assert.AreEqual(1, ((IEnumerable<MarketType>)okResult.Value).Count());
        }

        [TestMethod]
        public async Task GetMarketTypesByCompetitionId_ServiceReturnsTwo_ReturnsOk()
        {
            SetupNonNullParentObjects();
            _marketTypesService.Setup(r => r.GetMarketTypesByCompetitionId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[] {
                    new MarketType() {Name="WinDrawLose" },
                    new MarketType() {Name="FirstGoalScorer" }
                });
            var result = await _controller.GetMarketTypesByCompetition(1, "1");
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetMarketTypesByCompetitionId_ServiceReturnsOne_IEnumerableOfCountTwo()
        {
            SetupNonNullParentObjects();
            _marketTypesService.Setup(r => r.GetMarketTypesByCompetitionId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[] {
                    new MarketType() {Name="WinDrawLose" },
                    new MarketType() {Name="FirstGoalScorer" }
                });
            var okResult = (OkObjectResult)await _controller.GetMarketTypesByCompetition(1, "1");
            Assert.AreEqual(2, ((IEnumerable<MarketType>)okResult.Value).Count());
        }

        [TestMethod]
        public async Task GetMarketTypesByEventTypeId_ServiceThrowsArgumentException_ThrowsArgumentException()
        {
            SetupNonNullParentObjects();
            _marketTypesService.Setup(r => r.GetMarketTypesByEventTypeId(It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException());

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _controller.GetMarketTypesByEventType(1, "1"));
        }

        [TestMethod]
        public async Task GetMarketTypesByEventTypeId_ServiceReturnsNull_ThrowsException()
        {
            SetupNonNullParentObjects();
            _marketTypesService.Setup(r => r.GetMarketTypesByEventTypeId(It.IsAny<string>()))
                .ReturnsAsync((IEnumerable<MarketType>)null);

            await Assert.ThrowsExceptionAsync<Exception>(async () => await _controller.GetMarketTypesByEventType(1, "1"));
        }

        [TestMethod]
        public async Task GetMarketTypesByEventTypeId_ServiceReturnsEmpty_ReturnsOk()
        {
            SetupNonNullParentObjects();
            _marketTypesService.Setup(r => r.GetMarketTypesByEventTypeId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[0]);
            var result = await _controller.GetMarketTypesByEventType(1, "1");
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetMarketTypesByEventTypeId_ServiceReturnsEmpty_EmptyIEnumerable()
        {
            SetupNonNullParentObjects();
            _marketTypesService.Setup(r => r.GetMarketTypesByEventTypeId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[0]);
            var okResult = (OkObjectResult)await _controller.GetMarketTypesByEventType(1, "1");
            Assert.IsFalse(((IEnumerable<MarketType>)okResult.Value).Any());
        }

        [TestMethod]
        public async Task GetMarketTypesByEventTypeId_ServiceReturnsOne_ReturnsOk()
        {
            SetupNonNullParentObjects();
            _marketTypesService.Setup(r => r.GetMarketTypesByEventTypeId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[] {
                    new MarketType() {Name="WinDrawLose" }
                });
            var result = await _controller.GetMarketTypesByEventType(1, "1");
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetMarketTypesByEventTypeId_ServiceReturnsOne_IEnumerableOfCountOne()
        {
            SetupNonNullParentObjects();
            _marketTypesService.Setup(r => r.GetMarketTypesByEventTypeId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[] {
                    new MarketType() {Name="WinDrawLose" }
                });
            var okResult = (OkObjectResult)await _controller.GetMarketTypesByEventType(1, "1");
            Assert.AreEqual(1, ((IEnumerable<MarketType>)okResult.Value).Count());
        }

        [TestMethod]
        public async Task GetMarketTypesByEventTypeId_ServiceReturnsTwo_ReturnsOk()
        {
            SetupNonNullParentObjects();
            _marketTypesService.Setup(r => r.GetMarketTypesByEventTypeId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[] {
                    new MarketType() {Name="WinDrawLose" },
                    new MarketType() {Name="FirstGoalScorer" }
                });
            var result = await _controller.GetMarketTypesByEventType(1, "1");
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetMarketTypesByEventTypeId_ServiceReturnsOne_IEnumerableOfCountTwo()
        {
            SetupNonNullParentObjects();
            _marketTypesService.Setup(r => r.GetMarketTypesByEventTypeId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[] {
                    new MarketType() {Name="WinDrawLose" },
                    new MarketType() {Name="FirstGoalScorer" }
                });
            var okResult = (OkObjectResult)await _controller.GetMarketTypesByEventType(1, "1");
            Assert.AreEqual(2, ((IEnumerable<MarketType>)okResult.Value).Count());
        }
    }
}
