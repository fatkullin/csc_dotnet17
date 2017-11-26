using System.Threading;

namespace BlockingArrayQueue
{
    public class LockFreeQueue<T> : IQueue<T>
    {
        public LockFreeQueue(uint maxItems = uint.MaxValue)
        {
            _maxItems = maxItems;
        }

        public void Enqueue(T value)
        {
            while (!TryEnqueue(value))
            {
            }
        }

        public T Dequeue()
        {
            T result;
            while (!TryDequeue(out result))
            {
            }
            return result;
        }

        public bool TryEnqueue(T value)
        {
            bool enqueued;
            do
            {
                uint count = 0;
                var prev = _head;
                var current = prev.Next;

                while (current != null && count < _maxItems)
                {
                    prev = current;
                    current = prev.Next;
                    count += 1;
                }

                if (count == _maxItems)
                {
                    return false;
                }

                var newNode = new Node<T> {Value = value};
                enqueued = Interlocked.CompareExchange(ref prev.Next, newNode, current) == current;
            } while (!enqueued);
            return true;
        }

        public bool TryDequeue(out T value)
        {
            bool dequeued;
            Node<T> curr;
            do
            {
                curr = _head.Next;

                if (curr == null)
                {
                    value = default(T);
                    return false;
                }

                dequeued = Interlocked.CompareExchange(ref _head.Next, curr.Next, curr) == curr;
            } while (!dequeued);

            value = curr.Value;

            return true;
        }

        public void Clear()
        {
            _head.Next = null;
        }

        private readonly uint _maxItems;
        private readonly Node<T> _head = new Node<T>();
    }
}
