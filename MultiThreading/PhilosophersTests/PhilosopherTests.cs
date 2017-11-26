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
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructPhilisopherWithTheSameForkTest()
        {
            var leftFork = new Fork(0);
            var rightFork = new Fork(0);
            // ReSharper disable once UnusedVariable
            var philosopher = new Philosopher(leftFork, rightFork);
        }

        [TestMethod]
        public void PhilosopherTest()
        {
            const int count = 5;
            var philosofers = new Philosopher[count];
            var forks = new Fork[count];
            var thread = new Thread[count];

            for (var i = 0; i < count; i++)
            {
                forks[i] = new Fork(i);
            }

            for (var i = 0; i < count; i++)
            {
                philosofers[i] = new Philosopher(forks[i], forks[(i + 1) % count]);
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

            for (var i = 0; i < count; i++)
            {
                Assert.IsFalse(philosofers[i].Hungry);
            }
        }
    }
}