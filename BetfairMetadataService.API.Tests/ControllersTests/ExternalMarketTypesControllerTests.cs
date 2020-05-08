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
        private IMapper _mockMapper;
        private Func<int, IMarketTypesService> _marketTypesServiceFactory;
        private Mock<IMarketTypesService> _marketTypesService;
        private ExternalMarketTypesController _controller;

        [TestInitialize]
        public void TestInit()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new ExternalDtosProfile()));
            _mockMapper = new Mapper(configuration);
            _marketTypesService = new Mock<IMarketTypesService>();
            _marketTypesServiceFactory = n => _marketTypesService.Object;
            _controller = new ExternalMarketTypesController(_marketTypesServiceFactory, _mockMapper);
        }

        [TestMethod]
        public async Task GetMarketTypesByCompetitionId_ServiceThrowsArgumentException_ThrowsArgumentException()
        {
            _marketTypesService.Setup(r => r.GetMarketTypesByCompetitionId(It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException());

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _controller.GetMarketTypesByCompetition(1, "1"));
        }

        [TestMethod]
        public async Task GetMarketTypesByCompetitionId_ServiceReturnsNull_ThrowsException()
        {
            _marketTypesService.Setup(r => r.GetMarketTypesByCompetitionId(It.IsAny<string>()))
                .ReturnsAsync((IEnumerable<MarketType>)null);

            await Assert.ThrowsExceptionAsync<Exception>(async () => await _controller.GetMarketTypesByCompetition(1, "1"));
        }

        [TestMethod]
        public async Task GetMarketTypesByCompetitionId_ServiceReturnsEmpty_ReturnsOk()
        {
            _marketTypesService.Setup(r => r.GetMarketTypesByCompetitionId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[0]);
            var result = await _controller.GetMarketTypesByCompetition(1,"1");
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetMarketTypesByCompetitionId_ServiceReturnsEmpty_EmptyIEnumerable()
        {
            _marketTypesService.Setup(r => r.GetMarketTypesByCompetitionId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[0]);
            var okResult = (OkObjectResult)await _controller.GetMarketTypesByCompetition(1, "1");
            Assert.IsFalse(((IEnumerable<MarketTypeDto>)okResult.Value).Any());
        }

        [TestMethod]
        public async Task GetMarketTypesByCompetitionId_ServiceReturnsOne_ReturnsOk()
        {
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
            _marketTypesService.Setup(r => r.GetMarketTypesByCompetitionId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[] {
                    new MarketType() {Name="WinDrawLose" }
                });
            var okResult = (OkObjectResult)await _controller.GetMarketTypesByCompetition(1, "1");
            Assert.AreEqual(1, ((IEnumerable<MarketTypeDto>)okResult.Value).Count());
        }

        [TestMethod]
        public async Task GetMarketTypesByCompetitionId_ServiceReturnsTwo_ReturnsOk()
        {
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
            _marketTypesService.Setup(r => r.GetMarketTypesByCompetitionId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[] {
                    new MarketType() {Name="WinDrawLose" },
                    new MarketType() {Name="FirstGoalScorer" }
                });
            var okResult = (OkObjectResult)await _controller.GetMarketTypesByCompetition(1, "1");
            Assert.AreEqual(2, ((IEnumerable<MarketTypeDto>)okResult.Value).Count());
        }

        [TestMethod]
        public async Task GetMarketTypesByEventTypeId_ServiceThrowsArgumentException_ThrowsArgumentException()
        {
            _marketTypesService.Setup(r => r.GetMarketTypesByEventTypeId(It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException());

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _controller.GetMarketTypesByEventType(1, "1"));
        }

        [TestMethod]
        public async Task GetMarketTypesByEventTypeId_ServiceReturnsNull_ThrowsException()
        {
            _marketTypesService.Setup(r => r.GetMarketTypesByEventTypeId(It.IsAny<string>()))
                .ReturnsAsync((IEnumerable<MarketType>)null);

            await Assert.ThrowsExceptionAsync<Exception>(async () => await _controller.GetMarketTypesByEventType(1, "1"));
        }

        [TestMethod]
        public async Task GetMarketTypesByEventTypeId_ServiceReturnsEmpty_ReturnsOk()
        {
            _marketTypesService.Setup(r => r.GetMarketTypesByEventTypeId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[0]);
            var result = await _controller.GetMarketTypesByEventType(1, "1");
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetMarketTypesByEventTypeId_ServiceReturnsEmpty_EmptyIEnumerable()
        {
            _marketTypesService.Setup(r => r.GetMarketTypesByEventTypeId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[0]);
            var okResult = (OkObjectResult)await _controller.GetMarketTypesByEventType(1, "1");
            Assert.IsFalse(((IEnumerable<MarketTypeDto>)okResult.Value).Any());
        }

        [TestMethod]
        public async Task GetMarketTypesByEventTypeId_ServiceReturnsOne_ReturnsOk()
        {
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
            _marketTypesService.Setup(r => r.GetMarketTypesByEventTypeId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[] {
                    new MarketType() {Name="WinDrawLose" }
                });
            var okResult = (OkObjectResult)await _controller.GetMarketTypesByEventType(1, "1");
            Assert.AreEqual(1, ((IEnumerable<MarketTypeDto>)okResult.Value).Count());
        }

        [TestMethod]
        public async Task GetMarketTypesByEventTypeId_ServiceReturnsTwo_ReturnsOk()
        {
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
            _marketTypesService.Setup(r => r.GetMarketTypesByEventTypeId(It.IsAny<string>()))
                .ReturnsAsync(new MarketType[] {
                    new MarketType() {Name="WinDrawLose" },
                    new MarketType() {Name="FirstGoalScorer" }
                });
            var okResult = (OkObjectResult)await _controller.GetMarketTypesByEventType(1, "1");
            Assert.AreEqual(2, ((IEnumerable<MarketTypeDto>)okResult.Value).Count());
        }
    }
}
