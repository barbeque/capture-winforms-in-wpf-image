using System;
using System.Runtime.InteropServices;

namespace WinFormsBufferingDemo.Utils
{
    /// <summary>
    /// RAII wrapper for getting and disposing of a handle from a bitmap
    /// </summary>
    internal class HBitmapWrapper : IDisposable
    {
        private IntPtr _hBitmap;

        public HBitmapWrapper(System.Drawing.Bitmap source)
        {
            _hBitmap = source.GetHbitmap();
        }

        public void Dispose()
        {
            DeleteObject(_hBitmap);
        }

        public static implicit operator IntPtr(HBitmapWrapper wrapper)
        {
            return wrapper._hBitmap;
        }

        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);
    }
}
