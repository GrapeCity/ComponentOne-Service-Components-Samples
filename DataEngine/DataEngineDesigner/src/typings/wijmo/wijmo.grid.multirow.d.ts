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
declare module wijmo.grid.multirow {
    /**
     * Extends the {@link Column} class with <b>colspan</b> property to
     * describe a cell in a {@link _CellGroup}.
     */
    class _Cell extends wijmo.grid.Column {
        _row: number;
        _col: number;
        _colspan: number;
        _rowspan: number;
        /**
         * Initializes a new instance of the {@link _Cell} class.
         *
         * @param options JavaScript object containing initialization data for the {@link _Cell}.
         */
        constructor(options?: any);
        /**
         * Gets or sets the number of physical columns spanned by the {@link _Cell}.
         */
        colspan: number;
        /**
         * Gets or sets the number of physical rows spanned by the {@link _Cell}.
         */
        rowspan: number;
    }
}
declare module wijmo.grid.multirow {
    /**
     * Extends the {@link Row} to provide additional information for multi-row records.
     */
    class _MultiRow extends wijmo.grid.Row {
        _idxData: number;
        _idxRecord: number;
        /**
         * Initializes a new instance of the {@link Row} class.
         *
         * @param dataItem The data item this row is bound to.
         * @param dataIndex The index of the record within the items source.
         * @param recordIndex The index of this row within the record (data item).
         */
        constructor(dataItem: any, dataIndex: number, recordIndex: number);
        /**
         * Gets the index of this row within the record (data item) it represents.
         */
        readonly recordIndex: number;
        /**
         * Gets the index of this row within the data source collection.
         */
        readonly dataIndex: number;
    }
}
declare module wijmo.grid.multirow {
    /**
     * Describes a group of cells that may span multiple rows and columns.
     */
    class _CellGroup extends _Cell {
        _g: MultiRow;
        _colstart: number;
        _cells: _Cell[];
        _rng: wijmo.grid.CellRange[];
        _cols: wijmo.grid.ColumnCollection;
        /**
         * Initializes a new instance of the {@link _CellGroup} class.
         *
         * @param grid {@link MultiRow} that owns the {@link _CellGroup}.
         * @param options JavaScript object containing initialization data for the new {@link _CellGroup}.
         */
        constructor(grid: MultiRow, options?: any);
        _copy(key: string, value: any): boolean;
        readonly cells: _Cell[];
        closeGroup(rowsPerItem: number): void;
        getColumnWidth(c: number): any;
        getMergedRange(p: wijmo.grid.GridPanel, r: number, c: number): wijmo.grid.CellRange;
        getBindingColumn(p: wijmo.grid.GridPanel, r: number, c: number): wijmo.grid.Column;
        getColumn(name: string): wijmo.grid.Column;
    }
}
declare module wijmo.grid.multirow {
    /**
     * Provides custom merging for {@link MultiRow} controls.
     */
    class _MergeManager extends wijmo.grid.MergeManager {
        /**
         * Gets a {@link CellRange} that specifies the merged extent of a cell
         * in a {@link GridPanel}.
         *
         * @param p The {@link GridPanel} that contains the range.
         * @param r The index of the row that contains the cell.
         * @param c The index of the column that contains the cell.
         * @param clip Specifies whether to clip the merged range to the grid's current view range.
         * @return A {@link CellRange} that specifies the merged range, or null if the cell is not merged.
         */
        getMergedRange(p: wijmo.grid.GridPanel, r: number, c: number, clip?: boolean): wijmo.grid.CellRange;
    }
}
declare module wijmo.grid.multirow {
    /**
     * Extends the {@link FlexGrid} control to provide multiple rows per item.
     *
     * Use the <b>layoutDefinition</b> property to define the layout of the rows
     * used to display each data item.
     *
     * A few {@link FlexGrid} properties are disabled in the {@link MultiRow} control
     * because they would interfere with the custom multi-row layouts.
     * The list of disabled properties includes {@link FlexGrid.allowMerging} and
     * {@link FlexGrid.childItemsPath}.
     */
    class MultiRow extends wijmo.grid.FlexGrid {
        _rowsPerItem: number;
        _layoutDef: any[];
        _cellBindingGroups: _CellGroup[];
        _cellGroupsByColumn: any;
        _centerVert: boolean;
        _collapsedHeaders: boolean;
        _collapsedHeadersWasNull: boolean;
        _btnCollapse: HTMLElement;
        /**
         * Initializes a new instance of the {@link MultiRow} class.
         *
         * In most cases, the <b>options</b> parameter will include the value for the
         * {@link layoutDefinition} property.
         *
         * @param element The DOM element that hosts the control, or a CSS selector for the host element (e.g. '#theCtrl').
         * @param options JavaScript object containing initialization data for the control.
         */
        constructor(element: any, options?: any);
        _getProductInfo(): string;
        /**
         * Gets or sets an array that defines the layout of the rows used to display each data item.
         *
         * The array contains a list of cell group objects which have the following properties:
         *
         * <ul>
         * <li><b>header</b>: Group header (shown when the headers are collapsed)</li>
         * <li><b>colspan</b>: Number of grid columns spanned by the group</li>
         * <li><b>cells</b>: Array of cell objects, which extend {@link Column} with a <b>colspan</b> property.</li>
         * </ul>
         *
         * When the {@link layoutDefinition} property is set, the grid scans the cells in each
         * group as follows:
         *
         * <ol>
         * <li>The grid calculates the colspan of the group either as group's own colspan
         * or as span of the widest cell in the group, whichever is wider.</li>
         * <li>If the cell fits the current row within the group, it is added to the current row.</li>
         * <li>If it doesn't fit, it is added to a new row.</li>
         * </ol>
         *
         * When all groups are ready, the grid calculates the number of rows per record to the maximum
         * rowspan of all groups, and adds rows to each group to pad their height as needed.
         *
         * This scheme is simple and flexible. For example:
         * <pre>{ header: 'Group 1', cells: [{ binding: 'c1' }, { bnding: 'c2'}, { binding: 'c3' }]}</pre>
         *
         * The group has colspan 1, so there will be one cell per column. The result is:
         * <pre>
         * | C1 |
         * | C2 |
         * | C3 |
         * </pre>
         *
         * To create a group with two columns, set <b>colspan</b> property of the group:
         *
         * <pre>{ header: 'Group 1', colspan: 2, cells:[{ binding: 'c1' }, { binding: 'c2'}, { binding: 'c3' }]}</pre>
         *
         * The cells will wrap as follows:
         * <pre>
         * | C1  | C2 |
         * | C3       |
         * </pre>
         *
         * Note that the last cell spans two columns (to fill the group).
         *
         * You can also specify the colspan on individual cells rather than on the group:
         *
         * <pre>{ header: 'Group 1', cells: [{binding: 'c1', colspan: 2 }, { bnding: 'c2'}, { binding: 'c3' }]}</pre>
         *
         * Now the first cell has colspan 2, so the result is:
         * <pre>
         * | C1       |
         * | C2 |  C3 |
         * </pre>
         *
         * Because cells extend the {@link Column} class, you can add all the usual {@link Column}
         * properties to any cells:
         * <pre>
         * { header: 'Group 1', cells: [
         *    { binding: 'c1', colspan: 2 },
         *    { bnding: 'c2'},
         *    { binding: 'c3', format: 'n0', required: false, etc... }
         * ]}</pre>
         */
        layoutDefinition: any[];
        /**
         * Gets the number of rows used to display each item.
         *
         * This value is calculated automatically based on the value
         * of the <b>layoutDefinition</b> property.
         */
        readonly rowsPerItem: number;
        /**
         * Gets the {@link Column} object used to bind a data item to a grid cell.
         *
         * @param p {@link GridPanel} that contains the cell.
         * @param r Index of the row that contains the cell.
         * @param c Index of the column that contains the cell.
         */
        getBindingColumn(p: wijmo.grid.GridPanel, r: number, c: number): wijmo.grid.Column;
        /**
         * Gets a column by name or by binding.
         *
         * The method searches the column by name. If a column with the given name
         * is not found, it searches by binding. The searches are case-sensitive.
         *
         * @param name The name or binding to find.
         * @return The column with the specified name or binding, or null if not found.
         */
        getColumn(name: string): wijmo.grid.Column;
        /**
         * Gets or sets a value that determines whether the content of cells
         * that span multiple rows should be vertically centered.
         *
         * The default value for this property is <b>true</b>.
         */
        centerHeadersVertically: boolean;
        /**
         * Gets or sets a value that determines whether column headers
         * should be collapsed and displayed as a single row containing
         * the group headers.
         *
         * If you set the {@link collapsedHeaders} property to <b>true</b>,
         * remember to set the <b>header</b> property of every group in
         * order to avoid empty header cells.
         *
         * Setting the {@link collapsedHeaders} property to <b>null</b> causes
         * the grid to show all header information (groups and columns).
         * In this case, the first row will show the group headers and the
         * remaining rows will show the individual column headers.
         *
         * The default value for this property is <b>false</b>.
         */
        collapsedHeaders: boolean;
        /**
         * Gets or sets a value that determines whether the grid should display
         * a button in the column header panel to allow users to collapse and
         * expand the column headers.
         *
         * If the button is visible, clicking on it will cause the grid to
         * toggle the value of the <b>collapsedHeaders</b> property.
         *
         * The default value for this property is <b>false</b>.
         */
        showHeaderCollapseButton: boolean;
        /**
         * Occurs after the value of the {@link collapsedHeaders} property changes.
         */
        readonly collapsedHeadersChanging: Event;
        /**
         * Raises the {@link collapsedHeadersChanging} event.
         *
         * @param e {@link CancelEventArgs} that contains the event data.
         * @return True if the event was not canceled.
         */
        onCollapsedHeadersChanging(e: wijmo.CancelEventArgs): boolean;
        /**
         * Occurs after the value of the {@link collapsedHeaders} property has changed.
         */
        readonly collapsedHeadersChanged: Event;
        /**
         * Raises the {@link collapsedHeadersChanged} event.
         */
        onCollapsedHeadersChanged(e?: wijmo.EventArgs): void;
        _getQuickAutoSize(): boolean;
        _addBoundRow(items: any[], index: number): void;
        _addNode(items: any[], index: number, level: number): void;
        _bindColumns(): void;
        _updateCollapsedHeaders(): void;
        _updateColumnTypes(): void;
        _getBindingColumn(p: wijmo.grid.GridPanel, r: number, c: wijmo.grid.Column): wijmo.grid.Column;
        _cvCollectionChanged(sender: any, e: wijmo.collections.NotifyCollectionChangedEventArgs): void;
        _parseCellGroups(groups: any[]): _CellGroup[];
        _formatItem(s: MultiRow, e: wijmo.grid.FormatItemEventArgs): void;
        _updateButtonGlyph(): void;
        _getError(p: wijmo.grid.GridPanel, r: number, c: number): string;
    }
}
declare module wijmo.grid.multirow {
    /**
     * Manages the new row template used to add rows to the grid.
     */
    class _MultiRowAddNewHandler extends wijmo.grid._AddNewHandler {
        /**
         * Initializes a new instance of the {@link _AddNewHandler} class.
         *
         * @param grid {@link FlexGrid} that owns this {@link _AddNewHandler}.
         */
        constructor(grid: wijmo.grid.FlexGrid);
        /**
         * Updates the new row template to ensure that it is visible only when the grid is
         * bound to a data source that supports adding new items, and that it is
         * in the right position.
         */
        updateNewRowTemplate(): void;
        _keydown(e: KeyboardEvent): void;
        _rowEditEnded(s: MultiRow, e: wijmo.grid.CellRangeEventArgs): void;
        _beginningEdit(s: MultiRow, e: wijmo.grid.CellRangeEventArgs): void;
        _copyNewDataItem(): void;
        _removeNewRowTemplate(): void;
    }
}
declare module wijmo.grid.multirow {
}
