using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WinFormsBufferingDemo.Utils
{
    public class AirspaceOverlay : Decorator
    {
        private readonly Window _transparentInputWindow;
        private Window _parentWindow;

        public AirspaceOverlay()
        {
            _transparentInputWindow = CreateTransparentWindow();
            _transparentInputWindow.PreviewMouseDown += _transparentInputWindow_PreviewMouseDown;
        }

        public object OverlayChild
        {
            get { return _transparentInputWindow.Content; }
            set { _transparentInputWindow.Content = value; }
        }

        private static Window CreateTransparentWindow()
        {
            var w = new Window();

            w.Background = System.Windows.Media.Brushes.Transparent;
            w.AllowsTransparency = true;
            w.WindowStyle = WindowStyle.None;

            w.ShowInTaskbar = false;
            w.Focusable = false;

            return w;
        }

        private void _transparentInputWindow_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _parentWindow.Focus();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateOverlaySize();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if(_transparentInputWindow.Visibility != Visibility.Visible && !IsInDesigner())
            {
                UpdateOverlaySize();
                _parentWindow = GetParentWindow(this);
                _transparentInputWindow.Show();
                _transparentInputWindow.Owner = _parentWindow;
                _parentWindow.LocationChanged += _parentWindow_LocationChanged;
                _parentWindow.SizeChanged += _parentWindow_SizeChanged;
            }
        }

        private static Window GetParentWindow(DependencyObject o)
        {
            var parent = VisualTreeHelper.GetParent(o);
            if(parent != null)
            {
                return GetParentWindow(parent);
            }

            var fe = o as FrameworkElement;
            if(fe is Window)
            {
                return fe as Window;
            }
            if(fe != null && fe.Parent != null)
            {
                return GetParentWindow(fe.Parent);
            }

            throw new ApplicationException($"Window parent could not be found for {o}");
        }

        private void _parentWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateOverlaySize();
        }

        private void _parentWindow_LocationChanged(object sender, EventArgs e)
        {
            UpdateOverlaySize();
        }

        private void UpdateOverlaySize()
        {
            var hostTopLeft = GoodPointToScreen(0, 0);
            _transparentInputWindow.Left = hostTopLeft.X;
            _transparentInputWindow.Top = hostTopLeft.Y;
            _transparentInputWindow.Width = ActualWidth;
            _transparentInputWindow.Height = ActualHeight;
        }

        private System.Windows.Point GoodPointToScreen(float x, float y)
        {
            if(_parentWindow == null)
            {
                return PointToScreen(new System.Windows.Point(0, 0)); // big suck
            }

            var presentationSource = PresentationSource.FromVisual(_parentWindow);
            var screenSpaceTransform = presentationSource.CompositionTarget.TransformFromDevice;
            var windowSpaceCoord = PointToScreen(new System.Windows.Point(0, 0));

            return screenSpaceTransform.Transform(windowSpaceCoord);
        }

        private bool IsInDesigner()
        {
            return System.ComponentModel.DesignerProperties.GetIsInDesignMode(this);
        }
    }
}
