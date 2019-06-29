using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using absenapp.Helpers;
using absenapp.Models;

namespace absenapp.DataAccess {
    public class AbsenServices {
        public Task<absen> GetById (int id) {
            using (var db = new OcphDbContext ()) {
                try {
                    var result = db.Absens.Where (x => x.idabsen == id).FirstOrDefault ();
                    return Task.FromResult (result);
                } catch (System.Exception ex) {
                    throw new AppException (ex.Message);
                }
            }
        }

        public Task<List<absen>> Get () {
            using (var db = new OcphDbContext ()) {
                try {

                var result = from a in db.Pagawai.Select ()
                join b in db.Absens.Select ().DefaultIfEmpty () on a.idpegawai equals b.idpegawai
                select new absen {
                idabsen = b.idabsen,
                idpegawai = b.idpegawai,
                jamdatang = b.jamdatang,
                jampulang = b.jampulang,
                status = b.status,
                keterangan = b.keterangan,
                namapegawai = a.nama
                    };

                    return Task.FromResult (result.ToList ());
                } catch (System.Exception ex) {
                    throw new AppException (ex.Message);
                }
            }
        }

        public Task<bool> Update (absen item) {
            using (var db = new OcphDbContext ()) {
                try {
                    return Task.FromResult (db.Absens.Update (x => new { x.jamdatang, x.jampulang, x.keterangan }, item, x => x.idabsen == item.idabsen));
                } catch (System.Exception ex) {
                    throw new AppException (ex.Message);
                }
            }
        }

        public Task<absen> Insert (absen model) {
            using (var db = new OcphDbContext ()) {
                try {
                    //edit
                    if (model.idabsen > 0) {
                        if (!db.Absens.Update (x => new { x.status, x.jamdatang, x.jampulang, x.keterangan }, model,
                                x => x.idabsen == model.idabsen))
                            throw new SystemException ("Terjadi Kesalahan , Coba Ulangi Lagi");
                    } else {

                        int tanggal = model.jamdatang.Day;
                        int bulan = model.jamdatang.Month;
                        int tahun = model.jamdatang.Year;

                        var today = db.Absens.Where (x => x.idpegawai == model.idpegawai && x.jamdatang.Day == tanggal &&
                            x.jamdatang.Month == bulan && x.jamdatang.Year == tanggal).FirstOrDefault ();

                        if (today == null) {
                            model.idabsen = db.Absens.InsertAndGetLastID (model);
                        } else {
                            if (today.jampulang.Day != model.jampulang.Day || today.jampulang.Month != model.jampulang.Month ||
                                today.jampulang.Year != model.jampulang.Year) {
                                if (!db.Absens.Update (x => new { x.jampulang }, today, x => x.idabsen == today.idabsen))
                                    throw new SystemException ("Terjadi Kesalahan , Coba Ulangi Lagi");
                            } else {
                                throw new SystemException ("Anda Sudah Absen hari Ini");
                            }
                        }
                    }
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