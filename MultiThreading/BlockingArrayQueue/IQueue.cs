namespace BlockingArrayQueue
{
    public interface IQueue
    {
        void Enqueue(object value);
        object Dequeue();
        bool TryEnqueue(object value);
        bool TryDequeue(out object value);
        void Clear();
    }
}