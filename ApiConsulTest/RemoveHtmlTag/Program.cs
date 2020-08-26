using System;
using System.IO;
using System.Linq;

namespace RemoveHtmlTag
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Directory.GetCurrentDirectory();
           

            DirectoryInfo di = new DirectoryInfo(path);
            var filePath = di.Parent.Parent.Parent.FullName;
            filePath += "\\otomeTrim\\";
            var newPathFilePath = di.Parent.Parent.Parent.FullName + "\\newotomeTrim\\";
            var files = Directory.GetFiles(filePath);

            for (int i = 0; i < files.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("File path = > " + files[i]);
                Console.ResetColor();
                var fullTextString = File.ReadAllText(files[i]);
                fullTextString = StripTagsCharArray(fullTextString);
                fullTextString = " <div style=\"word-wrap: normal;width:90%;\">" + fullTextString + " </div> ";

                if (!Directory.Exists(newPathFilePath))
                { 
                    Directory.CreateDirectory(newPathFilePath);
                }

                string newFilePath = newPathFilePath + "chap_" + i + ".html";
                if (!File.Exists(newFilePath))
                {
                    using (var tw = new StreamWriter(newFilePath, true))
                    {
                        tw.WriteLine(fullTextString);
                    }
                }
                else if (File.Exists(newFilePath))
                {
                    using (var tw = new StreamWriter(newFilePath, true))
                    {
                        tw.WriteLine(fullTextString);
                    }
                }
                Console.WriteLine(fullTextString);
                Console.WriteLine(" ! ");
                Console.WriteLine(" ! ");
            }
            
          //  File.ReadAllText();


            Console.WriteLine("Hello World!");
            Console.ReadLine();
            Console.ReadKey();
        }

        public static string StripTagsCharArray(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;
            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }
}
