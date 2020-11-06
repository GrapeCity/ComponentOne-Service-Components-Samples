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
declare module wijmo.grid {
    /**
     * Represents a data map for use with a column's {@link Column.dataMap} property.
     *
     * Data maps provide the grid with automatic look up capabilities. For example,
     * you may want to display a customer name instead of his ID, or a color name
     * instead of its RGB value.
     *
     * The code below binds a grid to a collection of products, then assigns a
     * {@link DataMap} to the grid's 'CategoryID' column so the grid displays the
     * category names rather than the raw IDs.
     *
     * The grid takes advantage of data maps also for editing. If the <b>wijmo.input</b>
     * module is loaded, then when editing data-mapped columns the grid will show
     * a drop-down list containing the values on the map.
     *
     * <pre>
     * // bind grid to products
     * var flex = new wijmo.grid.FlexGrid();
     * flex.itemsSource = products;
     * // map CategoryID column to show category name instead of ID
     * var col = flex.columns.getColumn('CategoryID');
     * col.dataMap = new wijmo.grid.DataMap(categories, 'CategoryID', 'CategoryName');
     * </pre>
     *
     * In general, data maps apply to whole columns. However, there are situations
     * where you may want to restrict the options available for a cell based on a
     * value on a different column. For example, if you have "Country" and "City"
     * columns, you will probably want to restrict the cities based on the current
     * country.
     *
     * There are two ways you can implement these "dynamic" data maps:
     *
     * <ol>
     *   <li>
     *     If the {@link DataMap} is just a list of strings, you can change it before
     *     the grid enters edit mode. In this case, the cells contain the string
     *     being displayed, and changing the map won't affect other cells in the
     *     same column.
     *     This fiddle demonstrates:
     *     <a href="https://jsfiddle.net/Wijmo5/8brL80r8/">show me</a>.
     *   </li>
     *   <li>
     *     If the {@link DataMap} is a real map (stores key values in the cells, shows
     *     a corresponding string), then you can apply a filter to restrict the
     *     values shown in the drop-down. The {@link DataMap} will still contain the
     *     same keys and values, so other cells in the same column won't be disturbed
     *     by the filter.
     *     This fiddle demonstrates:
     *     <a href="https://jsfiddle.net/Wijmo5/xborLd4t/">show me</a>.
     *   </li>
     * </ol>
     *
     * In some cases, you may want to create a {@link DataMap} to represent an enumeration.
     * This can be done with the following code:
     *
     * <pre>// build a DataMap for a given enum
     *  function getDataMap(enumClass) {
     *      var pairs = [];
     *      for (var key in enumClass) {
     *          var val = parseInt(key);
     *          if (!isNaN(val)) {
     *              pairs.push({ key: val, name: enumClass[val] });
     *          }
     *      }
     *      return new wijmo.grid.DataMap(pairs, 'key', 'name');
     * }</pre>
     */
    class DataMap {
        _cv: wijmo.collections.ICollectionView;
        _keyPath: string;
        _displayPath: string;
        _editable: boolean;
        _sortByKey: boolean;
        _hash: any;
        /**
         * Initializes a new instance of the {@link DataMap} class.
         *
         * @param itemsSource An array or {@link ICollectionView} that contains the items to map.
         * @param selectedValuePath The name of the property that contains the keys (data values).
         * @param displayMemberPath The name of the property to use as the visual representation of the items.
         */
        constructor(itemsSource: any, selectedValuePath?: string, displayMemberPath?: string);
        /**
         * Gets or sets a value that determines whether to use mapped (display)
         * or raw values when sorting the data.
         */
        sortByDisplayValues: boolean;
        /**
         * Gets the {@link ICollectionView} object that contains the map data.
         */
        readonly collectionView: wijmo.collections.ICollectionView;
        /**
         * Gets the name of the property to use as a key for the item (data value).
         */
        readonly selectedValuePath: string;
        /**
         * Gets the name of the property to use as the visual representation of the item.
         */
        readonly displayMemberPath: string;
        /**
         * Gets the key that corresponds to a given display value.
         *
         * @param displayValue The display value of the item to retrieve.
         */
        getKeyValue(displayValue: string): any;
        /**
         * Gets the display value that corresponds to a given key.
         *
         * @param key The key of the item to retrieve.
         */
        getDisplayValue(key: any): any;
        /**
         * Gets an array with all of the display values on the map.
         *
         * @param dataItem Data item for which to get the display items.
         * This parameter is optional. If not provided, all possible display
         * values should be returned.
         */
        getDisplayValues(dataItem?: any): string[];
        /**
         * Gets an array with all of the keys on the map.
         */
        getKeyValues(): string[];
        /**
         * Gets or sets a value that indicates whether users should be allowed to enter
         * values that are not present on the {@link DataMap}.
         *
         * In order for a {@link DataMap} to be editable, the {@link selectedValuePath} and
         * {@link displayMemberPath} must be set to the same value.
         */
        isEditable: boolean;
        /**
         * Occurs when the map data changes.
         */
        readonly mapChanged: Event;
        /**
         * Raises the {@link mapChanged} event.
         */
        onMapChanged(e?: wijmo.EventArgs): void;
        private _indexOf;
    }
}
declare module wijmo.grid {
    /**
     * Represents a rectangular group of cells defined by two row indices and
     * two column indices.
     */
    class CellRange {
        _row: number;
        _col: number;
        _row2: number;
        _col2: number;
        /**
         * Initializes a new instance of the {@link CellRange} class.
         *
         * @param r The index of the first row in the range (defaults to -1).
         * @param c The index of the first column in the range (defaults to -1).
         * @param r2 The index of the last row in the range (defaults to <b>r</b>).
         * @param c2 The index of the last column in the range (defaults to <b>c</b>).
         */
        constructor(r?: number, c?: number, r2?: number, c2?: number);
        /**
         * Initializes an existing {@link CellRange}.
         *
         * @param r The index of the first row in the range (defaults to -1).
         * @param c The index of the first column in the range (defaults to -1).
         * @param r2 The index of the last row in the range (defaults to <b>r</b>).
         * @param c2 The index of the last column in the range (defaults to <b>c</b>).
         */
        setRange(r?: number, c?: number, r2?: number, c2?: number): void;
        /**
         * Gets or sets the index of the first row in the range.
         */
        row: number;
        /**
         * Gets or sets the index of the first column in the range.
         */
        col: number;
        /**
         * Gets or sets the index of the second row in the range.
         */
        row2: number;
        /**
         * Gets or sets the index of the second column in the range.
         */
        col2: number;
        /**
         * Creates a copy of the range.
         */
        clone(): CellRange;
        /**
         * Copies an existing cell range into this one.
         *
         * @param rng {@link CellRange} to copy into this one.
         */
        copy(rng: CellRange): void;
        /**
         * Gets the number of rows in the range.
         */
        readonly rowSpan: number;
        /**
         * Gets the number of columns in the range.
         */
        readonly columnSpan: number;
        /**
         * Gets the index of the top row in the range.
         */
        readonly topRow: number;
        /**
         * Gets the index of the bottom row in the range.
         */
        readonly bottomRow: number;
        /**
         * Gets the index of the leftmost column in the range.
         */
        readonly leftCol: number;
        /**
         * Gets the index of the rightmost column in the range.
         */
        readonly rightCol: number;
        /**
         * Checks whether the range contains valid row and column indices
         * (row and column values are zero or greater).
         */
        readonly isValid: boolean;
        /**
         * Checks whether this range corresponds to a single cell (beginning and ending rows have
         * the same index, and beginning and ending columns have the same index).
         */
        readonly isSingleCell: boolean;
        /**
         * Checks whether the range contains another range or a specific cell.
         *
         * @param r The CellRange object or row index to find.
         * @param c The column index (required if the r parameter is not a CellRange object).
         */
        contains(r: any, c?: number): boolean;
        /**
         * Checks whether the range contains a given row.
         *
         * @param r The index of the row to find.
         */
        containsRow(r: number): boolean;
        /**
         * Checks whether the range contains a given column.
         *
         * @param c The index of the column to find.
         */
        containsColumn(c: number): boolean;
        /**
         * Checks whether the range intersects another range.
         *
         * @param rng The CellRange object to check.
         */
        intersects(rng: CellRange): boolean;
        /**
         * Checks whether the range intersects the rows in another range.
         *
         * @param rng The CellRange object to check.
         */
        intersectsRow(rng: CellRange): boolean;
        /**
         * Checks whether the range intersects the columns in another range.
         *
         * @param rng The CellRange object to check.
         */
        intersectsColumn(rng: CellRange): boolean;
        /**
         * Gets the rendered size of this range.
         *
         * @param p The {@link GridPanel} object that contains the range.
         * @return A {@link Size} object that represents the sum of row heights and column widths in the range.
         */
        getRenderSize(p: GridPanel): wijmo.Size;
        /**
         * Checks whether the range equals another range.
         *
         * @param rng The CellRange object to compare to this range.
         */
        equals(rng: CellRange): boolean;
    }
}
declare module wijmo.grid {
    /**
     * Provides arguments for {@link CellRange} events.
     */
    class CellRangeEventArgs extends wijmo.CancelEventArgs {
        _p: GridPanel;
        _rng: CellRange;
        _data: any;
        /**
         * Initializes a new instance of the {@link CellRangeEventArgs} class.
         *
         * @param p {@link GridPanel} that contains the range.
         * @param rng Range of cells affected by the event.
         * @param data Data related to the event.
         */
        constructor(p: GridPanel, rng: CellRange, data?: any);
        /**
         * Gets the {@link GridPanel} affected by this event.
         */
        readonly panel: GridPanel;
        /**
         * Gets the {@link CellRange} affected by this event.
         */
        readonly range: CellRange;
        /**
         * Gets the row affected by this event.
         */
        readonly row: number;
        /**
         * Gets the column affected by this event.
         */
        readonly col: number;
        /**
         * Gets or sets the data associated with the event.
         */
        data: any;
    }
    /**
     * Provides arguments for the {@link FlexGrid.formatItem} event.
     */
    class FormatItemEventArgs extends CellRangeEventArgs {
        _cell: HTMLElement;
        /**
        * Initializes a new instance of the {@link FormatItemEventArgs} class.
        *
        * @param p {@link GridPanel} that contains the range.
        * @param rng Range of cells affected by the event.
        * @param cell Element that represents the grid cell to be formatted.
        */
        constructor(p: GridPanel, rng: CellRange, cell: HTMLElement);
        /**
         * Gets a reference to the element that represents the grid cell to be formatted.
         */
        readonly cell: HTMLElement;
    }
    /**
     * Provides arguments for the {@link FlexGrid.cellEditEnding} event.
     */
    class CellEditEndingEventArgs extends CellRangeEventArgs {
        _stayInEditMode: boolean;
        _refresh: boolean;
        /**
         * Gets or sets whether the cell should remain in edit mode instead
         * of finishing the edits.
         */
        stayInEditMode: boolean;
        /**
         * Gets or sets a value that determines whether the grid should refresh
         * all its contents after the edits are done.
         */
        refresh: boolean;
    }
}
declare module wijmo.grid {
    /**
     * Specifies constants that define the selection behavior.
     */
    enum SelectionMode {
        /** The user cannot select cells using the mouse or keyboard. */
        None = 0,
        /** The user can select only a single cell at a time. */
        Cell = 1,
        /** The user can select contiguous blocks of cells. */
        CellRange = 2,
        /** The user can select a single row at a time. */
        Row = 3,
        /** The user can select contiguous rows. */
        RowRange = 4,
        /** The user can select non-contiguous rows. */
        ListBox = 5
    }
    /**
     * Specifies constants that represent the selected state of a cell.
     */
    enum SelectedState {
        /** The cell is not selected. */
        None = 0,
        /** The cell is selected but is not the active cell. */
        Selected = 1,
        /** The cell is selected and is the active cell. */
        Cursor = 2,
        /** The cell is active cell but not in a selected state. */
        Active = 3
    }
    /**
     * Specifies constants that represent a type of movement for the selection.
     */
    enum SelMove {
        /** Do not change the selection. */
        None = 0,
        /** Select the next visible cell. */
        Next = 1,
        /** Select the previous visible cell. */
        Prev = 2,
        /** Select the first visible cell in the next page. */
        NextPage = 3,
        /** Select the first visible cell in the previous page. */
        PrevPage = 4,
        /** Select the first visible cell. */
        Home = 5,
        /** Select the last visible cell. */
        End = 6,
        /** Select the next visible cell skipping rows if necessary. */
        NextCell = 7,
        /** Select the previous visible cell skipping rows if necessary. */
        PrevCell = 8
    }
    /**
     * Handles the grid's selection.
     */
    class _SelectionHandler {
        private _g;
        private _sel;
        private _mode;
        private _rng;
        private _e;
        /**
         * Initializes a new instance of the {@link _SelectionHandler} class.
         *
         * @param g {@link FlexGrid} that owns this {@link _SelectionHandler}.
         */
        constructor(g: FlexGrid);
        /**
         * Gets or sets the current selection mode.
         */
        selectionMode: SelectionMode;
        /**
         * Gets or sets the current selection.
         */
        selection: CellRange;
        /**
         * Selects a cell range and optionally scrolls it into view.
         *
         * @param rng Range to select.
         * @param show Whether to scroll the new selection into view.
         */
        select(rng: any, show?: any): void;
        /**
         * Moves the selection by a specified amount in the vertical and horizontal directions.
         * @param rowMove How to move the row selection.
         * @param colMove How to move the column selection.
         * @param extend Whether to extend the current selection or start a new one.
         */
        moveSelection(rowMove: SelMove, colMove: SelMove, extend: boolean): void;
        private _showSelection;
        private _adjustReferenceCell;
    }
}
declare module wijmo.grid {
    /**
     * Specifies flags that represent the state of a grid row or column.
     */
    enum RowColFlags {
        /** The row or column is visible. */
        Visible = 1,
        /** The row or column can be resized. */
        AllowResizing = 2,
        /** The row or column can be dragged to a new position with the mouse. */
        AllowDragging = 4,
        /** The row or column can contain merged cells. */
        AllowMerging = 8,
        /** The column can be sorted by clicking its header with the mouse. */
        AllowSorting = 16,
        /** The column was generated automatically. */
        AutoGenerated = 32,
        /** The group row is collapsed. */
        Collapsed = 64,
        /** The row has a parent group that is collapsed. */
        ParentCollapsed = 128,
        /** The row or column is selected. */
        Selected = 256,
        /** The row or column is read-only (cannot be edited). */
        ReadOnly = 512,
        /** Cells in this row or column contain HTML text. */
        HtmlContent = 1024,
        /** Cells in this row or column may contain wrapped text. */
        WordWrap = 2048,
        /** Cells in this row or column may contain wrapped text. */
        MultiLine = 4096,
        /** Cells in this column have templates. */
        HasTemplate = 8192,
        /** Default settings for new rows. */
        RowDefault = 3,
        /** Default settings for new columns. */
        ColumnDefault = 23
    }
    /**
     * An abstract class that serves as a base for the {@link Row} and {@link Column} classes.
     */
    class RowCol {
        protected _type: wijmo.DataType;
        protected _align: string;
        protected _inpType: string;
        protected _mask: string;
        protected _maxLen: number;
        protected _required: boolean;
        protected _fmt: string;
        protected _map: DataMap;
        protected _showDropDown: boolean;
        protected _ddCssClass: string;
        protected _sz: number;
        protected _cssClass: string;
        protected _cssClassAll: string;
        protected _szMin: number;
        protected _szMax: number;
        protected _f: RowColFlags;
        _list: RowColCollection;
        _pos: number;
        _idx: number;
        _vidx: number;
        /**
         * Gets or sets the type of value stored in the column or row.
         *
         * Values are coerced into the proper type when editing the grid.
         */
        dataType: wijmo.DataType;
        /**
         * Gets or sets the "type" attribute of the HTML input element used to
         * edit values in this column or row.
         *
         * By default, this property is set to "tel" for numeric columns, and to
         * "text" for all other non-boolean column types. The "tel" input type
         * causes mobile devices to show a numeric keyboard that includes a negative
         * sign and a decimal separator.
         *
         * Use this property to change the default setting if the default does not
         * work well for the current culture, device, or application.
         * In these cases, try setting the property to "number" or simply "text."
         */
        inputType: string;
        /**
         * Gets or sets a mask to use while editing values in this column or row.
         *
         * The mask format is the same used by the {@link wijmo.input.InputMask}
         * control.
         *
         * If specified, the mask must be compatible with the value of
         * the {@link format} property. For example, the mask '99/99/9999' can
         * be used for entering dates formatted as 'MM/dd/yyyy'.
         */
        mask: string;
        /**
         * Gets or sets the maximum number of characters that the can
         * be entered into cells in this column or row.
         *
         * This property is set to null by default, which allows entries
         * with any number of characters.
         */
        maxLength: number;
        /**
         * Gets or sets the horizontal alignment of cells in the column or row.
         *
         * The default value for this property is null, which causes the grid to select
         * the alignment automatically based on the column's {@link dataType} (numbers are
         * right-aligned, Boolean values are centered, and other types are left-aligned).
         *
         * If you want to override the default alignment, set this property
         * to 'left', 'right', 'center', or 'justify'.
         */
        align: string;
        /**
         * Gets or sets the format string used to convert raw values into
         * display values for the column or row (see {@link Globalize}).
         */
        format: string;
        /**
         * Gets or sets the {@link DataMap} used to convert raw values into display
         * values for the column or row.
         *
         * Columns with an associated {@link dataMap} show drop-down buttons that
         * can be used for quick editing. If you do not want to show the drop-down
         * buttons, set the column's {@link showDropDown} property to false.
         *
         * Cell drop-downs require the wijmo.input module to be loaded.
         */
        dataMap: DataMap;
        /**
         * Gets or sets a value that indicates whether the grid adds drop-down
         * buttons to the cells in this column or row.
         *
         * The drop-down buttons are shown only if the column has a {@link dataMap}
         * set and is editable. Clicking on the drop-down buttons causes the grid
         * to show a list where users can select the value for the cell.
         *
         * Cell drop-downs require the wijmo.input module to be loaded.
         *
         * The default value for this property is <b>true</b>.
         */
        showDropDown: boolean;
        /**
         * Gets or sets a CSS class name to add to drop-downs in this column or row.
         *
         * The drop-down buttons are shown only if the column has a {@link dataMap}
         * set and is editable. Clicking on the drop-down buttons causes the grid
         * to show a list where users can select the value for the cell.
         *
         * Cell drop-downs require the wijmo.input module to be loaded.
         */
        dropDownCssClass: string;
        /**
         * Gets or sets a value that indicates whether the column or row
         * is visible.
         */
        visible: boolean;
        /**
         * Gets a value that indicates whether the column or row is
         * visible and not collapsed.
         *
         * This property is read-only. To change the visibility of a
         * column or row, use the {@link visible} property instead.
         */
        readonly isVisible: boolean;
        /**
         * Gets the position of the column or row in pixels.
         */
        readonly pos: number;
        /**
         * Gets the index of the column or row in the parent collection.
         */
        readonly index: number;
        /**
         * Gets the index of the column or row in the parent collection
         * ignoring invisible elements ({@link isVisible}).
         */
        readonly visibleIndex: number;
        /**
         * Gets or sets the size of the column or row.
         *
         * Setting this property to null or negative values causes
         * the element to use the parent collection's default size.
         */
        size: number;
        /**
         * Gets the render size of the column or row.
         *
         * This property accounts for visibility, default size,
         * and min and max sizes.
         */
        readonly renderSize: number;
        /**
         * Gets or sets a value that indicates whether the user can resize
         * the column or row with the mouse.
         *
         * The default value for this property is <b>true</b>.
         */
        allowResizing: boolean;
        /**
         * Gets or sets a value that indicates whether the user can move
         * the column or row to a new position with the mouse.
         *
         * The default value for this property is <b>true</b>.
         */
        allowDragging: boolean;
        /**
         * Gets or sets a value that indicates whether cells in the
         * column or row can be merged.
         *
         * The default value for this property is <b>false</b>.
         */
        allowMerging: boolean;
        /**
         * Gets or sets a value that indicates whether the column or row
         * is selected.
         */
        isSelected: boolean;
        /**
         * Gets or sets a value that indicates whether cells in the
         * column or row can be edited.
         *
         * The default value for this property is <b>true</b>.
         */
        isReadOnly: boolean;
        /**
         * Gets or sets a value that determines whether values in this
         * column or row are required.
         *
         * By default, this property is set to null, which means values
         * are required, but non-masked string columns may contain empty
         * strings.
         *
         * When set to true, values are required and empty strings are
         * not allowed.
         *
         * When set to false, null values and empty strings are allowed.
         */
        isRequired: boolean;
        /**
         * Gets or sets a value that indicates whether cells in this column or row
         * contain HTML content rather than plain text.
         *
         * This property only applies to regular cells. Row and column header
         * cells contain plain text by default. If you want to display HTML
         * in column or row headers, you must use the {@link FlexGrid.formatItem}
         * event and set the cell's innerHTML content in code.
         *
         * The default value for this property is <b>false</b>.
         */
        isContentHtml: boolean;
        /**
         * Gets or sets a value that indicates whether the content of cells in
         * this column or row should wrap to fit the available column width.
         *
         * The default value for this property is <b>false</b>.
         */
        wordWrap: boolean;
        /**
         * Gets or sets a value that indicates whether the content of cells in
         * this column or row should wrap at new line characters (\n).
         *
         * The default value for this property is <b>false</b>.
         */
        multiLine: boolean;
        /**
         * Gets or sets a CSS class name to use when rendering
         * data (non-header) cells in the column or row.
         */
        cssClass: string;
        /**
         * Gets or sets a CSS class name to use when rendering
         * all cells (data and headers) in the column or row.
         */
        cssClassAll: string;
        /**
         * Gets the {@link FlexGrid} that owns this column or row.
         */
        readonly grid: FlexGrid;
        /**
         * Gets the {@link ICollectionView} bound to this column or row.
         */
        readonly collectionView: wijmo.collections.ICollectionView;
        /**
         * Marks the owner list as dirty and refreshes the owner grid.
         */
        onPropertyChanged(): void;
        /**
         * Occurs when the value of the {@link grid} property changes.
         */
        readonly gridChanged: Event;
        /**
         * Raises the {@link gridChanged} event.
         */
        onGridChanged(e?: wijmo.EventArgs): void;
        _setList(list: RowColCollection): void;
        _getFlag(flag: RowColFlags): boolean;
        _setFlag(flag: RowColFlags, value: boolean, quiet?: boolean): boolean;
    }
    /**
     * Represents a column on the grid.
     */
    class Column extends RowCol {
        private static _ctr;
        private _hdr;
        private _name;
        private _agg;
        private _quickSize;
        private _descById;
        _binding: wijmo.Binding;
        _bindingSort: wijmo.Binding;
        _szStar: string;
        _hash: string;
        /**
         * Initializes a new instance of the {@link Column} class.
         *
         * @param options Initialization options for the column.
         */
        constructor(options?: any);
        /**
         * Gets or sets the name of the column.
         *
         * The column name can be used to retrieve the column using the
         * {@link FlexGrid.getColumn} method.
         */
        name: string;
        /**
         * Gets or sets the name of the property the column is bound to.
         */
        binding: string;
        /**
         * Gets or sets the name of the property to use when sorting this column.
         *
         * Use this property in cases where you want the sorting to be performed
         * based on values other than the ones specified by the {@link binding} property.
         *
         * Setting this property is null causes the grid to use the value of the
         * {@link binding} property to sort the column.
         */
        sortMemberPath: string;
        /**
         * Gets or sets the width of the column.
         *
         * Column widths may be positive numbers (sets the column width in pixels),
         * null or negative numbers (uses the collection's default column width), or
         * strings in the format '{number}*' (star sizing).
         *
         * The star-sizing option performs a XAML-style dynamic sizing where column
         * widths are proportional to the number before the star. For example, if
         * a grid has three columns with widths "100", "*", and "3*", the first column
         * will be 100 pixels wide, the second will take up 1/4th of the remaining
         * space, and the last will take up the remaining 3/4ths of the remaining space.
         *
         * Star-sizing allows you to define columns that automatically stretch to fill
         * the width available. For example, set the width of the last column to "*"
         * and it will automatically extend to fill the entire grid width so there's
         * no empty space. You may also want to set the column's {@link minWidth} property
         * to prevent the column from getting too narrow.
         */
        width: any;
        /**
         * Gets or sets the minimum width of the column.
         *
         * This property is set to null by default, which means there
         * is the minimum width is zero.
         */
        minWidth: number;
        /**
         * Gets or sets the maximum width (in pixels) of the column.
         *
         * This property is set to null by default, which means there
         * is no maximum width.
         */
        maxWidth: number;
        /**
         * Gets or sets a value that determines whether the grid should optimize
         * performance over precision when auto-sizing this column.
         *
         * Setting this property to false disables quick auto-sizing for this column.
         * Setting it to true enables the feature, subject to the value of the grid's
         * {@link wijmo.grid.FlexGrid.quickAutoSize} property.
         * Setting it to null (the default value) enables the feature for columns
         * that display plain text and don't have templates.
         */
        quickAutoSize: boolean;
        private _getQuickAutoSize;
        /**
         * Gets the render width of the column.
         *
         * The value returned takes into account the column's visibility, default size, and min and max sizes.
         */
        readonly renderWidth: number;
        /**
         * Gets or sets the text displayed in the column header.
         */
        header: string;
        /**
         * Gets or sets a value that indicates whether the user can sort the column by clicking its header.
         */
        allowSorting: boolean;
        /**
         * Gets a string that describes the current sorting applied to the column.
         * Possible values are '+' for ascending order, '-' for descending order, or
         * null for unsorted columns.
         */
        readonly currentSort: string;
        /**
         * Gets or sets the {@link Aggregate} to display in the group header rows
         * for the column.
         */
        aggregate: wijmo.Aggregate;
        /**
         * Gets or sets the ID of an element that contains a description
         * of the column.
         *
         * The ID is used as the value of the <b>aria-describedby</b>
         * attribute for the column header element.
         */
        describedById: string;
        /**
         * Gets a value that determines whether values in the column/row are required.
         *
         * Returns the value of the {@link isRequired} property if it is not null, or
         * determines the required status based on the column's {@link dataType}.
         *
         * By default, string columns are not required unless they have an associated
         * {@link dataMap} or {@link mask}; all other data types are required.
         *
         * @param row Row that contains the cell being checked.
         * @return True if the value is required, false otherwise.
         */
        getIsRequired(row?: Row): boolean;
        /**
         * Gets the actual alignment if items in the column or row.
         *
         * Returns the value of the {@link align} property if it is not null, or
         * selects the alignment based on the column's {@link dataType}.
         *
         * @param row Row that contains the cell being checked.
         * @return A string representing the cell alignment.
         */
        getAlignment(row?: Row): string;
        _getBindingSort(): string;
        static _parseStarSize(value: any): number;
    }
    /**
     * Represents a row in the grid.
     */
    class Row extends RowCol {
        private _data;
        _ubv: any;
        /**
         * Initializes a new instance of the {@link Row} class.
         *
         * @param dataItem The data item that this row is bound to.
         */
        constructor(dataItem?: any);
        /**
         * Gets or sets the item in the data collection that the item is bound to.
         */
        dataItem: any;
        /**
         * Gets or sets the height of the row.
         * Setting this property to null or negative values causes the element to use the
         * parent collection's default size.
         */
        height: number;
        /**
         * Gets the render height of the row.
         *
         * The value returned takes into account the row's visibility, default size, and min and max sizes.
         */
        readonly renderHeight: number;
    }
    /**
     * Represents a row that serves as a header for a group of rows.
     */
    class GroupRow extends Row {
        _level: number;
        /**
         * Initializes a new instance of the {@link GroupRow} class.
         */
        constructor();
        /**
         * Gets or sets the hierarchical level of the group associated with the GroupRow.
         */
        level: number;
        /**
         * Gets a value that indicates whether the group row has child rows.
         */
        readonly hasChildren: boolean;
        /**
         * Gets or sets a value that indicates whether the GroupRow is collapsed
         * (child rows are hidden) or expanded (child rows are visible).
         */
        isCollapsed: boolean;
        /**
         * Gets the header text for this {@link GroupRow}.
         */
        getGroupHeader(): string;
        _setCollapsed(collapsed: boolean): void;
        /**
         * Gets a {@link CellRange} object that contains all of the rows in the group represented
         * by this {@link GroupRow} and all of the columns in the grid.
         */
        getCellRange(): CellRange;
    }
    /**
     * Abstract class that serves as a base for row and column collections.
     */
    class RowColCollection extends wijmo.collections.ObservableArray {
        _g: FlexGrid;
        _frozen: number;
        _lastFrozen: number;
        _firstVisible: number;
        _vlen: number;
        _szDef: number;
        _szTot: number;
        _szCustom: boolean;
        _dirty: boolean;
        _szMin: number;
        _szMax: number;
        /**
         * Initializes a new instance of the {@link RowColCollection} class.
         *
         * @param g The {@link FlexGrid} that owns the collection.
         * @param defaultSize The default size of the elements in the collection.
         */
        constructor(g: FlexGrid, defaultSize: number);
        /**
         * Gets the {@link FlexGrid} that owns this collection.
         */
        readonly grid: FlexGrid;
        /**
         * Gets or sets the default size of elements in the collection.
         */
        defaultSize: number;
        /**
         * Gets or sets the number of frozen rows or columns in the collection.
         *
         * Frozen rows and columns do not scroll, and instead remain at the top or left of
         * the grid, next to the fixed cells. Unlike fixed cells, however, frozen
         * cells may be selected and edited like regular cells.
         */
        frozen: number;
        /**
         * Checks whether a column or row is frozen.
         *
         * @param index The index of the column or row to check.
         */
        isFrozen(index: number): boolean;
        /**
         * Gets or sets the minimum size of elements in the collection.
         */
        minSize: number;
        /**
         * Gets or sets the maximum size of elements in the collection.
         */
        maxSize: number;
        /**
         * Gets the total size of the elements in the collection.
         */
        getTotalSize(): number;
        /**
         * Gets the number of visible elements in the collection ({@link Row.isVisible}).
         */
        readonly visibleLength: number;
        /**
         * Gets the index of the element at a given physical position.
         * @param position Position of the item in the collection, in pixels.
         */
        getItemAt(position: number): number;
        /**
         * Finds the next visible cell for a selection change.
         * @param index Starting index for the search.
         * @param move Type of move (size and direction).
         * @param pageSize Size of a page (in case the move is a page up/down).
         */
        getNextCell(index: number, move: SelMove, pageSize: number): any;
        /**
         * Checks whether an element can be moved from one position to another.
         *
         * @param src The index of the element to move.
         * @param dst The position to which to move the element, or specify -1 to append the element.
         * @return Returns true if the move is valid, false otherwise.
         */
        canMoveElement(src: number, dst: number): boolean;
        /**
         * Moves an element from one position to another.
         * @param src Index of the element to move.
         * @param dst Position where the element should be moved to (-1 to append).
         */
        moveElement(src: number, dst: number): void;
        /**
         * Keeps track of dirty state and invalidate grid on changes.
         */
        onCollectionChanged(e?: collections.NotifyCollectionChangedEventArgs): void;
        /**
         * Appends an item to the array.
         *
         * @param item Item to add to the array.
         * @return The new length of the array.
         */
        push(item: any): number;
        /**
         * Removes or adds items to the array.
         *
         * @param index Position where items are added or removed.
         * @param count Number of items to remove from the array.
         * @param item Item to add to the array.
         * @return An array containing the removed elements.
         */
        splice(index: number, count: number, item?: any): any[];
        /**
         * Suspends notifications until the next call to {@link endUpdate}.
         */
        beginUpdate(): void;
        _setDefaultSize(value: number): void;
        _update(): boolean;
    }
    /**
     * Represents a collection of {@link Column} objects in a {@link FlexGrid} control.
     */
    class ColumnCollection extends RowColCollection {
        _descById: string;
        /**
         * Gets a column by name or by binding.
         *
         * The method searches the column by name. If a column with the given name
         * is not found, it searches by binding. The searches are case-sensitive.
         *
         * @param name The name or binding to find.
         * @return The column with the specified name or binding, or null if not found.
         */
        getColumn(name: string): Column;
        /**
         * Gets the index of a column by name or binding.
         *
         * The method searches the column by name. If a column with the given name
         * is not found, it searches by binding. The searches are case-sensitive.
         *
         * @param name The name or binding to find.
         * @return The index of column with the specified name or binding, or -1 if not found.
         */
        indexOf(name: any): number;
        /**
         * Gets or sets the ID of an element that contains a description
         * of the column headers.
         *
         * The ID is used as the value of the <b>aria-describedby</b>
         * attribute for all column header elements. For column-specific
         * descriptions, use the column's {@link Column.describedById} instead.
         */
        describedById: string;
        /**
         * Gets the index of the first visible column (where the outline tree is displayed).
         */
        readonly firstVisibleIndex: number;
        _updateStarSizes(szAvailable: number): boolean;
    }
    /**
     * Represents a collection of {@link Row} objects in a {@link FlexGrid} control.
     */
    class RowCollection extends RowColCollection {
        _maxLevel: number;
        _ariaLabel: string;
        /**
         * Gets or sets a string used as an ARIA label for all rows in this
         * collection.
         *
         * For example, the code below adds ARIA labels to the header and
         * data rows:
         *
         * <pre>
         * grid.rows.ariaLabel = 'data row';
         * grid.columnHeaders.rows.ariaLabel = 'header row';
         * </pre>
         */
        ariaLabel: string;
        /**
         * Gets the maximum group level in the grid.
         *
         * @return The maximum group level or -1 if the grid has no group rows.
         */
        readonly maxGroupLevel: number;
        _update(): boolean;
    }
}
declare module wijmo.grid {
    /**
     * Manages the new row template used to add rows to the grid.
     */
    class _AddNewHandler {
        protected _g: FlexGrid;
        protected _nrt: _NewRowTemplate;
        protected _keydownBnd: any;
        protected _top: boolean;
        protected _committing: boolean;
        /**
         * Initializes a new instance of the {@link _AddNewHandler} class.
         *
         * @param g {@link FlexGrid} that owns this {@link _AddNewHandler}.
         */
        constructor(g: FlexGrid);
        /**
         * Gets or sets a value that indicates whether the new row template
         * should be located at the top of the grid or at the bottom.
         */
        newRowAtTop: boolean;
        /**
         * Updates the new row template to ensure it's visible only if the
         * grid is bound to a data source that supports adding new items,
         * and that it is in the right position.
         */
        updateNewRowTemplate(): void;
        _attach(): void;
        _detach(): void;
        _keydown(e: KeyboardEvent): void;
        _beginningEdit(s: FlexGrid, e: CellRangeEventArgs): void;
        _rowEditEnded(s: FlexGrid, e: CellRangeEventArgs): void;
    }
    /**
     * Represents a row template used to add items to the source collection.
     */
    class _NewRowTemplate extends Row {
    }
}
declare module wijmo.grid {
    /**
     * Specifies constants that define the visibility of row and column headers.
     */
    enum HeadersVisibility {
        /** No header cells are displayed. */
        None = 0,
        /** Only column header cells are displayed. */
        Column = 1,
        /** Only row header cells are displayed. */
        Row = 2,
        /** Both column and row header cells are displayed. */
        All = 3
    }
    /**
     * The {@link FlexGrid} control provides a powerful and flexible way to
     * display and edit data in a tabular format.
     *
     * The {@link FlexGrid} control is a full-featured grid, providing all the
     * features you are used to including several selection modes, sorting,
     * column reordering, grouping, filtering, editing, custom cells,
     * XAML-style star-sizing columns, row and column virtualization, etc.
     *
     * The {@link FlexGrid} control supports the following keyboard commands:
     *
     * <table>
     *   <thead>
     *     <tr><th>Key Combination</th><th>Action</th></tr>
     *   </thead>
     *   <tbody>
     *     <tr><td>Shift + Space</td><td>Select row</td></tr>
     *     <tr><td>Control + Space</td><td>Select column</td></tr>
     *     <tr><td>F2</td><td>Start editing the current cell</td></tr>
     *     <tr><td>Space</td><td>Start editing or toggle checkbox</td></tr>
     *     <tr><td>Control + A</td><td>Select the entire grid contents</td></tr>
     *     <tr><td>Left/Right</td><td>Select the cell to the left/right of the selection, collapse/expand group rows</td></tr>
     *     <tr><td>Shift + Left/Right</td><td>Extend the selection to include the next cell to the left/right of the selection</td></tr>
     *     <tr><td>Up/Down</td><td>Select the next cell above or below the selection</td></tr>
     *     <tr><td>Shift + Up/Down</td><td>Extend the selection to include the cell above or below the selection</td></tr>
     *     <tr><td>Alt + Up/Down</td><td>Drop down the listbox editor for the current cell</td></tr>
     *     <tr><td>PageUp/Down</td><td>Select the cell one page above or below the selection</td></tr>
     *     <tr><td>Shift + PageUp/Down</td><td>Extend the selection to include the cell one page above or below the selection</td></tr>
     *     <tr><td>Alt + PageUp/Down</td><td>Move the selection to the first or last row</td></tr>
     *     <tr><td>Shift + Alt + PageUp/Down</td><td>Extend the selection to include the first or last row</td></tr>
     *     <tr><td>Home/End</td><td>Move the selection to the first or last column</td></tr>
     *     <tr><td>Shift + Home/End</td><td>Extend the selection to include the first or last column</td></tr>
     *     <tr><td>Ctrl + Home/End</td><td>Move the selection to the first/last row and column</td></tr>
     *     <tr><td>Shift + Ctrl + Home/End</td><td>Extend the selection to include the first/last row and column</td></tr>
     *     <tr><td>Escape</td><td>Cancel current cell or row editing operation</td></tr>
     *     <tr><td>Tab</td><td>Move the selection to the next focusable element on the page (by default, can be overridden using the {@link keyActionTab} property)</td></tr>
     *     <tr><td>Enter</td><td>Exit editing mode and move the selection to the cell below the current one (by default, can be overridden using the {@link keyActionEnter} property)</td></tr>
     *     <tr><td>Delete, Backspace</td><td>Delete the currently selected rows (if the {@link allowDelete} property is set to true), or clear the content of the selected cells (if the values are not required).</td></tr>
     *     <tr><td>Control + C or Control + Insert</td><td>Copy the selection to the clipboard (if the {@link autoClipboard} property is set to true)</td></tr>
     *     <tr><td>Control + V or Shift + Insert</td><td>Paste the content of the clipboard into the selected area (if the {@link autoClipboard} property is set to true)</td></tr>
     *   </tbody>
     * </table>
     *
     * {@sample Grid/Overview/purejs Example}
     */
    class FlexGrid extends wijmo.Control {
        static _WJS_STICKY: string;
        static _WJS_MEASURE: string;
        static _WJS_UPDATING: string;
        static _MIN_VIRT_ROWS: number;
        _root: HTMLDivElement;
        _eCt: HTMLDivElement;
        _fCt: HTMLDivElement;
        _eTL: HTMLDivElement;
        _eBL: HTMLDivElement;
        _eFocus: HTMLDivElement;
        _activeCell: HTMLElement;
        _eCHdr: HTMLDivElement;
        _eCFtr: HTMLDivElement;
        _eRHdr: HTMLDivElement;
        _eCHdrCt: HTMLDivElement;
        _eCFtrCt: HTMLDivElement;
        _eRHdrCt: HTMLDivElement;
        _eTLCt: HTMLDivElement;
        _eBLCt: HTMLDivElement;
        _eSz: HTMLDivElement;
        _eMarquee: HTMLDivElement;
        private _gpTL;
        private _gpCHdr;
        private _gpRHdr;
        private _gpCells;
        private _gpBL;
        private _gpCFtr;
        private _maxOffsetY;
        private _heightBrowser;
        _szClient: Size;
        _offsetY: number;
        _lastCount: number;
        _rcBounds: wijmo.Rect;
        _ptScrl: Point;
        _cellPadLeft: number;
        _cellPadVert: number;
        _clipToScreen: boolean;
        _mouseHdl: _MouseHandler;
        _edtHdl: _EditHandler;
        _selHdl: _SelectionHandler;
        _addHdl: _AddNewHandler;
        _keyHdl: _KeyboardHandler;
        _imeHdl: _ImeHandler;
        _mrgMgr: MergeManager;
        protected _autoGenCols: boolean;
        protected _autoClipboard: boolean;
        protected _autoScroll: boolean;
        protected _autoSearch: boolean;
        protected _readOnly: boolean;
        protected _indent: number;
        protected _autoSizeMode: AutoSizeMode;
        protected _quickSize: boolean;
        protected _hdrVis: HeadersVisibility;
        protected _alSorting: boolean;
        protected _alAddNew: boolean;
        protected _alDelete: boolean;
        protected _alResizing: AllowResizing;
        protected _alDragging: AllowDragging;
        protected _alMerging: AllowMerging;
        protected _ssHdr: HeadersVisibility;
        protected _shSort: boolean;
        protected _shGroups: boolean;
        protected _altStep: number;
        protected _shErr: boolean;
        protected _hasValidation: boolean;
        protected _shDropDown: boolean;
        protected _tglDropDown: any;
        protected _valEdt: boolean;
        protected _gHdrFmt: string;
        protected _rows: RowCollection;
        protected _cols: ColumnCollection;
        protected _hdrRows: RowCollection;
        protected _ftrRows: RowCollection;
        protected _hdrCols: ColumnCollection;
        protected _cf: CellFactory;
        protected _itemFormatter: Function;
        protected _items: any;
        protected _cv: wijmo.collections.ICollectionView;
        protected _childItemsPath: any;
        protected _rowHdrPath: wijmo.Binding;
        protected _sortRowIndex: number;
        protected _editColIndex: number;
        protected _deferResizing: boolean;
        protected _errorTip: wijmo.Tooltip;
        protected _pSel: boolean;
        protected _pOutline: boolean;
        protected _stickyHdr: boolean;
        protected _anchorCursor: boolean;
        private _bndSortConverter;
        private _afClip;
        private _afSticky;
        private _toErrorTips;
        private _scrollHandlerAttached;
        private _itemValidator;
        private _fzClone;
        private _vt;
        _vtRows: number;
        _vtCols: number;
        _reorderCells: boolean;
        _refreshOnEdit: boolean;
        _lazyRender: boolean;
        /**
         * Gets or sets the template used to instantiate {@link FlexGrid} controls.
         */
        static controlTemplate: string;
        /**
         * Initializes a new instance of the {@link FlexGrid} class.
         *
         * @param element The DOM element that hosts the control, or a CSS selector for the host element (e.g. '#theCtrl').
         * @param options JavaScript object containing initialization data for the control.
         */
        constructor(element: any, options?: any);
        _handleResize(): void;
        /**
         * Gets or sets a value that determines whether the row and column headers
         * are visible.
         *
         * The default value for this property is <b>HeadersVisibility.All</b>.
         */
        headersVisibility: HeadersVisibility;
        /**
         * Gets or sets a value that determines whether column headers should remain
         * visible when the user scrolls the document.
         *
         * The default value for this property is <b>false</b>.
         */
        stickyHeaders: boolean;
        /**
         * Gets or sets a value that determines whether the grid should preserve
         * the selected state of rows when the data is refreshed.
         *
         * The default value for this property is <b>true</b>.
         */
        preserveSelectedState: boolean;
        /**
         * Gets or sets a value that determines whether the grid should preserve
         * the expanded/collapsed state of nodes when the data is refreshed.
         *
         * The {@link preserveOutlineState} property implementation is based on
         * JavaScript's {@link Map} object, which is not available in IE 9 or 10.
         *
         * The default value for this property is <b>true</b>.
         */
        preserveOutlineState: boolean;
        /**
         * Gets or sets a value that determines whether extending selections
         * with the mouse or keyboard should change the start (cursor) or the
         * end of the current selection.
         *
         * The default value for this property is <b>false</b>, which causes
         * the grid to move the cursor and keep the selection end anchored.
         *
         * Setting this property to <b>true</b> causes the grid to move the
         * selection end and keep the cursor anchored. This is Excel's behavior.
        */
        anchorCursor: boolean;
        /**
         * Gets or sets the minimum number of rows and/or columns required to enable
         * virtualization.
         *
         * This property is set to zero by default, meaning virtualization is always
         * enabled. This improves binding performance and memory requirements, at the
         * expense of a small performance decrease while scrolling.
         *
         * If your grid has a small number of rows (about 50 to 100), you may be able to
         * improve scrolling performance by setting this property to a slightly higher
         * value (like 150). This will disable virtualization and will slow down binding,
         * but may improve perceived scroll performance. For example, the code below sets
         * causes the grid to virtualize cells when the data source has more than 150 items:
         *
         * <pre>// virtualize grid when there are more than 150 items
         * theGrid.virtualizationThreshold = 150;
         * </pre>
         *
         * Setting this property to values higher than 200 is not recommended. Loading
         * times will become too long; the grid will freeze for a few seconds while
         * creating cells for all rows, and the browser will become slow because of
         * the large number of elements on the page.
         *
         * If you want to set separate virtualization thresholds for rows and columns,
         * you may set the {@link virtualizationThreshold} property to an array with two
         * numbers. In this case, the first number will be used as the row threshold
         * and the second as the column threshold. For example, the code below sets
         * causes the grid to virtualize rows but not columns:
         *
         * <pre>// virtualize rows (threshold 0) but not columns (threshold 10,000)
         * theGrid.virtualizationThreshold = [0, 10000];
         * </pre>
         *
         * The default value for this property is <b>0</b>, which virtualizes all
         * rows and columns.
         */
        virtualizationThreshold: any;
        /**
         * Gets or sets a value that determines whether the grid should generate
         * columns automatically based on the {@link itemsSource}.
         *
         * The column generation depends on the {@link itemsSource} property containing
         * at least one item. This data item is inspected and a column is created and
         * bound to each property that contains a primitive value (number, string,
         * Boolean, or Date).
         *
         * Properties set to null do not generate columns, because the grid would
         * have no way of guessing the appropriate type. In this type of scenario,
         * you should set the {@link autoGenerateColumns} property to false and create
         * the columns explicitly. For example:
         *
         * ```typescript
         * import { FlexGrid } from '@grapecity/wijmo.grid';
         * var grid = new FlexGrid('#theGrid', {
         *   autoGenerateColumns: false, // data items may contain null values
         *   columns: [                  // so define columns explicitly
         *     { binding: 'name', header: 'Name', dataType: 'String' },
         *     { binding: 'amount', header: 'Amount', dataType: 'Number' },
         *     { binding: 'date', header: 'Date', dataType: 'Date' },
         *     { binding: 'active', header: 'Active', dataType: 'Boolean' }
         *   ],
         *   itemsSource: customers
         * });
         * ```
         *
         * The default value for this property is <b>true</b>.
         */
        autoGenerateColumns: boolean;
        /**
         * Gets or sets a value that determines whether the grid should handle
         * clipboard shortcuts.
         *
         * The clipboard shortcuts are as follows:
         *
         * <dl class="dl-horizontal">
         *   <dt>ctrl+C, ctrl+Ins</dt>    <dd>Copy grid selection to clipboard.</dd>
         *   <dt>ctrl+V, shift+Ins</dt>   <dd>Paste clipboard text to grid selection.</dd>
         * </dl>
         *
         * Only visible rows and columns are included in clipboard operations.
         *
         * Read-only cells are not affected by paste operations.
         *
         * The default value for this property is <b>true</b>.
         */
        autoClipboard: boolean;
        /**
         * Gets or sets a value that determines whether the grid should automatically
         * scroll its contents while users drag rows or columns into new positions.
         *
         * Row and column dragging are controlled by the {@link allowDragging} property.
         *
         * The default value for this property is <b>true</b>.
         */
        autoScroll: boolean;
        /**
         * Gets or sets a value that determines whether the grid should search for
         * cells as the users types into read-only cells.
         *
         * The search happens on the column that is currently selected, if it is
         * not editable. The search starts at the currently selected row and is
         * case-insensitive.
         *
         * The default value for this property is <b>false</b>.
         */
        autoSearch: boolean;
        /**
         * Gets or sets a JSON string that defines the current column layout.
         *
         * The column layout string represents an array with the columns and their
         * properties. It can be used to persist column layouts defined by users so
         * they are preserved across sessions, and can also be used to implement undo/redo
         * functionality in applications that allow users to modify the column layout.
         *
         * The column layout string does not include <b>dataMap</b> properties because
         * data maps are not serializable.
         */
        columnLayout: string;
        /**
         * Gets or sets a value that determines whether the user can modify
         * cell values using the mouse and keyboard.
         *
         * The default value for this property is <b>false</b>.
         */
        isReadOnly: boolean;
        /**
         * Gets or sets a value that determines whether the grid should support
         * Input Method Editors (IME) while not in edit mode.
         *
         * This property is relevant only for sites/applications in Japanese,
         * Chinese, Korean, and other languages that require IME support.
         *
         * The default value for this property is <b>false</b>.
         */
        imeEnabled: boolean;
        /**
         * Gets or sets a value that determines whether users may resize
         * rows and/or columns with the mouse.
         *
         * If resizing is enabled, users can resize columns by dragging
         * the right edge of column header cells, or rows by dragging the
         * bottom edge of row header cells.
         *
         * Users may also double-click the edge of the header cells to
         * automatically resize rows and columns to fit their content.
         * The auto-size behavior can be customized using the {@link autoSizeMode}
         * property.
         *
         * The default value for this property is <b>AllowResizing.Columns</b>.
         */
        allowResizing: AllowResizing;
        /**
         * Gets or sets a value that determines whether row and column resizing
         * should be deferred until the user releases the mouse button.
         *
         * By default, {@link deferResizing} is set to false, causing rows and columns
         * to be resized as the user drags the mouse. Setting this property to true
         * causes the grid to show a resizing marker and to resize the row or column
         * only when the user releases the mouse button.
         *
         * The default value for this property is <b>false</b>.
         */
        deferResizing: boolean;
        /**
         * Gets or sets which cells should be taken into account when auto-sizing a
         * row or column.
         *
         * This property controls what happens when users double-click the edge of
         * a column header.
         *
         * By default, the grid will automatically set the column width based on the
         * content of the header and data cells in the column. This property allows
         * you to change that to include only the headers or only the data.
         *
         * The default value for this property is <b>AutoSizeMode.Both</b>.
         */
        autoSizeMode: AutoSizeMode;
        /**
         * Gets or sets a value that determines whether the grid should optimize
         * performance over precision when auto-sizing columns.
         *
         * Setting this property to false disables quick auto-sizing. Setting
         * it to true enables the feature, subject to the value of each column's
         * {@link wijmo.grid.Column.quickAutoSize} property. Setting it to null
         * (the default value) enables the feature for grids that don't have a
         * custom {@link itemFormatter} or handlers attached to the {@link formatItem}
         * event.
         */
        quickAutoSize: boolean;
        _getQuickAutoSize(): boolean;
        /**
         * Gets or sets a value that determines whether users are allowed to sort columns
         * by clicking the column header cells.
         *
         * The default value for this property is <b>true</b>.
         */
        allowSorting: boolean;
        /**
         * Gets or sets a value that indicates whether the grid should provide a new row
         * template so users can add items to the source collection.
         *
         * The new row template will not be displayed if the {@link isReadOnly} property
         * is set to true.
         *
         * The default value for this property is <b>false</b>.
         */
        allowAddNew: boolean;
        /**
         * Gets or sets a value that indicates whether the new row template should be
         * located at the top of the grid or at the bottom.
         *
         * If you set the {@link newRowAtTop} property to true, and you want the new
         * row template to remain visible at all times, set the {@link frozenRows}
         * property to one. This will freeze the new row template at the top so
         * it won't scroll off the view.
         *
         * The new row template will be displayed only if the {@link allowAddNew} property
         * is set to true and if the {@link itemsSource} object supports adding new items.
         *
         * The default value for this property is <b>false</b>.
         */
        newRowAtTop: boolean;
        /**
         * Gets or sets a value that indicates whether the grid should delete
         * selected rows when the user presses the Delete key.
         *
         * Selected rows will not be deleted if the {@link isReadOnly} property
         * is set to true.
         *
         * The default value for this property is <b>false</b>.
         */
        allowDelete: boolean;
        /**
         * Gets or sets which parts of the grid provide cell merging.
         *
         * The default value for this property is <b>AllowMerging.None</b>.
         */
        allowMerging: AllowMerging;
        /**
         * Gets or sets a value that indicates whether the grid should
         * add class names to indicate selected header cells.
         *
         * The default value for this property is <b>HeadersVisibility.None</b>.
         */
        showSelectedHeaders: HeadersVisibility;
        /**
         * Gets or sets a value that indicates whether the grid should
         * display an Excel-style marquee around the current selection.
         *
         * The default value for this property is <b>false</b>.
         */
        showMarquee: boolean;
        /**
         * Gets or sets a value that determines whether the grid should display
         * sort indicators in the column headers.
         *
         * Sorting is controlled by the {@link ICollectionView.sortDescriptions}
         * property of the {@link ICollectionView} object used as a the grid's
         * {@link itemsSource}.
         *
         * The default value for this property is <b>true</b>.
         */
        showSort: boolean;
        /**
         * Gets or sets a value that determines whether the {@link FlexGrid} should insert
         * group rows to delimit data groups.
         *
         * Data groups are created by modifying the {@link ICollectionView.groupDescriptions}
         * property of the {@link ICollectionView} object used as an {@link itemsSource}.
         *
         * The default value for this property is <b>true</b>.
         */
        showGroups: boolean;
        /**
         * Gets or sets a value that determines the number of regular rows
         * between 'alternating' rows.
         *
         * The default value for this property is <b>1</b>. Set it to zero
         * to disable alternating rows, or to a number greater than one to
         * insert multiple regular rows between alternating rows.
         */
        alternatingRowStep: number;
        showAlternatingRows: boolean;
        /**
         * Gets or sets a value that determines whether the grid should add the
         * 'wj-state-invalid' class to cells that contain validation errors and
         * tooltips with error descriptions.
         *
         * The grid detects validation errors using the {@link itemValidator}
         * property and the {@link CollectionView.getError} property on the grid's
         * {@link itemsSource}.
         *
         * The default value for this property is <b>true</b>.
         */
        showErrors: boolean;
        /**
         * Gets or sets the {@link Tooltip} object used to show validation
         * errors detected by the grid when the {@link showErrors} property
         * is set to true.
         *
         * By default, this property is set to a tooltip with zero show delay
         * (so it appears immediately when hovering over invalid cells),
         * no HTML content, and a "wj-error-tip" class which can be used to
         * customize the tooltip's appearance.
         *
         * Setting this property to null causes the control to use the cell's
         * "title" attribute to show validation errors.
         */
        errorTip: wijmo.Tooltip;
        /**
         * Gets or sets a validator function to determine whether cells contain
         * valid data.
         *
         * If specified, the validator function should take two parameters containing
         * the cell's row and column indices, and should return a string containing
         * the error description.
         *
         * This property is especially useful when dealing with unbound grids,
         * since bound grids can be validated using the {@link CollectionView.getError}
         * property instead.
         *
         * This example shows how you could prevent cells from containing the same
         * data as the cell immediately above it:
         * <pre>// check that the cell above doesn't contain the same value as this one
         * theGrid.itemValidator = function (row, col) {
         *   if (row &gt; 0) {
         *     var valThis = theGrid.getCellData(row, col, false),
         *         valPrev = theGrid.getCellData(row - 1, col, false);
         *     if (valThis != null && valThis == valPrev) {
         *       return 'This is a duplicate value...'
         *     }
         *   }
         *   return null; // no errors
         * }</pre>
         */
        itemValidator: Function;
        /**
         * Gets or sets a value that determines whether the grid should remain
         * in edit mode when the user tries to commit edits that fail validation.
         *
         * The grid detects validation errors by calling the {@link CollectionView.getError}
         * method on the grid's {@link itemsSource}.
         *
         * The default value for this property is <b>true</b>.
         */
        validateEdits: boolean;
        /**
         * Gets or sets the format string used to create the group header content.
         *
         * The string may contain any text, plus the following replacement strings:
         * <ul>
         *   <li><b>{name}</b>: The name of the property being grouped on.</li>
         *   <li><b>{value}</b>: The value of the property being grouped on.</li>
         *   <li><b>{level}</b>: The group level.</li>
         *   <li><b>{count}</b>: The total number of items in this group.</li>
         * </ul>
         *
         * If a column is bound to the grouping property, the column header is used
         * to replace the <code>{name}</code> parameter, and the column's format and
         * data maps are used to calculate the <code>{value}</code> parameter.
         * If no column is available, the group information is used instead.
         *
         * You may add invisible columns bound to the group properties in order to
         * customize the formatting of the group header cells.
         *
         * The default value for this property is<br/>
         * <code>'{name}: &lt;b&gt;{value}&lt;/b&gt;({count:n0} items)'</code>,
         * which creates group headers similar to<br/>
         * <code>'Country: <b>UK</b> (12 items)'</code> or<br/>
         * <code>'Country: <b>Japan</b> (8 items)'</code>.
         */
        groupHeaderFormat: string;
        /**
         * Gets or sets a value that determines whether users are allowed to drag
         * rows and/or columns with the mouse.
         *
         * If the {@link autoScroll} property is set to true, the grid will automatically
         * scroll its contents while the user drags rows or columns into new positions.
         *
         * The grid allows dragging columns by default.
         *
         * Dragging rows requires special considerations in bound scenarios.
         *
         * When you drag rows on bound grids, the rows will get out of sync with the
         * data source (row 4 may refer to item 6 for example).
         * To avoid this, you should handle the {@link draggedRow} event and
         * synchronize the data with the new row positions.
         *
         * Also, remember to set the {@link allowSorting} property to false or you
         * the row order will be determined by the data, and dragging rows will be
         * pointless.
         *
         * This fiddle demonstrates row dragging with a bound grid:
         * <a href="https://jsfiddle.net/Wijmo5/kyg0qsda/" target="_blank">Bound Row Dragging</a>.
         *
         * The default value for this property is <b>AllowDragging.Columns</b>.
         */
        allowDragging: AllowDragging;
        /**
         * Gets or sets the array or {@link ICollectionView} that contains items
         * shown on the grid.
         */
        itemsSource: any;
        /**
         * Gets the {@link ICollectionView} that contains the grid data.
         *
         * If the {@link itemsSource} property was set to an {@link ICollectionView},
         * this property returns that value.
         *
         * If the {@link itemsSource} property was set to an array of data items,
         * this property returns the internal {@link CollectionView} created
         * by the grid to support currency, editing, and sorting.
         */
        readonly collectionView: wijmo.collections.ICollectionView;
        /**
         * Gets the {@link IEditableCollectionView} that contains the grid data.
         */
        readonly editableCollectionView: wijmo.collections.IEditableCollectionView;
        /**
         * Gets or sets the name of the property (or properties) used to generate
         * child rows in hierarchical grids.
         *
         * Set this property to a string to specify the name of the property that
         * contains an item's child items
         * (e.g. <code>childItemsPath = 'items';</code>).
         *
         * If items at different levels have child items with different names,
         * set this property to an array containing the names of the properties
         * that contain child items et each level
         * (e.g. <code>childItemsPath = ['checks','earnings'];</code>).
         *
         * {@sample Grid/TreeGrid/ChildItems/purejs Example}
         */
        childItemsPath: any;
        /**
         * Gets or sets the name of the property used to create row header
         * cells.
         *
         * Row header cells are not visible or selectable. They are meant
         * for use with accessibility tools.
         */
        rowHeaderPath: string;
        /**
         * Gets the {@link GridPanel} that contains the data cells.
         */
        readonly cells: GridPanel;
        /**
         * Gets the {@link GridPanel} that contains the column header cells.
         */
        readonly columnHeaders: GridPanel;
        /**
         * Gets the {@link GridPanel} that contains the column footer cells.
         *
         * The {@link columnFooters} panel appears below the grid cells, to the
         * right of the {@link bottomLeftCells} panel. It can be used to display
         * summary information below the grid data.
         *
         * The example below shows how you can add a row to the {@link columnFooters}
         * panel to display summary data for columns that have the
         * {@link Column.aggregate} property set:
         *
         * <pre>function addFooterRow(flex) {
         *   // create a GroupRow to show aggregates
         *   var row = new wijmo.grid.GroupRow();
         *
         *   // add the row to the column footer panel
         *   flex.columnFooters.rows.push(row);
         *
         *   // show a sigma on the header
         *   flex.bottomLeftCells.setCellData(0, 0, '\u03A3');
         * }</pre>
         */
        readonly columnFooters: GridPanel;
        /**
         * Gets the {@link GridPanel} that contains the row header cells.
         */
        readonly rowHeaders: GridPanel;
        /**
         * Gets the {@link GridPanel} that contains the top left cells
         * (to the left of the column headers).
         */
        readonly topLeftCells: GridPanel;
        /**
         * Gets the {@link GridPanel} that contains the bottom left cells.
         *
         * The {@link bottomLeftCells} panel appears below the row headers, to the
         * left of the {@link columnFooters} panel.
         */
        readonly bottomLeftCells: GridPanel;
        /**
         * Gets the grid's row collection.
         */
        readonly rows: RowCollection;
        /**
         * Gets the grid's column collection.
         */
        readonly columns: ColumnCollection;
        /**
         * Gets a column by name or by binding.
         *
         * The method searches the column by name. If a column with the given name
         * is not found, it searches by binding. The searches are case-sensitive.
         *
         * @param name The name or binding to find.
         * @return The column with the specified name or binding, or null if not found.
         */
        getColumn(name: string): Column;
        /**
         * Gets or sets the number of frozen rows.
         *
         * Frozen rows do not scroll vertically, but the cells they contain
         * may be selected and edited.
         *
         * The default value for this property is <b>0</b>.
         */
        frozenRows: number;
        /**
         * Gets or sets the number of frozen columns.
         *
         * Frozen columns do not scroll horizontally, but the cells they contain
         * may be selected and edited.
         *
         * The default value for this property is <b>0</b>.
         */
        frozenColumns: number;
        /**
         * Gets or sets a value that determines whether the FlexGrid should
         * clone frozen cells and show then in a separate element to reduce
         * flicker while scrolling.
         *
         * The default value for this property is <b>null</b>, which causes
         * the grid to select the best setting depending on the browser.
         */
        cloneFrozenCells: boolean;
        /**
         * Gets or sets the index of row in the column header panel that
         * shows and changes the current sort.
         *
         * The default value for this property is <b>null</b>,
         * which causes the bottom row in the {@link columnHeaders}
         * panel to act as the sort row.
         */
        sortRowIndex: number;
        /**
         * Gets or sets the index of column in the row header panel that
         * shows whether items are being edited.
         *
         * The default value for this property is <b>null</b>, which cause
         * the grid to show the edit glyph on the last column of the
         * {@link rowHeaders} panel.
         */
        editColumnIndex: number;
        /**
         * Gets or sets a {@link Point} that represents the value of the grid's scrollbars.
         */
        scrollPosition: wijmo.Point;
        /**
         * Gets the client size of the control (control size minus headers and scrollbars).
         */
        readonly clientSize: wijmo.Size;
        /**
         * Gets the bounding rectangle of the control in page coordinates.
         */
        readonly controlRect: wijmo.Rect;
        /**
         * Gets the size of the grid content in pixels.
         */
        readonly scrollSize: wijmo.Size;
        /**
         * Gets the range of cells currently in view.
         */
        readonly viewRange: CellRange;
        /**
         * Gets or sets the {@link CellFactory} that creates and updates cells for this grid.
         */
        cellFactory: CellFactory;
        /**
         * Gets or sets a formatter function used to customize cells on this grid.
         *
         * The formatter function can add any content to any cell. It provides
         * complete flexibility over the appearance and behavior of grid cells.
         *
         * If specified, the function should take four parameters: the {@link GridPanel}
         * that contains the cell, the row and column indices of the cell, and the
         * HTML element that represents the cell. The function will typically change
         * the <b>innerHTML</b> property of the cell element.
         *
         * For example:
         * <pre>
         * flex.itemFormatter = function(panel, r, c, cell) {
         *   if (panel.cellType == CellType.Cell) {
         *     // draw sparklines in the cell
         *     var col = panel.columns[c];
         *     if (col.name == 'sparklines') {
         *       cell.innerHTML = getSparklike(panel, r, c);
         *     }
         *   }
         * }
         * </pre>
         *
         * Note that the FlexGrid recycles cells, so if your {@link itemFormatter}
         * modifies the cell's style attributes, you must make sure that it resets
         * these attributes for cells that should not have them. For example:
         *
         * <pre>
         * flex.itemFormatter = function(panel, r, c, cell) {
         *   // reset attributes we are about to customize
         *   var s = cell.style;
         *   s.color = '';
         *   s.backgroundColor = '';
         *   // customize color and backgroundColor attributes for this cell
         *   ...
         * }
         * </pre>
         *
         * If you have a scenario where multiple clients may want to customize the
         * grid rendering (for example when creating directives or re-usable libraries),
         * consider using the {@link formatItem} event instead. The event allows multiple
         * clients to attach their own handlers.
         */
        itemFormatter: Function;
        /**
         * Gets a value that indicates whether a given cell can be edited.
         *
         * @param r Index of the row that contains the cell.
         * @param c Index of the column that contains the cell.
         */
        canEditCell(r: number, c: number): boolean;
        /**
         * Gets the value stored in a cell in the scrollable area of the grid.
         *
         * @param r Index of the row that contains the cell.
         * @param c Index of the column that contains the cell.
         * @param formatted Whether to format the value for display.
         */
        getCellData(r: number, c: number, formatted: boolean): any;
        /**
         * Gets a the bounds of a cell element in viewport coordinates.
         *
         * This method returns the bounds of cells in the {@link cells}
         * panel (scrollable data cells). To get the bounds of cells
         * in other panels, use the {@link getCellBoundingRect} method
         * in the appropriate {@link GridPanel} object.
         *
         * The returned value is a {@link Rect} object which contains the
         * position and dimensions of the cell in viewport coordinates.
         * The viewport coordinates are the same used by the
         * <a href="https://developer.mozilla.org/en-US/docs/Web/API/Element.getBoundingClientRect">getBoundingClientRect</a>
         * method.
         *
         * @param r Index of the row that contains the cell.
         * @param c Index of the column that contains the cell.
         * @param raw Whether to return the rectangle in raw panel coordinates
         * as opposed to viewport coordinates.
         */
        getCellBoundingRect(r: number, c: number, raw?: boolean): wijmo.Rect;
        /**
         * Sets the value of a cell in the scrollable area of the grid.
         *
         * @param r Index of the row that contains the cell.
         * @param c Index, name, or binding of the column that contains the cell.
         * @param value Value to store in the cell.
         * @param coerce Whether to change the value automatically to match the column's data type.
         * @param invalidate Whether to invalidate the grid to show the change.
         * @return True if the value was stored successfully, false otherwise.
         */
        setCellData(r: number, c: any, value: any, coerce?: boolean, invalidate?: boolean): boolean;
        /**
         * Gets a {@link wijmo.grid.HitTestInfo} object with information about a given point.
         *
         * For example:
         *
         * <pre>// hit test a point when the user clicks on the grid
         * flex.hostElement.addEventListener('click', function (e) {
         *   var ht = flex.hitTest(e.pageX, e.pageY);
         *   console.log('you clicked a cell of type "' +
         *     wijmo.grid.CellType[ht.cellType] + '".');
         * });</pre>
         *
         * @param pt {@link Point} to investigate, in page coordinates, or a MouseEvent object, or x coordinate of the point.
         * @param y Y coordinate of the point in page coordinates (if the first parameter is a number).
         * @return A {@link wijmo.grid.HitTestInfo} object with information about the point.
         */
        hitTest(pt: any, y?: any): HitTestInfo;
        /**
         * Gets the content of a {@link CellRange} as a string suitable for
         * copying to the clipboard.
         *
         * Hidden rows and columns are not included in the clip string.
         *
         * @param rng {@link CellRange} to copy. If omitted, the current selection is used.
         * @param csv Whether to use the CSV format (comma-delimited cells).
         * @param colHdrs Whether to include the column headers.
         * @param rowHdrs Whether to include the row headers.
         */
        getClipString(rng?: CellRange, csv?: boolean, colHdrs?: boolean, rowHdrs?: boolean): string;
        /**
         * Parses a string into rows and columns and applies the content to a given range.
         *
         * Hidden rows and columns are skipped.
         *
         * @param text Tab and newline delimited text to parse into the grid.
         * @param rng {@link CellRange} to copy. If omitted, the current selection is used.
         */
        setClipString(text: string, rng?: CellRange): void;
        /**
         * Overridden to set the focus to the grid without scrolling the
         * whole grid into view.
         */
        focus(): void;
        /**
         * Disposes of the control by removing its association with the host element.
         */
        dispose(): void;
        /**
         * Refreshes the grid display.
         *
         * @param fullUpdate Whether to update the grid layout and content, or just the content.
         */
        refresh(fullUpdate?: boolean): void;
        /**
         * Refreshes the grid display.
         *
         * @param fullUpdate Whether to update the grid layout and content, or just the content.
         * @param recycle Whether to recycle existing elements.
         * @param state Whether to keep existing elements and update their state.
         */
        refreshCells(fullUpdate: boolean, recycle?: boolean, state?: boolean): void;
        /**
         * Refreshes the cells in a range, updating their content and styles.
         *
         * Unlike the {@link refreshCells} method, which updates all the cells,
         * {@link refreshRange} allows you to specify which cells should be
         * refreshed, which in some cases can improve performance.
         *
         * @param rng {@link CellRange} to be refreshed.
         */
        refreshRange(rng: CellRange): void;
        /**
         * Resizes a column to fit its content.
         *
         * This method only works if the grid is visible. If its host element
         * has not been added to the DOM, or if any of the grid's ancestor
         * elements is hidden, the grid will not be able to measure the cells
         * and therefore will not be able to auto-size the columns.
         *
         * @param c Index of the column to resize.
         * @param header Whether the column index refers to a regular or a header row.
         * @param extra Extra spacing, in pixels.
         */
        autoSizeColumn(c: number, header?: boolean, extra?: number): void;
        /**
         * Resizes a range of columns to fit their content.
         *
         * The grid will always measure all rows in the current view range, plus up
         * to 2,000 rows not currently in view. If the grid contains a large amount
         * of data (say 50,000 rows),  then not all rows will be measured since that
         * could take a long time.
         *
         * This method only works if the grid is visible. If its host element has not
         * been added to the DOM, or if any of the grid's ancestor elements is hidden,
         * the grid will not be able to measure the cells and therefore will not be
         * able to auto-size the columns.
         *
         * @param firstColumn Index of the first column to resize (defaults to the first column).
         * @param lastColumn Index of the last column to resize (defaults to the last column).
         * @param header Whether the column indices refer to regular or header columns.
         * @param extra Extra spacing, in pixels.
         */
        autoSizeColumns(firstColumn?: number, lastColumn?: number, header?: boolean, extra?: number): void;
        /**
         * Resizes a row to fit its content.
         *
         * This method only works if the grid is visible. If its host element
         * has not been added to the DOM, or if any of the grid's ancestor
         * elements are hidden, the grid will not be able to measure the cells
         * and therefore will not be able to auto-size the rows.
         *
         * @param r Index of the row to resize.
         * @param header Whether the row index refers to a regular or a header row.
         * @param extra Extra spacing, in pixels.
         */
        autoSizeRow(r: number, header?: boolean, extra?: number): void;
        /**
         * Resizes a range of rows to fit their content.
         *
         * This method only works if the grid is visible. If its host element
         * has not been added to the DOM, or if any of the grid's ancestor
         * elements is hidden, the grid will not be able to measure the cells
         * and therefore will not be able to auto-size the rows.
         *
         * @param firstRow Index of the first row to resize.
         * @param lastRow Index of the last row to resize.
         * @param header Whether the row indices refer to regular or header rows.
         * @param extra Extra spacing, in pixels.
         */
        autoSizeRows(firstRow?: number, lastRow?: number, header?: boolean, extra?: number): void;
        /**
         * Gets or sets the indent used to offset row groups of different levels.
         */
        treeIndent: number;
        /**
         * Collapses all the group rows to a given level.
         *
         * @param level Maximum group level to show.
         */
        collapseGroupsToLevel(level: number): void;
        /**
         * Gets or sets the current selection mode.
         */
        selectionMode: SelectionMode;
        /**
         * Gets or sets the current selection.
         */
        selection: CellRange;
        /**
         * Selects a cell range and optionally scrolls it into view.
         *
         * The {@link select} method can be called by passing a {@link CellRange} and
         * an optional boolean parameter that indicates whether the new selection
         * should be scrolled into view. For example:
         *
         * <pre>// select cell 1,1 and scroll it into view
         * grid.select(new CellRange(1, 1), true);
         *
         * // select range (1,1)-(2,4) and do not scroll it into view
         * grid.select(new CellRange(1, 1, 2, 4), false);
         * </pre>
         *
         * You can also call the {@link select} method passing the index or the
         * row and column you want to select. In this case, the new selection
         * always scrolls into view. For example:
         *
         * <pre>// select cell 1,1 and scroll it into view
         * grid.select(1, 1);
         * </pre>
         *
         * @param rng Range to select.
         * @param show Whether to scroll the new selection into view.
         */
        select(rng: any, show?: any): void;
        /**
         * Gets a {@link SelectedState} value that indicates the selected state of a cell.
         *
         * @param r Row index of the cell to inspect.
         * @param c Column index of the cell to inspect.
         */
        getSelectedState(r: number, c: number): SelectedState;
        /**
         * Gets or sets an array containing the rows that are currently selected.
         *
         * Note: this property can be read in all selection modes, but it can be
         * set only when {@link selectionMode} is set to <b>SelectionMode.ListBox</b>.
         */
        selectedRows: any[];
        /**
         * Gets or sets an array containing the data items that are currently selected.
         *
         * Note: this property can be read in all selection modes, but it can be
         * set only when {@link selectionMode} is set to <b>SelectionMode.ListBox</b>.
         */
        selectedItems: any[];
        /**
         * Scrolls the grid to bring a specific cell into view.
         *
         * Negative row and column indices are ignored, so if you call
         *
         * <pre>grid.scrollIntoView(200, -1);</pre>
         *
         * The grid will scroll vertically to show row 200, and will not
         * scroll horizontally.
         *
         * @param r Index of the row to scroll into view.
         * @param c Index of the column to scroll into view.
         * @param refresh Optional parameter that determines whether the grid
         * should refresh to show the new scroll position immediately.
         * @return True if the grid scrolled.
         */
        scrollIntoView(r: number, c: number, refresh?: boolean): boolean;
        /**
         * Checks whether a given CellRange is valid for this grid's row and column collections.
         *
         * @param rng Range to check.
         */
        isRangeValid(rng: CellRange): boolean;
        /**
         * Starts editing a given cell.
         *
         * Editing in the {@link FlexGrid} is similar to editing in Excel:
         * Pressing F2 or double-clicking a cell puts the grid in <b>full-edit</b> mode.
         * In this mode, the cell editor remains active until the user presses Enter, Tab,
         * or Escape, or until he moves the selection with the mouse. In full-edit mode,
         * pressing the cursor keys does not cause the grid to exit edit mode.
         *
         * Typing text directly into a cell puts the grid in <b>quick-edit mode</b>.
         * In this mode, the cell editor remains active until the user presses Enter,
         * Tab, or Escape, or any arrow keys.
         *
         * Full-edit mode is normally used to make changes to existing values.
         * Quick-edit mode is normally used for entering new data quickly.
         *
         * While editing, the user can toggle between full and quick modes by
         * pressing the F2 key.
         *
         * @param fullEdit Whether to stay in edit mode when the user presses the cursor keys. Defaults to true.
         * @param r Index of the row to be edited. Defaults to the currently selected row.
         * @param c Index of the column to be edited. Defaults to the currently selected column.
         * @param focus Whether to give the editor the focus when editing starts. Defaults to true.
         * @return True if the edit operation started successfully.
         */
        startEditing(fullEdit?: boolean, r?: number, c?: number, focus?: boolean): boolean;
        /**
         * Commits any pending edits and exits edit mode.
         *
         * @param cancel Whether pending edits should be canceled or committed.
         * @return True if the edit operation finished successfully.
         */
        finishEditing(cancel?: boolean): boolean;
        /**
         * Gets the <b>HTMLInputElement</b> that represents the cell editor currently active.
         */
        readonly activeEditor: HTMLInputElement;
        /**
         * Gets a {@link CellRange} that identifies the cell currently being edited.
         */
        readonly editRange: CellRange;
        /**
         * Gets or sets the {@link MergeManager} object responsible for determining how cells
         * should be merged.
         */
        mergeManager: MergeManager;
        /**
         * Gets a {@link CellRange} that specifies the merged extent of a cell
         * in a {@link GridPanel}.
         *
         * @param p The {@link GridPanel} that contains the range.
         * @param r Index of the row that contains the cell.
         * @param c Index of the column that contains the cell.
         * @param clip Whether to clip the merged range to the grid's current view range.
         * @return A {@link CellRange} that specifies the merged range, or null if the cell is not merged.
         */
        getMergedRange(p: GridPanel, r: number, c: number, clip?: boolean): CellRange;
        /**
         * Gets or sets the action to perform when the TAB key is pressed.
         *
         * The default setting for this property is {@link KeyAction.None},
         * which causes the browser to select the next or previous controls
         * on the page when the TAB key is pressed. This is the recommended
         * setting to improve page accessibility.
         *
         * In previous versions, the default was set to {@link KeyAction.Cycle},
         * which caused the control to move the selection across and down
         * the grid. This is the standard Excel behavior, but is not good
         * for accessibility.
         *
         * There is also a {@link KeyAction.CycleOut} setting that causes the
         * selection to move through the cells (as {@link KeyAction.Cycle}),
         * and then on to the next/previous control on the page when the
         * last or first cells are selected.
         */
        keyActionTab: KeyAction;
        /**
         * Gets or sets the action to perform when the ENTER key is pressed.
         *
         * The default setting for this property is {@link KeyAction.MoveDown},
         * which causes the control to move the selection to the next row.
         * This is the standard Excel behavior.
         */
        keyActionEnter: KeyAction;
        /**
         * Gets or sets a value that indicates whether the grid adds drop-down
         * buttons to the cells in columns that have the {@link Column.showDropDown}
         * property set to true.
         *
         * The drop-down buttons are shown only on columns that have a {@link Column.dataMap}
         * set and are editable. Clicking on the drop-down buttons causes the grid
         * to show a list where users can select the value for the cell.
         *
         * Cell drop-downs require the wijmo.input module to be loaded.
         */
        showDropDown: boolean;
        /**
         * Toggles the drop-down list-box associated with the currently selected
         * cell.
         *
         * This method can be used to show the drop-down list automatically
         * when the cell enters edit mode, or when the user presses certain
         * keys.
         *
         * For example, this code causes the grid to show the drop-down list
         * whenever the grid enters edit mode:
         * <pre>// show the drop-down list when the grid enters edit mode
         * theGrid.beginningEdit = function () {
         *   theGrid.toggleDropDownList();
         * }</pre>
         *
         * This code causes the grid to show the drop-down list when the grid
         * enters edit mode after the user presses the space bar:
         * <pre> // show the drop-down list when the user presses the space bar
         * theGrid.hostElement.addEventListener('keydown', function (e) {
         *   if (e.keyCode == 32) {
         *     e.preventDefault();
         *     theGrid.toggleDropDownList();
         *   }
         * }, true);</pre>
         */
        toggleDropDownList(): void;
        /**
         * Occurs before the grid is bound to a new items source.
         */
        readonly itemsSourceChanging: Event;
        /**
         * Raises the {@link itemsSourceChanging} event.
         *
         * @param e {@link CancelEventArgs} that contains the event data.
         * @return True if the event was not canceled.
         */
        onItemsSourceChanging(e: wijmo.CancelEventArgs): boolean;
        /**
         * Occurs after the grid has been bound to a new items source.
         */
        readonly itemsSourceChanged: Event;
        /**
         * Raises the {@link itemsSourceChanged} event.
         */
        onItemsSourceChanged(e?: wijmo.EventArgs): void;
        /**
         * Occurs after the control has scrolled.
         */
        readonly scrollPositionChanged: Event;
        /**
         * Raises the {@link scrollPositionChanged} event.
         */
        onScrollPositionChanged(e?: wijmo.EventArgs): void;
        /**
         * Occurs before selection changes.
         */
        readonly selectionChanging: Event;
        /**
         * Raises the {@link selectionChanging} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         * @return True if the event was not canceled.
         */
        onSelectionChanging(e: CellRangeEventArgs): boolean;
        /**
         * Occurs after selection changes.
         */
        readonly selectionChanged: Event;
        /**
         * Raises the {@link selectionChanged} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         */
        onSelectionChanged(e: CellRangeEventArgs): void;
        /**
         * Occurs before the grid rows are bound to items in the data source.
         */
        readonly loadingRows: Event;
        /**
         * Raises the {@link loadingRows} event.
         *
         * @param e {@link CancelEventArgs} that contains the event data.
         * @return True if the event was not canceled.
         */
        onLoadingRows(e: wijmo.CancelEventArgs): boolean;
        /**
         * Occurs after the grid rows have been bound to items in the data source.
         */
        readonly loadedRows: Event;
        /**
         * Raises the {@link loadedRows} event.
         */
        onLoadedRows(e?: wijmo.EventArgs): void;
        /**
         * Occurs before the grid updates its internal layout.
         */
        readonly updatingLayout: Event;
        /**
         * Raises the {@link updatingLayout} event.
         *
         * @param e {@link CancelEventArgs} that contains the event data.
         * @return True if the event was not canceled.
         */
        onUpdatingLayout(e: wijmo.CancelEventArgs): boolean;
        /**
         * Occurs after the grid has updated its internal layout.
         */
        readonly updatedLayout: Event;
        /**
         * Raises the {@link updatedLayout} event.
         */
        onUpdatedLayout(e?: wijmo.EventArgs): void;
        /**
         * Occurs as columns are resized.
         */
        readonly resizingColumn: Event;
        /**
         * Raises the {@link resizingColumn} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         * @return True if the event was not canceled.
         */
        onResizingColumn(e: CellRangeEventArgs): boolean;
        /**
         * Occurs when the user finishes resizing a column.
         */
        readonly resizedColumn: Event;
        /**
         * Raises the {@link resizedColumn} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         */
        onResizedColumn(e: CellRangeEventArgs): void;
        /**
         * Occurs before the user auto-sizes a column by double-clicking the
         * right edge of a column header cell.
         */
        readonly autoSizingColumn: Event;
        /**
         * Raises the {@link autoSizingColumn} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         * @return True if the event was not canceled.
         */
        onAutoSizingColumn(e: CellRangeEventArgs): boolean;
        /**
         * Occurs after the user auto-sizes a column by double-clicking the
         * right edge of a column header cell.
         */
        readonly autoSizedColumn: Event;
        /**
         * Raises the {@link autoSizedColumn} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         */
        onAutoSizedColumn(e: CellRangeEventArgs): void;
        /**
         * Occurs when the user starts dragging a column.
         */
        readonly draggingColumn: Event;
        /**
         * Raises the {@link draggingColumn} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         * @return True if the event was not canceled.
         */
        onDraggingColumn(e: CellRangeEventArgs): boolean;
        /**
         * Occurs as the user drags a column to a new position.
         *
         * The handler may cancel the event to prevent users from
         * dropping columns at certain positions. For example:
         *
         * <pre>// remember column being dragged
         * flex.draggingColumn.addHandler(function (s, e) {
         *     theColumn = s.columns[e.col].binding;
         * });
         *
         * // prevent 'sales' column from being dragged to index 0
         * s.draggingColumnOver.addHandler(function (s, e) {
         *     if (theColumn == 'sales' && e.col == 0) {
         *         e.cancel = true;
         *     }
         * });</pre>
         */
        readonly draggingColumnOver: Event;
        /**
         * Raises the {@link draggingColumnOver} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         * @return True if the event was not canceled.
         */
        onDraggingColumnOver(e: CellRangeEventArgs): boolean;
        /**
         * Occurs when the user finishes dragging a column.
         */
        readonly draggedColumn: Event;
        /**
         * Raises the {@link draggedColumn} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         */
        onDraggedColumn(e: CellRangeEventArgs): void;
        /**
         * Occurs as rows are resized.
         */
        readonly resizingRow: Event;
        /**
         * Raises the {@link resizingRow} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         * @return True if the event was not canceled.
         */
        onResizingRow(e: CellRangeEventArgs): boolean;
        /**
         * Occurs when the user finishes resizing rows.
         */
        readonly resizedRow: Event;
        /**
         * Raises the {@link resizedRow} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         */
        onResizedRow(e: CellRangeEventArgs): void;
        /**
         * Occurs before the user auto-sizes a row by double-clicking the
         * bottom edge of a row header cell.
         */
        readonly autoSizingRow: Event;
        /**
         * Raises the {@link autoSizingRow} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         * @return True if the event was not canceled.
         */
        onAutoSizingRow(e: CellRangeEventArgs): boolean;
        /**
         * Occurs after the user auto-sizes a row by double-clicking the
         * bottom edge of a row header cell.
         */
        readonly autoSizedRow: Event;
        /**
         * Raises the {@link autoSizedRow} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         */
        onAutoSizedRow(e: CellRangeEventArgs): void;
        /**
         * Occurs when the user starts dragging a row.
         */
        readonly draggingRow: Event;
        /**
         * Raises the {@link draggingRow} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         * @return True if the event was not canceled.
         */
        onDraggingRow(e: CellRangeEventArgs): boolean;
        /**
         * Occurs as the user drags a row to a new position.
         */
        readonly draggingRowOver: Event;
        /**
         * Raises the {@link draggingRowOver} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         * @return True if the event was not canceled.
         */
        onDraggingRowOver(e: CellRangeEventArgs): boolean;
        /**
         * Occurs when the user finishes dragging a row.
         */
        readonly draggedRow: Event;
        /**
         * Raises the {@link draggedRow} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         */
        onDraggedRow(e: CellRangeEventArgs): void;
        /**
         * Occurs when a group is about to be expanded or collapsed.
         */
        readonly groupCollapsedChanging: Event;
        /**
         * Raises the {@link groupCollapsedChanging} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         * @return True if the event was not canceled.
         */
        onGroupCollapsedChanging(e: CellRangeEventArgs): boolean;
        /**
         * Occurs after a group has been expanded or collapsed.
         */
        readonly groupCollapsedChanged: Event;
        /**
         * Raises the {@link groupCollapsedChanged} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         */
        onGroupCollapsedChanged(e: CellRangeEventArgs): void;
        /**
         * Occurs before the user applies a sort by clicking on a column header.
         */
        readonly sortingColumn: Event;
        /**
         * Raises the {@link sortingColumn} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         * @return True if the event was not canceled.
         */
        onSortingColumn(e: CellRangeEventArgs): boolean;
        /**
         * Occurs after the user applies a sort by clicking on a column header.
         */
        readonly sortedColumn: Event;
        /**
         * Raises the {@link sortedColumn} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         */
        onSortedColumn(e: CellRangeEventArgs): void;
        /**
         * Occurs before a cell enters edit mode.
         */
        readonly beginningEdit: Event;
        /**
         * Raises the {@link beginningEdit} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         * @return True if the event was not canceled.
         */
        onBeginningEdit(e: CellRangeEventArgs): boolean;
        /**
         * Occurs when an editor cell is created and before it becomes active.
         */
        readonly prepareCellForEdit: Event;
        /**
         * Raises the {@link prepareCellForEdit} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         */
        onPrepareCellForEdit(e: CellRangeEventArgs): void;
        /**
         * Occurs when a cell edit is ending.
         *
         * You can use this event to perform validation and prevent invalid edits.
         * For example, the code below prevents users from entering values that
         * do not contain the letter 'a'. The code demonstrates how you can obtain
         * the old and new values before the edits are applied.
         *
         * <pre>function cellEditEnding (sender, e) {
         *   // get old and new values
         *   var flex = sender,
         *       oldVal = flex.getCellData(e.row, e.col),
         *       newVal = flex.activeEditor.value;
         *   // cancel edits if newVal doesn't contain 'a'
         *   e.cancel = newVal.indexOf('a') &lt; 0;
         * }</pre>
         *
         * Setting the {@link CellEditEndingEventArgs.cancel} parameter to
         * true causes the grid to discard the edited value and keep the
         * cell's original value.
         *
         * If you also set the {@link CellEditEndingEventArgs.stayInEditMode}
         * parameter to true, the grid will remain in edit mode so the user
         * can correct invalid entries before committing the edits.
         */
        readonly cellEditEnding: Event;
        /**
         * Raises the {@link cellEditEnding} event.
         *
         * @param e {@link CellEditEndingEventArgs} that contains the event data.
         * @return True if the event was not canceled.
         */
        onCellEditEnding(e: CellEditEndingEventArgs): boolean;
        /**
         * Occurs when a cell edit has been committed or canceled.
         */
        readonly cellEditEnded: Event;
        /**
         * Raises the {@link cellEditEnded} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         */
        onCellEditEnded(e: CellRangeEventArgs): void;
        /**
         * Occurs before a row enters edit mode.
         */
        readonly rowEditStarting: Event;
        /**
         * Raises the {@link rowEditStarting} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         */
        onRowEditStarting(e: CellRangeEventArgs): void;
        /**
         * Occurs after a row enters edit mode.
         */
        readonly rowEditStarted: Event;
        /**
         * Raises the {@link rowEditStarted} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         */
        onRowEditStarted(e: CellRangeEventArgs): void;
        /**
         * Occurs when a row edit is ending, before the changes are committed or canceled.
         *
         * This event can be used in conjunction with the {@link rowEditStarted} event to
         * implement deep-binding edit undos. For example:
         *
         * <pre>// save deep bound values when editing starts
         * var itemData = {};
         * s.rowEditStarted.addHandler(function (s, e) {
         *   var item = s.collectionView.currentEditItem;
         *   itemData = {};
         *   s.columns.forEach(function (col) {
         *     if (col.binding.indexOf('.') &gt; -1) { // deep binding
         *       var binding = new wijmo.Binding(col.binding);
         *       itemData[col.binding] = binding.getValue(item);
         *     }
         *   })
         * });
         *
         * // restore deep bound values when edits are canceled
         * s.rowEditEnded.addHandler(function (s, e) {
         *   if (e.cancel) { // edits were canceled by the user
         *     var item = s.collectionView.currentEditItem;
         *     for (var k in itemData) {
         *       var binding = new wijmo.Binding(k);
         *       binding.setValue(item, itemData[k]);
         *     }
         *   }
         *   itemData = {};
         * });</pre>
         */
        readonly rowEditEnding: Event;
        /**
         * Raises the {@link rowEditEnding} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         */
        onRowEditEnding(e: CellRangeEventArgs): void;
        /**
         * Occurs when a row edit has been committed or canceled.
         */
        readonly rowEditEnded: Event;
        /**
         * Raises the {@link rowEditEnded} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         */
        onRowEditEnded(e: CellRangeEventArgs): void;
        /**
         * Occurs when the user creates a new item by editing the new row template
         * (see the {@link allowAddNew} property).
         *
         * The event handler may customize the content of the new item or cancel
         * the new item creation.
         */
        readonly rowAdded: Event;
        /**
         * Raises the {@link rowAdded} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         * @return True if the event was not canceled.
         */
        onRowAdded(e: CellRangeEventArgs): boolean;
        /**
         * Occurs when the user is deleting a selected row by pressing the Delete
         * key (see the {@link allowDelete} property).
         *
         * The event handler may cancel the row deletion.
         */
        readonly deletingRow: Event;
        /**
         * Raises the {@link deletingRow} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         * @return True if the event was not canceled.
         */
        onDeletingRow(e: CellRangeEventArgs): boolean;
        /**
         * Occurs after the user has deleted a row by pressing the Delete
         * key (see the {@link allowDelete} property).
         */
        readonly deletedRow: Event;
        /**
         * Raises the {@link deletedRow} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         */
        onDeletedRow(e: CellRangeEventArgs): void;
        /**
         * Occurs when the user is copying the selection content to the
         * clipboard by pressing one of the clipboard shortcut keys
         * (see the {@link autoClipboard} property).
         *
         * The event handler may cancel the copy operation.
         */
        readonly copying: Event;
        /**
         * Raises the {@link copying} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         * @return True if the event was not canceled.
         */
        onCopying(e: CellRangeEventArgs): boolean;
        /**
         * Occurs after the user has copied the selection content to the
         * clipboard by pressing one of the clipboard shortcut keys
         * (see the {@link autoClipboard} property).
         */
        readonly copied: Event;
        /**
         * Raises the {@link copied} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         */
        onCopied(e: CellRangeEventArgs): void;
        /**
         * Occurs when the user is pasting content from the clipboard
         * by pressing one of the clipboard shortcut keys
         * (see the {@link autoClipboard} property).
         *
         * The event handler may cancel the paste operation.
         */
        readonly pasting: Event;
        /**
         * Raises the {@link pasting} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         * @return True if the event was not canceled.
         */
        onPasting(e: CellRangeEventArgs): boolean;
        /**
         * Occurs after the user has pasted content from the
         * clipboard by pressing one of the clipboard shortcut keys
         * (see the {@link autoClipboard} property).
         */
        readonly pasted: Event;
        /**
         * Raises the {@link pasted} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         */
        onPasted(e: CellRangeEventArgs): void;
        /**
         * Occurs when the user is pasting content from the clipboard
         * into a cell (see the {@link autoClipboard} property).
         *
         * The event handler may cancel the paste operation.
         */
        readonly pastingCell: Event;
        /**
         * Raises the {@link pastingCell} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         * @return True if the event was not canceled.
         */
        onPastingCell(e: CellRangeEventArgs): boolean;
        /**
         * Occurs after the user has pasted content from the
         * clipboard into a cell (see the {@link autoClipboard} property).
         */
        readonly pastedCell: Event;
        /**
         * Raises the {@link pastedCell} event.
         *
         * @param e {@link CellRangeEventArgs} that contains the event data.
         */
        onPastedCell(e: CellRangeEventArgs): void;
        /**
         * Occurs when an element representing a cell has been created.
         *
         * This event can be used to format cells for display. It is similar
         * in purpose to the {@link itemFormatter} property, but has the advantage
         * of allowing multiple independent handlers.
         *
         * For example, this code removes the 'wj-wrap' class from cells in
         * group rows:
         *
         * <pre>flex.formatItem.addHandler(function (s, e) {
         *   if (flex.rows[e.row] instanceof wijmo.grid.GroupRow) {
         *     wijmo.removeClass(e.cell, 'wj-wrap');
         *   }
         * });</pre>
         */
        readonly formatItem: Event;
        /**
         * Raises the {@link formatItem} event.
         *
         * @param e {@link FormatItemEventArgs} that contains the event data.
         */
        onFormatItem(e: FormatItemEventArgs): void;
        /**
         * Occurs when the grid starts creating/updating the elements that
         * make up the current view.
         */
        readonly updatingView: Event;
        /**
         * Raises the {@link updatingView} event.
         *
         * @param e {@link CancelEventArgs} that contains the event data.
         * @return True if the event was not canceled.
         */
        onUpdatingView(e: wijmo.CancelEventArgs): boolean;
        /**
         * Occurs when the grid finishes creating/updating the elements that
         * make up the current view.
         *
         * The grid updates the view in response to several actions, including:
         *
         * <ul>
         * <li>refreshing the grid or its data source,</li>
         * <li>adding, removing, or changing rows or columns,</li>
         * <li>resizing or scrolling the grid,</li>
         * <li>changing the selection.</li>
         * </ul>
         */
        readonly updatedView: Event;
        /**
         * Raises the {@link updatedView} event.
         */
        onUpdatedView(e?: wijmo.EventArgs): void;
        _getShowErrors(): boolean;
        _getHasValidation(): boolean;
        _getError(p: GridPanel, r: number, c: number): string;
        private _setAria;
        private _setFocus;
        _setFocusNoScroll(e: HTMLElement): void;
        private _getDefaultRowHeight;
        protected _getCollectionView(value: any): wijmo.collections.ICollectionView;
        private _getCanvasContext;
        private _getWidestRow;
        private _getDesiredWidth;
        private _getDesiredHeight;
        _getDesiredRowHeight(panel: GridPanel, r: number, eMeasure: HTMLElement, cache: any): number;
        _getSortRowIndex(): number;
        _getEditColumnIndex(): number;
        _mappedColumns: any;
        private _sortConverter;
        protected _bindGrid(full: boolean): void;
        _cvCollectionChanged(sender: any, e: wijmo.collections.NotifyCollectionChangedEventArgs): void;
        private _cvCurrentChanged;
        private _syncSelection;
        private _getRowIndex;
        _getCvIndex(index: number): number;
        private _findRow;
        private _updateLayout;
        private _updateStickyHeaders;
        private _updateScrollHandler;
        _getClipToScreen(): boolean;
        private _scroll;
        private _updateScrollPosition;
        private _updateContent;
        private _clearCells;
        _useFrozenDiv(): boolean;
        private _updateFrozenCells;
        private _getMarqueeRect;
        _bindColumns(): void;
        _updateColumnTypes(): void;
        _getBindingColumn(p: GridPanel, r: Number, c: Column): Column;
        _getRowHeaderPath(): wijmo.Binding;
        _bindRows(): void;
        _addBoundRow(items: any[], index: number): void;
        _addNode(items: any[], index: number, level: number): void;
        private _addGroup;
        private static _getSerializableProperties;
        _copy(key: string, value: any): boolean;
        _isInputElement(e: any): boolean;
        _wantsInput(e: any): boolean;
        private static _maxCssHeight;
        private static _getMaxSupportedCssHeight;
        static _rtlMode: string;
        private static _getRtlMode;
    }
}
declare module wijmo.grid {
    /**
     * Specifies constants that define the action to perform when special
     * keys such as ENTER and TAB are pressed.
     */
    enum KeyAction {
        /** No special action (let the browser handle the key). */
        None = 0,
        /** Move the selection to the next row. */
        MoveDown = 1,
        /** Move the selection to the next column. */
        MoveAcross = 2,
        /** Move the selection to the next column, then wrap to the next row. */
        Cycle = 3,
        /** Move the selection to the next column, then wrap to the next row, then out of the control. */
        CycleOut = 4
    }
    /**
     * Handles the grid's keyboard commands.
     */
    class _KeyboardHandler {
        _g: FlexGrid;
        _altDown: boolean;
        _kaTab: KeyAction;
        _kaEnter: KeyAction;
        _search: string;
        _toSearch: any;
        /**
         * Initializes a new instance of the {@link _KeyboardHandler} class.
         *
         * @param g {@link FlexGrid} that owns this {@link _KeyboardHandler}.
         */
        constructor(g: FlexGrid);
        _keydown(e: KeyboardEvent): void;
        _performKeyAction(action: KeyAction, shift: boolean): boolean;
        private _keypress;
        private _findNext;
        private _moveSel;
        private _deleteSel;
        private _startEditing;
    }
}
declare module wijmo.grid {
    /**
     * Implements a hidden input element so users can choose IME modes when
     * the FlexGrid has focus, and start composing before the grid enters
     * edit mode.
     */
    class _ImeHandler {
        _g: FlexGrid;
        _tbx: HTMLInputElement;
        _isMouseDown: boolean;
        _updateImeFocusBnd: any;
        _cmpstartBnd: any;
        _mousedownBnd: any;
        _mouseupBnd: any;
        _keydownBnd: any;
        _ignoreKey: boolean;
        static _cssHidden: {
            position: string;
            left: number;
            top: number;
            width: string;
            overflow: string;
        };
        /**
         * Initializes a new instance of the {@link _ImeHandler} class
         * and attaches it to a {@link FlexGrid}.
         *
         * @param g {@link FlexGrid} that this {@link _ImeHandler} will be attached to.
         */
        constructor(g: FlexGrid);
        /**
         * Disposes of this {@link _ImeHandler}.
         */
        dispose(): void;
        _keydown(e: KeyboardEvent): void;
        _compositionstart(): void;
        _cellEditEnded(): void;
        _mousedown(e: any): void;
        _mouseup(e: any): void;
        _updateImeFocus(): void;
        _enableIme(): boolean;
    }
}
declare module wijmo.grid {
    /**
     * Specifies constants that define the type of cell in a {@link GridPanel}.
     */
    enum CellType {
        /** Unknown or invalid cell type. */
        None = 0,
        /** Regular data cell. */
        Cell = 1,
        /** Column header cell. */
        ColumnHeader = 2,
        /** Row header cell. */
        RowHeader = 3,
        /** Top-left cell. */
        TopLeft = 4,
        /** Column footer cell. */
        ColumnFooter = 5,
        /** Bottom left cell (at the intersection of the row header and column footer cells). **/
        BottomLeft = 6
    }
    /**
     * Represents a logical part of the grid, such as the column headers, row headers,
     * and scrollable data part.
     */
    class GridPanel {
        private _g;
        private _ct;
        private _e;
        private _rows;
        private _cols;
        private _offsetY;
        private _activeCell;
        private _docRange;
        private _rng;
        private _recycle;
        _vrb: CellRange;
        _vru: CellRange;
        static readonly _INDEX_KEY: string;
        static readonly _HTML_CELL: string;
        /**
         * Initializes a new instance of the {@link GridPanel} class.
         *
         * @param g The {@link FlexGrid} object that owns the panel.
         * @param cellType The type of cell in the panel.
         * @param rows The rows displayed in the panel.
         * @param cols The columns displayed in the panel.
         * @param host The HTMLElement that hosts the cells in the control.
         */
        constructor(g: FlexGrid, cellType: CellType, rows: RowCollection, cols: ColumnCollection, host: HTMLElement);
        /**
         * Gets the grid that owns the panel.
         */
        readonly grid: FlexGrid;
        /**
         * Gets the type of cell contained in the panel.
         */
        readonly cellType: CellType;
        /**
         * Gets a {@link CellRange} that indicates the range of cells currently visible on the panel.
         */
        readonly viewRange: CellRange;
        /**
         * Gets the total width of the content in the panel.
         */
        readonly width: number;
        /**
         * Gets the total height of the content in this panel.
         */
        readonly height: number;
        /**
         * Gets the panel's row collection.
         */
        readonly rows: RowCollection;
        /**
         * Gets the panel's column collection.
         */
        readonly columns: ColumnCollection;
        /**
         * Gets the value stored in a cell in the panel.
         *
         * @param r The row index of the cell.
         * @param c The index, name, or binding of the column that contains the cell.
         * @param formatted Whether to format the value for display.
         */
        getCellData(r: number, c: any, formatted: boolean): any;
        /**
         * Sets the content of a cell in the panel.
         *
         * @param r The index of the row that contains the cell.
         * @param c The index, name, or binding of the column that contains the cell.
         * @param value The value to store in the cell.
         * @param coerce Whether to change the value automatically to match the column's data type.
         * @param invalidate Whether to invalidate the grid to show the change.
         * @return Returns true if the value is stored successfully, false otherwise (failed cast).
         */
        setCellData(r: number, c: any, value: any, coerce?: boolean, invalidate?: boolean): boolean;
        /**
         * Gets a cell's bounds in viewport coordinates.
         *
         * The returned value is a {@link Rect} object which contains the position and dimensions
         * of the cell in viewport coordinates.
         * The viewport coordinates are the same as those used by the
         * <a href="https://developer.mozilla.org/en-US/docs/Web/API/Element.getBoundingClientRect"
         * target="_blank">getBoundingClientRect</a> method.
         *
         * @param r The index of the row that contains the cell.
         * @param c The index of the column that contains the cell.
         * @param raw Whether to return the rectangle in raw panel coordinates as opposed to viewport coordinates.
         */
        getCellBoundingRect(r: number, c: number, raw?: boolean): wijmo.Rect;
        /**
         * Gets the element that represents a cell within this {@link GridPanel}.
         *
         * If the cell is not currently in view, this method returns null.
         *
         * @param r The index of the row that contains the cell.
         * @param c The index of the column that contains the cell.
         */
        getCellElement(r: number, c: number): HTMLElement;
        /**
         * Gets a {@link SelectedState} value that indicates the selected state of a cell.
         *
         * @param r Row index of the cell to inspect.
         * @param c Column index of the cell to inspect.
         * @param rng {@link CellRange} that contains the cell to inspect.
         */
        getSelectedState(r: number, c: number, rng: CellRange): SelectedState;
        /**
         * Gets the host element for the panel.
         */
        readonly hostElement: HTMLElement;
        _getAdjustedSelection(sel: CellRange): CellRange;
        _getOffsetY(): number;
        _updateContent(recycle: boolean, state: boolean, offsetY: number): HTMLElement;
        _clearCells(): void;
        _reorderCells(rngNew: CellRange, rngOld: CellRange): void;
        _createRange(host: Element, start: number, end: number): Range;
        _renderColHdrRow(rng: CellRange, state: boolean): number;
        _renderColHdrCell(row: HTMLElement, c: number, rng: CellRange, state: boolean, ctr: number): number;
        _renderRowHdrCell(row: HTMLElement, r: number, value: any): number;
        _renderRow(r: number, rng: CellRange, state: boolean, ctr: number): number;
        _renderCell(row: HTMLElement, r: number, c: number, rng: CellRange, state: boolean, ctr: number): number;
        _removeExtraCells(row: HTMLElement, count: number): void;
        _getViewRange(): CellRange;
        _getFrozenPos(): wijmo.Point;
    }
}
declare module wijmo.grid {
    /**
     * Specifies constants that define which areas of the grid support cell merging.
     */
    enum AllowMerging {
        /** No merging. */
        None = 0,
        /** Merge scrollable cells. */
        Cells = 1,
        /** Merge column headers. */
        ColumnHeaders = 2,
        /** Merge row headers. */
        RowHeaders = 4,
        /** Merge column and row headers. */
        AllHeaders = 6,
        /** Merge all areas. */
        All = 7
    }
    /**
     * Defines the {@link FlexGrid}'s cell merging behavior.
     *
     * An instance of this class is automatically created and assigned to
     * the grid's {@link FlexGrid.mergeManager} property to implement the
     * grid's default merging behavior.
     *
     * If you want to customize the default merging behavior, create a class
     * that derives from {@link MergeManager} and override the {@link getMergedRange}
     * method.
     */
    class MergeManager {
        _g: FlexGrid;
        /**
         * Initializes a new instance of the {@link MergeManager} class.
         *
         * @param g The {@link FlexGrid} object that owns this {@link MergeManager}.
         */
        constructor(g: FlexGrid);
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
        getMergedRange(p: GridPanel, r: number, c: number, clip?: boolean): CellRange;
        private _mergeCell;
    }
}
declare module wijmo.grid {
    /**
     * Contains information about the part of a {@link FlexGrid} control
     * at a given position on the page.
     */
    class HitTestInfo {
        _g: FlexGrid;
        _p: GridPanel;
        _pt: wijmo.Point;
        _row: number;
        _col: number;
        _rng: CellRange;
        _edge: number;
        static _SZEDGE: number[];
        /**
         * Initializes a new instance of the {@link wijmo.grid.HitTestInfo} class.
         *
         * @param grid The {@link FlexGrid} control, {@link GridPanel}, or cell element
         * to investigate.
         * @param pt The {@link Point} object in page coordinates to investigate.
         */
        constructor(grid: any, pt: any);
        /**
         * Gets the point in control coordinates that this {@link wijmo.grid.HitTestInfo} refers to.
         */
        readonly point: wijmo.Point;
        /**
         * Gets the type of cell found at the specified position.
         */
        readonly cellType: CellType;
        /**
         * Gets the {@link GridPanel} that this {@link HitTestInfo} refers to.
         */
        readonly panel: GridPanel;
        /**
         * Gets the {@link FlexGrid} that this {@link HitTestInfo} refers to.
         */
        readonly grid: FlexGrid;
        /**
         * Gets the row index of the cell at the specified position.
         */
        readonly row: number;
        /**
         * Gets the column index of the cell at the specified position.
         */
        readonly col: number;
        /**
         * Gets the cell range at the specified position.
         */
        readonly range: CellRange;
        /**
         * Gets a value that indicates whether the mouse is near the left edge of the cell.
         */
        readonly edgeLeft: boolean;
        /**
         * Gets a value that indicates whether the mouse is near the top edge of the cell.
         */
        readonly edgeTop: boolean;
        /**
         * Gets a value that indicates whether the mouse is near the right edge of the cell.
         */
        readonly edgeRight: boolean;
        /**
         * Gets a value that indicates whether the mouse is near the bottom edge of the cell.
         */
        readonly edgeBottom: boolean;
    }
}
declare module wijmo.grid {
    function softInput(): typeof wijmo.input;
}
declare module wijmo.grid {
    /**
     * Creates HTML elements that represent cells within a {@link FlexGrid} control.
     */
    class CellFactory {
        static _WJC_COLLAPSE: string;
        static _WJC_DROPDOWN: string;
        static _ddBtn: HTMLElement;
        static _fmtRng: CellRange;
        /**
         * Creates or updates a cell in the grid.
         *
         * @param p The {@link GridPanel} that contains the cell.
         * @param r The index of the row that contains the cell.
         * @param c The index of the column that contains the cell.
         * @param cell The element that represents the cell.
         * @param rng The {@link CellRange} object that contains the cell's
         * merged range, or null if the cell is not merged.
         * @param updateContent Whether to update the cell's content as
         * well as its position and style.
         */
        updateCell(p: GridPanel, r: number, c: number, cell: HTMLElement, rng?: CellRange, updateContent?: boolean): void;
        /**
         * Disposes of a cell element and releases all resources associated with it.
         *
         * @param cell The element that represents the cell.
         */
        disposeCell(cell: HTMLElement): void;
        /**
         * Gets the value of the editor currently being used.
         *
         * @param g {@link FlexGrid} that owns the editor.
         */
        getEditorValue(g: FlexGrid): any;
        private _isEditingCell;
        private _getTreeBtn;
        private _getSortIcon;
    }
}
declare module wijmo.grid {
    /**
     * Handles the grid's editing.
     */
    class _EditHandler {
        _g: FlexGrid;
        _rng: CellRange;
        _edt: HTMLInputElement;
        _edItem: any;
        _lbx: wijmo.input.ListBox;
        _fullEdit: boolean;
        _list: any;
        _evtInput: any;
        _evtChange: any;
        _edtValue: string;
        /**
         * Initializes a new instance of the {@link _EditHandler} class.
         *
         * @param g {@link FlexGrid} that owns this {@link _EditHandler}.
         */
        constructor(g: FlexGrid);
        /**
         * Starts editing a given cell.
         *
         * @param fullEdit Whether to stay in edit mode when the user presses the cursor keys. Defaults to false.
         * @param r Index of the row to be edited. Defaults to the currently selected row.
         * @param c Index of the column to be edited. Defaults to the currently selected column.
         * @param focus Whether to give the editor the focus. Defaults to true.
         * @param evt Event that triggered this action (usually a keypress or keydown).
         * @return True if the edit operation started successfully.
         */
        startEditing(fullEdit?: boolean, r?: number, c?: number, focus?: boolean, evt?: any): boolean;
        /**
         * Commits any pending edits and exits edit mode.
         *
         * @param cancel Whether pending edits should be canceled or committed.
         * @return True if the edit operation finished successfully.
         */
        finishEditing(cancel?: boolean): boolean;
        _setCellError(cell: HTMLElement, error: string): void;
        /**
         * Gets the <b>HTMLInputElement</b> that represents the cell editor currently active.
         */
        readonly activeEditor: HTMLInputElement;
        /**
         * Gets a {@link CellRange} that identifies the cell currently being edited.
         */
        readonly editRange: CellRange;
        /**
         * Gets the content of a {@link CellRange} as a string suitable for
         * copying to the clipboard.
         *
         * Hidden rows and columns are not included in the clip string.
         *
         * @param rng {@link CellRange} to copy. If omitted, the current selection is used.
         * @param csv Whether to use the CSV format (comma-delimited cells).
         * @param colHdrs Whether to include the column headers.
         * @param rowHdrs Whether to include the row headers.
         */
        getClipString(rng?: CellRange, csv?: boolean, colHdrs?: boolean, rowHdrs?: boolean): string;
        _getRowClipString(p: GridPanel, r: number, firstCol: number, lastCol: number, csv: boolean, rh: boolean): string;
        protected _getCellClipString(cell: string, csv: boolean): string;
        /**
         * Parses a string into rows and columns and applies the content to a given range.
         *
         * Hidden rows and columns are skipped.
         *
         * @param text Tab and newline delimited text to parse into the grid.
         * @param rng {@link CellRange} to copy. If omitted, the current selection is used.
         */
        setClipString(text: string, rng?: CellRange): boolean;
        _isNativeCheckbox(edt: any): boolean;
        _parseClipString(text: string): string[][];
        private _parseClipCell;
        _expandClipRows(rows: string[][], rng: CellRange): void;
        private _updateEditorCell;
        private _updateCell;
        private _getValidationError;
        _allowEditing(r: number, c: number): boolean;
        _commitRowEdits(): void;
        _keydown(e: KeyboardEvent): boolean;
        private _keydownListBox;
        _keypress(e: KeyboardEvent): void;
        _findString(items: string[], text: string, caseSensitive: boolean): number;
        _toggleListBox(evt: any, rng?: CellRange): boolean;
        private _createListBox;
        private _removeListBox;
    }
}
declare module wijmo.grid {
    /**
     * Specifies constants that define the row/column sizing behavior.
     */
    enum AllowResizing {
        /** The user may not resize rows or columns. */
        None = 0,
        /** The user may resize columns by dragging the edge of the column headers. */
        Columns = 1,
        /** The user may resize rows by dragging the edge of the row headers. */
        Rows = 2,
        /** The user may resize rows and columns by dragging the edge of the headers. */
        Both = 3,
        /** The user may resize columns by dragging the edge of any cell. */
        ColumnsAllCells,
        /** The user may resize rows by dragging the edge of any cell. */
        RowsAllCells,
        /** The user may resize rows and columns by dragging the edge of any cell. */
        BothAllCells
    }
    /**
     * Specifies constants that define the row/column auto-sizing behavior.
     */
    enum AutoSizeMode {
        /** Autosizing is disabled. */
        None = 0,
        /** Autosizing accounts for header cells. */
        Headers = 1,
        /** Autosizing accounts for data cells. */
        Cells = 2,
        /** Autosizing accounts for header and data cells. */
        Both = 3
    }
    /**
     * Specifies constants that define the row/column dragging behavior.
     */
    enum AllowDragging {
        /** The user may not drag rows or columns. */
        None = 0,
        /** The user may drag columns. */
        Columns = 1,
        /** The user may drag rows. */
        Rows = 2,
        /** The user may drag rows and columns. */
        Both = 3
    }
    /**
     * Handles the grid's mouse commands.
     */
    class _MouseHandler {
        _g: FlexGrid;
        _htDown: HitTestInfo;
        _htDrag: HitTestInfo;
        _selDown: CellRange;
        _tsLast: number;
        _isDown: boolean;
        _eMouse: MouseEvent;
        _lbSelState: boolean;
        _lbSelRows: Object;
        _szRowCol: RowCol;
        _szStart: number;
        _szArgs: CellRangeEventArgs;
        _dragSrc: any;
        _dvMarker: HTMLElement;
        _rngTarget: CellRange;
        _updating: boolean;
        static _SZ_MIN: number;
        /**
         * Initializes a new instance of the {@link _MouseHandler} class.
         *
         * @param g {@link FlexGrid} that owns this {@link _MouseHandler}.
         */
        constructor(g: FlexGrid);
        /**
         * Resets the mouse state.
         */
        resetMouseState(): void;
        private _enableTouchResizing;
        private _mousedown;
        private _mousemove;
        private _mouseup;
        private _click;
        private _handleClick;
        private _clickSort;
        private _dblclick;
        private _hover;
        private _getResizeCol;
        private _getResizeRow;
        private _mouseSelect;
        private _handleResizing;
        private _dragstart;
        private _dragend;
        private _dragover;
        private _drop;
        private _hitTest;
        private _showResizeMarker;
        private _showDragMarker;
        private _finishResizing;
        private _startListBoxSelection;
        private _handleSelection;
    }
}
declare module wijmo.grid {
}
