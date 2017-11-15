using System;
using System.Collections.Generic;

namespace PrimaryNumbers
{
    public sealed class TaskModel : ITaskModel
    {
        public event EventHandler NewTaskAdded;
        public IEnumerable<IPrimaryTask> Tasks => _tasks;

        public void AddTask(ulong value)
        {
            var task = new PrimaryTask(value);
            _tasks.Add(task);
            OnNewTaskAdded(task);
        }

        private void OnNewTaskAdded(IPrimaryTask task)
        {
            NewTaskAdded?.Invoke(this, EventArgs.Empty);
        }

        private readonly List<PrimaryTask> _tasks = new List<PrimaryTask>();
    }
}