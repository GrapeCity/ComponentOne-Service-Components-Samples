import { C1PaserTask } from './C1ParserTask';
import { IC1FileInput } from '../interface/IC1FileInput';
import { IC1Template } from '../interface/IC1Template';
import { C1ParserService } from '../../app/services/c1-parser.service';
import { C1ParserResult } from './C1ParserResult';

export class C1Parser {

    Tasks: Array<C1PaserTask> = [];
    Service: C1ParserService = null;
    isParsing: boolean = false;
    Callback: Function = null;
    constructor() { }

    setService(paserService: C1ParserService) {
        this.Service = paserService;
    }

    setTasks(emails: Array<IC1FileInput>, template: IC1Template) {
        this.Tasks = [];
        emails.forEach(email => {
            this.Tasks.push(new C1PaserTask(email, template));
        });
    }

    parse(emails: Array<IC1FileInput>, template: IC1Template, finishCallback: Function) {
        this.Tasks = [];
        emails.forEach(email => {
            this.Tasks.push(new C1PaserTask(email, template));
        });
        this._Start(finishCallback);
    }
    public Start(callback?: Function) {
        this._Start(callback);
    }
    private _Start(callback?: Function) {
        this.Callback = callback;
        let remainTask = this.Tasks.find(item => {
            return item.Status == C1ParserResult.STATUS_NONE;
        });

        if (!remainTask) {
            this.isParsing = false;
            if (this.Callback) {
                this.Callback();
            }
            this.Callback = null;
            return;
        }
        this.isParsing = true;
        remainTask.setStatus(C1ParserResult.STATUS_PARSING);

        this.Service.load().then(res => {
            this.Service.extractData(remainTask.Template, remainTask.Email).then(
                (data) => {
                    remainTask.setExtractData(data);
                }, (err) => {
                    remainTask.setExtractData(null);
                });
            this._Start(this.Callback);
        }, err => {
            remainTask.setExtractData(null);
            this._Start(this.Callback);
        });

    }
}