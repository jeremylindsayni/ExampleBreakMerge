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

                var pages = new List<byte[]>();
                foreach (var filename in fileNames)
                {
                    pages.Add(File.ReadAllBytes(filename));
                }

                var mergedArray = MergePages(pages);

                if (i == 0)
                {
                    sizeOfMergedFileFirstTimeRound = mergedArray.Length;
                }

                if (mergedArray.Length == sizeOfMergedFileFirstTimeRound)
                {
                    Console.WriteLine("worked - size = " + mergedArray.Length);
                }
                else
                {
                    Console.WriteLine("odd size - size = " + mergedArray.Length + " (i = " + i + ")");
                }
            }
        }

        public static byte[] MergePages(List<byte[]> pages)
        {
            byte[] parentDocumentContents = Array.Empty<byte>();

            using (var parentStream = new MemoryStream())
            {
                using (var document = new PdfDocument())
                {
                    foreach (var pdfByteArray in pages)
                    {
                        using (var pdfStream = new MemoryStream())
                        {
                            pdfStream.Write(pdfByteArray, 0, pdfByteArray.Length);

                            using (var source = PdfDocument.Load(pdfStream))
                            {
                                document.Pages.AddClone(source.Pages[0]);
                            }
                        }
                    }

                    document.Save(parentStream);
                    parentDocumentContents = parentStream.ToArray();
                }
            }

            return parentDocumentContents;
        }
    }
}
