using ArtsInChicago.Models;
using Microsoft.AspNetCore.Hosting;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ArtsInChicago.Helpers
{

    public class PdfPrinterMigradoc : IPdfPrinter
    {
        private const bool unicode = false;
        private const PdfFontEmbedding embedding = PdfFontEmbedding.Always;
        private readonly MigraDoc.DocumentObjectModel.Color defaultColor = MigraDoc.DocumentObjectModel.Color.FromCmyk(0, 0, 0, 100);
        private readonly IWebHostEnvironment environment;

        public PdfPrinterMigradoc(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public void OpenDoc(string fileName)
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

        public async Task<string> PrintIndividualArtwork(ArtworkDataFull model)
        {
            Document document = await CreateDocument(model);
            document.UseCmykColor = true;

            // Create a renderer for the MigraDoc document.
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode);

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;

            // Layout and render document to PDF
            pdfRenderer.RenderDocument();

            string fileName = $@"{AppDomain.CurrentDomain.BaseDirectory}\{Guid.NewGuid()}.pdf";

            pdfRenderer.PdfDocument.Save(fileName);

            return fileName;
        }



        #region Private methods
        /// <summary>
        /// Creates an absolutely minimalistic document.
        /// </summary>
        private async Task<Document> CreateDocument(ArtworkDataFull model)
        {
            // Create a new MigraDoc document
            Document document = new Document();

            // Add a section to the document
            Section section = document.AddSection();
            section.PageSetup.PageFormat = PageFormat.A4;
            section.PageSetup.LeftMargin = Unit.FromPoint(72);
            section.PageSetup.RightMargin = Unit.FromPoint(72);
            float sectionWidth = document.DefaultPageSetup.PageWidth - section.PageSetup.LeftMargin - section.PageSetup.RightMargin;

            string imageString = await GetImage(model.ImageUrl);
            Paragraph paragraph = section.AddParagraph();
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            var image = paragraph.AddImage(imageString);
            //var image = section.AddImage(imageString);
            image.Width = sectionWidth/2;
            image.LockAspectRatio = true;

            // Add a paragraph to the section
            //for (int i = 0; i < 100; i++)
            //{
            //    Paragraph paragraph = section.AddParagraph();

            //    paragraph.Format.Font.Color = this.defaultColor;
            //    paragraph.Format.Font.Bold = true;
            //    paragraph.Format.Alignment = ParagraphAlignment.Center;

            //    // Add some text to the paragraph
            //    paragraph.AddFormattedText($"{model.Description}{Environment.NewLine}");

            //    //paragraph.AddFormattedText($"{model.Title}");
            //    //paragraph.AddFormattedText($"{model.Artist}, {model.PlaceOfOrigin}, {model.Date}");

            //}
            return document;
        }

        private async Task<string> GetImage(string imageUrl)
        {
            // *** change it
            byte[] arr = new byte[] { };
            try
            {
                WebRequest request = WebRequest.Create(imageUrl);

                WebResponse response = await request.GetResponseAsync();
                Stream responseStream = response.GetResponseStream();
                //Image img = new Image().Fro
                System.Drawing.Image bmp = new Bitmap(responseStream);
                ImageConverter converter = new ImageConverter();
                arr = (byte[])converter.ConvertTo(bmp, typeof(byte[]));

            }
            catch (Exception)
            {
                string rootPath = this.environment.ContentRootPath;
                string imagePath = Path.Combine(rootPath, "wwwroot", "images", "PictureUanavailable.jpg");
                arr = File.ReadAllBytes(imagePath);
            }
            return "base64:" + Convert.ToBase64String(arr);
        }

        #endregion
    }
}
