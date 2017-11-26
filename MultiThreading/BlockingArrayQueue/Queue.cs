namespace BlockingArrayQueue
{
    public class Queue<T>
    {
        public int Count => _size;

        public T Dequeue()
        {
            _size -= 1;

            var oldHead = _head;
            _head = _head.Next;
            return oldHead.Value;
        }

        public void Enqueue(T value)
        {
            _size += 1;

            if (_head == null)
            {
                _head = new Node<T> {Value = value};
                _tail = _head;
                return;
            }

            _tail.Next = new Node<T> {Value = value};
            _tail = _tail.Next;
        }

        public void Clear()
        {
            _head.Next = null;
            _tail = null;
            _size = 0;
        }

        private Node<T> _head;
        private Node<T> _tail;
        private int _size;
    }
}