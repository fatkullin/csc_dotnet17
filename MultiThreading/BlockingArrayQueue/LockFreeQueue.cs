using System.Threading;

namespace BlockingArrayQueue
{
    internal class Node
    {
        public Node Next;
        public object Value;
    }

    public class LockFreeQueue : IQueue
    {
        public LockFreeQueue(uint maxItems = uint.MaxValue)
        {
            _maxItems = maxItems;
        }

        public void Enqueue(object value)
        {
            while (!TryEnqueue(value))
            {
            }
        }

        public object Dequeue()
        {
            object result;
            while (!TryDequeue(out result))
            {
            }
            return result;
        }

        public bool TryEnqueue(object value)
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

            var newNode = new Node { Value = value };
            return Interlocked.CompareExchange(ref prev.Next, newNode, current) == current;
        }

        public bool TryDequeue(out object value)
        {
            var curr = _head.Next;

            if (curr == null)
            {
                value = null;
                return false;
            }

            if (Interlocked.CompareExchange(ref _head.Next, curr.Next, curr) != curr)
            {
                value = null;
                return false;
            }

            value = curr.Value;
            return true;
        }

        public void Clear()
        {
            while (true)
            {
                var curr = _head.Next;

                if (curr == null)
                {
                    return;
                }

                if (Interlocked.CompareExchange(ref _head.Next, null, curr) == curr)
                {
                    return;
                }
            }
        }

        private readonly uint _maxItems;
        private readonly Node _head = new Node();
    }
}
