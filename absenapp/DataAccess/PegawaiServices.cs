using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using absenapp.Helpers;
using absenapp.Models;

namespace absenapp.DataAccess {
  public class PegawaiServices {

    public Task<pegawai> GetById (int id) {
      using (var db = new OcphDbContext ()) {
        try {
          return Task.FromResult (db.Pagawai.Where (x => x.idpegawai == id).FirstOrDefault ());
        } catch (System.Exception ex) {
          throw new AppException (ex.Message);
        }
      }
    }

    public Task<List<pegawai>> Get () {
      using (var db = new OcphDbContext ()) {
        try {
          return Task.FromResult (db.Pagawai.Select ().ToList ());
        } catch (System.Exception ex) {
          throw new AppException (ex.Message);
        }
      }
    }

    public Task<bool> Update (pegawai item) {
      using (var db = new OcphDbContext ()) {
        try {
          return Task.FromResult (db.Pagawai.Update (x => new { x.alamat, x.jabatan, x.nama, x.nip, x.sex }, item, x => x.idpegawai == item.idpegawai));
        } catch (System.Exception ex) {
          throw new AppException (ex.Message);
        }
      }
    }

    internal Task<pegawai> Insert (pegawai model) {
      using (var db = new OcphDbContext ()) {
        try {
          model.idpegawai = db.Pagawai.InsertAndGetLastID (model);
          return Task.FromResult (model);
        } catch (System.Exception ex) {
          throw new AppException (ex.Message);
        }
      }
    }

    public Task<bool> Delete (int id) {
      using (var db = new OcphDbContext ()) {
        try {
          return Task.FromResult (db.Pagawai.Delete (x => x.idpegawai == id));
        } catch (System.Exception ex) {
          throw new AppException (ex.Message);
        }
      }
    }

  }
}