import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewDigitPanelComponent } from './new-digit-panel.component';

describe('NewDigitPanelComponent', () => {
  let component: NewDigitPanelComponent;
  let fixture: ComponentFixture<NewDigitPanelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewDigitPanelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewDigitPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
