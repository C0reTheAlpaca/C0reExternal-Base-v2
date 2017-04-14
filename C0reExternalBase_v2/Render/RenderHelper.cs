using System;
using SlimDX;
using System.Drawing;
using System.Windows.Forms;
using D3D = SlimDX.Direct3D9;
using C0reExternalBase_v2.Math;
using C0reExternalBase_v2.Structs;
using static System.Math;
using static C0reExternalBase_v2.Menu.CheatMenu;
using static C0reExternalBase_v2.Structs.Entitys.Entity;

namespace C0reExternalBase_v2
{
    public class RenderHelper
    {
        private D3D.Device device;
        private readonly D3D.Font[] fonts;
        private readonly D3D.Line line;
        private int shiftX;
        private int shiftY;

        // Sets Up Drawing & Fonts
        public RenderHelper(D3D.Device device, int shiftX = 0, int shiftY = 0)
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
        // Returns String Dimensions
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

        // Draws A Shadowed String
        public void DrawShadowText(string text, int x, int y, Color4 color, int font = 0)
        {
            x += shiftX;
            y += shiftY;
            fonts[font].DrawString(null, text, x + 1, y + 1, Color.Black);
            fonts[font].DrawString(null, text, x, y, color);
        }

        // Draws String
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

        // Draws Circle
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

        // Draws Filled Circle
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

        // Draws Filled Box
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

        // Draws Box
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

        // Draws Line
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

        // Draws Filled Line
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

        // Draws ESP-Box
        public void DrawESPBox(int EntityID, Color color, Rectangle m_WindowRectangle)
        {
            Vector3D Temporaray, TransformedOrigin, TransformedTop;

            Vector3D Origin = Arrays.Entity[EntityID].m_Origin;

            Temporaray = Origin;

            Temporaray.z += Arrays.Entity[EntityID].m_VecMax.z;

            if (!Transformation.World2Screen(Origin, out TransformedOrigin, m_WindowRectangle))
                return;

            if (!Transformation.World2Screen(Temporaray, out TransformedTop, m_WindowRectangle))
                return;

            int Height = (int)TransformedTop.y - (int)TransformedOrigin.y;

            DrawShadowedLine(TransformedOrigin.x - Height / 4, TransformedTop.y, TransformedOrigin.x - Height / 4, TransformedOrigin.y, 1, color);
            DrawShadowedLine(TransformedOrigin.x + Height / 4, TransformedTop.y, TransformedOrigin.x + Height / 4, TransformedOrigin.y, 1, color);
            DrawShadowedLine(TransformedOrigin.x - Height / 4, TransformedTop.y, TransformedOrigin.x + Height / 4, TransformedTop.y, 1, color);
            DrawShadowedLine(TransformedOrigin.x - Height / 4, TransformedOrigin.y, TransformedOrigin.x + Height / 4, TransformedOrigin.y, 1, color);
        }

        // Draws Snaplines
        public void DrawSnaplines(int EntityID, Color color, Rectangle m_WindowRectangle)
        {
            Vector3D TransformedOrigin;

            Vector3D Origin = Arrays.Entity[EntityID].m_Origin;

            if (!Transformation.World2Screen(Origin, out TransformedOrigin, m_WindowRectangle))
                return;

            DrawLine(TransformedOrigin.x, TransformedOrigin.y, m_WindowRectangle.Width / 2, m_WindowRectangle.Height / 2, 1, color);
        }
        #endregion
    }
}
