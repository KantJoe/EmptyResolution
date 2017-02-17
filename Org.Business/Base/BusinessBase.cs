using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Org.Business.Base
{
    public abstract class BusinessBase<T> where T : class
    {
        static T instance;
        static object anycObj = new object();
        protected static object cacheAnycObj = new object();

        /// <summary>
        /// 获取实例对象
        /// </summary>
        /// <returns></returns>
        public static T GetInstance()
        {
            lock (anycObj)
            {
                if (instance == null)
                {
                    ConstructorInfo[] ciArray = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);

                    if (ciArray.Length > 0)
                    {
                        foreach (ConstructorInfo ci in ciArray)
                        {
                            if (0 == ci.GetParameters().Length)
                            {
                                instance = (T)ci.Invoke(null);
                                break;
                            }
                        }
                    }
                    else
                    {
                        instance = Activator.CreateInstance<T>();
                    }
                }
            }

            return instance;
        }
    }
}
