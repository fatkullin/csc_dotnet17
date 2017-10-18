using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MiniRoguelike
{
    struct Coordinate
    {
        public int X;
        public int Y;
    }
    class Map
    {
        public const int Width = 60;
        public const int Height = 20;

        private char[,] value = new char[Width, Height];
        public Coordinate rogue;

        public Map()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    value[x, y] = ' ';
                }
            }
        }

        public int MaxX { get; private set; }

        public void ReplaceRogue(Coordinate position)
        {
            value[rogue.X, rogue.Y] = ' ';
            value[position.X, position.Y] = '@';
            rogue = position;
        }

        public char GetValue(Coordinate coordinate)
        {
            return value[coordinate.X, coordinate.Y];
        }

        public static Map LoadFrom(string filename)
        {
            var result = new Map();

            int y = 0;
            int x = 0;
            var body = File.ReadAllText(filename);
            foreach (var c in body)
            {
                if (c == '\r')
                {
                    x += 1;
                    continue;
                }

                if (c == '\n')
                {
                    y += 1;
                    x = 0;
                    continue;
                }

                if (c == '@')
                {
                    result.rogue.X = x;
                    result.rogue.Y = y;
                }

                result.value[x, y] = c;
                x += 1;
            }
            return result;
        }
    }

    enum Arrow
    {
        Up,
        Down,
        Left,
        Right
    }

    class Mover
    {
        private Map _map;

        public Mover(Map map)
        {
            _map = map;
        }


        private static readonly Dictionary<Arrow, int> XDiff = 
            new Dictionary<Arrow, int> {{Arrow.Up, 0}, { Arrow.Down, 0 }, { Arrow.Left, -1 }, { Arrow.Right, 1 } };

        private static readonly Dictionary<Arrow, int> YDiff =
            new Dictionary<Arrow, int> { { Arrow.Up, -1 }, { Arrow.Down, 1 }, { Arrow.Left, 0 }, { Arrow.Right, 0 } };


        Coordinate MoveToArrow(Coordinate value, Arrow arrow)
        {
            return new Coordinate
            {
                X = value.X + XDiff[arrow],
                Y = value.Y + YDiff[arrow]
            };
        }

        bool EndOfScreen(Coordinate value)
        {
            return value.X < 0
                   || value.Y < 0
                   || value.X >= Map.Width
                   || value.Y >= Map.Height;
        }

        public bool TryMove(Arrow arrow)
        {
            var nextCoord = MoveToArrow(_map.rogue, arrow);
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

        private bool IsBarrier(Map map, Coordinate nextCoord)
        {
            return map.GetValue(nextCoord) != ' ';
        }
    }

    class Painter
    {
        private Map _map;
        private string _helpMessage;

        public Painter(Map map, string helpMessage)
        {
            _map = map;
            _helpMessage = helpMessage;
        }

        protected static void WriteAt(string s, int x, int y)
        {
            try
            {
                Console.SetCursorPosition(x, y);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }

        public void Paint()
        {
            Console.SetCursorPosition(0, 0);
            var coordinate = new Coordinate();
            for (coordinate.Y = 0; coordinate.Y < Map.Height; ++coordinate.Y)
            {
                for (coordinate.X = 0; coordinate.X < Map.Width; ++coordinate.X)
                {
                    var val = _map.GetValue(coordinate);
                    Console.Write(val);
                }
                Console.WriteLine();
            }
            Console.WriteLine(_helpMessage);
        }
    }

    class ConsoleEventSender
    {
        internal class ArrowKeyEventArgs : EventArgs
        {
            public ArrowKeyEventArgs(Arrow arrow)
            {
                Value = arrow;
            }
            public Arrow Value { get; }
        }

        public event EventHandler<ArrowKeyEventArgs> ArrowPressed;
        public event Action EscapePressed;

        protected virtual void OnArrowPressed(ArrowKeyEventArgs e)
        {
            ArrowPressed?.Invoke(this, e);
        }

        protected virtual void OnEscapePressed()
        {
            EscapePressed?.Invoke();
        }

        public void Run()
        {
            while (!_stopped)
            {
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        OnArrowPressed(new ArrowKeyEventArgs(Arrow.Left));
                        break;
                    case ConsoleKey.RightArrow:
                        OnArrowPressed(new ArrowKeyEventArgs(Arrow.Right));
                        break;
                    case ConsoleKey.UpArrow:
                        OnArrowPressed(new ArrowKeyEventArgs(Arrow.Up));
                        break;
                    case ConsoleKey.DownArrow:
                        OnArrowPressed(new ArrowKeyEventArgs(Arrow.Down));
                        break;
                    case ConsoleKey.Escape:
                        OnEscapePressed();
                        break;
                }
            }
        }

        private bool _stopped;

        public void Stop()
        {
            _stopped = true;
        }
    }

    class Game
    {
        private Painter _painter;
        private Map _map;
        private Mover _mover;

        public Game(Map map, Painter painter)
        {
            _map = map;
            _painter = painter;
            painter.Paint();
            _mover = new Mover(map);
        }

        public void OnArrowPressed(object sender, ConsoleEventSender.ArrowKeyEventArgs e)
        {
            var arrow = e.Value;
            if (_mover.TryMove(arrow))
            {
                _painter.Paint();
            }
        }

    }

    internal static class Program
    {
        public static void Main()
        {
            Console.CursorVisible = false;
            var map = Map.LoadFrom("Map.txt");
            var painter = new Painter(map, "Press ESC to exit");
            var game = new Game(map, painter);

            var eventSender = new ConsoleEventSender();
            eventSender.ArrowPressed += game.OnArrowPressed;
            eventSender.EscapePressed += () => eventSender.Stop();
            eventSender.Run();
        }
    }
}
