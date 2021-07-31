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
            page.TrimMargins.All = XUnit.FromPoint(72);

            var gfx = XGraphics.FromPdfPage(page);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var xFontRegular = new XFont("Times New Roman", 12, XFontStyle.Regular);
            var xFontBold = new XFont("Times New Roman", 12, XFontStyle.Bold);

            XImage image = await GetImage(model.ImageUrl);

            XUnit currentY = XUnit.FromPoint(0);

            DrawImage(page, image, gfx, ref currentY);

            WriteText($"{model.Title}", page, xFontBold, gfx, ref currentY, XParagraphAlignment.Center);
            WriteText($"{model.Artist}, {model.PlaceOfOrigin}, {model.Date}", page, xFontBold, gfx, ref currentY, XParagraphAlignment.Center);
            WriteText("Medium:", page, xFontBold, gfx, ref currentY, XParagraphAlignment.Left);
            WriteText($"{model.Medium}", page, xFontRegular, gfx, ref currentY, XParagraphAlignment.Left);
            WriteText("Dimentions:", page, xFontBold, gfx, ref currentY, XParagraphAlignment.Left);
            WriteText($"{model.Dimentions}", page, xFontRegular, gfx, ref currentY, XParagraphAlignment.Left);
            WriteText("Description:", page, xFontBold, gfx, ref currentY, XParagraphAlignment.Left);
            WriteText($"{model.Description}", page, xFontRegular, gfx, ref currentY, XParagraphAlignment.Left);


            string fileName = $@"{AppDomain.CurrentDomain.BaseDirectory}\{Guid.NewGuid()}.pdf";

            doc.Save(fileName);

            return fileName;

        }

        private async Task<XImage> GetImage(string imageUrl)
        {
            // *** change it
            XImage image;
            try
            {
                WebRequest request = WebRequest.Create(imageUrl);

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

            return image;
        }

        private void DrawImage(PdfPage page, XImage image, XGraphics gfx, ref XUnit currentY)
        {
            XUnit pageClearWidth = page.Width - page.TrimMargins.Left - page.TrimMargins.Right;

            XUnit imageX = page.TrimMargins.Left + pageClearWidth / 2 - imgScale * image.PointWidth / 2;
            XUnit imageY = currentY;
            gfx.DrawImage(image, imageX, imageY, image.PointWidth * imgScale, image.PointHeight * imgScale);

            currentY += image.PointHeight * imgScale + XUnit.FromPoint(72/2);

        }

        private XUnit GetTextRectangleHeight(string text, XGraphics gfx, XFont font, XUnit rectWidth)
        {
            XSize size = gfx.MeasureString(text, font);

            double height = size.Height * (Math.Ceiling(size.Width / rectWidth) + 1);

            return height;

        }

        private void WriteText(string text, PdfPage page, XFont font, XGraphics gfx, ref XUnit currentY, XParagraphAlignment textAlignment)
        {
            XUnit pageClearWidth = page.Width - page.TrimMargins.Left - page.TrimMargins.Right;
            XUnit rectHeight = GetTextRectangleHeight(text, gfx, font, pageClearWidth);

            XRect rect = new XRect(page.TrimMargins.Left, currentY, pageClearWidth, rectHeight);

            var tf = new XTextFormatter(gfx);

            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            tf.Alignment = textAlignment;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            currentY += rectHeight;
        }

        //private static string ComposeTextHeading(ArtworkDataFull model)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    sb.AppendLine($"{model.Title}, {model.Date}");
        //    sb.AppendLine();
        //    sb.AppendLine($"{model.Artist}, {model.PlaceOfOrigin}");

        //    return sb.ToString().Trim();
        //}

        //private static string ComposeTextParagraph(ArtworkDataFull model)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    sb.AppendLine("Medium: ");
        //    sb.AppendLine(model.Medium);
        //    sb.AppendLine();
        //    sb.AppendLine("Dimentions: ");
        //    sb.AppendLine(model.Dimentions);
        //    sb.AppendLine();
        //    sb.AppendLine("Description: ");
        //    sb.AppendLine(model.Description);

        //    return sb.ToString().Trim();
        //}

        //private static string ComposeTextLeft(ArtworkDataFull model)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    sb.AppendLine("Medium: ");
        //    sb.AppendLine(model.Medium);
        //    sb.AppendLine();
        //    sb.AppendLine("Dimentions: ");
        //    sb.AppendLine(model.Dimentions);

        //    return sb.ToString().Trim();
        //}

        //private static string ComposeTextRight(ArtworkDataFull model)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    sb.AppendLine("Description: ");
        //    sb.AppendLine(model.Description);

        //    return sb.ToString().Trim();
        //}

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
