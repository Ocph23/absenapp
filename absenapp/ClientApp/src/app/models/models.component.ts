import { Component } from '@angular/core';

@Component({
  selector: 'app-models',
  template:''
})
export class ModelsComponent  {
  constructor() { }
}

export interface absen
    {
        idabsen: number;

        idpegawai: number;

        jamdatang: Date | string;

        jampulang: Date | string;

        keterangan: Date | string;

    }



    export interface AbsenSetting
    {
        idsetting: number;

        nominal: number;

    }



    export interface pegawai
    {
        idpegawai: number;

        nama: string;

        nip: string;

        alamat: string;

        email: string;

        jabatan: string;

        password: string;

        sex: string;
    }