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
  namapegawai: string;
  idpegawai: number;

  jamdatang: string | Date;

  jampulang: string | Date;

  status: string;

  keterangan: string;

}

export interface pegawai {
  idpegawai: number;
  idjabatan: number;
  nama: string;
  nip: string;
  alamat: string;
  email: string;
  kontak: string;
  sex: string;
  jabatan: jabatan;
  bendahara: boolean;
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

export interface role {
  idrole: number;
  name: string;

}

export interface jabatan {
  idjabatan: number;
  nama: string;
  tunjangan: number;
}
