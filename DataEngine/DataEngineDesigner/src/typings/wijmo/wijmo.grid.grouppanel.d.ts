/*!
    *
    * Wijmo Library 5.20191.615
    * http://wijmo.com/
    *
    * Copyright(c) GrapeCity, Inc.  All rights reserved.
    *
    * Licensed under the GrapeCity Commercial License.
    * sales@wijmo.com
    * wijmo.com/products/wijmo-5/license/
    *
    */
declare module wijmo.grid.grouppanel {
    /**
     * The {@link GroupPanel} control provides a drag and drop UI for editing
     * groups in a bound {@link FlexGrid} control.
     *
     * It allows users to drag columns from the {@link FlexGrid} into the
     * panel and to move groups within the panel. Users may click the
     * group markers in the panel to sort based on the group column or to
     * remove groups.
     *
     * In order to use a {@link GroupPanel}, add it to a page that contains a
     * {@link FlexGrid} control and set the panel's {@link grid} property to the
     * {@link FlexGrid} control. For example:
     *
     * <pre>// create a FlexGrid
     * var flex = new wijmo.grid.FlexGrid('#flex-grid');
     * flex.itemsSource = getData();
     * // add a GroupPanel to edit data groups
     * var groupPanel = new wijmo.grid.grouppanel.GroupPanel('#group-panel');
     * groupPanel.placeholder = "Drag columns here to create groups.";
     * groupPanel.grid = flex;</pre>
     *
     * The example below shows how you can use a {@link GroupPanel} control to
     * add Outlook-style grouping to a {@link FlexGrid} control:
     *
     * {@sample Grid/Grouping/GroupPanel/purejs Example}
     */
    class GroupPanel extends wijmo.Control {
        _g: any;
        _view: wijmo.collections.ICollectionView;
        _gds: wijmo.collections.ObservableArray;
        _hideGroupedCols: boolean;
        _showDragGlyphs: boolean;
        _maxGroups: number;
        _dragCol: wijmo.grid.Column;
        _dragMarker: HTMLElement;
        _divMarkers: HTMLElement;
        _divPH: HTMLElement;
        _hiddenCols: any[];
        _filter: wijmo.grid.filter.FlexGridFilter;
        /**
         * Gets or sets the template used to instantiate {@link GroupPanel} controls.
         */
        static controlTemplate: string;
        /**
         * Initializes a new instance of the {@link GroupPanel} class.
         *
         * @param element The DOM element that hosts the control, or a CSS selector for the host element (e.g. '#theCtrl').
         * @param options The JavaScript object containing initialization data for the control.
         */
        constructor(element: any, options?: any);
        /**
         * Gets or sets a value indicating whether the panel hides grouped columns in the owner grid.
         *
         * The {@link FlexGrid} displays grouping information in row headers, so it is
         * usually a good idea to hide grouped columns since they display redundant
         * information.
         *
         * The default value for this property is <b>true</b>.
         */
        hideGroupedColumns: boolean;
        /**
         * Gets or sets a value that determines whether the control should
         * add drag glyphs to the group marker elements.
         *
         * The default value for this property is <b>true</b>.
         */
        showDragGlyphs: boolean;
        /**
         * Gets or sets the maximum number of groups allowed.
         *
         * Setting this property to -1 allows any number of groups
         * to be created. Setting it to zero prevents any grouping.
         *
         * The default value for this property is <b>6</b>.
         */
        maxGroups: number;
        /**
         * Gets or sets a string to display in the control when it contains no groups.
         *
         * The default value for this property is <b>''</b> (empty string).
         */
        placeholder: string;
        /**
         * Gets or sets the {@link FlexGrid} that is connected to this {@link GroupPanel}.
         *
         * Once a grid is connected to the panel, the panel displays the groups
         * defined in the grid's data source. Users can drag grid columns
         * into the panel to create new groups, drag groups within the panel to
         * re-arrange the groups, or delete items in the panel to remove the groups.
         */
        grid: wijmo.grid.FlexGrid;
        /**
         * Gets or sets the {@link grid.filter.FlexGridFilter} to use for filtering
         * the grid data.
         *
         * If you set this property to a valid filter, the group descriptors will
         * display filter icons that can be used to see and edit the filer conditions
         * associated with the groups.
         */
        filter: wijmo.grid.filter.FlexGridFilter;
        /**
         * Updates the panel to show the current groups.
         */
        refresh(): void;
        _getColumnFilter(col: wijmo.grid.Column): any;
        _editFilter(marker: HTMLElement): void;
        _addGroup(col: wijmo.grid.Column, e: MouseEvent): void;
        _moveGroup(marker: HTMLElement, e: MouseEvent): void;
        _removeGroup(index: number, groups?: collections.ObservableArray): void;
        _getIndex(e: MouseEvent): number;
        _getElementIndex(e: HTMLElement): number;
        _draggingColumn(s: wijmo.grid.FlexGrid, e: wijmo.grid.CellRangeEventArgs): void;
        _itemsSourceChanging(s: wijmo.grid.FlexGrid, e: wijmo.EventArgs): void;
        _itemsSourceChanged(s: wijmo.grid.FlexGrid, e: wijmo.EventArgs): void;
        _collectionChanged(sender: any, e: wijmo.collections.NotifyCollectionChangedEventArgs): void;
        _dragStart(e: any): void;
        _dragOver(e: any): void;
        _drop(e: MouseEvent): void;
        _dragEnd(e: any): void;
        _click(e: any): void;
    }
}
declare module wijmo.grid.grouppanel {
}
