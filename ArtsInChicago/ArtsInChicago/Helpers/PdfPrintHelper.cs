using ArtsInChicago.Models;
using ArtsInChicago.Services.Cotracts;
using Microsoft.AspNetCore.Hosting;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ArtsInChicago.Helpers
{

    public static class PdfPrintHelper
    {
        public static async Task<string> GetImageBase64String(string imageUrl, IWebHostEnvironment environment)
        {
            byte[] arr = new byte[] { };
            try
            {
                WebRequest request = WebRequest.Create(imageUrl);

                WebResponse response = await request.GetResponseAsync();
                Stream responseStream = response.GetResponseStream();
                //Image img = new Image().Fro
                Image bmp = new Bitmap(responseStream);
                ImageConverter converter = new ImageConverter();
                arr = (byte[])converter.ConvertTo(bmp, typeof(byte[]));

            }
            catch (Exception)
            {
                string rootPath = environment.ContentRootPath;
                string imagePath = Path.Combine(rootPath, "wwwroot", "images", "PictureUanavailable.jpg");
                arr = File.ReadAllBytes(imagePath);
            }
            return "base64:" + Convert.ToBase64String(arr);
        }
    }
}
