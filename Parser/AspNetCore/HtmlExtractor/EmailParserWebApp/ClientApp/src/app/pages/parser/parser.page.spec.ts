import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ParserPage } from './parser.page';

describe('ParserPage', () => {
  let component: ParserPage;
  let fixture: ComponentFixture<ParserPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ParserPage ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ParserPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
