/// <reference path="typings/wijmo/wijmo.d.ts" />

module wijmo.razor {
    'use strict';

    export enum DataSourceType {
        None,
        MultipleValuesAndCategories,    // standard chart
        MultipleValuesSingleCategory,   // radar chart
        SingleValueAndCategory,         // pie chart
        SingleValueOnly,                // gauge
        SingleValueMultipleCategories,  // tree map
        PivotEngine                     // pivot control
    }

    export function cssUnit(value) {
        return wijmo.isNumber(value) ? value.toString() + "px" : value;
    }

    export interface RuntimeControl extends wijmo.Control {
        dataSourceType: DataSourceType;
        defaults(): any;
        apply(object: any): void;
        bind(object: any): void;
        populate(object: any): void;
        onProperty(e: PropertyInfoEventArgs): void;
    }

    export class RuntimeDefault {

        static apply(self: RuntimeControl, object: any) {
            if (object.width) {
                self.hostElement.style.width = cssUnit(object.width);
            }
            if (object.height) {
                self.hostElement.style.height = cssUnit(object.height);
            }
            if (object.left) {
                self.hostElement.style.left = cssUnit(object.left);
            }
            if (object.top) {
                self.hostElement.style.top = cssUnit(object.top);
            }
            if (object.dataSource) {
                self.bind(object.dataSource);
            }
            self.invalidate(false);
        }
    }
}