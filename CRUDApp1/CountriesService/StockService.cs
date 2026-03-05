using System;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class StockService : IStocksService
    {
        private readonly List<BuyOrder>? _buyOrders;
        private readonly List<SellOrder>? _sellOrders;

        public StockService() 
        {
            _buyOrders = new List<BuyOrder>();
            _sellOrders = new List<SellOrder>();
        }

        public BuyOrderResponse CreateBuyOrder(BuyOrderRequest buyOrderRequest)
        {
            //if the request is null, throw an exception
            if (buyOrderRequest == null) throw new ArgumentNullException(nameof(buyOrderRequest));

            //if any property of the request is null, throw an exception
            foreach (var property in typeof(BuyOrderRequest).GetProperties()) 
            {
                if (property.GetValue(buyOrderRequest) == null) 
                {
                    throw new ArgumentException($"Property {property.Name} is null in the buy order request.");
                }
            }

            //current count of buy orders in the list, we will use this to check if the new buy order was successfully added to the list
            int buyOrderQuantity = _buyOrders.Count();

            //adding the new buy order to the list, we will check if the count of buy orders in the list has increased by 1 after adding the new buy order
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid(); //assign a new unique ID to the buy order
            _buyOrders.Add(buyOrder);

            //buy order successfully added to the list, now create a response object and return it
            if (_buyOrders.Count() > buyOrderQuantity)
            {
                BuyOrderResponse buyOrderResponse = buyOrder.ToBuyOrderResponse();

                if (buyOrderResponse != null)
                {
                    return buyOrderResponse;
                }

                throw new Exception("Failed to create buy order response.");
            }
            else
            {
                throw new Exception("Failed to add buy order.");
            }
        }

        public SellOrderResponse CreateSellOrder(SellOrderRequest sellOrderRequest)
        {
            //if the request is null, throw an exception
            if (sellOrderRequest == null) throw new ArgumentNullException(nameof(sellOrderRequest));

            //if any property of the request is null, throw an exception
            foreach (var property in typeof(SellOrderRequest).GetProperties())
            {
                if (property.GetValue(sellOrderRequest) == null)
                {
                    throw new ArgumentException($"Property {property.Name} is null in the sell order request.");
                }
            }

            //current count of sell orders in the list, we will use this to check if the new sell order was successfully added to the list
            int sellOrderQuantity = _sellOrders.Count();

            //adding the new sell order to the list, we will check if the count of sell orders in the list has increased by 1 after adding the new buy order
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid(); //assign a new unique ID to the sell order
            _sellOrders.Add(sellOrder);

            //sell order successfully added to the list, now create a response object and return it
            if (_sellOrders.Count() > sellOrderQuantity)
            {
                SellOrderResponse sellOrderResponse = sellOrder.ToSellOrderResponse();

                if (sellOrderResponse != null)
                {
                    return sellOrderResponse;
                }

                throw new Exception("Failed to create sell order response.");
            }
            //failed to add the new sell order to the list, throw an exception
            else
            {
                throw new Exception("Failed to add sell order.");
            }
        }

        public List<BuyOrderResponse> GetBuyOrders()
        {
            return _buyOrders.Select(buyOrder => buyOrder.ToBuyOrderResponse()).ToList();
        }

        public List<SellOrderResponse> GetSellOrders()
        {
            return _sellOrders.Select(sellOrder => sellOrder.ToSellOrderResponse()).ToList();
        }
    }
}
