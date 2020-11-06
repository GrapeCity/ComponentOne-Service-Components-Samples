/// <reference path="typings/wijmo/wijmo.d.ts" />
/// <reference path="typings/wijmo/wijmo.chart.d.ts" />
/// <reference path="typings/wijmo/wijmo.grid.d.ts" />
/// <reference path="typings/wijmo/wijmo.input.d.ts" />

module wijmo.razor.viewer {
    'use strict';

    export class DesignerWidget {

        private static _DATA_KEY = "kintone-Widget";
        protected _placeholder: HTMLElement;
        private _controller: ViewerController;
        private _control: Control;
        private _id: any;
        private _type: string;
        private _x: number;
        private _y: number;

        cssUnit(value: any) {
            return isNumber(value) ? value.toString() + "px" : value;
        }

        constructor(element: any, controller: ViewerController, options: any) {
            var self = this;
            var div = document.createElement("div");
            this._initialSize(div, options);

            div.addEventListener('mouseup', function (e) {
                controller.selectWidget(self);
                e.stopPropagation();
            });
           
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

        get controller(): ViewerController {
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

        protected _initialSize(div: HTMLElement, options: any) {
            div.style.position = "relative";
            div.style.height = this.cssUnit(options.height);
            div.style.width = this.cssUnit(options.width);
            div.style.left = this.cssUnit(options.left);
            div.style.top = this.cssUnit(options.top);
        }
    }

    export class DataSourceDesignerWidget extends DesignerWidget {

        private _dataSource: any;

        constructor(element: any, controller: ViewerController, options: any) {
            super(element, controller, options);
            this._dataSource = options.dataSource;
        }

        get dataSource(): any {
            return this._dataSource;
        }

        set dataSource(value: any) {
            if (this._dataSource != value) {
                this._dataSource = value;
                this.control.bind(value);
                this.populate(value);
            }
        }

        populate(value: any) {
            var self = this;
            var q = this.controller.getQuery(value.id);
            if (q) {
                var user = this.controller.getQueryVariable("user");
                var sort = this.controller.getQuerySortOrder(value.id);
                wijmo.httpRequest("/Viewer", {
                    data: {user: user, name: q.name, sort: sort},
                    success: function(xhr) {
                        var items, json = JSON.parse(xhr.response);
                        if (q.maxRows > 0) {
                            items = new wijmo.collections.CollectionView(json.slice(0, q.maxRows));
                        } else {
                            items = new wijmo.collections.CollectionView(json);
                        }
                        self.control.populate(items);
                    },
                    error: function(xhr) {
                        self.control.hostElement.innerText = "error";
                    }
                });
            }
        }
    }

    export class ViewSurface extends Control {

        private _controller: ViewerController;

        cssUnit(value: any) {
            return isNumber(value) ? value.toString() + "px" : value;
        }

        constructor(element: any, controller: ViewerController) {
            super(element);
            var self = this;
            var host = wijmo.getElement(element);

            host.addEventListener('mouseup', function (e) {
                controller.selectWidget(null);
                e.stopPropagation();
            });
           
            this._controller = controller;
        }

        get controller(): ViewerController {
            return this._controller;
        }
    }
}