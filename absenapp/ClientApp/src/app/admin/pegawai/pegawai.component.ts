import { Component, OnInit, ViewChild } from '@angular/core';
import { PegawaiService } from '../../services/pegawai.service';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, Validators } from '@angular/forms';
import { pegawai, absen } from '../../models/models.component';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { JabatanService } from '../../services/jabatan.service';
import { AbsenService } from '../../services/absen.service';
import { from } from 'rxjs';
import { groupBy, mergeMap, toArray } from 'rxjs/operators';

@Component({
  selector: 'app-pegawai',
  templateUrl: './pegawai.component.html',
  styleUrls: ['./pegawai.component.css', '../admin.component.scss']
})
export class PegawaiComponent implements OnInit {
  pegawaiForm: any;
  @ViewChild("swalInfo") swal: SwalComponent;
  SelectedEdit: pegawai;
  Absens: absen[];
  DetailTitle:string;
  tahun = new Date().getFullYear();
  bulan = new Date().getMonth() + 1;


  constructor(public pegawaiService: PegawaiService,
    public jabatanService: JabatanService,
    private absenServices: AbsenService,
    private modalService: NgbModal, private fb: FormBuilder, ) {

  }

  ngOnInit() {

  }

  addPegawai(content: any) {
    this.pegawaiForm = this.fb.group({
      'idpegawai': [0],
      'idjabatan': [0, Validators.required],
      'nama': [null, Validators.required],
      'nip': [null, Validators.required],
      'email': [null, Validators.required],
      'kontak': [null, Validators.required],
      'alamat': [null, Validators.required],
      'sex': [null, Validators.required],
      'jabatan': [null, Validators.required],
    });
    this.modalService.open(content, {
      ariaLabelledBy: 'modal-basic-title', backdrop: 'static'
    }).result.then((result) => {
      const closeResult = `Closed with: ${result}`;
    }, (reason) => {
      const closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }
  getDismissReason(reason: any) {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  Detail(item: pegawai, content: any) {
    this.DetailTitle=item.nama;
   
    this.Absens = [];
    this.Absens= this.absenServices.Datas.filter(x => x.idpegawai === item.idpegawai && new Date(x.jamdatang).getFullYear() === this.tahun &&
      new Date(x.jamdatang).getMonth() + 1 == this.bulan);

    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', backdrop: 'static', size: 'lg' }).result.then((result) => {
      const closeResult = `Closed with: ${result}`;
    }, (reason) => {
      const closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }


  editPegawai(item: pegawai, content: any) {
    this.SelectedEdit = item;
    this.pegawaiForm = this.fb.group({
      'idpegawai': item.idpegawai,
      'idjabatan': item.idjabatan,
      'nama': [item.nama, Validators.required],
      'nip': [item.nip, Validators.required],
      'email': [item.email, Validators.required],
      'alamat': [item.alamat, Validators.required],
      'sex': [item.sex, Validators.required],
      'kontak': [item.kontak, Validators.required],
      'jabatan': [item.jabatan, Validators.required],
    });
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', backdrop: 'static' }).result.then((result) => {
      const closeResult = `Closed with: ${result}`;
    }, (reason) => {
      const closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }

  saveChange(data: pegawai, Selected: pegawai) {
    data.idjabatan = data.jabatan.idjabatan;
    this.pegawaiService.SaveChange(data).subscribe(
      x => {
        if (data.idpegawai <= 0) {
          this.pegawaiService.Datas.push(x);
        } else {
          Selected.nama = data.nama;
          Selected.alamat = data.alamat;
          Selected.nip = data.nip;
          Selected.jabatan = data.jabatan;
          Selected.email = data.email;
          Selected.sex = data.sex;
          Selected.kontak = data.kontak;
          Selected.idjabatan = data.idjabatan;
        }
        this.swal.text = "Sukses";
        this.swal.title = "Info"
        this.swal.type = "info";
        this.swal.show();
        this.modalService.dismissAll();
      },
      e => {

        this.swal.text = e.error;
        this.swal.title = "Error"
        this.swal.type = "error";
        this.swal.show();

      });
  }

}
