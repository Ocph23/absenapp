using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using absenapp.Models;

namespace absenapp.Services {

    public interface IUserRoleService {
        Task<bool> UserIsInRoleAsync (User user, string roleName);

        Task<bool> RoleExsistsAsync (string roleName);

        Task<bool> CreateRoleAsync (string roleName);

        Task<bool> AddUserInRoleAsync (User user, string roleName);

        Task<IEnumerable<role>> GetUserRoleAsync (User user);


    }

    public class UserRoleServices : IUserRoleService {
        public async Task<bool> AddUserInRoleAsync (User user, string roleName) {
            using (var db = new OcphDbContext ()) {
                if (await RoleExsistsAsync (roleName)) {
                    var result = db.Roles.Where (x => x.name == roleName).FirstOrDefault();
                    if (result != null) {
                        if (db.UserRoles.Insert (new userinrole { iduser = user.iduser, idrole = result.idrole }))
                            return true;
                    }
                }
                return false;
            }
        }

        public async Task<bool> CreateRoleAsync (string roleName) {
            if (await RoleExsistsAsync (roleName))
                throw new System.Exception ($"Role {roleName} Exists");
            else {
                using (var db = new OcphDbContext ()) {
                    if (db.Roles.Insert (new role { name = roleName.ToLower () }))
                        return true;
                    return false;
                }
            }
        }

        public async Task<IEnumerable<role>> GetUserRoleAsync (User user) {
            using (var db = new OcphDbContext ()) {
                var result = (from a in db.UserRoles.Where (x => x.iduser == user.iduser) join b in db.Roles.Select () 
                              on a.idrole equals b.idrole select b);

                return await Task.FromResult<IEnumerable<role>> (result.ToList());
            }
        }

        public async Task<bool> RoleExsistsAsync (string roleName) {
            bool exists = false;
            using (var db = new OcphDbContext ()) {
                var result = db.Roles.Where (x => x.name == roleName);
                if (result != null)
                    exists = true;
            }

            return await Task.FromResult (exists);
        }

        public async Task<bool> UserIsInRoleAsync (User user, string roleName) {
            if (await RoleExsistsAsync (roleName)) {
                var userRoles = await GetUserRoleAsync (user);
                if (userRoles.Where (x => x.name == roleName).FirstOrDefault () != null) {
                    return true;
                }
            }
            return false;
        }
    }
}