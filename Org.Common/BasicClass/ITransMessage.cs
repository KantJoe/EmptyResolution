using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Common.BasicClass
{
    /// <summary>
    /// 服务、Api统一返回结果类
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface ITransMessage<TResult>
    {
        /// <summary>
        /// 消息
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        TResult Result { get; set; }

        /// <summary>
        /// 本次传输状态
        /// </summary>
        TransStatus Status { get; set; }
    }

    /// <summary>
    /// 传输状态枚举
    /// </summary>
    public enum TransStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 数据验证出错
        /// </summary>
        ValidateError = 1,
        /// <summary>
        /// 执行出错
        /// </summary>
        Error = 2,
        /// <summary>
        /// 自定义错误
        /// </summary>
        CustomerError = 3,
        /// <summary>
        /// 未登录
        /// </summary>
        NoLogin = 4,
        /// <summary>
        /// 没有权限
        /// </summary>
        NoAccess = 5
    }
}
