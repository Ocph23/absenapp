import { Component, OnInit, ViewChild } from '@angular/core';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { JabatanService } from '../../services/jabatan.service';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, Validators } from '@angular/forms';
import { jabatan } from '../../models/models.component';

@Component({
  selector: 'app-jabatan',
  templateUrl: './jabatan.component.html',
  styleUrls: ['./jabatan.component.scss', '../admin.component.scss']
})
export class JabatanComponent implements OnInit {
    jabatanForm: any;
    @ViewChild("swalInfo") swal: SwalComponent;
  Datas: jabatan[];
  constructor(public jabatanService: JabatanService,  private modalService: NgbModal, private fb: FormBuilder, ) { }

  ngOnInit() {

this.jabatanService.get().then(x=>{
this.Datas=x;

});

  }

  add(content: any) {
    this.jabatanForm = this.fb.group({
      'idjabatan': [0],
      'nama': [null, Validators.required],
      'tunjangan': [null, Validators.required],
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


    edit(item:jabatan ,content: any) {
        this.jabatanForm = this.fb.group({
            'idjabatan': item.idjabatan,
            'nama': [item.nama, Validators.required],
            'tunjangan': [item.tunjangan, Validators.required],
        });
        this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', backdrop: 'static' }).result.then((result) => {
            const closeResult = `Closed with: ${result}` ;
        }, (reason) => {
            const closeResult = `Dismissed ${this.getDismissReason(reason)}`;
        });
    }

  saveChange(data: jabatan) {
    this.jabatanService.SaveChange(data).subscribe(
      x => {
        if (data.idjabatan <= 0) {
            this.jabatanService.Datas.push(x);
        } else {
          data.nama = x.nama;
         
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
