using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    class GainKid : BaseObject
    {
        private Color _color;
        public GainKid(Point pos,Point dir, Size size) : base(pos, dir, size)
        {
            Random rnd = new Random();
            int n = rnd.Next(0, 3);
            if(n==0)
                    _color = Color.Green;
            else if(n==1)
                    _color = Color.Red;
            else if (n>=2)
                    _color = Color.Blue;
        }
        public Color GetKidType()
        {
            return _color;
        }
        public override void Draw()
        {
            Point n1 = new Point(Pos.X, Pos.Y + 5);
            Point n2 = new Point(Pos.X + 3, Pos.Y + 5);
            Point n3 = new Point(Pos.X + 3, Pos.Y);
            Point n4 = new Point(Pos.X + 8, Pos.Y);
            Point n5 = new Point(Pos.X + 8, Pos.Y - 3);
            Point n6 = new Point(Pos.X + 3, Pos.Y - 3);
            Point n7 = new Point(Pos.X + 3, Pos.Y - 8);
            Point n8 = new Point(Pos.X, Pos.Y - 8);
            Point n9 = new Point(Pos.X, Pos.Y - 3);
            Point n10 = new Point(Pos.X-5, Pos.Y-3);
            Point n11 = new Point(Pos.X - 5, Pos.Y);
            Game.Buffer.Graphics.DrawLines(new Pen(_color), new[] {Pos,n1,n2,n3,n4,n5,n6,n7,n8,n9,n10,n11,Pos });
            Game.Buffer.Graphics.DrawEllipse(new Pen(_color), Pos.X-5, Pos.Y-8, Size.Width, Size.Height);
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
