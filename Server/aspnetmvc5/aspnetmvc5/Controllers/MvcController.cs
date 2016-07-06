using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace aspnetmvc5.Controllers
{
    internal class JsonNetRequestBodyModelBinder : IModelBinder
    {
        // JsonNetRequestBodyModelBinder allows model binding using Json.Net
        //  - it support json sent in the [requestbody] only (not [querystring] or [formdata])
        //  - hence it is only appropriate for $.post({ contentType: 'application/json' ... }) style requests (not GET requests)

        // The alternative is to use a ValueProvider (see link below) - but that gets applied globally (in a Global.asax) so it's not explicit at each Action method

        // From: http://stackoverflow.com/questions/23995210/how-to-use-json-net-for-json-modelbinding-in-an-mvc5-project

        // MVC has 2 concepts: (see http://haacked.com/archive/2011/06/30/whatrsquos-the-difference-between-a-value-provider-and-model-binder.aspx/)
        //  - ValueProviders - deserializes values one at a time in order to look for model binding errors per value)
        //  - ModelBinders - supposed to deserialize dictionary values to model

        // The default MVC model binder doesn't use json.net (in MVC 5):
        //  hence "2016-07-01" in [querystring], [formdata] or [requestbody] won't deserialize to a date during modelbinding, 
        //  whereas it does when you explicitly use json.net to deserialize e.g. JsonConvert.DeserializeObject<Inputs>(data);

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // Read the [requestbody]
            controllerContext.HttpContext.Request.InputStream.Position = 0;
            var stream = controllerContext.RequestContext.HttpContext.Request.InputStream;
            var readStream = new StreamReader(stream, Encoding.UTF8);
            var json = readStream.ReadToEnd();

            // Deserialize it
            return JsonConvert.DeserializeObject(json, bindingContext.ModelType);
        }
    }

    internal class JsonNetQueryStringModelBinder : IModelBinder
    {
        // JsonNetQueryStringModelBinder allows model binding using Json.Net
        //  - it support json sent in the [querystring] only (not [requestbody] or [formdata])
        //  - hence it is only appropriate for GET requests

        // See notes in JsonNetRequestBodyModelBinder above

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // Read the [querystring]
            // NOTE: Expects url like so:
            //  GET  http://localhost:64498/mvc/Get_QueryString_ModelBinder_JsonNet?{%22a%22:1,%22b%22:%22b_string%22,%22c%22:%222016-07-01%22}
            //  i.e. http://localhost:64498/mvc/Get_QueryString_ModelBinder_JsonNet?{"a":1,"b":"b_string","c":"2016-07-01"}
            var rawUrl = controllerContext.HttpContext.Request.RawUrl;
            var querystring = rawUrl.Substring(rawUrl.IndexOf('?') + 1);
            var json = System.Web.HttpUtility.UrlDecode(querystring);

            // Deserialize it
            return JsonConvert.DeserializeObject(json, bindingContext.ModelType);
        }
    }

    class yyyyMMdd : IsoDateTimeConverter
    {
        public yyyyMMdd()
        {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }


    public class MvcController : Controller
    {
        public class Inputs
        {
            // NOTE: must be properties (NOT fields) in order for Default MVC model binding to work. Can be fields if Json.Net is explicitly used.
            public int a { get; set; }
            public string b { get; set; }
            [JsonConverter(typeof(yyyyMMdd))] // NOTE: not necessary to deserialize yyyy-MM-dd (will read this format and others). Rather it is used to force dates to be in this format on read and write.
            public DateTime c { get; set; }
        }
        
        #region GET

        // GET /mvc/Get_Default?a=1&b="b_string"&c="2016-07-01"
        [System.Web.Mvc.HttpGet]
        public ActionResult Get_Default(Inputs inputs) // will deserialize Inputs {a, b} correctly but not Inputs {c}
        {
            return DoAction(inputs);
        }

        // NOTE: it's not very easy to send values in the [requestbody] for a GET request
        // See demo.js
        // The following [querystring] approach won't work (JsonNetRequestBodyModelBinder doesn't support params in the [querystring] only the [requestbody]):
        // GET /mvc/Get_RequestBody_ModelBinder_JsonNet?a=1&b="b_string"&c="2016-07-01"
        [System.Web.Mvc.HttpGet]
        public ActionResult Get_RequestBody_ModelBinder_JsonNet([ModelBinder(typeof(JsonNetRequestBodyModelBinder))] Inputs inputs)
        {
            return DoAction(inputs);
        }

        //  GET  http://localhost:64498/mvc/Get_QueryString_ModelBinder_JsonNet?{%22a%22:1,%22b%22:%22b_string%22,%22c%22:%222016-07-01%22}
        //  i.e. http://localhost:64498/mvc/Get_QueryString_ModelBinder_JsonNet?{"a":1,"b":"b_string","c":"2016-07-01"}
        [System.Web.Mvc.HttpGet]
        public ActionResult Get_QueryString_ModelBinder_JsonNet([ModelBinder(typeof(JsonNetQueryStringModelBinder))] Inputs inputs)
        {
            return DoAction(inputs);
        }

        // GET /mvc/Get_JsonNet?json={a:1,b:"b_string",c:"2016-07-01"}
        [System.Web.Mvc.HttpGet]
        public ActionResult Get_JsonNet(string json)
        {
            return DoActionString(json);
        }

        #endregion

        #region POST

        // See demo.js
        // POST /mvc/Post_FormData_Default
        [System.Web.Mvc.HttpPost]
        public ActionResult Post_FormData_Default(Inputs inputs)
        {
            return DoAction(inputs);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Post_RequestBody_Default([FromBody] Inputs inputs)
        {
            return DoAction(inputs);
        }

        // See demo.js
        // POST /mvc/Post_FormData_JsonNet
        [System.Web.Mvc.HttpPost]
        public ActionResult Post_FormData_JsonNet(string json)
        {
            return DoActionString(json);
        }

        // See demo.js
        // POST /mvc/Post_RequestBody_JsonNet
        [System.Web.Mvc.HttpPost]
        public ActionResult Post_RequestBody_JsonNet([FromBody] string json)
        {
            return DoActionString(json);
        }

        #endregion

        #region Implementation

        ActionResult DoAction(Inputs inputs)
        {
            if (inputs == null)
                return JsonNet(new { result = "fail", message = "inputs = null" });

            try
            {
                return JsonNet(new { result = "success", inputs = inputs });
            }
            catch (Exception e)
            {
                return JsonNet(new { result = "fail", message = e.Message });
            }
        }

        ActionResult DoActionString(string json)
        {
            if (json == null)
                return JsonNet(new { result = "fail", message = "json = null" });

            try
            {
                var inputs = JsonConvert.DeserializeObject<Inputs>(json);
                return JsonNet(new { result = "success", inputs = inputs });
            }
            catch (Exception e)
            {
                return JsonNet(new { result = "fail", message = e.Message });
            }
        }
        ActionResult JsonNet(object o)
        {
            return new ContentResult
            {
                ContentType = "application/json",
                ContentEncoding = Encoding.UTF8,
                Content = JsonConvert.SerializeObject(o)
            };
        }

        #endregion
    }
}