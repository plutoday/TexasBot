import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewDigitControlComponent } from './new-digit-control.component';

describe('NewDigitControlComponent', () => {
  let component: NewDigitControlComponent;
  let fixture: ComponentFixture<NewDigitControlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewDigitControlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewDigitControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
