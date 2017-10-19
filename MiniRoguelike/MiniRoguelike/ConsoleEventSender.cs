using System;

namespace MiniRoguelike
{
    internal sealed class ConsoleEventSender
    {
        public event EventHandler<ArrowKeyEventArgs> ArrowPressed;
        public event Action EscapePressed;

        internal class ArrowKeyEventArgs : EventArgs
        {
            public ArrowKeyEventArgs(Arrow arrow)
            {
                Value = arrow;
            }
            public Arrow Value { get; }
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

        public void Stop()
        {
            _stopped = true;
        }

        private void OnArrowPressed(ArrowKeyEventArgs e)
        {
            ArrowPressed?.Invoke(this, e);
        }

        private void OnEscapePressed()
        {
            EscapePressed?.Invoke();
        }

        private bool _stopped;
    }
}