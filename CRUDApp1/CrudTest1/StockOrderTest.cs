using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;

namespace CrudTest1
{
    public class StockOrderTest
    {
        private readonly IStocksService _stocksService;

        public StockOrderTest()
        {
            _stocksService = new StockService();
        }

        #region Create Buy Order Tests

        [Fact]
        public void CreateBuyOrder_NullRequest()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = null;

            //assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void CreateBuyOrder_NullRequestParameter()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockName = null};

            //assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void CreateBuyOrder_ValidRequest() 
        { 
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() 
            { 
                StockName = "Test Stock", 
                StockSymbol = "TST", 
                DateAndTimeOfOrder = DateTime.Now, 
                Quantity = 100, 
                Price = 10.5 
            };

            //Act
            BuyOrderResponse? buyOrderResponse = _stocksService.CreateBuyOrder(buyOrderRequest);

            //Assert
            Assert.NotNull(buyOrderResponse);
        }
        #endregion

        #region Create Sell Order Tests
        [Fact]
        public void CreateSellOrder_NullRequest()
        {
            SellOrderRequest? sellOrderRequest = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public void CreateSellOrder_NullRequestParameter()
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest { StockName = null };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public void CreateSellOrder_ValidRequest()
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest
            {
                StockName = "Test Stock",
                StockSymbol = "TST",
                DateAndTimeOfOrder = DateTime.Now,
                Quantity = 100,
                Price = 10.5
            };

            SellOrderResponse? sellOrderResponse = _stocksService.CreateSellOrder(sellOrderRequest);

            Assert.NotNull(sellOrderResponse);
        }

        #endregion

        #region Get Buy Order Tests
        [Fact]
        public void GetBuyOrders_ReturnsAllOrdersWithCorrectData()
        {
            // Arrange
            var buyOrders = new List<BuyOrderRequest>
    {
        new BuyOrderRequest
        {
            StockName = "Apple",
            StockSymbol = "AAPL",
            DateAndTimeOfOrder = DateTime.Now,
            Quantity = 50,
            Price = 150.25
        },
        new BuyOrderRequest
        {
            StockName = "Microsoft",
            StockSymbol = "MSFT",
            DateAndTimeOfOrder = DateTime.Now.AddMinutes(1),
            Quantity = 100,
            Price = 250.75
        },
        new BuyOrderRequest
        {
            StockName = "Tesla",
            StockSymbol = "TSLA",
            DateAndTimeOfOrder = DateTime.Now.AddMinutes(2),
            Quantity = 200,
            Price = 700.10
        }
    };

            foreach (var request in buyOrders)
            {
                _stocksService.CreateBuyOrder(request);
            }

            // Act
            List<BuyOrderResponse> responses = _stocksService.GetBuyOrders();

            // Assert
            Assert.Equal(buyOrders.Count, responses.Count);

            for (int i = 0; i < buyOrders.Count; i++)
            {
                Assert.Equal(buyOrders[i].StockName, responses[i].StockName);
                Assert.Equal(buyOrders[i].StockSymbol, responses[i].StockSymbol);
                Assert.Equal(buyOrders[i].DateAndTimeOfOrder, responses[i].DateAndTimeOfOrder);
                Assert.Equal(buyOrders[i].Quantity, responses[i].Quantity);
                Assert.Equal(buyOrders[i].Price, responses[i].Price);
            }
        }

        #endregion

        #region Get Sell Order Tests
        [Fact]
        public void GetSellOrders_ReturnsAllOrdersWithCorrectData()
        {
            // Arrange
            var sellOrders = new List<SellOrderRequest>
    {
        new SellOrderRequest
        {
            StockName = "Apple",
            StockSymbol = "AAPL",
            DateAndTimeOfOrder = DateTime.Now,
            Quantity = 50,
            Price = 150.25
        },
        new SellOrderRequest
        {
            StockName = "Microsoft",
            StockSymbol = "MSFT",
            DateAndTimeOfOrder = DateTime.Now.AddMinutes(1),
            Quantity = 100,
            Price = 250.75
        },
        new SellOrderRequest
        {
            StockName = "Tesla",
            StockSymbol = "TSLA",
            DateAndTimeOfOrder = DateTime.Now.AddMinutes(2),
            Quantity = 200,
            Price = 700.10
        }
    };

            foreach (var request in sellOrders)
            {
                _stocksService.CreateSellOrder(request);
            }

            // Act
            List<SellOrderResponse> responses = _stocksService.GetSellOrders();

            // Assert
            Assert.Equal(sellOrders.Count, responses.Count);

            for (int i = 0; i < sellOrders.Count; i++)
            {
                Assert.Equal(sellOrders[i].StockName, responses[i].StockName);
                Assert.Equal(sellOrders[i].StockSymbol, responses[i].StockSymbol);
                Assert.Equal(sellOrders[i].DateAndTimeOfOrder, responses[i].DateAndTimeOfOrder);
                Assert.Equal(sellOrders[i].Quantity, responses[i].Quantity);
                Assert.Equal(sellOrders[i].Price, responses[i].Price);
            }
        }

        #endregion
    }
}
