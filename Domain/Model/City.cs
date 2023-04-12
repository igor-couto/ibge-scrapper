﻿namespace IBGE_Scrapper.Domain.Model
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string Microregion { get; set; }
        public string Mesoregion { get; set; }
        public string Region { get; set; }

        //Districts": ["a","b"]
    }
}
