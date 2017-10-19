using System.IO;

namespace MiniRoguelike
{
    internal class MapLoader
    {
        public MapLoader(string filepath)
        {
            _filePath = filepath;
        }

        public Map Load()
        {
            var result = new Map();

            var y = 0;
            var x = 0;
            var body = File.ReadAllText(_filePath);
            foreach (var c in body)
            {
                if (SkippedSymbol(c))
                {
                    continue;
                }

                if (c == '\n')
                {
                    y += 1;
                    x = 0;
                    continue;
                }

                result.Value[x, y] = c;
                x += 1;
            }
            return result;
        }

        private static bool SkippedSymbol(char c)
        {
            return c == '\r';
        }

        private readonly string _filePath;
    }
}