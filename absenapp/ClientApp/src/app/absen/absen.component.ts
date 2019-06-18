import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-absen',
  templateUrl: './absen.component.html',
  styleUrls: ['./absen.component.scss']
})
export class AbsenComponent implements OnInit {
test:string;
  loginForm: any;
constructor(private router: Router, private fb: FormBuilder) {
  this.loginForm = fb.group({
    'userName': [null, Validators.required],
    'password': [null, Validators.required]
  });
}
  ngOnInit() {
  }

}
