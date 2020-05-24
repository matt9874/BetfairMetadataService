using BetfairMetadataService.API.Controllers;
using BetfairMetadataService.DataAccess.Interfaces.Repositories;
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
    public class ExternalCompetitionsControllerTests
    {
        private Mock<IExternalEventTypesRepository> _eventTypesRepository;
        private Mock<IExternalCompetitionsRepository> _competitionsRepository;
        private ExternalCompetitionsController _controller;

        [TestInitialize]
        public void TestInit()
        {
            _eventTypesRepository = new Mock<IExternalEventTypesRepository>();
            Func<int, IExternalEventTypesRepository> eventTypesRepositoryFactory = n => _eventTypesRepository.Object;
            _competitionsRepository = new Mock<IExternalCompetitionsRepository>();
            Func<int, IExternalCompetitionsRepository> competitionsRepositoryFactory = n => _competitionsRepository.Object;
            _controller = new ExternalCompetitionsController(competitionsRepositoryFactory, eventTypesRepositoryFactory);
        }

        [TestMethod]
        public async Task GetCompetitions_RepositoryThrowsArgumentException_ThrowsArgumentException()
        {
            _competitionsRepository.Setup(r => r.GetCompetitions())
                .ThrowsAsync(new ArgumentException());

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _controller.GetCompetitions(1));
        }

        [TestMethod]
        public async Task GetCompetitions_RepositoryReturnsNull_ThrowsException()
        {
            _competitionsRepository.Setup(r => r.GetCompetitions())
                .ReturnsAsync((IEnumerable<Competition>)null);

            await Assert.ThrowsExceptionAsync<Exception>(async () => await _controller.GetCompetitions(1));
        }

        [TestMethod]
        public async Task GetCompetitions_RepositoryReturnsEmpty_ReturnsOk()
        {
            _competitionsRepository.Setup(r => r.GetCompetitions())
                .ReturnsAsync(new Competition[0]);
            var result = await _controller.GetCompetitions(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetCompetitions_RepositoryReturnsEmpty_EmptyIEnumerable()
        {
            _competitionsRepository.Setup(r => r.GetCompetitions())
                .ReturnsAsync(new Competition[0]);
            var okResult = (OkObjectResult)await _controller.GetCompetitions(1);
            Assert.IsFalse(((IEnumerable<Competition>)okResult.Value).Any());
        }

        [TestMethod]
        public async Task GetCompetitions_RepositoryReturnsOne_ReturnsOk()
        {
            _competitionsRepository.Setup(r => r.GetCompetitions())
                .ReturnsAsync(new Competition[] {
                    new Competition() {Id="1",Name="Prem League" }
                });
            var result = await _controller.GetCompetitions(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetCompetitions_RepositoryReturnsOne_IEnumerableOfCountOne()
        {
            _competitionsRepository.Setup(r => r.GetCompetitions())
                .ReturnsAsync(new Competition[] {
                    new Competition() {Id="1",Name="Prem League" }
                });
            var okResult = (OkObjectResult)await _controller.GetCompetitions(1);
            Assert.AreEqual(1, ((IEnumerable<Competition>)okResult.Value).Count());
        }

        [TestMethod]
        public async Task GetCompetitions_RepositoryReturnsTwo_ReturnsOk()
        {
            _competitionsRepository.Setup(r => r.GetCompetitions())
                .ReturnsAsync(new Competition[] {
                    new Competition() {Id="1",Name="Prem League" },
                    new Competition() {Id="2",Name="Championship" }
                });
            var result = await _controller.GetCompetitions(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetCompetitions_RepositoryReturnsTwo_IEnumerableOfCountTwo()
        {
            _competitionsRepository.Setup(r => r.GetCompetitions())
                .ReturnsAsync(new Competition[] {
                    new Competition() {Id="1",Name="Football" },
                    new Competition() {Id="2",Name="Horse Racing" }
                });
            var okResult = (OkObjectResult)await _controller.GetCompetitions(1);
            Assert.AreEqual(2, ((IEnumerable<Competition>)okResult.Value).Count());
        }


    }
}
