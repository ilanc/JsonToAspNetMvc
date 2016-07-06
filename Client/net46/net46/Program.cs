using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed: " + e.Message);
            }
        }

        private static async Task GetUrlAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string response = await client.GetStringAsync(url);
                    Console.WriteLine(url); // NB after request because it's async
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
    }
}
