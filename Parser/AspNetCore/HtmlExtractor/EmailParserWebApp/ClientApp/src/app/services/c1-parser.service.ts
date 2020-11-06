import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpClient } from '@angular/common/http';
import { throwError } from 'rxjs';
import { IC1Template } from 'src/classes/interface/IC1Template';
import { IC1FileInput } from 'src/classes/interface/IC1FileInput';
import { ConfigService } from './config.service';
import { ParamsKey } from '../../classes/network/ParamsKey';
import { Cmd } from '../../classes/network/Command';

@Injectable({
  providedIn: 'root'
})
export class C1ParserService {

  public API_URL: string = "";

  constructor(private config: ConfigService, public _HttpClient: HttpClient) { }

  public load() {
    return new Promise((resolve, reject) => {
      if (this.API_URL) return resolve();
      this.config.loadConfig().then(res => {
        if (res) {
          this.API_URL = res["host"];
          return resolve();
        }
        return reject();
      }, error => {
        return reject();
      });
    });
  }


  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      console.error(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.error}`);
    }
    // return an observable with a user-facing error message
    return throwError('Something bad happened; please try again later.');
  }

  private parseResponse(res: Response) {
    let body = res;
    return body || {};
  }


  getListDefaultFiles(): Promise<any> {
    return new Promise((resolve, reject) => {
      this._HttpClient.get(this.API_URL + Cmd.DEFAULT_FILES).subscribe(res => {
        return resolve(res);
      }, err => {
        return reject(err);
      });
    });
  }
  
  extractData(template: IC1Template, file: IC1FileInput): Promise<any> {
    return new Promise((resolve, reject) => {
      var formData = new FormData();
      formData.append(ParamsKey.FILE_NAME, file.getName());
      formData.append(ParamsKey.FILE, file.getFile());
      if (template) {
        var content = template.writeXML().toString();
        var blob = new Blob([content], { type: "text/xml" });
        formData.append(ParamsKey.TEMPLATE, blob);
      }
      try {
        var request = new XMLHttpRequest();
        request.onreadystatechange = () => {
          if (request.readyState === 4) {
            if (request.response) {
              var response = JSON.parse(request.response);
              if (response && response[ParamsKey.STATUS] == 1) {
                return resolve(response[ParamsKey.CONTENT]);
              } else {
                return reject(response[ParamsKey.CONTENT]);
              }
            } else {
              return reject("Cannot send request to server");
            }
          }
        }
        request.open("POST", this.API_URL + Cmd.EXTRACT_DATA);
        request.send(formData);
      } catch (e) {
        return reject("Cannot send request to server");
      }
    });
  }
}
