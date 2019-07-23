import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { AbsenService } from '../services/absen.service';
import { absen, pegawai } from '../models/models.component';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { PegawaiService } from '../services/pegawai.service';

@Component({
  selector: 'app-absen',
  templateUrl: './absen.component.html',
  styleUrls: ['./absen.component.scss']
})
export class AbsenComponent implements OnInit {
  @ViewChild('swalMessage') swal: SwalComponent;
    test: string;
    loginForm: any;
    absens: absen[];
    HariIni: Date = new Date();

    constructor(
      private router: Router, private fb: FormBuilder, 
      private absenService: AbsenService, public pegawaiService:PegawaiService) {
  this.loginForm = fb.group({
    'pegawai': [null, Validators.required],
  });
}
  ngOnInit() {

      this.absenService.getAbsenToday().then(x => {
          this.absens = x;
      });

  }


    absenSave(event: any, item: any): void {
        const data: absen = { idpegawai: item.pegawai.idpegawai, status: 'masuk', jamdatang: new Date(), jampulang: new Date() } as absen;
      this.absenService.SaveChange(data).subscribe(x => {
        this.swal.text = 'Sukses';
        this.swal.type = 'info';
        this.swal.title = 'INFO';
          this.swal.show();

          if (!x.jampulang)
          {
              x.namapegawai = item.pegawai.nama;
              this.absens.push(x);
          }
          else {
              const d = this.absens.find(x => x.idpegawai==item.pegawai.idpegawai);
              d.jampulang = x.jampulang;
              d.keterangan = x.keterangan;
          }


      },
          err => {
            this.swal.text = err.error;
            this.swal.type = 'error';
            this.swal.title = 'ERROR';
            this.swal.show();
           }
      );
  }

}
