using Aspose.Pdf;
using Aspose.Pdf.Facades;
using Aspose.Pdf.Operators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PDF_stamp_remove
{
    internal class Program
    {

        static void Main(string[] args)
        {

            try
            {

                string docPath = @"C:\Users\VP649NY\Downloads\Watermark";

                List<string> dirs = new List<string>(Directory.EnumerateDirectories(docPath));

                if (dirs.Count == 0)
                {
                    getAllFilesInaDirectory(docPath);
                }
                else
                {

                    foreach (var dir in dirs)
                    {
                        Console.WriteLine($"{dir.Substring(dir.LastIndexOf(Path.DirectorySeparatorChar) + 1)}");
                        getAllFilesInaDirectory(dir);
                    }
                }
                Console.WriteLine($"{dirs.Count} directories found.");
                Console.Read();
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (PathTooLongException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        static void getAllFilesInaDirectory(string docPath)
        {
            try
            {


                var files = from file in Directory.EnumerateFiles(docPath, "*.pdf")
                            select new
                            {
                                File = file
                            };

                foreach (var f in files)
                {
                    Console.WriteLine($"{f.File}");
                    removeWatermark(f.File);
                }
                Console.WriteLine($"{files.Count().ToString()} files found.");
            }
            catch (UnauthorizedAccessException uAEx)
            {
                Console.WriteLine(uAEx.Message);
            }
            catch (PathTooLongException pathEx)
            {
                Console.WriteLine(pathEx.Message);
            }
        }

        static void removeWatermark(string filePath)
        {
            // Initialize license
            Aspose.Pdf.License lic = new Aspose.Pdf.License();
            //lic.SetLicense("Aspose.Total.lic");
            const string ASPOSE_LICENSE_PUBLIC_KEY = "";
            const string ASPOSE_LICENSE_PRIVATE_KEY = ""; new Aspose.Pdf.Metered().SetMeteredKey(ASPOSE_LICENSE_PUBLIC_KEY, ASPOSE_LICENSE_PRIVATE_KEY);
            //new Aspose.Cells.Metered().SetMeteredKey(ASPOSE_LICENSE_PUBLIC_KEY, ASPOSE_LICENSE_PRIVATE_KEY);
            //new Aspose.Words.Metered().SetMeteredKey(ASPOSE_LICENSE_PUBLIC_KEY, ASPOSE_LICENSE_PRIVATE_KEY);

            // Create PDF content editor.
            Aspose.Pdf.Facades.PdfContentEditor contentEditor = new Aspose.Pdf.Facades.PdfContentEditor();
            // Open the temp file.
            contentEditor.BindPdf(filePath);

            // Process all pages.
            foreach (Page page in contentEditor.Document.Pages)
            {
                // Get the stamp infos.
                Aspose.Pdf.Facades.StampInfo[] stampInfos = contentEditor.GetStamps(page.Number);

                int[] index = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                // Process all stamp infos
                foreach (Aspose.Pdf.Facades.StampInfo stampInfo in stampInfos)
                {
                    try
                    {
                        contentEditor.DeleteStamp(page.Number, index);
                        //contentEditor.HideStampById(page.Number, index);
                        //contentEditor.DeleteStampById(stampInfo.StampId);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            string updatedDirectory = Path.GetDirectoryName(filePath) + @"\Watermark removed";
            if (!Directory.Exists(updatedDirectory))
            {
                Directory.CreateDirectory(updatedDirectory);
            }
            string updatedFileName = Path.GetFileName(filePath);


            // Save changes to a file.
            contentEditor.Save(Path.Combine(updatedDirectory, updatedFileName));
        }
    }
}
