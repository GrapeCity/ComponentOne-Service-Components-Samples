import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class FileService {

  constructor(public httpClient: HttpClient) {

  }

  public blobToFile(theBlob: Blob, fileName: string): File {
    var b: any = theBlob;
    //A Blob() is almost a File() - it's just missing the two properties below which we will add
    b.lastModifiedDate = new Date();
    b.name = fileName;

    //Cast to a File() type
    return <File>theBlob;
  }

  public loadFile(filePath: string, callback) {
    this.httpClient.get(filePath, {
      headers: new HttpHeaders(),
      responseType: 'blob'
    }).subscribe(blob => {
      if (blob) {
        let file = this.blobToFile(blob, filePath.substring(filePath.lastIndexOf("/") + 1, filePath.length));
        if (file) {
          if (callback) callback(file);
        }
      }
    }, err => {
    });
  }
  public loadFiles(filePaths: Array<string>, callback) {
    filePaths.forEach(filePath => {
      this.loadFile(filePath, callback);
    });
  }
  public loadDefaultHtmlFiles(callback) {
    var filesToLoad = [
      "assets/html/amazonEmail1.html",
      "assets/html/amazonEmail2.html",
      "assets/html/bookingEmail1.html",
      "assets/html/bookingEmail2.html",
      "assets/html/bookingEmail3.html",
      "assets/html/vietjetairEmail1.html",
      "assets/html/vietjetairEmail2.html"
    ];
    this.loadFiles(filesToLoad, callback);
  }

  public loadDefaultTemplateFiles(callback) {
    var filesToLoad = [
      "assets/template/AmazonTemplate.xml",
    ];
    this.loadFiles(filesToLoad, callback);
  }
}
