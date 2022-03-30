using Infrastructure;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class OebbWebService : BaseService
    {
        public OebbWebService(Database db) : base(db)
        {

        }

        private string ApiURL => @"https://konzern-apps.web.oebb.at/lok/index/";

        public async Task<List<Stop>> GetStops(VehicleClassification vehicleClassification)
        {
            var uri = new Uri(@$"{ApiURL}{vehicleClassification}");
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(uri);
            var repsonse = await new StreamReader(await response.Content.ReadAsStreamAsync()).ReadToEndAsync();
            IEnumerable<dynamic> List = JsonConvert.DeserializeObject(repsonse) as IEnumerable<dynamic>;

            List.Select(e =>
            {
                IDictionary<string, string> i = e as IDictionary<string, string>;
                return "";
            });

            return new();
        }
    }
}
