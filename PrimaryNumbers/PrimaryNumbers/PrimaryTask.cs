using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrimaryNumbers
{
    internal class PrimaryTask : IPrimaryTask
    {
        public TaskState State { get; private set; }
        public ulong Value { get; }
        public ulong Result { get; private set; }
        public double Progress { get; private set; }

        public PrimaryTask(ulong value)
        {
            Value = value;
            State = TaskState.Waiting;
            Execute();
        }

        public void Cancel()
        {
            _cancellationSource.Cancel();
        }

        private async void Execute()
        {
            var task = Task.Run(() => PrimaryCount(), _cancellationSource.Token);

            try
            {
                Result = await task;
                State = TaskState.Completed;
            }
            catch (OperationCanceledException)
            {
                State = TaskState.Cancelled;
            }
        }

        private ulong PrimaryCount()
        {
            var token = _cancellationSource.Token;

            State = TaskState.Running;

            ulong primaryCount = 0;
            for (ulong value = 1; value <= Value; value++)
            {
                bool isPrimary = true;
                for (ulong i = 2, sq = (ulong)Math.Sqrt(value); i <= sq; i++)
                {
                    if (value % i == 0)
                    {
                        isPrimary = false;
                        break;
                    }
                }

                if (isPrimary)
                {
                    primaryCount += 1;
                }

                if (value % (1024 * 8) == 0)
                {
                    token.ThrowIfCancellationRequested();
                    Progress = (double)value / Value;
                }
            }

            Progress = 1;
            return primaryCount;
        }

        private readonly CancellationTokenSource _cancellationSource = new CancellationTokenSource();
    }
}