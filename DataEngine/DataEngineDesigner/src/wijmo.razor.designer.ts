/// <reference path="typings/wijmo/wijmo.d.ts" />
/// <reference path="typings/wijmo/wijmo.grid.d.ts" />
/// <reference path="typings/wijmo/wijmo.input.d.ts" />

module wijmo.razor.designer {
    'use strict';

    export class DesignerController {

        private _zIndex = 1000;
        private _baseTables: any;
        private _apps: AvailableApps;
        private _queries: QueryDefinitions;
        private _dataSurface: DataSurface;
        private _queryGrid: QueryDesignGrid;
        private _queryMenu: QueryContextMenu;
        private _tableMenu: DataTableContextMenu;
        private _joinMenu: TableJoinContextMenu;
        private _toolbox: ControlToolbox;
        private _properties: PropertyGrid;
        private _widgetMenu: wijmo.input.Menu;
        private _dataProperties: PropertyGrid;
        private _renamePopup: wijmo.input.Popup;
        private _queryPopup: wijmo.input.Popup;
        private _viewPopup: wijmo.input.Popup;
        private _resetPopup: wijmo.input.Popup;
        private _successPopup: wijmo.input.Popup;
        private _failurePopup: wijmo.input.Popup;
        private _failurePopupMsg: HTMLElement;
        private _loadingPopup: wijmo.input.Popup;
        private _designSurface: DesignSurface;
        private _dataSourcePanel: DataSourcePanel;
        private _valueMenu: wijmo.input.Menu;
        private _currentView = "data";
        private _dragObject: any;
        private _dragOrigin = new wijmo.Point();
        private _schema = {};
        private _dashboard = {};
        private _dirty = false;
        private _initializing = false;
        private _dirtyQueries = [];
        private _selectedQuery: any = null;
        private _selectedTable: any = null;
        private _selectedObject: wijmo.razor.designer.DesignerWidget;
        private _dirtyViews = [];
        private _selectedView: any = null;
        private _views: ViewDefinitions;
        private _viewMenu: ViewContextMenu;

        constructor(tables: any) {

            var self = this;
            this._baseTables = tables;
            this._apps = new AvailableApps("#available-apps", this);
            this._queries = new QueryDefinitions("#query-definitions", this);
            this._dataSurface = new DataSurface("#selected-apps", this);
            this._queryGrid = new QueryDesignGrid("#query-grid", this);
            this._queryMenu = new QueryContextMenu("#query-menu", this);
            this._tableMenu = new DataTableContextMenu("#data-table-menu", this);
            this._joinMenu = new TableJoinContextMenu("#table-join-menu", this);
            this._toolbox = new ControlToolbox(".control-gallery", this);
            this._designSurface = new DesignSurface("#design-surface", this);
            this._properties = new PropertyGrid("#property-grid");
            this._dataProperties = new PropertyGrid("#data-properties");
            this._widgetMenu = new DesignerWidgetContextMenu("#ui-widget-menu", this);
            this._dataSourcePanel = new DataSourcePanel(".datasource-panel", this);
            this._valueMenu = new DataValueContextMenu("#data-value-menu", this._dataSourcePanel);
            this._views = new ViewDefinitions("#view-definitions", this);
            this._viewMenu = new ViewContextMenu("#view-menu", this);

            this._dataSourcePanel.dataSourceChanged.addHandler(function (e) {
                if (self.selectedObject) {
                    var data = self._dataSourcePanel.dataSource;
                    self.selectedObject["dataSource"] = data;
                }
            });

            this._renamePopup = new wijmo.input.Popup("#rename-popup");
            this._renamePopup.dialogResultEnter = "wj-hide-ok";

            this._queryPopup = new wijmo.input.Popup("#query-popup");
            this._queryPopup.dialogResultEnter = "wj-hide-ok";

            this._viewPopup = new wijmo.input.Popup("#view-popup");
            this._viewPopup.dialogResultEnter = "wj-hide-ok";

            this._resetPopup = new wijmo.input.Popup("#reset-popup");
            this._resetPopup.dialogResultEnter = "wj-hide-ok";

            this._successPopup = new wijmo.input.Popup("#success-popup");
            this._successPopup.dialogResultEnter = "wj-hide-ok";

            this._failurePopup = new wijmo.input.Popup("#failure-popup");
            this._failurePopup.dialogResultEnter = "wj-hide-cancel";
            this._failurePopupMsg = wijmo.getElement("#failure-popup-msg");

            this._loadingPopup = new wijmo.input.Popup("#loading-popup");
            this._loadingPopup.hideTrigger = wijmo.input.PopupTrigger.None;

            var tabDataSources = wijmo.getElement("#tab-data-sources");
            if (tabDataSources) {
                tabDataSources.addEventListener("click", function(e) {
                    wijmo.Control.invalidateAll();
                });
            }

            var tabDesigner = wijmo.getElement("#tab-ui-designer");
            if (tabDesigner) {
                tabDesigner.addEventListener("click", function(e) {
                    self.repopulate();
                    wijmo.Control.invalidateAll();
                });
            }

            var queryMenuLabel = wijmo.getElement("#query-menu-label")
            if (queryMenuLabel) {
                queryMenuLabel.addEventListener("click", function(e) {
                    self._queryMenu.show(e.target);
                });
            }

            var viewMenuLabel = wijmo.getElement("#view-menu-label")
            if (viewMenuLabel) {
                viewMenuLabel.addEventListener("click", function(e) {
                    self._viewMenu.show(e.target);
                });
            }

            this._properties.headersVisibility = wijmo.grid.HeadersVisibility.None;
            this._properties.isGrouped = true;
            this._properties.rows.defaultSize = 24;
            this._properties.selectionChanging.addHandler(function (s, e:wijmo.grid.CellRangeEventArgs) {
                if (e.col !== 1 || self._properties.rows[e.row] instanceof wijmo.grid.GroupRow) {
                    e.cancel = true;
                }
            });

            this._properties.addingProperty.addHandler(function (s, e:PropertyInfoEventArgs) {
                if (self.selectedObject) {
                    self.selectedObject.control.onProperty(e);
                } else if (e.propertyInfo.name === "name") {
                    e.propertyInfo.alias = "Name";
                    e.propertyInfo.group = "View";
                }
            });

            this._properties.propertyChanged.addHandler(function (s, e:wijmo.PropertyChangedEventArgs) {
                if (self.selectedObject) {
                    self.setWidgetProperty(self.selectedObject.id, e.propertyName, e.newValue);
                } else if (e.propertyName === "name") {
                    var v = self._properties.selectedObject;
                    self._views.collectionView.currentItem.value = v.name;
                    self._views.collectionView.refresh();
                    wijmo.getElement("#view-label").innerText = v.name;
                    self.setViewDirty();
                }
            });

            this._dataProperties.headersVisibility = wijmo.grid.HeadersVisibility.None;
            this._dataProperties.isGrouped = true;
            this._dataProperties.rows.defaultSize = 24;
            this._dataProperties.selectionChanging.addHandler(function (s, e:wijmo.grid.CellRangeEventArgs) {
                if (e.col !== 1 || self._dataProperties.rows[e.row] instanceof wijmo.grid.GroupRow) {
                    e.cancel = true;
                }
            });
            
            this._dataProperties.addingProperty.addHandler(function (s, e:PropertyInfoEventArgs) {
                var info = e.propertyInfo;
                if (info.name === "name") {
                    info.alias = "Name";
                    info.group = "Data Source";
                } else if (info.name === "maxRows") {
                    info.alias = "Row Limit";
                    info.group = "Data Source";
                    info.type = wijmo.DataType.Number;
                    info.required = false;
                    info.min = 0;
                    info.spin = false;
                } else if (info.name === "joinType") {
                    info.alias = "Join Type";
                    info.group = "Join";
                    info.required = true;
                    info.choice = true;
                } else if (info.name === "fromField") {
                    info.alias = "From Field";
                    info.group = "Join";
                    info.type = self._dataProperties.selectedObject.getFromFields(self);
                    info.required = true;
                    info.choice = true;
                    info["object"] = true;
                } else if (info.name === "toField") {
                    info.alias = "To Field";
                    info.group = "Join";
                    info.type = self._dataProperties.selectedObject.getToFields(self);
                    info.required = true;
                    info.choice = true;
                    info["object"] = true;
                } else {
                    e.cancel = true;
                }
            });

            this._dataProperties.propertyChanged.addHandler(function (s, e:wijmo.PropertyChangedEventArgs) {
                if (e.propertyName === "name") {
                    var q = self._dataProperties.selectedObject;
                    self._queries.collectionView.currentItem.value = q.name;
                    self._queries.collectionView.refresh();
                    wijmo.getElement("#query-label").innerText = q.name;
                    self.setQueryDirty();
                } else {
                    self.setQueryDirty();
                }
            });

            this.getUserState();
        }

        get baseTables(): string[] {
            return Object.keys(this._baseTables);
        }

        get dataSurface(): DataSurface {
            return this._dataSurface;
        }

        getUserState() {
            var schema = window.localStorage.getItem("schema");
            var dashboard = window.localStorage.getItem("dashboard");
            this.configure(schema, dashboard);
        }

        configure(schema, dashboard) {
            try {
                this._initializing = true;
                this.createRootWidget();

                if (schema && schema.length > 2) {
                    this._schema = JSON.parse(schema);
                }
                if (dashboard && dashboard.length > 2) {
                    this._dashboard = JSON.parse(dashboard);
                }

                this._queries.itemsSource = this.getDataSourceItems();
                this._views.itemsSource = this.getDashboardViews();
                this._dataSourcePanel.refresh();
                this.selectWidget(null);
                wijmo.Control.refreshAll();
            }
            finally {
                this._initializing = false;
            }
        }

        deserialize(options, selector) {
            var self = this;
            if (options.children) {
                Object.keys(options.children).forEach(function (c) {
                    var child = options.children[c];
                    var ctor = eval(child.widget);
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
                });
            }
        }

        repopulate() {
            var self = this;
            var dirty = this._dirtyQueries.length > 0;
            var options: any = this.getSelectedView();
            if (dirty && options.children) {
                Object.keys(options.children).forEach(function (c) {
                    var child = options.children[c];
                    if (child.dataSource && self.isQueryDirty(child.dataSource.id)) {
                        var id = "[ctrlid='" + child.id + "']";
                        var widget = <DataSourceDesignerWidget>DesignerWidget.getWidget(id);
                        widget.populate(child.dataSource);
                    }
                });
                self._dirtyQueries = [];
            }
        }

        removeChildren(e: HTMLElement) {
            while (e.hasChildNodes()) {
                e.removeChild(e.lastChild);
            }
        }

        clear() {
            this.removeChildren(this._dataSurface.hostElement);
            this.removeChildren(this._designSurface.hostElement);
            this._queries.itemsSource = null;
            this._schema = {};
            this._dashboard = {};
            this._dirtyQueries = [];
            this._dirtyViews = [];
            this._dirty = false;
        }

        setQueryDirty() {
            var id = this._selectedQuery;
            if (id && !this.isQueryDirty(id)) {
                this._dirtyQueries.push(id);
            }
            this.setDirty(true);
        }

        isQueryDirty(id: any): boolean {
            return !this._dirtyQueries.every(function (q) {
                return q !== id;
            })
        }

        selectQuery(id: any) {
            if (this._selectedQuery !== id) {
                this._selectedQuery = id;
                this.removeChildren(this._dataSurface.hostElement);

                if (id) {
                    var q = this.getQuery(id);
                    wijmo.getElement("#query-label").innerText = q.name;
                    Object.keys(q.tables).map(function (key) {
                        new wijmo.razor.designer.DataTable(this._dataSurface.hostElement, this, q.tables[key]);
                    }, this);
                    this._dataSurface.drawLines();
                    this._dataProperties.selectedObject = new QueryProperties(this);
                    this._queryGrid.apply(q);
                } else {
                    wijmo.getElement("#query-label").innerText = "";
                    this._dataProperties.selectedObject = null;
                    this._queryGrid.apply(null);
                    this._queries.selectedValue = null;
                }
            }
        }

        selectQueryProperties() {
            var q = this.getSelectedQuery();
            this._dataProperties.selectedObject = q ? new QueryProperties(this) : null;
        }

        isSelectedQuery(id: any) {
            return id === this._selectedQuery;
        }

        getQuery(id: any) {
            return this._schema[id];
        }

        getSelectedQuery() {
            return this._schema[this._selectedQuery];
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

        createQuery(name) {
            var id = new Date().getTime();
            this._schema[id] = {
                id: id,
                name: name,
                tables: {},
                joins: {},
                fields: []
            }
            this._queries.itemsSource = this.getDataSourceItems();
            this._queries.selectedValue = id;
            this.setDirty(true);
        }

        deleteQuery() {
            var q = this.getSelectedQuery();
            if (q) {
                var self = this;
                this._queryPopup.show(true, function(popup: wijmo.input.Popup) {
                    if (popup.dialogResult === "wj-hide-ok") {
                        var json = {
                            name: q.name
                        };
                        delete self._schema[self._selectedQuery];
                        self._queries.itemsSource = self.getDataSourceItems();
                        self.setDirty(true);
                        wijmo.httpRequest("/QueryData", {
                            method: "DELETE",
                            data: json,
                            requestHeaders: {
                                "RequestVerificationToken": (<HTMLInputElement>wijmo.getElement("[name='__RequestVerificationToken']")).value
                            },
                            success: function(xhr) {
                                self._loadingPopup.hide();
                                self._successPopup.show(true);
                            },
                            error: function(xhr) {
                                self._loadingPopup.hide();
                                self.showError(xhr.responseText);
                            },
                            beforeSend: function(xhr) {
                                self._loadingPopup.show(true);
                            }
                        });
                    }
                });
            }
        }

        executeQuery() {
            var q = this.getSelectedQuery();
            if (q) {
                var self = this;
                var json = {
                    name: q.name,
                    tables: [],
                    columns: [],
                    range: [],
                    joins: []
                };
                Object.keys(q.tables).forEach(function (key) {
                    json.tables.push(q.tables[key].name);
                });
                Object.keys(q.joins).forEach(function (key) {
                    var join = q.joins[key];
                    json.joins.push({
                        from: q.tables[join.fromId].name + "." + join.fromField,
                        to: q.tables[join.toId].name + "." + join.toField
                    });
                });
                for (var i = 0; i < q.fields.length; i++) {
                    var f = q.fields[i];
                    if (f[0] != null && f[0].length) {
                        if (f[1] != null && f[1].length) {
                            if (i + 1 < q.fields.length) {
                                var g = q.fields[i + 1];
                                if (f[1] === g[1]) {
                                    if (g[0] == null) {
                                        self.showError("Second field required for binary operation.");
                                        return;
                                    }
                                    if (f[3] == null) {
                                        self.showError("Binary operator required.");
                                        return;
                                    }
                                    json.columns.push({
                                        names: [f[0], g[0]],
                                        op: BinaryOperation[f[3]],
                                        alias: f[1]
                                    });
                                    i++;
                                    continue;
                                }
                            }
                        }
                        json.columns.push({
                            names: [f[0]],
                            op: f[3] != null ? GroupOperation[f[3]] : null,
                            alias: f[1]
                        });
                        if (f[4] != null && f[5] != null && f[5].length) {
                            json.range.push({
                                name: f[0],
                                expr: [{
                                    op: RangeCondition[f[4]],
                                    value: f[5]
                                }]
                            });
                        }
                    }
                }
                if (json.columns.length == 0) {
                    self.showError("Query does not define any columns.");
                    return;
                }
                if (json.tables.length > 1 && json.joins.length < json.tables.length - 1) {
                    self.showError("Secondary table must be joined to a primary table.");
                    return;
                }
                wijmo.httpRequest("/QueryData", {
                    method: "POST",
                    data: json,
                    requestHeaders: {
                        "RequestVerificationToken": (<HTMLInputElement>wijmo.getElement("[name='__RequestVerificationToken']")).value
                    },
                    success: function(xhr) {
                        self._loadingPopup.hide();
                        self._successPopup.show(true);
                    },
                    error: function(xhr) {
                        self._loadingPopup.hide();
                        self.showError(xhr.responseText);
                    },
                    beforeSend: function(xhr) {
                        self._loadingPopup.show(true);
                    }
                });
            }
        }

        saveUserState() {
            window.localStorage.setItem("schema", JSON.stringify(this._schema));
            window.localStorage.setItem("dashboard", JSON.stringify(this._dashboard));
        }

        resetWorkspace() {
            var self = this;
            this._resetPopup.show(true, function(popup: wijmo.input.Popup) {
                if (popup.dialogResult === "wj-hide-ok") {
                    var json = {
                        name: "*"
                    };
                    wijmo.httpRequest("/QueryData", {
                        method: "DELETE",
                        data: json,
                        requestHeaders: {
                            "RequestVerificationToken": (<HTMLInputElement>wijmo.getElement("[name='__RequestVerificationToken']")).value
                        },
                        success: function(xhr) {
                            window.localStorage.clear();
                            self.clear();
                            self.configure(null, null);
                            self._loadingPopup.hide();
                            self._successPopup.show(true);
                        },
                        error: function(xhr) {
                            self._loadingPopup.hide();
                            self.showError(xhr.responseText);
                        },
                        beforeSend: function(xhr) {
                            self._loadingPopup.show(true);
                        }
                    });
                }
            });
        }

        ensureQuery(name: string) {
            var q = this.getSelectedQuery();
            if (!q) {
                this.createQuery(this.generateQueryName());
            }
        }

        generateQueryName() {
            var n = 1, q = "Query1", names = [];
            Object.keys(this._schema).map(function (id) {
                names.push(this._schema[id].name);
            }, this);
            while (names.indexOf(q) >= 0) {
                n++;
                q = "Query" + n.toString();
            }
            return q;
        }

        selectJoinProperties(id: any) {
            if (id) {
                var q = this.getSelectedQuery();
                var j = q.joins[id];
                if (j) {
                    this._dataProperties.selectedObject = new JoinProperties(j);
                }
            } else {
                this._dataProperties.selectedObject = null;
            }
        }

        createJoin(options: any) {
            var q = this.getSelectedQuery();
            q.joins[options.id] = options;
            this.setQueryDirty();
        }

        deleteJoin(id: any) {
            var q = this.getSelectedQuery();
            delete q.joins[id];
            this.selectQueryProperties();
            this.setQueryDirty();
        }

        getBaseTableFields(name: string): string[] {
            var fields = [];
            this._baseTables[name].forEach(function (f) {
                fields.push({ "name": f, "on": true });
            });
            return fields;
        }

        createTable(options: any) {
            this.ensureQuery(options.name);
            this.getSelectedQuery().tables[options.id] = {
                id: options.id,
                x: options.x,
                y: options.y,
                name: options.name,
                fields: this.getBaseTableFields(options.name)
            };
            this.setQueryDirty();
        }

        moveTable(id: any, x: number, y: number) {
            this.getTable(id).x = x;
            this.getTable(id).y = y;
            this.setDirty();
        }

        deleteTable(id: any) {
            var unjoin = [];
            var q = this.getSelectedQuery();
            var n = Number(id);
            Object.keys(q.joins).map(function (key) {
                var j = q.joins[key];
                if (j.fromId === n || j.toId === n) {
                    unjoin.push(key);
                }
            });
            unjoin.map(function (key) {
                delete q.joins[key];
            });
            var name = q.tables[id].name;
            q.fields.forEach(function (f) {
                if (f[0]) {
                    var names = f[0].split(".");
                    if (names[0] === name) {
                        f[0] = null;
                    }
                }
            });
            delete q.tables[id];
            this.selectTable(null);
            this.setQueryDirty();
            this._queryGrid.apply(q);
        }

        selectTable(id: any) {
            if (this._selectedTable !== id) {
                this._selectedTable = id;
                wijmo.Control.invalidateAll(wijmo.getElement("#selected-apps"));
            }
        }

        isSelectedTable(id: any) {
            return id === this._selectedTable;
        }

        hasTable(name: string) {
            var result = false;
            var query = this.getSelectedQuery();
            if (query) {
                Object.keys(query.tables).forEach(function (key) {
                    var table = query.tables[key];
                    if (table.name === name) {
                        result = true;
                    }
                });
            }
            return result;
        }

        getTable(id: any) {
            var query = this.getSelectedQuery();
            return query ? query.tables[id] : null;
        }

        getTableFields(id: any) {
            assert(this.getTable(id), "getTable(id)");
            return this.getTable(id).fields;
        }

        applyFields(id: any, fields: any) {
            assert(this.getTable(id), "getTable(id)");
            this.getTable(id).fields = fields;
            this.setQueryDirty();
        }

        selectFields(id: any, on: boolean) {
            assert(this.getTable(id), "getTable(id)");
            this.getTable(id).fields.forEach(function (f) {
                f.on = on;
            });
            this.setQueryDirty();
        }

        selectField(id: any, index: number, on: boolean) {
            assert(this.getTable(id), "getTable(id)");
            var fields = this.getTable(id).fields;
            if (index < fields.length) {
                fields[index].on = on;
            }
            this.setQueryDirty();
        }

        isSelectedField(id: any, index: number): boolean {
            assert(this.getTable(id), "getTable(id)");
            var fields = this.getTable(id).fields;
            return index < fields.length ? fields[index].on : true;
        }

        bringToFront(e: HTMLElement) {
            e.style.zIndex = this._zIndex.toString();
            this._zIndex++;
        }

        startDrag(data: any, e: DragEvent) {
            this._dragObject = data;
            this._dragOrigin = new wijmo.Point(e.offsetX, e.offsetY);
        }

        endDrag() {
            this._dragObject = null;
            this._dragOrigin = new wijmo.Point();
        }

        getDragObject() {
            return this._dragObject;
        }

        getDragOrigin() {
            return this._dragOrigin;
        }

        get selectedObject(): wijmo.razor.designer.DesignerWidget {
            return this._selectedObject;
        }

        selectWidget(widget: wijmo.razor.designer.DesignerWidget) {
            if (this._selectedObject !== widget) {
                if (this._selectedObject) {
                    wijmo.removeClass(this._selectedObject.control.hostElement, "widget-selected");
                }
                this._selectedObject = widget;
                if (this._selectedObject) {
                    wijmo.addClass(this._selectedObject.control.hostElement, "widget-selected");
                    this.bringToFront(this._selectedObject.control.hostElement);
                    this._dataSourcePanel.dataSource = this._selectedObject["dataSource"];
                    this._dataSourcePanel.dataSourceType = this._selectedObject.control.dataSourceType;
                    this._properties.selectedObject = widget.control;
                } else {
                    this._dataSourcePanel.dataSource = null;
                    this._dataSourcePanel.dataSourceType = DataSourceType.None;
                    this._properties.selectedObject = null;
                }
            }
        }

        getDataSourceItems() {
            var items = [];
            if (this._schema) {
                Object.keys(this._schema).map(function (id) {
                    items.push({
                        key: Number(id),
                        value: this._schema[id].name
                    });
                }, this);
            }
            return items;
        }

        getDataSourceFields(id: any, all?: boolean) {
            var fields = {};
            var query = id ? this.getQuery(id) : null;
            if (query) {
                query.fields.forEach(function (f) {
                    if (f[1]) {
                        if (f[1].substr(0, 1) !== "_") {
                            fields[f[1]] = f[1];
                        }
                    } else if (f[0]) {
                        var names = f[0].split(".");
                        fields[names[1]] = names[1];
                    }
                });
            }
            return fields;
        }

        findWidget(id: any, start?: any) {
            var self = this;
            var result = null;
            var widget = start ? start : this.getSelectedView();
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

        createRootWidget() {
            this._dashboard = {
                1: {
                    id: 1,
                    name: "Dashboard",
                    children: {}
                }
            }
        }

        createWidget(options: any) {
            var widget = this.findWidget(options.parent);
            if (widget) {
                widget.children[options.id] = {
                    id: options.id,
                    parent: options.parent,
                    ctor: options.ctor,
                    widget: options.widget,
                    left: options.left,
                    top: options.top,
                    width: options.width,
                    height: options.height,
                    children: {}
                };
                this.setDirty();
            };
        }

        deleteWidget(id: any) {
            var widget = this.findWidget(id);
            if (widget) {
                var parent = this.findWidget(widget.parent);
                if (parent) {
                    delete parent.children[id];
                    this.setDirty();
                }
            }
        }

        moveWidget(id: any, parentId: any, beforeId: any) {
            var widget = this.findWidget(id);
            var parent = this.findWidget(parentId);
            if (widget && parent) {
                this.deleteWidget(id);
                widget.parent = Number(parentId);
                if (beforeId) {
                    var children = {};
                    var orphans = parent.children;
                    Object.keys(orphans).forEach(function (key) {
                        if (key === beforeId) {
                            children[id] = widget;
                        }
                        children[key] = orphans[key];
                    });
                    parent.children = children;
                } else {
                    parent.children[id] = widget;
                }
            }
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
                this.setDirty();
            }
        }

        setWidgetProperty(id: any, name: string, value: any) {
            var widget = this.findWidget(id);
            if (widget) {
                widget[name] = value;
                this.setDirty();
            }
        }

        setDirty(refreshData?: boolean) {
            this._dirty = true;
            if (refreshData) {
                this._dataSourcePanel.refresh();
            }
            this.saveUserState();
        }

        getDashboardViews() {
            var items = [];
            Object.keys(this._dashboard).map(function (id) {
                items.push({
                    key: Number(id),
                    value: this._dashboard[id].name
                });
            }, this);
            return items;
        }

        setViewDirty() {
            var id = this._selectedView;
            if (id && !this.isViewDirty(id)) {
                this._dirtyViews.push(id);
            }
            this.setDirty(true);
        }

        isViewDirty(id: any): boolean {
            return !this._dirtyViews.every(function (v) {
                return v !== id;
            })
        }

        selectView(id: any) {
            if (this._selectedView !== id) {
                this._selectedView = id;
                this.removeChildren(this._designSurface.hostElement);

                if (id) {
                    var v = this.getView(id);
                    wijmo.getElement("#view-label").innerText = v.name;
                    this.deserialize(v, "#design-surface");
                    this._properties.selectedObject = new ViewProperties(this);
                } else {
                    wijmo.getElement("#view-label").innerText = "";
                    this._properties.selectedObject = null;
                    this._views.selectedValue = null;
                }

                this._dataSourcePanel.dataSource = null;
                this._dataSourcePanel.dataSourceType = DataSourceType.None;
            }
        }

        selectViewProperties() {
            var v = this.getSelectedView();
            this._properties.selectedObject = v ? new ViewProperties(this) : null;
        }

        isSelectedView(id: any) {
            return id === this._selectedView;
        }

        getView(id: any) {
            return this._dashboard[id];
        }

        getSelectedView() {
            return this._selectedView ? this._dashboard[this._selectedView] : null;
        }

        generateViewName() {
            var n = 1, v = "View1", names = [];
            Object.keys(this._dashboard).map(function (id) {
                names.push(this._dashboard[id].name);
            }, this);
            while (names.indexOf(v) >= 0) {
                n++;
                v = "View" + n.toString();
            }
            return v;
        }

        createView() {
            var id = new Date().getTime();
            this._dashboard[id] = {
                id: id,
                name: this.generateViewName(),
                children: {}
            }
            this._views.itemsSource = this.getDashboardViews();
            this._views.selectedValue = id;
            this.setDirty(true);
        }

        shareView() {
            var v = this.getSelectedView();
            if (v) {
                var self = this;
                wijmo.httpRequest("/Share", {
                    method: "GET",
                    async: false,
                    requestHeaders: {
                        "RequestVerificationToken": (<HTMLInputElement>wijmo.getElement("[name='__RequestVerificationToken']")).value
                    },
                    success: function(xhr) {
                        var user = xhr.response;
                        var view = "/Viewer?user=" + user + "&id=" + v.id;
                        window.open(view, "_blank");
                    },
                    error: function(xhr) {
                        self.showError(xhr.responseText);
                    },
                });
            }
        }

        deleteView() {
            var v = this.getSelectedView();
            if (v) {
                var self = this;
                this._viewPopup.show(true, function(popup: wijmo.input.Popup) {
                    if (popup.dialogResult === "wj-hide-ok") {
                        delete self._dashboard[self._selectedView];
                        var views = self.getDashboardViews();
                        if (views.length > 0) {
                            self._views.itemsSource = views;
                            self.setDirty(true);
                        } else {
                            self.createView();
                        }
                    }
                });
            }
        }

        showError(msg: string) {
            this._failurePopupMsg.innerText = msg;
            this._failurePopup.show(true);
        }
    }
}
