/// <reference path="typings/wijmo/wijmo.d.ts" />

module wijmo.razor {
    'use strict';

    export class Numeric extends wijmo.Control {

        private _binding;
        private _format: string;
        private _value: number;
        private _placeholder: HTMLElement;
        private _alignment = LabelAlignment.Left;
        private _aggregate = wijmo.Aggregate.None;

        // ctor

        constructor(element: any, options?) {
            super(element, options);
            this._placeholder = wijmo.createElement("<div>", element);
            this._placeholder.className = "static-label";
            this.hostElement.style.position = "absolute";
        }

        // object model

        get alignment(): LabelAlignment {
            return this._alignment;
        };

        set alignment(value: LabelAlignment) {
            if (this._alignment !== value) {
                this._alignment = value;
                this._placeholder.style.textAlign = ["left", "center", "right"][this._alignment];
            }
        }

        get format(): string {
            return this._format;
        }

        set format(value: string) {
            if (this._format != value) {
                this._format = value;
                this._refresh();
            }
        }

        _refresh() {
            if (this._value === undefined || this._value === null) {
                this._placeholder.innerText = "";
            } else if (this._format && this._format.length > 0) {
                this._placeholder.innerText = wijmo.Globalize.format(this._value, this._format);
            } else {
                this._placeholder.innerText = this._value.toString();
            }
        }

        // RuntimeControl interface

        get dataSourceType(): DataSourceType {
            return DataSourceType.SingleValueOnly;
        }

        defaults(): any {
            return {
                format: "",
                alignment: LabelAlignment.Left
            }
        }

        apply(object: any) {
            this.format = object.format;
            this.alignment = object.alignment ? object.alignment : LabelAlignment.Left;
            RuntimeDefault.apply(this, object);
        }

        bind(object: any) {
            if (object.values.length > 0) {
                var v = object.values[0];
                this._binding = v[0];
                this._aggregate = v[2] ? <Aggregate>v[2] : Aggregate.None;
            }
        }

        populate(object: any) {
            if (this._aggregate !== wijmo.Aggregate.None) {
                if (this._binding) {
                    this._value = object.getAggregate(this._aggregate, this._binding);
                    this._refresh();
                }
            } else {
                object.moveCurrentToFirst();
                if (this._binding && object.currentItem) {
                    this._value = object.currentItem[this._binding];
                    this._refresh();
                }
            }
        }

        onProperty(e: PropertyInfoEventArgs) {
            var info = e.propertyInfo;
            switch (info.name) {
                case "format":
                    info.alias = "Format";
                    info.type = wijmo.DataType.String;
                    info.group = "Presentation";
                    break;
                case "alignment":
                    info.alias = "Alignment";
                    info.type = LabelAlignment;
                    info.choice = true;
                    info.required = true;
                    info.group = "Presentation";
                    break;
                default:
                    e.cancel = true;
                    break;
            }
        }
    }

    export class Label extends wijmo.Control {

        private _text: string;
        private _placeholder: HTMLElement;
        private _alignment = LabelAlignment.Left;

        // ctor

        constructor(element: any, options?) {
            super(element, options);
            this._placeholder = wijmo.createElement("<div>", element);
            this._placeholder.className = "static-label";
            this.hostElement.style.position = "absolute";
        }

        // object model

        get alignment(): LabelAlignment {
            return this._alignment;
        };

        set alignment(value: LabelAlignment) {
            if (this._alignment !== value) {
                this._alignment = value;
                this._placeholder.style.textAlign = ["left", "center", "right"][this._alignment];
            }
        }

        get text(): string {
            return this._text;
        }

        set text(value: string) {
            if (this._text != value) {
                this._text = value;
                this._placeholder.innerText = this._text;
            }
        }

        // RuntimeControl interface

        get dataSourceType(): DataSourceType {
            return DataSourceType.None;
        }

        defaults(): any {
            return {
                text: "",
                alignment: LabelAlignment.Left
            }
        }

        apply(object: any) {
            this.text = object.text;
            this.alignment = object.alignment ? object.alignment : LabelAlignment.Left;
            RuntimeDefault.apply(this, object);
        }

        bind(object: any) {
        }

        populate(object: any) {
        }

        onProperty(e: PropertyInfoEventArgs) {
            var info = e.propertyInfo;
            switch (info.name) {
                case "text":
                    info.alias = "Text";
                    info.type = wijmo.DataType.String;
                    info.group = "Presentation";
                    break;
                case "alignment":
                    info.alias = "Alignment";
                    info.type = LabelAlignment;
                    info.choice = true;
                    info.required = true;
                    info.group = "Presentation";
                    break;
                default:
                    e.cancel = true;
                    break;
            }
        }
    }
}