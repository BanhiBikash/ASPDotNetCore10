using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ServicesContracts.DTO
{
    public class BuyOrderRequest
    {
        string StockSymbol { get; set; }

        string StockName { get; set; }

        DateTime DateAndTimeOfOrder { get; set; }

        //[Value should be between 1 and 100000] 
        [Range(1, 100000)]
        int Quantity { get; set; }

        //[Value should be between 1 and 10000] 
        [Range(1, 1000)]
        double Price { get; set; }
    }
}
