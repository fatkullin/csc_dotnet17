using System.Collections.Generic;

namespace MiniRoguelike
{
    internal class Mover
    {
        public Mover(RogueMap map)
        {
            _map = map;
        }

        public bool TryMove(Arrow arrow)
        {
            var nextCoord = MoveToArrow(_map.Rogue, arrow);
            if (EndOfScreen(nextCoord))
            {
                return false;
            }

            if (IsBarrier(_map, nextCoord))
            {
                return false;
            }

            _map.ReplaceRogue(nextCoord);
            return true;
        }

        private static Coordinate MoveToArrow(Coordinate value, Arrow arrow)
        {
            return new Coordinate(value.X + XDiff[arrow], value.Y + YDiff[arrow]);
        }

        private static bool EndOfScreen(Coordinate value)
        {
            return value.X < 0
                   || value.Y < 0
                   || value.X >= Map.Width
                   || value.Y >= Map.Height;
        }

        private static bool IsBarrier(RogueMap map, Coordinate nextCoord)
        {
            return map.GetValue(nextCoord) != ' ';
        }

        private static readonly Dictionary<Arrow, int> XDiff =
            new Dictionary<Arrow, int> { { Arrow.Up, 0 }, { Arrow.Down, 0 }, { Arrow.Left, -1 }, { Arrow.Right, 1 } };

        private static readonly Dictionary<Arrow, int> YDiff =
            new Dictionary<Arrow, int> { { Arrow.Up, -1 }, { Arrow.Down, 1 }, { Arrow.Left, 0 }, { Arrow.Right, 0 } };

        private readonly RogueMap _map;
    }
}