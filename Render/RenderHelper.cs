using System;
using SlimDX;
using System.Drawing;
using System.Windows.Forms;
using D3D = SlimDX.Direct3D9;
using static System.Math;

namespace C0reExternalBase_v2
{
    public class Renderer
    {
        private D3D.Device device;
        private readonly D3D.Font[] fonts;
        private readonly D3D.Line line;
        private int shiftX;
        private int shiftY;

        // Sets Up Drawing & Fonts
        public Renderer(D3D.Device device, int shiftX = 0, int shiftY = 0)
        {
            this.device = device;
            this.shiftX = shiftX;
            this.shiftY = shiftY;
            line = new D3D.Line(device);
            fonts = new D3D.Font[4];
            fonts[0] = new D3D.Font(device, new Font("Dotum", 8, FontStyle.Regular));
            fonts[1] = new D3D.Font(device, new Font("Calibri", 10, FontStyle.Regular));
            fonts[2] = new D3D.Font(device, new Font("Tahoma", 9, FontStyle.Regular));
            fonts[3] = new D3D.Font(device, new Font("Calibri", 16, FontStyle.Regular));
        }

        // Update Overlay Shifting
        public void UpdateShift(int shiftX, int shiftY)
        {
            this.shiftX = shiftX;
            this.shiftY = shiftY;
        }

        #region DrawingFuncs

        public Rectangle MeasureString(string text, int font = 0)
        {
            Rectangle rect = new Rectangle();
            try
            {
                rect = fonts[font].MeasureString(null, text, D3D.DrawTextFormat.Left);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Data.ToString(), "Exception!", MessageBoxButtons.OK);
            }
            return rect;
        }

        public void DrawShadowText(string text, int x, int y, Color4 color, int font = 0)
        {
            x += shiftX;
            y += shiftY;
            fonts[font].DrawString(null, text, x + 1, y + 1, Color.Black);
            fonts[font].DrawString(null, text, x, y, color);
        }

        public void DrawText(string text, bool Centered, float x, float y, Color4 color, int font = 0)
        {
            if (Centered)
            {
                int width = MeasureString(text, font).Width;
                x -= width / 2;
            }

            x += shiftX;
            y += shiftY;
            fonts[font].DrawString(null, text, (int)x, (int)y, color);
        }

        public void DrawCircle(int X, int Y, int radius, int numSides, Color Color)
        {
            if (radius <= 0)
                return;
            if (numSides <= 4)
                return;

            var Step = (float)(PI * 2.0 / numSides);
            var Count = 0;
            for (float a = 0; a < PI * 2.2; a += Step)
            {
                var X1 = (float)(radius * Cos(a) + X);
                var Y1 = (float)(radius * Sin(a) + Y);
                var X2 = (float)(radius * Cos(a + Step) + X);
                var Y2 = (float)(radius * Sin(a + Step) + Y);
                if (Count != 0)
                {
                    DrawLine(X1, Y1, X2, Y2, 2, Color);
                }
                Count += 2;
            }
        }

        public void DrawFilledCircle(int X, int Y, int radius, Color Color)
        {
            int r2 = radius * radius;
            for (int cy = -radius; cy <= radius; cy++)
            {
                int cx = (int)(Sqrt(r2 - cy * cy) + 0.5);
                int cyy = cy + Y;

                DrawLine(X - cx, cyy, X + cx, cyy, 2, Color);
            }
        }

        public void DrawFilledBox(float x, float y, float w, float h, Color4 Color)
        {
            x += shiftX;
            y += shiftY;
            var vLine = new Vector2[2];

            line.GLLines = true;
            line.Antialias = true;
            line.Width = w;

            vLine[0].X = x + w / 2;
            vLine[0].Y = y;
            vLine[1].X = x + w / 2;
            vLine[1].Y = y + h;

            line.Draw(vLine, Color);
        }

        public void DrawBox(float x, float y, float w, float h, float px, Color4 Color)
        {
            if (px <= 0)
                return;

            x += shiftX;
            y += shiftY;
            line.GLLines = true;
            line.Antialias = true;
            line.Width = px;

            line.Begin();
            line.Draw(new Vector2[2] { new Vector2(x, y), new Vector2(x + w, y) }, Color);
            line.Draw(new Vector2[2] { new Vector2(x + w, y), new Vector2(x + w, y + h) }, Color);
            line.Draw(new Vector2[2] { new Vector2(x + w, y + h), new Vector2(x, y + h) }, Color);
            line.Draw(new Vector2[2] { new Vector2(x, y + h), new Vector2(x, y) }, Color);
            line.End();
        }

        public void DrawLine(float x1, float y1, float x2, float y2, float w, Color4 Color)
        {
            if (w <= 0)
                return;

            x1 += shiftX;
            y1 += shiftY;
            x2 += shiftX;
            y2 += shiftY;
            var vLine = new Vector2[2] { new Vector2(x1, y1), new Vector2(x2, y2) };

            line.GLLines = true;
            line.Antialias = true;
            line.Width = w;

            line.Draw(vLine, Color);
        }

        public void DrawShadowedLine(float x1, float y1, float x2, float y2, float w, Color Color)
        {
            if (w <= 0)
                return;

            x1 += shiftX;
            y1 += shiftY;
            x2 += shiftX;
            y2 += shiftY;
            var vLine = new Vector2[2] { new Vector2(x1, y1), new Vector2(x2, y2) };
            var vLineShadow = new Vector2[2] { new Vector2(x1 - 1, y1 - 1), new Vector2(x2 - 1, y2 - 1) };
            var vLineShadow2 = new Vector2[2] { new Vector2(x1 + 1, y1 + 1), new Vector2(x2 + 1, y2 + 1) };

            line.GLLines = true;
            line.Antialias = true;
            line.Width = w;

            line.Begin();
            line.Draw(vLineShadow2, Color.Black);
            line.Draw(vLineShadow, Color.Black);
            line.Draw(vLine, Color);
            line.End();
        }
        #endregion
    }
}
