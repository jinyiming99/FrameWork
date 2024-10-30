using System.Collections.Generic;
using System.IO;
using Export;
using UnityEditor;
using UnityEngine;

namespace Tools.Editor
{
    public class CreateMeshTool
    {
        [MenuItem("GameTools/CreateMesh")]
        public static void CreateMesh()
        {
            var path = EditorUtility.OpenFilePanel("title", "", "txt");
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            
            var list = CreateMeshStaticFunction.ConvertPositionList(path);
            list = CreateMeshStaticFunction.SelectPositionList(list);
            // for (int i = 0; i < list.Count; i++)
            // {
            //     GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //     go.transform.position = list[i];
            //     go.transform.localScale = Vector3.one * .1f;
            // }
            CreateMeshStaticFunction.CreateWallMeshObject(list);
            CreateMeshStaticFunction.CreateTerrainMeshObject(list);
        }
        
    }
}