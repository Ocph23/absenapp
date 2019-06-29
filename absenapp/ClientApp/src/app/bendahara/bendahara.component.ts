import { Component, OnInit } from '@angular/core';
import { absen } from '../models/models.component';
import { from } from 'rxjs';
import { groupBy, mergeMap, toArray } from 'rxjs/operators';
import { AbsenService } from '../services/absen.service';
import { PegawaiService } from '../services/pegawai.service';

@Component({
  selector: 'app-bendahara',
  templateUrl: './bendahara.component.html',
  styleUrls: ['./bendahara.component.scss', "../admin/admin.component.scss"]
})
export class BendaharaComponent implements OnInit {
  public tahun: number = new Date().getFullYear();
  public bulan: number = new Date().getMonth() + 1;

  tahuns: number[] = [];
  bulans: number[] = [];
  datas = [];

  constructor(
    public absenService: AbsenService,
    private pegawaiService: PegawaiService
  ) {
    for (let i = 1; i <= 12; i++) {
      this.bulans.push(i);
      if (i < 4) {
        this.tahuns.push(new Date().getFullYear() - (i - 1));
      }
    }
  }

  async ngOnInit() {
    this.absenService.get().then(x => {
      this.onChange();
    });
  }

  hitungTunjangan(data: any): number {
    let a = this.pegawaiService.Datas.find(z => z.idpegawai === data.idpegawai);
    if (a) {
      return data.masuk * a.jabatan.tunjangan;
    }
    return 0;
  }

  onChange() {
    this.datas = [];
    let datatemp: absen[] = this.absenService.Datas.filter(
      x =>
        new Date(x.jamdatang).getFullYear() === this.tahun &&
        new Date(x.jamdatang).getMonth() + 1 == this.bulan
    );
    var result = from(datatemp).pipe(
      groupBy(x => x.namapegawai),
      mergeMap(group => group.pipe(toArray()))
    );
    result.subscribe(x => {
      const pegawai: any = {
        namapegawai: x[0].namapegawai,
        idpegawai: x[0].idpegawai
      };
      pegawai.masuk = x.filter(z => z.status === "masuk").length;
      pegawai.sakit = x.filter(z => z.status === "sakit").length;
      pegawai.izin = x.filter(z => z.status === "izin").length;
      pegawai.dinas = x.filter(z => z.status === "dinas").length;
      this.datas.push(pegawai);
    });
  }

  print() {
    window.print();
  }

  ConvertBulan(data: number) {
    switch (data) {
      case 1:
        return "JANUARI";
        break;
      case 2:
        return "FEBRUARI";
        break;

      case 3:
        return "MARET";
        break;

      case 4:
        return "APRIL";
        break;

      case 5:
        return "MEI";
        break;

      case 6:
        return "JUNI";
        break;

      case 7:
        return "JULI";
        break;

      case 8:
        return "AGUSTUS";
        break;

      case 9:
        return "SEPTEMBER";
        break;
      case 10:
        return "OKTOBER";
        break;
      case 11:
        return "NOVEMBER";
        break;
      case 12:
        return "DESEMBER";
        break;

      default:
        break;
    }
  }
}
