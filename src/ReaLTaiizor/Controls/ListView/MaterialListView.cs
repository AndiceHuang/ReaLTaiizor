﻿#region Imports

using System;
using System.Drawing;
using ReaLTaiizor.Utils;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using static ReaLTaiizor.Helpers.MaterialDrawHelper;

#endregion

namespace ReaLTaiizor.Controls.ListView
{
    #region MaterialListView

    public class MaterialListView : System.Windows.Forms.ListView, MaterialControlI
    {
        [Browsable(false)]
        public int Depth { get; set; }

        [Browsable(false)]
        public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;

        [Browsable(false)]
        public MaterialMouseState MouseState { get; set; }

        [Browsable(false)]
        public Point MouseLocation { get; set; }

        private bool _autoSizeTable;

        [Category("Appearance"), Browsable(true)]
        public bool AutoSizeTable
        {
            get
            {
                return _autoSizeTable;
            }
            set
            {
                _autoSizeTable = value;
                Scrollable = !value;
            }
        }

        [Browsable(false)]
        private ListViewItem HoveredItem { get; set; }

        private const int PAD = 16;
        private const int ITEMS_HEIGHT = 52;

        public MaterialListView()
        {
            GridLines = false;
            FullRowSelect = true;
            View = View.Details;
            OwnerDraw = true;
            ResizeRedraw = true;
            BorderStyle = BorderStyle.None;
            MinimumSize = new Size(200, 100);

            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
            BackColor = SkinManager.BackgroundColor;

            // Fix for hovers, by default it doesn't redraw
            MouseLocation = new Point(-1, -1);
            MouseState = MaterialMouseState.OUT;
            MouseEnter += delegate
            {
                MouseState = MaterialMouseState.HOVER;
            };
            MouseLeave += delegate
            {
                MouseState = MaterialMouseState.OUT;
                MouseLocation = new Point(-1, -1);
                HoveredItem = null;
                Invalidate();
            };
            MouseDown += delegate
            {
                MouseState = MaterialMouseState.DOWN;
            };
            MouseUp += delegate
            {
                MouseState = MaterialMouseState.HOVER;
            };
            MouseMove += delegate (object sender, MouseEventArgs args)
            {
                MouseLocation = args.Location;
                var currentHoveredItem = GetItemAt(MouseLocation.X, MouseLocation.Y);
                if (HoveredItem != currentHoveredItem)
                {
                    HoveredItem = currentHoveredItem;
                    Invalidate();
                }
            };
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.FillRectangle(new SolidBrush(BackColor), e.Bounds);
            // Draw Text
            using (MaterialNativeTextRenderer NativeText = new MaterialNativeTextRenderer(g))
            {
                NativeText.DrawTransparentText(
                    e.Header.Text,
                    SkinManager.getLogFontByType(MaterialSkinManager.fontType.Subtitle2),
                    Enabled ? SkinManager.TextHighEmphasisColor : SkinManager.TextDisabledOrHintColor,
                    new Point(e.Bounds.Location.X + PAD, e.Bounds.Location.Y),
                    new Size(e.Bounds.Size.Width - PAD * 2, e.Bounds.Size.Height),
                    MaterialNativeTextRenderer.TextAlignFlags.Left | MaterialNativeTextRenderer.TextAlignFlags.Middle);
            }
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Always draw default background
            g.FillRectangle(SkinManager.BackgroundBrush, e.Bounds);

            if (e.Item.Selected)
            {
                // Selected background
                g.FillRectangle(SkinManager.BackgroundFocusBrush, e.Bounds);
            }
            else if (e.Bounds.Contains(MouseLocation) && MouseState == MaterialMouseState.HOVER)
            {
                // Hover background
                g.FillRectangle(SkinManager.BackgroundHoverBrush, e.Bounds);
            }

            // Draw separator line
            g.DrawLine(new Pen(SkinManager.DividersColor), e.Bounds.Left, e.Bounds.Y, e.Bounds.Right, e.Bounds.Y);

            foreach (ListViewItem.ListViewSubItem subItem in e.Item.SubItems)
            {
                // Draw Text
                using (MaterialNativeTextRenderer NativeText = new MaterialNativeTextRenderer(g))
                {
                    NativeText.DrawTransparentText(
                        subItem.Text,
                        SkinManager.getLogFontByType(MaterialSkinManager.fontType.Body2),
                        Enabled ? SkinManager.TextHighEmphasisColor : SkinManager.TextDisabledOrHintColor,
                        new Point(subItem.Bounds.X + PAD, subItem.Bounds.Y),
                        new Size(subItem.Bounds.Width - PAD * 2, subItem.Bounds.Height),
                        MaterialNativeTextRenderer.TextAlignFlags.Left | MaterialNativeTextRenderer.TextAlignFlags.Middle);
                }
            }
        }

        // Resize
        protected override void OnColumnWidthChanging(ColumnWidthChangingEventArgs e)
        {
            base.OnColumnWidthChanging(e);
            AutoResize();
        }

        protected override void OnColumnWidthChanged(ColumnWidthChangedEventArgs e)
        {
            base.OnColumnWidthChanged(e);
            AutoResize();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AutoResize();
        }

        private void AutoResize()
        {
            if (!AutoSizeTable) return;

            // Width
            int w = 0;
            foreach (ColumnHeader col in Columns)
                w += col.Width;

            // Height
            int h = 50; //Header size
            if (Items.Count > 0) h = TopItem.Bounds.Top;
            foreach (ListViewItem item in Items)
                h += item.Bounds.Height;

            Size = new Size(w, h);
        }

        protected override void InitLayout()
        {
            base.InitLayout();

            // enforce settings
            GridLines = false;
            FullRowSelect = true;
            View = View.Details;
            OwnerDraw = true;
            ResizeRedraw = true;
            BorderStyle = BorderStyle.None;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            BackColor = SkinManager.BackgroundColor;
        }
    }

    #endregion
}