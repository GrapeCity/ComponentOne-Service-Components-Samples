/// <reference path="typings/wijmo/wijmo.d.ts" />
/// <reference path="typings/wijmo/wijmo.grid.d.ts" />
/// <reference path="typings/wijmo/wijmo.input.d.ts" />

module wijmo.razor.designer {
    'use strict';

    export enum GroupOperation {
        Sum,
        Avg,
        First,
        Last,
        Count,
        Max,
        Min,
        VarP,
        Var,
        StdP,
        Std
    }

    export enum BinaryOperation {
        Add,
        Sub,
        Mul,
        Div,
        Mod
    }

    export enum RangeCondition {
        Eq,
        Gt,
        Lt,
        Gte,
        Lte,
        Ne,
        Cnt,
        Nc,
        Bw,
        Ew
    }

    export class AvailableApps extends wijmo.input.ListBox {

        constructor(element: any, controller: DesignerController) {
            super(element);
            var self = this;

            this.loadedItems.addHandler(function (e) {
                var items = this.hostElement.querySelectorAll(".wj-listbox-item");
                for (var i = 0; i < items.length; i++) {
                    items[i].draggable = true;
                }
            }, this);

            this.hostElement.addEventListener('mousedown', function (e: MouseEvent) {
                var children = self.hostElement.children;
                for (var index = 0; index < children.length; index++) {
                    if (contains(children[index], e.target)) {
                        self.selectedIndex = index;
                        return;
                    }
                }
            });

            this.hostElement.addEventListener('dragstart', function (e) {
                if (self.selectedItem) {
                    controller.startDrag(self.selectedItem, e);
                    e.dataTransfer.effectAllowed = 'copy';
                    e.dataTransfer.setData('text', ''); // required in FireFox (note: text/html will throw in IE!)
                }
            });

            this.itemsSource = controller.baseTables;
        }
    }

    export class QueryDefinitions extends wijmo.input.ListBox {

        constructor(element: any, controller: DesignerController) {
            super(element, {
                displayMemberPath: "value",
                selectedValuePath: "key"
            });
            var self = this;

            this.selectedIndexChanged.addHandler(function (sender, e) {
                controller.selectQuery(self.selectedValue);
                controller.selectTable(null);
            });
        }
    }

    export class QueryProperties {

        private _controller: DesignerController;

        constructor(controller: DesignerController) {
            this._controller = controller;
        }

        get name(): string {
            return this._controller.getSelectedQuery().name;
        }
        set name(value: string) {
            this._controller.getSelectedQuery().name = value;
        }

        get maxRows(): number {
            return this._controller.getSelectedQuery().maxRows;
        }
        set maxRows(value: number) {
            this._controller.getSelectedQuery().maxRows = value;
        }
    }

    export class JoinProperties {

        private _json: any;

        constructor(json: any) {
            this._json = json;
        }

        get fromField(): string {
            return this._json.fromField;
        }
        set fromField(value: string) {
            this._json.fromField = value;
        }

        get toField(): string {
            return this._json.toField;
        }
        set toField(value: string) {
            this._json.toField = value;
        }

        getFromFields(controller: DesignerController) {
            var result = {};
            controller.getTableFields(this._json.fromId).map(function (m) {
                result[m.name] = m.name;
            });
            return result;
        }

        getToFields(controller: DesignerController) {
            var result = {};
            controller.getTableFields(this._json.toId).map(function (m) {
                result[m.name] = m.name;
            });
            return result;
        }
    }

    export class DataSurface extends wijmo.Control {

        private _controller: DesignerController;

        constructor(element: any, controller: DesignerController) {
            super(element);
            wijmo.addClass(this.hostElement, "wj-control wj-content");
            var self = this;
            this._controller = controller;

            this.hostElement.addEventListener('click', function (e) {
                 controller.selectQueryProperties();
            }, false);

            this.hostElement.addEventListener('dragover', function (e) {
                var drag = controller.getDragObject();
                if (drag instanceof HTMLElement) {
                    e.dataTransfer.dropEffect = 'move';
                    e.preventDefault();
                } else if (drag instanceof wijmo.grid.HitTestInfo) {
                    // ignore
                } else if (drag && !controller.hasTable(drag)) {
                    e.dataTransfer.dropEffect = 'copy';
                    e.preventDefault();
                }
            });

            this.hostElement.addEventListener('drop', function (e) {
                var drag = controller.getDragObject();
                var origin = controller.getDragOrigin();
                var x = e.offsetX - origin.x;
                var y = e.offsetY - origin.y;
                if (drag instanceof HTMLElement) {
                    let table = <HTMLElement>drag;
                    let id = table.getAttribute("dataId");
                    table.style.left = x.toString() + "px";
                    table.style.top = y.toString() + "px";
                    controller.moveTable(id, x, y);
                    controller.dataSurface.drawLines(true);
                } else if (drag) {
                    var options = {
                        id: new Date().getTime(),
                        name: drag,
                        x: x,
                        y: y
                    };
                    controller.createTable(options);
                    var table = new wijmo.razor.designer.DataTable(element, controller, options);
                    controller.bringToFront(table.hostElement);
                    controller.selectTable(options.id);
                }
                controller.endDrag();
            });
        }

        clearLines() {
            var lines = this.hostElement.querySelectorAll(".line");
            for (var n = 0; n < lines.length; n++) {
                var parent = lines[0].parentElement;
                parent.removeChild(lines[0]);
            }
        }

        drawLines(clear?: boolean) {
            var self = this;
            var q = this._controller.getSelectedQuery();
            if (clear) {
                this.clearLines();
            }
            if (q && q.joins) {
                Object.keys(q.joins).map(function (key) {
                    var join = q.joins[key];
                    var from = <HTMLElement>self.hostElement.querySelector("[dataId='" + join.fromId.toString() + "']");
                    var to = <HTMLElement>self.hostElement.querySelector("[dataId='" + join.toId.toString() + "']");
                    var x1 = from.offsetLeft + from.offsetWidth;
                    var y1 = from.offsetTop + from.offsetHeight / 2;
                    var x2 = to.offsetLeft;
                    var y2 = to.offsetTop + to.offsetHeight / 2;
                    var line = self.drawLine(x1, y1, x2, y2);
                    line.setAttribute("joinId", key);
                    line.setAttribute("wj-context-menu", "#table-join-menu");
                    line.addEventListener("click", function (e) {
                        self._controller.selectJoinProperties(join.id);
                        e.stopPropagation();
                    }, false);
                    line.addEventListener("contextmenu", function (e) {
                        var menu = <wijmo.input.Menu>wijmo.Control.getControl("#table-join-menu");
                        var drop = menu.dropDown;
                        if (menu && drop && !wijmo.closest(e.target, '[disabled]')) {
                            e.preventDefault();
                            menu.owner = line;
                            menu.selectedIndex = -1;
                            if (menu.onIsDroppedDownChanging(new wijmo.CancelEventArgs())) {
                                wijmo.showPopup(drop, e);
                                menu.onIsDroppedDownChanged();
                                drop.focus();
                            }
                        }
                    });
                })
            }
        }
        
        drawLine(x1, y1, x2, y2): HTMLElement {
            var length = Math.sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
            var angle = Math.atan2(y2 - y1, x2 - x1) * 180 / Math.PI;
            var transform = 'rotate(' + angle + 'deg)';
            var canvas = wijmo.getElement("#selected-apps");
            var line = wijmo.createElement("<div class='line'>", canvas);
            line.style.position = "absolute";
            line.style.transform = transform;
            line.style.width = length.toString() + "px";
            line.style.left = x1.toString() + "px";
            line.style.top = y1.toString() + "px";
            return line;
        }        
    }

    export class DataTable extends wijmo.grid.FlexGrid {

        constructor(element: any, controller: DesignerController, options: any) {
            var div = document.createElement("div");
            div.style.height = "196px";
            div.style.width = "190px";
            div.style.position = "absolute";
            div.style.left = options.x.toString() + "px";
            div.style.top = options.y.toString() + "px";
            div.setAttribute("wj-context-menu", "#data-table-menu");
            
            super(div, {
                isReadOnly: true,
                autoGenerateColumns: false,
                allowSorting: false,
                headersVisibility: wijmo.grid.HeadersVisibility.Column,
                selectionMode: wijmo.grid.SelectionMode.None,
                columns: [{
                    binding: "name",
                    header: options.name,
                    width: "*",
                    allowResizing: false
                }]
            });

            var self = this;
            wijmo.getElement(element).appendChild(div);
            this.hostElement.setAttribute("dataId", options.id);

            this.itemFormatter = function (panel, r, c, cell) {
                if (panel.cellType == wijmo.grid.CellType.Cell) {
                    var text = cell.innerHTML;
                    cell.innerHTML = '<div><a href="#" draggable="true">' + text + '</a></div>';
                    //var checked = controller.isSelectedField(options.id, r) ? " checked" : "";
                    //cell.innerHTML = '<div><input type="checkbox"' + checked + '>&nbsp;<a href="#" draggable="true">' + text + '</a></div>';
                    //cell.firstChild.firstChild.addEventListener('click', function (e) {
                    //    controller.selectField(options.id, r, e.target.checked);
                    //    e.stopPropagation();
                    //});
                } else if (panel.cellType == wijmo.grid.CellType.ColumnHeader) {
                    cell.draggable = true;
                    wijmo.toggleClass(cell, "wj-state-selected", controller.isSelectedTable(options.id));
                }
            };

            this.hostElement.addEventListener('mousedown', function (e) {
                if (self.hitTest(e).cellType == wijmo.grid.CellType.ColumnHeader) {
                    controller.bringToFront(self.hostElement);
                    controller.selectTable(options.id);
                }
            });

            this.hostElement.addEventListener('contextmenu', function (e) {
                var hit = self.hitTest(e);
                var menu = <wijmo.input.Menu>wijmo.Control.getControl("#data-table-menu");
                var drop = menu.dropDown;
                if (menu && drop && !wijmo.closest(e.target, '[disabled]')) {
                    e.preventDefault();
                    menu.owner = self.hostElement;
                    menu.selectedIndex = -1;
                    if (menu.onIsDroppedDownChanging(new wijmo.CancelEventArgs())) {
                        wijmo.showPopup(drop, e);
                        menu.onIsDroppedDownChanged();
                        drop.focus();
                    }
                }
            });

            this.hostElement.addEventListener('dragstart', function (e) {
                var ht = self.hitTest(e);
                if (ht.cellType == wijmo.grid.CellType.ColumnHeader) {
                    controller.startDrag(self.hostElement, e);
                    e.dataTransfer.effectAllowed = 'move';
                    e.dataTransfer.setData('text', ''); // required in FireFox (note: text/html will throw in IE!)
                } else if (ht.cellType == wijmo.grid.CellType.Cell) {
                    controller.startDrag(ht, e);
                    e.dataTransfer.effectAllowed = 'copy';
                    e.dataTransfer.setData('text', ''); // required in FireFox (note: text/html will throw in IE!)
                }
            });

            this.hostElement.addEventListener('dragover', function (e) {
                var stop = true;
                var drag = controller.getDragObject();
                if (drag && drag instanceof wijmo.grid.HitTestInfo) {
                    var ht = self.hitTest(e);
                    if (ht.panel && drag.panel && ht.panel.grid != drag.panel.grid) {
                        if (ht.row >= 0 && ht.cellType == wijmo.grid.CellType.Cell) {
                            stop = false;
                            e.dataTransfer.dropEffect = 'copy';
                            e.preventDefault();
                        }
                    }
                } else if (drag && drag instanceof HTMLElement) {
                    e.dataTransfer.dropEffect = 'move';
                    e.preventDefault();
                    stop = false;
                }
                if (stop) {
                    e.dataTransfer.dropEffect = 'none';
                    e.stopPropagation();
                }
            });

            this.hostElement.addEventListener('drop', function (e) {
                var drag = controller.getDragObject();
                if (drag && drag instanceof wijmo.grid.HitTestInfo) {
                    var ht = self.hitTest(e);
                    e.stopPropagation();
                    var fromId = Number(drag.panel.grid.hostElement.getAttribute("dataId"));
                    var fromField = drag.panel.grid.rows[drag.row].dataItem.name;
                    var toId = Number(self.hostElement.getAttribute("dataId"));
                    var toField = self.rows[ht.row].dataItem.name;
                    var options = {
                        id: new Date().getTime(),
                        fromId: fromId,
                        fromField: fromField,
                        toId: toId,
                        toField: toField
                        // type: JoinType.OneToOne
                    };
                    controller.createJoin(options);
                    controller.dataSurface.drawLines(true);
                } else if (drag && drag instanceof HTMLElement) {
                    var origin = controller.getDragOrigin();
                    var id = Number(self.hostElement.getAttribute("dataId"));
                    var x = self.hostElement.offsetLeft + e.offsetX - origin.x;
                    var y = self.hostElement.offsetTop + e.offsetY - origin.y;
                    self.hostElement.style.left = x.toString() + "px";
                    self.hostElement.style.top = y.toString() + "px";
                    controller.moveTable(id, x, y);
                    controller.dataSurface.drawLines(true);
                    e.stopPropagation();
                }
            });

            if (options.fields) {
                this.itemsSource = options.fields;
            } else {
                this.itemsSource = controller.getBaseTableFields(options.name);
            }
        }

        rename(name: string) {
            this.columns[0].header = name;
        }
    }

    export class QueryDesignGrid extends wijmo.grid.FlexGrid {

        private _query: any;
        private _controller: DesignerController;
        private _groupOps = [];
        private _binaryOps = [];
        private _rangeOps = [];
        private _sorts = [];
        
        static _MAX_ROWS = 6;
        static _MAX_COLS = 8;
        static _ROW_FIELD = 0;
        static _ROW_ALIAS = 1;
        static _ROW_SORT = 2;
        static _ROW_OPERATION = 3;
        static _ROW_CONDITION = 4;
        static _ROW_VALUE = 5;

        constructor(element: any, controller: DesignerController) {
            
            super(element, {
                allowMerging: wijmo.grid.AllowMerging.All,
                headersVisibility: wijmo.grid.HeadersVisibility.Row,
                selectionMode: wijmo.grid.SelectionMode.Cell,
                isDisabled: true
            });

            var self = this;
            this.mergeManager = new QueryDesignGridMergeManager();

            for (var r = 0; r < QueryDesignGrid._MAX_ROWS; r++) {
                this.rows.push(new wijmo.grid.Row({
                    allowMerging: true
                }));
            }
            for (var c = 0; c < QueryDesignGrid._MAX_COLS; c++) {
                this.columns.push(new wijmo.grid.Column({
                    width: "*"
                }));
            }

            Object.keys(GroupOperation).forEach(function (value: string, index: number) {
                var n = Number(value);
                if (!isNaN(n)) {
                    self._groupOps.push({ code: n, label: GroupOperation[n]});
                }
            });

            Object.keys(BinaryOperation).forEach(function (value: string, index: number) {
                var n = Number(value);
                if (!isNaN(n)) {
                    self._binaryOps.push({ code: n, label: BinaryOperation[n]});
                }
            });

            Object.keys(RangeCondition).forEach(function (value: string, index: number) {
                var n = Number(value);
                if (!isNaN(n)) {
                    self._rangeOps.push({ code: n, label: RangeCondition[n]});
                }
            });

            this._sorts.push({code: 1, label: "Ascending"});
            this._sorts.push({code: -1, label: "Descending"});
            this._sorts.push({code: 0, label: "(not sorted)"});

            this.rowHeaders.setCellData(QueryDesignGrid._ROW_FIELD, 0, "Field");
            this.rowHeaders.setCellData(QueryDesignGrid._ROW_ALIAS, 0, "Alias");
            this.rowHeaders.setCellData(QueryDesignGrid._ROW_SORT, 0, "Sort");
            this.rowHeaders.setCellData(QueryDesignGrid._ROW_OPERATION, 0, "Op");
            this.rowHeaders.setCellData(QueryDesignGrid._ROW_CONDITION, 0, "Range");
            this.rowHeaders.setCellData(QueryDesignGrid._ROW_VALUE, 0, "Range");

            this.rowHeaders.columns.defaultSize = 64;
            this.rowHeaders.columns[0].align = "right";
            this.rowHeaders.columns[0].allowMerging = true;

            this._controller = controller;
            this.formatItem.addHandler(this._formatItem, this);
            this.cellEditEnded.addHandler(this._cellEditEnded, this);
        }

        apply(query: any) {
            this._query = query;
            for (var r = 0; r < QueryDesignGrid._MAX_ROWS; r++) {
                for (var c = 0; c < QueryDesignGrid._MAX_COLS; c++) {
                    this.setCellData(r, c, null, false, false);
                }
            }
            if (query) {
                if (query.fields.length > 0) {
                   for (var c = 0; c < QueryDesignGrid._MAX_COLS; c++) {
                        for (var r = 0; r < QueryDesignGrid._MAX_ROWS; r++) {
                            this.setCellData(r, c, query.fields[c][r], false, false);
                        }
                    }
                }
            }
            this.selection = new wijmo.grid.CellRange(0, 0);
            this.isDisabled = !query;
            this.invalidate();
        }

        private _updateQuery() {
            if (this._query) {
                this._query.fields = this._getCriteria();
                this._controller.setQueryDirty();
            }
        }

        private _getCriteria() {
            var items = [];
            for (var c = 0; c < QueryDesignGrid._MAX_COLS; c++) {
                var item = [];
                for (var r = 0; r < QueryDesignGrid._MAX_ROWS; r++) {
                    item.push(this.getCellData(r, c, false));
                }
                items.push(item);
            }
            return items;
        }

        private _formatItem(sender, e: wijmo.grid.FormatItemEventArgs) {
            if (e.panel == this.cells) {
                var data = this.getCellData(e.row, e.col, false);
                var range = e.panel.grid.editRange;
                var editing = range && range.row === e.row && range.col === e.col;
                if (e.row == QueryDesignGrid._ROW_FIELD) {
                    if (editing) {
                        this._editDropDown(e);
                    } else if (e.cell.firstChild) {
                        var n = data.indexOf(".");
                        e.cell.firstChild.textContent = (n >= 0) ? data.slice(n + 1) : data;
                    }
                } else if (e.row == QueryDesignGrid._ROW_SORT) {
                    if (editing) {
                        this._editDropDown(e);
                    } else if (e.cell.firstChild) {
                        var text = (data == 1) ? "Ascending" : (data == -1) ? "Descending" : "";
                        e.cell.firstChild.textContent = text;
                    }
                } else if (e.row == QueryDesignGrid._ROW_OPERATION) {
                    if (editing) {
                        this._editDropDown(e);
                    } else if (e.cell.firstChild) {
                        e.cell.firstChild.textContent = e.range.isSingleCell ? this._groupOps[data].label : this._binaryOps[data].label;
                    }
                } else if (e.row == QueryDesignGrid._ROW_CONDITION) {
                    if (editing) {
                        this._editDropDown(e);
                    } else if (e.cell.firstChild) {
                        e.cell.firstChild.textContent = this._rangeOps[data].label;
                    }
                }
            }
        }

        private _cellEditEnded(sender, e: wijmo.grid.CellRangeEventArgs) {
            if (!e.cancel) {
                this._updateQuery();
            }
        }

        private _editDropDown(args: wijmo.grid.FormatItemEventArgs) {
            var self = this;
            var data = this.getCellData(args.row, args.col, false);
            var items = [];
            
            if (args.row == QueryDesignGrid._ROW_FIELD) {
                Object.keys(self._query.tables).forEach(function (key) {
                    var t = self._query.tables[key];
                    t.fields.forEach(function (f) {
                        items.push({code: t.name + "." + f.name, label: f.name});
                    });
                });
            } else if (args.row == QueryDesignGrid._ROW_SORT) {
                items = this._sorts;
            } else if (args.row == QueryDesignGrid._ROW_OPERATION) {
                items = args.range.isSingleCell ? this._groupOps : this._binaryOps;
            } else if (args.row == QueryDesignGrid._ROW_CONDITION) {
                items = this._rangeOps;
            }

            // hide first child element
            var child = <HTMLElement>args.cell.firstElementChild;
            if (child) {
                child.hidden = true;
            }

            // create host element for combo box
            var host = document.createElement("div");
            host.style.border = "none";
            host.style.width = "100%";
            args.cell.style.padding = "0px";
            args.cell.appendChild(host);

            // create combo box and bind to key/value pairs
            var combo = new wijmo.input.ComboBox(host);
            combo.displayMemberPath = "label";
            combo.selectedValuePath = "code";
            combo.itemsSource = items;
            combo.selectedValue = data;
            combo.showDropDownButton = true;
            combo.isRequired = false;
            combo.isEditable = false;

            // handle cellEditEnding event to update the underlying value
            let editEnding = function (s, a: wijmo.grid.CellRangeEventArgs) {
                this.cellEditEnding.removeHandler(editEnding);
                if (!a.cancel) {
                    a.cancel = true;
                    var value = combo.selectedValue == null ? combo.text : combo.selectedValue;
                    this.finishEditing(true);
                    this.setCellData(a.row, a.col, value, false, false);
                    this._updateQuery();
                }
            };
            this.cellEditEnding.addHandler(editEnding, this);

            // force dropdown open on merged cells
            if (!args.range.isSingleCell) {
                combo.isDroppedDown = true;
            }
        }    
    }

    export class QueryDesignGridMergeManager extends wijmo.grid.MergeManager {

        getMergedRange(p: wijmo.grid.GridPanel, r: number, c: number, clip?: boolean) : wijmo.grid.CellRange {
            var range = new wijmo.grid.CellRange(r, c);
            if (r > 0 && p.cellType == wijmo.grid.CellType.Cell) {
                var alias = p.getCellData(1, c, false);
                if (alias != null) {
                    if (c + 1 < p.grid.columns.length) {
                        var adjacent = p.getCellData(1, c + 1, false);
                        if (alias == adjacent) {
                            range.col2 = c + 1;
                        }    
                    }
                    if (c > 0) {
                        var adjacent = p.getCellData(1, c - 1, false);
                        if (alias == adjacent) {
                            range.col = c - 1;
                        }    
                    }
                }
            } else if (r >= QueryDesignGrid._ROW_CONDITION && p.cellType == wijmo.grid.CellType.RowHeader) {
                range = new wijmo.grid.CellRange(QueryDesignGrid._ROW_CONDITION, 0, QueryDesignGrid._ROW_VALUE, 0);
            }
            return range;
        }
    }

    export class QueryContextMenu extends wijmo.input.Menu {

        constructor(element: any, controller: DesignerController) {
            super(element);

            this.itemClicked.addHandler(function (sender, args) {
                var owner = sender.owner;
                var value = sender.selectedValue;
                if (value === "new") {
                    controller.selectQuery(null);
                } else if (value === "execute") {
                    controller.executeQuery();
                } else if (value === "delete") {
                    controller.deleteQuery();
                } else if (value === "reset") {
                    controller.resetWorkspace();
                }
            });
        }
    }

    export class DataTableContextMenu extends wijmo.input.Menu {

        constructor(element: any, controller: DesignerController) {
            super(element);

            this.itemClicked.addHandler(function (sender, args) {
                var owner = sender.owner;
                var value = sender.selectedValue;
                var table = owner.getAttribute("dataId");
                if (value === "delete") {
                    owner.parentElement.removeChild(owner);
                    controller.deleteTable(table);
                    controller.dataSurface.drawLines(true);
                } else {
                    controller.selectFields(table, value === "all");
                    var flex = wijmo.Control.getControl(owner);
                    flex.invalidate(false);
                }
            });
        }
    }

    export class TableJoinContextMenu extends wijmo.input.Menu {

        constructor(element: any, controller: DesignerController) {
            super(element);

            this.itemClicked.addHandler(function (sender, args) {
                var owner = sender.owner;
                var value = sender.selectedValue;
                var join = owner.getAttribute("joinId");
                if (value === "delete") {
                    controller.deleteJoin(join);
                    controller.dataSurface.drawLines(true);
                }
            });
        }
    }
}
