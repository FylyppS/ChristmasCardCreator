using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace test
{

    public partial class MainWindow : Window
    {
        private void CheckBoxGreyscale_Checked(object sender, RoutedEventArgs e)
        {
            foreach (UIElement elem in imageSpace.Children)
            {
                if (elem is TextBlock)
                    continue;

                var imageElem = (System.Windows.Controls.Image)elem;
                if (imageElem.Name == "frame")
                    continue;

                imageElem.Source = Grayscale.ConvertToGrayscale((BitmapImage)imageElem.Source);
            }

            string framePath;

            // Konwertuj obraz na odcienie szarości
            Radio1.Tag = "frame0_Grey";
            Radio2.Tag = "frame1_Grey";
            Radio3.Tag = "frame2_Grey";

            if (Radio1.IsChecked == true)
                framePath = Radio1.Tag.ToString();
            else if (Radio2.IsChecked == true)
                framePath = Radio2.Tag.ToString();
            else if (Radio3.IsChecked == true)
                framePath = Radio3.Tag.ToString();
            else
                framePath = null;

            LoadFrameImage(framePath);

            // Zamień kolor tekstu na odpowiedni w odcieni szarości
            switch (currentFontColorName)
            {
                case "White":
                case "Black":
                    break;
                default:
                    textBlock.Foreground = System.Windows.Media.Brushes.DarkGray; break;
            }
        }
        private void CheckBoxGreyscale_Unchecked(object sender, RoutedEventArgs e)
        {
            // Przywróć oryginalny obraz
            LoadBackgroundImage(currentImg);

            string framePath;

            // Przywróc domyślne ramki
            Radio1.Tag = "frame0";
            Radio2.Tag = "frame1";
            Radio3.Tag = "frame2";

            if (Radio1.IsChecked == true)
                framePath = Radio1.Tag.ToString();
            else if (Radio2.IsChecked == true)
                framePath = Radio2.Tag.ToString();
            else if (Radio3.IsChecked == true)
                framePath = Radio3.Tag.ToString();
            else
                framePath = null;

            LoadFrameImage(framePath);

            // Ustaw kolor czcionki na podstawie wyboru z ComboBox
            textBlock.Foreground = GetBrushFromColorName(currentFontColorName);
        }
    }

    public static class Grayscale
    {
        public static MemoryStream ConvertBitmapSource(BitmapSource bitmap)
        {
            MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(stream);
            return stream;
        }

        public static BitmapImage ConvertToGrayscale(BitmapImage originalImage)
        {
            // Przetwarzaj tylko piksele nieprzezroczyste
            BitmapSource processedImage = new FormatConvertedBitmap(originalImage, PixelFormats.Gray32Float, null, 0);

            // Utwórz nowy BitmapImage z przetworzonym obrazem
            BitmapImage grayImage = new BitmapImage();
            grayImage.BeginInit();
            grayImage.CacheOption = BitmapCacheOption.OnLoad;
            grayImage.StreamSource = ConvertBitmapSource(processedImage);
            grayImage.EndInit();

            return grayImage;
        }

        public static System.Drawing.Image ConvertToGrayscaleFinal(Bitmap originalImage)
        {
            // Konwertuj obraz na odcienie szarości
            Bitmap grayBitmap = new Bitmap(originalImage.Width, originalImage.Height);

            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    System.Drawing.Color pixelColor = originalImage.GetPixel(x, y);
                    int grayValue = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);
                    System.Drawing.Color newColor = System.Drawing.Color.FromArgb(pixelColor.A, grayValue, grayValue, grayValue);
                    grayBitmap.SetPixel(x, y, newColor);
                }
            }

            // Zapisz odcienie szarości do strumienia bajtów
            MemoryStream memoryStream = new MemoryStream();
            grayBitmap.Save(memoryStream, ImageFormat.Png);
            memoryStream.Position = 0;

            // Utwórz obraz z odcieniami szarości z tego strumienia
            System.Drawing.Image grayImage = System.Drawing.Image.FromStream(memoryStream);

            // Zwolnij zasoby
            grayBitmap.Dispose();
            memoryStream.Dispose();

            return grayImage;
        }
    }
}
