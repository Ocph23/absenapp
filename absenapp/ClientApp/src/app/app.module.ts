import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import { fas } from '@fortawesome/free-solid-svg-icons';
import { far } from '@fortawesome/free-regular-svg-icons';
import { fab, faFacebookF, faFacebook, faTwitter, faInstagram } from '@fortawesome/free-brands-svg-icons';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { AdminComponent } from './admin/admin.component';
import { AbsenComponent } from './absen/absen.component';
import { PegawaiComponent } from './admin/pegawai/pegawai.component';
import { SettingComponent } from './admin/setting/setting.component';
import { LaporanComponent } from './admin/laporan/laporan.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AdminHomeComponent } from './admin/admin-home/admin-home.component';
import { HelperComponent } from './helper/helper.component';
import { ModelsComponent } from './models/models.component';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent,
    AdminComponent,
    AbsenComponent,
    PegawaiComponent,
    SettingComponent,
    LaporanComponent,
    AdminHomeComponent,
    HelperComponent,
    ModelsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule, FontAwesomeModule, NgbModule, SweetAlert2Module.forRoot(),
    FormsModule,ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'login', component: LoginComponent },
      { path: 'absen', component: AbsenComponent},
      { path: 'admin', component: AdminComponent,
      children: [
        { path: '', component: AdminHomeComponent },
        { path: 'pegawai', component: PegawaiComponent },
        { path: 'setting', component: SettingComponent },
        { path: 'laporan', component: LaporanComponent },
      ] }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor() {
    // Add an icon to the library for convenient access in other components
    library.add(fas, far, fab);
  }
 }
