using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace uploader
{
    /// <summary>
    /// Windows 11 (Fluent) dark palette, fonts and window chrome helpers.
    /// </summary>
    internal static class Theme
    {
        public static readonly Color Background = Color.FromArgb(32, 32, 32);
        public static readonly Color Card = Color.FromArgb(43, 43, 43);
        public static readonly Color CardBorder = Color.FromArgb(29, 29, 29);

        public static readonly Color Control = Color.FromArgb(55, 55, 55);
        public static readonly Color ControlHover = Color.FromArgb(61, 61, 61);
        public static readonly Color ControlPressed = Color.FromArgb(48, 48, 48);
        public static readonly Color ControlBorder = Color.FromArgb(67, 67, 67);
        public static readonly Color ControlBorderBottom = Color.FromArgb(78, 78, 78);

        public static readonly Color Input = Color.FromArgb(45, 45, 45);
        public static readonly Color InputFocused = Color.FromArgb(31, 31, 31);

        public static readonly Color Text = Color.FromArgb(255, 255, 255);
        public static readonly Color TextSecondary = Color.FromArgb(200, 200, 200);
        public static readonly Color TextDisabled = Color.FromArgb(120, 120, 120);
        public static readonly Color TextOnAccent = Color.FromArgb(0, 0, 0);

        public static readonly Color Accent = ReadAccent();
        public static readonly Color AccentHover = Blend(Accent, Background, 0.9f);
        public static readonly Color AccentPressed = Blend(Accent, Background, 0.8f);

        private static readonly string UiFamily = PickFamily("Segoe UI Variable Text", "Segoe UI");
        private static readonly string UiFamilySemibold = PickFamily("Segoe UI Variable Text Semibold", "Segoe UI Semibold", "Segoe UI");
        private static readonly string MonoFamily = PickFamily("Cascadia Mono", "Consolas");
        public static readonly string IconFamily = PickFamily("Segoe Fluent Icons", "Segoe MDL2 Assets");

        public static Font Body => new Font(UiFamily, 10.5f, FontStyle.Regular, GraphicsUnit.Point);
        public static Font BodyStrong => new Font(UiFamilySemibold, 10.5f, FontStyle.Regular, GraphicsUnit.Point);
        public static Font Subtitle => new Font(UiFamilySemibold, 12f, FontStyle.Regular, GraphicsUnit.Point);
        public static Font Mono => new Font(MonoFamily, 10f, FontStyle.Regular, GraphicsUnit.Point);
        public static Font Icon(float size) => new Font(IconFamily, size, FontStyle.Regular, GraphicsUnit.Point);

        public const float CornerRadius = 4f;
        public const float CardRadius = 8f;

        private static string PickFamily(params string[] names)
        {
            foreach (var name in names)
            {
                using (var font = new Font(name, 10f))
                {
                    if (string.Equals(font.Name, name, StringComparison.OrdinalIgnoreCase)) return name;
                }
            }
            return names[names.Length - 1];
        }

        private static Color Blend(Color a, Color b, float amount)
        {
            return Color.FromArgb(
                (int)(a.R * amount + b.R * (1 - amount)),
                (int)(a.G * amount + b.G * (1 - amount)),
                (int)(a.B * amount + b.B * (1 - amount)));
        }

        /// <summary>
        /// Windows 11 dark mode uses the "Light 2" shade of the user's accent colour for primary controls.
        /// </summary>
        private static Color ReadAccent()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Accent"))
                {
                    if (key?.GetValue("AccentPalette") is byte[] palette && palette.Length >= 8)
                    {
                        return Color.FromArgb(palette[4], palette[5], palette[6]);
                    }
                }
            }
            catch
            {
                // Fall through to the default Windows 11 accent.
            }
            return Color.FromArgb(96, 205, 255);
        }

        public static float Scale(Control control)
        {
            return control.DeviceDpi / 96f;
        }

        private static readonly float SystemDpi = GetSystemDpi();

        private static float GetSystemDpi()
        {
            using (var g = Graphics.FromHwnd(IntPtr.Zero))
            {
                return g.DpiY;
            }
        }

        /// <summary>
        /// Native controls get their HFONT sized for the system DPI. When the window sits on a monitor with a
        /// different DPI (PerMonitorV2), the font has to be enlarged by hand or the text renders too small.
        /// </summary>
        public static Font ForNativeControl(Font font, Control control)
        {
            if (Math.Abs(control.DeviceDpi - SystemDpi) < 0.5f) return font;
            return new Font(font.FontFamily, font.SizeInPoints * control.DeviceDpi / SystemDpi, font.Style, GraphicsUnit.Point);
        }

        public static GraphicsPath RoundedRect(RectangleF rect, float radius)
        {
            var path = new GraphicsPath();
            var d = radius * 2;
            if (d <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Paints the area behind a control with its parent's colour, so rounded corners blend in.
        /// </summary>
        public static void PaintParentBackground(Control control, Graphics g)
        {
            var parentColor = control.Parent?.BackColor ?? Background;
            using (var brush = new SolidBrush(parentColor))
            {
                g.FillRectangle(brush, control.ClientRectangle);
            }
        }

        #region DWM

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
        private const int DWMWA_CAPTION_COLOR = 35;
        private const int DWMWCP_ROUND = 2;

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int value, int size);

        /// <summary>
        /// Dark title bar, rounded corners and a caption that matches the window background (Windows 11).
        /// Silently ignored on older systems.
        /// </summary>
        public static void ApplyWindowChrome(Form form)
        {
            try
            {
                var handle = form.Handle;
                var dark = 1;
                DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref dark, sizeof(int));

                var corner = DWMWCP_ROUND;
                DwmSetWindowAttribute(handle, DWMWA_WINDOW_CORNER_PREFERENCE, ref corner, sizeof(int));

                var caption = ColorTranslator.ToWin32(Background);
                DwmSetWindowAttribute(handle, DWMWA_CAPTION_COLOR, ref caption, sizeof(int));
            }
            catch (DllNotFoundException)
            {
            }
            catch (EntryPointNotFoundException)
            {
            }
        }

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string appName, string idList);

        /// <summary>
        /// Dark scrollbars / dropdowns for native child windows.
        /// </summary>
        public static void ApplyDarkControlTheme(IntPtr handle, string theme = "DarkMode_Explorer")
        {
            try
            {
                SetWindowTheme(handle, theme, null);
            }
            catch (DllNotFoundException)
            {
            }
        }

        #endregion
    }
}
