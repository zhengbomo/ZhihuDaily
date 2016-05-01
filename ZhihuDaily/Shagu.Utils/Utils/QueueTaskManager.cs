using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Shagu.Utils.Utils
{
    public class QueueTaskManager
    {

        private readonly Queue<object> _actionQueue;
        private static readonly object LockObj = new object();

        private readonly Semaphore _semaphore;

        public QueueTaskManager()
        {
            _actionQueue = new Queue<object>();
            _semaphore = new Semaphore(0, int.MaxValue);
            Task.Run(() => Run());
        }

        public bool IsEmpty()
        {
            lock (LockObj)
            {
                return _actionQueue.Count == 0;
            }
        }

        public int ActionCount()
        {
            lock (LockObj)
            {
                return _actionQueue.Count;
            }
        }

        private async void Run()
        {
            while (true)
            {
                var task = Dequeue();
                var action = task as Action;
                if (action != null)
                {
                    action();
                    continue;

                }

                var func = task as Func<Task>;
                if (func != null)
                {
                    await func();
                    continue;
                }

            }
        }

        /// <summary>
        /// 阻塞当前线程，直到acion完成
        /// </summary>
        public void EnqueueSync(Action action)
        {
            AutoResetEvent resetEvent = new AutoResetEvent(false);

            lock (LockObj)
            {
                Action act = () =>
                {
                    action();
                    //任务执行完成，释放信号量，让阻塞进程运行
                    resetEvent.Set();
                };
                _actionQueue.Enqueue(act);
            }
            _semaphore.Release();

            //等待任务完成
            resetEvent.WaitOne();
        }

        /// <summary>
        /// 阻塞当前线程，知道func完成
        /// </summary>
        public void EnqueueSync(Func<Task> func)
        {
            AutoResetEvent resetEvent = new AutoResetEvent(false);

            lock (LockObj)
            {
                Func<Task> fun = async () =>
                {
                    await func();
                    //任务执行完成，释放信号量，让阻塞进程运行
                    resetEvent.Set();
                };
                _actionQueue.Enqueue(fun);
            }
            _semaphore.Release();

            //等待任务完成
            resetEvent.WaitOne();
        }

        /// <summary>
        /// action入队列，不等待任务执行完成
        /// </summary>
        public void EnqueueAsync(Action action)
        {
            lock (LockObj)
            {
                _actionQueue.Enqueue(action);
            }
            _semaphore.Release();
        }

        /// <summary>
        /// func入队列，不等待任务执行完成
        /// </summary>
        public void EnqueueAsync(Func<Task> func)
        {
            lock (LockObj)
            {
                _actionQueue.Enqueue(func);
            }
            _semaphore.Release();

        }

        private object Dequeue()
        {
            _semaphore.WaitOne();
            lock (LockObj)
            {
                return _actionQueue.Dequeue();
            }
        }

    }
}
