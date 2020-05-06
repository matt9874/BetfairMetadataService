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
    public class ExternalEventTypesControllerTests
    {
        private IMapper _mockMapper;
        private Func<int, IBatchReader<EventType>> _readerFactory;
        private Mock<IBatchReader<EventType>> _reader;
        private ExternalEventTypesController _controller;

        [TestInitialize]
        public void TestInit()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new ExternalDtosProfile()));
            _mockMapper = new Mapper(configuration);
            _reader = new Mock<IBatchReader<EventType>>();
            _readerFactory = n => _reader.Object;
            _controller = new ExternalEventTypesController(_readerFactory, _mockMapper);
        }

        [TestMethod]
        public async Task GetEventTypes_BatchReaderThrowsArgumentException_ThrowsArgumentException()
        {
            _reader.Setup(r => r.Read(It.IsAny<Func<EventType, bool>>()))
                .ThrowsAsync(new ArgumentException());

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _controller.GetEventTypes(1));
        }

        [TestMethod]
        public async Task GetEventTypes_BatchReaderReturnsNull_ThrowsException()
        {
            _reader.Setup(r => r.Read(It.IsAny<Func<EventType, bool>>()))
                .ReturnsAsync((IEnumerable<EventType>)null);

            await Assert.ThrowsExceptionAsync<Exception>(async () => await _controller.GetEventTypes(1));
        }

        [TestMethod]
        public async Task GetEventTypes_BatchReaderReturnsEmpty_ReturnsOk()
        {
            _reader.Setup(r => r.Read(It.IsAny<Func<EventType, bool>>()))
                .ReturnsAsync(new EventType[0]);
            var result = await _controller.GetEventTypes(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetEventTypes_BatchReaderReturnsEmpty_EmptyIEnumerable()
        {
            _reader.Setup(r => r.Read(It.IsAny<Func<EventType, bool>>()))
                .ReturnsAsync(new EventType[0]);
            var okResult = (OkObjectResult)await _controller.GetEventTypes(1);
            Assert.IsFalse(((IEnumerable<EventTypeDto>)okResult.Value).Any());
        }

        [TestMethod]
        public async Task GetEventTypes_BatchReaderReturnsOne_ReturnsOk()
        {
            _reader.Setup(r => r.Read(It.IsAny<Func<EventType, bool>>()))
                .ReturnsAsync(new EventType[] {
                    new EventType() {Id="1",Name="Football" }
                });
            var result = await _controller.GetEventTypes(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetEventTypes_BatchReaderReturnsOne_IEnumerableOfCountOne()
        {
            _reader.Setup(r => r.Read(It.IsAny<Func<EventType, bool>>()))
                .ReturnsAsync(new EventType[] {
                    new EventType() {Id="1",Name="Football" }
                });
            var okResult = (OkObjectResult)await _controller.GetEventTypes(1);
            Assert.AreEqual(1, ((IEnumerable<EventTypeDto>)okResult.Value).Count());
        }

        [TestMethod]
        public async Task GetEventTypes_BatchReaderReturnsTwo_ReturnsOk()
        {
            _reader.Setup(r => r.Read(It.IsAny<Func<EventType, bool>>()))
                .ReturnsAsync(new EventType[] {
                    new EventType() {Id="1",Name="Football" },
                    new EventType() {Id="2",Name="Horse Racing" }
                });
            var result = await _controller.GetEventTypes(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetEventTypes_BatchReaderReturnsOne_IEnumerableOfCountTwo()
        {
            _reader.Setup(r => r.Read(It.IsAny<Func<EventType, bool>>()))
                .ReturnsAsync(new EventType[] {
                    new EventType() {Id="1",Name="Football" },
                    new EventType() {Id="2",Name="Horse Racing" }
                });
            var okResult = (OkObjectResult)await _controller.GetEventTypes(1);
            Assert.AreEqual(2, ((IEnumerable<EventTypeDto>)okResult.Value).Count());
        }


    }
}
