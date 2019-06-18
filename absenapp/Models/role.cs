
using Ocph.DAL;

namespace absenapp.Models
{
    [TableName("role")] 
     public class role 
   {
          [PrimaryKey("idrole")] 
          [DbColumn("idrole")] 
          public int idrole {  get; set;} 

          [DbColumn("name")] 
          public string name {  get; set;} 

     }
}


