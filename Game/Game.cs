using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
namespace MyGame
{
	static class Game
	{
		private static Timer _timer = new Timer();
		public static Random Rnd = new Random();
		private static BufferedGraphicsContext _context;
		private static List<Bullet> _bullet=new List<Bullet>();
		private static List<Asteroid> _asteroids=new List<Asteroid>();
		private static Ship _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(10, 10));
		private static List<GainKid> _gainKids=new List<GainKid>();
		private static int _score = 0;
		private static int _level = 0;
		private static int _bulletspeed = 1;
		private static int _bulletdamage = 5;

		private static void Timer_tick(object sender, EventArgs e)
		{
			Update();
			Draw();
		}
		public static BufferedGraphics Buffer;
		public static BaseObject[] _objs;
		public static int Width { get; set; }
		public static int Height { get; set; }
		static Game()
		{

		}
		public static void Init(Form form)
		{
			Graphics g;
			_context = BufferedGraphicsManager.Current;
			g = form.CreateGraphics();
			form.KeyDown += Form_KeyDown;
			Width = form.ClientSize.Width;
			Height = form.ClientSize.Height;
			//Width = 900;
			//Height = 900;
			Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
			Load();
			//Timer timer = new Timer { Interval = 100 };
			Ship.MessageDie += Finish;
			_timer.Start();
			_timer.Tick += Timer_tick;
			

		}
		private static void SpawnKids(int lvl)
		{
			int n = Rnd.Next(1, lvl);
			
			int px = 0;
			int py = 0;
			for (int i=0; i < n; i++)
			{
				int x;
				int y;
				int r=Rnd.Next(10,20);
				do
				{
					x = Rnd.Next(100, Width-100);
					y = Rnd.Next(100, Height-100);
				}
				while (Math.Abs(x - px) < 51 && Math.Abs(y - py) < 51);
				int s = Rnd.Next(0, 3);
				if (s == 0)
					_gainKids.Add(new GainKid(new Point(1000, y), new Point(-r , 0), new Size(13, 13)));
				else if (s == 1)
					_gainKids.Add(new GainKid(new Point(x, 800), new Point(0, -r ), new Size(13, 13)));
				else if (s == 2)
					_gainKids.Add(new GainKid(new Point(0, y), new Point(+r, 0), new Size(13, 13)));
				else if (s == 4)
					_gainKids.Add(new GainKid(new Point(x, 0), new Point(0, +r ), new Size(13, 13)));
			}
		}
		public static void Draw()
		{
			Buffer.Graphics.Clear(Color.Black);
			//Buffer.Graphics.DrawRectangle(Pens.White, new Rectangle(100, 100, 200, 200));
			//Buffer.Graphics.FillEllipse(Brushes.Wheat, new Rectangle(100, 100, 200, 200));
			foreach (BaseObject obj in _objs)
			{
				obj.Draw();
			}
			foreach (Asteroid obj in _asteroids)
			{
				obj?.Draw();
				
			}
			foreach (GainKid g in _gainKids) g.Draw();
			foreach (Bullet b in _bullet) b.Draw();
			_ship.Draw();
			if (_ship != null)
			{
				Buffer.Graphics.DrawString("Energy:" + _ship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);
				Buffer.Graphics.DrawString("Score:" + _score, SystemFonts.DefaultFont, Brushes.White, Width-80, 0);
				Buffer.Render();
			}
			Buffer.Render();
		}
		public static void Update()
		{
			foreach (BaseObject obj in _objs) obj.Update();
			foreach (Bullet b in _bullet) b.Update();
			if (_asteroids.Count == 0)
			{
				_level += 1;
				Add_newAsteroids(_level);
				SpawnKids(_level);

			}
			for (var i=0;i<_asteroids.Count;i++)
			{
				if (_asteroids[i] == null) continue;
				_asteroids[i].Update();
				for (int j = 0; j < _bullet.Count; j++)
				{
					if (_asteroids.Count == 0) continue;
					if (_bullet[j].Collision(_asteroids[i]))
					{
						System.Media.SystemSounds.Hand.Play();
						_asteroids[i].GetDamage(_bulletdamage);
						if (_asteroids[i].IsDead)
						{
							_score += Rnd.Next(10 + _level,30+_level);
							_asteroids.RemoveAt(i);
							if (i != 0)
								i--;
						}

						_bullet.RemoveAt(j);
						j--;
					}
				}
				if (_asteroids.Count!=0 && !_ship.Collision(_asteroids[i])) continue;
				_ship?.EnergyLow(Rnd.Next(1, 10));
				System.Media.SystemSounds.Asterisk.Play();
				
			}
			for (var i = 0; i < _asteroids.Count-1; i++)
			{
				for (var j = i; j < _asteroids.Count; j++)
				{
					if (_asteroids[i].Collision(_asteroids[j]))
					{
						_asteroids[i].ChangeDir();
						_asteroids[j].ChangeDir();
					}
				}
			}
			if (_gainKids.Count > 0)
				for (int i = 0; i < _gainKids.Count; i++)
				{
					_gainKids[i].Update();
					if (_ship.Collision(_gainKids[i]))
					{
						if (_gainKids[i].GetKidType() == Color.Green)
							_ship.AddHealth(10);
						else if (_gainKids[i].GetKidType() == Color.Red)
							_bulletdamage += 5;
						else if (_gainKids[i].GetKidType() == Color.Blue)
							_bulletspeed += 1;
						_gainKids.RemoveAt(i);
						if (i != 0)
							i--;
					}
				}
			if (_ship.Energy <= 0) _ship.Die();

		}
		private static void Load()
		{
			_objs = new BaseObject[30];
			var rnd = new Random();
			for (var i = 0; i < _objs.Length; i++)
			{
				int r = rnd.Next(5, 50);
				_objs[i] = new Star(new Point(1000, rnd.Next(0, Game.Height)), new Point(-r, r), new Size(3, 3));
			}
			Add_newAsteroids(_level);
		}
		private static void Add_newAsteroids(int lvl)
		{
			if (lvl == 0)
			{
				int r;
				int py = 0;
				int y;
				for (var i = 0; i < 5; i++)
				{
					r = Rnd.Next(5, 50);
					y = Rnd.Next(0, Height);
					while (Math.Abs(y - py) < 100)
					{
						y = Rnd.Next(50, Height-50);
					}
					py = y;
					_asteroids.Add(new Asteroid(new Point(1000, y), new Point(-r / 2, 0), new Size(r, r)));
				}
			}
			else
			{
				int r;
				int px = 0;
				int py = 0;
				for (var i = 0; i < 5 + lvl; i++)
				{
					r = Rnd.Next(5 + lvl, 50 + lvl);
					int x = Rnd.Next(50 + lvl, Width - 50 -lvl) ;
					int y = Rnd.Next(50 +lvl, Height - 50-lvl) ;
					while (Math.Abs(x - px) < 51 && Math.Abs(y - py) < 51)
					{
						x = Rnd.Next(50, Width-50);
						y = Rnd.Next(50, Height - 50);
					}
					
					int s = Rnd.Next(0, 3);
					if (s == 0)
						_asteroids.Add(new Asteroid(new Point(1000, y), new Point(-r / 2, 0), new Size(r, r)));
					else if (s == 1)
						_asteroids.Add(new Asteroid(new Point(x, 800), new Point(0, -r / 2), new Size(r, r)));
					else if (s == 2)
						_asteroids.Add(new Asteroid(new Point(0, y), new Point(+r / 2, 0), new Size(r, r)));
					else if (s == 4)
						_asteroids.Add(new Asteroid(new Point(x, 0), new Point(0, +r / 2), new Size(r, r)));
					px = x;
					py = y;
				}
			}
		}
		private static void Form_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.ControlKey) 
			{
				switch (_ship.GetDir())
				{
					case Direction.Right:
						_bullet.Add(new Bullet(new Point(_ship.Rect.X + 2, _ship.Rect.Y + 4), new Point(9+_bulletspeed, 0), new Size(4, 1)));
						break;
					case Direction.Left:
						_bullet.Add(new Bullet(new Point(_ship.Rect.X - 5, _ship.Rect.Y + 4), new Point(-9-_bulletspeed, 0), new Size(4, 1)));
						break;
					case Direction.Up:
						_bullet.Add(new Bullet(new Point(_ship.Rect.X+4, _ship.Rect.Y - 5), new Point(0,-9-_bulletspeed), new Size(1, 4)));;
						break;
					case Direction.Down:
						_bullet.Add(new Bullet(new Point(_ship.Rect.X + 4, _ship.Rect.Y +2), new Point(0, 9+_bulletspeed), new Size(1, 4)));
						break;
				}
			}
			if (e.KeyCode == Keys.Up) { 
				_ship.Up();
			}
			if (e.KeyCode == Keys.Down) _ship.Down();
			if (e.KeyCode == Keys.Right) _ship.Right();
			if (e.KeyCode == Keys.Left) _ship.Left();
			if (e.KeyCode == Keys.A) _ship.ChangeDir(Direction.Left);
			if (e.KeyCode == Keys.D) _ship.ChangeDir(Direction.Right);
			if (e.KeyCode == Keys.W) _ship.ChangeDir(Direction.Up);
			if (e.KeyCode == Keys.S) _ship.ChangeDir(Direction.Down);
		}
		public static void Finish()
		{
			Buffer.Graphics.DrawString("Death", SystemFonts.DefaultFont, Brushes.White, 0,50);
			_timer.Stop();
			Buffer.Render();
		}
	}
	interface ICollision
	{
		bool Collision(ICollision obj);
		Rectangle Rect { get; }
	}
}
