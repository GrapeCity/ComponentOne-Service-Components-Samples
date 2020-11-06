/// <reference path="typings/wijmo/wijmo.d.ts" />
/// <reference path="typings/wijmo/wijmo.grid.d.ts" />
/// <reference path="typings/wijmo/wijmo.input.d.ts" />

module wijmo.razor.viewer {
    'use strict';

    export class ViewerController {

        private _zIndex = 1000;
        private _loadingPopup: wijmo.input.Popup;
        private _viewSurface: ViewSurface;
        private _schema = {};
        private _dashboard = {};
        private _initializing = false;
        private _selectedView: any = null;

        constructor() {
            this._viewSurface = new ViewSurface("#viewer-surface", this);
            this._loadingPopup = new wijmo.input.Popup("#loading-popup");
            this._loadingPopup.hideTrigger = wijmo.input.PopupTrigger.None;
            this.getUserProfile();
        }

        getQueryVariable(name) {
            var query = window.location.search.substring(1);
            var vars = query.split("&");
            for (var i = 0; i < vars.length; i++) {
                var pair = vars[i].split("=");
                if (pair[0] == name) {
                    return pair[1];
                }
            }
            return null;
        }

        getUserProfile() {
            var self = this;
            var id = this.getQueryVariable("user");
            wijmo.httpRequest("/UserProfile", {
                method: "GET",
                data: { user: id },
                requestHeaders: {
                    "RequestVerificationToken": (<HTMLInputElement>wijmo.getElement("[name='__RequestVerificationToken']")).value
                },
                success: function(xhr) {
                    var json = JSON.parse(xhr.response);
                    self.configure(json.Schema, json.Dashboard);
                },
                error: function(xhr) {
                    self.configure(null, null);
                },
                beforeSend: function(xhr) {
                    self._loadingPopup.show(true);
                }
            });
        }

        configure(schema, dashboard) {
            try {
                this._initializing = true;

                if (schema && schema.length > 2) {
                    this._schema = JSON.parse(schema);
                }
                if (dashboard && dashboard.length > 2) {
                    this._dashboard = JSON.parse(dashboard);
                }

                var id = this.getQueryVariable("id");
                this.selectView(Number(id));
            }
            finally {
                this._initializing = false;
                this._loadingPopup.hide();
            }
        }

        deserialize(options, selector) {
            var self = this;
            var offset = 9999;
            if (options.children) {
                Object.keys(options.children).forEach(function (c) {
                    var child = options.children[c];
                    var ctor = eval(child.widget.replace(".designer.", ".viewer."));
                    var div = document.createElement("div");
                    wijmo.getElement(selector).appendChild(div);
                    div.style.position = "absolute";
                    var widget = new ctor(div, self, child);
                    if (widget.control.apply) {
                        widget.control.apply(child);
                    }
                    if (child.dataSource) {
                        widget.populate(child.dataSource);
                    }
                    if (widget.left) {
                        var n = widget.left.indexOf("px");
                        if (n > 0) {
                            var left = Number(widget.left.slice(0, n));
                            if (left < offset) {
                                offset = left;
                            }
                        }
                    }
                });
                if (offset != 9999) {
                    wijmo.getElement(selector).style.marginLeft = (-offset).toString() + "px";
                }
            }
        }

        bringToFront(e: HTMLElement) {
            e.style.zIndex = this._zIndex.toString();
            this._zIndex++;
        }

        getQuery(id: any) {
            return this._schema[id];
        }

        getQuerySortOrder(id: any) {
            var sorts = [];
            var q = this.getQuery(id);
            if (q) {
                for (var i = 0; i < q.fields.length; i++) {
                    var f = q.fields[i];
                    if (f[0] != null && f[2]) {
                        var name = f[0];
                        var sign = f[2] > 0 ? "+" : "-";
                        if (f[1] != null) {
                            name = f[1];
                            if (i + 1 < q.fields.length) {
                                var g = q.fields[i + 1];
                                if (f[1] === g[1]) {
                                    name = null;
                                    i++;
                                }   
                            }
                        }
                        if (name) {
                            sorts.push(sign + name);
                        }
                    }
                }
            }
            return sorts.join(",");
        }

        selectWidget(widget: DesignerWidget) {
            if (widget) {
                this.bringToFront(widget.control.hostElement);
            }
        }

        findWidget(id: any, start?: any) {
            var self = this;
            var result = null;
            var widget = start ? start : this._selectedView;
            if (widget.id == id) {
                return widget;
            }
            if (widget.children) {
                Object.keys(widget.children).forEach(function (child) {
                    var w = self.findWidget(id, widget.children[child]);
                    if (w) {
                        result = w;
                    }
                });
            }
            return result;
        }

        selectView(id: any) {
            var v = this.getView(id);
            if (v) {
                wijmo.getElement("#view-label").innerText = v.name;
                this.deserialize(v, "#viewer-surface");
            }
        }

        getView(id: any) {
            return this._dashboard[id];
        }
 
        updateStyle(id: any, style: CSSStyleDeclaration) {
            if (this._initializing) {
                return;
            }
            var widget = this.findWidget(id);
            if (widget) {
                widget.left = style.left;
                widget.top = style.top;
                widget.width = style.width;
                widget.height = style.height;
            }
        }
    }
}
