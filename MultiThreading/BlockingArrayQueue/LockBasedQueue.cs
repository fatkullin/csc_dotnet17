using System.Collections;
using System.Threading;

namespace BlockingArrayQueue
{
    public class LockBasedQueue : IQueue
    {
        public LockBasedQueue(uint maxItems = uint.MaxValue)
        {
            _maxItems = maxItems;
        }

        public void Enqueue(object value)
        {
            lock (_locker)
            {
                while (_queue.Count >= _maxItems)
                {
                    Monitor.Wait(_locker);
                }

                _queue.Enqueue(value);
                Monitor.PulseAll(_locker);
            }
        }

        public object Dequeue()
        {
            lock (_locker)
            {
                while (_queue.Count == 0)
                {
                    Monitor.Wait(_locker);
                }

                var result = _queue.Dequeue();
                Monitor.PulseAll(_locker);
                return result;
            }
        }

        public bool TryEnqueue(object value)
        {
            if (!Monitor.TryEnter(_locker))
            {
                return false;
            }

            try
            {
                if (_queue.Count >= _maxItems)
                {
                    return false;
                }
                _queue.Enqueue(value);
                Monitor.PulseAll(_locker);
                return true;
            }
            finally
            {
                Monitor.Exit(_locker);
            }
        }

        public bool TryDequeue(out object value)
        {
            if (!Monitor.TryEnter(_locker))
            {
                value = null;
                return false;
            }

            try
            {
                if (_queue.Count == 0)
                {
                    value = null;
                    return false;
                }
                value = _queue.Dequeue();
                Monitor.PulseAll(_locker);
                return true;
            }
            finally
            {
                Monitor.Exit(_locker);
            }
        }

        public void Clear()
        {
            lock (_locker)
            {
                _queue.Clear();
                Monitor.PulseAll(_locker);
            }
        }

        private readonly object _locker = new object();

        private readonly uint _maxItems;
        private readonly Queue _queue = new Queue();
    }
}
