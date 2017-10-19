namespace MiniRoguelike
{
    internal class RogueMap
    {
        public Coordinate Rogue { get; private set; }

        public RogueMap(Map map)
        {
            _map = map;

            for (var x = 0; x < Map.Width; x++)
            {
                for (var y = 0; y < Map.Height; y++)
                {
                    if (_map.Value[x, y] == '@')
                    {
                        Rogue = new Coordinate(x, y);
                    }
                }
            }
        }

        public void ReplaceRogue(Coordinate position)
        {
            _map.Value[Rogue.X, Rogue.Y] = ' ';
            _map.Value[position.X, position.Y] = '@';
            Rogue = position;
        }

        public char GetValue(Coordinate coordinate)
        {
            return _map.Value[coordinate.X, coordinate.Y];
        }

        private readonly Map _map;
    }
}