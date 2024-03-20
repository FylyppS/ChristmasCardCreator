using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace test
{
    public partial class MainWindow : Window
    {
        private void RadioF_Checked(object sender, RoutedEventArgs e)
        {
            string imagePath = ((RadioButton)sender).Tag?.ToString();
            LoadFrameImage(imagePath);
        }

        private void RadioB_Checked(object sender, RoutedEventArgs e)
        {
            string imagePath = ((RadioButton)sender).Tag?.ToString();
            LoadBackgroundImage(imagePath);
        }

        private void ButtonE_Click(object sender, RoutedEventArgs e)
        {
            string ElemPath = ((Button)sender).Tag?.ToString();
            CreateElement(ElemPath);
        }

        private void RadioT_Checked(object sender, RoutedEventArgs e)
        {
            string Text = ((RadioButton)sender).Tag?.ToString();
            SelectedTextBlock.Text = Text;
            cardText.Text = Text;
        }

        #region Text Events

        private void CardText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SelectedTextBlock != null)
            {
                Radio6.IsChecked = Radio7.IsChecked = false;
                SelectedTextBlock.Text = cardText.Text;
            }
        }

        private void FontList_SelectionChanged(object sender, RoutedEventArgs e)
        {
            textBlock.FontFamily = new FontFamily(FontList.SelectedItem.ToString());

            UpdateAllTextBoxes(imageSpace);
        }

        private void FontColorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Color newFontColor = (Color)(FontColorList.SelectedItem as PropertyInfo).GetValue(this);
            textBlock.Foreground = new SolidColorBrush(newFontColor);

            UpdateAllTextBoxes(imageSpace);
        }

        private void UpdateAllTextBoxes(Canvas canvas)
        {
            foreach (UIElement element in canvas.Children)
            {
                if (element is TextBlock text)
                {
                    text.FontFamily = textBlock.FontFamily;
                    text.Foreground = textBlock.Foreground;
                }
            }
        }

        #endregion

        #region Ink Canvas Events

        private void InteractionCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            inkCanvas.IsHitTestVisible = true;
            lineThicknessComboBox.IsEnabled = lineColorComboBox.IsEnabled = true;
            Mouse.OverrideCursor = Cursors.Pen;
        }

        private void InteractionCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            inkCanvas.IsHitTestVisible = false;
            lineThicknessComboBox.IsEnabled = lineColorComboBox.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void LineThicknessComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedThickness = int.Parse(((ComboBoxItem)lineThicknessComboBox.SelectedItem).Content.ToString().Split('p')[0]);

            if (inkCanvas != null)
                inkCanvas.DefaultDrawingAttributes.Width = inkCanvas.DefaultDrawingAttributes.Height = selectedThickness;
        }

        private void LineColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Color selectedColor = (Color)(lineColorComboBox.SelectedItem as PropertyInfo).GetValue(this);

            if (inkCanvas != null)
                inkCanvas.DefaultDrawingAttributes.Color = selectedColor;
        }

        #endregion
    }
}
