import { ITextSelection } from '../interface/ITextSelection';
import { C1Utils } from './C1Utils';

export class C1TextSelection implements ITextSelection {
    _Text: string = '';
    _XPath: string = '';
    _XPathFocus: string = '';
    _AnchorNode: Node;
    _AnchorOffset: number;
    _FocusNode: Node;
    _FocusOffset: number;
    _Selection: Selection;
    _Range: Range;
    _OriginDocument: Document;
    _Target;

    _RangeData = {
        startOffset: 0,
        endOffset: 0,
        xPathStart: "",
        xPathEnd: "",
        textContent: "",
        text: ""
    };

    constructor(document?: Document, event?) {
        this._OriginDocument = document;
        if (event) this._Target = event.target;
        this.clear();
    }
    onSelection(selection: Selection) {
        if (!selection) return;
        this._Selection = selection;
        this._Text = selection.toString();
        this._AnchorNode = selection.anchorNode;
        this._AnchorOffset = selection.anchorOffset;
        this._FocusNode = selection.focusNode;
        this._FocusOffset = selection.focusOffset;
        this._XPath = C1Utils.GetPathTo(this._AnchorNode.parentElement);
        this._XPathFocus = C1Utils.GetPathTo(this._FocusNode.parentElement);
        if (selection.getRangeAt && selection.rangeCount) {
            this._Range = selection.getRangeAt(0);
        }
    }

    isContainsTemplateFields(): boolean {
        let range = this.getRange().cloneContents();
        let div = document.createElement('div');
        div.appendChild(range.cloneNode(true));
        return div.getElementsByClassName('c1shadowdom').length > 0;
    }



    clear() {
        this._Text = '';
        this._XPath = '';
        this._AnchorNode = null;
        this._AnchorOffset = 0;
        this._FocusNode = null;
        this._FocusOffset = 0;
        this._Selection = null;
    }

    getSelection(): Selection {
        return this._Selection;
    }

    getRange(): Range {
        return this._Range;
    }

    _DeepReproceduce(document: Document, event) {
        let range = this.getRange();
        let beginNode = this._FindLowestContainer(document, this._GetTextContentFromElement(range.startContainer.parentElement));
        if (beginNode) {
            let anchorNodeIndex: number = 0;
            let anchorNode = null;
            for (let i = 0; i < beginNode.childNodes.length; i++) {
                let childNode = beginNode.childNodes[i];
                if (childNode.nodeName == '#text') {
                    let str: string = childNode.data;
                    if (str.length > 1) {
                        anchorNodeIndex++;
                    }
                    let childTextContent: string = childNode.textContent;
                    let index: number = childTextContent.indexOf(this._AnchorNode.textContent);
                    if (index != -1) {
                        anchorNode = childNode;
                        this._AnchorOffset = index + range.startOffset;
                        break;
                    }
                }
            }
            this._XPath = C1Utils.GetPathTo(beginNode.childNodes.length != 0 ? beginNode : beginNode.parentElement, document) + (anchorNode ? `/text()[${anchorNodeIndex}]` : '');
        }

        let endNode = this._FindLowestContainer(document, this._GetTextContentFromElement(range.endContainer.parentElement));
        if (endNode) {
            let focusNodeIndex: number = 0;
            let focusNode = null;
            for (let i = 0; i < endNode.childNodes.length; i++) {
                let childNode = endNode.childNodes[i];
                if (childNode.nodeName == '#text') {
                    var str: string = childNode.data;
                    if (str.length > 1) {
                        focusNodeIndex++;
                    }
                    let childTextContent: string = childNode.textContent;
                    let index: number = childTextContent.indexOf(this._FocusNode.textContent);
                    if (index != -1) {
                        focusNode = childNode;
                        this._FocusOffset = index + range.endOffset;
                        break;
                    }
                }
            }
            this._XPathFocus = C1Utils.GetPathTo(endNode.childNodes.length != 0 ? endNode : endNode.parentElement, document) + (focusNode ? `/text()[${focusNodeIndex}]` : '');
        }

    }

    reproceduceEvents(document: Document, event) {
        if (!document) return;
        if (!this._Text) return;
        let range = this.getRange();

        // find common acesster from document

        let startTextContent: string = this._GetTextContentFromElement(range.startContainer);
        let endTextContent: string = this._GetTextContentFromElement(range.endContainer);
        endTextContent = endTextContent.substr(0, range.endOffset);

        let commonAcestorContainer = this._FindCommonAncestor(document, startTextContent, endTextContent);
        let ancestorIndex: number = 0;
        let ancestorNode = null;

        if (commonAcestorContainer.childNodes.length > 0) {
            for (let i = 0; i < commonAcestorContainer.childNodes.length; i++) {
                let childNode = commonAcestorContainer.childNodes[i];
                if (childNode.nodeName == '#text') {
                    let str: string = childNode.data;
                    if (str.length > 1) {
                        ancestorIndex++;
                    }
                    let childTextContent: string = childNode.textContent;
                    if (childTextContent.indexOf(startTextContent) != -1 && childTextContent.indexOf(endTextContent) != -1) {
                        ancestorNode = childNode;
                        break;
                    }
                }
            }
        }

        // Find XPath
        this._XPath = C1Utils.GetPathTo(commonAcestorContainer.childNodes.length != 0 ? commonAcestorContainer : commonAcestorContainer.parentElement, document) + (ancestorNode ? `/text()[${ancestorIndex}]` : '');

        // Find real Offset
        let realCommonAncestor = ancestorNode ? ancestorNode : commonAcestorContainer;
        let textContentOfAncestor: string = this._GetTextContentFromElement(realCommonAncestor);

        this._AnchorOffset = textContentOfAncestor.indexOf(startTextContent) + range.startOffset;


        // Store selection data;

        this._RangeData.startOffset = range.startOffset;
        this._RangeData.endOffset = range.endOffset;
        this._RangeData.xPathStart = this._FindActualyXPathOfNode(range.startContainer, this._AnchorNode.textContent, this._OriginDocument);
        this._RangeData.xPathEnd = this._FindActualyXPathOfNode(range.endContainer, this._FocusNode.textContent, this._OriginDocument);
        this._RangeData.text = this._Text;
        this._RangeData.textContent = range.commonAncestorContainer.textContent;
    }

    _GetTextContentFromElement(ele) {
        if (ele && typeof ele.hasAttribute === "function" && ele.hasAttribute('c1shadowdata')) {
            return ele.getAttribute('c1shadowdata');
        }
        if (ele) return ele.textContent;
        return '';
    }
    _FindLowestContainer(node, text: string) {
        if (!node || !text) return;
        var eles = node.children;
        for (let i = 0; i < eles.length; i++) {
            var ele = eles.item(i);
            var textContent: string = this._GetTextContentFromElement(ele);
            if (textContent.indexOf(text) != -1) {
                return this._FindLowestContainer(ele, text);
            }
        }
        return node;
    }
    _FindCommonAncestor(node, start: string, end: string) {
        if (!node || !start || !end) return;
        var eles = node.children;
        for (let i = 0; i < eles.length; i++) {
            var ele = eles.item(i);
            var textContent: string = this._GetTextContentFromElement(ele);
            if (textContent.indexOf(start) != -1 && textContent.indexOf(end) != -1) {
                return this._FindCommonAncestor(ele, start, end);
            }
        }
        return node;
    }

    getOriginDocument(): Document {
        return this._OriginDocument;
    }


    _FindActualyXPathOfNode(node: Node, nodeTextContent, doc?: Document) {
        doc = doc ? doc : document;
        let lowestContainer = this._FindLowestContainer(document, this._GetTextContentFromElement(node.parentElement));
        if (lowestContainer) {
            let foundNodeIndex: number = 0;
            let foundNode = null;
            for (let i = 0; i < lowestContainer.childNodes.length; i++) {
                let childNode = lowestContainer.childNodes[i];
                if (childNode.nodeName == '#text') {
                    let str: string = childNode.data;
                    if (str.length > 1) {
                        foundNodeIndex++;
                    }
                    let childTextContent: string = childNode.textContent;
                    let index: number = childTextContent.indexOf(nodeTextContent);
                    if (index != -1) {
                        foundNode = childNode;
                        break;
                    }
                }
            }
            return C1Utils.GetPathTo(lowestContainer.childNodes.length != 0 ? lowestContainer : lowestContainer.parentElement, document) + (foundNode ? `/text()[${foundNodeIndex}]` : '');
        }
        return "";
    }
    getRangeData(): object {
        return this._RangeData;
    }

}