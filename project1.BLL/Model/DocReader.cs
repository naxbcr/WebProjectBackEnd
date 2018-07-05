using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Word;

namespace project1.BLL.Model
{
    public class DocReader
    {
        public static int ReadDoc(string filename)
        {
            List<string> list = new List<string>();
            Application application = new Application();
            Document document = application.Documents.Open(AppDomain.CurrentDomain.BaseDirectory + "App_Data/UploadedFiles/" + filename);
            //Document document = application.Documents.Open(@"C:\Users\Maks\Documents\Visual Studio 2015\Projects\ConsoleApplication2\ConsoleApplication2\UploadedFiles\test2.docx");

            foreach (Paragraph paragraph in document.Paragraphs)
            {
                //list.Add(paragraph.Range.Text);
                list.Add(paragraph.Range.Text.Replace("\r", "").Replace("\a", "").Replace("\t", "").Replace("  ", "").Replace(":", "").Replace(";", "").Replace("-", "").Replace(",", "").Replace("–", ""));

            }
            ((_Application)application).Quit();
            application.Quit();

            string cleardata = null;
            for (int i = 0; i < list.Count(); i++)
            {
                cleardata += list[i] + " ";
            }
            string[] data = cleardata.Split(new Char[] { ' ' });

            data = data.Where(val => val != "\t" && val != " ").ToArray();

            return data.Length;
        }
    }
}