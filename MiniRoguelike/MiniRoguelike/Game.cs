namespace MiniRoguelike
{
    internal class Game
    {
        public Game(Map map)
        {
            var rogueMap = new RogueMap(map);
            _mover = new Mover(rogueMap);
            _painter = new Painter(rogueMap, "Press ESC to exit");
            _painter.Paint();
        }

        public void OnArrowPressed(object sender, ConsoleEventSender.ArrowKeyEventArgs e)
        {
            var arrow = e.Value;
            if (_mover.TryMove(arrow))
            {
                _painter.Paint();
            }
        }

        private readonly Painter _painter;
        private readonly Mover _mover;
    }
}