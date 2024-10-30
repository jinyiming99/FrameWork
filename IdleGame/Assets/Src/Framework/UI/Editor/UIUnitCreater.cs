using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace GameFrameWork.UI.Editor
{
    public static class UIUnitCreater
    {
        [MenuItem("GameObject/UITools/Create UI Unit")]
        public static void CreateUIUnit()
        {
            var obj = Selection.activeGameObject;
            var className = obj.name;
            CreateViewFile(obj, className);
            CreatePartialFile(obj, className);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateViewFile(GameObject obj, string className)
        {
            var transforms = obj.GetComponentsInChildren<Transform>();

            string fullPath = Application.dataPath + "/Src/UI/" + className + "View.cs";
            string path = Path.GetDirectoryName(fullPath);
            if (!File.Exists(path))
            {
                CreatePath(path);
                Debug.Log($"Create path {fullPath}");
            }

            List<Component> list = new List<Component>();
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                Debug.Log($"delete file {fullPath}");
            }
            using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate,FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("using GameFrameWork.UI;");
                    sw.WriteLine("using UnityEngine;");
                    sw.WriteLine("using UnityEngine.UI;");
                    sw.WriteLine();
                    sw.WriteLine("namespace Game.UI");
                    sw.WriteLine("{");
                    sw.WriteLine("    public class " + className + "View : MonoBehaviour");
                    sw.WriteLine("    {");

                    List<string> Namelist = new List<string>();

                    foreach (var transform in transforms)
                    {
                        if (transform.name.Contains(UIEditorConfigDefine.IgnoreName))
                            continue;
                        foreach (var type in UIEditorConfigDefine._uiTypes)
                        {
                            var t =transform.gameObject.GetComponent(type);
                            if (t is not null)
                            {
                                //表达式筛选名字里面不能包含（ ）空格等特殊字符 包含就continue
                                if (transform.name.Contains("(") || transform.name.Contains(")") || transform.name.Contains(" ") || transform.name.Contains("-"))
                                {
                                    continue;
                                }
                                
                                string valueName = $"_{transform.name}_{type.Name}";
                                if (Namelist.Contains(valueName))
                                {
                                    valueName = transform.parent.name + "_" + valueName;
                                }
                                
                                valueName = valueName.Replace(" ", "");
                                if (Namelist.Contains(valueName))
                                {
                                    Debug.LogError($"name is duplicate {valueName}");
                                }
                                Namelist.Add(valueName);
                                sw.WriteLine($"        protected {type.Name} {valueName} = null;");
                                sw.WriteLine($"        public {type.Name} {transform.name}_{type.Name}");
                                sw.WriteLine($"        {{");
                                sw.WriteLine($"             get");
                                sw.WriteLine($"             {{");
                                sw.WriteLine($"                 if ({valueName} == null)");
                                sw.WriteLine($"                 {{");
                                string TransformPath = transform.name;
                                Transform temp = transform;
                                while (temp.gameObject != obj && temp.parent.gameObject != obj)
                                {
                                    TransformPath = temp.parent.name + "/" + TransformPath;
                                    temp = temp.parent;
                                }
                                sw.WriteLine($"                     {valueName} = transform.Find(\"{TransformPath}\").gameObject.GetComponent<{type.Name}>();");
                                sw.WriteLine($"                 }}");
                                sw.WriteLine($"                 return {valueName};");
                                sw.WriteLine($"             }}");
                                sw.WriteLine($"       }}");
                                //sw.WriteLine($"        public {type.Name} {transform.name}_{type.Name} {{ get; set; }}");
                                list.Add(t);
                            }
                        }
                    }
                    sw.WriteLine("    }");
                    sw.WriteLine("}");
                    sw.Flush();
                    sw.Close();
                }
            }
        }

        private static void CreatePartialFile(GameObject obj, string className)
        {
            var transforms = obj.GetComponentsInChildren<Transform>();

            string fullPath = Application.dataPath + "/Src/UI/" + className + "_Partial.cs";
            string path = Path.GetDirectoryName(fullPath);
            if (!File.Exists(path))
            {
                CreatePath(path);
            }

            List<Component> list = new List<Component>();
            if (File.Exists(fullPath))
                File.Delete(fullPath);
            using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate,FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("using GameFrameWork.UI;");
                    sw.WriteLine("using UnityEngine;");
                    sw.WriteLine("using UnityEngine.UI;");
                    sw.WriteLine();
                    sw.WriteLine("namespace Game.UI");
                    sw.WriteLine("{");
                    sw.WriteLine($"    public partial class {className} : UIPanelBase<{className}View>");
                    sw.WriteLine("    {");
                    sw.WriteLine($"        protected override string UIViewName => \"{obj.name}\";");
                    // sw.WriteLine("         protected override void Awake()");
                    // sw.WriteLine("        {");
                    // sw.WriteLine("            base.Awake();");
                    // sw.WriteLine("            LoadResource();");
                    // sw.WriteLine("        }");

                    sw.WriteLine("    }");
                    sw.WriteLine("}");
                    sw.Flush();
                    sw.Close();
                }
            }
        }

        public static void CreatePath(string path)
        {
            if (!Directory.Exists(path))
            {
                string p = Path.Combine(path, "..");
                string root = Path.GetFullPath(p);
                if (!Directory.Exists(root))
                    CreatePath(root);
            
                Directory.CreateDirectory(path);
            }
        }
    }
}