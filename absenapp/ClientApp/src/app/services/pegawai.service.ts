import { Injectable, Inject } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';
import { pegawai } from '../models/models.component';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PegawaiService {
  private instance = false;
  Datas: pegawai[];
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
    return new Promise<pegawai[]>((p, r) => {
      try {
        if (!this.instance) {
          this.http
            .get<pegawai[]>(
              this.baseUrl + 'api/pegawai',
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

  SaveChange(model: pegawai) {
    try {
      if (model.idpegawai !== undefined && model.idpegawai > 0) {
        return this.http.put<pegawai>(
          this.baseUrl + 'api/pegawai',
          model,
          this.auth.getHttpHeader()
        );
      } else {
        return this.http.post<pegawai>(
          this.baseUrl + 'api/pegawai',
          model,
          this.auth.getHttpHeader()
        );
      }
    } catch (error) {
      console.log(error);
    }
  }

  async Delete(data: pegawai) {
    return await  this.http.delete<pegawai>(
      this.baseUrl + 'api/pegawai/' + data.idpegawai,
      this.auth.getHttpHeader()
    ).toPromise();
  }

}
