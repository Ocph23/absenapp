import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
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
    }, error => {
      alert(error.error);
    });
  }

}
