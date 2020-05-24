using BetfairMetadataService.API.Controllers;
using BetfairMetadataService.DataAccess.Interfaces.Repositories;
using System;
using BetfairMetadataService.Domain.External;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetfairMetadataService.API.Tests.ControllersTests
{
    [TestClass]
    public class ExternalEventTypesControllerTests
    {
        private Mock<IExternalEventTypesRepository> _eventTypesRepository;
        private ExternalEventTypesController _controller;

        [TestInitialize]
        public void TestInit()
        {
            _eventTypesRepository = new Mock<IExternalEventTypesRepository>();
            Func<int, IExternalEventTypesRepository> repositoryFactory = n => _eventTypesRepository.Object;
            _controller = new ExternalEventTypesController(repositoryFactory);
        }

        [TestMethod]
        public async Task GetEventTypes_RepositoryThrowsArgumentException_ThrowsArgumentException()
        {
            _eventTypesRepository.Setup(r => r.GetEventTypes())
                .ThrowsAsync(new ArgumentException());

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _controller.GetEventTypes(1));
        }

        [TestMethod]
        public async Task GetEventTypes_RepositoryReturnsNull_ThrowsException()
        {
            _eventTypesRepository.Setup(r => r.GetEventTypes())
                .ReturnsAsync((IEnumerable<EventType>)null);

            await Assert.ThrowsExceptionAsync<Exception>(async () => await _controller.GetEventTypes(1));
        }

        [TestMethod]
        public async Task GetEventTypes_RepositoryReturnsEmpty_ReturnsOk()
        {
            _eventTypesRepository.Setup(r => r.GetEventTypes())
                .ReturnsAsync(new EventType[0]);
            var result = await _controller.GetEventTypes(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetEventTypes_RepositoryReturnsEmpty_EmptyIEnumerable()
        {
            _eventTypesRepository.Setup(r => r.GetEventTypes())
                .ReturnsAsync(new EventType[0]);
            var okResult = (OkObjectResult)await _controller.GetEventTypes(1);
            Assert.IsFalse(((IEnumerable<EventType>)okResult.Value).Any());
        }

        [TestMethod]
        public async Task GetEventTypes_RepositoryReturnsOne_ReturnsOk()
        {
            _eventTypesRepository.Setup(r => r.GetEventTypes())
                .ReturnsAsync(new EventType[] {
                    new EventType() {Id="1",Name="Football" }
                });
            var result = await _controller.GetEventTypes(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetEventTypes_RepositoryReturnsOne_IEnumerableOfCountOne()
        {
            _eventTypesRepository.Setup(r => r.GetEventTypes())
                .ReturnsAsync(new EventType[] {
                    new EventType() {Id="1",Name="Football" }
                });
            var okResult = (OkObjectResult)await _controller.GetEventTypes(1);
            Assert.AreEqual(1, ((IEnumerable<EventType>)okResult.Value).Count());
        }

        [TestMethod]
        public async Task GetEventTypes_RepositoryReturnsTwo_ReturnsOk()
        {
            _eventTypesRepository.Setup(r => r.GetEventTypes())
                .ReturnsAsync(new EventType[] {
                    new EventType() {Id="1",Name="Football" },
                    new EventType() {Id="2",Name="Horse Racing" }
                });
            var result = await _controller.GetEventTypes(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetEventTypes_RepositoryReturnsOne_IEnumerableOfCountTwo()
        {
            _eventTypesRepository.Setup(r => r.GetEventTypes())
                .ReturnsAsync(new EventType[] {
                    new EventType() {Id="1",Name="Football" },
                    new EventType() {Id="2",Name="Horse Racing" }
                });
            var okResult = (OkObjectResult)await _controller.GetEventTypes(1);
            Assert.AreEqual(2, ((IEnumerable<EventType>)okResult.Value).Count());
        }
    }
}
