using absenapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace absenapp.Services {
    public interface IUserService {
        Task<User> Authenticate (string username, string password);
        IEnumerable<User> GetAll ();
        User GetById (int id);
        Task<User> Create (User user, string password);
        void Update (User user, string password = null);
        void Delete (int id);
        pegawai GetPegawaiProfile(int id);

        User GetByEmail (string email);
        bool ChangePassword (ChangePassword data, User userData);
    }

    public class UserService : IUserService {

        private UserRoleServices roleServices = new UserRoleServices ();

        public async Task<User> Authenticate (string username, string password) {
            if (string.IsNullOrEmpty (username) || string.IsNullOrEmpty (password))
                return null;

            using (var db = new OcphDbContext ()) {

                var user = db.Users.Where (x => x.username == username).FirstOrDefault ();
                if (user == null)
                    return null;

                if (!VerifyPasswordHash (password, user.PasswordHash, user.PasswordSalt))
                    return null;

                user.roles = await this.roleServices.GetUserRoleAsync (user);;
                return user;
            }

        }

        public IEnumerable<User> GetAll () {
            using (var db = new OcphDbContext ()) {
                return db.Users.Select ();
            }
        }

        public User GetById (int id) {
            using (var db = new OcphDbContext ()) {
                return db.Users.Where (x => x.iduser == id).FirstOrDefault ();
            }

        }

        public Task<User> Create (User user, string password) {
            // validation
            if (string.IsNullOrWhiteSpace (password))
                throw new SystemException ("Password is required");
            using (var db = new OcphDbContext ()) {
                var data = db.Users.Where (x => x.username == user.username).FirstOrDefault ();

                if (data != null)
                    throw new SystemException("Username \"" + user.username + "\" is already taken");

                byte[] passwordHash, passwordSalt;
                CreatePasswordHash (password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.iduser = db.Users.InsertAndGetLastID (user);
                if (user.iduser <= 0)
                    user = null;
                return Task.FromResult (user);
            }
        }

        public void Update (User userParam, string password = null) {
            using (var db = new OcphDbContext ()) {
                var user = db.Users.Where (x => x.iduser == userParam.iduser).FirstOrDefault ();

                if (user == null)
                    throw new SystemException("User not found");

                if (userParam.username != user.username) {
                    // username has changed so check if the new username is already taken
                    if (db.Users.Where (x => x.username == userParam.username).FirstOrDefault () != null)
                        throw new SystemException("Username " + userParam.username + " is already taken");
                }

                // update user properties

                // update password if it was entered
                if (!string.IsNullOrWhiteSpace (password)) {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash (password, out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                }
                if (!db.Users.Update (x => new { x.username, x.PasswordHash, x.PasswordSalt }, user, x => x.iduser == user.iduser)) {
                    throw new SystemException("not saved");
                }
            }
        }

        public void Delete (int id) {
            using (var db = new OcphDbContext ()) {
                var user = db.Users.Where (x => x.iduser == id).FirstOrDefault ();
                if (user != null) {
                    db.Users.Delete (x => x.iduser == id);
                }
            }
        }

        private static void CreatePasswordHash (string password, out byte[] passwordHash, out byte[] passwordSalt) {
            if (password == null) throw new ArgumentNullException ("password");
            if (string.IsNullOrWhiteSpace (password)) throw new ArgumentException ("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512 ()) {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash (System.Text.Encoding.UTF8.GetBytes (password));
                Console.WriteLine ($"pasword has {passwordSalt}   {passwordHash}");
            }
        }

        private static bool VerifyPasswordHash (string password, byte[] storedHash, byte[] storedSalt) {
            if (password == null) throw new ArgumentNullException ("password");
            if (string.IsNullOrWhiteSpace (password)) throw new ArgumentException ("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException ("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException ("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512 (storedSalt)) {
                var computedHash = hmac.ComputeHash (System.Text.Encoding.UTF8.GetBytes (password));
                for (int i = 0; i < computedHash.Length; i++) {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        public pegawai GetPegawaiProfile (int id) {
            try {
                using (var db = new OcphDbContext ()) {
                    var t = db.Pagawai.Where (x => x.idpegawai == id).FirstOrDefault ();
                    if (t != null)
                        return t;
                    else
                        throw new SystemException ("Data Not Found");
                }
            } catch (Exception ex) {
                throw new SystemException (ex.Message);
            }
        }

        public User GetByEmail (string email) {
            using (var db = new OcphDbContext ()) {
                return db.Users.Where (x => x.username == email).FirstOrDefault ();
            }
        }

        public bool ChangePassword (ChangePassword data, User user) {
            try {

                byte[] passwordHash, passwordSalt;
                CreatePasswordHash (data.newpassword, out passwordHash, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                using (var db = new OcphDbContext ()) {
                    if (db.Users.Update (x => new { x.PasswordHash, x.PasswordSalt }, user, x => x.iduser == user.iduser))
                        return true;
                    return false;
                }
            } catch (System.Exception ex) {

                throw new SystemException(ex.Message);
            }
        }

    }
}