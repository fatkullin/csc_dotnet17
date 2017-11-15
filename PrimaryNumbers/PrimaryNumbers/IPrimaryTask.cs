namespace PrimaryNumbers
{
    public interface IPrimaryTask
    {
        TaskState State { get; }
        ulong Value { get; }
        ulong Result { get; }
        double Progress { get; }
        void Cancel();
    }
}