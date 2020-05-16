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
    public class ExternalCompetitionsControllerTests
    {
        private Func<int, IBatchReader<Competition>> _readerFactory;
        private Mock<IBatchReader<Competition>> _reader;
        private ExternalCompetitionsController _controller;

        [TestInitialize]
        public void TestInit()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new ExternalDtosProfile()));
            _reader = new Mock<IBatchReader<Competition>>();
            _readerFactory = n => _reader.Object;
            _controller = new ExternalCompetitionsController(_readerFactory);
        }

        [TestMethod]
        public async Task GetCompetitions_BatchReaderThrowsArgumentException_ThrowsArgumentException()
        {
            _reader.Setup(r => r.Read(It.IsAny<Func<Competition, bool>>()))
                .ThrowsAsync(new ArgumentException());

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _controller.GetCompetitions(1));
        }

        [TestMethod]
        public async Task GetCompetitions_BatchReaderReturnsNull_ThrowsException()
        {
            _reader.Setup(r => r.Read(It.IsAny<Func<Competition, bool>>()))
                .ReturnsAsync((IEnumerable<Competition>)null);

            await Assert.ThrowsExceptionAsync<Exception>(async () => await _controller.GetCompetitions(1));
        }

        [TestMethod]
        public async Task GetCompetitions_BatchReaderReturnsEmpty_ReturnsOk()
        {
            _reader.Setup(r => r.Read(It.IsAny<Func<Competition, bool>>()))
                .ReturnsAsync(new Competition[0]);
            var result = await _controller.GetCompetitions(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetCompetitions_BatchReaderReturnsEmpty_EmptyIEnumerable()
        {
            _reader.Setup(r => r.Read(It.IsAny<Func<Competition, bool>>()))
                .ReturnsAsync(new Competition[0]);
            var okResult = (OkObjectResult)await _controller.GetCompetitions(1);
            Assert.IsFalse(((IEnumerable<Competition>)okResult.Value).Any());
        }

        [TestMethod]
        public async Task GetCompetitions_BatchReaderReturnsOne_ReturnsOk()
        {
            _reader.Setup(r => r.Read(It.IsAny<Func<Competition, bool>>()))
                .ReturnsAsync(new Competition[] {
                    new Competition() {Id="1",Name="Prem League" }
                });
            var result = await _controller.GetCompetitions(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetCompetitions_BatchReaderReturnsOne_IEnumerableOfCountOne()
        {
            _reader.Setup(r => r.Read(It.IsAny<Func<Competition, bool>>()))
                .ReturnsAsync(new Competition[] {
                    new Competition() {Id="1",Name="Prem League" }
                });
            var okResult = (OkObjectResult)await _controller.GetCompetitions(1);
            Assert.AreEqual(1, ((IEnumerable<Competition>)okResult.Value).Count());
        }

        [TestMethod]
        public async Task GetCompetitions_BatchReaderReturnsTwo_ReturnsOk()
        {
            _reader.Setup(r => r.Read(It.IsAny<Func<Competition, bool>>()))
                .ReturnsAsync(new Competition[] {
                    new Competition() {Id="1",Name="Prem League" },
                    new Competition() {Id="2",Name="Championship" }
                });
            var result = await _controller.GetCompetitions(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetCompetitions_BatchReaderReturnsOne_IEnumerableOfCountTwo()
        {
            _reader.Setup(r => r.Read(It.IsAny<Func<Competition, bool>>()))
                .ReturnsAsync(new Competition[] {
                    new Competition() {Id="1",Name="Football" },
                    new Competition() {Id="2",Name="Horse Racing" }
                });
            var okResult = (OkObjectResult)await _controller.GetCompetitions(1);
            Assert.AreEqual(2, ((IEnumerable<Competition>)okResult.Value).Count());
        }


    }
}
