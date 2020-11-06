import { Component, OnInit } from '@angular/core';
import { ParserService } from '../../providers/parser.service';
import { C1ParserService } from '../../services/c1-parser.service';
import { C1Parser } from '../../../classes/parser/C1Parser';
import { LoadingController } from '@ionic/angular';
import { ConfigService } from '../../services/config.service';

@Component({
  selector: 'app-parser',
  templateUrl: './parser.page.html',
  styleUrls: ['./parser.page.scss'],
})
export class ParserPage implements OnInit {

  parser: C1Parser = new C1Parser();
  viewIndex: number = 1;
  _Config = null;
  constructor(public configService : ConfigService, public loadingController: LoadingController, public parserService: ParserService, public c1ParserService: C1ParserService) { }

  ngOnInit() {
    this.parser = this.parserService.parser;
    this.parser.setService(this.c1ParserService);
    this.startParser();

    this.configService.loadConfig().then(data => {
      this._Config = this.configService.getData();
    }, error => { });
  }

  async startParser() {
    if (this.parser.Tasks.length > 0) {

      const loading = await this.loadingController.create({
        message: 'Please wait..',
        duration: 10000
      });
      await loading.present();

      this.parser.Start(() => {
        loading.dismiss();
        this.onParserFinished();
      });

    }
  }

  onClickViewData(view) {
    this.viewIndex = view;
  }

  onParserFinished() {

  }
}
