using Org.Common.BasicClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Dispatcher;
using System.Web;

namespace Org.Service.BasicSetting
{
    /// <summary>
    /// 服务异常处理
    /// </summary>
    public class ErrorHandlingInvoker : IOperationInvoker
    {
        private IOperationInvoker invoker;
        private string operationName;

        public ErrorHandlingInvoker(IOperationInvoker invoker, DispatchOperation operation)
        {
            this.invoker = invoker;
            this.operationName = operation.Name;
        }

        public virtual object[] AllocateInputs()
        {
            return invoker.AllocateInputs();
        }

        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            object returnedValue = null;
            object[] outputParams = new object[] { };

            try
            {

                returnedValue = invoker.Invoke(instance, inputs, out outputParams);
                outputs = outputParams;
                return returnedValue;
            }
            catch (PropValiException pv)
            {
                TransResponse response = this.CreateResponse(out outputParams);

                if (response != null)
                {
                    response.Status = TransStatus.ValidateError;
                    response.Message = pv.Message;
                    returnedValue = response;
                }
                else
                {
                    returnedValue = null;
                }
            }
            catch (Exception ex)
            {
                TransResponse response = this.CreateResponse(out outputParams);

                if (response != null)
                {
                    response.Status = TransStatus.Error;
                    response.Message = ex.Message;
                    returnedValue = response;
                }
                else
                {
                    returnedValue = null;
                }
            }

            outputs = outputParams;
            return returnedValue;
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            return invoker.InvokeBegin(instance, inputs, callback, state);
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            object returnedValue = null;
            object[] outputParams = { };

            try
            {
                returnedValue = invoker.InvokeEnd(instance, out outputs, result);
                outputs = outputParams;
                return returnedValue;
            }
            catch (PropValiException pv)
            {
                TransResponse response = this.CreateResponse(out outputParams);

                if (response != null)
                {
                    response.Status = TransStatus.ValidateError;
                    response.Message = pv.Message;
                    returnedValue = response;
                }
                else
                {
                    returnedValue = null;
                }
            }
            catch (Exception ex)
            {
                TransResponse response = this.CreateResponse(out outputParams);

                if (response != null)
                {
                    response.Status = TransStatus.Error;
                    response.Message = ex.Message;
                    returnedValue = response;
                }
                else
                {
                    returnedValue = null;
                }
            }

            outputs = outputParams;
            return returnedValue;
        }

        public bool IsSynchronous
        {
            get
            {
                return invoker.IsSynchronous;
            }
        }

        private TransResponse CreateResponse(out object[] outputParams)
        {
            List<object> outParams = new List<object>();

            PropertyInfo prop = invoker.GetType().GetProperty("Method");

            if (prop != null)
            {
                MethodInfo operationMethod = prop.GetValue(invoker) as MethodInfo;

                if (operationMethod != null)
                {
                    ParameterInfo[] ps = operationMethod.GetParameters();

                    foreach (var pi in ps)
                    {
                        if (pi.IsOut)
                        {
                            var piType = pi.ParameterType.GetElementType();

                            if (piType.IsValueType)
                            {
                                if (piType == typeof(bool))
                                {
                                    outParams.Add(false);
                                }
                                else if (piType == typeof(DateTime))
                                {
                                    outParams.Add(DateTime.MinValue);
                                }
                                else
                                {
                                    outParams.Add(0);
                                }
                            }
                            else
                            {
                                outParams.Add(null);
                            }
                        }
                    }

                    Type trType = typeof(TransResponse<>);
                    trType = trType.MakeGenericType(operationMethod.ReturnType.GenericTypeArguments);
                    TransResponse response = Activator.CreateInstance(trType) as TransResponse;
                    outputParams = outParams.ToArray();
                    return response;
                }
            }

            outputParams = outParams.ToArray();
            return null;
        }
    }
}