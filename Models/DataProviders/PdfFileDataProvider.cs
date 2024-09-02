using DelitaTrade.Interfaces.DayReport;
using System;
using System.IO;
using System.Text;
using System.Text.Encodings;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using iTextSharp.text;


namespace DelitaTrade.Models.DataProviders
{
    public class PdfFileDataProvider : IFileDataProvider
    {   
        private string _data;

        public void ImportData(string filePath)
        {
            StringBuilder sb = new StringBuilder();
            PdfReader reader = new PdfReader(filePath);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);                    
            foreach (var encode in Encoding.GetEncodings())
            {
                sb.AppendLine($"---{encode.CodePage.ToString()}---");
                 //for (int page = 1; page <= reader.NumberOfPages; page++)
                 //{
                     ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
                     String s = PdfTextExtractor.GetTextFromPage(reader, 1, its);                     
                     sb.Append(Encoding.GetEncoding(encode.CodePage).GetString(Encoding.GetEncoding(encode.CodePage).GetBytes(s)));
                // }
                 
                 sb.AppendLine();                
                 sb.AppendLine("--------------------------------------------------------");
            }
            reader.Close();

            _data = sb.ToString();
            File.WriteAllText("../../../DayReportsDataBase/Output.txt", _data);
        }
       

    }
}
