using System.Threading;
using BlockingArrayQueue;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlockingArrayQueueTests
{
    [TestClass]
    public class LockFreeQueueTests
    {
        [TestMethod]
        public void EnqueueDequeueTest()
        {
            var queue = new LockFreeQueue<int>(1);
            queue.Enqueue(1);

            var t = new Thread(() => queue.Enqueue(2));
            t.Start();

            Assert.AreEqual(1, queue.Dequeue());
            Assert.AreEqual(2, queue.Dequeue());
        }

        [TestMethod]
        public void TryEnqueueTest()
        {
            var queue = new LockFreeQueue<int>(1);
            queue.Enqueue(1);

            Assert.IsFalse(queue.TryEnqueue(2));
        }

        [TestMethod]
        public void TryDequeueTest()
        {
            var queue = new LockFreeQueue<int>();
            Assert.IsFalse(queue.TryDequeue(out _));
        }

        [TestMethod]
        public void ClearTest()
        {
            var queue = new LockFreeQueue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);

            queue.Clear();
            Assert.IsFalse(queue.TryDequeue(out _));
        }
    }
}