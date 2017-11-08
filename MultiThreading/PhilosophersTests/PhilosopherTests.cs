using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Philosophers;

namespace PhilosophersTests
{
    [TestClass]
    public class PhilosopherTests
    {
        [TestMethod]
        public void PhilosopherTest()
        {
            const int count = 5;
            var philosofers = new Philosopher[count];
            var forks = new Fork[count];
            var locker = new object();
            var thread = new Thread[count];

            for (var i = 0; i < count; i++)
            {
                forks[i] = new Fork();
            }

            for (var i = 0; i < count; i++)
            {
                philosofers[i] = new Philosopher(forks[i], forks[(i + 1) % count], locker);
            }

            for (var i = 0; i < count; i++)
            {
                var index = i;
                thread[i] = new Thread(() =>
                {
                    var rnd = new Random();
                    Thread.Sleep(rnd.Next() % 100);
                    philosofers[index].Eat(rnd.Next() % 100);
                });
                thread[i].Start();
            }

            foreach (var th in thread)
            {
                th.Join();
            }
        }
    }
}