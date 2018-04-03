using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsmGdrive
{
    public class Operations
    {

        public static string ParentFolder { get; set; } = "";
        public static string LastError { get; set; } = "";
        public static string LastID { get; set; } = "";
        public static object LastErrorObject { get; set; } = null;
        public static object LastMessage { get; set; } = "";
        public static bool HasError()
        {
            if (String.IsNullOrEmpty(LastError))
            {
                return false;
            }
            return true;
        }

        private static void ClearError()
        {
            LastErrorObject = null;
            LastError = "";
            LastMessage = "";
            LastID = "";
        }

        public static List<FileItemList> ListFiles(Google.Apis.Drive.v3.DriveService servico)
        {
            ClearError();
            List<FileItemList> retorno = new List<FileItemList>();
            var request = servico.Files.List();
            request.Fields = "files(id, name)";
            var resultado = request.Execute();
            var arquivos = resultado.Files;

            if (arquivos != null && arquivos.Any())
            {
                foreach (var arquivo in arquivos)
                {
                    retorno.Add(new FileItemList(arquivo.Id, arquivo.Name));
                }
            }
            return retorno;
        }


        public static void CreateDirectory(Google.Apis.Drive.v3.DriveService servico, string DirName)
        {
            ClearError();
            try
            {
                if (!String.IsNullOrEmpty(DirName))
                {
                    if (!String.IsNullOrWhiteSpace(DirName))
                    {
                        var diretorio = new Google.Apis.Drive.v3.Data.File();
                        diretorio.Name = DirName;
                        diretorio.MimeType = "application/vnd.google-apps.folder";

                        var request = servico.Files.Create(diretorio);
                        request.Fields = "id";
                        var directory = request.Execute();
                        LastMessage = String.Format("Diretório {0} criado com sucesso", DirName);
                        LastID = directory.Id;
                    }
                    else
                        LastError = "Diretório não fornecido";
                }
                else
                    LastError = "Diretório não fornecido";
            }
            catch (Exception Error)
            {
                LastErrorObject = Error;
                LastError = Error.Message;
                LastMessage = Error.Message;
            }
        }


        public static string[] GetFolderID(Google.Apis.Drive.v3.DriveService servico, string nome, bool SearchOnTheGarbage = false)
        {
            var retorno = new List<string>();
            var request = servico.Files.List();
            request.Q = string.Format("name = '{0}'", nome);
            if (!SearchOnTheGarbage)
            {
                request.Q += " and trashed = false";
            }
            request.Fields = "files(id)";
            var resultado = request.Execute();
            var arquivos = resultado.Files;

            if (arquivos != null && arquivos.Any())
            {
                foreach (var arquivo in arquivos)
                {
                    retorno.Add(arquivo.Id);
                }
            }

            return retorno.ToArray();
        }
        public static void Upload(Google.Apis.Drive.v3.DriveService servico, string FilePathName)
        {
            //1DV_BV7HcUQaO-OWWDMjsZT_xAZbmJDqJ
            ClearError();
            string _mimetype = "";
            if (!String.IsNullOrEmpty(FilePathName))
            {
                if (!String.IsNullOrWhiteSpace(FilePathName))
                {
                    _mimetype = MimeTypes.MimeTypeMap.GetMimeType(System.IO.Path.GetExtension(FilePathName));
                    var arquivo = new Google.Apis.Drive.v3.Data.File();
                    arquivo.Name = System.IO.Path.GetFileName(FilePathName);
                    arquivo.MimeType = _mimetype;

                    if(!String.IsNullOrEmpty(ParentFolder))
                    arquivo.Parents = new List<string> { ParentFolder };


                    using (var stream = new System.IO.FileStream(FilePathName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        var ids = GetFolderID(servico, arquivo.Name);
                        Google.Apis.Upload.ResumableUpload<Google.Apis.Drive.v3.Data.File, Google.Apis.Drive.v3.Data.File> request;
                        if (ids == null || !ids.Any())
                        {
                            request = servico.Files.Create(arquivo, stream, _mimetype);
                        }
                        else
                        {
                            request = servico.Files.Update(arquivo, ids.First(), stream, _mimetype);
                        }
                        request.Upload();
                    }
                }
            }
        }


        public static void Download(Google.Apis.Drive.v3.DriveService servico, string filename, string destino)
        {
            ClearError();
            string target = "";
            var ids = GetFolderID(servico, filename);
            try
            {
                if (ids != null && ids.Any())
                {
                    var request = servico.Files.Get(ids.First());
                    using (var stream = new System.IO.FileStream(destino, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                    {
                        request.Download(stream);
                    }
                }
            }
            catch (Exception Error)
            {
                LastErrorObject = Error;
                LastError = Error.Message;
                LastMessage = Error.Message;
            }

        }

        public static void MoveToGarbage(Google.Apis.Drive.v3.DriveService servico, string filename)
        {
            ClearError();
            var ids = GetFolderID(servico, filename);
            if (ids != null && ids.Any())
            {
                foreach (var id in ids)
                {
                    var arquivo = new Google.Apis.Drive.v3.Data.File();
                    arquivo.Trashed = true;
                    var request = servico.Files.Update(arquivo, id);
                    request.Execute();
                }
            }
        }
    }
}
