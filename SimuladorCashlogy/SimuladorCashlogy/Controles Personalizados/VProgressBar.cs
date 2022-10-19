using System.Drawing;

namespace System.Windows.Forms
{
    public class VProgressBar : ProgressBar
    {
        public VProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Nada... Esto ayuda a controlar el parpadeo.
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (Image offscreenImage = new Bitmap(this.Width, this.Height))
            {
                using (Graphics offscreen = Graphics.FromImage(offscreenImage))
                {
                    Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

                    if (ProgressBarRenderer.IsSupported) ProgressBarRenderer.DrawVerticalBar(offscreen, rect);

                    double scale = (double)(this.Value - this.Minimum) / (this.Maximum - this.Minimum);
                    int progress = (int)(scale * rect.Height);

                    SolidBrush brush = new SolidBrush(this.ForeColor);
                    offscreen.FillRectangle(brush, 0, rect.Height - progress, rect.Width, progress);

                    e.Graphics.DrawImage(offscreenImage, 0, 0);
                    offscreenImage.Dispose();
                }
            }
        }
    }
}