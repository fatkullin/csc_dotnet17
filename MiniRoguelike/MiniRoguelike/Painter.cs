using System;

namespace MiniRoguelike
{
    internal class Painter
    {
        public Painter(RogueMap map, string helpMessage)
        {
            _map = map;
            _helpMessage = helpMessage;
            Console.CursorVisible = false;
        }

        public void Paint()
        {
            Console.SetCursorPosition(0, 0);
            for (var y = 0; y < Map.Height; ++y)
            {
                for (var x = 0; x < Map.Width; ++x)
                {
                    var val = _map.GetValue(new Coordinate(x, y));
                    Console.Write(val);
                }
                Console.WriteLine();
            }
            Console.WriteLine(_helpMessage);
        }

        private readonly RogueMap _map;
        private readonly string _helpMessage;
    }
}