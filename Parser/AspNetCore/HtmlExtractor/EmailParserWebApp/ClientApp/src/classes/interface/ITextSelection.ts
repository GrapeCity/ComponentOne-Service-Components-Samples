export interface ITextSelection {
  _Text: string;
  _XPath: string;
  _XPathFocus: string;
  _AnchorNode: Node;
  _AnchorOffset: number;
  _FocusNode: Node;
  _FocusOffset: number;
  _Range: Range;
  _Target;
  clear(): void;
  onSelection(selection: Selection): void;
  getSelection(): Selection;
  getRange(): Range;
  isContainsTemplateFields();
  reproceduceEvents(document: Document, event);
  getOriginDocument(): Document;
  getRangeData(): object;
}
