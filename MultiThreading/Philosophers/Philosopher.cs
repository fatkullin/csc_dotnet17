using System;
using System.Threading;

namespace Philosophers
{
    public class Philosopher
    {
        public bool Hungry { get; private set; }

        public Philosopher(Fork leftFork, Fork rightFork)
        {
            if (leftFork.Index == rightFork.Index)
                throw new ArgumentException("The same fork index for each hand.");
            
            // get the right order
            if (leftFork.Index > rightFork.Index)
            {
                var tmp = leftFork;
                leftFork = rightFork;
                rightFork = tmp;
            }
            _leftFork = leftFork;
            _rightFork = rightFork;

            Hungry = true;
        }

        public void Eat(int timeMs)
        {
            lock (_leftFork)
            lock (_rightFork)
            {
                Thread.Sleep(timeMs);
            }
            Hungry = false;
        }

        private readonly Fork _leftFork;
        private readonly Fork _rightFork;
    }

    public class Fork
    {
        public Fork(int index)
        {
            Index = index;
        }
        public int Index { get; }
    }
}
