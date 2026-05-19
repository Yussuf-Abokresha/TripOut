using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Infrastructure.Seeds.Dto
{
    public class PlaceCsv
    {
        public string PlaceName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string Tags { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CategoryClean { get; set; } = string.Empty;
        public string Governorate { get; set; } = string.Empty;
    }

    public sealed class PlaceMap : ClassMap<PlaceCsv>
    {
        public PlaceMap()
        {
            Map(x => x.PlaceName).Name("place_name");
            Map(x => x.Address).Name("address");
            Map(x => x.Lat).Name("lat");
            Map(x => x.Lon).Name("lon");
            Map(x => x.Tags).Name("tags");
            Map(x => x.Description).Name("description");
            Map(x => x.CategoryClean).Name("category_clean");
            Map(x => x.Governorate).Name("governorate");
        }
    }
}