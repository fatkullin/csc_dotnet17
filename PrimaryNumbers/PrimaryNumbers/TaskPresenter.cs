using System;

namespace PrimaryNumbers
{
    public class TaskPresenter
    {
        public TaskPresenter(IPrimaryTask task)
        {
            _primaryTask = task;
        }

        public void Cancel()
        {
            _primaryTask.Cancel();
        }

        public override string ToString()
        {
            var state = _primaryTask.State;

            var result = "Task: " + _primaryTask.Value + " "
                         + state;

            switch (state)
            {
                case TaskState.Waiting:
                case TaskState.Cancelled:
                    return result;
                case TaskState.Completed:
                    return result + " Result: " + _primaryTask.Result;
                case TaskState.Running:
                    return result + " "
                           + Math.Round(_primaryTask.Progress * 100, 2) + "%";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private readonly IPrimaryTask _primaryTask;
    }
}