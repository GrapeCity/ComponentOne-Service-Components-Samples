import { IDocument } from './IDocument';
import { IC1Template } from './IC1Template';
export interface IC1Parser {
    _Document: IDocument;
    _ITemplate: IC1Template;

    getTemplate(): IC1Template;
    setTemplate(template: IC1Template): void;

    getDocument(): IDocument;
    setDocument(document: IDocument): void;
    
}