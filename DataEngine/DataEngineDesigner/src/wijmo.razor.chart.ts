/// <reference path="typings/wijmo/wijmo.d.ts" />
/// <reference path="typings/wijmo/wijmo.chart.d.ts" />
/// <reference path="typings/wijmo/wijmo.chart.radar.d.ts" />
/// <reference path="typings/wijmo/wijmo.chart.hierarchical.d.ts" />

module wijmo.razor {
    'use strict';

    export enum Palettes {
        standard,
        cocoa,
        coral,
        dark,
        highcontrast,
        light,
        midnight,
        modern,
        organic,
        slate,
        zen,
        cyborg,
        superhero,
        flatly,
        darkly,
        cerulan
    }

    function getChartPalette(value: wijmo.razor.Palettes) {
        switch (value) {
            case wijmo.razor.Palettes.cerulan:
                return wijmo.chart.Palettes.cerulan;
            case wijmo.razor.Palettes.cocoa:
                return wijmo.chart.Palettes.cocoa;
            case wijmo.razor.Palettes.coral:
                return wijmo.chart.Palettes.coral;
            case wijmo.razor.Palettes.cyborg:
                return wijmo.chart.Palettes.cyborg;
            case wijmo.razor.Palettes.dark:
                return wijmo.chart.Palettes.dark;
            case wijmo.razor.Palettes.darkly:
                return wijmo.chart.Palettes.darkly;
            case wijmo.razor.Palettes.flatly:
                return wijmo.chart.Palettes.flatly;
            case wijmo.razor.Palettes.highcontrast:
                return wijmo.chart.Palettes.highcontrast;
            case wijmo.razor.Palettes.light:
                return wijmo.chart.Palettes.light;
            case wijmo.razor.Palettes.midnight:
                return wijmo.chart.Palettes.midnight;
            case wijmo.razor.Palettes.modern:
                return wijmo.chart.Palettes.modern;
            case wijmo.razor.Palettes.organic:
                return wijmo.chart.Palettes.organic;
            case wijmo.razor.Palettes.slate:
                return wijmo.chart.Palettes.slate;
            case wijmo.razor.Palettes.standard:
                return wijmo.chart.Palettes.standard;
            case wijmo.razor.Palettes.superhero:
                return wijmo.chart.Palettes.superhero;
            case wijmo.razor.Palettes.zen:
                return wijmo.chart.Palettes.zen;
            default:
                return wijmo.chart.Palettes.standard;
        }
    }

    export class AggregateSeries extends wijmo.chart.Series {

        private _aggregate = Aggregate.Sum;

        get aggregate(): Aggregate {
            return this._aggregate;
        }

        set aggregate(value: Aggregate) {
            this._aggregate = (value == Aggregate.None) ? Aggregate.Sum : value;
        }
    }

    export class FlexChart extends wijmo.chart.FlexChart {

        protected _colors: wijmo.razor.Palettes = wijmo.razor.Palettes.standard;
        private _groupBy;

        // ctor

        constructor(element: any, options?) {
            super(element, options);
            this.hostElement.style.position = "absolute";
            this.tooltip.content = "<b>{seriesName}</b><br/>{__name} {y}";
        }

        // object model

        get legendPosition(): wijmo.chart.Position {
            return this.legend.position;
        }

        set legendPosition(value: wijmo.chart.Position) {
            if (this.legend.position != value) {
                this.legend.position = value;
            }
        }

        get axisXaxisLine(): boolean {
            return this.axisX.axisLine;
        }
        
        set axisXaxisLine(value: boolean) {
            if (this.axisX.axisLine != value) {
                this.axisX.axisLine = value;
            }
        }

        get axisXformat(): string {
            return this.axisX.format;
        }

        set axisXformat(value: string) {
            if (this.axisX.format != value) {
                this.axisX.format = value;
            }
        }

        get axisXlabels(): boolean {
            return this.axisX.labels;
        }
        
        set axisXlabels(value: boolean) {
            if (this.axisX.labels != value) {
                this.axisX.labels = value;
            }
        }

        get axisXmajorGrid(): boolean {
            return this.axisX.majorGrid;
        }
        
        set axisXmajorGrid(value: boolean) {
            if (this.axisX.majorGrid != value) {
                this.axisX.majorGrid = value;
            }
        }

        get axisXminorGrid(): boolean {
            return this.axisX.minorGrid;
        }
        
        set axisXminorGrid(value: boolean) {
            if (this.axisX.minorGrid != value) {
                this.axisX.minorGrid = value;
            }
        }

        get axisXreversed(): boolean {
            return this.axisX.reversed;
        }
        
        set axisXreversed(value: boolean) {
            if (this.axisX.reversed != value) {
                this.axisX.reversed = value;
            }
        }

        get axisXtitle(): string {
            return this.axisX.title;
        }
        
        set axisXtitle(value: string) {
            if (this.axisX.title != value) {
                this.axisX.title = value;
            }
        }

        get axisYaxisLine(): boolean {
            return this.axisY.axisLine;
        }
        
        set axisYaxisLine(value: boolean) {
            if (this.axisY.axisLine != value) {
                this.axisY.axisLine = value;
            }
        }

        get axisYformat(): string {
            return this.axisY.format;
        }

        set axisYformat(value: string) {
            if (this.axisY.format != value) {
                this.axisY.format = value;
            }
        }
        
        get axisYlabels(): boolean {
            return this.axisY.labels;
        }
        
        set axisYlabels(value: boolean) {
            if (this.axisY.labels != value) {
                this.axisY.labels = value;
            }
        }

        get axisYmajorGrid(): boolean {
            return this.axisY.majorGrid;
        }
        
        set axisYmajorGrid(value: boolean) {
            if (this.axisY.majorGrid != value) {
                this.axisY.majorGrid = value;
            }
        }

        get axisYminorGrid(): boolean {
            return this.axisY.minorGrid;
        }
        
        set axisYminorGrid(value: boolean) {
            if (this.axisY.minorGrid != value) {
                this.axisY.minorGrid = value;
            }
        }

        get axisYreversed(): boolean {
            return this.axisY.reversed;
        }
        
        set axisYreversed(value: boolean) {
            if (this.axisY.reversed != value) {
                this.axisY.reversed = value;
            }
        }

        get axisYtitle(): string {
            return this.axisY.title;
        }
        
        set axisYtitle(value: string) {
            if (this.axisY.title != value) {
                this.axisY.title = value;
            }
        }

        get colors(): wijmo.razor.Palettes {
            return this._colors;
        }

        set colors(value: wijmo.razor.Palettes) {
            if (this._colors != value) {
                this._colors = value;
                this.palette = getChartPalette(value);
            }
        }

        // RuntimeControl interface

        get dataSourceType(): DataSourceType {
            return DataSourceType.MultipleValuesAndCategories;
        }

        defaults(): any {
            return {
                legendPosition: wijmo.chart.Position.Auto,
                legendToggle: false,
                header: null,
                footer: null,
                rotated: false,
                stacking: wijmo.chart.Stacking.None,
                colors: wijmo.razor.Palettes.standard,
                axisXaxisLine: true,
                axisXformat: null,
                axisXlabels: true,
                axisXmajorGrid: false,
                axisXminorGrid: false,
                axisXreversed: false,
                axisYaxisLine: false,
                axisYformat: null,
                axisYlabels: true,
                axisYmajorGrid: true,
                axisYminorGrid: false,
                axisYreversed: false,
            }
        }

        apply(object: any) {
            this.colors = object.colors ? object.colors : wijmo.razor.Palettes.standard;
            this.legendPosition = object.legendPosition ? object.legendPosition : wijmo.chart.Position.None;
            this.legendToggle = object.legendToggle ? true : false;
            this.header = object.header;
            this.footer = object.footer;
            this.rotated = object.rotated ? true : false;
            this.stacking = object.stacking ? object.stacking : wijmo.chart.Stacking.None;
            this.axisXaxisLine = object.axisXaxisLine ? true : false;
            this.axisXformat = object.axisXformat;
            this.axisXlabels = object.axisXlabels ? true : false;
            this.axisXmajorGrid = object.axisXmajorGrid ? true : false;
            this.axisXminorGrid = object.axisXminorGrid ? true : false;
            this.axisXreversed = object.axisXreversed ? true : false;
            this.axisXtitle = object.axisXtitle;
            this.axisYaxisLine = object.axisYaxisLine ? true : false;
            this.axisYformat = object.axisYformat;
            this.axisYlabels = object.axisYlabels ? true : false;
            this.axisYmajorGrid = object.axisYmajorGrid ? true : false;
            this.axisYminorGrid = object.axisYminorGrid ? true : false;
            this.axisYreversed = object.axisYreversed ? true : false;
            this.axisYtitle = object.axisYtitle;
            RuntimeDefault.apply(this, object);
        }

        bind(object: any) {
            var self = this;
            var groups = object.categories.length;
            this.series.clear();
            if (groups > 0) {
                this.bindingX = object.categories[groups - 1][0];
                this._groupBy = object.categories.map(function (c) {
                    return c[0];
                });
            } else {
                this.bindingX = null;
                this._groupBy = null;
            }
            object.values.forEach(function (v) {
                var s = new AggregateSeries();
                s.binding = v[0];
                s.name = v[1];
                s.aggregate = v[2] ? <Aggregate>v[2] : Aggregate.None;
                self.series.push(s);
            });
        }

        populate(object: any) {
            if (this._groupBy) {
                var converter = null, self = this, fmt = this.axisXformat;
                var items = [], labels = [], prefix = [], groupLabels = [];
                if (fmt && fmt.length > 0) {
                    converter = function(item, name) {
                        let b = new wijmo.Binding(name);
                        return Globalize.format(b.getValue(item), fmt);
                    };
                }
                this._groupBy.forEach(function (g) {
                    object.groupDescriptions.push(new wijmo.collections.PropertyGroupDescription(g, converter));
                    groupLabels.push([]);
                });
                this._aggregate(object.groups, items, labels, prefix, groupLabels);
                this.itemsSource = items;
                var len = groupLabels.length;
                if (this._isBarChart()) {
                    this.axisY.itemsSource = (len > 1) ? groupLabels[len - 1] : labels;
                } else {
                    this.axisX.itemsSource = (len > 1) ? groupLabels[len - 1] : labels;
                }
                if (len > 1) {       
                    for (var i = len - 2; i >= 0; i--) {
                        for (var j = 0; j < groupLabels[i].length; j++) {
                            groupLabels[i][j].value += (groupLabels[i][j].width - 1) / 2;
                        }
                        this._createGroupAxes(groupLabels[i]);
                    }
                }
            } else {
                this.itemsSource = object;
            }
        }

        onProperty(e: PropertyInfoEventArgs) {
            var info = e.propertyInfo;
            switch (info.name) {
                case "legendPosition":
                    info.alias = "Position";
                    info.type = wijmo.chart.Position;
                    info.choice = true;
                    info.required = true;
                    info.group = "Legend";
                    break;
                case "legendToggle":
                    info.alias = "Toggle";
                    info.type = wijmo.DataType.Boolean;
                    info.group = "Legend";
                    break;
                case "header":
                    info.alias = "Header";
                    info.type = wijmo.DataType.String;
                    info.group = "Presentation";
                    break;
                case "footer":
                    info.alias = "Footer";
                    info.type = wijmo.DataType.String;
                    info.group = "Presentation";
                    break;
                case "rotated":
                    info.alias = "Rotated";
                    info.type = wijmo.DataType.Boolean;
                    info.group = "Presentation";
                    break;
                case "stacking":
                    info.alias = "Stacking";
                    info.type = wijmo.chart.Stacking;
                    info.choice = true;
                    info.required = true;
                    info.group = "Presentation";
                    break;
                case "colors":
                    info.alias = "Palette";
                    info.type = wijmo.razor.Palettes;
                    info.choice = true;
                    info.required = true;
                    info.sorted = true;
                    info.group = "Presentation";
                    break;
                case "axisXaxisLine":
                    info.alias = "Show Line";
                    info.type = wijmo.DataType.Boolean;
                    info.group = "Category Axis";
                    break;
                case "axisXformat":
                    info.alias = "Format";
                    info.type = wijmo.DataType.String;
                    info.group = "Category Axis";
                    break;
                case "axisXlabels":
                    info.alias = "Show Labels";
                    info.type = wijmo.DataType.Boolean;
                    info.group = "Category Axis";
                    break;
                case "axisXmajorGrid":
                    info.alias = "Show Major Grid";
                    info.type = wijmo.DataType.Boolean;
                    info.group = "Category Axis";
                    break;
                case "axisXminorGrid":
                    info.alias = "Show Minor Grid";
                    info.type = wijmo.DataType.Boolean;
                    info.group = "Category Axis";
                    break;
                case "axisXreversed":
                    info.alias = "Reversed";
                    info.type = wijmo.DataType.Boolean;
                    info.group = "Category Axis";
                    break;
                case "axisXtitle":
                    info.alias = "Title";
                    info.type = wijmo.DataType.String;
                    info.group = "Category Axis";
                    break;
                case "axisYaxisLine":
                    info.alias = "Show Line";
                    info.type = wijmo.DataType.Boolean;
                    info.group = "Value Axis";
                    break;
                case "axisYformat":
                    info.alias = "Format";
                    info.type = wijmo.DataType.String;
                    info.group = "Value Axis";
                    break;
                case "axisYlabels":
                    info.alias = "Show Labels";
                    info.type = wijmo.DataType.Boolean;
                    info.group = "Value Axis";
                    break;
                case "axisYmajorGrid":
                    info.alias = "Show Major Grid";
                    info.type = wijmo.DataType.Boolean;
                    info.group = "Value Axis";
                    break;
                case "axisYminorGrid":
                    info.alias = "Show Minor Grid";
                    info.type = wijmo.DataType.Boolean;
                    info.group = "Value Axis";
                    break;
                case "axisYreversed":
                    info.alias = "Reversed";
                    info.type = wijmo.DataType.Boolean;
                    info.group = "Value Axis";
                    break;
                case "axisYtitle":
                    info.alias = "Title";
                    info.type = wijmo.DataType.String;
                    info.group = "Value Axis";
                    break;
                default:
                    e.cancel = true;
                    break;
            }
        }

        // implementation

        private _isBarChart(): boolean {
            return this.chartType === wijmo.chart.ChartType.Bar;
        }

        private _aggregate(groups: wijmo.collections.CollectionViewGroup[], items: any[], labels: any[], prefix: string[], groupLabels: any[]) {
            for (var i = 0; i < groups.length; i++) {
                var g = groups[i];
                if (g.isBottomLevel) {
                    prefix.push(g.name);
                    var item = {__name: prefix.join("; ")};
                    this.series.forEach(function (s:AggregateSeries) {
                        if (s.aggregate) {
                            item[s.binding] = g.getAggregate(s.aggregate, s.binding);
                        }
                    });
                    items.push(item);
                    labels.push({value: items.length - 1, text: item.__name});
                    prefix.pop();
                } else {
                    prefix.push(g.name);
                    this._aggregate(g.groups, items, labels, prefix, groupLabels);
                    prefix.pop();
                }
                var level = groupLabels[g.level];
                var prevLabel = level.length ? level[level.length - 1] : null;
                var label = {
                    text: g.name,
                    value: prevLabel ? prevLabel.value + prevLabel.width : 0,
                    width: g.groups.length ? g.groups.length : 1
                };
                groupLabels[g.level].push(label);
            }
        }

        private _createGroupAxes(groups: any) {
            var chart = this,
                rawAxis = this._isBarChart() ? chart.axisY : chart.axisX,
                ax;

            if (!groups || this.axes.length > this._groupBy.length) {
                return;
            }

            // create auxiliary series
            ax = new wijmo.chart.Axis();
            ax.labelAngle = 0;
            ax.labelPadding = 6;
            ax.position = this._isBarChart() ? wijmo.chart.Position.Left : wijmo.chart.Position.Bottom;
            ax.majorTickMarks = wijmo.chart.TickMark.None;
            ax.axisLine = false;;

            // set axis data source
            ax.itemsSource = groups;

            // custom item formatting
            ax.itemFormatter = (engine, label) => {
                // find group
                var group = groups.filter(function (obj) {
                    return obj.value == label.val;
                })[0];
                // draw custom decoration
                var w, x, x1, x2, y, y1, y2;
                w = 0.5 * group.width;
                if (!this._isBarChart()) {
                    x1 = ax.convert(label.val - w) + 5;
                    x2 = ax.convert(label.val + w) - 5;
                    if (ax._axrect) {
                        y = ax._axrect.top;
                        engine.drawLine(x1, y, x2, y);
                        engine.drawLine(x1, y, x1, y - 5);
                        engine.drawLine(x2, y, x2, y - 5);
                        engine.drawLine(label.pos.x, y, label.pos.x, y + 5);
                    }
                } else {
                    y1 = ax.convert(label.val + w) + 5;
                    y2 = ax.convert(label.val - w) - 5;
                    if (ax._axrect) {
                        x = ax._axrect.left + ax._axrect.width - 5;
                        engine.drawLine(x, y1, x, y2);
                        engine.drawLine(x, y1, x + 5, y1);
                        engine.drawLine(x, y2, x + 5, y2);
                        engine.drawLine(x, label.pos.y, x - 5, label.pos.y);
                    }
                }
                return label;
            };

            ax.min = rawAxis.actualMin;
            ax.max = rawAxis.actualMax;
            // sync axis limits with main x-axis
            rawAxis.rangeChanged.addHandler(function () {
                ax.min = rawAxis.actualMin;
                ax.max = rawAxis.actualMax;
            });
            var series = new wijmo.chart.Series();
            series.visibility = wijmo.chart.SeriesVisibility.Hidden;
            if (!this._isBarChart()) {
                series.axisX = ax;
            } else {
                series.axisY = ax;
            }
            chart.series.push(series);
        }
    }

    export class FlexPie extends wijmo.chart.FlexPie {

        protected _colors: wijmo.razor.Palettes = wijmo.razor.Palettes.standard;
        protected _function: wijmo.Aggregate = wijmo.Aggregate.Sum;
 
        // ctor
        
        constructor(element: any, options?) {
            super(element, options);
            this.hostElement.style.position = "absolute";
        }

        // object model

        get legendPosition(): wijmo.chart.Position {
            return this.legend.position;
        }

        set legendPosition(value: wijmo.chart.Position) {
            if (this.legend.position != value) {
                this.legend.position = value;
            }
        }

        get colors(): wijmo.razor.Palettes {
            return this._colors;
        }

        set colors(value: wijmo.razor.Palettes) {
            if (this._colors != value) {
                this._colors = value;
                this.palette = getChartPalette(value);
            }
        }

        // RuntimeControl interface
        
        get dataSourceType(): DataSourceType {
            return DataSourceType.SingleValueAndCategory;
        }
        
        defaults(): any {
            return {
                legendPosition: wijmo.chart.Position.Auto,
                colors: wijmo.razor.Palettes.standard
            }
        }

        apply(object: any) {
            this.colors = object.colors ? object.colors : wijmo.razor.Palettes.standard;
            this.legendPosition = object.legendPosition ? object.legendPosition : wijmo.chart.Position.None;
            RuntimeDefault.apply(this, object);
        }

        bind(object: any) {
            var self = this;
            if (object.categories.length) {
                this.bindingName = object.categories[0][0];
            } else {
                this.bindingName = null;
            }
            if (object.values.length) {
                this.binding = object.values[0][0];
                this._function = object.values[0][2] ? <Aggregate>object.values[0][2] : Aggregate.Sum;
                if (this._function == Aggregate.None) {
                    this._function = Aggregate.Sum;
                }
            } else {
                this.binding = null;
            }
        }

        populate(object: any) {
            if (this.binding && this.bindingName) {
                var items = [];
                object.groupDescriptions.push(new wijmo.collections.PropertyGroupDescription(this.bindingName));
                this._aggregate(object.groups, items);
                this.itemsSource = object;
            } else {
                this.itemsSource = null;
            }
        }

        onProperty(e: PropertyInfoEventArgs) {
            var info = e.propertyInfo;
            switch (info.name) {
                case "legendPosition":
                    info.alias = "Position";
                    info.type = wijmo.chart.Position;
                    info.choice = true;
                    info.required = true;
                    info.group = "Legend";
                    break;
                case "colors":
                    info.alias = "Palette";
                    info.type = wijmo.razor.Palettes;
                    info.choice = true;
                    info.required = true;
                    info.sorted = true;
                    info.group = "Presentation";
                    break;
                default:
                    e.cancel = true;
                    break;
            }
        }

        // implementation
        
        private _aggregate(groups: wijmo.collections.CollectionViewGroup[], items: any[]) {
            for (var i = 0; i < groups.length; i++) {
                var g = groups[i];
                var item = {};
                item[this.bindingName] = g.name;
                item[this.binding] = g.getAggregate(this._function, this.binding);
                items.push(item);
            }
        }
    }

    export class AggregateRadarSeries extends wijmo.chart.radar.FlexRadarSeries {

        private _aggregate = Aggregate.Sum;

        get aggregate(): Aggregate {
            return this._aggregate;
        }

        set aggregate(value: Aggregate) {
            this._aggregate = (value == Aggregate.None) ? Aggregate.Sum : value;
        }
    }

    export class FlexRadar extends wijmo.chart.radar.FlexRadar {

        protected _colors: wijmo.razor.Palettes = wijmo.razor.Palettes.standard;
 
        // ctor
        
        constructor(element: any, options?) {
            super(element, options);
            this.interpolateNulls = true;
            this.hostElement.style.position = "absolute";
        }

        // object model

        get legendPosition(): wijmo.chart.Position {
            return this.legend.position;
        }

        set legendPosition(value: wijmo.chart.Position) {
            if (this.legend.position != value) {
                this.legend.position = value;
            }
        }

        get colors(): wijmo.razor.Palettes {
            return this._colors;
        }

        set colors(value: wijmo.razor.Palettes) {
            if (this._colors != value) {
                this._colors = value;
                this.palette = getChartPalette(value);
            }
        }

        // RuntimeControl interface
        
        get dataSourceType(): DataSourceType {
            return DataSourceType.MultipleValuesSingleCategory;
        }
        
        defaults(): any {
            return {
                legendPosition: wijmo.chart.Position.Auto,
                colors: wijmo.razor.Palettes.standard
            }
        }

        apply(object: any) {
            this.colors = object.colors ? object.colors : wijmo.razor.Palettes.standard;
            this.legendPosition = object.legendPosition ? object.legendPosition : wijmo.chart.Position.None;
            RuntimeDefault.apply(this, object);
        }

        bind(object: any) {
            var self = this;
            this.series.clear();
            if (object.categories.length) {
                this.bindingX = object.categories[0][0];
            } else {
                this.bindingX = null;
            }
            object.values.forEach(function (v) {
                var s = new AggregateRadarSeries();
                s.binding = v[0];
                s.name = v[1];
                s.aggregate = v[2] ? <Aggregate>v[2] : Aggregate.None;
                self.series.push(s);
            });
        }

        populate(object: any) {
            if (this.bindingX) {
                var items = [];
                object.groupDescriptions.push(new wijmo.collections.PropertyGroupDescription(this.bindingX));
                this._aggregate(object.groups, items);
                this.itemsSource = items;
            } else {
                this.itemsSource = null;
            }
        }

        onProperty(e: PropertyInfoEventArgs) {
            var info = e.propertyInfo;
            switch (info.name) {
                case "legendPosition":
                    info.alias = "Position";
                    info.type = wijmo.chart.Position;
                    info.choice = true;
                    info.required = true;
                    info.group = "Legend";
                    break;
                case "colors":
                    info.alias = "Palette";
                    info.type = wijmo.razor.Palettes;
                    info.choice = true;
                    info.required = true;
                    info.sorted = true;
                    info.group = "Presentation";
                    break;
                default:
                    e.cancel = true;
                    break;
            }
        }

        // implementation
        
        private _aggregate(groups: wijmo.collections.CollectionViewGroup[], items: any[]) {
            for (var i = 0; i < groups.length; i++) {
                var g = groups[i];
                var item = {};
                item[this.bindingX] = g.name;
                this.series.forEach(function (s:AggregateRadarSeries) {
                    item[s.binding] = g.getAggregate(s.aggregate, s.binding);
                });
                items.push(item);
            }
        }
    }

    export class ColumnChart extends wijmo.razor.FlexChart {

        constructor(element: any, options?) {
            super(element, options);
            this.chartType = wijmo.chart.ChartType.Column;
        }
    }

    export class BarChart extends wijmo.razor.FlexChart {

        constructor(element: any, options?) {
            super(element, options);
            this.chartType = wijmo.chart.ChartType.Bar;
        }
    }

    export class LineChart extends wijmo.razor.FlexChart {

        constructor(element: any, options?) {
            super(element, options);
            this.chartType = wijmo.chart.ChartType.Line;
        }
    }

    export class AreaChart extends wijmo.razor.FlexChart {

        constructor(element: any, options?) {
            super(element, options);
            this.chartType = wijmo.chart.ChartType.Area;
        }
    }

    export class ScatterChart extends wijmo.razor.FlexChart {

        constructor(element: any, options?) {
            super(element, options);
            this.chartType = wijmo.chart.ChartType.Scatter;
        }
    }

    export class SplineChart extends wijmo.razor.FlexChart {

        constructor(element: any, options?) {
            super(element, options);
            this.chartType = wijmo.chart.ChartType.Spline;
        }
    }

    export class BubbleChart extends wijmo.razor.FlexChart {

        constructor(element: any, options?) {
            super(element, options);
            this.chartType = wijmo.chart.ChartType.Bubble;
        }
    }
    
    export class PieChart extends wijmo.razor.FlexPie {

        constructor(element: any, options?) {
            super(element, options);
        }
    }

    export class RadarChart extends wijmo.razor.FlexRadar {

        constructor(element: any, options?) {
            super(element, options);
        }
    }

    export class TreeMap extends wijmo.chart.hierarchical.TreeMap {

        protected _colors: wijmo.razor.Palettes = wijmo.razor.Palettes.standard;
        protected _function: wijmo.Aggregate = wijmo.Aggregate.Sum;
 
        // ctor
        
        constructor(element: any, options?) {
            super(element, options);
            this.dataLabel.content = "{name}";
            this.hostElement.style.position = "absolute";
        }

        // object model

        get legendPosition(): wijmo.chart.Position {
            return this.legend.position;
        }

        set legendPosition(value: wijmo.chart.Position) {
            if (this.legend.position != value) {
                this.legend.position = value;
            }
        }

        get colors(): wijmo.razor.Palettes {
            return this._colors;
        }

        set colors(value: wijmo.razor.Palettes) {
            if (this._colors != value) {
                this._colors = value;
                this.palette = getChartPalette(value);
            }
        }

        // RuntimeControl interface
        
        get dataSourceType(): DataSourceType {
            return DataSourceType.SingleValueMultipleCategories;
        }
        
        defaults(): any {
            return {
                legendPosition: wijmo.chart.Position.None,
                colors: wijmo.razor.Palettes.standard
            }
        }

        apply(object: any) {
            this.colors = object.colors ? object.colors : wijmo.razor.Palettes.standard;
            this.legendPosition = object.legendPosition ? object.legendPosition : wijmo.chart.Position.None;
            RuntimeDefault.apply(this, object);
        }

        bind(object: any) {
            var self = this;
            if (object.categories.length) {
                //this.bindingName = object.categories[0][0];
                this.bindingName = object.categories.map(function (c) {
                    return c[0];
                });
            } else {
                this.bindingName = null;
            }
            if (object.values.length) {
                this.binding = object.values[0][0];
                this._function = object.values[0][2] ? <Aggregate>object.values[0][2] : Aggregate.Sum;
                if (this._function == Aggregate.None) {
                    this._function = Aggregate.Sum;
                }
            } else {
                this.binding = null;
            }
        }

        populate(object: any) {
            if (this.binding && this.bindingName) {
                //var items = [];
                //object.groupDescriptions.push(new wijmo.collections.PropertyGroupDescription(this.bindingName));
                //this._aggregate(object.groups, items);
                //this.itemsSource = items;
                this.bindingName.forEach(function (g) {
                    object.groupDescriptions.push(new wijmo.collections.PropertyGroupDescription(g));
                });
                this.itemsSource = object;
            } else {
                this.itemsSource = null;
            }
        }

        onProperty(e: PropertyInfoEventArgs) {
            var info = e.propertyInfo;
            switch (info.name) {
                case "legendPosition":
                    info.alias = "Position";
                    info.type = wijmo.chart.Position;
                    info.choice = true;
                    info.required = true;
                    info.group = "Legend";
                    break;
                case "colors":
                    info.alias = "Palette";
                    info.type = wijmo.razor.Palettes;
                    info.choice = true;
                    info.required = true;
                    info.sorted = true;
                    info.group = "Presentation";
                    break;
                default:
                    e.cancel = true;
                    break;
            }
        }

        // implementation
        
        private _aggregate(groups: wijmo.collections.CollectionViewGroup[], items: any[]) {
            var self = this;
            for (var i = 0; i < groups.length; i++) {
                var g = groups[i];
                var item = {};
                item[this.bindingName] = g.name;
                item[this.binding] = g.getAggregate(this._function, this.binding);
                items.push(item);
            }
        }
    }
}