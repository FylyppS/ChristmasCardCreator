using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using QRCoder;
using System;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using static QRCoder.PayloadGenerator;

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

            cardSize.SelectedIndex = 0; // 1000x1000px

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
                dc.DrawRectangle(vb, null, new Rect(new System.Windows.Point(0, 0), imageSpace.RenderSize));
            }

            renderedCanvas.Render(dv);

            return renderedCanvas;
        }

        private void BtnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            SavePicture();
        }
        private string SavePicture()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Obraz JPG|*.jpg|" +
                        "Obraz PNG|*.png|" +
                        "Obraz TIFF|*.tif|" +
                        "Plik PDF|*.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                // Select file size from ComboBox
                string selectedSize = (string)((ComboBoxItem)cardSize.SelectedItem).Content;
                string[] sizeParts = selectedSize.Split('x');
                int width = int.Parse(sizeParts[0]);
                int height = int.Parse(sizeParts[1].Split('p')[0]);

                // Render, encode and save the canvas
                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(RenderCanvas(width, height)));

                using (var fs = File.OpenWrite(saveFileDialog.FileName))
                    pngEncoder.Save(fs);
                string attachmentFilePath = saveFileDialog.FileName;
                MessageBox.Show("Zdjęcie zapisane pomyślnie.");
                return attachmentFilePath;
            }
            return null;
        }

        #endregion
        #region Extra windows / email
        private void OpenAdditionalWindow_Click(object sender, RoutedEventArgs e)
        {
            tips additionalWindow = new tips();
            additionalWindow.Show();
        }

        
        private void OpenEmailWindow_Click(object sender, RoutedEventArgs e)
        {
            email emailWindow = new email();
            emailWindow.EmailDataSubmitted += EmailDataSubmitted;
            emailWindow.Show();
        }
        private void EmailDataSubmitted(object sender, email.EmailDataEventArgs e)
        {
            // Wywołanie metody wysyłającej e-mail w MainWindow, przekazując dane z okna Email
            SendEmailWithAttachment(e.To, e.Subject, e.Body);
        }
        private void SendEmailWithAttachment(string To, string Subject, string Body)
        {
            try
            {
                using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("generatorkartek@gmail.com", "vfyu hfyy mrlq jtne");
                    MailMessage message = new MailMessage();
                    message.To.Add(To);
                    message.From = new MailAddress("generatorkartek@gmail.com");
                    message.Subject = Subject;
                    message.Body = Body;
                    
                    // Dodanie załącznika
                    Attachment attachment = new Attachment(SavePicture());
                    message.Attachments.Add(attachment);
                    client.Send(message);
                    MessageBox.Show("Twój email został pomyślnie wysłany!", "Email został wysłany", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}
