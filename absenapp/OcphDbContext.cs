//using MongoDB.Driver;
using absenapp.Models;
using Ocph.DAL.Repository;

namespace absenapp
{
    public class OcphDbContext : Ocph.DAL.Provider.MySql.MySqlDbConnection {
        // private MongoClient dbClient;
        // private IMongoDatabase db;
        // private string appSettingsSection = Configuration.GetSection ("AppSettings");
        public OcphDbContext () {
            // dbClient = new MongoClient("mongodb://ocph23:Sony77@ds147125.mlab.com:47125/heroku_l5k9k33h");
            // db = dbClient.GetDatabase("heroku_l5k9k33h");
            // var a = Configuration.GetSection ("AppSettings");

         this.ConnectionString = "server=localhost;database=absendb;uid=root;password=";
        }

        public IRepository<User> Users { get { return new Repository<User> (this); } }
        public IRepository<role> Roles { get { return new Repository<role> (this); } }
        public IRepository<userinrole> UserRoles { get { return new Repository<userinrole> (this); } }
        public IRepository<pegawai> Pagawai{ get { return new Repository<pegawai>(this); } }
        public IRepository<absen> Absens{ get { return new Repository<absen>(this); } }
        public IRepository<jabatan>Jabatan { get { return new Repository<jabatan>(this); } }

    }
}