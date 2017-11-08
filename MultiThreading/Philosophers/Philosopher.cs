using System.Threading;

namespace Philosophers
{
    public class Philosopher
    {
        public Philosopher(Fork leftFork, Fork rightFork, object locker)
        {
            _leftFork = leftFork;
            _rightFork = rightFork;
            _locker = locker;
        }

        public void Eat(int timeMs)
        {
            lock (_locker)
            {
                lock (_leftFork)
                lock (_rightFork)
                {
                    Thread.Sleep(timeMs);
                }
            }
        }

        private readonly object _locker;
        private readonly Fork _leftFork;
        private readonly Fork _rightFork;
    }

    public class Fork
    {

    }
}
