import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  public loginForm: FormGroup;
  constructor(private router: Router, private fb: FormBuilder) {
    this.loginForm = fb.group({
      'userName': [null, Validators.required],
      'password': [null, Validators.required]
    });
  }
  ngOnInit() {
  }

  
  login(item) {
    
  }

}
