﻿namespace mvc_api.Model
{
    public class Product
    {
        // type of data for Product 
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public Category ProductCategory { get; set; }
    }
}
