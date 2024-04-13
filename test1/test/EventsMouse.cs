using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace test
{
    public partial class MainWindow : Window
    {
        Point dragStart = new Point(0, 0);

        private void OnMouseScroll(object sender, MouseWheelEventArgs e)
        {
            var element = (UIElement)sender;

            if (Keyboard.IsKeyDown(Key.LeftShift))
            {   // Change order of elements
                if (e.Delta > 0)
                    Canvas.SetZIndex(element, Canvas.GetZIndex(element) + 1);
                else
                    Canvas.SetZIndex(element, Canvas.GetZIndex(element) - 1);

                Console.WriteLine("ZIndex of focused element: " + Canvas.GetZIndex(element));
            }

            else if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {   // Rotate
                var transform = element.RenderTransform as TransformGroup;
                RotateTransform child = (RotateTransform)transform.Children[0];

                var angleChange = e.Delta * 0.05;
                child.Angle += angleChange;
            }

            else
            {   // Scale
                var transform = element.RenderTransform as TransformGroup;
                ScaleTransform child = (ScaleTransform)transform.Children[1];

                var scaleChange = 0.05F;
                if (e.Delta > 0)
                {
                    child.ScaleX += scaleChange;
                    child.ScaleY += scaleChange;
                }
                else
                {
                    child.ScaleX -= scaleChange;
                    child.ScaleY -= scaleChange;
                }
            }
        }

        private void OnMouseDownTextBlock(object sender, MouseButtonEventArgs e)
        {
            SelectedTextBlock = (TextBlock)sender;
            cardText.Text = SelectedTextBlock.Text;

            OnMouseDown(sender, e);
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            var element = (UIElement)sender;

            if (e.LeftButton == MouseButtonState.Pressed)
            {   // Save position for calculations during drag
                dragStart = e.GetPosition(element);
                element.CaptureMouse();
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {   // Reset element position
                var transform = element.RenderTransform as TransformGroup;
                TranslateTransform child = (TranslateTransform)transform.Children[2];

                child.X = 0;
                child.Y = 0;
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            var element = (UIElement)sender;

            // Starting position is no longer needed
            dragStart = new Point(0, 0);

            element.ReleaseMouseCapture();
        }

        private void OnMouseDrag(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {   // Move / Pan (gets buggy when close to edges of image)
                var element = (UIElement)sender;
                var p2 = e.GetPosition(imageSpace);

                var transform = element.RenderTransform as TransformGroup;
                TranslateTransform child = (TranslateTransform)transform.Children[2];

                child.X = p2.X - dragStart.X;
                child.Y = p2.Y - dragStart.Y;
            }
        }

        #region InkCanvas

        private void InkCanvas_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Rozpocznij mazanie gumką
            Mouse.OverrideCursor = Cursors.Cross;
            inkCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
        }

        private void InkCanvas_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Zakończ mazanie gumką
            Mouse.OverrideCursor = Cursors.Pen;
            inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
        }

        #endregion
    }
}
