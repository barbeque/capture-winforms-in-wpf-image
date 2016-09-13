using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsBufferingDemo
{
    public partial class AnimatingGuy : UserControl
    {
        private Timer _timer;
        private int i;

        public AnimatingGuy()
        {
            InitializeComponent();

            _timer = new Timer();
            _timer.Interval = 6;
            _timer.Tick += OnTick;

            _timer.Start();

            this.Paint += AnimatingGuy_Paint;
        }

        private void AnimatingGuy_Paint(object sender, PaintEventArgs e)
        {
            using (var g = e.Graphics)
            {
                // clear the entire thing
                g.FillRectangle(Brushes.AliceBlue, Bounds);

                var rads = DegreesToRadians(i);
                var radius = Math.Min(Bounds.Width / 2, Bounds.Height / 2);
                var targetX = Math.Cos(rads) * radius + (Bounds.Width / 2);
                var targetY = Math.Sin(rads) * radius + (Bounds.Height / 2);

                // now draw a line to the extents
                var p = new Pen(Brushes.Red, 5);
                g.DrawLine(p, GetMidpoint(), new Point((int)targetX, (int)targetY));
            }
        }

        private void OnTick(object sender, EventArgs e)
        {
            i++;
            this.Invalidate();
        }

        private double DegreesToRadians(int degrees)
        {
            return degrees * (Math.PI / 180.0);
        }

        private Point GetMidpoint()
        {
            return new Point(Bounds.Width / 2, Bounds.Height / 2);
        }
    }
}
