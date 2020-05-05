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
    public class ExternalDataProvidersControllerTests
    {
        private IMapper _mockMapper;
        private Mock<IReader<DataProvider, int>> _mockDataProviderReader;
        private Mock<IBatchReader<DataProvider>> _mockBatchDataProviderReader;
        private ExternalDataProvidersController _controller;

        [TestInitialize]
        public void TestInit()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new ExternalDtosProfile()));
            _mockMapper = new Mapper(configuration);
            _mockDataProviderReader = new Mock<IReader<DataProvider, int>>();
            _mockBatchDataProviderReader = new Mock<IBatchReader<DataProvider>>();
            _controller = new ExternalDataProvidersController(_mockMapper, _mockDataProviderReader.Object, _mockBatchDataProviderReader.Object);
        }

        [TestMethod]
        public async Task GetDataProviders_BatchReaderThrowsArgumentException_ThrowsArgumentException()
        {
            _mockBatchDataProviderReader.Setup(r => r.Read(It.IsAny<Func<DataProvider, bool>>()))
                .ThrowsAsync(new ArgumentException());

            await Assert.ThrowsExceptionAsync<ArgumentException>(async ()=> await _controller.GetDataProviders());
        }

        [TestMethod]
        public async Task GetDataProviders_BatchReaderReturnsNull_ThrowsException()
        {
            _mockBatchDataProviderReader.Setup(r => r.Read(It.IsAny<Func<DataProvider, bool>>()))
                .ReturnsAsync((IEnumerable<DataProvider>)null);

            await Assert.ThrowsExceptionAsync<Exception>(async () => await _controller.GetDataProviders());
        }

        [TestMethod]
        public async Task GetDataProviders_BatchReaderReturnsEmpty_ReturnsOk()
        {
            _mockBatchDataProviderReader.Setup(r => r.Read(It.IsAny<Func<DataProvider, bool>>()))
                .ReturnsAsync(new DataProvider[0]);
            var result = await _controller.GetDataProviders();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetDataProviders_BatchReaderReturnsEmpty_EmptyIEnumerable()
        {
            _mockBatchDataProviderReader.Setup(r => r.Read(It.IsAny<Func<DataProvider, bool>>()))
                .ReturnsAsync(new DataProvider[0]);
            var okResult = (OkObjectResult)await _controller.GetDataProviders();
            Assert.IsFalse(((IEnumerable<DataProviderDto>)okResult.Value).Any());
        }

        [TestMethod]
        public async Task GetDataProviders_BatchReaderReturnsOne_ReturnsOk()
        {
            _mockBatchDataProviderReader.Setup(r => r.Read(It.IsAny<Func<DataProvider, bool>>()))
                .ReturnsAsync(new DataProvider[] { 
                    new DataProvider() {Id=1,Name="Betfair" } 
                });
            var result = await _controller.GetDataProviders();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetDataProviders_BatchReaderReturnsOne_IEnumerableOfCountOne()
        {
            _mockBatchDataProviderReader.Setup(r => r.Read(It.IsAny<Func<DataProvider, bool>>()))
                .ReturnsAsync(new DataProvider[] {
                    new DataProvider() {Id=1,Name="Betfair" }
                });
            var okResult = (OkObjectResult)await _controller.GetDataProviders();
            Assert.AreEqual(1, ((IEnumerable<DataProviderDto>)okResult.Value).Count());
        }

        [TestMethod]
        public async Task GetDataProviders_BatchReaderReturnsTwo_ReturnsOk()
        {
            _mockBatchDataProviderReader.Setup(r => r.Read(It.IsAny<Func<DataProvider, bool>>()))
                .ReturnsAsync(new DataProvider[] {
                    new DataProvider() {Id=1,Name="Betfair" },
                    new DataProvider() {Id=2,Name="Betdaq" }
                });
            var result = await _controller.GetDataProviders();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetDataProviders_BatchReaderReturnsOne_IEnumerableOfCountTwo()
        {
            _mockBatchDataProviderReader.Setup(r => r.Read(It.IsAny<Func<DataProvider, bool>>()))
                .ReturnsAsync(new DataProvider[] {
                    new DataProvider() {Id=1,Name="Betfair" },
                    new DataProvider() {Id=2,Name="Betdaq" }
                });
            var okResult = (OkObjectResult)await _controller.GetDataProviders();
            Assert.AreEqual(2, ((IEnumerable<DataProviderDto>)okResult.Value).Count());
        }

        [TestMethod]
        public async Task GetDataProvider_ReaderThrowsArgumentException_ThrowsArgumentException()
        {
            _mockDataProviderReader.Setup(r => r.Read(It.IsAny<int>()))
                .ThrowsAsync(new ArgumentException());

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _controller.GetDataProvider(1));
        }

        [TestMethod]
        public async Task GetDataProvider_ReaderReturnsNull_ReturnsNotFound()
        {
            _mockDataProviderReader.Setup(r => r.Read(It.IsAny<int>()))
                .ReturnsAsync((DataProvider)null);

            var result = await _controller.GetDataProvider(1);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task GetDataProvider_ReaderReturnsNull_NotFoundValueIsInt()
        {
            _mockDataProviderReader.Setup(r => r.Read(It.IsAny<int>()))
                .ReturnsAsync((DataProvider)null);

            var notFoundResult = (NotFoundObjectResult)await _controller.GetDataProvider(1);
            Assert.IsInstanceOfType(notFoundResult.Value, typeof(int));
        }

        [TestMethod]
        public async Task GetDataProvider_ReaderReturnsNull_NotFoundValueIsCorrect()
        {
            _mockDataProviderReader.Setup(r => r.Read(It.IsAny<int>()))
                .ReturnsAsync((DataProvider)null);

            var notFoundResult = (NotFoundObjectResult)await _controller.GetDataProvider(1);
            Assert.AreEqual(1, notFoundResult.Value);
        }

        [TestMethod]
        public async Task GetDataProviders_ReaderReturnsADataProvider_ReturnsOk()
        {
            _mockDataProviderReader.Setup(r => r.Read(It.IsAny<int>()))
                .ReturnsAsync(new DataProvider() {Id=1,Name="Betfair" });
            var result = await _controller.GetDataProvider(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetDataProviders_ReaderReturnsADataProvider_HasCorrectId()
        {
            _mockDataProviderReader.Setup(r => r.Read(It.IsAny<int>()))
                .ReturnsAsync(new DataProvider() { Id = 1, Name = "Betfair" });
            OkObjectResult okResult = (OkObjectResult)await _controller.GetDataProvider(1);
            Assert.AreEqual(1, ((DataProviderDto)okResult.Value).Id);
        }

        [TestMethod]
        public async Task GetDataProviders_ReaderReturnsADataProvider_HasCorrectName()
        {
            _mockDataProviderReader.Setup(r => r.Read(It.IsAny<int>()))
                .ReturnsAsync(new DataProvider() { Id = 1, Name = "Betfair" });
            OkObjectResult okResult = (OkObjectResult)await _controller.GetDataProvider(1);
            Assert.AreEqual("Betfair", ((DataProviderDto)okResult.Value).Name);
        }

    }
}
