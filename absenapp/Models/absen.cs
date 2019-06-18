using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace absenapp.Models
{
    [TableName("role")]
    public class absen
    {
        [PrimaryKey("idabsen")]
        [DbColumn("idabsen")]
        public int idabsen { get; set; }

        [DbColumn("idpegawai")]
        public int idpegawai { get; set; }

        [DbColumn("jamdatang")]
        public DateTime jamdatang { get; set; }

        [DbColumn("jampulang")]
        public DateTime jampulang { get; set; }

        [DbColumn("keterangan")]
        public DateTime keterangan { get; set; }

    }
}
