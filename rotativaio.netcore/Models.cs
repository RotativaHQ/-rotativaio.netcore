using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RotativaIO.NetCore
{

    public class Margins
    {
        /// <summary>
        /// Page bottom margin in mm.
        /// </summary>
        [OptionFlag("-B")]
        public int? Bottom;

        /// <summary>
        /// Page left margin in mm.
        /// </summary>
        [OptionFlag("-L")]
        public int? Left;

        /// <summary>
        /// Page right margin in mm.
        /// </summary>
        [OptionFlag("-R")]
        public int? Right;

        /// <summary>
        /// Page top margin in mm.
        /// </summary>
        [OptionFlag("-T")]
        public int? Top;

        public Margins()
        {
        }

        /// <summary>
        /// Sets the page margins.
        /// </summary>
        /// <param name="top">Page top margin in mm.</param>
        /// <param name="right">Page right margin in mm.</param>
        /// <param name="bottom">Page bottom margin in mm.</param>
        /// <param name="left">Page left margin in mm.</param>
        public Margins(int top, int right, int bottom, int left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            FieldInfo[] fields = GetType().GetFields();
            foreach (FieldInfo fi in fields)
            {
                var of = fi.GetCustomAttributes(typeof(OptionFlag), true).FirstOrDefault() as OptionFlag;
                if (of == null)
                    continue;

                object value = fi.GetValue(this);
                if (value != null)
                    result.AppendFormat(CultureInfo.InvariantCulture, " {0} {1}", of.Name, value);
            }

            return result.ToString().Trim();
        }
    }

    public class OptionFlag : Attribute
    {
        public string Name { get; private set; }

        public OptionFlag(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Page size.
    /// </summary>
    public enum Size
    {
        /// <summary>
        /// 841 x 1189 mm
        /// </summary>
        A0,

        /// <summary>
        /// 594 x 841 mm
        /// </summary>
        A1,

        /// <summary>
        /// 420 x 594 mm
        /// </summary>
        A2,

        /// <summary>
        /// 297 x 420 mm
        /// </summary>
        A3,

        /// <summary>
        /// 210 x 297 mm
        /// </summary>
        A4,

        /// <summary>
        /// 148 x 210 mm
        /// </summary>
        A5,

        /// <summary>
        /// 105 x 148 mm
        /// </summary>
        A6,

        /// <summary>
        /// 74 x 105 mm
        /// </summary>
        A7,

        /// <summary>
        /// 52 x 74 mm
        /// </summary>
        A8,

        /// <summary>
        /// 37 x 52 mm
        /// </summary>
        A9,

        /// <summary>
        /// 1000 x 1414 mm
        /// </summary>
        B0,

        /// <summary>
        /// 707 x 1000 mm
        /// </summary>
        B1,

        /// <summary>
        /// 500 x 707 mm
        /// </summary>
        B2,

        /// <summary>
        /// 353 x 500 mm
        /// </summary>
        B3,

        /// <summary>
        /// 250 x 353 mm
        /// </summary>
        B4,

        /// <summary>
        /// 176 x 250 mm
        /// </summary>
        B5,

        /// <summary>
        /// 125 x 176 mm
        /// </summary>
        B6,

        /// <summary>
        /// 88 x 125 mm
        /// </summary>
        B7,

        /// <summary>
        /// 62 x 88 mm
        /// </summary>
        B8,

        /// <summary>
        /// 33 x 62 mm
        /// </summary>
        B9,

        /// <summary>
        /// 31 x 44 mm
        /// </summary>
        B10,

        /// <summary>
        /// 163 x 229 mm
        /// </summary>
        C5E,

        /// <summary>
        /// 105 x 241 mm - U.S. Common 10 Envelope
        /// </summary>
        Comm10E,

        /// <summary>
        /// 110 x 220 mm
        /// </summary>
        Dle,

        /// <summary>
        /// 190.5 x 254 mm
        /// </summary>
        Executive,

        /// <summary>
        /// 210 x 330 mm
        /// </summary>
        Folio,

        /// <summary>
        /// 431.8 x 279.4 mm
        /// </summary>
        Ledger,

        /// <summary>
        /// 215.9 x 355.6 mm
        /// </summary>
        Legal,

        /// <summary>
        /// 215.9 x 279.4 mm
        /// </summary>
        Letter,

        /// <summary>
        /// 279.4 x 431.8 mm
        /// </summary>
        Tabloid
    }

    /// <summary>
    /// Page orientation.
    /// </summary>
    public enum Orientation
    {
        Landscape,
        Portrait
    }

    public class RotativaOptions
    {

        /// <summary>
        /// Sets the page margins.
        /// </summary>
        public Margins PageMargins { get; set; }

        /// <summary>
        /// Sets the page size.
        /// </summary>
        [OptionFlag("-s")]
        public Size? PageSize { get; set; }

        /// <summary>
        /// Sets the page width in mm.
        /// </summary>
        /// <remarks>Has priority over <see cref="PageSize"/> but <see cref="PageHeight"/> has to be also specified.</remarks>
        [OptionFlag("--page-width")]
        public double? PageWidth { get; set; }

        /// <summary>
        /// Sets the page height in mm.
        /// </summary>
        /// <remarks>Has priority over <see cref="PageSize"/> but <see cref="PageWidth"/> has to be also specified.</remarks>
        [OptionFlag("--page-height")]
        public double? PageHeight { get; set; }

        /// <summary>
        /// Sets the page orientation.
        /// </summary>
        [OptionFlag("-O")]
        public Orientation? PageOrientation { get; set; }

        /// <summary>
        /// Indicates whether the page can run JavaScript.
        /// </summary>
        [OptionFlag("-n")]
        public bool IsJavaScriptDisabled { get; set; }

        /// <summary>
        /// Indicates whether the PDF should be generated in lower quality.
        /// </summary>
        [OptionFlag("-l")]
        public bool IsLowQuality { get; set; }

        /// <summary>
        /// Indicates whether the page background should be disabled.
        /// </summary>
        [OptionFlag("--no-background")]
        public bool IsBackgroundDisabled { get; set; }

        /// <summary>
        /// Minimum font size.
        /// </summary>
        [OptionFlag("--minimum-font-size")]
        public int? MinimumFontSize { get; set; }

        /// <summary>
        /// Number of copies to print into the PDF file.
        /// </summary>
        [OptionFlag("--copies")]
        public int? Copies { get; set; }

        /// <summary>
        /// Indicates whether the PDF should be generated in grayscale.
        /// </summary>
        [OptionFlag("-g")]
        public bool IsGrayScale { get; set; }

        /// <summary>
        /// Use this if you need another switches that are not currently supported by Rotativa.
        /// </summary>
        [OptionFlag("")]
        public string CustomSwitches { get; set; }
    }
}
