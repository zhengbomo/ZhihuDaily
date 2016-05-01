using System;
using System.Threading.Tasks;
using ZhihuDaily.Domain.Models;
using Shagu.Utils.Utils;
using ZhihuDaily.Domain.Service;

namespace ZhihuDaily.Util
{
    public class RequestUtil:RequestUtilBase
    {
        public static bool DoResult<T>(RequestResult<T> result, Action<T> action)
        {
            switch (result.ResultType)
            {
                case RequestResultType.Success:
                    var apiResult = result.Content as ApiResult;
                    if (apiResult != null)
                    {
                        DataService.Span = apiResult.Now - DateTime.Now;
                    }

                    action?.Invoke(result.Content);
                    return true;
                case RequestResultType.Error:
#if DEBUG
                    ToastHelper.ToastError(string.Format("错误2：{0}", result.ErrorMessage));
#else
                ToastHelper.ToastError(result.ErrorMessage);
#endif
                    break;
                case RequestResultType.Cancel:
                    break;
            }
            return false;

        }

        public static void DoResult<T>(RequestResult<T> result, Action<T> success, Action<string> error)
        {
            if (result.IsSuccess)
            {
                success?.Invoke(result.Content);
            }
            else
            {
                error?.Invoke(result.ErrorMessage);
            }
        }

        public static async Task DoResult<T>(RequestResult<T> result, Func<T, Task> success, Action<string> error)
        {
            if (result.IsSuccess)
            {
                if (success != null)
                {
                    await success(result.Content);
                }
            }
            else
            {
                error?.Invoke(result.ErrorMessage);
            }
        }
    }
}
