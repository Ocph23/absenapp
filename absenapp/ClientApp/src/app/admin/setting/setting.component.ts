import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AbsenService } from '../../services/absen.service';
import { AbsenSetting } from '../../models/models.component';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';

@Component({
  selector: 'app-setting',
  templateUrl: './setting.component.html',
  styleUrls: ['./setting.component.css', '../admin.component.scss']
})
export class SettingComponent implements OnInit {
  settingForm: any;
  @ViewChild('swalMessage')  swal: SwalComponent;
  constructor(private fb: FormBuilder, private  absenService: AbsenService) {

    this.settingForm = this.fb.group({
      'idsetting': [0],
      'nominal': [null, Validators.required],
    });

   }

  ngOnInit() {
  }

  save(item: any) {
      const data: AbsenSetting = {nominal: item.nominal} as AbsenSetting;
      this.absenService.Setting(data).subscribe(x => {
        this.swal.text = 'Sukses';
        this.swal.title = 'Info';
        this.swal.type = 'info';
        this.swal.show();

      }, err => {
        this.swal.text = err.message;
        this.swal.title = 'Error';
        this.swal.type = 'error';
        this.swal.show();

      });
  }

}
