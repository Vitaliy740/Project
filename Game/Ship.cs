using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyGame
{
    class Ship : BaseObject
    {
        const int DEF = 100;
        private int _energy=DEF;
        private Direction _dir=Direction.Right;
        public int Energy => _energy;
        public delegate void Message();
        public static event Message MessageDie;
        public void EnergyLow(int n)
        {
            _energy -= n;
        }
        public void AddHealth(int n)
        {
            if (_energy < DEF)
            {
                _energy += n;
                if (_energy > 100) _energy = DEF;
            }
        }
        public void Die()
        {
            MessageDie?.Invoke();
        }
        public void ChangeDir(Direction dir)
        {
            _dir = dir;
        }
        public Direction GetDir()
        {
            return _dir;
        }
        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }
        public override void Draw()
        {
            switch (_dir)
            {
                case Direction.Right:
                    Point n1 = new Point(Pos.X, Pos.Y + 10);
                    Point n2 = new Point(Pos.X + 10, Pos.Y + 5);
                    Game.Buffer.Graphics.DrawLines(new Pen(Color.Red), new[] { n1, Pos, n2, n1 });
                    break;
                case Direction.Left:
                    n1 = new Point(Pos.X, Pos.Y + 10);
                    n2 = new Point(Pos.X - 10, Pos.Y + 5);
                    Game.Buffer.Graphics.DrawLines(new Pen(Color.Red), new[] { n1, Pos, n2, n1 });
                    break;
                case Direction.Down:
                    n1 = new Point(Pos.X + 10, Pos.Y);
                    n2 = new Point(Pos.X + 5, Pos.Y + 10);
                    Game.Buffer.Graphics.DrawLines(new Pen(Color.Red), new[] { n1, Pos, n2, n1 });
                    break;
                case Direction.Up:
                    n1 = new Point(Pos.X + 10, Pos.Y);
                    n2 = new Point(Pos.X + 5, Pos.Y - 10);
                    Game.Buffer.Graphics.DrawLines(new Pen(Color.Red), new[] { n1, Pos, n2, n1 });
                    break;
            }
        }
        public override void Update()
        {
        }
        public void Up()
        {
            if (Pos.Y > 0) Pos.Y = Pos.Y - Dir.Y;
        }
        public void Down()
        {
            if (Pos.Y < Game.Height) Pos.Y = Pos.Y + Dir.Y;
        }
        public void Right()
        {
            if (Pos.X < Game.Width) Pos.X = Pos.X + Dir.X;
        }
        public void Left()
        {
            if (Pos.X > 0) Pos.X = Pos.X - Dir.X;
            Console.WriteLine("ворк");
        }
    }
}
