using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LuizaLabs.Challenge.Models
{
    public class ProductResponse
    {
        [JsonProperty("meta")]
        public MetaRecords Meta { get; set; }

        [JsonProperty("products")]
        public List<Product> Products { get; set; }
        public class MetaRecords
        {
            [JsonProperty("page_number")]
            public int PageNumber { get; set; }
            [JsonProperty("page_size")]
            public int PageSize { get; set; }
        }
    }
    public class Product
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}