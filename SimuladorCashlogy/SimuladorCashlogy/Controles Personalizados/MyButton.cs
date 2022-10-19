using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    #region Enums
    public enum IconChar
    {
        None,
        Cashlogy = 0xE900,
        ConfigCont = 0xE901,
        Errores = 0xE902,
        Ordenes = 0xE903,
        Settings = 0xE904,
        DoorOpen = 0xE905,
        DoorClosed = 0xE906,
        Save = 0xE907
    }

    public enum FlipOrientation
    {
        Normal,
        Horizontal,
        Vertical
    }
    #endregion

    public class MyButton : Button
    {
        #region Fields and Properties
        private FontFamily fontFamily;
        private FlipOrientation flip;
        private IconChar iconChar;
        private Color iconColor;
        private int iconSize;
        private double rotation;

        [Browsable(false)]
        public new Image Image
        {
            get => base.Image;
            set => base.Image = value;
        }

        [Category("Cashlogy")]
        public FlipOrientation Flip
        {
            get => flip;
            set
            {
                if (flip == value) return;
                flip = value;
                UpdateImage();
            }
        }
        [Category("Cashlogy")]
        public IconChar IconChar
        {
            get => iconChar;
            set
            {
                if (iconChar == value) return;
                iconChar = value;
                UpdateImage();
            }
        }

        [Category("Cashlogy")]
        public Color IconColor
        {
            get => iconColor;
            set
            {
                if (iconColor == value) return;
                iconColor = value;
                UpdateImage();
            }
        }
        [Category("Cashlogy")]
        public int IconSize
        {
            get => iconSize;
            set
            {
                if (iconSize == value) return;
                iconSize = value;
                UpdateImage();
            }
        }
        [Category("Cashlogy")]
        public double Rotation
        {
            get => rotation;
            set
            {
                var v = value % 360.0;
                if (Math.Abs(rotation - v) <= 0.5) return;
                rotation = v;
                UpdateImage();
            }
        }
        #endregion

        // Constructor
        public MyButton()
        {
            fontFamily = GetResourceFontFamily(SimuladorCashlogy.Properties.Resources.icocashlogy);
            this.Size = new Drawing.Size(50,50);
            this.flip = FlipOrientation.Normal;
            this.iconColor = Color.Black;
            this.iconSize = 48;
            this.rotation = 0;
            this.IconChar = IconChar.None;
        }

        #region Metodos
        private void UpdateImage()
        {
            this.Image = ToBitmap(iconChar, iconSize, iconColor, rotation, flip);
        }

        private FontFamily GetResourceFontFamily(byte[] fontbytes)
        {
            PrivateFontCollection pfc = new PrivateFontCollection();
            IntPtr fontMemPointer = Marshal.AllocCoTaskMem(fontbytes.Length);
            Marshal.Copy(fontbytes, 0, fontMemPointer, fontbytes.Length);
            pfc.AddMemoryFont(fontMemPointer, fontbytes.Length);
            Marshal.FreeCoTaskMem(fontMemPointer);
            return pfc.Families[0];
        }

        private Bitmap ToBitmap(IconChar icon, int size, Color color, double rotation, FlipOrientation flip)
        {
            Bitmap bitmap = new Bitmap(size, size);
            Graphics g = Graphics.FromImage(bitmap);

            var text = ToChar(icon);
            var font = GetAdjustedIconFont(g, fontFamily, text, new SizeF(size, size));
            Rotate(ref g, rotation, size);
            var brush = new SolidBrush(color);
            DrawIcon(ref g, font, text, size, brush);
            FlipImg(ref bitmap, flip);

            return bitmap;
        }

        private string ToChar(IconChar icon)
        {
            return char.ConvertFromUtf32((int)icon);
        }

        private static Font GetAdjustedIconFont(Graphics graphics, FontFamily family, string text, SizeF size)
        {
            int maxFontSize = 0;
            int minFontSize = 4;
            bool smallestOnFail = true;
            var safeMaxFontSize = maxFontSize > 0 ? maxFontSize : size.Height;
            for (double adjustedSize = safeMaxFontSize; adjustedSize >= minFontSize; adjustedSize -= 0.5)
            {
                var font = GetIconFont(family, (float)adjustedSize);
                // Test the string with the new size
                var iconSize = GetIconSize(graphics, text, font, size);
                if (iconSize.Width < size.Width && iconSize.Height < size.Height)
                    return font;
            }

            // Could not find a font size
            // return min or max or maxFontSize?
            return GetIconFont(family, smallestOnFail ? minFontSize : maxFontSize);
        }

        private static void Rotate(ref Graphics graphics, double rotation, int size)
        {
            if (Math.Abs(rotation) < 0.5) return;
            float mx = .5f * size, my = .5f * size;
            graphics.TranslateTransform(mx, my);
            graphics.RotateTransform((float)rotation);
            graphics.TranslateTransform(-mx, -my);
        }

        private static void DrawIcon(ref Graphics graphics, Font font, string text, int size, Brush brush)
        {
            // Set best quality
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PageUnit = GraphicsUnit.Pixel;

            var topLeft = GetTopLeft(graphics, text, font, new SizeF(size, size));
            graphics.DrawString(text, font, brush, topLeft);
        }

        public static void FlipImg(ref Bitmap image, FlipOrientation flip)
        {
            RotateFlipType rotateFlip;
            switch (flip)
            {
                case FlipOrientation.Horizontal:
                    rotateFlip = RotateFlipType.RotateNoneFlipX;
                    break;
                case FlipOrientation.Vertical:
                    rotateFlip = RotateFlipType.RotateNoneFlipY;
                    break;
                default:
                    rotateFlip = RotateFlipType.RotateNoneFlipNone;
                    break;
            }
            if (rotateFlip == RotateFlipType.RotateNoneFlipNone) return;
            image.RotateFlip(rotateFlip);
        }

        private static Font GetIconFont(FontFamily fontFamily, float size)
        {
            return new Font(fontFamily, size, GraphicsUnit.Point);
        }

        private static SizeF GetIconSize(Graphics graphics, string text, Font font, SizeF size)
        {
            var format = new StringFormat();
            var ranges = new[] { new CharacterRange(0, text.Length) };
            format.SetMeasurableCharacterRanges(ranges);
            format.Alignment = StringAlignment.Center;
            var iconSize = graphics.MeasureString(text, font, size, format);
            return iconSize;
        }

        private static PointF GetTopLeft(Graphics graphics, string text, Font font, SizeF size)
        {
            var iconSize = GetIconSize(graphics, text, font, size);

            var left = Math.Max(0f, (size.Width - iconSize.Width) / 2);
            var top = Math.Max(0f, (size.Height - iconSize.Height) / 2);
            return new PointF(left, top);
        }
        #endregion
    }
}