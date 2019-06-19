import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  @ViewChild('swalMessage') swal:SwalComponent;
  public loginForm: FormGroup;
  constructor(private router: Router, private fb: FormBuilder, private auth: AuthService) {
    this.loginForm = fb.group({
      'userName': [null, Validators.required],
      'password': [null, Validators.required]
    });
  }
  ngOnInit() {
  }

  login(item) {
    this.auth.login(item.userName, item.password)
    .subscribe(x => {
      this.auth.storage.addObject('user', x);
      if (this.auth.IsInRole('admin')) {
        this.router.navigate(['/admin']);
      } else {
        this.router.navigate(['/bendahara']);
      }
    }, err => {
      this.swal.text=err.error.message;
      this.swal.type="error";
      this.swal.title="ERROR";
      this.swal.show();
    });
  }

}
