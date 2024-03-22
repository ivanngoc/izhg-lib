using IziHardGames.Libs.Engine.CustomTypes;

namespace UnityEngine
{
    public static class ExtensionRectTransformAsCustomTypes
    {
        public static void SetRect(this RectTransform self, in RectPosGlobal rectPosGlobal)
        {
            //Vector2 anchorDiff = self.anchorMax - self.anchorMin;

            float deltaWidth = rectPosGlobal.Width - self.rect.width;

            float deltaHeight = rectPosGlobal.Height - self.rect.width;

            self.sizeDelta += new Vector2(deltaWidth, deltaHeight);
        }
        public static void SetPositionCentered(this RectTransform self, in RectPosGlobal rectPosGlobal)
        {
            Vector2 center = rectPosGlobal.Center;

            Vector2 centerSelf = self.rect.center;

            self.position = new Vector3(center.x + centerSelf.x, center.y + centerSelf.y, self.position.z);
        }
    }
}