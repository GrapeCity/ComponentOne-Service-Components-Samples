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
declare module wijmo.gauge {
    /**
     * Defines ranges to be used with {@link Gauge} controls.
     *
     * {@link Range} objects have {@link min} and {@link max} properties that
     * define the range's domain, as well as {@link color} and {@link thickness}
     * properties that define the range's appearance.
     *
     * Every {@link Gauge} control has at least two ranges:
     * the 'face' defines the minimum and maximum values for the gauge, and
     * the 'pointer' displays the gauge's current value.
     *
     * In addition to the built-in ranges, gauges may have additional
     * ranges used to display regions of significance (for example,
     * low, medium, and high values).
     */
    class Range {
        static _ctr: number;
        private _min;
        private _max;
        private _thickness;
        private _color;
        private _name;
        /**
         * Initializes a new instance of the {@link Range} class.
         *
         * @param options Initialization options for the {@link Range} or a string
         * containing the {@link Range} name.
         */
        constructor(options?: any);
        /**
         * Gets or sets the minimum value for this range.
         */
        min: number;
        /**
         * Gets or sets the maximum value for this range.
         */
        max: number;
        /**
         * Gets or sets the color used to display this range.
         */
        color: string;
        /**
         * Gets or sets the thickness of this range as a percentage of
         * the parent gauge's thickness.
         */
        thickness: number;
        /**
         * Gets or sets the name of this {@link Range}.
         */
        name: string;
        /**
         * Occurs when the value of a property in this {@link Range} changes.
         */
        readonly propertyChanged: Event;
        /**
         * Raises the {@link propertyChanged} event.
         *
         * @param e {@link PropertyChangedEventArgs} that contains the property
         * name, old, and new values.
         */
        onPropertyChanged(e: wijmo.PropertyChangedEventArgs): void;
        _setProp(name: string, value: any): void;
    }
}
declare module wijmo.gauge {
    /**
     * Specifies which values to display as text.
     */
    enum ShowText {
        /** Do not show any text in the gauge. */
        None = 0,
        /** Show the gauge's {@link Gauge.value} as text. */
        Value = 1,
        /** Show the gauge's {@link Gauge.min} and {@link Gauge.max} values as text. */
        MinMax = 2,
        /** Show the gauge's {@link Gauge.value}, {@link Gauge.min}, and {@link Gauge.max} as text. */
        All = 3
    }
    /**
     * Base class for the Wijmo Gauge controls (abstract).
     */
    class Gauge extends wijmo.Control {
        static _SVGNS: string;
        static _ctr: number;
        private _ranges;
        private _rngElements;
        private _format;
        private _getText;
        private _showRanges;
        private _stackRanges;
        private _shadow;
        private _animated;
        private _animInterval;
        private _readOnly;
        private _step;
        private _showText;
        private _showTicks;
        private _tickSpacing;
        private _thumbSize;
        private _filterID;
        private _rangesDirty;
        private _origin;
        private _dragging;
        protected _thickness: number;
        protected _initialized: boolean;
        protected _animColor: string;
        protected _face: Range;
        protected _pointer: Range;
        protected _dSvg: HTMLDivElement;
        protected _svg: SVGSVGElement;
        protected _gFace: SVGGElement;
        protected _gRanges: SVGGElement;
        protected _gPointer: SVGGElement;
        protected _gCover: SVGGElement;
        protected _pFace: SVGPathElement;
        protected _pPointer: SVGPathElement;
        protected _pTicks: SVGPathElement;
        protected _filter: SVGFilterElement;
        protected _cValue: SVGCircleElement;
        protected _tValue: SVGTextElement;
        protected _tMin: SVGTextElement;
        protected _tMax: SVGTextElement;
        /**
         * Gets or sets the template used to instantiate {@link Gauge} controls.
         */
        static controlTemplate: string;
        /**
         * Initializes a new instance of the {@link Gauge} class.
         *
         * @param element The DOM element that hosts the control, or a CSS selector for the host element (e.g. '#theCtrl').
         * @param options The JavaScript object containing initialization data for the control.
         */
        constructor(element: any, options?: any);
        /**
         * Gets or sets the value displayed on the gauge.
         */
        value: number;
        /**
         * Gets or sets the minimum value that can be displayed on the gauge.
         *
         * For details about using the {@link min} and {@link max} properties, please see the
         * <a href="/wijmo/docs/Topics/Input/Using-Min-Max">Using the min and max properties</a> topic.
         */
        min: number;
        /**
         * Gets or sets the maximum value that can be displayed on the gauge.
         *
         * For details about using the {@link min} and {@link max} properties, please see the
         * <a href="/wijmo/docs/Topics/Input/Using-Min-Max">Using the min and max properties</a> topic.
         */
        max: number;
        /**
         * Gets or sets the starting point used for painting the range.
         *
         * By default, this property is set to null, which causes the value range
         * to start at the gauge's minimum value, or zero if the minimum is less
         * than zero.
         */
        origin: number;
        /**
         * Gets or sets a value that indicates whether the user can edit the value
         * using the mouse and keyboard.
         *
         * The default value for this property is <b>true</b>.
         */
        isReadOnly: boolean;
        /**
         * Gets or sets the amount to add to or subtract from the {@link value} property
         * when the user presses the arrow keys or moves the mouse wheel.
         */
        step: number;
        /**
         * Gets or sets the format string used to display gauge values as text.
         */
        format: string;
        /**
         * Gets or sets a callback that returns customized strings used to
         * display gauge values.
         *
         * Use this property if you want to customize the strings shown on
         * the gauge in cases where the {@link format} property is not enough.
         *
         * If provided, the callback should be a function as that takes as
         * parameters the gauge, the part name, the value, and the formatted
         * value. The callback should return the string to be displayed on
         * the gauge.
         *
         * For example:
         *
         * <pre>// callback to convert values into strings
         * gauge.getText = function (gauge, part, value, text) {
         *   switch (part) {
         *     case 'value':
         *       if (value &lt;= 10) return 'Empty!';
         *       if (value &lt;= 25) return 'Low...';
         *       if (value &lt;= 95) return 'Good';
         *       return 'Full';
         *     case 'min':
         *       return 'EMPTY';
         *     case 'max':
         *       return 'FULL';
         *   }
         *   return text;
         * }</pre>
         */
        getText: Function;
        /**
         * Gets or sets the thickness of the gauge, on a scale between zero and one.
         *
         * Setting the thickness to one causes the gauge to fill as much of the
         * control area as possible. Smaller values create thinner gauges.
         */
        thickness: number;
        /**
         * Gets or sets the {@link Range} used to represent the gauge's overall geometry
         * and appearance.
         */
        face: Range;
        /**
         * Gets or sets the {@link Range} used to represent the gauge's current value.
         */
        pointer: Range;
        /**
         * Gets or sets the {@link ShowText} values to display as text in the gauge.
         *
         * The default value for this property is <b>ShowText.All</b> for {@link RadialGauge}
         * controls, and to <b>ShowText.None</b> for {@link LinearGauge} controls.
         */
        showText: ShowText;
        /**
         * Gets or sets a property that determines whether the gauge should
         * display tickmarks at each {@link step} value.
         *
         * The tickmarks can be formatted in CSS using the <b>wj-gauge</b> and
         * <b>wj-ticks</b> class names. For example:
         *
         * <pre>.wj-gauge .wj-ticks {
         *     stroke-width: 2px;
         *     stroke: white;
         * }</pre>
         *
         * The default value for this property is <b>false</b>.
         */
        showTicks: boolean;
        /**
         * Gets or sets the spacing between tickmarks.
         *
         * Set the {@link showTicks} property to true if you want the
         * gauge to show tickmarks along its face. By default, the
         * interval between tickmarks is defined by the {@link step}
         * property.
         *
         * Use the {@link tickSpacing} property to override the default
         * and use a spacing that is different from the {@link step}
         * value. Set the {@link tickSpacing} property to null to revert
         * to the default behavior.
         */
        tickSpacing: number;
        /**
         * Gets or sets the size of the element that shows the gauge's current value,
         * in pixels.
         */
        thumbSize: number;
        /**
         * Gets or sets a value that indicates whether the gauge displays the ranges
         * contained in  the {@link ranges} property.
         *
         * If this property is set to false, the ranges contained in the {@link ranges}
         * property are not displayed in the gauge. Instead, they are used to
         * interpolate the color of the {@link pointer} range while animating value changes.
         *
         * The default value for this property is <b>true</b>.
         */
        showRanges: boolean;
        /**
         * Gets or sets a value that determines whether the ranges contained in
         * the {@link ranges} collection should be stacked within the gauge.
         *
         * By default, {@link stackRanges} is set to false, and the ranges in the
         * {@link ranges} collection are displayed with the same thickness as the
         * gauge's face.
         *
         * Setting {@link stackRanges} to true causes the ranges to become narrower,
         * and to be displayed side-by-side.
         */
        stackRanges: boolean;
        /**
         * Gets or sets a value that indicates whether the gauge displays
         * a shadow effect.
         *
         * The default value for this property is <b>true</b>.
         */
        hasShadow: boolean;
        /**
         * Gets or sets a value that determines whether the {@link Gauge}
         * should use animation to show value changes.
         *
         * The default value for this property is <b>true</b>.
         */
        isAnimated: boolean;
        /**
         * Gets the collection of ranges in this gauge.
         */
        readonly ranges: wijmo.collections.ObservableArray;
        /**
         * Occurs when the value of the {@link value} property changes.
         */
        readonly valueChanged: Event;
        /**
         * Raises the {@link valueChanged} event.
         */
        onValueChanged(e?: wijmo.EventArgs): void;
        /**
         * Refreshes the control.
         *
         * @param fullUpdate Indicates whether to update the control layout as well as the content.
         */
        refresh(fullUpdate?: boolean): void;
        /**
         * Gets a number that corresponds to the value of the gauge at a given point.
         *
         * For example:
         *
         * <pre>
         * // hit test a point when the user clicks on the gauge
         * gauge.hostElement.addEventListener('click', function (e) {
         *   var ht = gauge.hitTest(e.pageX, e.pageY);
         *   if (ht != null) {
         *     console.log('you clicked the gauge at value ' + ht.toString());
         *   }
         * });
         * </pre>
         *
         * @param pt The point to investigate, in window coordinates, or a MouseEvent object,
         * or the x coordinate of the point.
         * @param y The Y coordinate of the point (if the first parameter is a number).
         * @return Value of the gauge at the point, or null if the point is not on the gauge's face.
         */
        hitTest(pt: any, y?: number): number;
        static _getBBox(e: any): any;
        protected _getFilterUrl(): string;
        protected _getRangeElement(rng: Range): SVGPathElement;
        protected _rangeChanged(rng: Range, e: wijmo.PropertyChangedEventArgs): void;
        protected _createElement(tag: string, parent: SVGElement, cls?: string): Element;
        protected _centerText(e: SVGTextElement, value: number, center: wijmo.Point): void;
        protected _copy(key: string, value: any): boolean;
        protected _getPercent: (value: any) => number;
        protected _showElement(e: SVGElement, show: boolean): void;
        protected _setAttribute(e: SVGElement, att: string, value: string): void;
        protected _updateRange(rng: Range, value?: number): void;
        protected _getPointerColor(value: number): string;
        protected _keydown(e: KeyboardEvent): void;
        protected _getKey(key: number): number;
        protected _applyMouseValue(e: any, instant?: boolean): boolean;
        protected _updateRangeElement(e: SVGPathElement, rng: Range, value: number): void;
        protected _updateText(): void;
        protected _updateTicks(): void;
        protected _getValueFromPoint(pt: wijmo.Point): any;
        protected _fix(n: any): string;
        protected _updateAria(): void;
    }
}
declare module wijmo.gauge {
    /**
     * The {@link RadialGauge} displays a circular scale with an indicator
     * that represents a single value and optional ranges to represent
     * reference values.
     *
     * If you set the gauge's {@link RadialGauge.isReadOnly} property to
     * false, then users will be able to edit the value by clicking on
     * the gauge.
     *
     * {@sample Gauge/RadialGauge Example}
     */
    class RadialGauge extends Gauge {
        private _startAngle;
        private _sweepAngle;
        private _autoScale;
        private _rcSvg;
        private _ctmInv;
        private _ptSvg;
        /**
         * Initializes a new instance of the {@link RadialGauge} class.
         *
         * @param element The DOM element that hosts the control, or a CSS selector for the host element (e.g. '#theCtrl').
         * @param options The JavaScript object containing initialization data for the control.
         */
        constructor(element: any, options?: any);
        /**
         * Gets or sets the starting angle for the gauge.
         *
         * Angles are measured in degrees, clockwise, starting from the
         * 9 o'clock position.
         *
         * The default value for this property is <b>0</b>.
         */
        startAngle: number;
        /**
         * Gets or sets the sweep angle for the gauge.
         *
         * Angles are measured in degrees, clockwise,
         * starting from the 9 o'clock position.
         *
         * The default value for this property is <b>180</b>.
         */
        sweepAngle: number;
        /**
         * Gets or sets a value that indicates whether the gauge automatically
         * scales to fill the host element.
         *
         * The default value for this property is <b>true</b>.
         */
        autoScale: boolean;
        /**
         * Gets the bounds of the gauge's face element.
         *
         * This property is useful for positioning custom SVG elements
         * on the gauge.
         *
         * {@fiddle mbno06j3}
         */
        readonly faceBounds: wijmo.Rect;
        readonly clientSize: wijmo.Size;
        /**
         * Refreshes the control.
         *
         * @param fullUpdate Indicates whether to update the control layout as well as the content.
         */
        refresh(fullUpdate?: boolean): void;
        _updateRangeElement(e: SVGPathElement, rng: Range, value: number): void;
        _updateText(): void;
        _updateTicks(): void;
        _updateSegment(path: SVGPathElement, ctr: wijmo.Point, rOut: number, rIn: number, start: number, sweep: number): void;
        _getPoint(ctr: wijmo.Point, angle: number, radius: number): wijmo.Point;
        _getValueFromPoint(pt: wijmo.Point): number;
    }
}
declare module wijmo.gauge {
    /**
     * Represents the direction in which the pointer of a {@link LinearGauge}
     * increases.
     */
    enum GaugeDirection {
        /** Gauge value increases from left to right. */
        Right = 0,
        /** Gauge value increases from right to left. */
        Left = 1,
        /** Gauge value increases from bottom to top. */
        Up = 2,
        /** Gauge value increases from top to bottom. */
        Down = 3
    }
    /**
     * The {@link LinearGauge} displays a linear scale with an indicator
     * that represents a single value and optional ranges to represent
     * reference values.
     *
     * If you set the gauge's {@link LinearGauge.isReadOnly} property to
     * false, then users will be able to edit the value by clicking on
     * the gauge.
     *
     * {@sample Gauge/LinearGauge Example}
     */
    class LinearGauge extends Gauge {
        private _direction;
        /**
         * Initializes a new instance of the {@link LinearGauge} class.
         *
         * @param element The DOM element that hosts the control, or a CSS selector for the host element (e.g. '#theCtrl').
         * @param options JavaScript object containing initialization data for the control.
         */
        constructor(element: any, options?: any);
        /**
         * Gets or sets the direction in which the gauge is filled.
         *
         * The default value for this property is <b>GaugeDirection.Right</b>.
         */
        direction: GaugeDirection;
        /**
         * Gets the bounds of the gauge's face element.
         *
         * This property is useful for positioning custom SVG elements
         * on the gauge.
         *
         * {@fiddle tha1vms3}
         *
         */
        readonly faceBounds: wijmo.Rect;
        _updateRangeElement(e: SVGPathElement, rng: Range, value: number): void;
        _updateText(): void;
        _updateTicks(): void;
        _updateSegment(path: SVGPathElement, rc: wijmo.Rect): void;
        _setText(e: SVGTextElement, value: number, rc: wijmo.Rect, pos: string): void;
        _getRangeRect(rng: Range, value?: number): wijmo.Rect;
        _getValueFromPoint(pt: wijmo.Point): number;
        _getDirection(): GaugeDirection;
        _getKey(key: number): number;
    }
}
declare module wijmo.gauge {
    /**
     * The {@link BulletGraph} is a type of linear gauge designed specifically for use
     * in dashboards. It displays a single key measure along with a comparative
     * measure and qualitative ranges to instantly signal whether the measure is
     * good, bad, or in some other state.
     *
     * Bullet Graphs were created and popularized by dashboard design expert
     * Stephen Few. You can find more details and examples on
     * <a href="http://en.wikipedia.org/wiki/Bullet_graph">Wikipedia</a>.
     *
     * {@sample Gauge/BulletGraph Example}
     */
    class BulletGraph extends LinearGauge {
        _rngTarget: Range;
        _rngGood: Range;
        _rngBad: Range;
        /**
         * Initializes a new instance of the {@link BulletGraph} class.
         *
         * @param element The DOM element that hosts the control, or a CSS selector for the host element (e.g. '#theCtrl').
         * @param options The JavaScript object containing initialization data for the control.
         */
        constructor(element: any, options?: any);
        /**
         * Gets or sets the target value for the measure.
         */
        target: number;
        /**
         * Gets or sets a reference value considered good for the measure.
         */
        good: number;
        /**
         * Gets or sets a reference value considered bad for the measure.
         */
        bad: number;
        _getRangeRect(rng: Range, value?: number): wijmo.Rect;
    }
}
declare module wijmo.gauge {
}
