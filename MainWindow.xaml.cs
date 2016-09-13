using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WinFormsBufferingDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            _winformsControl.Paint += (s, e) =>
            {
                // Doo doo doo
                using(var bitmap = new System.Drawing.Bitmap(_winformsControl.Width, _winformsControl.Height))
                {
                    _winformsControl.DrawToBitmap(bitmap, _winformsControl.Bounds);

                    using (var memoryStream = new MemoryStream())
                    {
                        bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                        memoryStream.Position = 0; // seek back
                        var wpfBitmap = new BitmapImage();
                        wpfBitmap.BeginInit();

                        wpfBitmap.CacheOption = BitmapCacheOption.OnLoad;
                        wpfBitmap.StreamSource = memoryStream;

                        wpfBitmap.EndInit();

                        // Put it on the back buffer
                        _buf.Source = wpfBitmap;
                    }
                }
            };
        }
    }
}
