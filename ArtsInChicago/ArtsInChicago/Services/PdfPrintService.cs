using ArtsInChicago.Models;
using ArtsInChicago.Services.Cotracts;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ArtsInChicago.Services
{

    public class PdfPrintService : IPdfPrintService
    {
        private const bool unicode = false;
        private readonly Color defaultColor = Color.FromCmyk(0, 0, 0, 100);
        private readonly Font defaultFont_12 = new Font("Times new roman", 12);
        private readonly Font defaultFont_16 = new Font("Times new roman", 16);
        private readonly Unit defaultSpaceAfter = Unit.FromPoint(72/4);

        public PdfPrintService()
        {
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

        public string PrintIndividualArtwork(ArtworkDataFull model, string imageString)
        {
            Document document = CreateDocument(model, imageString);
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
        private Document CreateDocument(ArtworkDataFull model, string imageString)
        {
            // Create a new MigraDoc document
            Document document = new Document();

            // Add a section to the document
            Section section = document.AddSection();
            section.PageSetup.PageFormat = PageFormat.A4;
            section.PageSetup.Orientation = Orientation.Portrait;
            section.PageSetup.LeftMargin = Unit.FromPoint(72);
            section.PageSetup.RightMargin = Unit.FromPoint(72);
            section.PageSetup.TopMargin = Unit.FromPoint(72);
            section.PageSetup.BottomMargin = Unit.FromPoint(72);
            float sectionWidth = document.DefaultPageSetup.PageWidth - section.PageSetup.LeftMargin - section.PageSetup.RightMargin;

            // Add image
            Paragraph paragraph = section.AddParagraph();
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            var image = paragraph.AddImage(imageString);
            image.Width = sectionWidth/2;
            image.LockAspectRatio = true;
            paragraph.Format.SpaceAfter = this.defaultSpaceAfter;

            // Add heading
            paragraph = section.AddParagraph();
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Format.Font.ApplyFont(this.defaultFont_16);
            paragraph.Format.Font.Color = this.defaultColor;
            paragraph.Format.Font.Bold = true;
            paragraph.AddFormattedText($"{model.Title}");
            paragraph.AddLineBreak();
            paragraph.AddFormattedText($"{model.Artist}, {model.PlaceOfOrigin}, {model.Date}");
            paragraph.Format.SpaceAfter = this.defaultSpaceAfter;

            // Add remaining descriptions
            Dictionary<string, string> modelDescriptions = PrepareDescriptions(model);

            foreach (var item in modelDescriptions)
            {
                paragraph = section.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.Format.Font.ApplyFont(this.defaultFont_12);
                paragraph.Format.Font.Color = this.defaultColor;
                paragraph.Format.Font.Bold = true;
                paragraph.AddFormattedText($"{item.Key}:");
                paragraph.AddLineBreak();

                paragraph = section.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.Format.Font.ApplyFont(this.defaultFont_12);
                paragraph.Format.Font.Color = this.defaultColor;
                paragraph.Format.Font.Bold = false;
                paragraph.AddFormattedText(item.Value);

                paragraph.Format.SpaceAfter = this.defaultSpaceAfter;

            }

            return document;
        }

        private Dictionary<string,string> PrepareDescriptions(ArtworkDataFull model)
        {
            Dictionary<string, string> modelDescriptions = new Dictionary<string, string>();

            modelDescriptions.Add("Medium", model.Medium);
            modelDescriptions.Add("Dimentions", model.Dimentions);
            modelDescriptions.Add("Description", model.Description);

            return modelDescriptions;
        }

        #endregion
    }
}
