using System.Threading;
using BlockingArrayQueue;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlockingArrayQueueTests
{
    [TestClass]
    public class LockBasedQueueTests
    {
        [TestMethod]
        public void EnqueueDequeueTest()
        {
            var queue = new LockBasedQueue(1);
            queue.Enqueue(1);

            var t = new Thread(() => queue.Enqueue(2));
            t.Start();

            Assert.AreEqual(1, queue.Dequeue());
            Assert.AreEqual(2, queue.Dequeue());
        }

        [TestMethod]
        public void TryEnqueueTest()
        {
            var queue = new LockBasedQueue(1);
            queue.Enqueue(1);

            Assert.IsFalse(queue.TryEnqueue(2));
        }

        [TestMethod]
        public void TryDequeueTest()
        {
            var queue = new LockBasedQueue();
            Assert.IsFalse(queue.TryDequeue(out _));
        }

        [TestMethod]
        public void ClearTest()
        {
            var queue = new LockBasedQueue();
            queue.Enqueue(1);
            queue.Enqueue(2);

            queue.Clear();
            Assert.IsFalse(queue.TryDequeue(out _));
        }
    }
}