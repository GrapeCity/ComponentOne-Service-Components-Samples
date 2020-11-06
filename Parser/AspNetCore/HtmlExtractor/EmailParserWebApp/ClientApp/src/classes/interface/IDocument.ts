export interface IDocument {
    _Name : string;    
    getContentAsString(): string;
    getName(): string;
}