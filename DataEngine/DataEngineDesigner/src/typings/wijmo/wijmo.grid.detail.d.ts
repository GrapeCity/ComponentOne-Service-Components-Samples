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
declare module wijmo.grid.detail {
    /**
     * Row that contains a single detail cell spanning all grid columns.
     */
    class DetailRow extends wijmo.grid.Row {
        _detail: HTMLElement;
        /**
         * Initializes a new instance of the {@link DetailRow} class.
         *
         * @param parentRow {@link Row} that this {@link DetailRow} provides details for.
         */
        constructor(parentRow: wijmo.grid.Row);
        /**
         * Gets or sets the HTML element that represents the detail cell in this {@link DetailRow}.
         */
        detail: HTMLElement;
    }
}
declare module wijmo.grid.detail {
    /**
     * Merge manager class used by the {@link FlexGridDetailProvider} class.
     *
     * The {@link DetailMergeManager} merges detail cells (cells in a {@link DetailRow})
     * into a single detail cell that spans all grid columns.
     */
    class DetailMergeManager extends wijmo.grid.MergeManager {
        /**
         * Initializes a new instance of the {@link DetailMergeManager} class.
         *
         * @param grid The {@link FlexGrid} object that owns this {@link DetailMergeManager}.
         */
        constructor(grid: wijmo.grid.FlexGrid);
        /**
         * Gets a {@link CellRange} that specifies the merged extent of a cell
         * in a {@link GridPanel}.
         *
         * @param p The {@link GridPanel} that contains the range.
         * @param r The index of the row that contains the cell.
         * @param c The index of the column that contains the cell.
         * @param clip Whether to clip the merged range to the grid's current view range.
         * @return A {@link CellRange} that specifies the merged range, or null if the cell is not merged.
         */
        getMergedRange(p: wijmo.grid.GridPanel, r: number, c: number, clip?: boolean): wijmo.grid.CellRange;
    }
}
declare module wijmo.grid.detail {
    /**
     * Specifies constants that define the action to perform when the
     * ENTER key is pressed.
     */
    enum KeyAction {
        /** No special action (let the grid handle the key). */
        None = 0,
        /** Toggle the detail display. */
        ToggleDetail = 1
    }
    /**
     * Specifies when and how the row details are displayed.
     */
    enum DetailVisibilityMode {
        /**
         * Details are shown or hidden in code, using the
         * {@link FlexGridDetailProvider.showDetail} and
         * {@link FlexGridDetailProvider.hideDetail} methods.
         */
        Code = 0,
        /**
         * Details are shown for the row that is currently selected.
         */
        Selection = 1,
        /**
         * Details are shown or hidden using buttons added to the row headers.
         * Only one row may be expanded at a time.
         */
        ExpandSingle = 2,
        /**
         * Details are shown or hidden using buttons added to the row headers.
         * Multiple rows may be expanded at a time.
         */
        ExpandMulti = 3
    }
    /**
     * Implements detail rows for {@link FlexGrid} controls.
     *
     * To add detail rows to a {@link FlexGrid} control, create an instance of a
     * {@link FlexGridDetailProvider} and set the {@link createDetailCell} property
     * to a function that creates elements to be displayed in the detail cells.
     *
     * For example:
     *
     * <pre>// create FlexGrid to show categories
     * var gridCat = new wijmo.grid.FlexGrid('#gridCat');
     * gridCat.itemsSource = getCategories();
     * // add detail rows showing products in each category
     * var detailProvider = new wijmo.grid.detail.FlexGridDetailProvider(gridCat);
     * detailProvider.createDetailCell = function (row) {
     *   var cell = document.createElement('div');
     *   var gridProducts = new wijmo.grid.FlexGrid(cell);
     *   gridProducts.itemsSource = getProducts(row.dataItem.CategoryID);
     *   return cell;
     * }</pre>
     *
     * The {@link FlexGridDetailProvider} provides a {@link detailVisibilityMode} property
     * that determines when the detail rows should be displayed. The default value for
     * this property is <b>ExpandSingle</b>, which adds collapse/expand icons to the
     * row headers.
     *
     * The example below shows how you can use a {@link FlexGridDetailProvider} to add
     * different types of detail to the rows in a {@link FlexGrid}:
     *
     * {@sample Grid/Rows/RowDetail/purejs Example}
     */
    class FlexGridDetailProvider {
        static _WJC_DETAIL: string;
        _g: wijmo.grid.FlexGrid;
        _maxHeight: number;
        _mode: DetailVisibilityMode;
        _animated: boolean;
        _toSel: any;
        _createDetailCellFn: Function;
        _disposeDetailCellFn: Function;
        _rowHasDetailFn: Function;
        _keyActionEnter: KeyAction;
        /**
         * Initializes a new instance of the {@link FlexGridDetailProvider} class.
         *
         * @param grid {@link FlexGrid} that will receive detail rows.
         * @param options Initialization options for the new {@link FlexGridDetailProvider}.
         */
        constructor(grid: wijmo.grid.FlexGrid, options?: any);
        /**
         * Gets the {@link FlexGrid} that owns this {@link FlexGridDetailProvider}.
         */
        readonly grid: wijmo.grid.FlexGrid;
        /**
         * Gets or sets a value that determines when row details are displayed.
         *
         * The default value for this property is <b>DetailVisibilityMode.ExpandSingle</b>.
         */
        detailVisibilityMode: DetailVisibilityMode;
        /**
         * Gets or sets the maximum height of the detail rows, in pixels.
         *
         * The default value for this property is <b>null</b>, which means
         * there's no upper limit to the detail row height.
         */
        maxHeight: number;
        /**
         * Gets or sets a value that indicates whether to use animation when
         * showing row details.
         *
         * The default value for this property is <b>false</b>.
         */
        isAnimated: boolean;
        /**
         * Gets or sets the action to perform when the ENTER key is pressed.
         *
         * The default setting for this property is {@link KeyAction.None},
         * which lets the grid handle the key.
         * The other option is {@link KeyAction.ToggleDetail}, which handles
         * the Enter key to toggle the display of the row details.
         */
        keyActionEnter: KeyAction;
        /**
         * Gets or sets the callback function that creates detail cells.
         *
         * The callback function takes a {@link Row} as a parameter and
         * returns an HTML element representing the row details.
         * For example:
         *
         * <pre>// create detail cells for a given row
         * dp.createDetailCell = function (row) {
         *   var cell = document.createElement('div');
         *   var detailGrid = new wijmo.grid.FlexGrid(cell, {
         *     itemsSource: getProducts(row.dataItem.CategoryID),
         *     headersVisibility: wijmo.grid.HeadersVisibility.Column
         *   });
         *   return cell;
         * };</pre>
         */
        createDetailCell: Function;
        /**
         * Gets or sets the callback function that disposes of detail cells.
         *
         * The callback function takes a {@link Row} as a parameter and
         * disposes of any resources associated with the detail cell.
         *
         * This function is optional. Use it in cases where the
         * {@link createDetailCell} function allocates resources that are not
         * automatically garbage-collected.
         */
        disposeDetailCell: Function;
        /**
         * Gets or sets the callback function that determines whether a row
         * has details.
         *
         * The callback function takes a {@link Row} as a parameter and
         * returns a boolean value that indicates whether the row has
         * details. For example:
         *
         * <pre>// remove details from items with odd CategoryID
         * dp.rowHasDetail = function (row) {
         *   return row.dataItem.CategoryID % 2 == 0;
         * };</pre>
         *
         * Setting this property to null means all regular data
         * rows (not group rows or new item templates) have details.
         */
        rowHasDetail: Function;
        /**
         * Gets the detail row associated with a given grid row.
         *
         * @param row Row or index of the row to investigate.
         */
        getDetailRow(row: any): DetailRow;
        /**
         * Gets a value that determines if a row's details are visible.
         *
         * @param row Row or index of the row to investigate.
         */
        isDetailVisible(row: any): boolean;
        /**
         * Gets a value that determines if a row has details to show.
         *
         * @param row Row or index of the row to investigate.
         */
        isDetailAvailable(row: any): boolean;
        /**
         * Hides the detail row for a given row.
         *
         * @param row Row or index of the row that will have its details hidden.
         * This parameter is optional. If not provided, all detail rows are hidden.
         */
        hideDetail(row?: any): void;
        /**
         * Shows the detail row for a given row.
         *
         * @param row Row or index of the row that will have its details shown.
         * @param hideOthers Whether to hide details for all other rows.
         */
        showDetail(row: any, hideOthers?: boolean): void;
        _sizeDetailRow(row: DetailRow): void;
        _handleFixedCells(): void;
        _toIndex(row: any): number;
        _hdrClick(e: MouseEvent): void;
        _toggleRowDetail(row: number): boolean;
        _selectionChanged(s: wijmo.grid.FlexGrid, e: wijmo.EventArgs): void;
        _formatItem(s: any, e: wijmo.grid.FormatItemEventArgs): void;
        _resizedRow(s: any, e: wijmo.grid.CellRangeEventArgs): void;
        _hasDetail(row: number): boolean;
        _isRegularRow(row: wijmo.grid.Row): boolean;
        _createDetailCell(row: wijmo.grid.Row, col?: wijmo.grid.Column): HTMLElement;
    }
}
declare module wijmo.grid.detail {
}
