import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BendaharaComponent } from './bendahara.component';

describe('BendaharaComponent', () => {
  let component: BendaharaComponent;
  let fixture: ComponentFixture<BendaharaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BendaharaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BendaharaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
