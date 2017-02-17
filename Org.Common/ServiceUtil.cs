using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Org.Common
{
    /// <summary>
    /// 服务调用工具
    /// </summary>
    public class ServiceUtil
    {
        static Binding binding;
        static string uri;

        static ServiceUtil()
        {
            uri = System.Configuration.ConfigurationManager.AppSettings["ServiceSiteUrl"];
            binding = new WSHttpBinding("ServiceBinding");

            if (!string.IsNullOrEmpty(uri))
            {
                uri = uri.Trim('/') + "/";
            }
        }


        /// <summary>
        /// 获取服务接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>() where T : class
        {
            ServiceClient<T> serviceInstance = new ServiceClient<T>(binding, uri + typeof(T).Name.Substring(1) + ".svc");
            return serviceInstance.CurrentService;
        }

        class ServiceClient<T> : ClientBase<T> where T : class
        {
            public ServiceClient(Binding bindType, string uri) : base(bindType, new EndpointAddress(uri)) { }

            /// <summary>
            /// 服务对象
            /// </summary>
            public T CurrentService { get { return base.Channel; } }
        }
    }
}
