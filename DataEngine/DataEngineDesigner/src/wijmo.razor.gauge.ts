/// <reference path="typings/wijmo/wijmo.d.ts" />
/// <reference path="typings/wijmo/wijmo.gauge.d.ts" />

module wijmo.razor {
    'use strict';

    export enum LabelAlignment {
        Left,
        Center,
        Right
    }

    export class LinearGauge extends wijmo.gauge.LinearGauge {

        private _binding;
        private _divLabel: HTMLDivElement;
        private _label: string;
        private _labelAlignment = LabelAlignment.Left;
        private _showLabel = true;
        private _aggregate = wijmo.Aggregate.None;

        // ctor/template

        constructor(element: any, options?) {
            super(element, options);
            this.hostElement.style.position = "absolute";
            this.showText = wijmo.gauge.ShowText.Value;
        }

        initialize(options: any) {
            this._divLabel = <HTMLDivElement>this.hostElement.querySelector("[wj-part='gauge-label']");
            super.initialize(options);
        }

        getTemplate(): string {
            return '<label><div wj-part="gauge-label" style="display:none"></div>' + super.getTemplate() + '</label>';
        }

        // object model

        get label(): string {
            return this._label;
        };

        set label(value: string) {
            if (this._label !== value) {
                this._label = value;
                this._divLabel.innerText = this._label;
                this._divLabel.style.display = (this._label && this._showLabel) ? "block" : "none";
            }
        }

        get labelAlignment(): LabelAlignment {
            return this._labelAlignment;
        };

        set labelAlignment(value: LabelAlignment) {
            if (this._labelAlignment !== value) {
                this._labelAlignment = value;
                this._divLabel.style.textAlign = ["left", "center", "right"][this._labelAlignment];
            }
        }

        get showLabel(): boolean {
            return this._showLabel;
        }

        set showLabel(value: boolean) {
            if (this._showLabel == value) {
                this._showLabel = value;
                this._divLabel.style.display = (this._label && this._showLabel) ? "block" : "none";
            }
        }

        // RuntimeControl interface

        get dataSourceType(): DataSourceType {
            return DataSourceType.SingleValueOnly;
        }

        defaults(): any {
            return {
                min: 0,
                max: 100,
                format: "",
                label: "",
                labelAlignment: LabelAlignment.Left,
                showLabel: true,
                showText: wijmo.gauge.ShowText.Value
            }
        }

        apply(object: any) {
			RuntimeDefault.apply(this, object);
            this.format = object.format;
            this.label = object.label;
            this.labelAlignment = object.labelAlignment ? object.labelAlignment : LabelAlignment.Left;
            this.showLabel = object.showLabel ? true : false;
            this.showText = object.showText ? object.showText : wijmo.gauge.ShowText.Value;
            this.min = object.min ? object.min : 0;
            this.max = object.max ? object.max : 100;
        }

        bind(object: any) {
            if (object.values.length > 0) {
                var v = object.values[0];
                this._binding = v[0];
                this.label = v[1];
                this._aggregate = v[2] ? <Aggregate>v[2] : Aggregate.None;
            }
        }

        populate(object: any) {
            try {
                if (this._aggregate !== wijmo.Aggregate.None) {
                    if (this._binding) {
                        this.value = object.getAggregate(this._aggregate, this._binding);
                    }
                } else {
                    object.moveCurrentToFirst();
                    if (this._binding && object.currentItem) {
                        this.value = object.currentItem[this._binding];
                    }
                }
			} catch {
                throw "Number required."
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
                case "max":
                    info.alias = "Maximum Value";
                    info.type = wijmo.DataType.Number;
                    info.group = "Presentation";
                    break;
                case "min":
                    info.alias = "Minimum Value";
                    info.type = wijmo.DataType.Number;
                    info.group = "Presentation";
                    break;
                case "label":
                    info.alias = "Label";
                    info.type = wijmo.DataType.String;
                    info.group = "Text";
                    break;
                case "labelAlignment":
                    info.alias = "Label Alignment";
                    info.type = LabelAlignment;
                    info.choice = true;
                    info.required = true;
                    info.group = "Text";
                    break;
                case "showLabel":
                    info.alias = "Show Label";
                    info.type = wijmo.DataType.Boolean;
                    info.group = "Text";
                    break;
                case "showText":
                    info.alias = "Show Text";
                    info.type = wijmo.gauge.ShowText;
                    info.choice = true;
                    info.required = true;
                    info.group = "Text";
                    break;
                default:
                    e.cancel = true;
                    break;
            }
        }
    }
    
    export class RadialGauge extends wijmo.gauge.RadialGauge {

        private _binding;
        private _divLabel: HTMLDivElement;
        private _label: string;
        private _labelAlignment = LabelAlignment.Left;
        private _showLabel = true;
        private _aggregate = wijmo.Aggregate.None;

        // ctor/template

        constructor(element: any, options?) {
            super(element, options);
            this.hostElement.style.position = "absolute";
            this.showText = wijmo.gauge.ShowText.Value;
        }

        initialize(options: any) {
            this._divLabel = <HTMLDivElement>this.hostElement.querySelector("[wj-part='gauge-label']");
            super.initialize(options);
        }

        getTemplate(): string {
            return '<label><div wj-part="gauge-label" style="display:none"></div>' + super.getTemplate() + '</label>';
        }

        // object model

        get label(): string {
            return this._label;
        };

        set label(value: string) {
            if (this._label !== value) {
                this._label = value;
                this._divLabel.innerText = this._label;
                this._divLabel.style.display = (this._label && this._showLabel) ? "block" : "none";
            }
        }

        get labelAlignment(): LabelAlignment {
            return this._labelAlignment;
        };

        set labelAlignment(value: LabelAlignment) {
            if (this._labelAlignment !== value) {
                this._labelAlignment = value;
                this._divLabel.style.textAlign = ["left", "center", "right"][this._labelAlignment];
            }
        }

        get showLabel(): boolean {
            return this._showLabel;
        }

        set showLabel(value: boolean) {
            if (this._showLabel == value) {
                this._showLabel = value;
                this._divLabel.style.display = (this._label && this._showLabel) ? "block" : "none";
            }
        }

        // RuntimeControl interface

        get dataSourceType(): DataSourceType {
            return DataSourceType.SingleValueOnly;
        }

        defaults(): any {
            return {
                min: 0,
                max: 100,
                format: "",
                label: "",
                labelAlignment: LabelAlignment.Left,
                showLabel: true,
                showText: wijmo.gauge.ShowText.Value
            }
        }

        apply(object: any) {
			RuntimeDefault.apply(this, object);
            this.format = object.format;
            this.label = object.label;
            this.labelAlignment = object.labelAlignment ? object.labelAlignment : LabelAlignment.Left;
            this.showLabel = object.showLabel ? true : false;
            this.showText = object.showText ? object.showText : wijmo.gauge.ShowText.Value;
            this.min = object.min ? object.min : 0;
            this.max = object.max ? object.max : 100;
        }

        bind(object: any) {
            if (object.values.length > 0) {
                var v = object.values[0];
                this._binding = v[0];
                this.label = v[1];
                this._aggregate = v[2] ? <Aggregate>v[2] : Aggregate.None;
            }
        }

        populate(object: any) {
            try {
                if (this._aggregate !== wijmo.Aggregate.None) {
                    if (this._binding) {
                        this.value = object.getAggregate(this._aggregate, this._binding);
                    }
                } else {
                    object.moveCurrentToFirst();
                    if (this._binding && object.currentItem) {
                        this.value = object.currentItem[this._binding];
                    }
                }
			} catch {
				throw "Number required."
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
                case "max":
                    info.alias = "Maximum Value";
                    info.type = wijmo.DataType.Number;
                    info.group = "Presentation";
                    break;
                case "min":
                    info.alias = "Minimum Value";
                    info.type = wijmo.DataType.Number;
                    info.group = "Presentation";
                    break;
                case "label":
                    info.alias = "Label";
                    info.type = wijmo.DataType.String;
                    info.group = "Text";
                    break;
                case "labelAlignment":
                    info.alias = "Label Alignment";
                    info.type = LabelAlignment;
                    info.choice = true;
                    info.required = true;
                    info.group = "Text";
                    break;
                case "showLabel":
                    info.alias = "Show Label";
                    info.type = wijmo.DataType.Boolean;
                    info.group = "Text";
                    break;
                case "showText":
                    info.alias = "Show Text";
                    info.type = wijmo.gauge.ShowText;
                    info.choice = true;
                    info.required = true;
                    info.group = "Text";
                    break;
                default:
                    e.cancel = true;
                    break;
            }
        }
    }
}