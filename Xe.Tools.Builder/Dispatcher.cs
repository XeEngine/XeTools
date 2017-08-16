using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Builder
{
    internal class Dispatcher<T>
    {
        private Stopwatch _stopwatch;

        private List<T> _items;

        public long ElapsedMilliseconds => _stopwatch.ElapsedMilliseconds;
        
        public void Process(Func<T, Task> processFunc)
        {
            _stopwatch.Start();
            int maxTasksCount = System.Environment.ProcessorCount * 2;
            var queue = new List<Task>(maxTasksCount);
            while (_items.Count > 0)
            {
                if (queue.Count >= maxTasksCount)
                {
                    if (queue.Count >= maxTasksCount)
                    {
                        Task.WaitAny(queue.ToArray());
                        foreach (var taskItem in queue)
                        {
                            if (taskItem.IsCompleted |
                                taskItem.IsFaulted ||
                                taskItem.IsCanceled)
                            {
                                queue.Remove(taskItem);
                                break;
                            }
                        }
                    }
                }

                Task task;
                lock (queue)
                {
                    task = processFunc.Invoke(_items[0]);
                    queue.Add(task);
                    _items.RemoveAt(0);
                }
                task.Start();
            }
            Task.WaitAll(queue.ToArray());
            _stopwatch.Stop();
        }

        public Dispatcher(List<T> items)
        {
            _stopwatch = new Stopwatch();
            _items = items;
        }
    }
}
