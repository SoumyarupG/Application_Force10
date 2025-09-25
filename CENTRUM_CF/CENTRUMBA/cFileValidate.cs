using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using System.IO;

namespace CENTRUMBA
{
   public class cFileValidate
    {
       public bool ValidatePdf(byte [] pFileStream)
       {
          // string pdfPath = "C:/Users/priti.s/Desktop/FileUpload/test.pdf";            
           try
           {
               var reader = new PdfReader(pFileStream);
               var names = reader.Catalog.GetAsDict(PdfName.NAMES);
               if (names == null)
               {
                   //Label1.Text = "No embedded files found.";
                   return true;
               }
               var embeddedFiles = names.GetAsDict(PdfName.EMBEDDEDFILES);
               var filespecs = embeddedFiles.GetAsArray(PdfName.NAMES);
               for (int i = 0; i < filespecs.Size; i += 2)
               {
                   var fileObj = PdfReader.GetPdfObject(filespecs[i + 1]);
                   //if (fileObj is not PdfDictionary filespec)
                   PdfDictionary filespec = fileObj as PdfDictionary;
                   if (filespec == null)
                   {
                      // Label1.Text = "Skipping: Embedded file is not a dictionary.";
                       continue;
                   }

                   var ef = filespec.GetAsDict(PdfName.EF);
                   if (ef == null)
                   {
                       //Label1.Text = "Skipping: No EF (embedded file) dictionary.";
                       continue;
                   }
                   var fileStream = (PRStream)PdfReader.GetPdfObject(ef.GetAsIndirectObject(PdfName.F));
                   var bytes = PdfReader.GetStreamBytes(fileStream);
                   string content = Encoding.ASCII.GetString(bytes);
                   //Console.WriteLine("Extracted file content:\n");
                   //Console.WriteLine(content);
                   string eicar = "X5O!P%@AP[4\\PZX54(P^)7CC)7}$EICAR-STANDARD-ANTIVIRUS-TEST-FILE!$H+H*";
                   if (content.Contains(eicar))
                   {
                       //Label1.Text = "EICAR string found in attachment.";
                       return false;
                   }
                   else
                   {
                       //Label1.Text = "EICAR string NOT found.";
                       return true;
                   }
               }
               return false;
           }
           catch
           {
               //Label1.Text = "EICAR string found in attachment.";
               return false;
           }
       }
       
    }
}
