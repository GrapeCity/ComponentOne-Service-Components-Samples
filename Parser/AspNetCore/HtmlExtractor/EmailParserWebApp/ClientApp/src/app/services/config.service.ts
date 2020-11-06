import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  private _Data = null;
  constructor(public httpClient: HttpClient) { }

  loadConfig() {
    return new Promise((resolve, reject) => {
      if (this._Data) return resolve(this._Data);
      this.httpClient.get("assets/config.json", {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe(data => {
        this._Data = data;
        return resolve(this._Data);
      }, error => {
        reject(error);
      });
    });
  }

  getData(){
    return this._Data;
  }
}
