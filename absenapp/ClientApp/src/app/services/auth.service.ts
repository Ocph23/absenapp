import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpEventType } from '@angular/common/http';
import { Router } from '@angular/router';
import { map } from 'rxjs/operators';
import { StorageHelper } from '../helper/helper.component';
import { pegawai, User } from '../models/models.component';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public storage: StorageHelper.LocalStorageWorker = new StorageHelper.LocalStorageWorker();

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private router: Router
  ) { }

  login(username: string, password: string) {
    const data = { username: username, Password: password };
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

    return this.http.post<any>(
      this.baseUrl + 'account/authenticate',
      data,
      httpOptions
    );
  }

  logout(): any {
    this.storage.clear();
  }

  register(tourist: pegawai) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

    return this.http.post<any>(
      this.baseUrl + 'account/register',
      tourist,
      httpOptions
    );
  }

 reset(email: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

   return this.http.post<any>(
      this.baseUrl + 'account/resetpassword?email=' + email,
      httpOptions
    );
  }


  public userName(): string {
    const userData = this.storage.getObject('user');
    if (userData != null) {
      const userName = userData.username;
      return userName;
    } else {
      return null;
    }
  }

  public hasLogin() {
    if (this.getToken() != null) {
      return true;
    } else {
      return false;
    }
  }

  public getToken(): string {
    const userData = this.storage.getObject('user');
    if (userData != null) {
      const token = userData.token;
      return token;
    } else {
      return null;
    }
  }

  getUserId(): number {
    const userData = this.storage.getObject('user') as User;
    if (userData != null) {
      return userData.iduser;
    } else {
      return null;
    }
  }


  changeTouristAvatar(formData: FormData) {
    const token = this.getToken();
    if (token) {
      const httpOptions = {
        headers: new HttpHeaders({
          Authorization: 'Bearer ' + token,
        })
      };

      return this.http.post(this.baseUrl + 'api/image/avatar', formData, httpOptions)
        .subscribe(result => {
          console.log(result);
        }, error => {
          console.log(error);
        });
    }
  }

  public getHttpHeader() {
    try {
      const token = this.getToken();
      if (token) {
        const httpOptions = {
          headers: new HttpHeaders({
            'Content-Type': 'application/json',
            Authorization: 'Bearer ' + token
          })
        };


        return httpOptions;
      } else {
        const httpOptions = {
          headers: new HttpHeaders({
            'Content-Type': 'application/json'
          })
        };
        return httpOptions;
      }
    } catch (error) {
      throw new Error(error);
    }
  }

  public IsInRole(item: string): boolean {
    try {
      const userData = this.storage.getObject('user');
      let found = false;
      if (userData.roles != null) {
        userData.roles.forEach(element => {
          if (element.name === item) {
            found = true;
          }
        });
      }
      return found;
    } catch (e) {
      this.router.navigate(['user']);
    }
  }
}
