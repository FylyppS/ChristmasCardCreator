using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace test
{
    /// <summary>
    /// Logika interakcji dla klasy email.xaml
    /// </summary>
    public partial class email : Window
    {
        public event EventHandler<EmailDataEventArgs> EmailDataSubmitted;
        public email()
        {
            InitializeComponent();
        }
        private void SendEmail_Click(object sender, RoutedEventArgs e)
        {
            string to = emailTo.Text;
            string subject = emailSubject.Text;
            string body = emailText.Text;

            // Wywołanie zdarzenia i przekazanie danych do MainWindow
            EmailDataSubmitted.Invoke(this, new EmailDataEventArgs(to, subject, body));
        }
        public class EmailDataEventArgs : EventArgs
        {
            public string To { get; }
            public string Subject { get; }
            public string Body { get; }

            public EmailDataEventArgs(string to, string subject, string body)
            {
                To = to;
                Subject = subject;
                Body = body;
            }
        }
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var textBox = sender as TextBox;
                if (textBox != null)
                {
                    int selectionIndex = textBox.SelectionStart;
                    textBox.Text = textBox.Text.Insert(selectionIndex, Environment.NewLine);
                    textBox.SelectionStart = selectionIndex + Environment.NewLine.Length;
                    e.Handled = true;
                }
            }
        }
    }
}
