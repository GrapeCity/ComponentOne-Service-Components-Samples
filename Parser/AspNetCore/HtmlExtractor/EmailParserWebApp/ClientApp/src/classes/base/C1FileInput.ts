import { IC1FileInput } from '../interface/IC1FileInput';
export class C1FileInput implements IC1FileInput {
    _ID: string = '';
    _Name: string = '';
    _File: File = null;
    checked: boolean = false;
    constructor(file: File) {
        this.setFile(file);
    }

    public setFile(file: File) {
        this._File = file;
        this.setName(file ? file.name : '');
    }

    public setName(name: string) {
        this._Name = name;
        this._ID = name.toLowerCase().replace(/ /g, '');
    }

    public getID(): string {
        return this._ID;
    }

    public getName(): string {
        return this._Name;
    }

    public getFile(): File {
        return this._File;
    }


}