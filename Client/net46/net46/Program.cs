using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace net46
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                //GlobalProxySelection.Select = new WebProxy("127.0.0.1", 8888);

                //string url = @"http://localhost:64498/mvc/Get_QueryString_ModelBinder_JsonNet?{'a':1,'b':'b string with spaces!','c':'2016-07-01'}";
                string url = @"http://localhost:64498/mvc/Get_JsonNet?json={'a':1,'b':'b string with spaces!','c':'2016-07-01'}";
                GetUrlAsync(url).Wait();

                GetAllUrls();
                PostAllUrls();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed: " + e.Message);
            }
        }

        #region GET

        private static async Task GetUrlAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string response = await client.GetStringAsync(url);
                    Console.WriteLine("GET " + url); // NB after both async methods complete
                    Console.WriteLine(response);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception! " + e.Message);
                }
                Console.WriteLine();
            }
        }

        private static void GetAllUrls()
        {

            /*
             * GET
                http://localhost:64498/mvc/Get_Default?{'a':1,'b':'b string with spaces!','c':'2016-07-01'}
                http://localhost:64498/mvc/Get_RequestBody_ModelBinder_JsonNet?{'a':1,'b':'b string with spaces!','c':'2016-07-01'}
                http://localhost:64498/mvc/Get_QueryString_ModelBinder_JsonNet?{'a':1,'b':'b string with spaces!','c':'2016-07-01'}
                http://localhost:64498/mvc/Get_JsonNet?json={'a':1,'b':'b string with spaces!','c':'2016-07-01'}

                http://localhost:64498/mvc/Get_Default?{%22a%22:1,%22b%22:%22b%20string%20with%20spaces!%22,%22c%22:%222016-07-01%22}
                http://localhost:64498/mvc/Get_RequestBody_ModelBinder_JsonNet?{%22a%22:1,%22b%22:%22b%20string%20with%20spaces!%22,%22c%22:%222016-07-01%22}
                http://localhost:64498/mvc/Get_QueryString_ModelBinder_JsonNet?{%22a%22:1,%22b%22:%22b%20string%20with%20spaces!%22,%22c%22:%222016-07-01%22}
                http://localhost:64498/mvc/Get_JsonNet?json=%7B%22a%22%3A1%2C%22b%22%3A%22b%20string%20with%20spaces!%22%2C%22c%22%3A%222016-07-01%22%7D
             */

            // Trying to get Fiddler to pickup HttpClient requests:
            //string response = await client.GetStringAsync("http://127.0.0.4/");
            //string response = await client.GetStringAsync("http://127.0.0.4/ema/GetPortfolioHoldings?portfolio=OGEMDF3");
            //string response = await client.GetStringAsync("http://127.0.0.2/ema/GetPortfolioHoldings?portfolio=OGEMDF3");
            //string response = await client.GetStringAsync("http://CITICOPELYNLT:32640/ema/GetPortfolioHoldings?portfolio=OGEMDF3");
            //string response = await client.GetStringAsync("http://localhost.fiddler:32640/ema/GetPortfolioHoldings?portfolio=OGEMDF3");

            // UrlDecoded
            var get_urls_decoded = new List<string>
            {
                 @"http://localhost:64498/mvc/Get_Default?{'a':1,'b':'b string with spaces!','c':'2016-07-01'}",
                 @"http://localhost:64498/mvc/Get_RequestBody_ModelBinder_JsonNet?{'a':1,'b':'b string with spaces!','c':'2016-07-01'}",
                 @"http://localhost:64498/mvc/Get_QueryString_ModelBinder_JsonNet?{'a':1,'b':'b string with spaces!','c':'2016-07-01'}",
                 @"http://localhost:64498/mvc/Get_JsonNet?json={'a':1,'b':'b string with spaces!','c':'2016-07-01'}",
            };
            Console.WriteLine("--------------\nDecoded urls\n--------------");
            get_urls_decoded.ForEach(x => GetUrlAsync(x).Wait());

            // UrlEncoded
            var get_urls_encoded = new List<string>
            {
                 @"http://localhost:64498/mvc/Get_Default?{%22a%22:1,%22b%22:%22b%20string%20with%20spaces!%22,%22c%22:%222016-07-01%22}",
                 @"http://localhost:64498/mvc/Get_RequestBody_ModelBinder_JsonNet?{%22a%22:1,%22b%22:%22b%20string%20with%20spaces!%22,%22c%22:%222016-07-01%22}",
                 @"http://localhost:64498/mvc/Get_QueryString_ModelBinder_JsonNet?{%22a%22:1,%22b%22:%22b%20string%20with%20spaces!%22,%22c%22:%222016-07-01%22}",
                 @"http://localhost:64498/mvc/Get_JsonNet?json=%7B%22a%22%3A1%2C%22b%22%3A%22b%20string%20with%20spaces!%22%2C%22c%22%3A%222016-07-01%22%7D",
            };
            Console.WriteLine("--------------\nEncoded urls\n--------------");
            get_urls_encoded.ForEach(x => GetUrlAsync(x).Wait());
        }

        #endregion

        #region POST

        private static async Task<string> PostUrlTxJsonRxJsonAsync(string url, string jsonTx)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Send json
                    var response = await client.PostAsync(url, new StringContent(jsonTx, Encoding.UTF8, "application/json"));
                    // Receive json
                    var jsonRx = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("POST " + url); // NB after both async methods complete
                    Console.WriteLine(jsonRx);
                    Console.WriteLine();
                    return jsonRx;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception! " + e.Message);
                    Console.WriteLine();
                    return null;
                }
            }
        }

        private static async Task<string> PostUrlTxFormDataRxJsonAsync(string url, Dictionary<string, string> formdata)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Send formdata
                    var response = await client.PostAsync(url, new FormUrlEncodedContent(formdata.ToList()));
                    // Receive json
                    var jsonRx = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("POST " + url); // NB after both async methods complete
                    Console.WriteLine(jsonRx);
                    Console.WriteLine();
                    return jsonRx;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception! " + e.Message);
                    Console.WriteLine();
                    return null;
                }
            }
        }

        #region Tx Types (from aspnetmvc5)

        class yyyyMMdd : IsoDateTimeConverter
        {
            public yyyyMMdd()
            {
                base.DateTimeFormat = "yyyy-MM-dd";
            }
        }

        public class Inputs
        {
            // NOTE: must be properties (NOT fields) in order for Default MVC model binding to work. Can be fields if Json.Net is explicitly used.
            public int a { get; set; }
            public string b { get; set; }
            [JsonConverter(typeof(yyyyMMdd))] // NOTE: not necessary to deserialize yyyy-MM-dd (will read this format and others). Rather it is used to force dates to be in this format on read and write.
            public DateTime c { get; set; }
        }

        #endregion

        private static void PostAllUrls()
        {
            /*
             * POST
                http://localhost:64498/mvc/Post_FormData_Default
                    Content-Type: application/x-www-form-urlencoded; charset=UTF-8
                    {"a":1,"b":"b string with spaces!","c":"2016-07-01"}

                http://localhost:64498/mvc/Post_RequestBody_Default
                    Content-Type: application/json
                    {"a":1,"b":"b string with spaces!","c":"2016-07-01"}

                http://localhost:64498/mvc/Post_FormData_JsonNet
                    Content-Type: application/x-www-form-urlencoded; charset=UTF-8    
                    json=%7B%22a%22%3A1%2C%22b%22%3A%22b+string+with+spaces!%22%2C%22c%22%3A%222016-07-01%22%7D

                http://localhost:64498/mvc/Post_RequestBody_JsonNet
                    Content-Type: application/x-www-form-urlencoded; charset=UTF-8
                    json=%7B%22a%22%3A1%2C%22b%22%3A%22b+string+with+spaces!%22%2C%22c%22%3A%222016-07-01%22%7D
             */

            var inputs = new Inputs { a = 2, b = "string from net46", c = new DateTime(2016, 9, 2) };
            var formdata = new Dictionary<string, string>
            {
                { "a", inputs.a.ToString() },
                { "b", inputs.b },
                { "c", inputs.c.ToString("yyyy-MM-dd") },
            };
            var jsonTx = Newtonsoft.Json.JsonConvert.SerializeObject(inputs);
            var formdataJson = new Dictionary<string, string>
            {
                { "json", jsonTx }
            };
            var post_urls = new List<PostSettings>
            {
                 new PostSettings { Url = @"http://localhost:64498/mvc/Post_FormData_Default", Tx = PostSettings.SendType.FormData },
                 new PostSettings { Url = @"http://localhost:64498/mvc/Post_RequestBody_Default", Tx = PostSettings.SendType.Json_Default },
                 new PostSettings { Url = @"http://localhost:64498/mvc/Post_FormData_JsonNet", Tx = PostSettings.SendType.Json_Custom },    // this 2 are the same (just testing [FromBody] in aspnetmvc5.Controllers.MvcController.Post_RequestBody_JsonNet)
                 new PostSettings { Url = @"http://localhost:64498/mvc/Post_RequestBody_JsonNet", Tx = PostSettings.SendType.Json_Custom }, // this 2 are the same
            };
            Console.WriteLine("--------------\nPost urls\n--------------");
            post_urls.ForEach(x => {
                switch (x.Tx)
                {
                    case PostSettings.SendType.Json_Default:
                        PostUrlTxJsonRxJsonAsync(x.Url, jsonTx).Wait();
                        break;
                    case PostSettings.SendType.Json_Custom:
                        PostUrlTxFormDataRxJsonAsync(x.Url, formdataJson).Wait();   // NOTE: sends formdata : json=formencoded(jsonTx)
                        break;
                    case PostSettings.SendType.FormData:
                        PostUrlTxFormDataRxJsonAsync(x.Url, formdata).Wait();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            });
        }

        public class PostSettings
        {
            public string Url;
            public SendType Tx;

            public enum SendType
            {
                Json_Default,   // json in requestbody, deserialized using default asp.net serializer
                Json_Custom,    // json as string in formdata, deserialized using json.net serializer. (formdata => json=formencoded(jsonTx))
                FormData
            }
        }

        #endregion
    }
}
