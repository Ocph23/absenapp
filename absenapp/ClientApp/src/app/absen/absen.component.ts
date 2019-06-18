import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { AbsenService } from '../services/absen.service';
import { absen } from '../models/models.component';

@Component({
  selector: 'app-absen',
  templateUrl: './absen.component.html',
  styleUrls: ['./absen.component.scss']
})
export class AbsenComponent implements OnInit {
test:string;
    loginForm: any;
    constructor(private router: Router, private fb: FormBuilder, private absenService:AbsenService) {
  this.loginForm = fb.group({
    'userName': [null, Validators.required],
  });
}
  ngOnInit() {
  }


  absenSave(event:any, item:any):void
  {
      const data: absen = { idpegawai: 1 } as absen;
      this.absenService.SaveChange(data).subscribe(x => {

      },
          err => { }
      );
  }

  handleRefusal(item:any):void{

  }

}
