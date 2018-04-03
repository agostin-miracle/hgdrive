using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            //var credenciais = AsmGdrive.Start.Authenticate();

            //var servico = AsmGdrive.Start.OpenService(credenciais);
            //var list  = AsmGdrive.Operations.ListFiles(servico);
            //foreach (var item in list)
            //{
            //    Console.WriteLine(item.FileName);
            //}
            //Console.WriteLine("pressione qualquer tecla");
            
            //var credenciais = AsmGdrive.Start.Authenticate(System.Reflection.Assembly.GetEntryAssembly().Location);
            var credenciais = AsmGdrive.Start.Authenticate(String.Empty);
            var servico = AsmGdrive.Start.OpenService(credenciais);
            var IdTarget = AsmGdrive.Operations.GetFolderID(servico, "t01", false);
            string[] fileEntries = Directory.GetFiles("D:\\IMAGENS\\delicia");
            AsmGdrive.Operations.ParentFolder = "1DV_BV7HcUQaO-OWWDMjsZT_xAZbmJDqJ";
            //for (int i = 0; i < fileEntries.Length; i++)
            //{
            //    AsmGdrive.Operations.Upload(servico, fileEntries[i]);
            //}
            //AsmGdrive.Operations.ParentFolder = String.Empty;
            

            AsmGdrive.Operations.Download(servico,"111835_8.jpg", "D:\\lixo\\teste1.jpg");
            Console.WriteLine(AsmGdrive.Operations.LastError);
            AsmGdrive.Operations.MoveToGarbage(servico, "111835_8.jpg");
            Console.WriteLine(AsmGdrive.Operations.LastError);

            Console.ReadKey();
        }


        public static void ProcessDirectory(string targetDirectory)
        {
            //// Process the list of files found in the directory.
            //string[] fileEntries = Directory.GetFiles(targetDirectory);
            //foreach (string fileName in fileEntries)
            //    //ProcessFile(fileName);

            //// Recurse into subdirectories of this directory.
            //string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            //foreach (string subdirectory in subdirectoryEntries)
            //    ProcessDirectory(subdirectory);
        }

    }
}
