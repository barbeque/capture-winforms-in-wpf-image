using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _timer.Interval = 20;
            _timer.Tick += OnTick;

            _timer.Start();
        }

        private void OnTick(object sender, EventArgs e)
        {
            using (var g = this.CreateGraphics())
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

            i++;
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
