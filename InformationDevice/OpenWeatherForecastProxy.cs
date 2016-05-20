using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace InformationDevice
{
    class OpenWeatherForecastProxy
    {
        public async static Task<ForecastRootobject> GetForecast()
        {
            string line = "";

            try
            {
                using (StreamReader sr = new StreamReader(new FileStream("Assets/api.txt", FileMode.Open, FileAccess.Read)))
                {

                    line = sr.ReadLine();
                }
            }
            catch
            {

            }

            var http = new HttpClient();
            var response = await http.GetAsync("http://api.openweathermap.org/data/2.5/forecast/daily?q=Dachau,de&cnt=3&units=metric&APPID=" + line);
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(ForecastRootobject));

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (ForecastRootobject)serializer.ReadObject(ms);

            return data;
        }
    }

    [DataContract(Namespace = "RootObject")]
    public class ForecastRootobject
    {
        [DataMember]
        public City city { get; set; }

        [DataMember]
        public string cod { get; set; }

        [DataMember]
        public float message { get; set; }

        [DataMember]
        public int cnt { get; set; }

        [DataMember]
        public List[] list { get; set; }
    }

    [DataContract]
    public class City
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public Coord coord { get; set; }

        [DataMember]
        public string country { get; set; }

        [DataMember]
        public int population { get; set; }
    }

    [DataContract(Name = "Coord")]
    public class ForecastCoord
    {
        [DataMember]
        public float lon { get; set; }

        [DataMember]
        public float lat { get; set; }
    }

    [DataContract]
    public class List
    {
        [DataMember]
        public int dt { get; set; }

        [DataMember]
        public Temp temp { get; set; }

        [DataMember]
        public float pressure { get; set; }

        [DataMember]
        public int humidity { get; set; }

        [DataMember]
        public Weather[] weather { get; set; }

        [DataMember]
        public float speed { get; set; }

        [DataMember]
        public int deg { get; set; }

        [DataMember]
        public int clouds { get; set; }

        [DataMember]
        public float rain { get; set; }
    }

    [DataContract]
    public class Temp
    {
        [DataMember]
        public float day { get; set; }

        [DataMember]
        public float min { get; set; }

        [DataMember]
        public float max { get; set; }

        [DataMember]
        public float night { get; set; }

        [DataMember]
        public float eve { get; set; }

        [DataMember]
        public float morn { get; set; }
    }

    [DataContract(Name = "Weather")]
    public class ForecastWeather
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string main { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public string icon { get; set; }
    }

}
