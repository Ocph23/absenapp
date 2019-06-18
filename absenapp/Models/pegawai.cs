using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace absenapp.Models
{
    [TableName("user")]
    public class pegawai
    {
        [PrimaryKey("idpegawai")]
        [DbColumn("idpegawai")]
        public int idpegawai { get; set; }

        [DbColumn("nama")]
        public string nama { get; set; }

        [DbColumn("nip")]
        public string nip { get; set; }

        [DbColumn("alamat")]
        public string alamat { get; set; }

        [DbColumn("email")]
        public string email { get; set; }

        [DbColumn("jabatan")]
        public string jabatan { get; set; }

        [DbColumn("password")]
        public string password{ get; set; }

        [DbColumn("sex")]
        public string sex{ get; set; }
    }
}
