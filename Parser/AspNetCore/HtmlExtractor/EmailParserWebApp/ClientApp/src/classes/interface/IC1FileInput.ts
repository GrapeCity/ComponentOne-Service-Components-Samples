export interface IC1FileInput {
    _ID: string;
    _Name: string;
    _File: File;
    checked : boolean;

    setFile(file: File);

    getID(): string;

    getName(): string;

    getFile(): File;
}