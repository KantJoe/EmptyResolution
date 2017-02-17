using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Org.Common.BasicClass
{
    /// <summary>
    /// 服务通用返回结果
    /// </summary>
    [DataContract]
    public class TransResponse<TResult> : TransResponse, ITransMessage<TResult>
    {
        public TransResponse()
        {
            this.Message = string.Empty;
            this.Status = TransStatus.Success;
        }

        public TransResponse(string msg)
        {
            this.Message = msg;
            this.Status = TransStatus.Success;
        }

        public TransResponse(string msg, TResult result)
        {
            this.Message = msg;
            this.Result = result;
            this.Status = TransStatus.Success;
        }

        public TransResponse(TResult result)
        {
            this.Message = string.Empty;
            this.Result = result;
            this.Status = TransStatus.Success;
        }

        /// <summary>
        /// 返回结果
        /// </summary>
        [DataMember]
        public TResult Result { get; set; }

    }

    /// <summary>
    /// 服务通用返回结果基类
    /// </summary>
    [DataContract]
    public class TransResponse
    {
        /// <summary>
        /// 消息
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// 本次传输状态
        /// </summary>
        [DataMember]
        public TransStatus Status { get; set; }
    }
}
