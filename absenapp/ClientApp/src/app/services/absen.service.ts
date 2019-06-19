import { Injectable, Inject } from '@angular/core';
import { absen, AbsenSetting } from '../models/models.component';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AbsenService {
  private instance = false;
  Datas: absen[];
  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private router: Router,
    private auth: AuthService
  ) {
    this.get();
  }

  public get() {
    return new Promise<absen[]>((p, r) => {
      try {
        if (!this.instance) {
          this.http
            .get<absen[]>(
              this.baseUrl + 'api/absen',
              this.auth.getHttpHeader()
            )
            .subscribe(
              result => {
                this.instance = true;
                this.Datas = result;
                p(result);
              },
              error => {
                throw new Error(error);
              }
            );
        } else {
          p(this.Datas);
        }
      } catch (error) {
        r(error);
      }
    });
  }

    SaveChange(model: absen) {
    try {
        return this.http.post<absen>(
            this.baseUrl + '/api/absen', model,
            this.auth.getHttpHeader()
        );
    } catch (error) {
      console.log(error);
    }
  }

  async Delete(data: absen) {
    return await  this.http.delete<absen>(
      this.baseUrl + 'api/absen/' + data.idpegawai,
      this.auth.getHttpHeader()
    ).toPromise();
  }


  Setting(model: AbsenSetting) {
    try {
        return this.http.post<AbsenSetting>(
            this.baseUrl + '/api/absen/setting', model,
            this.auth.getHttpHeader()
        );
    } catch (error) {
      console.log(error);
    }}
}
