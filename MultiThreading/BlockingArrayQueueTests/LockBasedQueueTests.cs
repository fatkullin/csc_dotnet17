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
            var queue = new LockBasedQueue<int>(1);
            queue.Enqueue(1);

            var t = new Thread(() => queue.Enqueue(2));
            t.Start();

            Assert.AreEqual(1, queue.Dequeue());
            Assert.AreEqual(2, queue.Dequeue());
        }

        [TestMethod]
        public void WaitForEnqueueTest()
        {
            var queue1 = new LockBasedQueue<int>(1);
            var queue2 = new LockBasedQueue<int>(1);

            var t = new Thread(() =>
            {
                var val = queue1.Dequeue();
                queue2.Enqueue(val);
                queue2.Enqueue(val * 2);
            });
            t.Start();

            queue1.Enqueue(1);

            Assert.AreEqual(1, queue2.Dequeue());
            Assert.AreEqual(2, queue2.Dequeue());
        }

        [TestMethod]
        public void TryEnqueueToEmptyQueueTest()
        {
            var queue = new LockBasedQueue<int>(1);
            Assert.IsTrue(queue.TryEnqueue(2));
        }

        [TestMethod]
        public void TryEnqueueToFullQueueTest()
        {
            var queue = new LockBasedQueue<int>(1);
            queue.Enqueue(1);

            Assert.IsFalse(queue.TryEnqueue(2));
        }

        [TestMethod]
        public void TryDequeueFromEmptyQueueTest()
        {
            var queue = new LockBasedQueue<int>();
            Assert.IsFalse(queue.TryDequeue(out _));
        }

        [TestMethod]
        public void TryDequeueFromNonEmptyQueueTest()
        {
            var queue = new LockBasedQueue<int>();
            queue.Enqueue(1);
            Assert.IsTrue(queue.TryDequeue(out var value));
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void ClearTest()
        {
            var queue = new LockBasedQueue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);

            queue.Clear();
            Assert.IsFalse(queue.TryDequeue(out _));
        }
    }
}