namespace MiniRoguelike
{
    internal class Map
    {
        public const int Width = 60;
        public const int Height = 20;

        public char[,] Value { get; }

        public Map()
        {
            Value = new char[Width, Height];
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    Value[x, y] = ' ';
                }
            }
        }

    }
}