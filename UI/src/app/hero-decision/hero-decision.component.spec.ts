import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HeroDecisionComponent } from './hero-decision.component';

describe('HeroDecisionComponent', () => {
  let component: HeroDecisionComponent;
  let fixture: ComponentFixture<HeroDecisionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HeroDecisionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HeroDecisionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
