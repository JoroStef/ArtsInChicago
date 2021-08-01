using ArtsInChicago.Models;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ArtsInChicago.Helpers
{

    public class PdfPrinterMigradoc : IPdfPrinter
    {
        private const bool unicode = false;
        private const PdfFontEmbedding embedding = PdfFontEmbedding.Always;
        private readonly Color defaultColor = Color.FromCmyk(0, 0, 0, 100);

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
            Document document = CreateDocument(model);
            document.UseCmykColor = true;

            // Create a renderer for the MigraDoc document.
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode, embedding);

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
        private Document CreateDocument(ArtworkDataFull model)
        {
            // Create a new MigraDoc document
            Document document = new Document();

            // Add a section to the document
            Section section = document.AddSection();

            // Add a paragraph to the section
            Paragraph paragraph = section.AddParagraph();

            paragraph.Format.Font.Color = this.defaultColor;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            // Add some text to the paragraph
            paragraph.AddFormattedText($"{model.Title}");
            paragraph.AddFormattedText($"{model.Artist}, {model.PlaceOfOrigin}, {model.Date}");

            return document;
        }

        #endregion    
    }
}
