﻿#region Imports

using ReaLTaiizor.Design.Metro;
using ReaLTaiizor.Enum.Metro;
using ReaLTaiizor.Extension.Metro;
using ReaLTaiizor.Interface.Metro;
using ReaLTaiizor.Manager;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#endregion

namespace ReaLTaiizor.Controls
{
    #region MetroBadge

    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(MetroBadge), "Bitmaps.Button.bmp")]
    [Designer(typeof(MetroBadgeDesigner))]
    [DefaultEvent("Click")]
    [DefaultProperty("Text")]
    [ComVisible(true)]
    public class MetroBadge : Control, IMetroControl
    {
        #region Interfaces

        [Category("Metro"), Description("Gets or sets the style associated with the control.")]
        public Style Style
        {
            get => StyleManager?.Style ?? _style;
            set
            {
                _style = value;
                switch (value)
                {
                    case Style.Light:
                        ApplyTheme();
                        break;
                    case Style.Dark:
                        ApplyTheme(Style.Dark);
                        break;
                    case Style.Custom:
                        ApplyTheme(Style.Custom);
                        break;
                    default:
                        ApplyTheme();
                        break;
                }
                Invalidate();
            }
        }

        [Category("Metro"), Description("Gets or sets the The Author name associated with the theme.")]
        public string ThemeAuthor { get; set; }

        [Category("Metro"), Description("Gets or sets the The Theme name associated with the theme.")]
        public string ThemeName { get; set; }

        [Category("Metro"), Description("Gets or sets the Style Manager associated with the control.")]
        public MetroStyleManager StyleManager
        {
            get => _styleManager;
            set
            {
                _styleManager = value;
                Invalidate();
            }
        }

        #endregion Interfaces

        #region Global Vars

        private readonly Methods _mth;
        private readonly Utilites _utl;

        #endregion Global Vars

        #region Internal Vars

        private MouseMode _state;
        private Style _style;
        private MetroStyleManager _styleManager;

        private bool _isDerivedStyle = true;
        private BadgeAlign _badgeAlignment;
        private string _badgeText;
        private Color _normalColor;
        private Color _normalBorderColor;
        private Color _normalTextColor;
        private Color _hoverColor;
        private Color _hoverBorderColor;
        private Color _hoverTextColor;
        private Color _pressColor;
        private Color _pressBorderColor;
        private Color _pressTextColor;
        private Color _disabledBackColor;
        private Color _disabledForeColor;
        private Color _disabledBorderColor;
        private Color _normalBadgeColor;
        private Color _normalBadgeTextColor;
        private Color _hoverBadgeColor;
        private Color _hoverBadgeTextColor;
        private Color _pressBadgeColor;
        private Color _pressBadgeTextColor;

        #endregion Internal Vars

        #region Constructors

        public MetroBadge()
        {
            SetStyle
            (
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw | ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor,
                true
            );
            UpdateStyles();
            base.Font = MetroFonts.Light(10);
            base.BackColor = Color.Transparent;
            _badgeAlignment = BadgeAlign.TopRight;
            _badgeText = "3";
            _utl = new Utilites();
            _mth = new Methods();
            ApplyTheme();
        }

        #endregion Constructors

        #region Properties

        [Category("Metro"), Description("Gets or sets the badge alignment associated with the control.")]
        public BadgeAlign BadgeAlignment
        {
            get => _badgeAlignment;
            set
            {
                _badgeAlignment = value;
                Refresh();
            }
        }

        [Category("Metro"), Description("Gets or sets the badge text associated with the control.")]
        public string BadgeText
        {
            get => _badgeText;
            set
            {
                _badgeText = value;
                Refresh();
            }
        }

        [Category("Metro")]
        public new bool Enabled
        {
            get => base.Enabled;
            set
            {
                base.Enabled = value;
                if (value == false)
                {
                    _state = MouseMode.Disabled;
                }

                Invalidate();
            }
        }

        [Category("Metro")]
        [Description("Gets or sets the control background color in normal mouse sate.")]
        public Color NormalColor
        {
            get => _normalColor;
            set
            {
                _normalColor = value;
                Refresh();
            }
        }

        [Category("Metro")]
        [Description("Gets or sets the control border color in normal mouse sate.")]
        public Color NormalBorderColor
        {
            get => _normalBorderColor;
            set
            {
                _normalBorderColor = value;
                Refresh();
            }
        }

        [Category("Metro")]
        [Description("Gets or sets the control Text color in normal mouse sate.")]
        public Color NormalTextColor
        {
            get => _normalTextColor;
            set
            {
                _normalTextColor = value;
                Refresh();
            }
        }

        [Category("Metro")]
        [Description("Gets or sets the control background color in hover mouse sate.")]
        public Color HoverColor
        {
            get => _hoverColor;
            set
            {
                _hoverColor = value;
                Refresh();
            }
        }

        [Category("Metro")]
        [Description("Gets or sets the control border color in hover mouse sate.")]
        public Color HoverBorderColor
        {
            get => _hoverBorderColor;
            set
            {
                _hoverBorderColor = value;
                Refresh();
            }
        }

        [Category("Metro")]
        [Description("Gets or sets the control Text color in hover mouse sate.")]
        public Color HoverTextColor
        {
            get => _hoverTextColor;
            set
            {
                _hoverTextColor = value;
                Refresh();
            }
        }

        [Category("Metro")]
        [Description("Gets or sets the control background color in pushed mouse sate.")]
        public Color PressColor
        {
            get => _pressColor;
            set
            {
                _pressColor = value;
                Refresh();
            }
        }

        [Category("Metro")]
        [Description("Gets or sets the control border color in pushed mouse sate.")]
        public Color PressBorderColor
        {
            get => _pressBorderColor;
            set
            {
                _pressBorderColor = value;
                Refresh();
            }
        }

        [Category("Metro")]
        [Description("Gets or sets the control Text color in pushed mouse sate.")]
        public Color PressTextColor
        {
            get => _pressTextColor;
            set
            {
                _pressTextColor = value;
                Refresh();
            }
        }

        [Category("Metro")]
        [Description("Gets or sets backcolor used by the control while disabled.")]
        public Color DisabledBackColor
        {
            get => _disabledBackColor;
            set
            {
                _disabledBackColor = value;
                Refresh();
            }
        }

        [Category("Metro")]
        [Description("Gets or sets the forecolor of the control whenever while disabled.")]
        public Color DisabledForeColor
        {
            get => _disabledForeColor;
            set
            {
                _disabledForeColor = value;
                Refresh();
            }
        }

        [Category("Metro")]
        [Description("Gets or sets the border color of the control while disabled.")]
        public Color DisabledBorderColor
        {
            get => _disabledBorderColor;
            set
            {
                _disabledBorderColor = value;
                Refresh();
            }
        }

        [Category("Metro")]
        [Description("Gets or sets the Badge background color in normal mouse sate.")]
        public Color NormalBadgeColor
        {
            get => _normalBadgeColor;
            set
            {
                _normalBadgeColor = value;
                Refresh();
            }
        }

        [Category("Metro")]
        [Description("Gets or sets the Badge Text color in normal mouse sate.")]
        public Color NormalBadgeTextColor
        {
            get => _normalBadgeTextColor;
            set
            {
                _normalBadgeTextColor = value;
                Refresh();
            }
        }

        [Category("Metro")]
        [Description("Gets or sets the Badge background color in hover mouse sate.")]
        public Color HoverBadgeColor
        {
            get => _hoverBadgeColor;
            set
            {
                _hoverBadgeColor = value;
                Refresh();
            }
        }

        [Category("Metro")]
        [Description("Gets or sets the Badge Text color in hover mouse sate.")]
        public Color HoverBadgeTextColor
        {
            get => _hoverBadgeTextColor;
            set
            {
                _hoverBadgeTextColor = value;
                Refresh();
            }
        }

        [Category("Metro")]
        [Description("Gets or sets the Badge background color in pushed mouse sate.")]
        public Color PressBadgeColor
        {
            get => _pressBadgeColor;
            set
            {
                _pressBadgeColor = value;
                Refresh();
            }
        }

        [Category("Metro")]
        [Description("Gets or sets the Badge Text color in pushed mouse sate.")]
        public Color PressBadgeTextColor
        {
            get => _pressBadgeTextColor;
            set
            {
                _pressBadgeTextColor = value;
                Refresh();
            }
        }

        [Category("Metro")]
        [Description("Gets or sets the whether this control reflect to parent(s) style. \n " +
                     "Set it to false if you want the style of this control be independent. ")]
        public bool IsDerivedStyle
        {
            get => _isDerivedStyle;
            set
            {
                _isDerivedStyle = value;
                Refresh();
            }
        }

        #endregion Properties

        #region Draw Control

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle r;
            Rectangle badge;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            switch (BadgeAlignment)
            {
                case BadgeAlign.Topleft:
                    r = new(18, 18, Width - 21, Height - 21);
                    badge = new(5, 5, 29, 29);
                    break;
                case BadgeAlign.TopRight:
                    r = new(0, 18, Width - 18, Height - 21);
                    badge = new(Width - 35, 1, 29, 29);
                    break;
                case BadgeAlign.BottmLeft:
                    r = new(18, 0, Width - 19, Height - 18);
                    badge = new(1, Height - 35, 29, 29);
                    break;
                case BadgeAlign.BottomRight:
                    r = new(0, 0, Width - 19, Height - 18);
                    badge = new(Width - 35, Height - 35, 29, 29);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (_state)
            {
                case MouseMode.Normal:

                    using (SolidBrush bg = new(NormalColor))
                    using (Pen p = new(NormalBorderColor))
                    using (SolidBrush tb = new(NormalTextColor))
                    using (SolidBrush bdgBrush = new(NormalBadgeColor))
                    using (SolidBrush bdgtxtBrush = new(NormalBadgeTextColor))
                    {
                        g.FillRectangle(bg, r);
                        g.DrawRectangle(p, r);
                        g.DrawString(Text, Font, tb, r, _mth.SetPosition());
                        SmoothingType(g);
                        g.FillEllipse(bdgBrush, badge);
                        g.DrawString(BadgeText, Font, bdgtxtBrush, badge, _mth.SetPosition());
                    }
                    break;
                case MouseMode.Hovered:
                    Cursor = Cursors.Hand;
                    using (SolidBrush bg = new(HoverColor))
                    using (Pen p = new(HoverBorderColor))
                    using (SolidBrush tb = new(HoverTextColor))
                    using (SolidBrush bdgBrush = new(HoverBadgeColor))
                    using (SolidBrush bdgtxtBrush = new(HoverBadgeTextColor))
                    {
                        g.FillRectangle(bg, r);
                        g.DrawRectangle(p, r);
                        g.DrawString(Text, Font, tb, r, _mth.SetPosition());
                        SmoothingType(g);
                        g.FillEllipse(bdgBrush, badge);
                        g.DrawString(BadgeText, Font, bdgtxtBrush, badge, _mth.SetPosition());
                    }
                    break;
                case MouseMode.Pushed:

                    using (SolidBrush bg = new(PressColor))
                    using (Pen p = new(PressBorderColor))
                    using (SolidBrush tb = new(PressTextColor))
                    using (SolidBrush bdgBrush = new(PressBadgeColor))
                    using (SolidBrush bdgtxtBrush = new(PressBadgeTextColor))
                    {
                        g.FillRectangle(bg, r);
                        g.DrawRectangle(p, r);
                        g.DrawString(Text, Font, tb, r, _mth.SetPosition());
                        SmoothingType(g);
                        g.FillEllipse(bdgBrush, badge);
                        g.DrawString(BadgeText, Font, bdgtxtBrush, badge, _mth.SetPosition());
                    }
                    break;
                case MouseMode.Disabled:

                    using (SolidBrush bg = new(DisabledBackColor))
                    using (Pen p = new(DisabledBorderColor))
                    using (SolidBrush tb = new(DisabledForeColor))
                    using (SolidBrush bdgBrush = new(PressBadgeColor))
                    using (SolidBrush bdgtxtBrush = new(PressBadgeTextColor))
                    {
                        g.FillRectangle(bg, r);
                        g.DrawRectangle(p, r);
                        g.DrawString(Text, Font, tb, r, _mth.SetPosition());
                        SmoothingType(g);
                        g.FillEllipse(bdgBrush, badge);
                        g.DrawString(BadgeText, Font, bdgtxtBrush, badge, _mth.SetPosition());
                    }
                    break;
            }
        }

        #endregion Draw Control

        #region ApplyTheme

        private void ApplyTheme(Style style = Style.Light)
        {
            if (!IsDerivedStyle)
            {
                return;
            }

            switch (style)
            {
                case Style.Light:
                    NormalColor = Color.FromArgb(238, 238, 238);
                    NormalBorderColor = Color.FromArgb(204, 204, 204);
                    NormalTextColor = Color.Black;
                    HoverColor = Color.FromArgb(102, 102, 102);
                    HoverBorderColor = Color.FromArgb(102, 102, 102);
                    HoverTextColor = Color.White;
                    PressColor = Color.FromArgb(51, 51, 51);
                    PressBorderColor = Color.FromArgb(51, 51, 51);
                    PressTextColor = Color.White;
                    NormalBadgeColor = Color.FromArgb(65, 177, 225);
                    NormalBadgeTextColor = Color.White;
                    HoverBadgeColor = Color.FromArgb(85, 187, 245);
                    HoverBadgeTextColor = Color.White;
                    PressBadgeColor = Color.FromArgb(45, 147, 205);
                    PressBadgeTextColor = Color.White;
                    DisabledBackColor = Color.FromArgb(204, 204, 204);
                    DisabledBorderColor = Color.FromArgb(155, 155, 155);
                    DisabledForeColor = Color.FromArgb(136, 136, 136);
                    ThemeAuthor = "Taiizor";
                    ThemeName = "MetroLight";
                    break;
                case Style.Dark:
                    NormalColor = Color.FromArgb(32, 32, 32);
                    NormalBorderColor = Color.FromArgb(64, 64, 64);
                    NormalTextColor = Color.FromArgb(204, 204, 204);
                    HoverColor = Color.FromArgb(170, 170, 170);
                    HoverBorderColor = Color.FromArgb(170, 170, 170);
                    HoverTextColor = Color.White;
                    PressColor = Color.FromArgb(240, 240, 240);
                    PressBorderColor = Color.FromArgb(240, 240, 240);
                    PressTextColor = Color.White;
                    NormalBadgeColor = Color.FromArgb(65, 177, 225);
                    NormalBadgeTextColor = Color.White;
                    HoverBadgeColor = Color.FromArgb(85, 187, 245);
                    HoverBadgeTextColor = Color.White;
                    PressBadgeColor = Color.FromArgb(45, 147, 205);
                    PressBadgeTextColor = Color.White;
                    DisabledBackColor = Color.FromArgb(80, 80, 80);
                    DisabledBorderColor = Color.FromArgb(109, 109, 109);
                    DisabledForeColor = Color.FromArgb(109, 109, 109);
                    ThemeAuthor = "Taiizor";
                    ThemeName = "MetroDark";
                    break;
                case Style.Custom:
                    if (StyleManager != null)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, object> varkey in StyleManager.BadgeDictionary)
                        {
                            if (varkey.Key == null)
                            {
                                return;
                            }

                            switch (varkey.Key)
                            {
                                case "NormalColor":
                                    NormalColor = _utl.HexColor((string)varkey.Value);
                                    break;
                                case "NormalBorderColor":
                                    NormalBorderColor = _utl.HexColor((string)varkey.Value);
                                    break;
                                case "NormalTextColor":
                                    NormalTextColor = _utl.HexColor((string)varkey.Value);
                                    break;
                                case "HoverColor":
                                    HoverColor = _utl.HexColor((string)varkey.Value);
                                    break;
                                case "HoverBorderColor":
                                    HoverBorderColor = _utl.HexColor((string)varkey.Value);
                                    break;
                                case "HoverTextColor":
                                    HoverTextColor = _utl.HexColor((string)varkey.Value);
                                    break;
                                case "PressColor":
                                    PressColor = _utl.HexColor((string)varkey.Value);
                                    break;
                                case "PressBorderColor":
                                    PressBorderColor = _utl.HexColor((string)varkey.Value);
                                    break;
                                case "PressTextColor":
                                    PressTextColor = _utl.HexColor((string)varkey.Value);
                                    break;
                                case "NormalBadgeColor":
                                    NormalBadgeColor = _utl.HexColor((string)varkey.Value);
                                    break;
                                case "NormalBadgeTextColor":
                                    NormalBadgeTextColor = _utl.HexColor((string)varkey.Value);
                                    break;
                                case "HoverBadgeColor":
                                    HoverBadgeColor = _utl.HexColor((string)varkey.Value);
                                    break;
                                case "HoverBadgeTextColor":
                                    HoverBadgeTextColor = _utl.HexColor((string)varkey.Value);
                                    break;
                                case "PressBadgeColor":
                                    PressBadgeColor = _utl.HexColor((string)varkey.Value);
                                    break;
                                case "PressBadgeTextColor":
                                    PressBadgeTextColor = _utl.HexColor((string)varkey.Value);
                                    break;
                                case "DisabledBackColor":
                                    DisabledBackColor = _utl.HexColor((string)varkey.Value);
                                    break;
                                case "DisabledBorderColor":
                                    DisabledBorderColor = _utl.HexColor((string)varkey.Value);
                                    break;
                                case "DisabledForeColor":
                                    DisabledForeColor = _utl.HexColor((string)varkey.Value);
                                    break;
                            }
                        }
                    }

                    Invalidate();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(style), style, null);
            }
        }

        #endregion Theme Changing

        #region Events

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _state = MouseMode.Hovered;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _state = MouseMode.Pushed;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _state = MouseMode.Hovered;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseEnter(e);
            _state = MouseMode.Normal;
            Invalidate();
        }

        #endregion Events

        #region Methods

        private void SmoothingType(Graphics e, SmoothingMode state = SmoothingMode.AntiAlias)
        {
            e.SmoothingMode = state;
        }

        #endregion Methods
    }

    #endregion
}