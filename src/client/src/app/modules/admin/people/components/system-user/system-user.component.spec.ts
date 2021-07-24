import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SystemUserComponent } from './system-user.component';

describe('SystemUserComponent', () => {
  let component: SystemUserComponent;
  let fixture: ComponentFixture<SystemUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SystemUserComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SystemUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
