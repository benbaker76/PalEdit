using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ControlsEx.ColorManagement.ColorModels.Selection
{
    public class ColorSelectionModuleAlpha : ColorSelectionModule
    {
        #region variables
        protected Color _color;
        #endregion
        #region properties
        public override XYZ XYZ
        {
            get { return XYZ.FromRGB(Color.Empty); }
            set { }
        }
        public Color Color
        {
            get { return _color; }
            set
            {
                //Color newcolor = value;
                //if (newcolor == _color) return;
                //_color = newcolor;
                _color = value;
                //update controls
                UpdateFaderImage();
                UpdateFaderPosition();
            }
        }
        #endregion

        #region colorfader
        protected override void OnUpdateFaderImage(Bitmap bmp)
        {
            using (Graphics gr = Graphics.FromImage(bmp))
            {
                using (HatchBrush hatchBrush = new HatchBrush(HatchStyle.LargeCheckerBoard, Color.Silver, Color.Gray))
                    gr.FillRectangle(hatchBrush, new Rectangle(Point.Empty, bmp.Size));

                using (LinearGradientBrush brs = new LinearGradientBrush(
                          new Point(0, 0), new Point(bmp.Width),
                          Color.FromArgb(0, _color),
                          Color.FromArgb(255, _color)))
                {
                    gr.FillRectangle(brs, new Rectangle(Point.Empty, bmp.Size));
                }
            }
        }
        protected override void OnUpdateFaderPosition(ColorSelectionFader fader)
        {
            fader.Position = (double)(_color.A) / 255.0;
        }
        protected override void OnFaderScroll(ColorSelectionFader fader)
        {
            int newalpha = Math.Max(0, Math.Min(255, (int)(fader.Position * 255.0)));
            if (newalpha == _color.A) return;
            _color = Color.FromArgb(newalpha, _color);
            RaiseColorChanged();
        }
        #endregion
        #region colorplane
        protected override void OnUpdatePlaneImage(Bitmap bmp)
        {
        }
        protected override void OnUpdatePlanePosition(ColorSelectionPlane plane)
        {
        }
        protected override void OnPlaneScroll(ColorSelectionPlane plane)
        {
        }
        #endregion
    }
}
