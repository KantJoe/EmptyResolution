using Newtonsoft.Json.Converters;
using Org.Common.BasicClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Org.Common.Mvc
{
    /// <summary>
    /// 基础控制器
    /// </summary>
    public class BaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            string controllerName = filterContext.RouteData.Values["controller"].ToString().ToLower();
            string actionName = filterContext.RouteData.Values["action"].ToString().ToLower();

            string returnType = ActionResultTypeMapping.GetVal(controllerName + "." + actionName);

            if (string.IsNullOrEmpty(returnType))
            {
                MethodInfo method = filterContext.Controller.GetType().GetMethod(actionName, BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.InvokeMethod);

                if (method != null)
                {
                    returnType = method.ReturnType == null ? "void" : method.ReturnType.Name;
                    ActionResultTypeMapping.SetVal(controllerName + "." + actionName, returnType);
                }
            }

            if (returnType == typeof(JsonResult).Name)
            {
                var msg = filterContext.Exception.GetType() == typeof(HttpAntiForgeryException) ? "非法请求" : filterContext.Exception.Message;

                if (!string.IsNullOrEmpty(Request.QueryString["callback"]))
                {
                    filterContext.Result = new JsonpResult(new JsonTransResponse() { Message = msg, Status = TransStatus.Error, Result = null });
                }
                else
                {
                    filterContext.Result = CJson(new JsonTransResponse() { Message = msg, Status = TransStatus.Error, Result = null }, JsonRequestBehavior.AllowGet);
                }

                filterContext.ExceptionHandled = true;

                //Ajax异常记录处理
                if (filterContext.Exception.GetType() != typeof(HttpAntiForgeryException))
                {
                    ;
                }
                else
                {
                    ;
                }
            }
            else
            {
                base.OnException(filterContext);
            }
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        /// 重写404
        /// </summary>
        /// <param name="statusDescription"></param>
        /// <returns></returns>
        protected override HttpNotFoundResult HttpNotFound(string statusDescription)
        {
            this.Response.StatusCode = 404;
            this.Response.TrySkipIisCustomErrors = true;
            this.Response.StatusDescription = statusDescription;
            Response.Clear();
            Response.Redirect("~/Error/Error404");
            Response.End();
            return null;
        }

        /// <summary>
        /// 返回带执行状态的Json
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public JsonResult CJson<T>(ITransMessage<T> response, string dateFormateString = null)
        {
            CJsonResult result = new CJsonResult(null, dateFormateString);
            result.Data = response;

            return result;
        }

        /// <summary>
        /// 返回带执行状态的Json
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public JsonResult CJson<T>(ITransMessage<T> response, JsonRequestBehavior jsonbehavior, string dateFormateString = null)
        {
            CJsonResult result = new CJsonResult(null, dateFormateString);
            result.Data = response;
            result.JsonRequestBehavior = jsonbehavior;

            return result;
        }

        /// <summary>
        /// 返回带执行状态的Json
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public JsonResult CJson<T>(ITransMessage<T> response, JsonRequestBehavior jsonbehavior, string contentType, string dateFormateString = null)
        {
            CJsonResult result = new CJsonResult(null, dateFormateString);
            result.Data = response;
            result.JsonRequestBehavior = jsonbehavior;
            result.ContentType = contentType;
            return result;
        }

        /// <summary>
        /// 返回带执行状态的Json
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public JsonResult CJson<T>(ITransMessage<T> response, JsonRequestBehavior jsonbehavior, string contentType, Encoding encoding, string dateFormateString = null)
        {
            CJsonResult result = new CJsonResult(null, dateFormateString);
            result.Data = response;
            result.JsonRequestBehavior = jsonbehavior;
            result.ContentType = contentType;
            result.ContentEncoding = encoding;
            return result;
        }

        /// <summary>
        /// json传输返回类
        /// </summary>
        public class JsonTransResponse : ITransMessage<object>
        {
            /// <summary>
            /// 消息
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// 返回结果
            /// </summary>
            public object Result { get; set; }

            /// <summary>
            /// 本次传输状态
            /// </summary>
            public TransStatus Status { get; set; }
        }
    }

    public static class ActionResultTypeMapping
    {
        public static Dictionary<string, string> maps;

        static object anys = new object();

        static ActionResultTypeMapping()
        {
            maps = new Dictionary<string, string>();
        }

        public static string GetVal(string actoinName)
        {
            lock (anys)
            {
                if (maps.ContainsKey(actoinName))
                {
                    return maps[actoinName];
                }
                else
                {
                    return null;
                }
            }
        }

        public static void SetVal(string actoinName, string value)
        {
            if (maps.ContainsKey(actoinName))
            {
                maps[actoinName] = value;
            }
            else
            {
                maps.Add(actoinName, value);
            }
        }
    }

    public class JsonpResult : JsonResult
    {
        /// <summary>
        /// 时间格式化字符串
        /// </summary>
        public string DateFormateStr { get; set; }

        public JsonpResult(string dateFormateString = null)
        {
            this.DateFormateStr = dateFormateString;

            if (string.IsNullOrEmpty(this.DateFormateStr))
            {
                this.DateFormateStr = "yyyy-MM-dd HH:mm:ss";
            }
        }

        public JsonpResult(object data, string dateFormateString = null)
        {
            this.Data = data;

            this.DateFormateStr = dateFormateString;

            if (string.IsNullOrEmpty(this.DateFormateStr))
            {
                this.DateFormateStr = "yyyy-MM-dd HH:mm:ss";
            }
        }

        public override void ExecuteResult(ControllerContext controllerContext)
        {
            if (controllerContext != null)
            {
                HttpResponseBase Response = controllerContext.HttpContext.Response;
                HttpRequestBase Request = controllerContext.HttpContext.Request;

                string callbackfunction = Request.QueryString["callback"];

                if (string.IsNullOrEmpty(callbackfunction))
                {
                    throw new Exception("Callback function name must be provided in the request!");
                }

                Response.ContentType = "application/x-javascript";

                IsoDateTimeConverter convert = new IsoDateTimeConverter();
                convert.DateTimeFormat = this.DateFormateStr;
                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(this.Data, convert);

                if (!string.IsNullOrEmpty(jsonString))
                {
                    string p = @"\\/Date\((\d+)\)\\/";
                    MatchEvaluator matchEvaluator = new MatchEvaluator(this.ConvertJsonDateToDateString);
                    Regex reg = new Regex(p);
                    jsonString = reg.Replace(jsonString, matchEvaluator);
                }

                Response.Write(string.Format("{0}({1});", callbackfunction, jsonString));
            }
        }

        /// <summary>  
        /// 将Json序列化的时间由/Date(1294499956278)转为字符串 .
        /// </summary>  
        /// <param name="m">正则匹配</param>
        /// <returns>格式化后的字符串</returns>
        private string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString(DateFormateStr);
            return result;
        }
    }

    public class CJsonResult : JsonResult
    {
        /// <summary>
        /// 时间格式化字符串
        /// </summary>
        public string DateFormateStr { get; set; }

        public CJsonResult(object _data = null, string dateFormateString = null)
        {
            this.Data = _data;

            this.DateFormateStr = dateFormateString;

            if (string.IsNullOrEmpty(this.DateFormateStr))
            {
                this.DateFormateStr = "yyyy-MM-dd HH:mm:ss";
            }
        }

        public override void ExecuteResult(ControllerContext controllerContext)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = controllerContext.HttpContext.Response;

            if (!string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = "text/html";
            }

            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }

            IsoDateTimeConverter convert = new IsoDateTimeConverter();
            convert.DateTimeFormat = this.DateFormateStr;
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(Data, convert);

            if (!string.IsNullOrEmpty(jsonString))
            {
                string p = @"\\/Date\((\d+)\)\\/";
                MatchEvaluator matchEvaluator = new MatchEvaluator(this.ConvertJsonDateToDateString);
                Regex reg = new Regex(p);
                jsonString = reg.Replace(jsonString, matchEvaluator);
            }

            response.Write(jsonString);
        }

        /// <summary>  
        /// 将Json序列化的时间由/Date(1294499956278)转为字符串 .
        /// </summary>  
        /// <param name="m">正则匹配</param>
        /// <returns>格式化后的字符串</returns>
        private string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString(DateFormateStr);
            return result;
        }

    }
}
