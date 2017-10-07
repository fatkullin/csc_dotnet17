namespace Trie
{
    using System.Collections.Generic;

    internal class Trie
    {
        private Vertex root = new Vertex();

        // Expected complexity: O(|element|)
        // Returns true if this set did not already contain the specified element
        public bool Add(string element)
        {
            var pointer = this.root;
            var visitedPointers = new List<Vertex>();
            visitedPointers.Add(pointer);

            for (var index = 0; index < element.Length; ++index)
            {
                if (!pointer.Next.ContainsKey(element[index]))
                {
                    pointer.Next[element[index]] = new Vertex();
                }

                pointer = pointer.Next[element[index]];
                visitedPointers.Add(pointer);
            }

            var contains = pointer.IsTerminal;
            if (contains)
            {
                return false;
            }
            else
            {
                pointer.IsTerminal = true;
                foreach (var item in visitedPointers)
                {
                    item.Size++;
                }

                return true;
            }
        }

        // Expected complexity: O(|element|)
        public bool Contains(string element)
        {
            var pointer = this.root;

            for (var index = 0; index < element.Length; ++index)
            {
                if (!pointer.Next.ContainsKey(element[index]))
                {
                    return false;
                }

                pointer = pointer.Next[element[index]];
            }

            var contains = pointer.IsTerminal;
            return contains;
        }

        // Expected complexity: O(|element|)
        // Returns true if this set contained the specified element
        public bool Remove(string element)
        {
            var pointer = this.root;
            var visitedPointers = new List<Vertex>
            {
                pointer
            };

            for (var index = 0; index < element.Length; ++index)
            {
                if (!pointer.Next.ContainsKey(element[index]))
                {
                    return false;
                }

                pointer = pointer.Next[element[index]];
                visitedPointers.Add(pointer);
            }

            var contains = pointer.IsTerminal;
            if (contains)
            {
                foreach (var item in visitedPointers)
                {
                    item.Size--;
                }

                pointer.IsTerminal = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        // Expected complexity: O(1)
        public int Size()
        {
            return this.root.Size;
        }

        // Expected complexity: O(|prefix|)
        public int HowManyStartsWithPrefix(string prefix)
        {
            var pointer = this.root;

            for (var index = 0; index < prefix.Length; ++index)
            {
                if (!pointer.Next.ContainsKey(prefix[index]))
                {
                    return 0;
                }

                pointer = pointer.Next[prefix[index]];
            }

            return pointer.Size;
        }

        private class Vertex
        {
            private Dictionary<char, Vertex> next = new Dictionary<char, Vertex>();
            private bool isTerminal = false;
            private int size = 0;

            public Dictionary<char, Vertex> Next { get => this.next; set => this.next = value; }

            public bool IsTerminal { get => this.isTerminal; set => this.isTerminal = value; }

            public int Size { get => this.size; set => this.size = value; }
        }
    }
}
