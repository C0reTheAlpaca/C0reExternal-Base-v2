using System;
using System.Drawing;
using C0reExternalBase_v2.Structs;
using static C0reExternalBase_v2.RenderForm;
using static C0reExternalBase_v2.Structs.CheatMenu;
using static C0reExternalBase_v2.Structs.Entitys.Entity;

namespace C0reExternalBase_v2
{
    public class Visuals
    {
        public void DrawESPBox(int EntityID, Color color, Rectangle m_WindowRectangle)
        {
            Vector3D Temporary, TransformedOrigin, TransformedTop;
            Vector3D Origin = Arrays.Entity[EntityID].m_VecOrigin;

            Temporary = Origin;

            Temporary.z += Arrays.Entity[EntityID].m_VecMax.z;

            if (!Transformation.World2Screen(Origin, out TransformedOrigin, m_WindowRectangle))
                return;

            if (!Transformation.World2Screen(Temporary, out TransformedTop, m_WindowRectangle))
                return;

            int Height = (int)TransformedTop.y - (int)TransformedOrigin.y;

            _Render.DrawShadowedLine(TransformedOrigin.x - Height / 4, TransformedTop.y, TransformedOrigin.x - Height / 4, TransformedOrigin.y, 1, color);
            _Render.DrawShadowedLine(TransformedOrigin.x + Height / 4, TransformedTop.y, TransformedOrigin.x + Height / 4, TransformedOrigin.y, 1, color);
            _Render.DrawShadowedLine(TransformedOrigin.x - Height / 4, TransformedTop.y, TransformedOrigin.x + Height / 4, TransformedTop.y, 1, color);
            _Render.DrawShadowedLine(TransformedOrigin.x - Height / 4, TransformedOrigin.y, TransformedOrigin.x + Height / 4, TransformedOrigin.y, 1, color);
        }

        public void DrawRadar(Rectangle m_WindowRectangle)
        {
            int RadarSize = Settings.Radar.m_iRadarSize;

            Vector2D RadarPosition = Settings.Radar.m_VecRadarPosition;

            // Draw The Radar
            _Render.DrawFilledBox(RadarPosition.x, RadarPosition.y, RadarSize, RadarSize, Color.FromArgb(255, 27, 27, 27));
            _Render.DrawBox(RadarPosition.x, RadarPosition.y, RadarSize, RadarSize, 1, Color.Black);
            _Render.DrawLine(RadarPosition.x, RadarPosition.y, RadarPosition.x + (RadarSize / 2), RadarPosition.y + (RadarSize / 2), 1, Color.Black);
            _Render.DrawLine(RadarPosition.x + RadarSize, RadarPosition.y, RadarPosition.x + (RadarSize / 2), RadarPosition.y + (RadarSize / 2), 1, Color.Black);
            _Render.DrawLine(RadarPosition.x, RadarPosition.y + (RadarSize / 2), RadarPosition.x + RadarSize, RadarPosition.y + (RadarSize / 2), 1, Color.Black);
            _Render.DrawLine(RadarPosition.x + (RadarSize / 2), RadarPosition.y + (RadarSize / 2), RadarPosition.x + (RadarSize / 2), RadarPosition.y + RadarSize, 1, Color.Black);
            _Render.DrawFilledBox(RadarPosition.x + (RadarSize / 2) - 2, RadarPosition.y + (RadarSize / 2), 5, 5, Color.Gold);
        }

        public void DrawEntityOnRadar(int EntityID, Color color, Rectangle m_WindowRectangle)
        {
            // Get The Current Entitys "Radar Screen Position"
            Vector2D ScreenPosition = Transformation.EntityToRadar(EntityID, Settings.Radar.m_VecRadarPosition, Settings.Radar.m_iRadarSize);

            // Draw The RadarDot
            _Render.DrawFilledBox(ScreenPosition.x, ScreenPosition.y, 5, 5, color);
        }

        public void DrawSnaplines(int EntityID, Color color, Rectangle m_WindowRectangle)
        {
            Vector3D TransformedOrigin;

            Vector3D Origin = Arrays.Entity[EntityID].m_VecOrigin;

            if (!Transformation.World2Screen(Origin, out TransformedOrigin, m_WindowRectangle))
                return;

            _Render.DrawLine(TransformedOrigin.x, TransformedOrigin.y, m_WindowRectangle.Width / 2, m_WindowRectangle.Height / 2, 1, color);
        }
    }
}
