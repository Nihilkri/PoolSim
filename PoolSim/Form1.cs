using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoolSim {
	public partial class Form1 : Form {
		#region Variables
		#region Graphics
		Graphics gf, gb; Bitmap gi, ti;
		int fx, fy, fx2, fy2;
		float mm; int hudy; bool md = false;
		float xm = 0, ym = 0, tx = 0, ty = 0, ex = 0, ey = 0;

		#endregion Graphics
		#region Physics
		Timer phys;
		Ball[] balls; Ball cue;
		RectangleF table;

		#endregion Physics
		#endregion Variables
		#region Events
		public Form1() {InitializeComponent();}
		private void Form1_Load(object sender, EventArgs e) {
			fx = Width = Screen.PrimaryScreen.WorkingArea.Width; fx2 = fx / 2; Left = 0;
			fy = Height = Screen.PrimaryScreen.WorkingArea.Height; fy2 = fy / 2; Top = 0;
			gf = CreateGraphics(); gi = new Bitmap(fx, fy); gb = Graphics.FromImage(gi);

			NewGame("Snooker");

			phys = new Timer() { Interval = 1000 / 60 };
			phys.Tick += Phys_Tick; phys.Start();
		}

		private void Form1_Paint(object sender, PaintEventArgs e) {
			gf.DrawImage(gi, 0, 0);
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e) {
			switch(e.KeyCode) {
				case Keys.Escape: Close(); break;
				default: break;
			}
		}

		private void Form1_MouseMove(object sender, MouseEventArgs e) {
			if(md) {
				ex = e.X; ey = e.Y;
				//tx = cue.x + (ex - cue.x - 000.0F) * cue.d / 200.0F;
				//ty = cue.y + (ey - cue.y - 000.0F) * cue.d / 200.0F;
				tx = cue.x + (ex - xm - 000.0F) * cue.d / 200.0F;
				ty = cue.y + (ey - ym - 000.0F) * cue.d / 200.0F;
			} else {
			xm = ex = e.X; ym = ey = e.Y; tx = cue.x; ty = cue.y;
			}

		}
		private void Form1_MouseDown(object sender, MouseEventArgs e) {
			md = true;
		}
		private void Form1_MouseUp(object sender, MouseEventArgs e) {
			md = false;
		}

		private void Phys_Tick(object sender, EventArgs e) {
			//throw new NotImplementedException();
			gb.DrawImage(ti, 0, 0);

			foreach(Ball b in balls) {
				b.x += b.xx;b.y += b.yy;

			}
			foreach(Ball b in balls) {
				//gb.DrawEllipse(b.p, b.col);
				gb.DrawRectangle(b.p, b.col.X, b.col.Y, b.col.Width, b.col.Height);
			}
			if(table.Contains(xm, ym)) {
				gb.DrawLine(Pens.Brown, xm, ym, tx, ty);
			}
			if(md) {
				//gb.DrawRectangle(Pens.White, cue.x - 100, cue.y - 100, 200, 200);
				gb.DrawRectangle(cue.p, xm - 100, ym - 100, 200, 200);
				gb.FillEllipse(cue.b, xm - 100, ym - 100, 200, 200);
				gb.DrawString(string.Format("CX: {0}\nTX: {1}\nEX: {2}\nMX: {3}", cue.x, tx, ex, xm), Font, Brushes.Black, table.X + 10, table.Y + 10);
				gb.DrawString(string.Format("CY: {0}\nTY: {1}\nEY: {2}\nMY: {3}", cue.y, ty, ey, ym), Font, Brushes.Black, table.X + 100, table.Y + 10);
				/*gb.DrawString(string.Format("ex - xm - 000.0F: {0}\n(ex - xm - 000.0F) * cue.d / 200.0F: {1}\nTX: {2}",
					ex - xm - 000.0F, (ex - xm - 000.0F) * cue.d / 200.0F, tx), Font, Brushes.Black, table.X + 200, table.Y + 10);
				gb.DrawString(string.Format("ey - ym - 000.0F: {0}\n(ey - ym - 000.0F) * cue.d / 200.0F: {1}\nTY: {2}",
					ey - ym - 000.0F, (ey - ym - 000.0F) * cue.d / 200.0F, ty), Font, Brushes.Black, table.X + 200, table.Y + 50);*/

			}

			gf.DrawImage(gi, 0, 0);
		}

		#endregion Events
		#region Game
		public void NewGame(String gametype) {
			ti = new Bitmap(fx, fy);
			Graphics tg = Graphics.FromImage(ti);
			tg.Clear(Color.Black);
			switch(gametype) {
				case "Snooker":
					mm = fx / 3569.0F;
					table = new RectangleF(0.0F, 0.0F, 3569.0F * mm, 1778.0F * mm);
					tg.FillRectangle(Brushes.Green, table); hudy = (int)table.Bottom;
					table = new RectangleF((0.0F + 52.5F * 0.5F) * mm, (0.0F + 52.5F * 0.5F) * mm, (3569.0F - 52.5F) * mm, (1778.0F - 52.5F) * mm);
					tg.DrawRectangle(Pens.White, table.X, table.Y, table.Width, table.Height);
					//tg.DrawImage(new Bitmap("C:\\programs\\poolsim\\2000px-Snooker_table_drawing_2.svg.png"), tables);

					balls = new Ball[] {
						new Ball("Cue", Color.White, fx2 * 0.5F, fy2, 52.5F, mm),
						new Ball("Yellow", Color.Yellow, fx2 * 0.75F, fy2, 52.5F, mm)
					};

					break;
				default:
					mm = 1.0F;
					table = new RectangleF(0, 0, fx, fy2);
					tg.FillRectangle(Brushes.Green, table); hudy = (int)table.Bottom;
					tg.DrawString("Invalid Game Type", Font, Brushes.Black, fx2 - 8.5F * 8.0F, fy2 * 0.5F);

					balls = new Ball[] { new Ball("Cue", Color.White, fx2, fy2/4.0F, 50.0F, mm) };
					break;
			} cue = balls[0];

			tg.Dispose();
		}
		#endregion Game

	}

	public class Ball {
		public String tag; public Color c; public Pen p; public Brush b;
		public float x, y, xx, yy, d, m; public RectangleF col;
		public float[] r = {1.0F, 0.0F, 0.0F, 0.0F }, rr = {0.0F, 0.0F, 0.0F, 0.0F };

		public Ball(String ali, Color nc, float nx, float ny, float nd, float mm) {
			tag = ali; c = nc; p = new Pen(c); b = new SolidBrush(c);
			x = nx; y = ny; xx = yy = 0; d = nd*mm;
			col = new RectangleF(x - d / 2.0F, y - d / 2.0F, d, d);

			//International Pool Balls: According to WPA/BCA equipment specifications,
			//the weight may be from 5.5 to 6 oz (156–170 g) with a diameter of 2.250 inch (57.15 mm), plus or minus 0.005 inch (0.127 mm).
			m = ((156.0F + 170.0F) / 2.0F) / (float)Math.Pow(57.15, 3.0) * (float)Math.Pow(nd, 3.0);
		}


	}
}
