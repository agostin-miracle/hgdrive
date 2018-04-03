using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsmGdrive
{
    public class FileItemList
    {
        public string ID { get; set; }
        public string FileName { get; set; }


        public FileItemList(string _id, string _filename)
        {
            this.ID = _id;
            this.FileName = _filename;
        }
    }
}
