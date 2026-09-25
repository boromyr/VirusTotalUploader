using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace uploader
{
    /// <summary>
    /// Base form with the Windows 11 dark look: Segoe UI Variable, dark caption and rounded corners.
    /// </summary>
    public class ModernForm : Form
    {
        public ModernForm()
        {
            BackColor = Theme.Background;
            ForeColor = Theme.Text;
            Font = Theme.Body;
            DoubleBuffered = true;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Theme.ApplyWindowChrome(this);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            BringToForeground();
        }

        /// <summary>
        /// Raises the window above the others and tries to give it focus. Windows may refuse the focus
        /// (it then flashes the taskbar button), but toggling TopMost still puts the window on top.
        /// </summary>
        public void BringToForeground()
        {
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;

            TopMost = true;
            TopMost = false;
            Activate();
        }
    }

    /// <summary>
    /// Rounded "settings card" surface.
    /// </summary>
    public class ModernCard : Panel
    {
        public ModernCard()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            BackColor = Theme.Card;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            var g = e.Graphics;
            Theme.PaintParentBackground(this, g);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var s = Theme.Scale(this);
            var rect = new RectangleF(0.5f, 0.5f, Width - 1f, Height - 1f);
            using (var path = Theme.RoundedRect(rect, Theme.CardRadius * s))
            using (var fill = new SolidBrush(Theme.Card))
            using (var border = new Pen(Theme.CardBorder))
            {
                g.FillPath(fill, path);
                g.DrawPath(border, path);
            }
        }
    }

    /// <summary>
    /// Fluent button. <see cref="Accent"/> renders it as the primary (accent coloured) action.
    /// </summary>
    public class ModernButton : Button
    {
        private bool _hover;
        private bool _pressed;
        private bool _accent;

        public ModernButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            FlatStyle = FlatStyle.Flat;
            Cursor = Cursors.Hand;
        }

        [DefaultValue(false)]
        public bool Accent
        {
            get => _accent;
            set { _accent = value; Invalidate(); }
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            var s = Theme.Scale(this);
            var text = TextRenderer.MeasureText(Text, Font);
            var width = Math.Max(text.Width + (int)(32 * s), (int)(120 * s));
            var height = Math.Max(text.Height + (int)(12 * s), (int)(32 * s));
            return new Size(width, height);
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; _pressed = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { _pressed = true; Invalidate(); }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _pressed = false;
            Invalidate();
            base.OnMouseUp(e);
        }

        protected override void OnEnabledChanged(EventArgs e) { Invalidate(); base.OnEnabledChanged(e); }
        protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            Theme.PaintParentBackground(this, g);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var s = Theme.Scale(this);
            Color fill, border, text;
            if (!Enabled)
            {
                fill = _accent ? Color.FromArgb(67, 67, 67) : Theme.ControlPressed;
                border = _accent ? fill : Theme.ControlBorder;
                text = Theme.TextDisabled;
            }
            else if (_accent)
            {
                fill = _pressed ? Theme.AccentPressed : _hover ? Theme.AccentHover : Theme.Accent;
                border = fill;
                text = Theme.TextOnAccent;
            }
            else
            {
                fill = _pressed ? Theme.ControlPressed : _hover ? Theme.ControlHover : Theme.Control;
                border = Theme.ControlBorder;
                text = _pressed ? Theme.TextSecondary : Theme.Text;
            }

            var rect = new RectangleF(0.5f, 0.5f, Width - 1f, Height - 1f);
            if (Focused && ShowFocusCues)
            {
                using (var ring = Theme.RoundedRect(rect, (Theme.CornerRadius + 2) * s))
                using (var pen = new Pen(Theme.Text, 2 * s))
                {
                    pen.Alignment = PenAlignment.Inset;
                    g.DrawPath(pen, ring);
                }
                rect.Inflate(-3 * s, -3 * s);
            }

            using (var path = Theme.RoundedRect(rect, Theme.CornerRadius * s))
            using (var brush = new SolidBrush(fill))
            using (var pen = new Pen(border))
            {
                g.FillPath(brush, path);
                g.DrawPath(pen, path);
            }

            TextRenderer.DrawText(g, Text, Font, ClientRectangle, text,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);
        }
    }

    /// <summary>
    /// Rounded text box with the Windows 11 accent underline when focused.
    /// </summary>
    [DefaultEvent("TextChanged")]
    public class ModernTextBox : Control
    {
        private readonly TextBox _inner = new TextBox();

        public ModernTextBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.Selectable, true);

            _inner.BorderStyle = BorderStyle.None;
            _inner.BackColor = Theme.Input;
            _inner.ForeColor = Theme.Text;
            _inner.GotFocus += (sender, e) => UpdateFocusState();
            _inner.LostFocus += (sender, e) => UpdateFocusState();
            _inner.TextChanged += (sender, e) => OnTextChanged(e);
            _inner.SizeChanged += (sender, e) => LayoutInner();
            _inner.LocationChanged += (sender, e) => LayoutInner();
            Controls.Add(_inner);

            BackColor = Theme.Input;
            Cursor = Cursors.IBeam;
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get => _inner.Text;
            set => _inner.Text = value;
        }

        [DefaultValue(false)]
        public bool ReadOnly
        {
            get => _inner.ReadOnly;
            set => _inner.ReadOnly = value;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            UpdateInnerFont();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateInnerFont();
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            UpdateInnerFont();
        }

        private void UpdateInnerFont()
        {
            _inner.Font = Theme.ForNativeControl(Font, this);
            PerformLayout();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            _inner.Focus();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _inner.Focus();
        }

        private void UpdateFocusState()
        {
            _inner.BackColor = _inner.Focused ? Theme.InputFocused : Theme.Input;
            Invalidate();
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);
            LayoutInner();
        }

        private bool _layingOut;

        /// <summary>
        /// Keeps the inner box centred; DPI scaling moves and resizes it after our own layout pass.
        /// </summary>
        private void LayoutInner()
        {
            if (_layingOut) return;
            _layingOut = true;
            try
            {
                var s = Theme.Scale(this);
                var padX = (int)(11 * s);
                _inner.Width = Math.Max(0, Width - padX * 2);
                _inner.Location = new Point(padX, (Height - _inner.Height) / 2);
            }
            finally
            {
                _layingOut = false;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            Theme.PaintParentBackground(this, g);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var s = Theme.Scale(this);
            var focused = _inner.Focused;
            var rect = new RectangleF(0.5f, 0.5f, Width - 1f, Height - 1f);
            var radius = Theme.CornerRadius * s;

            using (var path = Theme.RoundedRect(rect, radius))
            using (var fill = new SolidBrush(focused ? Theme.InputFocused : Theme.Input))
            using (var border = new Pen(Theme.ControlBorder))
            {
                g.FillPath(fill, path);
                g.DrawPath(border, path);

                // Bottom stroke: accent and thicker when focused, like WinUI's TextBox.
                var thickness = focused ? Math.Max(2, (int)Math.Round(2 * s)) : Math.Max(1, (int)Math.Round(s));
                var clip = g.Clip;
                g.SetClip(new RectangleF(0, Height - thickness, Width, thickness));
                using (var bottom = new SolidBrush(focused ? Theme.Accent : Color.FromArgb(140, 140, 140)))
                {
                    g.FillPath(bottom, path);
                }
                g.Clip = clip;
            }
        }
    }

    /// <summary>
    /// Fluent check box with a rounded, accent-filled box.
    /// </summary>
    public class ModernCheckBox : CheckBox
    {
        private bool _hover;

        public ModernCheckBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            Cursor = Cursors.Hand;
        }

        private int BoxSize => (int)Math.Round(20 * Theme.Scale(this));
        private int Gap => (int)Math.Round(10 * Theme.Scale(this));

        public override Size GetPreferredSize(Size proposedSize)
        {
            var text = TextRenderer.MeasureText(Text, Font);
            var s = Theme.Scale(this);
            return new Size(BoxSize + Gap + text.Width + (int)(4 * s), Math.Max(BoxSize, text.Height) + (int)(8 * s));
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            Theme.PaintParentBackground(this, g);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var s = Theme.Scale(this);
            var box = BoxSize;
            var boxRect = new RectangleF(0.5f, (Height - box) / 2f + 0.5f, box - 1f, box - 1f);

            using (var path = Theme.RoundedRect(boxRect, Theme.CornerRadius * s))
            {
                if (Checked)
                {
                    using (var fill = new SolidBrush(_hover ? Theme.AccentHover : Theme.Accent))
                    {
                        g.FillPath(fill, path);
                    }

                    using (var pen = new Pen(Theme.TextOnAccent, 1.6f * s) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
                    {
                        var x = boxRect.X;
                        var y = boxRect.Y;
                        var w = boxRect.Width;
                        g.DrawLines(pen, new[]
                        {
                            new PointF(x + w * 0.26f, y + w * 0.52f),
                            new PointF(x + w * 0.43f, y + w * 0.68f),
                            new PointF(x + w * 0.75f, y + w * 0.34f)
                        });
                    }
                }
                else
                {
                    using (var fill = new SolidBrush(_hover ? Theme.ControlHover : Theme.Input))
                    using (var pen = new Pen(Color.FromArgb(160, 160, 160)))
                    {
                        g.FillPath(fill, path);
                        g.DrawPath(pen, path);
                    }
                }
            }

            var textRect = new Rectangle(box + Gap, 0, Width - box - Gap, Height);
            TextRenderer.DrawText(g, Text, Font, textRect, Enabled ? ForeColor : Theme.TextDisabled,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);

            if (Focused && ShowFocusCues)
            {
                using (var pen = new Pen(Theme.Text, 1.5f * s))
                using (var ring = Theme.RoundedRect(new RectangleF(boxRect.X - 2 * s + 1, boxRect.Y - 2 * s, boxRect.Width + 4 * s - 1, boxRect.Height + 4 * s), (Theme.CornerRadius + 2) * s))
                {
                    g.DrawPath(pen, ring);
                }
            }
        }
    }

    /// <summary>
    /// Drop-down list styled as a WinUI ComboBox. The closed state is painted entirely by us.
    /// </summary>
    public class ModernComboBox : ComboBox
    {
        private const int WM_PAINT = 0x000F;
        private bool _hover;

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT { public int Left, Top, Right, Bottom; }

        [StructLayout(LayoutKind.Sequential)]
        private struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public bool fErase;
            public RECT rcPaint;
            public bool fRestore;
            public bool fIncUpdate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] public byte[] rgbReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct COMBOBOXINFO
        {
            public int cbSize;
            public RECT rcItem;
            public RECT rcButton;
            public int stateButton;
            public IntPtr hwndCombo;
            public IntPtr hwndItem;
            public IntPtr hwndList;
        }

        [DllImport("user32.dll")] private static extern IntPtr BeginPaint(IntPtr hWnd, out PAINTSTRUCT lpPaint);
        [DllImport("user32.dll")] private static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);
        [DllImport("user32.dll")] private static extern bool GetComboBoxInfo(IntPtr hWnd, ref COMBOBOXINFO pcbi);

        public ModernComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawFixed;
            FlatStyle = FlatStyle.Flat;
            BackColor = Theme.Card;
            ForeColor = Theme.Text;
            Cursor = Cursors.Hand;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateItemHeight();

            var info = new COMBOBOXINFO { cbSize = Marshal.SizeOf(typeof(COMBOBOXINFO)) };
            if (GetComboBoxInfo(Handle, ref info) && info.hwndList != IntPtr.Zero)
            {
                Theme.ApplyDarkControlTheme(info.hwndList);
            }
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            UpdateItemHeight();
        }

        private void UpdateItemHeight()
        {
            // The closed height of an owner-drawn drop-down list follows ItemHeight.
            ItemHeight = (int)Math.Round(26 * Theme.Scale(this));
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnDropDown(EventArgs e) { Invalidate(); base.OnDropDown(e); }
        protected override void OnDropDownClosed(EventArgs e) { Invalidate(); base.OnDropDownClosed(e); }
        protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
        protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }
        protected override void OnSelectedIndexChanged(EventArgs e) { Invalidate(); base.OnSelectedIndexChanged(e); }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            var g = e.Graphics;
            var s = Theme.Scale(this);
            var selected = (e.State & DrawItemState.Selected) != 0;

            using (var bg = new SolidBrush(selected ? Theme.ControlHover : Theme.Card))
            {
                g.FillRectangle(bg, e.Bounds);
            }

            if (selected)
            {
                // Accent "pill" on the left of the highlighted item.
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var pillHeight = e.Bounds.Height * 0.5f;
                var pill = new RectangleF(e.Bounds.X + 2 * s, e.Bounds.Y + (e.Bounds.Height - pillHeight) / 2, 3 * s, pillHeight);
                using (var path = Theme.RoundedRect(pill, 1.5f * s))
                using (var brush = new SolidBrush(Theme.Accent))
                {
                    g.FillPath(brush, path);
                }
            }

            var textRect = new Rectangle(e.Bounds.X + (int)(12 * s), e.Bounds.Y, e.Bounds.Width - (int)(12 * s), e.Bounds.Height);
            TextRenderer.DrawText(g, GetItemText(Items[e.Index]), Font, textRect, Theme.Text,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg != WM_PAINT)
            {
                base.WndProc(ref m);
                return;
            }

            BeginPaint(Handle, out var ps);
            try
            {
                using (var buffer = new Bitmap(Math.Max(1, Width), Math.Max(1, Height)))
                {
                    using (var g = Graphics.FromImage(buffer))
                    {
                        PaintClosed(g);
                    }
                    using (var target = Graphics.FromHdc(ps.hdc))
                    {
                        // Explicit size: the bitmap and the window DC may report different DPIs.
                        target.DrawImage(buffer, 0, 0, buffer.Width, buffer.Height);
                    }
                }
            }
            finally
            {
                EndPaint(Handle, ref ps);
            }
        }

        private void PaintClosed(Graphics g)
        {
            Theme.PaintParentBackground(this, g);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var s = Theme.Scale(this);
            var fill = DroppedDown ? Theme.ControlPressed : _hover ? Theme.ControlHover : Theme.Control;
            var rect = new RectangleF(0.5f, 0.5f, Width - 1f, Height - 1f);

            using (var path = Theme.RoundedRect(rect, Theme.CornerRadius * s))
            using (var brush = new SolidBrush(fill))
            using (var pen = new Pen(Focused && !DroppedDown ? Theme.Accent : Theme.ControlBorder))
            {
                g.FillPath(brush, path);
                g.DrawPath(pen, path);
            }

            var chevronWidth = (int)(32 * s);
            var textRect = new Rectangle((int)(11 * s), 0, Width - (int)(11 * s) - chevronWidth, Height);
            TextRenderer.DrawText(g, Text, Font, textRect, Enabled ? Theme.Text : Theme.TextDisabled,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);

            // Chevron
            var cx = Width - chevronWidth / 2f - 2 * s;
            var cy = Height / 2f;
            var half = 4.5f * s;
            using (var pen = new Pen(Theme.TextSecondary, 1.2f * s) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
            {
                g.DrawLines(pen, new[]
                {
                    new PointF(cx - half, cy - half / 2),
                    new PointF(cx, cy + half / 2),
                    new PointF(cx + half, cy - half / 2)
                });
            }
        }
    }

    /// <summary>
    /// Dashed drop target shown on the main window.
    /// </summary>
    public class DropZone : Control
    {
        private bool _highlight;

        public DropZone()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            AllowDrop = true;
        }

        [DefaultValue(false)]
        public bool Highlight
        {
            get => _highlight;
            set { _highlight = value; Invalidate(); }
        }

        protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            Theme.PaintParentBackground(this, g);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var s = Theme.Scale(this);
            var rect = new RectangleF(s, s, Width - 2 * s - 1, Height - 2 * s - 1);
            using (var path = Theme.RoundedRect(rect, Theme.CardRadius * s))
            using (var fill = new SolidBrush(_highlight ? Theme.ControlHover : Theme.Card))
            using (var pen = new Pen(_highlight ? Theme.Accent : Color.FromArgb(90, 90, 90), 1.5f * s) { DashStyle = DashStyle.Dash, DashPattern = new[] { 4f, 3f } })
            {
                g.FillPath(fill, path);
                g.DrawPath(pen, path);
            }

            using (var iconFont = Theme.Icon(28f))
            {
                var iconSize = TextRenderer.MeasureText("", iconFont);
                var textSize = TextRenderer.MeasureText(Text, Font);
                var spacing = (int)(12 * s);
                var top = (Height - iconSize.Height - spacing - textSize.Height) / 2;

                TextRenderer.DrawText(g, "", iconFont, new Rectangle(0, top, Width, iconSize.Height),
                    _highlight ? Theme.Accent : Theme.TextSecondary, TextFormatFlags.HorizontalCenter | TextFormatFlags.NoPadding);
                TextRenderer.DrawText(g, Text, Font, new Rectangle(0, top + iconSize.Height + spacing, Width, textSize.Height),
                    Theme.Text, TextFormatFlags.HorizontalCenter | TextFormatFlags.NoPadding);
            }
        }
    }
}
