import { Component, OnInit, ViewChild } from '@angular/core';
import { PegawaiService } from '../../services/pegawai.service';
import { NgbModal,ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, Validators } from '@angular/forms';
import { pegawai } from '../../models/models.component';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';

@Component({
  selector: 'app-pegawai',
  templateUrl: './pegawai.component.html',
  styleUrls: ['./pegawai.component.css', '../admin.component.scss']
})
export class PegawaiComponent implements OnInit {
    pegawaiForm: any;
    @ViewChild("swalInfo") swal: SwalComponent;
  constructor(public pegawaiService: PegawaiService,  private modalService: NgbModal, private fb: FormBuilder, ) { }

  ngOnInit() {

  }

  addPegawai(content: any) {
    this.pegawaiForm = this.fb.group({
      'idpegawai': [0],
      'nama': [null, Validators.required],
      'nip': [null, Validators.required],
      'email': [null, Validators.required],
      'alamat': [null, Validators.required],
      'sex': [null, Validators.required],
      'jabatan': [null, Validators.required],
    });
      this.modalService.open(content, {
          ariaLabelledBy: 'modal-basic-title', backdrop: 'static' }).result.then((result) => {
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
      return  `with: ${reason}`;
    }
  }


    editPegawai(item:pegawai ,content: any) {
        this.pegawaiForm = this.fb.group({
            'idpegawai': item.idpegawai,
            'nama': [item.nama, Validators.required],
            'nip': [item.nip, Validators.required],
            'email': [item.email, Validators.required],
            'alamat': [item.alamat, Validators.required],
            'sex': [item.sex, Validators.required],
            'jabatan': [item.jabatan, Validators.required],
        });
        this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', backdrop: 'static' }).result.then((result) => {
            const closeResult = `Closed with: ${result}`;
        }, (reason) => {
            const closeResult = `Dismissed ${this.getDismissReason(reason)}`;
        });
    }

  saveChange(data: pegawai) {
    this.pegawaiService.SaveChange(data).subscribe(
      x => {
        if (data.idpegawai <= 0) {
            this.pegawaiService.Datas.push(x);
        } else {
          data.nama = x.nama;
          data.alamat = x.alamat;
          data.nip = x.nip;
          data.jabatan = x.jabatan;
          data.email = x.email;
          data.sex = x.sex;
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
