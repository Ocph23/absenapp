using System.Threading.Tasks;
using absenapp.Helpers;
using absenapp.Models;
using System.Linq;
using System.Collections.Generic;

namespace absenapp.DataAccess {
    public class AbsenServices {

        public Task<absen> GetById (int id) {
            using (var db = new OcphDbContext ()) {
                try {
                    var result = db.Absens.Where (x => x.idabsen == id) .FirstOrDefault();
                    return Task.FromResult(result);
                } catch (System.Exception ex) {
                    throw new AppException (ex.Message);
                }
            }
        }

        public Task<List<absen>> Get () {
            using (var db = new OcphDbContext ()) {
                try {
                    return Task.FromResult (db.Absens.Select ().ToList ());
                } catch (System.Exception ex) {
                    throw new AppException (ex.Message);
                }
            }
        }

        public Task<bool> Update (absen item) {
            using (var db = new OcphDbContext ()) {
                try {
                    return Task.FromResult (db.Absens.Update (x => new { x.jamdatang, x.jampulang,x.keterangan}, item, x => x.idabsen == item.idabsen));
                } catch (System.Exception ex) {
                    throw new AppException (ex.Message);
                }
            }
        }

        public Task<absen> Insert (absen model) {
            using (var db = new OcphDbContext ()) {
                try {
                    model.idpegawai = db.Absens.InsertAndGetLastID (model);
                    return Task.FromResult (model);
                } catch (System.Exception ex) {
                    throw new AppException (ex.Message);
                }
            }
        }

        public Task<bool> Delete (int id) {
            using (var db = new OcphDbContext ()) {
                try {
                    return Task.FromResult (db.Absens.Delete (x => x.idpegawai == id));
                } catch (System.Exception ex) {
                    throw new AppException (ex.Message);
                }
            }
        }
    }
}