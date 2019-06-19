using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using absenapp.Helpers;
using absenapp.Models;

namespace absenapp.DataAccess {
  public class JabatanServices {

    public Task<jabatan> GetById (int id) {
      using (var db = new OcphDbContext ()) {
        try {
          return Task.FromResult (db.Jabatan.Where (x => x.idjabatan == id).FirstOrDefault ());
        } catch (System.Exception ex) {
          throw new AppException (ex.Message);
        }
      }
    }

    public Task<List<jabatan>> Get () {
      using (var db = new OcphDbContext ()) {
        try {
          return Task.FromResult (db.Jabatan.Select ().ToList ());
        } catch (System.Exception ex) {
          throw new AppException (ex.Message);
        }
      }
    }

    public Task<bool> Update (jabatan item) {
      using (var db = new OcphDbContext ()) {
        try {
          return Task.FromResult (db.Jabatan.Update (x => new { x.nama, x.tunjangan }, item, x => x.idjabatan == item.idjabatan));
        } catch (System.Exception ex) {
          throw new AppException (ex.Message);
        }
      }
    }

    internal Task<jabatan> Insert (jabatan model) {
      using (var db = new OcphDbContext ()) {
        try {
          model.idjabatan = db.Jabatan.InsertAndGetLastID (model);
          return Task.FromResult (model);
        } catch (System.Exception ex) {
          throw new AppException (ex.Message);
        }
      }
    }

    public Task<bool> Delete (int id) {
      using (var db = new OcphDbContext ()) {
        try {
          return Task.FromResult (db.Jabatan.Delete (x => x.idjabatan == id));
        } catch (System.Exception ex) {
          throw new AppException (ex.Message);
        }
      }
    }

  }
}