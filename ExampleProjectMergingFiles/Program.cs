using GemBox.Pdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExampleProjectMergingFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");

            var sizeOfMergedFileFirstTimeRound = 0;

            for (int i = 0; i < 100; i++)
            {
                // List of source file names.
                var fileNames = new string[]
                {
                    @".\PDFs\hello-1.pdf",
                    @".\PDFs\hello-2.pdf"
                };

                // load the source files into a list of byte arrays
                var pages = new List<byte[]>();
                foreach (var filename in fileNames)
                {
                    pages.Add(File.ReadAllBytes(filename));
                }

                // merge the list of byte arrays into a single byte array
                var mergedArray = MergePages(pages);

                // take a note of the size of the merged array the first time we go through the loop
                if (i == 0)
                {
                    sizeOfMergedFileFirstTimeRound = mergedArray.Length;
                }

                // compare the size of the merged file byte array with what it was the first time through the loop
                // would expect the byte array sizes to always be the same because we're always running the same code.
                if (mergedArray.Length == sizeOfMergedFileFirstTimeRound)
                {
                    Console.WriteLine("worked - size = " + mergedArray.Length);
                }
                else
                {
                    // but 5% of the time, the byte array of the merged files is 2 bytes smaller
                    Console.WriteLine("odd size - size = " + mergedArray.Length + " (i = " + i + ")");
                }
            }
        }

        public static byte[] MergePages(List<byte[]> pdfByteArrays)
        {
            byte[] parentDocumentContents = Array.Empty<byte>();

            using (var mergedStream = new MemoryStream())
            {
                using (var mergedDocument = new PdfDocument())
                {
                    foreach (var pdfByteArray in pdfByteArrays)
                    {
                        using (var pdfStream = new MemoryStream())
                        {
                            pdfStream.Write(pdfByteArray, 0, pdfByteArray.Length);

                            using (var source = PdfDocument.Load(pdfStream))
                            {
                                // in this sample each file is only one page long, so it's ok to add the first page of the source PDF document
                                mergedDocument.Pages.AddClone(source.Pages[0]);
                            }
                        }
                    }

                    mergedDocument.Save(mergedStream);
                    parentDocumentContents = mergedStream.ToArray();
                }
            }

            return parentDocumentContents;
        }
    }
}
