using System.Data.Common;
using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace absenapp.Models
{
    [TableName("absen")]
    public class absen
    {
        [PrimaryKey("idabsen")]
        [DbColumn("idabsen")]
        public int idabsen { get; set; }

        [DbColumn("idpegawai")]
        public int idpegawai { get; set; }

        [DbColumn("status")]
        public string status { get; set; }

        [DbColumn("jamdatang")]
        public DateTime jamdatang { get; set; }

        [DbColumn("jampulang")]
        public DateTime jampulang { get; set; }

        [DbColumn("keterangan")]
        public string keterangan { get; set; }

        public string namapegawai{get;set;}

    }
}
