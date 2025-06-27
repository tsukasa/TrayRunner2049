using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace TrayRunner2049.Controls
{
    /// <summary>
    /// RichTextBox control that supports ANSI escape sequences for text formatting
    /// </summary>
    public class AnsiRichTextBox : RichTextBox
    {
        // Win32 API constants and imports for scrollbar manipulation
        private const int GWL_STYLE = -16;
        private const int SB_VERT = 1;
        private const int SIF_POS = 0x0004;
        private const int WS_VSCROLL = 0x00200000;

        // ANSI control code constants
        private const int AnsiReset = 0;
        private const int AnsiBold = 1;
        private const int AnsiDim = 2;
        private const int AnsiItalic = 3;
        private const int AnsiUnderline = 4;
        private const int AnsiNoBold = 22;
        private const int AnsiNoItalic = 23;
        private const int AnsiNoUnderline = 24;
        private const int AnsiDefaultForeground = 39;
        private const int AnsiDefaultBackground = 49;
        private const int AnsiForegroundStart = 30;
        private const int AnsiForegroundEnd = 37;
        private const int AnsiBackgroundStart = 40;
        private const int AnsiBackgroundEnd = 47;
        private const int AnsiBrightForegroundStart = 90;
        private const int AnsiBrightForegroundEnd = 97;
        private const int AnsiBrightBackgroundStart = 100;
        private const int AnsiBrightBackgroundEnd = 107;
        private const int AnsiForegroundExtended = 38;
        private const int AnsiBackgroundExtended = 48;
        private const int AnsiExtendedMode8Bit = 5;
        private const int AnsiExtendedMode24Bit = 2;

        // Match ANSI escape sequences - more robust pattern
        private static readonly Regex AnsiEscapeRegex = new Regex(@"\u001b\[([0-9;]*?)m", RegexOptions.Compiled);

        // Font cache to avoid creating new Font objects repeatedly
        private readonly Dictionary<FontStyle, Font> _fontCache = new Dictionary<FontStyle, Font>();

        // Color caches to avoid creating new Color objects repeatedly
        private readonly Dictionary<int, Color> _3BitColorCache = new Dictionary<int, Color>();
        private readonly Dictionary<int, Color> _4BitColorCache = new Dictionary<int, Color>();
        private readonly Dictionary<int, Color> _8BitColorCache = new Dictionary<int, Color>();
        private readonly Dictionary<(int R, int G, int B), Color> _rgbColorCache = new Dictionary<(int R, int G, int B), Color>();

        // Update control variables
        private int _updateCount;
        private int _savedScrollPos;
        private bool _hasVerticalScrollBar;
        private bool _scrollAfterUpdate;

        // Buffer for incomplete ANSI sequences
        private readonly StringBuilder _incompleteBuffer = new StringBuilder();

        // Default colors
        private Color _defaultForeColor;
        private Color _defaultBackColor;

        // Persistent formatting state for streaming input
        private Color _currentForeColor;
        private Color _currentBackColor;
        private FontStyle _currentStyle;

        [StructLayout(LayoutKind.Sequential)]
        private struct SCROLLINFO
        {
            public int cbSize;
            public int fMask;
            public int nMin;
            public int nMax;
            public int nPage;
            public int nPos;
            public int nTrackPos;

            public void Initialize()
            {
                cbSize = Marshal.SizeOf(typeof(SCROLLINFO));
            }
        }

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern bool GetScrollInfo(IntPtr hwnd, int fnBar, ref SCROLLINFO lpsi);

        [DllImport("user32.dll")]
        private static extern int SetScrollInfo(IntPtr hwnd, int fnBar, [In] ref SCROLLINFO lpsi, bool redraw);

        /// <summary>
        /// Initializes a new instance of the AnsiRichTextBox class with default formatting state
        /// </summary>
        public AnsiRichTextBox()
        {
            _defaultForeColor = ForeColor;
            _defaultBackColor = BackColor;

            _currentForeColor = _defaultForeColor;
            _currentBackColor = _defaultBackColor;
            _currentStyle = Font.Style;

            _fontCache[Font.Style] = new Font(Font.FontFamily, Font.Size, Font.Style);
        }

        /// <summary>
        /// Cleans up resources used by the control, including cached fonts
        /// </summary>
        /// <param name="disposing">True if managed resources should be disposed; otherwise, false</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose all cached fonts
                foreach (var font in _fontCache.Values)
                {
                    font.Dispose();
                }
                _fontCache.Clear();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Called when the Font property changes. Clears the font cache and updates current font style.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            ClearFontCache();
            _fontCache[Font.Style] = new Font(Font.FontFamily, Font.Size, Font.Style);
        }

        /// <summary>
        /// Called when the BackColor property changes. Updates the default and current background colors.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            _defaultBackColor = BackColor;
            _currentBackColor = BackColor;
        }

        /// <summary>
        /// Called when the ForeColor property changes. Updates the default and current foreground colors.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            _defaultForeColor = ForeColor;
            _currentForeColor = ForeColor;
        }

        /// <summary>
        /// Appends text containing ANSI escape sequences, interpreting them for formatting
        /// </summary>
        /// <param name="text">Text that may contain ANSI escape sequences</param>
        public void AppendAnsiText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            BeginUpdate();

            try
            {
                // Combine any incomplete buffer with new text
                string fullText = _incompleteBuffer + text;
                _incompleteBuffer.Clear();

                ProcessTextWithAnsiCodes(fullText);
            }
            finally
            {
                EndUpdate();
                ScrollToCaret();
            }
        }

        /// <summary>
        /// Processes text containing ANSI escape sequences and extracts formatting information
        /// </summary>
        /// <param name="fullText">The complete text string to process for ANSI escape sequences</param>
        private void ProcessTextWithAnsiCodes(string fullText)
        {
            int currentPosition = 0;

            MatchCollection matches = AnsiEscapeRegex.Matches(fullText);

            foreach (Match match in matches)
            {
                // Append any text before the escape sequence with current formatting
                if (match.Index > currentPosition)
                {
                    string textSegment = fullText.Substring(currentPosition, match.Index - currentPosition);
                    AppendFormattedText(textSegment, _currentForeColor, _currentBackColor, _currentStyle);
                }

                string escapeCode = match.Groups[1].Value;
                ProcessEscapeCode(escapeCode, ref _currentForeColor, ref _currentBackColor, ref _currentStyle);

                currentPosition = match.Index + match.Length;
            }

            ProcessRemainingText(fullText, currentPosition);
        }

        /// <summary>
        /// Processes any text that remains after all ANSI sequences have been handled.
        /// Handles incomplete ANSI sequences by buffering them for the next call.
        /// </summary>
        /// <param name="fullText">The complete text string being processed</param>
        /// <param name="currentPosition">The current position in the text after processing ANSI sequences</param>
        private void ProcessRemainingText(string fullText, int currentPosition)
        {
            if (currentPosition >= fullText.Length)
                return;

            string remainingText = fullText.Substring(currentPosition);

            // Check if the remaining text might contain an incomplete ANSI sequence
            int lastEscapeStart = remainingText.LastIndexOf('\u001b');
            if (lastEscapeStart >= 0)
            {
                // There's a potential incomplete escape sequence
                string beforeEscape = remainingText.Substring(0, lastEscapeStart);
                string potentialEscape = remainingText.Substring(lastEscapeStart);

                // Append the text before the potential escape
                if (!string.IsNullOrEmpty(beforeEscape))
                {
                    AppendFormattedText(beforeEscape, _currentForeColor, _currentBackColor, _currentStyle);
                }

                // Store the potential incomplete escape sequence for next time
                _incompleteBuffer.Append(potentialEscape);
            }
            else
            {
                // No escape sequences, append all remaining text
                AppendFormattedText(remainingText, _currentForeColor, _currentBackColor, _currentStyle);
            }
        }

        /// <summary>
        /// Clears the font cache and disposes font resources
        /// </summary>
        private void ClearFontCache()
        {
            foreach (var font in _fontCache.Values)
            {
                font.Dispose();
            }
            _fontCache.Clear();
        }

        /// <summary>
        /// Gets a cached font with the specified style or creates and caches a new one
        /// </summary>
        /// <param name="style">The font style to retrieve or create</param>
        /// <returns>A Font object with the specified style</returns>
        private Font GetCachedFont(FontStyle style)
        {
            if (!_fontCache.TryGetValue(style, out Font? font))
            {
                font = new Font(Font.FontFamily, Font.Size, style);
                _fontCache[style] = font;
            }

            return font;
        }

        /// <summary>
        /// Appends formatted text to the RichTextBox with the specified colors and font style
        /// </summary>
        /// <param name="text">The text to append</param>
        /// <param name="foreColor">The foreground color for the text</param>
        /// <param name="backColor">The background color for the text</param>
        /// <param name="style">The font style for the text</param>
        private void AppendFormattedText(string text, Color foreColor, Color backColor, FontStyle style)
        {
            SelectionStart = TextLength;
            SelectionLength = 0;
            SelectionColor = foreColor;
            SelectionBackColor = backColor;
            SelectionFont = GetCachedFont(style);
            AppendText(text);
        }

        /// <summary>
        /// Processes ANSI escape code sequences and updates the formatting state
        /// </summary>
        /// <param name="code">The ANSI escape code string (without the ESC[ prefix and m suffix)</param>
        /// <param name="foreColor">Reference to the current foreground color that may be modified</param>
        /// <param name="backColor">Reference to the current background color that may be modified</param>
        /// <param name="style">Reference to the current font style that may be modified</param>
        private void ProcessEscapeCode(string code, ref Color foreColor, ref Color backColor, ref FontStyle style)
        {
            // Handle empty escape codes (ESC[m is equivalent to ESC[0m)
            if (string.IsNullOrEmpty(code))
            {
                foreColor = _defaultForeColor;
                backColor = _defaultBackColor;
                style = FontStyle.Regular;
                return;
            }

            string[] codes = code.Split(';');
            for (int i = 0; i < codes.Length; i++)
            {
                // Handle empty codes (treat as 0)
                if (string.IsNullOrEmpty(codes[i]))
                {
                    foreColor = _defaultForeColor;
                    backColor = _defaultBackColor;
                    style = FontStyle.Regular;
                    continue;
                }

                if (!int.TryParse(codes[i], out int codeValue))
                    continue;

                switch (codeValue)
                {
                    // Reset all attributes
                    case AnsiReset:
                        foreColor = _defaultForeColor;
                        backColor = _defaultBackColor;
                        style = FontStyle.Regular;
                        break;

                    // Bold
                    case AnsiBold:
                        style |= FontStyle.Bold;
                        break;

                    // Dim (implemented as regular since there's no direct equivalent)
                    case AnsiDim:
                        style &= ~FontStyle.Bold; // Remove bold if present
                        break;

                    // Italic
                    case AnsiItalic:
                        style |= FontStyle.Italic;
                        break;

                    // Underline
                    case AnsiUnderline:
                        style |= FontStyle.Underline;
                        break;

                    // Reset font style modifiers
                    case AnsiNoBold: // Not bold or dim
                        style &= ~FontStyle.Bold;
                        break;
                    case AnsiNoItalic: // Not italic
                        style &= ~FontStyle.Italic;
                        break;
                    case AnsiNoUnderline: // Not underlined
                        style &= ~FontStyle.Underline;
                        break;

                    // Standard foreground colors (3-bit: 30-37)
                    case >= AnsiForegroundStart and <= AnsiForegroundEnd:
                        foreColor = Get3BitColor(codeValue - AnsiForegroundStart);
                        break;

                    // Reset to default foreground
                    case AnsiDefaultForeground:
                        foreColor = _defaultForeColor;
                        break;

                    // Standard background colors (3-bit: 40-47)
                    case >= AnsiBackgroundStart and <= AnsiBackgroundEnd:
                        backColor = Get3BitColor(codeValue - AnsiBackgroundStart);
                        break;

                    // Reset to default background
                    case AnsiDefaultBackground:
                        backColor = _defaultBackColor;
                        break;

                    // Bright foreground colors (4-bit: 90-97)
                    case >= AnsiBrightForegroundStart and <= AnsiBrightForegroundEnd:
                        foreColor = Get4BitColor(codeValue - AnsiBrightForegroundStart);
                        break;

                    // Bright background colors (4-bit: 100-107)
                    case >= AnsiBrightBackgroundStart and <= AnsiBrightBackgroundEnd:
                        backColor = Get4BitColor(codeValue - AnsiBrightBackgroundStart);
                        break;

                    // 8-bit and 24-bit color support
                    case AnsiForegroundExtended:
                        // Foreground with extended color
                        i = Process8BitAnd24BitColor(codes, i, ref foreColor);
                        break;

                    case AnsiBackgroundExtended:
                        // Background with extended color
                        i = Process8BitAnd24BitColor(codes, i, ref backColor);
                        break;
                }
            }
        }

        /// <summary>
        /// Processes 8-bit (256 color) and 24-bit (RGB) color codes
        /// </summary>
        /// <returns>The new index in the codes array</returns>
        private int Process8BitAnd24BitColor(string[] codes, int currentIndex, ref Color color)
        {
            if (currentIndex + 1 >= codes.Length)
                return currentIndex;

            if (!int.TryParse(codes[currentIndex + 1], out int mode))
                return currentIndex;

            // 8-bit color mode (38;5;n or 48;5;n)
            if (mode == AnsiExtendedMode8Bit && currentIndex + 2 < codes.Length)
            {
                if (int.TryParse(codes[currentIndex + 2], out int colorIndex))
                {
                    color = Get256Color(colorIndex);
                    return currentIndex + 2; // Skip the processed parameters
                }
            }
            // 24-bit RGB color mode (38;2;r;g;b or 48;2;r;g;b)
            else if (mode == AnsiExtendedMode24Bit && currentIndex + 4 < codes.Length)
            {
                if (int.TryParse(codes[currentIndex + 2], out int r) &&
                    int.TryParse(codes[currentIndex + 3], out int g) &&
                    int.TryParse(codes[currentIndex + 4], out int b))
                {
                    color = GetRgbColor(r, g, b);
                    return currentIndex + 4; // Skip the processed parameters
                }
            }

            return currentIndex;
        }

        /// <summary>
        /// Returns a color based on 3-bit ANSI color code (0-7)
        /// </summary>
        private Color Get3BitColor(int colorIndex)
        {
            if (_3BitColorCache.TryGetValue(colorIndex, out Color cachedColor))
                return cachedColor;

            Color color = colorIndex switch
            {
                0 => Color.Black,
                1 => Color.DarkRed,
                2 => Color.DarkGreen,
                3 => Color.DarkGoldenrod,
                4 => Color.DarkBlue,
                5 => Color.DarkMagenta,
                6 => Color.DarkCyan,
                7 => Color.LightGray,
                _ => Color.Black // Default
            };

            _3BitColorCache[colorIndex] = color;
            return color;
        }

        /// <summary>
        /// Returns a color based on 4-bit ANSI bright color code (0-7)
        /// </summary>
        private Color Get4BitColor(int colorIndex)
        {
            if (_4BitColorCache.TryGetValue(colorIndex, out Color cachedColor))
                return cachedColor;

            Color color = colorIndex switch
            {
                0 => Color.DarkGray,
                1 => Color.Red,
                2 => Color.Green,
                3 => Color.Yellow,
                4 => Color.Blue,
                5 => Color.Magenta,
                6 => Color.Cyan,
                7 => Color.White,
                _ => Color.White // Default
            };

            _4BitColorCache[colorIndex] = color;
            return color;
        }

        /// <summary>
        /// Returns a color using 24-bit RGB values
        /// </summary>
        private Color GetRgbColor(int r, int g, int b)
        {
            var rgbKey = (r, g, b);

            if (_rgbColorCache.TryGetValue(rgbKey, out Color cachedColor))
                return cachedColor;

            Color color = Color.FromArgb(
                Math.Clamp(r, 0, 255),
                Math.Clamp(g, 0, 255),
                Math.Clamp(b, 0, 255)
            );

            _rgbColorCache[rgbKey] = color;
            return color;
        }

        /// <summary>
        /// Returns a color based on 8-bit ANSI color code (0-255)
        /// </summary>
        private Color Get256Color(int colorIndex)
        {
            if (_8BitColorCache.TryGetValue(colorIndex, out Color cachedColor))
                return cachedColor;

            // Ensure the index is within bounds
            colorIndex = Math.Max(0, Math.Min(255, colorIndex));
            Color color;

            // 16 basic colors (0-15)
            if (colorIndex < 16)
            {
                color = colorIndex switch
                {
                    0 => Color.Black,
                    1 => Color.DarkRed,
                    2 => Color.DarkGreen,
                    3 => Color.DarkGoldenrod,
                    4 => Color.DarkBlue,
                    5 => Color.DarkMagenta,
                    6 => Color.DarkCyan,
                    7 => Color.LightGray,
                    8 => Color.DarkGray,
                    9 => Color.Red,
                    10 => Color.Green,
                    11 => Color.Yellow,
                    12 => Color.Blue,
                    13 => Color.Magenta,
                    14 => Color.Cyan,
                    15 => Color.White,
                    _ => Color.Black
                };
            }
            // 6x6x6 color cube (16-231)
            else if (colorIndex is >= 16 and <= 231)
            {
                int adjustedIndex = colorIndex - 16;
                int r = adjustedIndex / 36;
                int g = (adjustedIndex % 36) / 6;
                int b = adjustedIndex % 6;

                // Convert to RGB values (0, 95, 135, 175, 215, 255)
                r = (r == 0) ? 0 : 55 + r * 40;
                g = (g == 0) ? 0 : 55 + g * 40;
                b = (b == 0) ? 0 : 55 + b * 40;

                color = Color.FromArgb(r, g, b);
            }
            // Grayscale ramp (232-255)
            else
            {
                int gray = (colorIndex - 232) * 10 + 8;
                gray = Math.Min(255, Math.Max(0, gray));
                color = Color.FromArgb(gray, gray, gray);
            }

            _8BitColorCache[colorIndex] = color;
            return color;
        }

        /// <summary>
        /// Flushes any incomplete ANSI sequence in the buffer as plain text
        /// </summary>
        private void FlushIncompleteBuffer()
        {
            if (_incompleteBuffer.Length > 0)
            {
                AppendFormattedText(_incompleteBuffer.ToString(), _currentForeColor, _currentBackColor, _currentStyle);
                _incompleteBuffer.Clear();
            }
        }

        /// <summary>
        /// Gets the current formatting state for debugging
        /// </summary>
        public (Color ForeColor, Color BackColor, FontStyle Style) GetCurrentFormattingState()
        {
            return (_currentForeColor, _currentBackColor, _currentStyle);
        }

        /// <summary>
        /// Checks if the vertical scrollbar is visible
        /// </summary>
        private bool HasVerticalScrollBar()
        {
            int style = GetWindowLong(Handle, GWL_STYLE);
            return (style & WS_VSCROLL) != 0;
        }

        /// <summary>
        /// Gets the current vertical scroll position
        /// </summary>
        private int GetScrollPosition()
        {
            var si = new SCROLLINFO();
            si.Initialize();
            si.fMask = SIF_POS;

            if (GetScrollInfo(Handle, SB_VERT, ref si))
                return si.nPos;

            return 0;
        }

        /// <summary>
        /// Sets the vertical scroll position
        /// </summary>
        private void SetScrollPosition(int pos, bool redraw = true)
        {
            var si = new SCROLLINFO();
            si.Initialize();
            si.fMask = SIF_POS;
            si.nPos = pos;

            SetScrollInfo(Handle, SB_VERT, ref si, redraw);
        }

        /// <summary>
        /// Begins a batch update, suspending layout and scrolling
        /// </summary>
        private void BeginUpdate()
        {
            _updateCount++;

            // Suspend layout only if this is the first call
            if (_updateCount == 1)
            {
                SuspendLayout();

                _hasVerticalScrollBar = HasVerticalScrollBar();
                if (_hasVerticalScrollBar)
                {
                    _savedScrollPos = GetScrollPosition();

                    // Hide the scrollbar by modifying the window style (more efficient than toggling visibility)
                    int style = GetWindowLong(Handle, GWL_STYLE);
                    SetWindowLong(Handle, GWL_STYLE, style & ~WS_VSCROLL);
                }

                _scrollAfterUpdate = true;
            }
        }

        /// <summary>
        /// Ends a batch update, resuming layout and scrolling if not nested
        /// </summary>
        private void EndUpdate()
        {
            if (_updateCount <= 0) return;

            _updateCount--;

            // Resume layout only if this is the last call
            if (_updateCount == 0)
            {
                // Restore scrollbar if it was previously visible
                if (_hasVerticalScrollBar)
                {
                    // Restore the scrollbar by setting the window style back
                    int style = GetWindowLong(Handle, GWL_STYLE);
                    SetWindowLong(Handle, GWL_STYLE, style | WS_VSCROLL);

                    // Optionally restore scroll position
                    if (!_scrollAfterUpdate)
                        SetScrollPosition(_savedScrollPos);
                }

                ResumeLayout();

                // Force a repaint after all updates are complete
                Invalidate();
                Update();
            }
        }
    }
}
