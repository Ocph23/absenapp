using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using absenapp.DataAccess;
using absenapp.Models;
using Microsoft.AspNetCore.Mvc;

namespace absenapp.Controllers {

    [Route("api/[controller]")]
    public class PegawaiController : Controller {

        PegawaiServices pegawaiService = new PegawaiServices();

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
                return Ok(result);
            } catch (System.Exception ex) {

                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        public async Task<IActionResult> Put([FromBody] pegawai model) {
            try {
                var result = await pegawaiService.Update (model);
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