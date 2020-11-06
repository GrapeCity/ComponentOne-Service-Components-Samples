import { Injectable } from '@angular/core';
import { C1Parser } from 'src/classes/parser/C1Parser';

@Injectable({
  providedIn: 'root'
})
export class ParserService {
  parser: C1Parser = new C1Parser();

  constructor() { }

  getParser() {
    return this.parser;
  }

}
