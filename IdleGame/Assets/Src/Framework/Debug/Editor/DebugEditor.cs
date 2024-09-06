using System.Linq;
using UnityEditor;
using UnityEditor.Build;

namespace GameFrameWork.DebugTools
{
    public class DebugEditor
    {
        [MenuItem("GameTools/Debug/Open")]
        public static void Open()
        {
            var target = EditorUserBuildSettings.selectedBuildTargetGroup;
            var group = NamedBuildTarget.FromBuildTargetGroup(target);
            //修改symbols
            var str = PlayerSettings.GetScriptingDefineSymbols(group);
            if (!str.Contains("LOG_OUT"))
            {
                str += ";LOG_OUT";
            }
            PlayerSettings.SetScriptingDefineSymbols(group,str);
            AssetDatabase.RefreshSettings();
        }
        
        [MenuItem("GameTools/Debug/Close")]
        public static void Close()
        {
            var target = EditorUserBuildSettings.selectedBuildTargetGroup;
            var group = NamedBuildTarget.FromBuildTargetGroup(target);
            //修改symbols
            var str = PlayerSettings.GetScriptingDefineSymbols(group);
            var list = str.Split(";").ToList();
            if (list.Contains("LOG_OUT"))
            {
                list.Remove("LOG_OUT");
            }
            str = string.Join(";", list);
            PlayerSettings.SetScriptingDefineSymbols(group,str);
            AssetDatabase.RefreshSettings();
        }
    }
}