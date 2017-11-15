using System;
using System.Collections.Generic;

namespace PrimaryNumbers
{
    public interface ITaskModel
    {
        void AddTask(ulong value);
        event EventHandler NewTaskAdded;
        IEnumerable<IPrimaryTask> Tasks { get; }
    }
}