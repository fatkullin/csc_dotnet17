namespace Trie
{
    using System;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var trie = new Trie();
            Test(trie.Add("a"), true);
            Test(trie.Add("ab"), true);
            Test(trie.Add("abc"), true);
            Test(trie.Add("abcd"), true);
            Test(trie.Add("abcd"), false);

            Test(trie.Contains("a"), true);
            Test(trie.Contains("ab"), true);
            Test(trie.Contains("abc"), true);
            Test(trie.Contains("abcd"), true);
            Test(trie.Contains("b"), false);
            Test(trie.Contains(string.Empty), false);

            Test(trie.Size(), 4);
            Test(trie.HowManyStartsWithPrefix("a"), 4);
            Test(trie.HowManyStartsWithPrefix("ab"), 3);
            Test(trie.HowManyStartsWithPrefix("abc"), 2);
            Test(trie.HowManyStartsWithPrefix("abcd"), 1);
            Test(trie.HowManyStartsWithPrefix("abcde"), 0);
            Test(trie.HowManyStartsWithPrefix("b"), 0);

            Test(trie.Remove("b"), false);
            Test(trie.Remove("a"), true);

            Test(trie.Size(), 3);
            Test(trie.HowManyStartsWithPrefix("a"), 3);
            Test(trie.HowManyStartsWithPrefix("ab"), 3);
            Test(trie.HowManyStartsWithPrefix("abc"), 2);
            Test(trie.HowManyStartsWithPrefix("abcd"), 1);
            Test(trie.HowManyStartsWithPrefix("abcde"), 0);

            Test(trie.Remove("ab"), true);
            Test(trie.Remove("abc"), true);
            Test(trie.Remove("abcd"), true);
            Test(trie.Remove("abcd"), false);

            Test(trie.Contains("a"), false);
            Test(trie.Contains("ab"), false);
            Test(trie.Contains("abc"), false);
            Test(trie.Contains("abcd"), false);

            Test(trie.Add("dcba"), true);
            Test(trie.Add("dcb"), true);

            Test(trie.Contains("dcba"), true);
            Test(trie.Contains("dcb"), true);
            Test(trie.Contains("dc"), false);
            Test(trie.Contains("d"), false);

            Test(trie.Size(), 2);
            Console.WriteLine("All tests successfully completed.");
        }

        private static void Test<T>(T expected, T received)
            where T : IComparable
        {
            if (expected.CompareTo(received) != 0)
            {
                Console.WriteLine("not equal valued: expected: {0}, received: {1}", expected, received);
                Environment.Exit(-1);
            }
        }
    }
}
