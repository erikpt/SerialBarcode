using System.Management;
using BarcodeStandard;
using SkiaSharp;

namespace SerialBarcode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Serial Barcode Generator - " + Application.ProductVersion;

            string serialNumber = GetBiosSerialNumber();

            if (string.IsNullOrWhiteSpace(serialNumber) || serialNumber == "N/A")
            {
                MessageBox.Show("BIOS Serial Number not found. Exiting.");
                this.Close();
            }
            else
            {
                // Generate barcode and display in PictureBox
                Barcode barcode = new Barcode();
                barcode.IncludeLabel = true;
                barcode.LabelFont = new SkiaSharp.SKFont(SkiaSharp.SKTypeface.FromFamilyName("Arial"), 60, 1, 0); // Use SkiaSharp for font handling
                var barcodeImage = barcode.Encode(BarcodeStandard.Type.Code128, serialNumber, SkiaSharp.SKColor.Parse("000000"), SkiaSharp.SKColor.Parse("ffffff"), 600, 200);
                pictureBox1.Image = SKImageToBitmap(barcodeImage);
            }
        }

        private string GetBiosSerialNumber()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BIOS");
                foreach (ManagementObject bios in searcher.Get())
                {
                    return bios["SerialNumber"]?.ToString() ?? "N/A";
                }
            }
            catch
            {
                // Handle exceptions as needed
            }
            return "N/A";
        }


        public static Bitmap SKImageToBitmap(SKImage skImage)
        {
            using var data = skImage.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = new MemoryStream(data.ToArray());
            return new Bitmap(stream);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();   
        }
    }
}
