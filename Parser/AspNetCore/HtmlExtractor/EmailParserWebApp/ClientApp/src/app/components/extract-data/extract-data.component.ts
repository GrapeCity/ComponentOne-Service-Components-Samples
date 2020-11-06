import { Component, OnInit, Input } from '@angular/core';
import { NavParams } from '@ionic/angular';

@Component({
  selector: 'app-extract-data',
  templateUrl: './extract-data.component.html',
  styleUrls: ['./extract-data.component.scss'],
})
export class ExtractDataComponent implements OnInit {
  @Input() content: string;
  constructor(navParams: NavParams) { }

  ngOnInit() {}

}
