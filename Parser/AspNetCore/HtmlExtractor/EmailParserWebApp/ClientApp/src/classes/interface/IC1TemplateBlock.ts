import { IC1TemplateField } from './IC1TemplateField';

export interface IC1TemplateBlock {
    _Name: string;
    _Fields: Array<IC1TemplateField>;
    getName(): string;
    setName(name: string): void;
    getFields(): Array<IC1TemplateField>;
    setFields(fields: Array<IC1TemplateField>): void;
    addField(field: IC1TemplateField): void;
    removeField(field: IC1TemplateField): void;
    removeAllField(): void;
    getFieldByName(name : string) : IC1TemplateField;
}