import { ITextSelection } from './ITextSelection';
export interface IC1TemplateField {
    _Name: string;
    _XPath: string;
    _Selection: ITextSelection;
    _RepeatedFields: Array<IC1TemplateField>;
    getName(): string;
    setName(name: string): void;
    getSelection(): ITextSelection;
    setSelection(selection: ITextSelection);
    /**Get JSON string holding data for this Field, including NodeStart, NodeEnd, OffsetStart, OffsetEnd */
    getData(): string;
    dispose();
    create();

}