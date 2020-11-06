import { IC1Template } from '../interface/IC1Template';
import { saveAs } from 'file-saver';

export class C1Utils {

    public static GetPathTo(element, doc?: Document) {
        if (element.id !== '') {
            if (element.id == 'contents') {
                return '/html/body';
            }
            return '//*[@id="' + element.id + '"]';
        }
        if (element === (doc ? doc : document).body) {
            return '/html/body';
        }

        let ix = 0;
        let siblings = element.parentNode.childNodes;

        let sameTagName = 0;

        for (let i = 0; i < siblings.length; i++) {

            let sibling = siblings[i];

            if (sibling.nodeType === 1 && sibling.tagName === element.tagName) {
                sameTagName++;
            }
        }

        for (let i = 0; i < siblings.length; i++) {

            let sibling = siblings[i];

            if (sibling.nodeType === 1 && sibling.tagName === element.tagName) {
                ix++;
            }

            if (sibling === element) {
                return this.GetPathTo(element.parentNode, doc) + '/' + (element.tagName as string).toLocaleLowerCase() + (sameTagName > 1 ? `[${ix}]` : '');
            }

        }

    }


    public static GetElementFromXPath(xpath: string, revert: boolean, doc?: Document): Node {
        if (revert) {
            if (xpath.startsWith("/html/body")) {
                xpath = xpath.replace("/html/body", '//*[@id="contents"]');
            }
        }
        doc = doc ? doc : document;
        return doc.evaluate(xpath, doc, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;
    }

    public static beautyJSON(json) {
        json = json.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
        return json.replace(/("(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\"])*"(\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)/g, function (match) {
            var cls = 'number';
            if (/^"/.test(match)) {
                if (/:$/.test(match)) {
                    cls = 'key';
                } else {
                    cls = 'string';
                }
            } else if (/true|false/.test(match)) {
                cls = 'boolean';
            } else if (/null/.test(match)) {
                cls = 'null';
            }
            return '<span class="' + cls + '">' + match + '</span>';
        });
    }

    public static getWindowSelection(): Selection {
        if (window.getSelection) return window.getSelection();
        if (document['selection']) return document['selection'];
        return null;
    }

    public static saveTemplateToFile(template: IC1Template, filename?: string) {
        if (!template) return;
        let xw = template.writeXML();
        const blob = new Blob([xw.toString()], { type: 'text/plain' });
        saveAs(blob, filename ? filename : (template._Name + '.xml'));
    }
}