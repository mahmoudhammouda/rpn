/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AppCalculatorComponent } from './app-calculator.component';

describe('AppCalculatorComponent', () => {
  let component: AppCalculatorComponent;
  let fixture: ComponentFixture<AppCalculatorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppCalculatorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppCalculatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
