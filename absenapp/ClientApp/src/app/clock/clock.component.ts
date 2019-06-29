import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-clock',
  templateUrl: './clock.component.html',
  styleUrls: ['./clock.component.scss']
})
export class ClockComponent implements OnInit {
  inc=1000;
  
  date:Date = new Date();

  hours = ((this.date.getHours() + 11) % 12 + 1);
  minutes = this.date.getMinutes();
  seconds = this.date.getSeconds();
  
  hour = this.hours * 30;
  minute = this.minutes * 6;
  second = this.seconds * 6;
  hari:string;
  
  
  constructor() { }

  ngOnInit() {
    this.clock();
            setInterval(() => {
                this.clock(); 
                }, 1000);
  }


  
clock() {
  this.date=new Date();
  this. hours = ((this.date.getHours() + 11) % 12 + 1);
  this.minutes = this.date.getMinutes();
  this.seconds = this.date.getSeconds();
  this. hour = this.hours * 30;
  this. minute = this.minutes * 6;
  this. second = this.seconds * 6;
  }

  getHari() {
    var d = new Date();
    var weekday = new Array(7);
    weekday[0] = "Minggu";
    weekday[1] = "Senin";
    weekday[2] = "Selasa";
    weekday[3] = "Rabu";
    weekday[4] = "Kamis";
    weekday[5] = "Jumat";
    weekday[6] = "Sabtu";
  
    return weekday[d.getDay()];
  }

  getDate(){
    return this.date;
  }

 

}
