using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using absenapp.DataAccess;
using absenapp.Models;
using Microsoft.AspNetCore.Mvc;

namespace absenapp.Controllers {

    [Route("api/[controller]")]
    public class JabatanController : Controller {

        JabatanServices service = new JabatanServices();

        [HttpGet]
        public async Task<IActionResult> Get() {
            try {
                var result = await service.Get();
                return Ok(result);
            } catch (System.Exception ex) {
                return BadRequest(ex.Message);
            }
        }


        [Route("[action]/{id}")]
        [HttpGet]
        public async Task<IActionResult> Get(int id) {
            try {
                var result = await service.GetById(id);
                return Ok(result);

            } catch (System.Exception ex) {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] jabatan model) {
            try {
                jabatan result = await service.Insert(model);
                return Ok(result);
            } catch (System.Exception ex) {

                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        public async Task<IActionResult> Put([FromBody] jabatan model) {
            try {
                var result = await service.Update (model);
                return Ok (result);
            } catch (System.Exception ex) {

                return BadRequest (ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id) {
            try {
                var result = await service.Delete (id);
                return Ok (result);
            } catch (System.Exception ex) {

                return BadRequest (ex.Message);
            }
        }

    }
}