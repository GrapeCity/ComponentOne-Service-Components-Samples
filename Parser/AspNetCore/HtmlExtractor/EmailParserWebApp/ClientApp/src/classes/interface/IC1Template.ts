import { IC1FileInput } from './IC1FileInput';
import { IC1TemplateField } from './IC1TemplateField';
import { IC1TemplateBlock } from './IC1TemplateBlock';
import * as XMLWriter from 'xml-writer';

export interface IC1Template {
    _Name: string;
    _File: IC1FileInput;
    _Fields: Array<IC1TemplateField>;
    _Blocks: Array<IC1TemplateBlock>;
    _XMLData: string;
    _Source: string;
    checked : boolean;

    getName(): string;
    setName(name: string): void;

    getFile(): IC1FileInput;
    setFile(file: IC1FileInput): void;

    getFields(): Array<IC1TemplateField>;
    setFields(fields: Array<IC1TemplateField>);

    getBlocks(): Array<IC1TemplateBlock>;
    setBlocks(blocks: Array<IC1TemplateBlock>);

    addField(field: IC1TemplateField): void;
    addBlock(block: IC1TemplateBlock): void;

    getFieldByName(name: string): IC1TemplateField;
    getBlockByName(name: string): IC1TemplateBlock;

    removeField(field: IC1TemplateField);
    removeBlock(block: IC1TemplateBlock);

    writeXML(): XMLWriter;

}