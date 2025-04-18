using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace PIA_MAD_FyD.Helpers
{
    public partial class BorderRadius: Component
    {
        [Flags]
        public enum RoundedCorners
        {
            None = 0,
            TopLeft = 1,
            TopRight = 2,
            BottomRight = 4,
            BottomLeft = 8,
            All = TopLeft | TopRight | BottomRight | BottomLeft
        }

        public BorderRadius()
        {
            InitializeComponent();
        }

        public BorderRadius(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
                    int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
                    int nWidthEllipse, int nHeightEllipse);

        private Control control;
        private int cornerRadius = 20;
        public Control TargetControl
        {
            get { return control; }
            set
            {
                control = value;
                control.SizeChanged += (sender, EventArgs) =>
                {
                    control.Region = new Region(GetRoundedPath(control.ClientRectangle, cornerRadius, CornersToRound));
                };
            }
        }


        public int CornerRadius
        {
            get { return cornerRadius; }
            set
            {
                cornerRadius = value;
                if (control != null)
                {
                    control.Region = new Region(GetRoundedPath(control.ClientRectangle, cornerRadius, CornersToRound));
                }
            }
        }

        public RoundedCorners CornersToRound { get; set; } = RoundedCorners.All;

        private GraphicsPath GetRoundedPath(Rectangle bounds, int radius, RoundedCorners corners)
        {
            int diameter = radius * 2;
            GraphicsPath path = new GraphicsPath();

            // Top Left
            if (corners.HasFlag(RoundedCorners.TopLeft))
                path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
            else
                path.AddLine(bounds.Left, bounds.Top, bounds.Left + radius, bounds.Top);

            // Top Right
            if (corners.HasFlag(RoundedCorners.TopRight))
                path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
            else
                path.AddLine(bounds.Right - radius, bounds.Top, bounds.Right, bounds.Top);

            // Bottom Right
            if (corners.HasFlag(RoundedCorners.BottomRight))
                path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            else
                path.AddLine(bounds.Right, bounds.Bottom - radius, bounds.Right, bounds.Bottom);

            // Bottom Left
            if (corners.HasFlag(RoundedCorners.BottomLeft))
                path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            else
                path.AddLine(bounds.Left + radius, bounds.Bottom, bounds.Left, bounds.Bottom);

            path.CloseFigure();
            return path;
        }

    }
}
