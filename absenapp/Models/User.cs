using System.Collections.Generic;
using Ocph.DAL;

namespace absenapp.Models
{
    [TableName ("user")]
    public class User {
        [PrimaryKey ("iduser")]
        [DbColumn ("iduser")]
        public int iduser { get; set; }

        [DbColumn ("username")]
        public string username { get; set; }

        [DbColumn ("password")]
        public string password { get; set; }

        [DbColumn ("avatar")]
        public byte[] avatar { get; set; }

        [DbColumn ("PasswordHash")]
        public byte[] PasswordHash { get; set; }

        [DbColumn ("PasswordSalt")]
        public byte[] PasswordSalt { get; set; }

        public string token { get; set; }

        public IEnumerable<role> roles { get; set; }

        public string name { get; set; }

    }

    public class ChangePassword{
        public string email {get;set;}
        public string password {get;set;}
        public string newpassword{get;set;}

        public string confirmpassword{get;set;}
    }

}