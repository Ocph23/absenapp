import { Component, OnInit } from '@angular/core';
import { AbsenService } from '../../services/absen.service';
import { absen, pegawai } from '../../models/models.component';
import { groupBy, mergeMap, toArray, delay } from 'rxjs/operators';
import { from, Observable } from 'rxjs';
import { PegawaiService } from '../../services/pegawai.service';



@Component({
  selector: 'app-laporan',
  templateUrl: './laporan.component.html',
  styleUrls: ['./laporan.component.css']
})
export class LaporanComponent implements OnInit {


  public tahun: number = new Date().getFullYear();
  public bulan: number = new Date().getMonth() + 1;

  tahuns: number[] = [];
  bulans: number[] = [];
  datas = [];

  constructor(public absenService: AbsenService, private pegawaiService: PegawaiService) {
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


  hitungTunjangan(data: any):number {
    let a=this.pegawaiService.Datas.find(z => z.idpegawai === data.idpegawai);
      if(a)
      {
        return data.masuk *a.jabatan.tunjangan;
      }
      return 0;
  }


  onChange() {
    this.datas = [];
    let datatemp: absen[] = this.absenService.Datas.filter(x => new Date(x.jamdatang).getFullYear() === this.tahun && new Date(x.jamdatang).getMonth() + 1 == this.bulan);
    var result = from(datatemp).pipe(
      groupBy(x => x.namapegawai),
      mergeMap(group => group.pipe(toArray())));
    result.subscribe(x => {
      const pegawai: any = { namapegawai: x[0].namapegawai, idpegawai: x[0].idpegawai };
      pegawai.masuk = x.filter(z => z.status === "masuk").length;
      pegawai.sakit = x.filter(z => z.status === "sakit").length;
      pegawai.izin = x.filter(z => z.status === "izin").length;
      pegawai.dinas = x.filter(z => z.status === "dinas").length;
      this.datas.push(pegawai);
    });
  }
}



