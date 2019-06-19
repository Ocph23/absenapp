import { Injectable, Inject } from '@angular/core';
import { jabatan } from '../models/models.component';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class JabatanService {
  private instance = false;
  Datas: jabatan[];
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
    return new Promise<jabatan[]>((p, r) => {
      try {
        if (!this.instance) {
          this.http
            .get<jabatan[]>(
              this.baseUrl + 'api/jabatan',
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

  SaveChange(model: jabatan) {
    try {
      if (model.idjabatan !== undefined && model.idjabatan > 0) {
        return this.http.put<jabatan>(
          this.baseUrl + 'api/jabatan',
          model,
          this.auth.getHttpHeader()
        );
      } else {
        return this.http.post<jabatan>(
          this.baseUrl + 'api/jabatan',
          model,
          this.auth.getHttpHeader()
        );
      }
    } catch (error) {
      console.log(error);
    }
  }

  async Delete(data: jabatan) {
    return await  this.http.delete<jabatan>(
      this.baseUrl + 'api/jabatan/' + data.idjabatan,
      this.auth.getHttpHeader()
    ).toPromise();
  }

}
