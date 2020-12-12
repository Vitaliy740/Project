using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGame
{
    class Asteroid:BaseObject
    {
        private bool _damaged;
        private int _health;
        private List<Point> _cracks = new List<Point>();
        private Random _rnd = new Random();
        public bool IsDead => _health < 1;
        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
;
            _health = _rnd.Next(1, Size.Width);
        }
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawEllipse(Pens.White, Pos.X, Pos.Y, Size.Width, Size.Height);
            if (_damaged)
            {
                for(int i=0;i<_cracks.Count;i+=2)
                    Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X + _cracks[i].X, Pos.Y + _cracks[i].Y, Pos.X +_cracks[i+1].X, Pos.Y + _cracks[i+1].Y);
            }
        }
        public void GetDamage(int n)
        {
            _health -= n;
            
            _cracks.Add(new Point(_rnd.Next(0, Size.Width), _rnd.Next(0, Size.Width)));
            _cracks.Add(new Point(_rnd.Next(0, Size.Width), _rnd.Next(0, Size.Width)));
            _damaged = true;
        }
        public void ChangeDir()
        {
            Dir.X = -Dir.X;
            Dir.Y = -Dir.Y;
        }

        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y + Dir.Y;
            if (Pos.X < 0 && Dir.X < 0) Pos.X = Game.Width + Size.Width;
            else if (Pos.Y < 0 && Dir.Y < 0) Pos.Y = Game.Height + Size.Height;
            else if (Pos.X > Game.Width && Dir.X > 0) Pos.X = 0 - Size.Width;
            else if (Pos.Y > Game.Height && Dir.Y > 0) Pos.Y = 0 - Size.Width;
        }
    }
}
