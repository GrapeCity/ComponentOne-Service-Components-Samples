/// <reference path="typings/wijmo/wijmo.d.ts" />
/// <reference path="typings/wijmo/wijmo.input.d.ts" />
/// <reference path="typings/wijmo/wijmo.grid.d.ts" />

module wijmo.razor {
    'use strict';

    /**
     * The @see:PropertyGrid control provides a user interface for browsing the properties of an object.
     */
    export class PropertyGrid extends wijmo.grid.FlexGrid {

        // constants
        static _NAME_COLUMN = 0;
        static _VALUE_COLUMN = 1;
        static _ROW_HEIGHT = 30;

        // property storage
        private _selectedObject: any = null;
        private _title: string = "Properties";

        // private members
        private _editType: wijmo.DataType;
        private _oldValue: any;
        private _append: Function;
        private _remove: Function;
        private _currentInfo: any;
        private _grouped: boolean;
        private _tooltip: wijmo.Tooltip;

        /**
         * Initializes a new instance of the @see:PropertyGrid class.
         *
         * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
         * @param options The JavaScript object containing initialization data for the control.
         */
        constructor(element: any, options?) {
            super(element, {
                autoGenerateColumns: false,
                groupHeaderFormat: "<b>{value}</b>",
                selectionMode: wijmo.grid.SelectionMode.Cell,
                headersVisibility: wijmo.grid.HeadersVisibility.Column,
                allowMerging: wijmo.grid.AllowMerging.ColumnHeaders,
                allowResizing: wijmo.grid.AllowResizing.None,
                allowSorting: false,
                showSort: false
            });

            // built-in merge manager whiffs on column headers
            this.mergeManager = new PropertyGridMergeManager();

            // increase row height to accommodate Wijmo input controls
            this.rows.defaultSize = PropertyGrid._ROW_HEIGHT;

            // create name/value columns
            this.columns.push(new wijmo.grid.Column({
                binding: "alias",
                width: "3*",
                isReadOnly: true,
                header: "Properties"
            }));

            this.columns.push(new wijmo.grid.Column({
                binding: "value",
                width: "2*",
                header: "Properties"
            }));

            this.columns.push(new wijmo.grid.Column({
                binding: "group",
                visible: false
            }));

            // tooltip for long property names
            this._tooltip = new wijmo.Tooltip();
            this._tooltip.cssClass = "property-grid-tooltip";

            // add event handlers
            this.beginningEdit.addHandler(this._beginningEdit, this);
            this.cellEditEnding.addHandler(this._cellEditEnding, this);
            this.cellEditEnded.addHandler(this._cellEditEnded, this);
            this.formatItem.addHandler(this._formatItem, this);
            this.propertyChanged.addHandler(this._propertyChanged, this);
            this.updatingView.addHandler(this._updatingView, this);

            // skip deprecated members
            this.addingProperty.addHandler(function (s, e:PropertyInfoEventArgs) {
                if (e.propertyInfo.name === "disabled") {
                    e.cancel = true;
                }
            });

            // add item handler for collections
            var self = this;
            this._append = () => {
                self._selectedObject.push(new self._currentInfo.ctor);
                self._showObject(self._currentInfo);
            };
            
            // remove item handler for collections
            this._remove = () => {
                if (self.selectedRows.length > 0) {
                    var member, row = self.selectedRows[0];
                    if (row instanceof wijmo.grid.GroupRow) {
                        var group = <wijmo.grid.GroupRow>row;
                        member = group.dataItem.items[0].target;
                    } else {
                        member = row.dataItem.target;
                    }
                    self._selectedObject.remove(member);
                    self._showObject(self._currentInfo);
                }
            };
        }

        //--------------------------------------------------------------------------
        //#region ** object model

        /**
         * Gets or sets the title string for the control.
         */
        get title(): string {
            return this._title;
        }
        set title(value: string) {
            if (value != this._title) {
                this._title = value ? value : "Properties";
                this.columns[0].header = this._title;
                this.columns[1].header = this._title;
            }
        }

        /**
         * Gets or sets the object for which the control displays properties.
         */
        get selectedObject(): any {
            return this._selectedObject;
        }
        set selectedObject(value: any) {
            if (value != this._selectedObject) {
                var self = this;
                this._prepareSelection(value);
                this.deferUpdate(() => {
                    self._refresh();
                });
            }
        }

        /**
         * Gets or sets whether to display collapsible property groups.
         */
        get isGrouped(): boolean {
            return this._grouped;
        }
        set isGrouped(value: boolean) {
            this._grouped = value;
        }

        /**
         * Sets the selected object, along with optional extender properties.
         * 
         * @param value The object for which the control displays properties.
         * @param extenders An array of PropertyInfo objects describing extender properties.
         */
        selectObject(value: any, extenders?: PropertyInfo[]) {
            if (value != this._selectedObject) {
                var self = this;
                this._prepareSelection(value);
                this.deferUpdate(() => {
                    self._refresh(extenders);
                });
            }
        }

        /**
         * Adds a new property definition to the control.
         *
         * @param name The name of the property.
         * @param target The object whose property will be set.
         * @param descriptor The corresponding PropertyDescriptor object, if any.
         * @return The property definition that was added.
         */
        defineProperty(name: string, target: any, descriptor?: PropertyDescriptor) {

            // ensure that the property grid is not empty
            var cv = <wijmo.collections.CollectionView>this.collectionView;
            if (!cv) {
                cv = new wijmo.collections.CollectionView();
                cv.sortDescriptions.push(new wijmo.collections.SortDescription("name", true));
                this.itemsSource = cv;
            }

            // define a new item creator with these parameters
            cv.newItemCreator = function() {
                return new PropertyInfo(name, target, descriptor);
            }

            // add a new item, but don't select it
            cv.addNew();
            var prop = cv.currentAddItem;
            cv.commitNew();
            cv.newItemCreator = null;
            cv.currentItem = null;
            this.refresh(true);
            return prop;
        }

        /**
         * Occurs when a new property is added to the control.
         */
        addingProperty = new Event();
        /**
         * Raises the @see:addingProperty event.
         */
        onAddingProperty(e?: PropertyInfoEventArgs) {
            this.addingProperty.raise(this, e);
        }

        /**
         * Occurs when the value of a property is changed.
         */
        propertyChanged = new Event();
        /**
         * Raises the @see:propertyChanged event.
         */
        onPropertyChanged(e?: PropertyChangedEventArgs) {
            this.propertyChanged.raise(this, e);
        }

        //#endregion

        //--------------------------------------------------------------------------
        //#region ** implementation

        // generates property descriptors for the selected object and refreshes the control
        private _refresh(extenders?: PropertyInfo[]) {
            var data = [];
            var grid = this;
            var self = this._selectedObject;
            var proto = self ? self["__proto__"] : null;
            var array = self ? wijmo.isArray(self) : false;
            var extended = extenders && extenders.length > 0;

            if (!proto || (array && !extended)) {
                this.itemsSource = null;
                return;
            }

            if (array) {
                var cv = new wijmo.collections.CollectionView(extenders);
                cv.sortDescriptions.push(new wijmo.collections.SortDescription("index", true));
                cv.sortDescriptions.push(new wijmo.collections.SortDescription("alias", true));
                var gd = new wijmo.collections.PropertyGroupDescription("group");
                cv.groupDescriptions.push(gd);
                this.itemsSource = cv;
                this.refresh(false);
                return;
            }

            // HACK: first property cannot be numeric, boolean, or choice
            data.push(new PropertyInfo("aardvark", self));

            while (proto) {
                Object.keys(proto).forEach(function (value) {
                    var pd = Object.getOwnPropertyDescriptor(proto, value);
                    if (pd.set) {
                        var info = new PropertyInfo(value, self, pd);
                        var args = new PropertyInfoEventArgs(info);
                        grid.onAddingProperty(args);
                        if (!args.cancel) {
                            data.push(info);
                        }
                    }
                }, self);
                proto = proto["__proto__"];
            }

            if (extenders && extenders.length > 0) {
                data = data.concat(extenders);
            }

            var cv = new wijmo.collections.CollectionView(data);
            cv.sortDescriptions.push(new wijmo.collections.SortDescription("alias", true));

            if (this._grouped) {
                var gd = new wijmo.collections.PropertyGroupDescription("group");
                cv.groupDescriptions.push(gd);
            }

            this.itemsSource = cv;

            // HACK: remove dummy property once the items are set
            cv.removeAt(0);
            this.refresh(false);
        }

        // cleans up event handlers and column footers
        private _prepareSelection(value: any, info?: any) {
            var include = this._currentInfo ? this._currentInfo.include : null;
            if (isFunction(include)) {
                this.addingProperty.removeHandler(include);
            }
            include = info ? info.include : null;
            if (isFunction(include)) {
                this.addingProperty.addHandler(include);
            }
            this._selectedObject = value;
            this._currentInfo = info;
            this.columnFooters.rows.clear();
        }

        // displays object properties and column footers for collections
        private _showObject(item: any) {
            var self = this;
            var extenders = null;
            var value = item.target[item.name];
            this._prepareSelection(value, item);
            if (wijmo.isArray(value)) {
                var o = <wijmo.collections.ObservableArray>value;
                o.collectionChanged.addHandler(function () {
                    var a = new PropertyChangedEventArgs(item.name, value, value);
                    self.onPropertyChanged(a);
                });
                this.columns.push(new wijmo.grid.Column({
                    binding: "group",
                    visible: false
                }));
                var fn = item["extenders"];
                extenders = fn ? fn() : null;
                if (this.columnFooters.rows.length == 0) {
                    this.columnFooters.rows.push(new wijmo.grid.Row());
                }
            }
            this.deferUpdate(() => {
                self._refresh(extenders);
            });
        }

        // handler for the propertyChanged event
        private _propertyChanged(sender, e) {
            if (this._currentInfo) {
                var fn = this._currentInfo.serialize;
                if (fn) {
                    this._currentInfo.value = fn();
                }
            }
        }

        // handler for the beginningEdit event
        private _beginningEdit(sender, e) {
            var args = <wijmo.grid.CellRangeEventArgs>e;
            var row = <wijmo.grid.Row>this.rows[args.row];
            var item = row.dataItem;
            var type = item.type;
            if (type == wijmo.DataType.Boolean) {
                var value = !(item.value);
                args.panel.setCellData(args.row, args.col, value, false, true);
                args.cancel = true;
                var a = new PropertyChangedEventArgs(item.name, this._oldValue, value);
                this.onPropertyChanged(a);
            } else if (item.readonly || (item.object && !item.choice)) {
                args.cancel = true;
            }
        }

        // handler for the cellEditEnding event
        private _cellEditEnding(sender, e) {
            var args = <wijmo.grid.CellRangeEventArgs>e;
            var row = <wijmo.grid.Row>this.rows[args.row];
            if (row) {
                var item = row.dataItem;
                if (item) {
                    this._oldValue = item.value;
                    if (this._editType == wijmo.DataType.Array) {
                        var value = args.panel.grid.activeEditor.value;
                        if (value.charAt(0) !== "[") {
                            value = "[" + value + "]";
                        }
                        item.value = JSON.parse(value);
                    }
                }
            }
        }

        // handler for the cellEditEnding event
        private _cellEditEnded(sender, e) {
            var args = <wijmo.grid.CellRangeEventArgs>e;
            if (!args.cancel) {
                var row = <wijmo.grid.Row>this.rows[args.row];
                if (row) {
                    var item = row.dataItem;
                    if (item && item.value !== this._oldValue) {
                        var a = new PropertyChangedEventArgs(item.name, this._oldValue, item.value);
                        this.onPropertyChanged(a);
                    }
                }
            }
        }

        // handler for the formatItem event
        private _formatItem(sender, e) {
            var self = this;
            var args = <wijmo.grid.FormatItemEventArgs>e;
            if (args.panel == this.columnFooters) {
                if (args.col == PropertyGrid._NAME_COLUMN) {
                    args.cell.style.padding = "1px";
                    args.cell.innerHTML = "<button type='button'>+</button><button type='button'>X</button>";
                    wijmo.getElement(args.cell.firstChild).onclick = function() {
                        self._append();
                    };
                    wijmo.getElement(args.cell.firstChild.nextSibling).onclick = function() {
                        self._remove();
                    };
                }
            } else if (args.panel == this.cells && args.col == PropertyGrid._NAME_COLUMN) {
                var row = <wijmo.grid.Row>this.rows[args.row];
                var item = row.dataItem;
                self._tooltip.setTooltip(args.cell, item.alias);
            } else if (args.panel == this.cells && args.col == PropertyGrid._VALUE_COLUMN) {
                var row = <wijmo.grid.Row>this.rows[args.row];
                var item = row.dataItem;
                var type = item.type;
                var editing = args.panel.grid.editRange && args.panel.grid.editRange.row === args.row;
                if (type == wijmo.DataType.Object) {
                    type = wijmo.getType(item.value);
                }
                if (editing) {
                    this._editType = type;
                    if (item.choice) {
                        this._editChoice(args);
                    } else if (type == wijmo.DataType.Number) {
                        this._editNumber(args);
                    } else if (type == wijmo.DataType.Date) {
                        this._editDate(args);
                    }
                } else {
                    if (type == wijmo.DataType.Boolean) {
                        args.cell.style.textAlign = "center";
                        if (args.cell.firstChild) {
                            args.cell.firstChild.textContent = item.value ? "\u2611" : "\u2610";
                        } else {
                            args.cell.innerText = item.value ? "\u2611" : "\u2610";
                        }
                    } else if (item.choice) {
                        if (args.cell.firstChild) {
                            args.cell.firstChild.textContent = type[item.value];
                        }
                    } else if (type == wijmo.DataType.Number) {
                        args.cell.style.textAlign = "right";
                        if (item.format) {
                            args.cell.innerText = wijmo.Globalize.format(item.value, item.format);
                        }
                    } else if (type == wijmo.DataType.Date) {
                        if (item.value) {
                            args.cell.innerText = wijmo.Globalize.formatDate(item.value, item.format ? item.format : "d");
                        }
                    } else if (item.object) {
                        var self = this;
                        args.cell.style.textAlign = "right";
                        args.cell.style.padding = "1px";
                        args.cell.innerHTML = "<button type='button'>...</button>";
                        wijmo.getElement(args.cell.firstChild).onclick = function() {
                            self._showObject(item);
                        };
                    }
                }
            }
        }

        // handler for the updatingView event
        private _updatingView() {
            this._tooltip.dispose();
        }

        // create dropdown combo for choice list cell
        private _editChoice(args: wijmo.grid.FormatItemEventArgs) {
            var row = <wijmo.grid.Row>this.rows[args.row];
            var item = row.dataItem;
            var type = this._editType;
            var enums = [];

            // derive key/value pairs for choice list
            Object.keys(type).forEach(function (value: string, index: number) {
                if (item.object) {
                    enums.push({ key: value, value: type[value]});                                
                } else { // enum
                    var k = Number(value);
                    if (!isNaN(k)) {
                        enums.push({ key: k, value: type[k]});
                    }
                }
            });

            // sort enums by display value if desired
            if (item.sorted) {
                enums.sort(function (a, b) {
                    return (a.value < b.value) ? -1 : (a.value > b.value) ? 1 : 0;
                });
            }

            // hide first child element
            var child = <HTMLElement>args.cell.firstElementChild;
            child.hidden = true;

            // create host element for combo box
            var host = document.createElement("div");
            host.style.border = "none";
            host.style.width = "100%";
            args.cell.style.padding = "0px";
            args.cell.appendChild(host);

            // create combo box and bind to key/value pairs
            var combo = new wijmo.input.ComboBox(host);
            combo.displayMemberPath = "value";
            combo.selectedValuePath = "key";
            combo.itemsSource = enums;
            combo.selectedValue = item.value;
            combo.showDropDownButton = true;
            combo.isRequired = item.required ? true : false;

            // remove host/combo after a selection is made or the combo loses focus
            combo.isDroppedDownChanged.addHandler(function (sender, e) {
                if (!combo.isDroppedDown) {
                    //host.remove();
                }
            });

            // handle cellEditEnding event to update the underlying value
            let editEnding = function (s, a) {
                this.cellEditEnding.removeHandler(editEnding);
                var x = <wijmo.grid.CellRangeEventArgs>a;
                if (!x.cancel) {
                    x.cancel = true;
                    var value = combo.selectedValue;
                    this.finishEditing(true);
                    item.value = item.object ? value : Number(value);
                    if (item.value !== this._oldValue) {
                        var y = new PropertyChangedEventArgs(item.name, this._oldValue, item.value);
                        this.onPropertyChanged(y);
                    }
                }
            };
            this.cellEditEnding.addHandler(editEnding, this);
        }    

        // create input control for numeric cell
        private _editNumber(args: wijmo.grid.FormatItemEventArgs) {
            var row = <wijmo.grid.Row>this.rows[args.row];
            var item = row.dataItem;

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

            // remove border characteristics
            //args.cell.style.borderLeft = "none";
            //args.cell.style.borderTop = "none";
            //args.cell.style.borderRadius = "0px";

            // create and initialize numeric control
            var input = new wijmo.input.InputNumber(host); // (args.cell);
            input.isRequired = false;
            input.showSpinner = false;
            input.format = "d";
            input.value = item.value;
            
            // configure numeric control properties, if specified
            if (item.format) {
                input.format = item.format;
            }
            if (isNumber(item.min)) {
                input.min = item.min;
            }
            if (isNumber(item.max)) {
                input.max = item.max;
            }
            if (item.spin) {
                input.showSpinner = true;
                input.step = 1;
            }
            if (item.placeholder) {
                input.placeholder = item.placeholder;
            }

            // handle cellEditEnding event to update the underlying value
            let editEnding = function (s, a) {
                this.cellEditEnding.removeHandler(editEnding);
                var x = <wijmo.grid.CellRangeEventArgs>a;
                if (!x.cancel) {
                    x.cancel = true;
                    item.value = (input.value !== null) ? Number(input.value) : null;
                    if (item.value !== this._oldValue) {
                        var y = new PropertyChangedEventArgs(item.name, this._oldValue, item.value);
                        this.onPropertyChanged(y);
                    }
                }
            };
            this.cellEditEnding.addHandler(editEnding, this);

            // give the numeric control focus
            input.selectAll();
            input.focus();
        }

        // create input control for date cell
        private _editDate(args: wijmo.grid.FormatItemEventArgs) {
            var row = <wijmo.grid.Row>this.rows[args.row];
            var item = row.dataItem;

            // hide first child element
            var child = <HTMLElement>args.cell.firstElementChild;
            child.hidden = true;

            // remove border characteristics
            args.cell.style.borderLeft = "none";
            args.cell.style.borderTop = "none";
            args.cell.style.borderRadius = "0px";

            // create and initialize date control
            var input = new wijmo.input.InputDate(args.cell);
            input.isRequired = false;
            input.showDropDownButton = true;
            input.value = item.value;
            
            // handle cellEditEnding event to update the underlying value
            let editEnding = function (s, a) {
                this.cellEditEnding.removeHandler(editEnding);
                var x = <wijmo.grid.CellRangeEventArgs>a;
                if (!x.cancel) {
                    x.cancel = true;
                    var iso = (input.value !== null) ? input.value.toISOString() : null;
                    var changed = iso !== this._oldValue;
                    item.value = (input.value !== null) ? input.value : null;
                    if (changed) {
                        var y = new PropertyChangedEventArgs(item.name, this._oldValue, item.value);
                        this.onPropertyChanged(y);
                    }
                }
            };
            this.cellEditEnding.addHandler(editEnding, this);

            // give the date control focus
            input.focus();
        }

        //#endregion
    }

    /**
     * The @see:PropertyInfo class provides an interface for getting/setting a single property of a particular object.
     */
    export class PropertyInfo {
        _name: string;
        _alias: string;
        _desc: PropertyDescriptor;
        _default: any;
        _target: any;
        _type: any;
        _group: string;
        _choice: boolean;
        _sorted: boolean;
        _required: boolean;
        _min: number;
        _max: number;
        _spin: boolean;

        /**
         * Initializes a new instance of the @see:PropertyInfo class.
         *
         * @param name The name of the property.
         * @param target The object whose property will be set.
         * @param descriptor The corresponding PropertyDescriptor object, if any.
         */
        constructor(name: string, target: any, descriptor?: PropertyDescriptor) {
            this._name = name;
            this._target = target;
            if (descriptor) {
                this._desc = descriptor;
            } else {
                this._desc = {
                    get: function() {
                        return this[name];
                    },
                    set: function(value) {
                        this[name] = value;
                    }
                }
            }
            this._type = wijmo.getType(this.value);
        }

        /**
         * Gets the name of the property.
         */
        get name(): string {
            return this._name;
        }

        /**
         * Gets or sets the display name of the property.
         */
        get alias(): string {
            return this._alias ? this._alias : this._name;
        }
        set alias(v: string) {
            this._alias = v;
        }

        /**
         * Gets or sets the value of the property.
         */
        get value(): any {
            return this._desc.get.call(this._target);
        }
        set value(v: any) {
            this._desc.set.call(this._target, this._coerce(v));
        }

        /**
         * Gets or sets the default value of the property.
         */
        get defaultValue(): any {
            return this._default;
        }
        set defaultValue(v: any) {
            this._default = v;
        }

        /**
         * Gets or sets the type of the property.
         */
        get type(): any {
            return this._type;
        }
        set type(t: any) {
            this._type = t;
        }

        /**
         * Gets the object whose property will be set.
         */
        get target(): any {
            return this._target;
        }

        /**
         * Gets or sets the group name of the property, if any.
         */
        get group(): string {
            return this._group;
        }
        set group(g: string) {
            this._group = g;
        }

        /**
         * Gets or sets whether the property should be rendered as a choice list.
         */
        get choice(): boolean {
            return this._choice;
        }
        set choice(c: boolean) {
            this._choice = c;
        }

        /**
         * Gets or sets whether choice list values should be sorted.
         */
        get sorted(): boolean {
            return this._sorted;
        }
        set sorted(s: boolean) {
            this._sorted = s;
        }

        /**
         * Gets or sets whether the property requires a non-null value.
         */
        get required(): boolean {
            return this._required;
        }
        set required(r: boolean) {
            this._required = r;
        }

        /**
         * Gets or sets the minimum value for a numeric property.
         */
        get min(): number {
            return this._min;
        }
        set min(n: number) {
            this._min = n;
        }

        /**
         * Gets or sets the maximum value for a numeric property.
         */
        get max(): number {
            return this._max;
        }
        set max(n: number) {
            this._max = n;
        }

        /**
         * Gets or sets whether to display a spin button for a numeric property.
         */
        get spin(): boolean {
            return this._spin;
        }
        set spin(s: boolean) {
            this._spin = s;
        }

        // converts string values to the appropriate type
        _coerce(value: any): any {
            if (!isString(value)) {
                return value;
            }
            if (this._type === wijmo.DataType.Array) {
                if (value.length === 0) {
                    return [];
                }
                if (value.charAt(0) === "[") {
                    return JSON.parse(value);
                } else {
                    return JSON.parse("[" + value + "]");
                }
            } else if (this._type === wijmo.DataType.Number) {
                return parseInt(value);
            } else if (this._type === wijmo.DataType.Date) {
                return (value === "") ? null : new Date(Date.parse(value));
            }
            return value;
        }
    }

    /**
     * Provides arguments for the @see:PropertyGrid.addingProperty event.
     */
    export class PropertyInfoEventArgs extends CancelEventArgs {
        _info: PropertyInfo;

        /**
         * Initializes a new instance of the @see:PropertyInfoEventArgs class.
         *
         * @param info The @see:PropertyInfo to be added to the control.
         */
        constructor(info: PropertyInfo) {
            super();
            this._info = info;
        }
        
        /**
         * Gets a reference to the @see:PropertyInfo to be added to the control.
         */
        get propertyInfo(): PropertyInfo {
            return this._info;
        }
    }

    /**
     * Handles cell merging for column headers.
     */
    export class PropertyGridMergeManager extends wijmo.grid.MergeManager {

        getMergedRange(p: wijmo.grid.GridPanel, r: number, c: number, clip?: boolean) : wijmo.grid.CellRange {
            if (p.cellType == wijmo.grid.CellType.ColumnHeader || p.cellType == wijmo.grid.CellType.ColumnFooter) {
                return new wijmo.grid.CellRange(0, 0, 0, p.grid.columns.length - 1);
            }
            return super.getMergedRange(p, r, c, clip);
        }
    }
}