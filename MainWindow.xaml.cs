using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinFormsBufferingDemo.Utils;

namespace WinFormsBufferingDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Drawing.Bitmap _legacyRenderTarget;

        public MainWindow()
        {
            InitializeComponent();

            _winformsControl.Paint += (s, e) =>
            {
                // WinForms finished painting, the control is ready to 'screenshot'

                if(_legacyRenderTarget == null)
                {
                    // Keep the same backbuffer forever, no reason to recreate it with every hit
                    _legacyRenderTarget = new System.Drawing.Bitmap(_winformsControl.Width, _winformsControl.Height);
                    // TODO: Will make small leak when this window is disposed
                }

                // Screenshot the WinForms control into a WinForms bitmap
                _winformsControl.DrawToBitmap(_legacyRenderTarget, _winformsControl.Bounds);

                // Wire up WPF to consume that WinForms bitmap using Interop Bitmap.
                // Consume that backbuffer forever
                using (var hBitmap = new HBitmapWrapper(_legacyRenderTarget))
                {
                    _buf.Source = (InteropBitmap)Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
            };
        }
    }
}
