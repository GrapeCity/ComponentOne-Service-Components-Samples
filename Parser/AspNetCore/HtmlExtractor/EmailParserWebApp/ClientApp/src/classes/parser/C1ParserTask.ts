import { IC1FileInput } from '../interface/IC1FileInput';
import { IC1Template } from '../interface/IC1Template';
import { C1ParserResult } from './C1ParserResult';
import { C1Utils } from 'src/classes/base/C1Utils';

export class C1PaserTask {
    Email: IC1FileInput = null;
    Template: IC1Template = null;
    Status: number = -1;
    Result: C1ParserResult;

    constructor(email: IC1FileInput, template: IC1Template) {
        this.Email = email;
        this.Template = template;
    }
    getStatus(): number {
        return this.Status;
    }
    getStatusStr() {
        if (this.Status == C1ParserResult.STATUS_NONE) {
            return "";
        }
        else if (this.Status == C1ParserResult.STATUS_DONE) {
            return "Extracted";
        }
        else if (this.Status == C1ParserResult.STATUS_DONE) {
            return "Extracting..";
        }
        return "";
    }

    setStatus(status: number) {
        this.Status = status;
    }
    getResult(): C1ParserResult {
        return this.Result;
    }

    setExtractData(data) {
        this.setStatus(C1ParserResult.STATUS_DONE);
        if (data) {
            this.Result = new C1ParserResult(data);
        }
    }

    toJSONHTML() {
        let prettyContent: string = '';
        prettyContent += `<h4> ${this.Email.getName()}</h4>`;
        prettyContent += `<br>`;
        if (this.Status == C1ParserResult.STATUS_NONE) {
            prettyContent += `<p>Ready to parse</p>`;
        } else if (this.Status == C1ParserResult.STATUS_PARSING) {
            prettyContent += `<p>Processing ...</p>`;
        } else if (this.Status == C1ParserResult.STATUS_DONE) {
            if (this.Result && this.Result.Data) {
                var result = JSON.parse(this.Result.Data);
                if (result && result.Result) {
                    prettyContent += `<p><pre>${C1Utils.beautyJSON(JSON.stringify(result.Result, null, 4))}</pre></p>`;
                }
            }
        }
        return prettyContent;
    }
}