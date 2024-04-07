using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

namespace test
{
    public partial class MainWindow : Window
    {
        private void RadioF_Checked(object sender, RoutedEventArgs e)
        {
            string imagePath = ((MenuItem)sender).Tag?.ToString();
            LoadFrameImage(imagePath);
        }

        private void RadioB_Checked(object sender, RoutedEventArgs e)
        {
            string imagePath = ((MenuItem)sender).Tag?.ToString();
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
            if (SelectedTextBlock == null) 
            { SelectedTextBlock = textBlock;
              SelectedTextBlock.Text = Text;
              cardText.Text = Text;
            }
            else 
            {
                SelectedTextBlock.Text = Text;
                cardText.Text = Text;
            }
        }

        private void TEST_Checked(object sender, RoutedEventArgs e)
        {
            var but = (MenuItem)sender;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Presets.xml");

            if (!xmlDoc.HasChildNodes)
                return;

            XmlNode preset = xmlDoc.SelectSingleNode("Presets").SelectSingleNode(but.Tag.ToString());
            Console.WriteLine(preset.InnerXml);

            imageSpace.Children.Clear();

            foreach (XmlNode node in preset.ChildNodes)
            {
                double posX, posY;

                TransformGroup newTransformGroup = new TransformGroup();
                newTransformGroup.Children.Add(new RotateTransform());
                newTransformGroup.Children.Add(new ScaleTransform());
                newTransformGroup.Children.Add(new TranslateTransform());

                switch (node.Attributes["Type"].Value)
                {
                    case "Image":
                        System.Windows.Controls.Image image = new System.Windows.Controls.Image();

                        image.Name = node.Attributes["Name"].Value;
                        switch (node.Attributes["Name"].Value)
                        {
                            case "back":    back = image; break;
                            case "frame":   frame = image; break;
                        }

                        image.Height = int.Parse(node.Attributes["Height"].Value);
                        image.Width = int.Parse(node.Attributes["Width"].Value);
                        if (node.Attributes["Source"]?.Value != null)
                            image.Source = new BitmapImage(new Uri(node.Attributes["Source"].Value));
                        else
                            image.Source = null;

                        image.MouseUp += OnMouseUp;
                        image.MouseDown += OnMouseDown;
                        image.MouseMove += OnMouseDrag;
                        image.MouseWheel += OnMouseScroll;

                        image.RenderTransform = newTransformGroup;
                        image.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);

                        image.IsHitTestVisible = bool.Parse(node.Attributes["IsHitTestVisible"].Value);

                        switch (node.Attributes["Stretch"].Value)
                        {
                            case "Uniform":         image.Stretch = Stretch.Uniform; break;
                            case "Fill":            image.Stretch = Stretch.Fill; break;
                            case "None":            image.Stretch = Stretch.None; break;
                            case "UniformToFill":   image.Stretch = Stretch.UniformToFill; break;
                        }

                        image.VerticalAlignment = VerticalAlignment.Top;
                        image.HorizontalAlignment = HorizontalAlignment.Left;

                        imageSpace.Children.Add(image);

                        Canvas.SetZIndex(image, int.Parse(node.Attributes["ZIndex"].Value));

                        // Image position through transform
                        var transform = image.RenderTransform as TransformGroup;
                        TranslateTransform child = (TranslateTransform)transform.Children[2];

                        if (double.TryParse(node.Attributes["X"]?.Value, out posX))
                            child.X = posX;
                        if (double.TryParse(node.Attributes["Y"]?.Value, out posY))
                            child.Y = posY;


                        break;

                    case "TextBlock":
                        System.Windows.Controls.TextBlock tb = new System.Windows.Controls.TextBlock();
                        tb.Name = node.Attributes["Name"].Value;
                        tb.Text = node.Attributes["Text"].Value;
                        
                        tb.FontSize = int.Parse(node.Attributes["FontSize"].Value);
                        tb.FontFamily = new System.Windows.Media.FontFamily(node.Attributes["FontFamily"].Value);
                        tb.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(node.Attributes["Foreground"].Value));

                        tb.TextAlignment = TextAlignment.Center;
                        tb.TextWrapping = TextWrapping.Wrap;
                        tb.MaxWidth = 256;

                        imageSpace.Children.Add(tb);

                        tb.MouseUp += OnMouseUp;
                        tb.MouseDown += OnMouseDownTextBlock;
                        tb.MouseMove += OnMouseDrag;
                        tb.MouseWheel += OnMouseScroll;

                        tb.RenderTransform = newTransformGroup;
                        tb.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);

                        // Image position through transform
                        var transformtb = tb.RenderTransform as TransformGroup;
                        TranslateTransform childtb = (TranslateTransform)transformtb.Children[2];

                        if (double.TryParse(node.Attributes["X"]?.Value, out posX))
                            childtb.X = posX;
                        if (double.TryParse(node.Attributes["Y"]?.Value, out posY))
                            childtb.Y = posY;

                        break;

                    case "InkCanvas":
                        System.Windows.Controls.InkCanvas ic = new System.Windows.Controls.InkCanvas();
                        ic.Name = node.Attributes["Name"].Value;
                        inkCanvas = ic;
                        ic.Height = int.Parse(node.Attributes["Height"].Value);
                        ic.Width = int.Parse(node.Attributes["Width"].Value);

                        ic.PreviewMouseRightButtonDown += InkCanvas_PreviewMouseRightButtonDown;
                        ic.PreviewMouseRightButtonUp += InkCanvas_PreviewMouseRightButtonUp;

                        ic.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(node.Attributes["Background"].Value));
                        ic.IsHitTestVisible = bool.Parse(node.Attributes["IsHitTestVisible"].Value);

                        imageSpace.Children.Add(ic);

                        Canvas.SetZIndex(ic, int.Parse(node.Attributes["ZIndex"].Value));

                        break;
                }
            }
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
            textBlock.FontFamily = new System.Windows.Media.FontFamily(FontList.SelectedItem.ToString());

            UpdateAllTextBoxes(imageSpace);
        }

        private void FontColorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Media.Color newFontColor = (System.Windows.Media.Color)(FontColorList.SelectedItem as PropertyInfo).GetValue(this);
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
            System.Windows.Media.Color selectedColor = (System.Windows.Media.Color)(lineColorComboBox.SelectedItem as PropertyInfo).GetValue(this);

            if (inkCanvas != null)
                inkCanvas.DefaultDrawingAttributes.Color = selectedColor;
        }

        #endregion
    }
}
