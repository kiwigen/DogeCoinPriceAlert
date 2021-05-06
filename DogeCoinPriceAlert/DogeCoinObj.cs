using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeCoinPriceAlert
{
    /*
     * {
  "status" : "success",
  "data" : {
    "network" : "DOGE",
    "prices" : [
      {
        "price" : "0.61822",
        "price_base" : "USD",
        "exchange" : "binance",
        "time" : 1620288458
      },
      {
        "price" : "0.62042",
        "price_base" : "USD",
        "exchange" : "gemini",
        "time" : 1620288469
      }
    ]
  }
}
     */
    public class DogeCoinObj
    {
        [JsonProperty]
        public string status { get; set; }
        [JsonProperty]
        public DogeCoinStatusObj data { get; set; }
    }

    public class DogeCoinStatusObj
    {
        [JsonProperty]
        public string network { get; set; }
        [JsonProperty]
        public List<DogeCoinPrice> prices { get; set; }
    }
    public class DogeCoinPrice
    {
        [JsonProperty]
        public double price { get; set; }
        [JsonProperty]
        public string price_base { get; set; }
        [JsonProperty]
        public string exchange { get; set; }
        [JsonProperty]
        public long time { get; set; }

    }
}
