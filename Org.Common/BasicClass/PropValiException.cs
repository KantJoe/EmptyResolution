using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Common.BasicClass
{
    /// <summary>
    /// 用于属性值验证抛出的异常类
    /// </summary>
    public class PropValiException : Exception
    {
        public PropValiException()
            : base()
        {

        }

        public PropValiException(string msg)
            : base(msg)
        {

        }

        public PropValiException(string msg, Exception innerExp)
            : base(msg, innerExp)
        {

        }
    }
}
