import { Component } from '@angular/core';

@Component({
  selector: 'app-models',
  template: ''
})
export class ModelsComponent {
  constructor() { }
}

export interface absen {
  idabsen: number;

  idpegawai: number;

  jamdatang: Date | string;

  jampulang: Date | string;

  keterangan:string;

}



export interface AbsenSetting {
  idsetting: number;

  nominal: number;

}



export interface pegawai {
  idpegawai: number;

  nama: string;

  nip: string;

  alamat: string;

  email: string;

  jabatan: string;

  password: string;

  sex: string;
}

export interface User {
  iduser: number;

  username: string;

  password: string;

  avatar: string;

  PasswordHash: string;

  PasswordSalt: string;

  token: string;

  roles: role[];

  name: string;

}

export interface role 
{
       idrole: number; 

       name: string; 

  }