using Microsoft.Win32;
using System.Drawing.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace test
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Load all installed fonts
            using (InstalledFontCollection col = new InstalledFontCollection())
            {
                foreach (var font in col.Families)
                    FontList.Items.Add(font.Name);
            }

            // Load colors
            FontColorList.ItemsSource = typeof(Colors).GetProperties();
            lineColorComboBox.ItemsSource = typeof(Colors).GetProperties();


            // Default selections for comboBoxes
            FontList.SelectedItem = "Arial";
            
            FontColorList.SelectedIndex = 7; // Black
            lineColorComboBox.SelectedIndex = 7; // Black

            cardSize.SelectedIndex = 2; // 1000x1000px

            lineThicknessComboBox.IsEnabled = false;
            lineColorComboBox.IsEnabled = false;

            LoadBackgroundImage(null);
            LoadFrameImage(null);
        }
            
        #region Save/Open Files and images

        private BitmapSource RenderCanvas(int width, int height)
        {
            float scaleRatiox = (float)(width / imageSpace.RenderSize.Width);
            float scaleRatioy = (float)(height / imageSpace.RenderSize.Width);

            RenderTargetBitmap renderedCanvas = new RenderTargetBitmap(
                width,
                height,
                96d * scaleRatiox,
                96d * scaleRatioy,
                PixelFormats.Default
                );

            // Get rid of margins on picture with the following lines:
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(imageSpace);
                dc.DrawRectangle(vb, null, new Rect(new Point(0, 0), imageSpace.RenderSize));
            }

            renderedCanvas.Render(dv);

            return renderedCanvas;
        }

        private void BtnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JPG Image|*.jpg|" +
                        "PNG Image|*.png|" +
                        "TIFF Images |*.tif|" +
                        "PDF Images |*.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                // Pobierz wybrany rozmiar z ComboBox
                string selectedSize = (string)((ComboBoxItem)cardSize.SelectedItem).Content;
                string[] sizeParts = selectedSize.Split('x');
                int width = int.Parse(sizeParts[0]);
                int height = int.Parse(sizeParts[1].Split('p')[0]);

                // Render, encode and save the canvas
                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(RenderCanvas(width, height)));

                using (var fs = File.OpenWrite(saveFileDialog.FileName))
                    pngEncoder.Save(fs);

                MessageBox.Show("Image saved successfully.");
            }
        }

        #endregion
    }
}