import { IC1TemplateBlock } from '../interface/IC1TemplateBlock';
import { IC1TemplateField } from '../interface/IC1TemplateField';

export class C1TemplateBlock implements IC1TemplateBlock {
    _Name: string;
    _Fields: Array<IC1TemplateField> = [];

    constructor(name: string) {
        this.setName(name);
    }

    getName(): string {
        return this._Name;
    }

    setName(name: string): void {
        this._Name = name;
    }
    getFields(): Array<IC1TemplateField> {
        return this._Fields;
    }
    setFields(fields: Array<IC1TemplateField>): void {
        this._Fields = fields;
    }
    addField(field: IC1TemplateField): void {
        this._Fields.push(field);
    }
    removeField(field: IC1TemplateField): void {
        if (!field) return;
        let index = this._Fields.indexOf(field);
        if (index != -1) this._Fields.splice(index, 1);
    }
    removeAllField(): void {
        this._Fields = [];
    }

    getFieldByName(name: string): IC1TemplateField {
        return this._Fields.find(ele => { return ele.getName().localeCompare(name) == 0; });
    }


}