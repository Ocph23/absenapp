using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace absenapp.Models
{
    [TableName("pegawai")]
    public class pegawai
    {
        [PrimaryKey("idpegawai")]
        [DbColumn("idpegawai")]
        public int idpegawai { get; set; }

        [DbColumn("idjabatan")]
        public int idjabatan { get; set; }

        [DbColumn("nama")]
        public string nama { get; set; }

        [DbColumn("nip")]
        public string nip { get; set; }

        [DbColumn("alamat")]
        public string alamat { get; set; }

        [DbColumn("email")]
        public string email { get; set; }
        
        [DbColumn("kontak")]
        public string kontak { get; set; }

        [DbColumn("bendahara")]
        public bool bendahara{ get; set; }

         [DbColumn("sex")]
        public string sex{ get; set; }

        public jabatan jabatan { get; set; }
    }
}
