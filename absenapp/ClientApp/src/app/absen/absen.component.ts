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
    constructor(
      private router: Router, private fb: FormBuilder, 
      private absenService: AbsenService, public pegawaiService:PegawaiService) {
  this.loginForm = fb.group({
    'pegawai': [null, Validators.required],
  });
}
  ngOnInit() {

  }


    absenSave(event: any, item: any): void {
        const data: absen = { idpegawai: item.pegawai.idpegawai, status: 'masuk', jamdatang: new Date(), jampulang: new Date() } as absen;
      this.absenService.SaveChange(data).subscribe(x => {
        this.swal.text = 'Sukses';
        this.swal.type = 'info';
        this.swal.title = 'INFO';
        this.swal.show();

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
