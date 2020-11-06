/// <reference path="typings/wijmo/wijmo.d.ts" />
/// <reference path="typings/wijmo/wijmo.grid.d.ts" />
/// <reference path="typings/wijmo/wijmo.input.d.ts" />
/// <reference path="typings/wijmo/wijmo.olap.d.ts" />

module wijmo.razor {
    'use strict';

    export class PivotGrid extends wijmo.olap.PivotGrid {

        private _dataSource: any;
        private _pivotEngine: wijmo.olap.PivotEngine;
        private _showColumnTotals = wijmo.olap.ShowTotals.None;
        private _showRowTotals = wijmo.olap.ShowTotals.GrandTotals;
        private _showZeros = false;
        private _totalsBeforeData = false;

        // ctor
        
        constructor(element: any, options?) {
            super(element, options);
            this.customContextMenu = false;
            this.showDetailOnDoubleClick = false;
            this.selectionMode = wijmo.grid.SelectionMode.None;
            this.rows.defaultSize = 28;
            this.columns.defaultSize = 112;
        }

        // object model

        get showColumnTotals(): wijmo.olap.ShowTotals {
            return this._showColumnTotals;
        }
        set showColumnTotals(value: wijmo.olap.ShowTotals) {
            this._showColumnTotals = value;
            if (this._pivotEngine) {
                this._pivotEngine.showColumnTotals = value;
            }
        }

        get showRowTotals(): wijmo.olap.ShowTotals {
            return this._showRowTotals;
        }
        set showRowTotals(value: wijmo.olap.ShowTotals) {
            this._showRowTotals = value;
            if (this._pivotEngine) {
                this._pivotEngine.showRowTotals = value;
            }
        }

        get showZeros(): boolean {
            return this._showZeros;
        }
        set showZeros(value: boolean) {
            this._showZeros = value;
            if (this._pivotEngine) {
                this._pivotEngine.showZeros = value;
            }
        }

        get totalsBeforeData(): boolean {
            return this._totalsBeforeData;
        }
        set totalsBeforeData(value: boolean) {
            this._totalsBeforeData = value;
            if (this._pivotEngine) {
                this._pivotEngine.totalsBeforeData = value;
            }
        }

        // RuntimeControl interface

        get dataSourceType(): DataSourceType {
            return DataSourceType.PivotEngine;
        }
        
        defaults(): any {
            return {
                showColumnTotals: wijmo.olap.ShowTotals.None,
                showRowTotals: wijmo.olap.ShowTotals.GrandTotals,
                showZeros: false,
                totalsBeforeData: false
            }
        }

        apply(object: any) {
            this.showColumnTotals = object.showColumnTotals ? object.showColumnTotals : wijmo.olap.ShowTotals.None;
            this.showRowTotals = object.showRowTotals ? object.showRowTotals : wijmo.olap.ShowTotals.GrandTotals;
            this.showZeros = object.showZeros ? true : false;
            this.totalsBeforeData = object.totalsBeforeData ? true : false;
            RuntimeDefault.apply(this, object);
        }

        bind(object: any) {
            this._dataSource = object;
        }

        populate(object: any) {
            var engine = new wijmo.olap.PivotEngine({
                autoGenerateFields: false,
                itemsSource: object,
                showColumnTotals: this.showColumnTotals,
                showRowTotals: this.showRowTotals,
                showZeros: this.showZeros,
                totalsBeforeData: this.totalsBeforeData
            });
            this._dataSource.values.forEach(function (v) {
                var f = new wijmo.olap.PivotField(engine, v[0], v[1]);
				f.aggregate = v[2] ? <Aggregate>v[2] : Aggregate.Sum;
				if (!engine.fields.getField(v[0])) {
                    engine.fields.push(f);
				}
                engine.valueFields.push(f);
            });
            this._dataSource.categories.forEach(function (v) {
                var f = new wijmo.olap.PivotField(engine, v[0], v[1]);
                f.aggregate = v[2] ? <Aggregate>v[2] : Aggregate.Sum;
                if (!engine.fields.getField(v[0])) {
                    engine.fields.push(f);
				}
                engine.rowFields.push(f);
            });
            this._dataSource.columns.forEach(function (v) {
                var f = new wijmo.olap.PivotField(engine, v[0], v[1]);
                f.aggregate = v[2] ? <Aggregate>v[2] : Aggregate.Sum;
                if (!engine.fields.getField(v[0])) {
                    engine.fields.push(f);
				}
                engine.columnFields.push(f);
            });
            this._pivotEngine = engine;
            this.itemsSource = engine;
            //this.autoSizeColumns();
        }

        onProperty(e: PropertyInfoEventArgs) {
            var info = e.propertyInfo;
            switch (info.name) {
                case "showColumnTotals":
                    info.alias = "Show Column Totals";
                    info.type = wijmo.olap.ShowTotals;
                    info.choice = true;
                    info.required = true;
                    info.group = "Presentation";
                    break;
                case "showRowTotals":
                    info.alias = "Show Row Totals";
                    info.type = wijmo.olap.ShowTotals;
                    info.choice = true;
                    info.required = true;
                    info.group = "Presentation";
                    break;
                case "showZeros":
                    info.alias = "Show Zeros";
                    info.type = wijmo.DataType.Boolean;
                    info.group = "Presentation";
                    break;
                case "totalsBeforeData":
                    info.alias = "Totals Before Data";
                    info.type = wijmo.DataType.Boolean;
                    info.group = "Presentation";
                    break;
                default:
                    e.cancel = true;
                    break;
            }
        }
    }
}