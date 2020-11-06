using System;
using System.Windows.Forms;
using System.Reflection;
using C1.Win.C1ScrollBar;

namespace DataConnectorExplorer
{
    public class ThemeablePropertyGrid : PropertyGrid
    {
        private MethodInfo _onScroll;

        private bool _changing;
        private bool _c1changing;

        private Control _gridView;
        private VScrollBar _scrollBar;

        public ThemeablePropertyGrid()
        {
            _gridView = Controls[2];
            _scrollBar = _gridView.Controls[0] as VScrollBar;
            _scrollBar.ValueChanged += _scrollBar_ValueChanged;

            // remove old scroll bar from the controls
            _gridView.Controls.Remove(_scrollBar);

            // gridView subscribed to its events, so we can't override and replace the scrollbar => use reflection.
            // this is protected method, so it's OK to use refection.
            _onScroll = _scrollBar.GetType().GetMethod("OnScroll", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            // add themeable scrollbar
            C1ScrollBar = new C1VScrollBar()
            {
                Dock = DockStyle.Right,
                Width = _scrollBar.Width + 1,
                Location = new System.Drawing.Point(0, 0)
            };
            C1ScrollBar.Scroll += _c1ScrollBar_Scroll;
            C1ScrollBar.ValueChanged += C1ScrollBar_ValueChanged;
            UpdateProperties();

            _gridView.Controls.Add(C1ScrollBar);

            var m = Margin;
            m.Right = C1ScrollBar.Width;
            Margin = m;
        }

        public C1VScrollBar C1ScrollBar
        {
            get;
            private set;
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            C1ScrollBar.Location = new System.Drawing.Point(0, 0);
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateProperties();
            C1ScrollBar.Location = new System.Drawing.Point(0, 0);
        }

        private void UpdateProperties()
        {
            C1ScrollBar.Minimum = _scrollBar.Minimum;
            C1ScrollBar.Maximum = _scrollBar.Maximum;
            C1ScrollBar.SmallChange = _scrollBar.SmallChange;
            C1ScrollBar.LargeChange = _scrollBar.LargeChange;
        }

        private void _c1ScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            // Let's update properties first
            UpdateProperties();

            // Passing the value of the themeable scrollbar to the old one
            _c1changing = true;

            _scrollBar.Value = C1ScrollBar.Value;

            // let the gridView know that user have moved the scrollbar
            _onScroll.Invoke(_scrollBar, new object[] { e });

            //_gridView.Invalidate();
            _c1changing = false;
        }

        private void C1ScrollBar_ValueChanged(object sender, EventArgs e)
        {
            if (!_changing)
            {
                // Let's update properties first
                UpdateProperties();
            }
        }

        private void _scrollBar_ValueChanged(object sender, EventArgs e)
        {
            if (!_c1changing)
            {
                // Let's update properties first
                UpdateProperties();

                // _scrollBar.Value can be changed using the mouse or keyboard,
                // Update the value of the themeable scroll bar accordingly.
                _changing = true;
                if (C1ScrollBar.Value != _scrollBar.Value)
                    C1ScrollBar.Value = _scrollBar.Value;
                _changing = false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_scrollBar != null)
                    _scrollBar.ValueChanged -= _scrollBar_ValueChanged;
                if (C1ScrollBar != null)
                {
                    C1ScrollBar.Scroll -= _c1ScrollBar_Scroll;
                    C1ScrollBar.ValueChanged -= C1ScrollBar_ValueChanged;
                    C1ScrollBar.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}
