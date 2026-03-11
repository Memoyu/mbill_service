using Memo.Bill.Application.Aggregations.Common;
using Memo.Bill.Application.Common.Models.Services.Amap;

namespace Memo.Bill.Application.Common.Mappings
{
    public class AggregationRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<GetGeocodeRegeoResponse, AddressResult>()
               .Map(d => d.Address, s => s.Regeocode.FormattedAddress)
               .Map(d => d.Country, s => s.Regeocode.AddressComponent.Country)
               .Map(d => d.Province, s => s.Regeocode.AddressComponent.Province)
               .Map(d => d.City, s => s.Regeocode.AddressComponent.City)
               .Map(d => d.District, s => s.Regeocode.AddressComponent.District)
               .Map(d => d.Township, s => s.Regeocode.AddressComponent.Township)
               .Map(d => d.Street, s => s.Regeocode.AddressComponent.StreetNumber.Street)
               .Map(d => d.StreetNumber, s => s.Regeocode.AddressComponent.StreetNumber.Number)
               .Map(d => d.Citycode, s => s.Regeocode.AddressComponent.Citycode)
               .Map(d => d.Adcode, s => s.Regeocode.AddressComponent.Adcode)
               .Map(d => d.Towncode, s => s.Regeocode.AddressComponent.Towncode);

            config.ForType<WeatherLive, WeatherInfoResult>()
              .Map(d => d.Weather, s => s.Weather)
              .Map(d => d.Temperature, s => s.Temperature)
              .Map(d => d.WindDirection, s => s.WindDirection)
              .Map(d => d.WindPower, s => s.WindPower)
              .Map(d => d.Humidity, s => s.Humidity)
              .Map(d => d.ReportTime, s => s.ReportTime);
        }
    }
}
