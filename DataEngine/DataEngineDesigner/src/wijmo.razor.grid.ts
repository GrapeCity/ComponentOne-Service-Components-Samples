/// <reference path="typings/wijmo/wijmo.d.ts" />
/// <reference path="typings/wijmo/wijmo.grid.d.ts" />
/// <reference path="typings/wijmo/wijmo.input.d.ts" />

module wijmo.razor {
    'use strict';

    export class FlexGrid extends wijmo.grid.FlexGrid {

        private _groupBy;
        private _panel: wijmo.grid.grouppanel.GroupPanel;
        private _showPanel = false;
        private _filter: wijmo.grid.filter.FlexGridFilter;
        private _showFilters = false;

        // ctor
        
        constructor(element: any, options?) {
            super(element, options);
            wijmo.culture.FlexGrid.groupHeaderFormat = "<b>{value}</b>";
            this.selectionMode = wijmo.grid.SelectionMode.None;
            this.groupHeaderFormat = wijmo.culture.FlexGrid.groupHeaderFormat;
            this.autoGenerateColumns = false;
            this.isReadOnly = true;
            this.rows.defaultSize = 28;
            this.columns.defaultSize = 112;
        }

        // object model

        get showColumnFilters(): boolean {
            return this._showFilters;
        }

        set showColumnFilters(value: boolean) {
            if (this._showFilters !== value) {
                this._showFilters = value;
                if (value && !this._filter) {
                    this._filter = new wijmo.grid.filter.FlexGridFilter(this);
                }
                this._filter.showFilterIcons = value;
            }
        }

        get showGroupPanel(): boolean {
            return this._showPanel;
        }

        set showGroupPanel(value: boolean) {
            if (this._showPanel !== value) {
                this._showPanel = value;
                if (value && !this._panel) {
                    var first = this.hostElement.firstElementChild;
                    var div = this.hostElement.insertBefore(document.createElement("div"), first);
                    this._panel = new wijmo.grid.grouppanel.GroupPanel(div);
                    this._panel.grid = this;
                }
                this._panel.hostElement.style.display = value ? "block" : "none";
                if (value) {
                    this._panel.refresh();
                }
            }
        }

        // RuntimeControl interface

        get dataSourceType(): DataSourceType {
            return DataSourceType.MultipleValuesAndCategories;
        }
        
        defaults(): any {
            return {
                frozenColumns: 0,
                frozenRows: 0,
                headersVisibility: wijmo.grid.HeadersVisibility.Column,
                showColumnFilters: false,
                showGroupPanel: false
            }
        }

        apply(object: any) {
            this.frozenColumns = object.frozenColumns ? object.frozenColumns : 0;
            this.frozenRows = object.frozenRows ? object.frozenRows : 0;
            this.headersVisibility = object.headersVisibility ? object.headersVisibility : wijmo.grid.HeadersVisibility.Column;
            this.showColumnFilters = object.showColumnFilters ? true : false;
            this.showGroupPanel = object.showGroupPanel ? true : false;
            RuntimeDefault.apply(this, object);
        }

        bind(object: any) {
            var self = this;
            this.columns.clear();
            if (object.categories.length) {
                this._groupBy = object.categories.map(function (c) {
                    return c[0];
                });
            } else {
                this._groupBy = null;
            }
            object.values.forEach(function (v) {
                var c = new wijmo.grid.Column();
                c.binding = v[0];
                c.header = v[1];
                c.aggregate = v[2] ? <Aggregate>v[2] : Aggregate.None;
                c.width = "*";
                self.columns.push(c);
            });
        }

        populate(object: any) {
            if (this._groupBy) {
                this._groupBy.forEach(function (g) {
                    object.groupDescriptions.push(new wijmo.collections.PropertyGroupDescription(g));
                });
                this.itemsSource = object;
            } else {
                this.itemsSource = object;
            }
            //this.autoSizeColumn(0);
        }

        onProperty(e: PropertyInfoEventArgs) {
            var info = e.propertyInfo;
            switch (info.name) {
                case "frozenColumns":
                    info.alias = "Frozen Columns";
                    info.type = wijmo.DataType.Number;
                    info.min = 0;
                    info.spin = false;
                    info.group = "Presentation";
                    break;
                case "frozenRows":
                    info.alias = "Frozen Rows";
                    info.type = wijmo.DataType.Number;
                    info.min = 0;
                    info.spin = false;
                    info.group = "Presentation";
                    break;
                case "headersVisibility":
                    info.alias = "Headers";
                    info.type = wijmo.grid.HeadersVisibility;
                    info.choice = true;
                    info.required = true;
                    info.group = "Presentation";
                    break;
                case "showColumnFilters":
                    info.alias = "Show Column Filters";
                    info.type = wijmo.DataType.Boolean;
                    info.group = "Interaction";
                    break;
                case "showGroupPanel":
                    info.alias = "Show Group Panel";
                    info.type = wijmo.DataType.Boolean;
                    info.group = "Interaction";
                    break;
                default:
                    e.cancel = true;
                    break;
            }
        }
    }
}