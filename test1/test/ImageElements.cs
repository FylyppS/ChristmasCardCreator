using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using Microsoft.Win32;
using System.Reflection;

namespace test
{
    public partial class MainWindow : Window
    {
        #region Background

        private void BtnOpenFile_back_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JPG Images |*.jpg;*.jpeg|" +
                        "PNG Images |*.png|" +
                        "TIFF Images |*.tif|" +
                        "PDF Images |*.pdf|" +
                        "All Images |*.png;*.jpg;*.jpeg;*.tif;*pdf"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                LoadBackgroundImage(fileName);
            }
        }

        private void LoadBackgroundImage(string imageName)
        {
            if (imageName == null)
            {
                back.Source = new BitmapImage(new Uri(@"pack://application:,,,/Pictures/back0.png"));
            }
            else
            {
                if (Uri.TryCreate(imageName, UriKind.Absolute, out Uri imageUri))
                {   // Treat imageName as URI
                    back.Source = new BitmapImage(imageUri);
                }
                else
                {   // Treat imageName as a resource image
                    back.Source = new BitmapImage(new Uri($@"pack://application:,,,/Pictures/{imageName}.png"));
                }
            }
        }

        #endregion

        #region Foreground / Frame

        private void BtnOpenFile_frame_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JPG Images |*.jpg;*.jpeg|" +
                        "PNG Images |*.png|" +
                        "TIFF Images |*.tif|" +
                        "PDF Images |*.pdf|" +
                        "All Images |*.png;*.jpg;*.jpeg;*.tif;*pdf"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                LoadFrameImage(fileName);
            }
        }

        private void LoadFrameImage(string imageName)
        {
            if (imageName == null)
            {
                frame.Source = null;
            }
            else
            {
                if (Uri.TryCreate(imageName, UriKind.Absolute, out Uri imageUri))
                {   // Treat imageName as URI
                    frame.Source = new BitmapImage(imageUri);
                }
                else
                {   // Treat imageName as a resource image
                    frame.Source = new BitmapImage(new Uri($@"pack://application:,,,/Frames/{imageName}.png"));
                }
            }
        }

        #endregion

        #region Elements

        private void BtnOpenFile_elem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JPG Images |*.jpg;*.jpeg|" +
                        "PNG Images |*.png|" +
                        "TIFF Images |*.tif|" +
                        "PDF Images |*.pdf|" +
                        "All Images |*.png;*.jpg;*.jpeg;*.tif;*pdf"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                CreateElement(fileName);
            }
        }

        private void CreateElement(string imageName)
        {
            BitmapImage tempBitmap;

            if (Uri.TryCreate(imageName, UriKind.Absolute, out Uri imageUri))
            {   // Treat imageName as URI
                tempBitmap = new BitmapImage(imageUri);
            }
            else
            {   // Treat imageName as a resource image
                tempBitmap = new BitmapImage(new Uri($@"pack://application:,,,/Elements/{imageName}.png"));
            }

            Image newElement = new Image();

            newElement.MouseUp += OnMouseUp;
            newElement.MouseDown += OnMouseDown;
            newElement.MouseMove += OnMouseDrag;
            newElement.MouseWheel += OnMouseScroll;

            newElement.Source = tempBitmap;

            TransformGroup newTransformGroup = new TransformGroup();
            newTransformGroup.Children.Add(new RotateTransform());
            newTransformGroup.Children.Add(new ScaleTransform());
            newTransformGroup.Children.Add(new TranslateTransform());

            newElement.RenderTransform = newTransformGroup;
            newElement.RenderTransformOrigin = new Point(0.5, 0.5);

            Size canvasSize = new Size(500, 500);
            if (tempBitmap.Width > canvasSize.Width || tempBitmap.Height > canvasSize.Height)
            {
                float fitScale = Math.Min((float)(canvasSize.Width / tempBitmap.Width), (float)(canvasSize.Height / tempBitmap.Height));

                newElement.Width = tempBitmap.Width * fitScale;
                newElement.Height = tempBitmap.Height * fitScale;
            }

            imageSpace.Children.Add(newElement);
        }

        #endregion

        #region Text

        TextBlock SelectedTextBlock = null;            

        private void BtnCreateTextBox_Click(object sender, RoutedEventArgs e)
        {
            // Tworzenie nowego pola tekstowego
            TextBlock nowePoleTekstowe = new TextBlock();

            nowePoleTekstowe.MouseUp += OnMouseUp;
            nowePoleTekstowe.MouseDown += OnMouseDownTextBlock;
            nowePoleTekstowe.MouseMove += OnMouseDrag;
            nowePoleTekstowe.MouseWheel += OnMouseScroll;

            nowePoleTekstowe.Text = "Przykładowy nowy tekst";

            nowePoleTekstowe.FontSize = 22;
            nowePoleTekstowe.FontFamily = textBlock.FontFamily;
            nowePoleTekstowe.Foreground = textBlock.Foreground;
            nowePoleTekstowe.TextWrapping = TextWrapping.Wrap;
            nowePoleTekstowe.TextAlignment = TextAlignment.Center;
            nowePoleTekstowe.MaxWidth = 256;

            TransformGroup newTransformGroup = new TransformGroup();
            newTransformGroup.Children.Add(new RotateTransform());
            newTransformGroup.Children.Add(new ScaleTransform());
            newTransformGroup.Children.Add(new TranslateTransform());

            nowePoleTekstowe.RenderTransform = newTransformGroup;
            nowePoleTekstowe.RenderTransformOrigin = new Point(0.5, 0.5);

            imageSpace.Children.Add(nowePoleTekstowe);
        }

        #endregion
    }
}
