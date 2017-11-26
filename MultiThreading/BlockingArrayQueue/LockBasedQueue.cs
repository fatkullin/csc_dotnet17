using System.Threading;

namespace BlockingArrayQueue
{
    public class LockBasedQueue<T> : IQueue<T>
    {
        public LockBasedQueue(uint maxItems = uint.MaxValue)
        {
            _maxItems = maxItems;
        }

        public void Enqueue(T value)
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

        public T Dequeue()
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

        public bool TryEnqueue(T value)
        {
            lock (_locker)
            {
                if (_queue.Count >= _maxItems)
                {
                    return false;
                }
                _queue.Enqueue(value);
                Monitor.PulseAll(_locker);
                return true;
            }
        }

        public bool TryDequeue(out T value)
        {
            lock (_locker)
            {
                if (_queue.Count == 0)
                {
                    value = default(T);
                    return false;
                }
                value = _queue.Dequeue();
                Monitor.PulseAll(_locker);
                return true;
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
        private readonly Queue<T> _queue = new Queue<T>();
    }
}
