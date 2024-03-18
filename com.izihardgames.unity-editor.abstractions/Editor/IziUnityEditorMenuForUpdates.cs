using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;
using static IziHardGames.ForUnityEditor.ConstantsForUnityEditor;

namespace IziHardGames.ForUnityEditor
{
    public static class IziUnityEditorMenuForUpdates
    {
        [MenuItem(MENU_CATEGORY_NAME + "/Update Loop Settings/" + nameof(ShowPlayerLoopDefault))]
        public static void ShowPlayerLoopDefault()
        {
            var sb = new StringBuilder();
            FillStringBuilder(PlayerLoop.GetDefaultPlayerLoop(), sb, 0);
            Debug.Log(sb);
        }

        [MenuItem(MENU_CATEGORY_NAME + "/Update Loop Settings/" + nameof(ShowPlayerLoopCurrent))]
        public static void ShowPlayerLoopCurrent()
        {
            var sb = new StringBuilder();
            FillStringBuilder(PlayerLoop.GetCurrentPlayerLoop(), sb, 0);
            Debug.Log(sb);
        }
        private static void FillStringBuilder(PlayerLoopSystem playerLoopSystem, StringBuilder text, int inline)
        {
            if (playerLoopSystem.type != null)
            {
                for (var i = 0; i < inline; i++)
                {
                    text.Append("\t");
                }
                text.AppendLine(playerLoopSystem.type.Name);
            }

            if (playerLoopSystem.subSystemList != null)
            {
                inline++;
                foreach (var s in playerLoopSystem.subSystemList)
                {
                    FillStringBuilder(s, text, inline);
                }
            }
        }
    }
}
