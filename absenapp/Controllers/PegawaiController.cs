using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using absenapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using absenapp.Helpers;
using absenapp.DataAccess;
using absenapp.Models;
using Microsoft.AspNetCore.Mvc;

namespace absenapp.Controllers {

    [Route("api/[controller]")]
    public class PegawaiController : Controller {

        PegawaiServices pegawaiService = new PegawaiServices();
         private IUserService _userService;
          private IUserRoleService _roleService;
        //  private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public PegawaiController (IUserService userService,IUserRoleService roleService,IOptions<AppSettings> appSettings) {
            _userService = userService;
            _roleService= roleService;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
            try {
                var result = await pegawaiService.Get();
                return Ok(result);
            } catch (System.Exception ex) {
                return BadRequest(ex.Message);
            }
        }


        [Route("[action]/{id}")]
        [HttpGet]
        public async Task<IActionResult> Get(int id) {
            try {
                var result = await pegawaiService.GetById(id);
                return Ok(result);

            } catch (System.Exception ex) {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] pegawai model) {
            try {
                pegawai result = await pegawaiService.Insert(model);

                if(result!=null && result.bendahara)
                {
                    var userCreated= await _userService.Create(new User{ username=model.email, password="BendaharaPassword"},"BendaharaPassword");
                    var roleName="bendahara";
                    if(userCreated!=null)
                    {
                        if(!await _roleService.RoleExsistsAsync(roleName))
                        {
                             var roleCreated= await _roleService.CreateRoleAsync(roleName);
                             if(!roleCreated)
                                throw new SystemException("User Akses Gagal Dibuat, ulangi lagi");
                        }

                        var userAddinRole = await _roleService.AddUserInRoleAsync(userCreated,roleName);
                        if(!userAddinRole)
                                throw new SystemException("User Akses Gagal Dibuat, ulangi lagi");
                    }
                }
                return Ok(result);
            } catch (System.Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] pegawai model) {
            try {
                var result = await pegawaiService.Update (model);
                if(result!=null && model.bendahara)
                {
                    var userCreated= await _userService.Create(new User{ username=model.email, password="BendaharaPassword"},"BendaharaPassword");
                    var roleName="bendahara";
                    if(userCreated!=null)
                    {
                        if(!await _roleService.RoleExsistsAsync(roleName))
                        {
                             var roleCreated= await _roleService.CreateRoleAsync(roleName);
                             if(!roleCreated)
                                throw new SystemException("User Akses Gagal Dibuat, ulangi lagi");
                        }

                        var userAddinRole = await _roleService.AddUserInRoleAsync(userCreated,roleName);
                        if(!userAddinRole)
                                throw new SystemException("User Akses Gagal Dibuat, ulangi lagi");
                    }
                }
                return Ok (result);
            } catch (System.Exception ex) {

                return BadRequest (ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id) {
            try {
                var result = await pegawaiService.Delete (id);
                return Ok (result);
            } catch (System.Exception ex) {

                return BadRequest (ex.Message);
            }
        }

    }
}