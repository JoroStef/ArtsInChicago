//using ArtsInChicago.Models;
//using PdfSharp.Drawing;
//using PdfSharp.Drawing.Layout;
//using PdfSharp.Pdf;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;

using ArtsInChicago.Models;
using Microsoft.AspNetCore.Hosting;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ArtsInChicago.Helpers
{
    public class PdfPrinter
    {
        private const double imgScale = 0.5;
        private const double leftMargin = 100;
        private const double rightMargin = 50;
        private readonly IWebHostEnvironment environment;

        public PdfPrinter(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public async Task<string> PrintIndividualArtwork(ArtworkDataFull model)
        {

            var doc = new PdfDocument();
            var page = doc.AddPage();

            page.Size = PdfSharp.PageSize.A4;
            page.Orientation = PdfSharp.PageOrientation.Portrait;
            var gfx = XGraphics.FromPdfPage(page);

            double pageClearWidth = page.Width - leftMargin - rightMargin;

            // *** change it
            XImage image;
            try
            {
                WebRequest request = WebRequest.Create(model.ImageUrl);

                WebResponse response = await request.GetResponseAsync();
                Stream responseStream = response.GetResponseStream();

                image = XImage.FromStream(responseStream);

            }
            catch (Exception)
            {
                string rootPath = this.environment.ContentRootPath;
                string imagePath = Path.Combine(rootPath, "wwwroot", "images", "PictureUanavailable.jpg");
                image = XImage.FromFile(imagePath);
            }

            double imageX = leftMargin + pageClearWidth / 2 - imgScale * image.PointWidth / 2;
            double imageY = 50f;
            gfx.DrawImage(image, imageX, imageY, image.PointWidth * imgScale, image.PointHeight * imgScale);

            var tf = new XTextFormatter(gfx);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var xFontRegular = new XFont("Times New Roman", 12, XFontStyle.Regular);
            var xFontBold = new XFont("Times New Roman", 12, XFontStyle.Bold);

            double rectTop = imageY + imgScale * image.PointHeight + 20;

            XRect rectCenter = new XRect(leftMargin, rectTop, pageClearWidth, 80);
            XRect rectLeft = new XRect(leftMargin, rectTop + 80, pageClearWidth / 2 - 5, 300);
            XRect rectRight = new XRect(leftMargin + pageClearWidth / 2 + 10, rectTop + 80, pageClearWidth / 2 - 5, 300);

            tf.Alignment = XParagraphAlignment.Center;
            tf.DrawString(ComposeTextCenter(model), xFontBold, XBrushes.Black, rectCenter, XStringFormats.TopLeft);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(ComposeTextLeft(model), xFontRegular, XBrushes.Black, rectLeft, XStringFormats.TopLeft);
            tf.DrawString(ComposeTextRight(model), xFontRegular, XBrushes.Black, rectRight, XStringFormats.TopLeft);

            string fileName = $@"{AppDomain.CurrentDomain.BaseDirectory}\{Guid.NewGuid()}.pdf";

            doc.Save(fileName);

            return fileName;

        }

        private static string ComposeTextCenter(ArtworkDataFull model)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{model.Title}, {model.Date}");
            sb.AppendLine();
            sb.AppendLine($"{model.Artist}, {model.PlaceOfOrigin}");

            return sb.ToString().Trim();
        }

        private static string ComposeTextLeft(ArtworkDataFull model)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Medium: ");
            sb.AppendLine(model.Medium);
            sb.AppendLine();
            sb.AppendLine("Dimentions: ");
            sb.AppendLine(model.Dimentions);

            return sb.ToString().Trim();
        }

        private static string ComposeTextRight(ArtworkDataFull model)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Description: ");
            sb.AppendLine(model.Description);

            return sb.ToString().Trim();
        }

        public static void OpenDoc(string fileName)
        {
            using (Process p = new Process())
            {
                p.StartInfo = new ProcessStartInfo()
                {
                    CreateNoWindow = true,
                    Verb = "open",
                    UseShellExecute = true,
                    FileName = fileName,
                    Arguments = ""
                };

                p.Start();

            }
        }
    }
}
