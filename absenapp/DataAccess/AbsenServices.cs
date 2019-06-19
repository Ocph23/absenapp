using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using absenapp.Helpers;
using absenapp.Models;

namespace absenapp.DataAccess
{
    public class AbsenServices
    {
        public Task<absen> GetById(int id)
        {
            using (var db = new OcphDbContext())
            {
                try
                {
                    var result = db.Absens.Where(x => x.idabsen == id).FirstOrDefault();
                    return Task.FromResult(result);
                }
                catch (System.Exception ex)
                {
                    throw new AppException(ex.Message);
                }
            }
        }


        public Task<List<absen>> Get()
        {
            using (var db = new OcphDbContext())
            {
                try
                {

                    var result = from a in db.Pagawai.Select()
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

        public Task<bool> Update(absen item)
        {
            using (var db = new OcphDbContext())
            {
                try
                {
                    return Task.FromResult(db.Absens.Update(x => new { x.jamdatang, x.jampulang, x.keterangan }, item, x => x.idabsen == item.idabsen));
                }
                catch (System.Exception ex)
                {
                    throw new AppException(ex.Message);
                }
            }
        }

        public Task<absen> Insert(absen model)
        {
            using (var db = new OcphDbContext())
            {
                try
                {
                    var tgl = DateTime.Now;
                    var datas = db.Absens.Where(x => x.idpegawai == model.idpegawai).ToList();
                    if (datas.Count > 0)
                    {
                        var today = datas.Where(x => x.jamdatang.Day == tgl.Day && x.jamdatang.Month == tgl.Month && x.jamdatang.Year == tgl.Year).FirstOrDefault();
                        if (today == null)
                        {
                            model.jamdatang = tgl;
                            model.idabsen = db.Absens.InsertAndGetLastID(model);
                        }
                        else
                        {
                            if (today.jampulang.Day != tgl.Day || today.jampulang.Month != tgl.Month || today.jampulang.Year != tgl.Year)
                            {
                                today.jampulang = tgl;
                                if (!db.Absens.Update(x => new { x.jampulang }, today, x => x.idabsen == today.idabsen))
                                    throw new SystemException("Terjadi Kesalahan , Coba Ulangi Lagi");
                            }
                            else
                            {
                                throw new SystemException("Anda Sudah Absen hari Ini");
                            }
                        }
                    }
                    else
                    {
                        model.jamdatang = tgl;
                        model.idabsen = db.Absens.InsertAndGetLastID(model);
                    }
                    return Task.FromResult(model);
                }
                catch (System.Exception ex)
                {
                    throw new AppException(ex.Message);
                }
            }
        }

        public Task<bool> Delete(int id)
        {
            using (var db = new OcphDbContext())
            {
                try
                {
                    return Task.FromResult(db.Absens.Delete(x => x.idpegawai == id));
                }
                catch (System.Exception ex)
                {
                    throw new AppException(ex.Message);
                }
            }
        }


    }
}