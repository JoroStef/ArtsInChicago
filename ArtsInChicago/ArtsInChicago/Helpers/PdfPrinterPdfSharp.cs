//using ArtsInChicago.Models;
//using Microsoft.AspNetCore.Hosting;
//using PdfSharp.Drawing;
//using PdfSharp.Drawing.BarCodes;
//using PdfSharp.Drawing.Layout;
//using PdfSharp.Pdf;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;

//namespace ArtsInChicago.Helpers
//{
//    public class PdfPrinter
//    {
//        private const double imgScale = 0.5;
//        private const double leftMargin = 100;
//        private const double rightMargin = 50;
//        private readonly IWebHostEnvironment environment;
//        XFont xFontRegular;
//        XFont xFontBold;

//        public PdfPrinter(IWebHostEnvironment environment)
//        {
//            this.environment = environment;

//            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
//            this.xFontRegular = new XFont("Times New Roman", 12, XFontStyle.Regular);
//            this.xFontBold = new XFont("Times New Roman", 12, XFontStyle.Bold);

//        }

//        public async Task<string> PrintIndividualArtwork(ArtworkDataFull model)
//        {
//            var doc = new PdfDocument();
//            AddPage(doc, out PdfPage page);

//            XGraphics gfx = XGraphics.FromPdfPage(page);

//            XImage image = await GetImage(model.ImageUrl);

//            XUnit currentY = XUnit.FromPoint(0);

//            //DrawImage(page, gfx, image, ref currentY);

//            //WriteText($"{model.Title}", page, gfx, this.xFontBold, ref currentY, XParagraphAlignment.Center);
//            //WriteText($"{model.Artist}, {model.PlaceOfOrigin}, {model.Date}", page, gfx, this.xFontBold, ref currentY, XParagraphAlignment.Center);
//            //WriteText("Medium:", page, gfx, this.xFontBold, ref currentY, XParagraphAlignment.Left);
//            //WriteText($"{model.Medium}", page, gfx, this.xFontRegular, ref currentY, XParagraphAlignment.Left);
//            //WriteText("Dimentions:", page, gfx, this.xFontBold, ref currentY, XParagraphAlignment.Left);
//            //WriteText($"{model.Dimentions}", page, gfx, this.xFontRegular, ref currentY, XParagraphAlignment.Left);
//            //WriteText("Description:", page, gfx, this.xFontBold, ref currentY, XParagraphAlignment.Left);
//            WriteText($"{model.Description}", page, gfx, this.xFontRegular, ref currentY, XParagraphAlignment.Left);

//            string fileName = $@"{AppDomain.CurrentDomain.BaseDirectory}\{Guid.NewGuid()}.pdf";

//            doc.Save(fileName);

//            return fileName;

//        }

//        private void AddPage(PdfDocument doc, out PdfPage page)
//        {
//            page = doc.AddPage();
//            page.Size = PdfSharp.PageSize.A4;
//            page.Orientation = PdfSharp.PageOrientation.Portrait;
//            page.TrimMargins.All = XUnit.FromPoint(72);

//        }

//        private async Task<XImage> GetImage(string imageUrl)
//        {
//            // *** change it
//            XImage image;
//            try
//            {
//                WebRequest request = WebRequest.Create(imageUrl);

//                WebResponse response = await request.GetResponseAsync();
//                Stream responseStream = response.GetResponseStream();

//                image = XImage.FromStream(responseStream);

//            }
//            catch (Exception)
//            {
//                string rootPath = this.environment.ContentRootPath;
//                string imagePath = Path.Combine(rootPath, "wwwroot", "images", "PictureUanavailable.jpg");
//                image = XImage.FromFile(imagePath);
//            }

//            return image;
//        }

//        private void DrawImage(PdfPage page, XGraphics gfx, XImage image, ref XUnit currentY)
//        {

//            XUnit pageClearWidth = page.Width - page.TrimMargins.Left - page.TrimMargins.Right;

//            XUnit imageX = page.TrimMargins.Left + pageClearWidth / 2 - imgScale * image.PointWidth / 2;
//            XUnit imageY = currentY;
//            gfx.DrawImage(image, imageX, imageY, image.PointWidth * imgScale, image.PointHeight * imgScale);

//            currentY += image.PointHeight * imgScale + XUnit.FromPoint(72 / 2);

//        }

//        private XUnit GetTextRectangleHeight(string text, XGraphics gfx, XFont font, XUnit rectWidth)
//        {
//            XSize size = gfx.MeasureString(text, font);

//            double height = size.Height * (Math.Ceiling(size.Width / rectWidth) + 1);

//            return height;

//        }

//        private void WriteText(string text, PdfPage page, XGraphics gfx, XFont font, ref XUnit currentY, XParagraphAlignment textAlignment)
//        {
//            XUnit pageClearWidth = page.Width - page.TrimMargins.Left - page.TrimMargins.Right;

//            XSize size = gfx.MeasureString(text, font);

//            XUnit rectHeight = XUnit.FromPoint(size.Height * (Math.Ceiling(size.Width / pageClearWidth)));
//            rectHeight /= 2;

//            //XUnit rectBottom = currentY + rectHeight;

//            //if (rectBottom > page.Height - page.TrimMargins.Bottom)
//            //{
//            //    rectHeight = size.Height * Math.Floor(page.Height - page.TrimMargins.Bottom);
//            //}

//            XRect rect = new XRect(page.TrimMargins.Left, currentY, pageClearWidth, rectHeight);

//            gfx.DrawRectangle(XPens.Black, rect);
//            CodeDataMatrix dm = new CodeDataMatrix("test", 26);

//            var tf = new XTextFormatter(gfx);
//            tf.Alignment = textAlignment;
//            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);
//            string writtenText = tf.Text;

//            currentY += rectHeight;
//        }

//        private List<int> GetPageBrakes(string text, PdfPage page, XGraphics gfx, XFont font, ref XUnit currentY)
//        {
//            XUnit pageClearWidth = page.Width - page.TrimMargins.Left - page.TrimMargins.Right;

//            XSize size = gfx.MeasureString(text, font);

//            string[] parts = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
//            StringBuilder sb = new StringBuilder();

//            // start from assigned place on the current page
//            double rectEquivalentLength = (Math.Floor(page.Height - page.TrimMargins.Bottom - currentY)/size.Height) * page.Width;

//            List<int> pageBrakes = new List<int>();
//            int i = 0;
//            while (i < parts.Length)
//            {
//                string part = $"{parts[i]} ";
//                sb.Append(part);
//                if (sb.Length > rectEquivalentLength)
//                {
//                    sb.Remove(sb.Length - part.Length, part.Length);
//                    pageBrakes.Add(sb.Length);
//                    i--;
//                    // procede next pages with rect 
//                    rectEquivalentLength = (Math.Floor(page.Height - page.TrimMargins.Top - page.TrimMargins.Bottom) / size.Height) * page.Width;
//                    continue;
//                }
//                i++;
//            }
//        }

//        #region Not used
//        //private static string ComposeTextHeading(ArtworkDataFull model)
//        //{
//        //    StringBuilder sb = new StringBuilder();

//        //    sb.AppendLine($"{model.Title}, {model.Date}");
//        //    sb.AppendLine();
//        //    sb.AppendLine($"{model.Artist}, {model.PlaceOfOrigin}");

//        //    return sb.ToString().Trim();
//        //}

//        //private static string ComposeTextParagraph(ArtworkDataFull model)
//        //{
//        //    StringBuilder sb = new StringBuilder();

//        //    sb.AppendLine("Medium: ");
//        //    sb.AppendLine(model.Medium);
//        //    sb.AppendLine();
//        //    sb.AppendLine("Dimentions: ");
//        //    sb.AppendLine(model.Dimentions);
//        //    sb.AppendLine();
//        //    sb.AppendLine("Description: ");
//        //    sb.AppendLine(model.Description);

//        //    return sb.ToString().Trim();
//        //}

//        //private static string ComposeTextLeft(ArtworkDataFull model)
//        //{
//        //    StringBuilder sb = new StringBuilder();

//        //    sb.AppendLine("Medium: ");
//        //    sb.AppendLine(model.Medium);
//        //    sb.AppendLine();
//        //    sb.AppendLine("Dimentions: ");
//        //    sb.AppendLine(model.Dimentions);

//        //    return sb.ToString().Trim();
//        //}

//        //private static string ComposeTextRight(ArtworkDataFull model)
//        //{
//        //    StringBuilder sb = new StringBuilder();

//        //    sb.AppendLine("Description: ");
//        //    sb.AppendLine(model.Description);

//        //    return sb.ToString().Trim();
//        //}

//        #endregion

//        public static void OpenDoc(string fileName)
//        {
//            using (Process p = new Process())
//            {
//                p.StartInfo = new ProcessStartInfo()
//                {
//                    CreateNoWindow = true,
//                    Verb = "open",
//                    UseShellExecute = true,
//                    FileName = fileName,
//                    Arguments = ""
//                };

//                p.Start();

//            }
//        }
//    }
//}
