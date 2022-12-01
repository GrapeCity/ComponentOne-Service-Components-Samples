/// <reference path="typings/wijmo/wijmo.d.ts" />
/// <reference path="typings/wijmo/wijmo.chart.d.ts" />
/// <reference path="typings/wijmo/wijmo.grid.d.ts" />
/// <reference path="typings/wijmo/wijmo.input.d.ts" />

module wijmo.razor.designer {
    'use strict';

    export class ControlToolbox extends Control {

        private _tools = {
            "wijmo.razor.ColumnChart": {
                name: "Column Chart",
                ctor: "wijmo.razor.ColumnChart",
                widget: "wijmo.razor.designer.DataSourceDesignerWidget",
                width: 400,
                height: 300,
                image: "images/ColumnChart.png"
            },
            "wijmo.razor.BarChart": {
                name: "Bar Chart",
                ctor: "wijmo.razor.BarChart",
                widget: "wijmo.razor.designer.DataSourceDesignerWidget",
                width: 400,
                height: 300,
                image: "images/BarChart.png"
            },
            "wijmo.razor.LineChart": {
                name: "Line Chart",
                ctor: "wijmo.razor.LineChart",
                widget: "wijmo.razor.designer.DataSourceDesignerWidget",
                width: 400,
                height: 300,
                image: "images/LineChart.png"
            },
            "wijmo.razor.AreaChart": {
                name: "Area Chart",
                ctor: "wijmo.razor.AreaChart",
                widget: "wijmo.razor.designer.DataSourceDesignerWidget",
                width: 400,
                height: 300,
                image: "images/AreaChart.png"
            },
            "wijmo.razor.ScatterChart": {
                name: "Scatter Chart",
                ctor: "wijmo.razor.ScatterChart",
                widget: "wijmo.razor.designer.DataSourceDesignerWidget",
                width: 400,
                height: 300,
                image: "images/ScatterChart.png"
            },
            "wijmo.razor.SplineChart": {
                name: "Spline Chart",
                ctor: "wijmo.razor.SplineChart",
                widget: "wijmo.razor.designer.DataSourceDesignerWidget",
                width: 400,
                height: 300,
                image: "images/SplineChart.png"
            },
            "wijmo.razor.PieChart": {
                name: "Pie Chart",
                ctor: "wijmo.razor.PieChart",
                widget: "wijmo.razor.designer.DataSourceDesignerWidget",
                width: 400,
                height: 300,
                image: "images/PieChart.png"
            },
            "wijmo.razor.RadarChart": {
                name: "Radar Chart",
                ctor: "wijmo.razor.RadarChart",
                widget: "wijmo.razor.designer.DataSourceDesignerWidget",
                width: 400,
                height: 300,
                image: "images/RadarChart.png"
            },
            "wijmo.razor.TreeMap": {
                name: "Tree Map",
                ctor: "wijmo.razor.TreeMap",
                widget: "wijmo.razor.designer.DataSourceDesignerWidget",
                width: 400,
                height: 300,
                image: "images/TreeMap.png"
            },
            "wijmo.razor.FlexGrid": {
                name: "Grid",
                ctor: "wijmo.razor.FlexGrid",
                widget: "wijmo.razor.designer.DataSourceDesignerWidget",
                width: 400,
                height: 300,
                image: "images/Grid.png"
            },
            "wijmo.razor.PivotGrid": {
                name: "Pivot Grid",
                ctor: "wijmo.razor.PivotGrid",
                widget: "wijmo.razor.designer.DataSourceDesignerWidget",
                width: 400,
                height: 300,
                image: "images/PivotGrid.png"
            },
            "wijmo.razor.LinearGauge": {
                name: "Linear Gauge",
                ctor: "wijmo.razor.LinearGauge",
                widget: "wijmo.razor.designer.DataSourceDesignerWidget",
                width: 300,
                height: 24,
                image: "images/LinearGauge.png"
            },
            "wijmo.razor.RadialGauge": {
                name: "Radial Gauge",
                ctor: "wijmo.razor.RadialGauge",
                widget: "wijmo.razor.designer.DataSourceDesignerWidget",
                width: 200,
                height: 128,
                image: "images/RadialGauge.png"
            },
            "wijmo.razor.Numeric": {
                name: "Number",
                ctor: "wijmo.razor.Numeric",
                widget: "wijmo.razor.designer.DataSourceDesignerWidget",
                width: 250,
                height: 46,
                image: "images/Number.png"
            },
            "wijmo.razor.Label": {
                name: "Label",
                ctor: "wijmo.razor.Label",
                widget: "wijmo.razor.designer.DesignerWidget",
                width: 250,
                height: 46,
                image: "images/Label.png"
            }
        };

        constructor(element: any, controller: DesignerController) {
            super(element);
            var self = this;
            var host = wijmo.getElement(element);

            // initialize toolbox icons
            Object.keys(this._tools).forEach(function (ctor) {
                var div = host.querySelector("[ctor='" + ctor + "']");
                var img = <HTMLImageElement>div.firstElementChild;
                var tool = self._tools[ctor];
                img.src = tool.image;
                img.title = tool.name;
                img.draggable = true;
                img.addEventListener('dragstart', function (e) {
                    controller.startDrag(tool, e);
                    e.dataTransfer.effectAllowed = 'copy';
                    e.dataTransfer.setData('text', ''); // required in FireFox (note: text/html will throw in IE!)
                });
            });
        }
    }

    export class ListBoxNoSelection extends wijmo.input.ListBox {

        constructor(element: any, options?) {
            super(element, options);
            var self = this;

            this.hostElement.addEventListener('mousedown', function (e: MouseEvent) {
                var children = self.hostElement.children;
                for (var index = 0; index < children.length; index++) {
                    if (contains(children[index], e.target)) {
                        self.selectedIndex = index;
                        return;
                    }
                }
            });
        }
    }

    export class SourceFieldList extends ListBoxNoSelection {

        constructor(element: any, controller: DataSourcePanel) {
            super(element, {
                displayMemberPath: "value",
                selectedValuePath: "key"
            });
            var self = this;

            this.loadedItems.addHandler(function (e) {
                var items = this.hostElement.querySelectorAll(".wj-listbox-item");
                for (var i = 0; i < items.length; i++) {
                    items[i].draggable = true;
                }
            }, this);

            this.hostElement.addEventListener('dragstart', function (e) {
                if (self.selectedItem) {
                    controller.dragObject = self.selectedItem;
                    e.dataTransfer.effectAllowed = 'copy';
                    e.dataTransfer.setData('text', ''); // required in FireFox (note: text/html will throw in IE!)
                }
            });
        }
    }

    export enum DropMode {
        MultipleValues,
        SingleValue
    }

    export class TargetFieldList extends ListBoxNoSelection {

        private _dropMode = DropMode.MultipleValues;

        constructor(element: any, controller: DataSourcePanel) {
            super(element, {
                displayMemberPath: "value",
                selectedValuePath: "key"
            });
            var self = this;

            this.hostElement.addEventListener('dragover', function (e) {
                if (controller.dragObject) {
                    e.dataTransfer.dropEffect = 'copy';
                    e.preventDefault();
                }
            });

            this.hostElement.addEventListener('drop', function (e) {
                if (controller.dragObject) {
                    var cv = <wijmo.collections.CollectionView>self.collectionView;
                    if (self._dropMode == DropMode.SingleValue) {
                        cv.sourceCollection.pop();
                    }
                    cv.sourceCollection.push(controller.dragObject  );
                    cv.refresh();
                    self.selectedIndex = cv.itemCount - 1;
                    self.showSelection();
                    controller.dragObject = null;
                    controller.onDataSourceChanged(EventArgs.empty);
                }
            });
        }

        get dropMode(): DropMode {
            return this._dropMode;
        }

        set dropMode(value: DropMode) {
            this._dropMode = value;
        }
    }

    export class ValuesFieldList extends TargetFieldList {

        private _hasAggregates = true;

        constructor(element: any, controller: DataSourcePanel) {
            super(element, controller);
            var self = this;

            this.formatItem.addHandler(function (sender, e: wijmo.input.FormatItemEventArgs) {
                if (self.hasAggregates) {
                    e.item.className += " listbox-menu-item";
                    e.item.innerHTML = e.item.innerHTML + "<span title='Menu'>\u2261</span>";
                } else {
                    e.item.className += " listbox-delete-item";
                    e.item.innerHTML = e.item.innerHTML + "<span title='Remove'>X</span>";
                }
            }, this);

            this.hostElement.addEventListener('click', function (e: MouseEvent) {
                var targetElement = <HTMLElement>e.target;
                if (targetElement.tagName.toLowerCase() == "span") {
                    var children = self.hostElement.children;
                    for (var index = 0; index < children.length; index++) {
                        if (contains(children[index], e.srcElement)) {
                            var cv = <wijmo.collections.CollectionView>self.collectionView;
                            if (!self.hasAggregates) {
                                cv.sourceCollection.splice(index, 1);
                                cv.refresh();
                                controller.onDataSourceChanged(EventArgs.empty);
                                return;
                            }
                            var menu = <DataValueContextMenu>wijmo.Control.getControl("#data-value-menu");
                            var drop = menu.dropDown;
                            menu.owner = <HTMLElement>e.target;
                            menu.selectedIndex = -1;
                            menu.target = cv.sourceCollection[index];
                            menu.itemFormatter = function (index, content) {
                                if (index != 6) {
                                    var agg = (menu.target && menu.target.agg) ? menu.target.agg : 0;
                                    var char = (index == agg) ? "\u2022" : "";
                                    content = "<span>" + char + "</span>" + content;
                                }
                                return content;
                            };
                            menu.onRemove = function () {
                                cv.sourceCollection.splice(index, 1);
                                cv.refresh();
                                controller.onDataSourceChanged(EventArgs.empty);
                            };
                            if (menu.onIsDroppedDownChanging(new wijmo.CancelEventArgs())) {
                                wijmo.showPopup(drop, e);
                                menu.onIsDroppedDownChanged();
                                drop.focus();
                            }
                            return;
                        }
                    }
                }
            });
        }

        get hasAggregates(): boolean {
            return this._hasAggregates;
        }

        set hasAggregates(value: boolean) {
            if (this._hasAggregates != value) {
                this._hasAggregates = value;
                this.loadList();
            }
        }
    }

    export class CategoryFieldList extends TargetFieldList {

        constructor(element: any, controller: DataSourcePanel) {
            super(element, controller);
            var self = this;

            this.formatItem.addHandler(function (sender, e: wijmo.input.FormatItemEventArgs) {
                e.item.className += " listbox-delete-item";
                e.item.innerHTML = e.item.innerHTML + "<span title='Remove'>X</span>";
            }, this);

            this.hostElement.addEventListener('click', function (e: MouseEvent) {
                var targetElement = <HTMLElement>e.target;
                if (targetElement.tagName.toLowerCase() == "span") {
                    var children = self.hostElement.children;
                    for (var index = 0; index < children.length; index++) {
                        if (contains(children[index], e.srcElement)) {
                            var cv = <wijmo.collections.CollectionView>self.collectionView;
                            cv.sourceCollection.splice(index, 1);
                            cv.refresh();
                            controller.onDataSourceChanged(EventArgs.empty);
                            return;
                        }
                    }
                }
            });
        }
    }

    export class DataValueContextMenu extends wijmo.input.Menu {

        private _target: any;
        private _separator = 6;
        private _remove = 7;
        private _onRemove: Function;

        constructor(element: any, controller: DataSourcePanel) {
            super(element);
            var self = this;
            this.dropDownCssClass = "listbox-context-menu";

            this.itemClicked.addHandler(function (sender, args) {
                var index = self.selectedIndex;
                if (index == self._remove) {
                    self.onRemove();
                } else if (index >= 0 && index < self._separator) {
                    self.target.agg = index;
                    controller.onDataSourceChanged(EventArgs.empty);
                }
            });
        }

        get target(): any {
            return this._target;
        }

        set target(value: any) {
            this._target = value;
        }

        get onRemove(): Function {
            return this._onRemove;
        }

        set onRemove(value: Function) {
            this._onRemove = value;
        }
    }

    export class DataSourcePanel extends Control {

        private _dragObject: any;
        private _dataSources: wijmo.input.ComboBox;
        private _fields: SourceFieldList;
        private _values: ValuesFieldList;
        private _categories: CategoryFieldList;
        private _columns: CategoryFieldList;
        private _labelValues: HTMLElement;
        private _labelCategories: HTMLElement;
        private _labelColumns: HTMLElement;
        private _controller: DesignerController;
        private _fireEvent: boolean;
        private _type = DataSourceType.MultipleValuesAndCategories;

        constructor(element: any, controller: DesignerController, options?: any) {
            super(element, options);

            this._controller = controller;
            this._fireEvent = true;
            this._dataSources = new wijmo.input.ComboBox("#datasource-combo");
            this._dataSources.displayMemberPath = "value";
            this._dataSources.selectedValuePath = "key";
            this._dataSources.isRequired = false;
            this._dataSources.placeholder = "Select a data source...";
            
            this._dataSources.selectedIndexChanged.addHandler(function (e) {
                var items = [];
                if (this._dataSources.selectedIndex >= 0) {
                    var fields = controller.getDataSourceFields(this._dataSources.selectedValue);
                    Object.keys(fields).map(function (id) {
                        items.push({
                            key: id,
                            value: fields[id]
                        });
                    });
                    this._fields.hostElement.style.display = "";
                } else {
                    this._fields.hostElement.style.display = "none";
                }
                this._fields.itemsSource = items;
                this._values.itemsSource = [];
                this._categories.itemsSource = [];
                this._columns.itemsSource = [];
                if (this._dataSources.selectedIndex >= 0) {
                    this.onDataSourceChanged(EventArgs.empty);
                }
            }, this);

            this._fields = new SourceFieldList("#datasource-fields", this);
            this._values = new ValuesFieldList("#datasource-values", this);
            this._categories = new CategoryFieldList("#datasource-category", this);
            this._columns = new CategoryFieldList("#datasource-columns", this);
            this._labelValues = wijmo.getElement("#label-datasource-values");
            this._labelCategories = wijmo.getElement("#label-datasource-category");
            this._labelColumns = wijmo.getElement("#label-datasource-columns");

            this._columns.hostElement.style.display = "none";
            this._labelColumns.style.display = "none";
        }

        refresh() {
            var items = this._controller.getDataSourceItems();
            this._fireEvent = false;
            this._dataSources.itemsSource = items;
            this._dataSources.selectedIndex = -1;
            this._fireEvent = true;
            this._controller.selectWidget(null);
        }

        get dragObject(): any {
            return this._dragObject;
        }

        set dragObject(value: any) {
            this._dragObject = value;
        }

        get dataSource(): any {
            function tuple(i) {
                return i.agg ? [i.key, i.value, i.agg] : [i.key, i.value];
            }
            return {
                id: this._dataSources.selectedValue,
                values: this._values.collectionView.items.map(tuple),
                categories: this._categories.collectionView.items.map(tuple),
                columns: this._columns.collectionView.items.map(tuple)
            };
        }

        set dataSource(value: any) {
            var self = this;
            this._fireEvent = false;
            function item(i) {
                return {
                    key: i[0],
                    value: i[1],
                    agg: i[2] ? i[2] : 0
                };
            }
            if (value) {
                this._dataSources.selectedValue = value.id;
                this._values.itemsSource = value.values.map(item);
                this._categories.itemsSource = value.categories.map(item);
                if (value.columns) {
                    this._columns.itemsSource = value.columns.map(item);
                }
            } else {
                this._dataSources.selectedValue = null;
            }
            this._fireEvent = true;
        }

        get dataSourceType(): DataSourceType {
            return this._type;
        }

        set dataSourceType(value: DataSourceType) {
            if (this._type != value) {
                var isPivot = false;
                var multiValue = true;
                var multiCategory = false;
                var hasValue = true;
                var hasCategory = true;
                var hasAggregates = true;
                this._type = value;

                switch (value) {
                    case DataSourceType.MultipleValuesAndCategories:
                        multiCategory = true;
                        break;
                    case DataSourceType.SingleValueAndCategory:
                        multiValue = false;
                        break;
                    case DataSourceType.SingleValueOnly:
                        multiValue = false;
                        hasCategory = false;
                        break;
                    case DataSourceType.SingleValueMultipleCategories:
                        multiValue = false;
                        multiCategory = true;
                        hasAggregates = false;
                        break;
                    case DataSourceType.PivotEngine:
                        isPivot = true;
                        multiCategory = true;
                        break;
                    case DataSourceType.None:
                        hasValue = false;
                        hasCategory = false;
                        hasAggregates = this._values.hasAggregates;
                        break;
                }

                this._labelValues.innerText = multiValue ? "Values" : "Value";
                this._labelValues.style.display = hasValue ? "" : "none";

                this._values.dropMode = multiValue ? DropMode.MultipleValues : DropMode.SingleValue;
                this._values.hasAggregates = hasAggregates;
                this._values.hostElement.style.display = hasValue ? "" : "none";

                this._labelCategories.innerText = isPivot ? "Rows" : (multiCategory ? "Categories" : "Category");
                this._labelCategories.style.display = hasCategory ? "" : "none";

                this._categories.dropMode = multiCategory ? DropMode.MultipleValues : DropMode.SingleValue;
                this._categories.hostElement.style.display = hasCategory ? "" : "none";

                this._labelColumns.innerText = "Columns";
                this._labelColumns.style.display = isPivot ? "" : "none";

                this._columns.dropMode = DropMode.MultipleValues;
                this._columns.hostElement.style.display = isPivot ? "" : "none";

                this.isDisabled = (value == DataSourceType.None);
            }
        }

        dataSourceChanged = new Event();

        onDataSourceChanged(e?: EventArgs) {
            if (this._fireEvent) {
                this.dataSourceChanged.raise(this, e);
            }
        }
    }

    export enum ResizeDirection {
        None,
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft
    }

    export class DesignerWidget {

        private static _DATA_KEY = "kintone-Widget";
        protected _placeholder: HTMLElement;
        private _szUp: HTMLElement;
        private _szUpRight: HTMLElement;
        private _szRight: HTMLElement;
        private _szDownRight: HTMLElement;
        private _szDown: HTMLElement;
        private _szDownLeft: HTMLElement;
        private _szLeft: HTMLElement;
        private _szUpLeft: HTMLElement;
        private _move: HTMLElement;
        private _direction: ResizeDirection;
        private _controller: DesignerController;
        private _control: Control;
        private _id: any;
        private _type: string;
        private _x: number;
        private _y: number;

        cssUnit(value: any) {
            return isNumber(value) ? (value > 0 ? value.toString() + "px" : "0px") : value;
        }

        constructor(element: any, controller: DesignerController, options: any) {
            var self = this;
            var div = document.createElement("div");
            this._initialSize(div, options);

            div.addEventListener('mouseup', function (e) {
                controller.selectWidget(self);
                e.stopPropagation();
            });
           
            if (!options.nomenu) {
                div.addEventListener('contextmenu', function (e) {
                    e.stopPropagation();
                    var menu = <wijmo.input.Menu>wijmo.Control.getControl("#ui-widget-menu");
                    var drop = menu.dropDown;
                    if (menu && drop && !wijmo.closest(e.target, '[disabled]')) {
                        e.preventDefault();
                        menu.owner = div;
                        menu.selectedIndex = -1;
                        if (menu.onIsDroppedDownChanging(new wijmo.CancelEventArgs())) {
                            wijmo.showPopup(drop, e);
                            menu.onIsDroppedDownChanged();
                            drop.focus();
                        }
                    }
                }, false);
            }
           
            this._placeholder = div;
            this._controller = controller;
            this._id = options.id;

            div[DesignerWidget._DATA_KEY] = this;
            div.draggable = false; // (options.id !== 1);
            div.style.overflow = "visible";
            div.style.zIndex = "1000";
            wijmo.getElement(element).appendChild(div);
            div.setAttribute("ctrlId", options.id);

            var ctor = eval(options.ctor);
            this._control = new ctor(div);
            var names = options.ctor.split(".");
            this._type = names[names.length - 1];

            function _resizeInit(e: MouseEvent) {
                self._x = e.x;
                self._y = e.y;
                if (wijmo.hasClass(<HTMLElement>e.target, "widget-move")) {
                    self._direction = ResizeDirection.None;
                } else if (wijmo.hasClass(<HTMLElement>e.target, "widget-resize-up-left")) {
                    self._direction = ResizeDirection.UpLeft;
                } else if (wijmo.hasClass(<HTMLElement>e.target, "widget-resize-up-right")) {
                    self._direction = ResizeDirection.UpRight;
                } else if (wijmo.hasClass(<HTMLElement>e.target, "widget-resize-down-left")) {
                    self._direction = ResizeDirection.DownLeft;
                } else if (wijmo.hasClass(<HTMLElement>e.target, "widget-resize-down-right")) {
                    self._direction = ResizeDirection.DownRight;
                } else if (wijmo.hasClass(<HTMLElement>e.target, "widget-resize-left")) {
                    self._direction = ResizeDirection.Left;
                } else if (wijmo.hasClass(<HTMLElement>e.target, "widget-resize-right")) {
                    self._direction = ResizeDirection.Right;
                } else if (wijmo.hasClass(<HTMLElement>e.target, "widget-resize-up")) {
                    self._direction = ResizeDirection.Up;
                } else if (wijmo.hasClass(<HTMLElement>e.target, "widget-resize-down")) {
                    self._direction = ResizeDirection.Down;
                }
                window.addEventListener("mousemove", _resizeStart, true);
                window.addEventListener("mouseup", _resizeStop, true);
            }

            function _resizeStart(e: MouseEvent) {
                if (self._direction == ResizeDirection.None) {
                    self.left = self.cssUnit(parseInt(self.left) + e.x - self._x);
                    self.top = self.cssUnit(parseInt(self.top) + e.y - self._y);
                } else if (self._direction == ResizeDirection.UpLeft) {
                    self.left = self.cssUnit(parseInt(self.left) + e.x - self._x);
                    self.top = self.cssUnit(parseInt(self.top) + e.y - self._y);
                    self.width = self.cssUnit(parseInt(self.width) - e.x + self._x);
                    self.height = self.cssUnit(parseInt(self.height) - e.y + self._y);
                } else if (self._direction == ResizeDirection.Up) {
                    self.top = self.cssUnit(parseInt(self.top) + e.y - self._y);
                    self.height = self.cssUnit(parseInt(self.height) - e.y + self._y);
                } else if (self._direction == ResizeDirection.UpRight) {
                    self.top = self.cssUnit(parseInt(self.top) + e.y - self._y);
                    self.width = self.cssUnit(parseInt(self.width) + e.x - self._x);
                    self.height = self.cssUnit(parseInt(self.height) - e.y + self._y);
                } else if (self._direction == ResizeDirection.Right) {
                    self.width = self.cssUnit(parseInt(self.width) + e.x - self._x);
                } else if (self._direction == ResizeDirection.DownRight) {
                    self.width = self.cssUnit(parseInt(self.width) + e.x - self._x);
                    self.height = self.cssUnit(parseInt(self.height) + e.y - self._y);
                } else if (self._direction == ResizeDirection.Down) {
                    self.height = self.cssUnit(parseInt(self.height) + e.y - self._y);
                } else if (self._direction == ResizeDirection.DownLeft) {
                    self.left = self.cssUnit(parseInt(self.left) + e.x - self._x);
                    self.width = self.cssUnit(parseInt(self.width) - e.x + self._x);
                    self.height = self.cssUnit(parseInt(self.height) + e.y - self._y);
                } else if (self._direction == ResizeDirection.Left) {
                    self.left = self.cssUnit(parseInt(self.left) + e.x - self._x);
                    self.width = self.cssUnit(parseInt(self.width) - e.x + self._x);
                }
                self._x = e.x;
                self._y = e.y;
            }

            function _resizeStop(e: MouseEvent) {
                window.removeEventListener("mousemove", _resizeStart, true);
                window.removeEventListener("mouseup", _resizeStop, true);
            }

            this._szUp = document.createElement("div");
            this._szUp.className = "widget-resize widget-resize-up";
            this._szUp.addEventListener("mousedown", _resizeInit, false);
            div.appendChild(this._szUp);

            this._szUpRight = document.createElement("div");
            this._szUpRight.className = "widget-resize widget-resize-up-right";
            this._szUpRight.addEventListener("mousedown", _resizeInit, false);
            div.appendChild(this._szUpRight);

            this._szRight = document.createElement("div");
            this._szRight.className = "widget-resize widget-resize-right";
            this._szRight.addEventListener("mousedown", _resizeInit, false);
            div.appendChild(this._szRight);

            this._szDownRight = document.createElement("div");
            this._szDownRight.className = "widget-resize widget-resize-down-right";
            this._szDownRight.addEventListener("mousedown", _resizeInit, false);
            div.appendChild(this._szDownRight);

            this._szDown = document.createElement("div");
            this._szDown.className = "widget-resize widget-resize-down";
            this._szDown.addEventListener("mousedown", _resizeInit, false);
            div.appendChild(this._szDown);

            this._szDownLeft = document.createElement("div");
            this._szDownLeft.className = "widget-resize widget-resize-down-left";
            this._szDownLeft.addEventListener("mousedown", _resizeInit, false);
            div.appendChild(this._szDownLeft);

            this._szLeft = document.createElement("div");
            this._szLeft.className = "widget-resize widget-resize-left";
            this._szLeft.addEventListener("mousedown", _resizeInit, false);
            div.appendChild(this._szLeft);

            this._szUpLeft = document.createElement("div");
            this._szUpLeft.className = "widget-resize widget-resize-up-left";
            this._szUpLeft.addEventListener("mousedown", _resizeInit, false);
            div.appendChild(this._szUpLeft);

            this._move = document.createElement("div");
            this._move.className = "widget-move";
            this._move.addEventListener("mousedown", _resizeInit, false);
            div.appendChild(this._move);
        }

        static getWidget(element: any): DesignerWidget {
            var e = getElement(element);
            return e ? asType(e[DesignerWidget._DATA_KEY], DesignerWidget, true) : null;
        }

        get type(): string {
            return this._type;
        }
        
        get control(): RuntimeControl {
            return <RuntimeControl>this._control;
        }

        get controller(): DesignerController {
            return this._controller;
        }

        get id(): any {
            return this._id;
        }

        get left(): string {
            return this._placeholder.style.left;
        }

        set left(value: string) {
            if (this._placeholder.style.left != value) {
                this._placeholder.style.left = value;
                this._controller.updateStyle(this._id, this._placeholder.style);
                if (this._control) {
                    this._control.invalidate(true);
                }
            }
        }

        get top(): string {
            return this._placeholder.style.top;
        }

        set top(value: string) {
            if (this._placeholder.style.top != value) {
                this._placeholder.style.top = value;
                this._controller.updateStyle(this._id, this._placeholder.style);
                if (this._control) {
                    this._control.invalidate(true);
                }
            }
        }

        get width(): string {
            return this._placeholder.style.width;
        }

        set width(value: string) {
            if (this._placeholder.style.width != value) {
                this._placeholder.style.width = value;
                this._controller.updateStyle(this._id, this._placeholder.style);
                if (this._control) {
                    this._control.invalidate(true);
                }
            }
        }

        get height(): string {
            return this._placeholder.style.height;
        }

        set height(value: string) {
            if (this._placeholder.style.height != value) {
                this._placeholder.style.height = value;
                this._controller.updateStyle(this._id, this._placeholder.style);
                if (this._control) {
                    this._control.invalidate(true);
                }
            }
        }

        protected _mouseMove(e: MouseEvent) {
            window.status = e.offsetX.toString() + ", " + e.offsetY.toString();
        }

        protected _initialSize(div: HTMLElement, options: any) {
            div.style.position = "relative";
            div.style.height = this.cssUnit(options.height);
            div.style.width = this.cssUnit(options.width);
            div.style.left = this.cssUnit(options.left);
            div.style.top = this.cssUnit(options.top);
        }

        protected _persist(name: string, value: string) {
            this._controller.setWidgetProperty(this._id, name, value);
        }
    }

    export class DataSourceDesignerWidget extends DesignerWidget {

        private _dataSource: any;

        constructor(element: any, controller: DesignerController, options: any) {
            super(element, controller, options);
            this._dataSource = options.dataSource;
        }

        get dataSource(): any {
            return this._dataSource;
        }

        set dataSource(value: any) {
            if (this._dataSource != value) {
                this._dataSource = value;
                this._persist("dataSource", value);
                this.control.bind(value);
                this.populate(value);
            }
        }

        populate(value: any) {
            var self = this;
            var q = this.controller.getQuery(value.id);
            if (q) {
                var sort = this.controller.getQuerySortOrder(value.id);
                wijmo.httpRequest("/QueryData", {
                    data: {name: q.name, sort: sort},
                    success: function(xhr) {
                        var items, json = JSON.parse(xhr.response);
                        if (q.maxRows > 0) {
                            items = new wijmo.collections.CollectionView(json.slice(0, q.maxRows));
                        } else {
                            items = new wijmo.collections.CollectionView(json);
                        }
                        try {
                            self.control.populate(items);
						} catch (msg) {
                            self.controller.showError(msg);
						}
                    },
                    error: function(xhr) {
                        self.control.hostElement.innerText = "error";
                    }
                });
            }
        }
    }

    export class DesignerWidgetContextMenu extends wijmo.input.Menu {

        constructor(element: any, controller: DesignerController) {
            super(element);

            this.itemClicked.addHandler(function (sender, args) {
                var owner = sender.owner;
                var value = sender.selectedValue;
                var ctrlId = owner.getAttribute("ctrlId");
                if (value === "delete") {
                    owner.parentElement.removeChild(owner);
                    controller.deleteWidget(ctrlId);
                    controller.selectWidget(null);
                }
            });
        }
    }

    export class DesignSurface extends Control {

        private _controller: DesignerController;

        cssUnit(value: any) {
            return isNumber(value) ? value.toString() + "px" : value;
        }

        constructor(element: any, controller: DesignerController) {
            super(element);
            var self = this;
            var host = wijmo.getElement(element);

            host.addEventListener('dragover', function (e) {
                if (controller.getDragObject()) {
                    e.dataTransfer.dropEffect = 'copy';
                    e.preventDefault();
                }
            });

            host.addEventListener('drop', function (e) {
                var tool = controller.getDragObject();
                e.preventDefault();
                e.stopPropagation();
                if (tool instanceof HTMLElement) {
                } else if (tool) {
                    var options = {
                        id: new Date().getTime(),
                        parent: controller.getSelectedView().id,
                        name: tool["name"],
                        ctor: tool["ctor"],
                        widget: tool["widget"],
                        width: tool["width"],
                        height: tool["height"],
                        left: e.offsetX,
                        top: e.offsetY
                    };
                    controller.createWidget(options);
                    var ctor = eval(options.widget);
                    var widget = new ctor(element, controller, options);
                    widget.control.hostElement.style.position = "absolute";
                    host.appendChild(widget.control.hostElement);
                    if (widget.control.defaults) {
                        var d = widget.control.defaults();
                        Object.keys(d).forEach(function (key) {
                            controller.setWidgetProperty(options.id, key, d[key]);
                        });
                    }
                    controller.selectWidget(widget);
                }
                controller.endDrag();
            }, false);

            host.addEventListener('mouseup', function (e) {
                controller.selectWidget(null);
                controller.selectViewProperties();
                e.stopPropagation();
            });
           
            this._controller = controller;
        }

        get controller(): DesignerController {
            return this._controller;
        }
    }

    export class ViewDefinitions extends wijmo.input.ListBox {

        constructor(element: any, controller: DesignerController) {
            super(element, {
                displayMemberPath: "value",
                selectedValuePath: "key"
            });
            var self = this;

            this.selectedIndexChanged.addHandler(function (sender, e) {
                controller.selectView(self.selectedValue);
            });
        }
    }

    export class ViewProperties {

        private _controller: DesignerController;

        constructor(controller: DesignerController) {
            this._controller = controller;
        }

        get name(): string {
            return this._controller.getSelectedView().name;
        }
        set name(value: string) {
            this._controller.getSelectedView().name = value;
        }
    }

    export class ViewContextMenu extends wijmo.input.Menu {

        constructor(element: any, controller: DesignerController) {
            super(element);

            this.itemClicked.addHandler(function (sender, args) {
                var value = sender.selectedValue;
                if (value === "new") {
                    controller.createView();
                } else if (value === "share") {
                    controller.shareView();
                } else if (value === "delete") {
                    controller.deleteView();
                }
            });
        }
    }
}