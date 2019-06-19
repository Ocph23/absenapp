
using Ocph.DAL;

namespace absenapp.Models
{
    [TableName("jabatan")] 
     public class jabatan 
   {
          [PrimaryKey("idjabatan")] 
          [DbColumn("idjabatan")] 
          public int idjabatan {  get; set;} 

          [DbColumn("nama")] 
          public string nama {  get; set;} 

           [DbColumn("nominal")] 
          public double tunjangan {  get; set;} 

     }
}


