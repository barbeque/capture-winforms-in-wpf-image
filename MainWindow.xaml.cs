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
        private MemoryStream _memoryStream;
        private System.Drawing.Bitmap _legacyRenderTarget;

        public MainWindow()
        {
            InitializeComponent();

            //_memoryStream = new MemoryStream();

            _winformsControl.Paint += (s, e) =>
            {
                // WinForms finished painting, the control is ready to 'screenshot'

                if(_legacyRenderTarget == null)
                {
                    // Keep the same backbuffer forever
                    _legacyRenderTarget = new System.Drawing.Bitmap(_winformsControl.Width, _winformsControl.Height);
                }

                // Screenshot the WinForms control into a WinForms bitmap
                _winformsControl.DrawToBitmap(_legacyRenderTarget, _winformsControl.Bounds);

                // Wire up WPF to consume that WinForms bitmap using Interop Bitmap.
                // Consume that backbuffer forever
                //(_buf.Source as InteropBitmap).Dispose();
                using (var hBitmap = new HBitmapWrapper(_legacyRenderTarget))
                {
                    _buf.Source = (InteropBitmap)Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
            };
        }
    }
}
