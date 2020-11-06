import { IC1Template } from '../interface/IC1Template';
import { parseString } from 'xml2js';
import { C1Utils } from './C1Utils';
import { C1TextSelection } from './C1TextSelection';
import { C1TemplateField } from './C1TemplateField';
import { C1TemplateBlock } from './C1TemlateBlock';

export class C1TemplateBuilder {
    _Data = null;
    constructor() {

    }
    rebuildFields(template: IC1Template, doc: Document) {
        if (!this._Data || !this._Data.hasOwnProperty("template")) return;
        let templateNode = this._Data["template"];
        if (!templateNode) return;
        if (templateNode.hasOwnProperty("placeHolders")) {
            let placeHolders = templateNode["placeHolders"];
            if (placeHolders && placeHolders.length > 0) {
                placeHolders.forEach(placeHolder => {
                    if (placeHolder) {
                        if (placeHolder.hasOwnProperty("fixedPlaceHolder")) {
                            let fixedPlaceHolders = placeHolder["fixedPlaceHolder"];
                            if (fixedPlaceHolders) {
                                fixedPlaceHolders.forEach(element => {
                                    let name = this._GetAttribute(element, "name", "PlaceHolderName");
                                    let dataStr = this._GetAttribute(element, "data", null);
                                    if (dataStr) {
                                        var fieldData = JSON.parse(dataStr);

                                        // Recreate range
                                        let range = doc.createRange();
                                        range.setStart(C1Utils.GetElementFromXPath(fieldData.xPathStart, true, doc), fieldData.startOffset);
                                        range.setEnd(C1Utils.GetElementFromXPath(fieldData.xPathEnd, true, doc), fieldData.endOffset);
                                        var selection = window.getSelection();
                                        selection.removeAllRanges();
                                        selection.addRange(range);

                                        // ReCreate selection and data
                                        let c1Selection = new C1TextSelection(doc);
                                        c1Selection.onSelection(selection);
                                        c1Selection._RangeData = fieldData;
                                        c1Selection._XPath = this._GetAttribute(element, "xPath", "");
                                        c1Selection._Text = fieldData.text;
                                        c1Selection._AnchorOffset = this._GetAttribute(element, "selectionBeginIndex", 0);

                                        let field = new C1TemplateField(name, c1Selection);
                                        template.addField(field);

                                    }
                                });
                            }
                        }

                        if (placeHolder.hasOwnProperty("repeatedBlock")) {
                            let repeatedBlocks = placeHolder["repeatedBlock"];
                            if (repeatedBlocks) {
                                repeatedBlocks.forEach(repeatedBlock => {
                                    let repeatBlockName = this._GetAttribute(repeatedBlock, "name", "RepeatBlockName");
                                    let block = new C1TemplateBlock(repeatBlockName);

                                    if (repeatedBlock && repeatedBlock.hasOwnProperty("repeatedPlaceHolder")) {
                                        let repeatedPlaceHolders = repeatedBlock["repeatedPlaceHolder"];
                                        if (repeatedPlaceHolders) {
                                            repeatedPlaceHolders.forEach(repeatedPlaceHolder => {
                                                let name = this._GetAttribute(repeatedPlaceHolder, "name", "PlaceHolderName");
                                                let dataStr = this._GetAttribute(repeatedPlaceHolder, "data", null);
                                                if (dataStr) {
                                                    var fieldData = JSON.parse(dataStr);

                                                    // Recreate range
                                                    let range = doc.createRange();
                                                    range.setStart(C1Utils.GetElementFromXPath(fieldData.xPathStart, true, doc), fieldData.startOffset);
                                                    range.setEnd(C1Utils.GetElementFromXPath(fieldData.xPathEnd, true, doc), fieldData.endOffset);
                                                    var selection = window.getSelection();
                                                    selection.removeAllRanges();
                                                    selection.addRange(range);

                                                    // ReCreate selection and data
                                                    let c1Selection = new C1TextSelection(doc);
                                                    c1Selection.onSelection(selection);
                                                    c1Selection._RangeData = fieldData;
                                                    c1Selection._XPath = this._GetAttribute(repeatedPlaceHolder, "xPath", "");
                                                    c1Selection._Text = fieldData.text;
                                                    c1Selection._AnchorOffset = this._GetAttribute(repeatedPlaceHolder, "selectionBeginIndex", 0);

                                                    let field = new C1TemplateField(name, c1Selection);


                                                    let repeatedDataStr = this._GetAttribute(repeatedPlaceHolder, "nextBlockData", null);
                                                    if (repeatedDataStr) {
                                                        var repeatedFieldData = JSON.parse(repeatedDataStr);

                                                        // Recreate range
                                                        let range = doc.createRange();
                                                        range.setStart(C1Utils.GetElementFromXPath(repeatedFieldData.xPathStart, true, doc), repeatedFieldData.startOffset);
                                                        range.setEnd(C1Utils.GetElementFromXPath(repeatedFieldData.xPathEnd, true, doc), repeatedFieldData.endOffset);
                                                        var selection = window.getSelection();
                                                        selection.removeAllRanges();
                                                        selection.addRange(range);

                                                        // ReCreate selection and data
                                                        let c1RepeatedFieldSelection = new C1TextSelection(doc);
                                                        c1RepeatedFieldSelection.onSelection(selection);
                                                        c1RepeatedFieldSelection._RangeData = repeatedFieldData;
                                                        c1RepeatedFieldSelection._XPath = this._GetAttribute(repeatedPlaceHolder, "xPathInNextBlock", "");
                                                        c1RepeatedFieldSelection._Text = repeatedFieldData.text;
                                                        c1RepeatedFieldSelection._AnchorOffset = this._GetAttribute(repeatedPlaceHolder, "nextBlockAnchor", 0);

                                                        let repeatedField = new C1TemplateField(name, c1RepeatedFieldSelection);
                                                        field._RepeatedFields.push(repeatedField);
                                                    }

                                                    block.addField(field);
                                                }



                                            });
                                        }
                                    }

                                    template.addBlock(block);
                                });
                            }
                        }
                    }
                });
            }
        }
    }

    build(template: IC1Template): Promise<any> {
        return new Promise((resolve, reject) => {

            if (!template) return reject();

            if (!template._XMLData) {
                template._XMLData = template.writeXML();
            }

            parseString(template._XMLData, { rootName: "template" }, (error, result) => {
                if (error) {
                    return reject(error);
                }
                if (result) {
                    this._Data = JSON.parse(JSON.stringify(result));
                    console.log(this._Data);
                    return resolve();
                }
                return reject();
            });
        });
    }
    _GetAttribute(node, attributeName, defaultValue?) {
        if (node && node.hasOwnProperty("$")) {
            let attributes = node["$"];
            if (attributes && attributes.hasOwnProperty(attributeName)) {
                return attributes[attributeName];
            }
        }
        return defaultValue;
    }
    getTemplateName() {
        if (this._Data) {
            if (this._Data.hasOwnProperty("template")) {
                let template = this._Data["template"];
                return this._GetAttribute(template, "name", "");
            }
        }
        return "";
    }

    getEmailName(): string {
        if (this._Data) {
            if (this._Data.hasOwnProperty("template")) {
                let template = this._Data["template"];
                if (template) {
                    if (template.hasOwnProperty("source_name")) {
                        let sources = template["source_name"];
                        if (sources && sources.length > 0) {
                            // return source from xml files
                            return sources[0];
                        }
                    }
                }
            }
        }
        return "";
    }
    
    getTemplateVersion(): string {
        if (this._Data) {
            if (this._Data.hasOwnProperty("template")) {
                let template = this._Data["template"];
                if (template) {
                    if (template.hasOwnProperty("version")) {
                        let sources = template["version"];
                        if (sources && sources.length > 0) {
                            // return source from xml files
                            return sources[0];
                        }
                    }
                }
            }
        }
        return "";
    }
    getHTMLContentString(): string {
        if (this._Data) {
            if (this._Data.hasOwnProperty("template")) {
                let template = this._Data["template"];
                if (template) {
                    if (template.hasOwnProperty("source")) {
                        let sources = template["source"];
                        if (sources && sources.length > 0) {
                            // return source from xml files
                            return sources[0];
                        }
                    }
                }
            }
        }
        return "";
    }
}