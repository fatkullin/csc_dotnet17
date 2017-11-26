namespace BlockingArrayQueue
{
    public interface IQueue<T>
    {
        void Enqueue(T value);
        T Dequeue();
        bool TryEnqueue(T value);
        bool TryDequeue(out T value);
        void Clear();
    }
}