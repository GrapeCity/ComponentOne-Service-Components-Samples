import { IC1TemplateField } from '../interface/IC1TemplateField';
import { ITextSelection } from '../interface/ITextSelection';

export class C1TemplateField implements IC1TemplateField {
    _Name: string = '';
    _XPath: string = '';
    _Selection: ITextSelection = null;
    _CloneContent: DocumentFragment;
    _OriginRange;
    _OriginNode;
    _RepeatedFields: Array<IC1TemplateField> = [];
    constructor(name: string, selection: ITextSelection) {
        this._Name = name;
        this._Selection = selection;
        this.create();
    }
    getName(): string {
        return this._Name;
    }
    setName(name: string): void {
        this._Name = name;
        if (this._OriginRange) {
            var html = `<span  class="c1shadowdom" style="background:#3880ff; color:white; padding : 2px 4px; border-radius:4px">${this._Name}</span>`;
            this._OriginRange.deleteContents();

            var el = document.createElement("div");
            el.innerHTML = html;
            var frag = document.createDocumentFragment(), node, lastNode;
            while ((node = el.firstChild)) {
                lastNode = frag.appendChild(node);
            }
            this._OriginRange.insertNode(frag);
        }
        for (let f of this._RepeatedFields) {
            f.setName(this.getName());
        }
    }
    getXPath(): string {
        return this._XPath;
    }
    setXPath(xpath: string): void {
        this._XPath = xpath;
    }

    getSelection(): ITextSelection {
        return this._Selection;
    }
    setSelection(selection: ITextSelection) {
        this._Selection = selection;
    }

    create() {
        if (!this._Selection) return;
        let range = this._Selection.getRange();
        this._OriginRange = range;
        if (range) {
            var html = `<span  class="c1shadowdom" c1shadowdata="${range.cloneContents().textContent}" style="background:#3880ff; color:white; padding : 2px 4px; border-radius:4px">${this._Name}</span>`;
            this._CloneContent = range.cloneContents();
            range.deleteContents();
            var el = document.createElement("div");
            el.innerHTML = html;
            var frag = document.createDocumentFragment(), node, lastNode;
            while ((node = el.firstChild)) {
                lastNode = frag.appendChild(node);
            }
            range.insertNode(frag);
            range.commonAncestorContainer['c1shadowdata'] = range.commonAncestorContainer.textContent;
        }
    }
    
    dispose() {
        if (this._OriginRange) {
            this._OriginRange.deleteContents();
            this._OriginRange.insertNode(this._CloneContent);
        }
        for (let field of this._RepeatedFields) {
            field.dispose();
        }
    }

    getData(): string {
        var data = {};
        if (this._Selection) {
            data = this._Selection.getRangeData();
        }
        return JSON.stringify(data);
    }
}