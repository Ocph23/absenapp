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

        public Task<List<absen>> GetByPegawaiId(int id)
        {
            using (var db = new OcphDbContext())
            {
                try
                {
                    var result = from a in db.Pagawai.Where(x=>x.idpegawai==id)
                                 join b in db.Absens.Select().DefaultIfEmpty() on a.idpegawai equals b.idpegawai
                                 select new absen
                                 {
                                     idabsen = b.idabsen,
                                     idpegawai = b.idpegawai,
                                     jamdatang = b.jamdatang,
                                     jampulang = b.jampulang,
                                     status = b.status,
                                     keterangan = b.keterangan,
                                     namapegawai = a.nama
                                 };

                    return Task.FromResult(result.ToList());
                }
                catch (System.Exception ex)
                {
                    throw new AppException(ex.Message);
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

        internal Task<List<absen>> GetAbsenToday()
        {
            using (var db = new OcphDbContext())
            {
                DateTime tanggalHariIni = DateTime.Now;

                var todayabsen = from a in db.Absens.Where(x => x.jamdatang.Value.Day == tanggalHariIni.Day &&
                    x.jamdatang.Value.Month == tanggalHariIni.Month && x.jamdatang.Value.Year == tanggalHariIni.Year)
                                 join p in db.Pagawai.Select() on a.idpegawai equals p.idpegawai
                                 select new absen
                                 {
                                     idabsen = a.idabsen,
                                     idpegawai = a.idpegawai,
                                     jamdatang = a.jamdatang,
                                     jampulang = a.jampulang,
                                     keterangan = a.keterangan,
                                     namapegawai = p.nama,
                                     status = a.status
                                 };
                return Task.FromResult(todayabsen.ToList());
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

        public Task<absen> TodayAbsen (absen model) {
            using (var db = new OcphDbContext ()) {
                try {
                    //edit
                    DateTime tanggalHariIni = DateTime.Now;
                    TimeSpan waktuHariIni = tanggalHariIni.TimeOfDay;
                    TimeSpan jamMasuk = new TimeSpan(8, 0, 0);
                    TimeSpan batasTerlambat = new TimeSpan (10, 0, 0);
                    TimeSpan batasSiang = new TimeSpan(12, 0, 0);
                    TimeSpan batasPulangCepat = new TimeSpan (16, 0, 0);
                    TimeSpan jamPulang = new TimeSpan(17, 0, 0);

                    var todayabsen = db.Absens.Where (x => x.idpegawai == model.idpegawai && x.jamdatang.Value.Day == tanggalHariIni.Day &&
                        x.jamdatang.Value.Month == tanggalHariIni.Month && x.jamdatang.Value.Year == tanggalHariIni.Year).FirstOrDefault ();

                    if(todayabsen ==null)
                    {
                        if(model.status== "masuk")
                        {
                            if (waktuHariIni > batasTerlambat)
                                throw new SystemException("Anda Tidak Dapat Absen Karena Terlambat");
                            else
                            {
                                model.jamdatang = tanggalHariIni;
                                model.jampulang = null;
                                if (waktuHariIni > jamMasuk)
                                {
                                    model.keterangan = "Datang : Terlambat, ";
                                }
                                else
                                {
                                    model.keterangan = "Datang : Tepat Waktu,";
                                }
                                model.idabsen = db.Absens.InsertAndGetLastID(model);
                                if (model.idabsen <= 0)
                                    throw new SystemException("Terjadi Kesalahan, Coba Ulangi Lagi");
                                return Task.FromResult(model);
                            }
                        }
                        else
                        {
                            model.jamdatang = tanggalHariIni;
                            model.jampulang = null;
                            model.idabsen = db.Absens.InsertAndGetLastID(model);
                            if (model.idabsen <= 0)
                                throw new SystemException("Terjadi Kesalahan, Coba Ulangi Lagi");
                            return Task.FromResult(model);
                        }
                    }
                    else
                    {
                        if (waktuHariIni <= batasTerlambat)
                        {
                            throw new SystemException("Anda Sudah Absen Datang");
                        }

                        if (waktuHariIni > batasTerlambat && waktuHariIni < batasPulangCepat)
                        {
                            throw new SystemException("belum Saatnya Pulang");
                        }

                        if(todayabsen.jampulang!=null)
                        {
                            throw new SystemException("Anda Sudah Absen Pulang");
                        }

                        if (waktuHariIni > batasPulangCepat)
                        {
                            todayabsen.keterangan += $"\r Pulang : Terlalu Cepat";
                        }

                        if (waktuHariIni > jamPulang)
                        {
                            todayabsen.keterangan += $"\r Pulang : Tepat Waktu";
                        }

                        todayabsen.jampulang = tanggalHariIni;

                        if (!db.Absens.Update(x => new { x.status, x.jamdatang, x.jampulang, x.keterangan }, todayabsen,
                                x => x.idabsen == todayabsen.idabsen))
                            throw new SystemException("Terjadi Kesalahan , Coba Ulangi Lagi");
                        model = todayabsen;
                        return Task.FromResult(model);
                    }
                 
                } catch (System.Exception ex) {
                    throw new AppException (ex.Message);
                }
            }
        }




        public Task<absen> Absenbyadmin(absen model)
        {
            using (var db = new OcphDbContext())
            {
                try
                {
                    //edit
                    DateTimeOffset localTime1 = DateTime.SpecifyKind(model.jamdatang.Value, DateTimeKind.Utc);
                    model.jamdatang = localTime1.LocalDateTime;

                    if (model.jampulang!=null)
                    {
                        DateTimeOffset localTime2 = DateTime.SpecifyKind(model.jampulang.Value, DateTimeKind.Utc);
                        model.jampulang = localTime2.LocalDateTime;
                    }


                    DateTime tanggalHariIni = model.jamdatang.Value;
                    var todayabsen = db.Absens.Where(x => x.idpegawai == model.idpegawai && x.jamdatang.Value.Day == tanggalHariIni.Day &&
                       x.jamdatang.Value.Month == tanggalHariIni.Month && x.jamdatang.Value.Year == tanggalHariIni.Year).FirstOrDefault();

                    if (todayabsen == null)
                    {
                        model.jamdatang = tanggalHariIni;
                        if(model.status!="masuk")
                            model.jampulang = null;

                        model.idabsen = db.Absens.InsertAndGetLastID(model);
                        if (model.idabsen <= 0)
                            throw new SystemException("Terjadi Kesalahan, Coba Ulangi Lagi");
                        return Task.FromResult(model);
                    }
                    else
                    {
                        todayabsen.jamdatang = model.jamdatang;
                        todayabsen.jampulang= model.jampulang;
                        todayabsen.status = model.status;
                        todayabsen.keterangan= model.keterangan;


                        if (!db.Absens.Update(x => new { x.status, x.jamdatang, x.jampulang, x.keterangan }, todayabsen,
                                x => x.idabsen == todayabsen.idabsen))
                            throw new SystemException("Terjadi Kesalahan , Coba Ulangi Lagi");
                        model = todayabsen;
                        return Task.FromResult(model);

                    }

                }
                catch (System.Exception ex)
                {
                    throw new AppException(ex.Message);
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

                        int tanggal = model.jamdatang.Value.Day;
                        int bulan = model.jamdatang.Value.Month;
                        int tahun = model.jamdatang.Value.Year;

                        var today = db.Absens.Where (x => x.idpegawai == model.idpegawai && x.jamdatang.Value.Day == tanggal &&
                            x.jamdatang.Value.Month == bulan && x.jamdatang.Value.Year == tanggal).FirstOrDefault ();

                        if (today == null) {
                            model.idabsen = db.Absens.InsertAndGetLastID (model);
                        } else {
                            if (today.jampulang.Value.Day != model.jampulang.Value.Day || today.jampulang.Value.Month != model.jampulang.Value.Month ||
                                today.jampulang.Value.Year != model.jampulang.Value.Year) {
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