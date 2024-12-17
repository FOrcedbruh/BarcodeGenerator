using System;
using System.Windows;
using System.Windows.Media.Imaging;
using ZXing;
using Microsoft.Win32;
using System.IO;

namespace BarcodeGenerator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            string inputText = InputTextBox.Text;

            if (string.IsNullOrWhiteSpace(inputText))
            {
                MessageBox.Show("Введите текст для генерации штрих-кода.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                
                var barcodeWriter = new BarcodeWriter
                {
                    Format = BarcodeFormat.CODE_128,
                    Options = new ZXing.Common.EncodingOptions
                    {
                        Height = 150, 
                        Width = 150,  
                        Margin = 10  
                    }
                };

               
                var barcodeBitmap = barcodeWriter.Write(inputText);

                
                BarcodeImage.Source = ConvertBitmapToBitmapImage(barcodeBitmap);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка генерации штрих-кода: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Выберите текстовый файл"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string fileContent = File.ReadAllText(openFileDialog.FileName);
                    InputTextBox.Text = fileContent;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка чтения файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private BitmapImage ConvertBitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            using (var memory = new System.IO.MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            InputTextBox.Text = "";
            BarcodeImage.Source = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(InputTextBox.Text))
            {
                MessageBox.Show("Введите текст для генерации штрих-кода.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 150,
                    Height = 150,
                    Margin = 0
                }
            };

            var bitmap = barcodeWriter.Write(InputTextBox.Text);
            var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            BarcodeImage.Source = bitmapSource;
        }

        private void reverse_Click(object sender, RoutedEventArgs e)
        {
            rotate.Angle += 90;
        }


        private void Button_ClickGenerate(object sender, RoutedEventArgs e)
        {
            string inputText = InputTextBox.Text;

            if (string.IsNullOrWhiteSpace(inputText))
            {
                MessageBox.Show("Введите текст для генерации штрих-кода.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {

                var barcodeWriter = new BarcodeWriter
                {
                    Format = BarcodeFormat.DATA_MATRIX,
                    Options = new ZXing.Common.EncodingOptions
                    {
                        Height = 150,
                        Width = 150,
                        Margin = 10
                    }
                };


                var barcodeBitmap = barcodeWriter.Write(inputText);


                BarcodeImage.Source = ConvertBitmapToBitmapImage(barcodeBitmap);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка генерации штрих-кода: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
