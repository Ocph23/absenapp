using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using absenapp.DataAccess;
using absenapp.Models;
using Microsoft.AspNetCore.Mvc;

namespace absenapp.Controllers {

    [Route ("api/[controller]")]
    public class AbsenController : Controller {

        AbsenServices absenService = new AbsenServices ();

        [HttpGet]
        public async Task<IActionResult> GetAsync () {
            try {
                var result = await absenService.Get ();
                return Ok (result);
            } catch (System.Exception ex) {
                return BadRequest (ex.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync (int id) {
            try {
                var result = await absenService.GetById (id);
                return Ok (result);
            
            } catch (System.Exception ex) {

                return BadRequest (ex.Message);
            }
        }


        [HttpGet("ByPegawaiId/{id}")]
        public async Task<IActionResult> GetByPegawaiIdAsync(int id)
        {
            try
            {
                var result = await absenService.GetByPegawaiId(id);
                return Ok(result);

            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("absentoday")]
        public async Task<IActionResult> GetAbsenToday()
        {
            try
            {
                var result = await absenService.GetAbsenToday();
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("TodayByadmin")]
        public async Task<IActionResult> TodayByadmin([FromBody] absen model)
        {
            try
            {
                absen result = await absenService.Absenbyadmin(model);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("today")]
        public async Task<IActionResult> Today ([FromBody] absen model) {
            try {
                absen result = await absenService.TodayAbsen (model);
                return Ok (result);
            } catch (System.Exception ex) {
                return BadRequest (ex.Message);
            }
        }

       

        [HttpPost]
        public async Task<IActionResult> Post ([FromBody] absen model) {
            try {
                absen result = await absenService.Insert (model);
                return Ok (result);
            } catch (System.Exception ex) {

                return BadRequest (ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync (int id, [FromBody] absen model) {
            try {
                var result = await absenService.Update (model);
                return Ok (result);
            } catch (System.Exception ex) {

                return BadRequest (ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync (int id) {
            try {
                var result = await absenService.Delete (id);
                return Ok (result);
            } catch (System.Exception ex) {

                return BadRequest (ex.Message);
            }
        }

      

    }
}