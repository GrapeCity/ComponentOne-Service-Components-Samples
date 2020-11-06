import { IC1Template } from '../interface/IC1Template';
import { IC1TemplateField } from '../interface/IC1TemplateField';
import { IC1TemplateBlock } from '../interface/IC1TemplateBlock';
import { IC1FileInput } from '../interface/IC1FileInput';
import { C1TemplateField } from './C1TemplateField';
import * as XMLWriter from 'xml-writer';

export class C1Template implements IC1Template {
    _Name: string = '';
    _Version: string = null;
    _File: IC1FileInput = null;
    _Fields: Array<IC1TemplateField> = [];
    _Blocks: Array<IC1TemplateBlock> = [];
    _XMLData: string = '';
    _Source: string = '';
    checked: boolean = false;

    constructor(name: string) {
        this.setName(name);
    }

    getName(): string {
        return this._Name;
    }
    setName(name: string): void {
        this._Name = name;
    }



    getFile(): IC1FileInput { return this._File; }
    setFile(file: IC1FileInput): void { this._File = file; }

    getFields(): Array<IC1TemplateField> { return this._Fields; }
    setFields(fields: Array<IC1TemplateField>) { this._Fields = fields; }

    getBlocks(): Array<IC1TemplateBlock> { return this._Blocks; }
    setBlocks(blocks: Array<IC1TemplateBlock>) { this._Blocks = blocks; }

    getFieldByName(name: string): IC1TemplateField {
        return this._Fields.find(ele => { return ele.getName().localeCompare(name) == 0; });
    }
    getBlockByName(name: string): IC1TemplateBlock {
        return this._Blocks.find(ele => { return ele.getName().localeCompare(name) == 0; });
    }

    addField(field: IC1TemplateField): void {
        if (!field) return;
        let ele = this.getFields().find(ele => {
            return (ele.getName().localeCompare(name) == 0)
        });
        if (ele) return;
        this.getFields().push(field);
    }

    addBlock(block: IC1TemplateBlock): void {
        if (!block) return;
        let ele = this.getBlocks().find(ele => {
            return (ele.getName().localeCompare(name) == 0)
        });
        if (ele) return;
        this.getBlocks().push(block);
    }
    removeField(field: C1TemplateField) {
        if (!field) return;
        let index: number = this._Fields.indexOf(field);
        if (index != -1) this._Fields.splice(index, 1);
    }
    removeBlock(block: IC1TemplateBlock) {
        if (!block) return;
        for (let field of block.getFields()) {
            field.dispose();
        }
        let index: number = this._Blocks.indexOf(block);
        if (index != -1) this._Blocks.splice(index, 1);
    }

    writeXML(): XMLWriter {
        if (this._XMLData) return this._XMLData;
        var xw = new XMLWriter();
        xw.startDocument();
        var root = xw.startElement('template');

        root.writeAttribute('name', this.getName());

        var placeHolders = root.startElement('placeHolders');
        for (let field of this._Fields) {
            placeHolders.startElement('fixedPlaceHolder')
                .writeAttribute('name', field.getName())
                .writeAttribute('text', field.getSelection()._Text)
                .writeAttribute('xPath', field.getSelection()._XPath)
                .writeAttribute('selectionBeginIndex', field.getSelection()._AnchorOffset)
                .writeAttribute('selectionLength', field.getSelection()._Text.length)
                .writeAttribute('data', field.getData())
                .endElement();
        }
        for (let block of this._Blocks) {
            var blockPlaceHolders = placeHolders.startElement('repeatedBlock').writeAttribute('name', block.getName());

            for (let field of block._Fields) {

                let blockField = blockPlaceHolders.startElement('repeatedPlaceHolder');
                blockField.writeAttribute('name', field.getName())
                    .writeAttribute('text', field.getSelection()._Text)
                    .writeAttribute('xPath', field.getSelection()._XPath)
                    .writeAttribute('selectionBeginIndex', field.getSelection()._AnchorOffset)
                    .writeAttribute('selectionLength', field.getSelection()._Text.length)
                    .writeAttribute('data', field.getData());
                if (field._RepeatedFields.length > 0) {
                    blockField.writeAttribute('xPathInNextBlock', field._RepeatedFields[0].getSelection()._XPath)
                        .writeAttribute('nextBlockData', field._RepeatedFields[0].getData())
                        .writeAttribute('nextBlockAnchor', field._RepeatedFields[0].getSelection()._AnchorOffset);
                }
                blockField.endElement();
            }
            placeHolders.endElement();
        }
        root.endElement();
        root.writeElement('source', this._Source);
        root.writeElement('source_name', this._File ? this._File.getName() : "default.html");
        if (this._Version) root.writeElement('version', this._Version);
        xw.endDocument();
        this._XMLData = xw.toString();
        return xw;
    }

}