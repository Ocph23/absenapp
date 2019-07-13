import { Component, OnInit, ViewChild, } from '@angular/core';
import { PegawaiService } from '../../services/pegawai.service';
import { NgbModal, ModalDismissReasons, NgbTimeStruct, NgbDate, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, Validators } from '@angular/forms';
import { pegawai, absen } from '../../models/models.component';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { JabatanService } from '../../services/jabatan.service';
import { AbsenService } from '../../services/absen.service';


@Component({
  selector: 'app-pegawai',
  templateUrl: './pegawai.component.html',
  styleUrls: ['./pegawai.component.scss', '../admin.component.scss']
})


export class PegawaiComponent implements OnInit {
  pegawaiForm: any;
  @ViewChild('swalInfo') swal: SwalComponent;
  SelectedEdit: pegawai;
  Absens: absen[];
  DetailTitle: string;
  SelectedPegawai: pegawai;
  timeSetting: any = { hourStep: 1, minuteStep: 15, secondStep: 30 };
  page = 1;
  pageSize = 10;
  collectionSize: number;

  absenPage = 1;
  absenPageSize = 10;
  absenCollectionSize: number;


  tahun: number;
  bulan: number;
  tahuns: number[] = [];
  bulans: number[] = [];
  absenForm: any;
  constructor(public pegawaiService: PegawaiService, public activeModal: NgbActiveModal,
    public jabatanService: JabatanService,
    private absenServices: AbsenService,
    private modalService: NgbModal, private fb: FormBuilder, ) {

    for (let i = 1; i <= 12; i++) {
      this.bulans.push(i);
      if (i < 4) {
        this.tahuns.push(new Date().getFullYear() - (i - 1));
      }
    }
  }

  ngOnInit() {

  }

  get pegawais(): pegawai[] {
    if (this.pegawaiService.Datas != null) {
      this.collectionSize = this.pegawaiService.Datas.length;
      return this.pegawaiService.Datas
        .map((country, i) => ({ id: i + 1, ...country }))
        .slice((this.page - 1) * this.pageSize, (this.page - 1) * this.pageSize + this.pageSize);
    }
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
      'bendahara': false,
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
    this.DetailTitle = item.nama;
    this.SelectedPegawai = item;
    this.tahun = new Date().getFullYear();
    this.bulan = new Date().getMonth() + 1;
    this.onChange(item);
    this.modalService.open(content, { backdrop: 'static', windowClass: 'xlModal', size: <any>'xl' }).result.then((result) => {
      const closeResult = `Closed with: ${result}`;
    }, (reason) => {
      const closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }




  onChange(item: pegawai): void {
    this.Absens = [];

    this.Absens = this.absenServices.Datas.filter(x => x.idpegawai === item.idpegawai &&
      new Date(x.jamdatang).getFullYear() === this.tahun &&
      new Date(x.jamdatang).getMonth() + 1 === this.bulan).map((country, i) => ({ id: i + 1, ...country }))
      .slice((this.absenPage - 1) * this.absenPageSize, (this.page - 1) * this.absenPageSize + this.absenPageSize);
    this.absenCollectionSize = this.Absens.length;
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
        this.swal.text = 'Sukses';
        this.swal.title = 'Info';
        this.swal.type = 'info';
        this.swal.show();
        this.modalService.dismissAll();
      },
      e => {

        this.swal.text = e.error;
        this.swal.title = 'Error';
        this.swal.type = 'error';
        this.swal.show();

      });
  }

  EditAbsen(item: any, content: any) {
    const datang = new Date(item.jamdatang);
    const pulang = new Date(item.jampulang);
    const tanggal = { 'year': datang.getFullYear(), 'month': datang.getMonth() + 1, 'day': datang.getDate() };
    const datangJam = { hour: datang.getHours(), minute: datang.getMinutes(), second: datang.getSeconds() };
    const pulangJam = { hour: pulang.getHours(), minute: pulang.getMinutes(), second: pulang.getSeconds() };

    this.absenForm = this.fb.group({
      'idpegawai': item.idpegawai,
      'idabsen': item.idabsen,
      'tanggal': tanggal,
      'status': [item.status, Validators.required],
      'datang': [datangJam, Validators.required],
      'pulang': [pulangJam, Validators.required],
      'keterangan': [item.keterangan, Validators.required],
    });

    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', backdrop: 'static' }).result.then((result) => {
      const closeResult = `Closed with: ${result}`;
    }, (reason) => {
      const closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }

  AddAbsen(item: any, content: any) {
    const datang = new Date();
    const tanggal = { 'year': datang.getFullYear(), 'month': datang.getMonth() + 1, 'day': datang.getDate() };
    const datangJam = { hour: datang.getHours(), minute: datang.getMinutes(), second: datang.getSeconds() };
    const pulangJam = { hour: datang.getHours(), minute: datang.getMinutes(), second: datang.getSeconds() };
    item.status = 'masuk';

    this.absenForm = this.fb.group({
      'idpegawai': item.idpegawai,
      'idabsen': 0,
      'tanggal': tanggal,
      'status': [item.status, Validators.required],
      'datang': [datangJam, Validators.required],
      'pulang': [pulangJam, Validators.required],
      'keterangan': [''],
    });

    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', backdrop: 'static' }).result.then((result) => {
      const closeResult = `Closed with: ${result}`;
    }, (reason) => {
      const closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }

  saveAbsen(item: any, data: any) {
    const datang = item.datang as NgbTimeStruct;
    const pulang = item.pulang as NgbTimeStruct;
    const model: absen = { idabsen: item.idabsen, idpegawai: item.idpegawai, status: item.status } as absen;

    model.jamdatang = new Date(item.tanggal.year, item.tanggal.month - 1, item.tanggal.day);
    model.jampulang = new Date(item.tanggal.year, item.tanggal.month - 1, item.tanggal.day);

    model.jamdatang.setHours(item.datang.hour);
    model.jamdatang.setMinutes(item.datang.minute);
    model.jamdatang.setSeconds(item.datang.second);

    model.jampulang.setHours(item.pulang.hour);
    model.jampulang.setMinutes(item.pulang.minute);
    model.jampulang.setSeconds(item.pulang.second);
    model.keterangan = item.keterangan;
    model.idabsen = item.idabsen;
    model.idpegawai = item.idpegawai;

    this.absenServices.SaveChange(model).subscribe(x => {
      this.swal.text = 'Sukses';
      this.swal.title = 'Info';
      this.swal.type = 'info';
      this.swal.show();
      this.activeModal.dismiss();

    }, e => {
      this.swal.text = e.error;
      this.swal.title = 'Error';
      this.swal.type = 'error';
      this.swal.show();
    });

  }

}
