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
        private MemoryStream _memoryStream;

        public MainWindow()
        {
            InitializeComponent();

            _memoryStream = new MemoryStream();

            _winformsControl.Paint += (s, e) =>
            {
                // Doo doo doo
                using (var bitmap = new System.Drawing.Bitmap(_winformsControl.Width, _winformsControl.Height))
                {
                    _winformsControl.DrawToBitmap(bitmap, _winformsControl.Bounds);

                    bitmap.Save(_memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
                    _memoryStream.Position = 0; // seek back                    

                    var wpfBitmap = new BitmapImage();
                    wpfBitmap.BeginInit();

                    wpfBitmap.CacheOption = BitmapCacheOption.OnLoad;
                    wpfBitmap.StreamSource = _memoryStream;

                    wpfBitmap.EndInit();

                    // Put it on the back buffer
                    _buf.Source = wpfBitmap;

                    _memoryStream.Position = 0;
                }
            };
        }
    }
}
