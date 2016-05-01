using System;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Shagu.Utils
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Run target async operation synchronously.
        /// </summary>
        public static T GetResultSync<T>(this IAsyncOperation<T> operation)
        {
            return operation.AsTask().GetResultSync();
        }

        /// <summary>
        /// Run target async action synchronously.
        /// </summary>
        public static void GetResultSync(this IAsyncAction action)
        {
            action.AsTask().GetResultSync();
        }

        /// <summary>
        /// Run target async operation synchronously.
        /// </summary>
        public static T GetResultSync<T>(this Task<T> task)
        {
            return task.ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Run target async action synchronously.
        /// </summary>
        public static void GetResultSync(this Task task)
        {
            task.ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
