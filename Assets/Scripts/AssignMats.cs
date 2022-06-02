using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class AssignMats : MonoBehaviour
{
    [MenuItem("AssetDatabase/LoadAll")]
    private static void LoadAll()
    {
        Object[] objs = Resources.LoadAll("Adorable 3d Items/Icons", typeof(GameObject));
        Object[] mats = Resources.LoadAll("Adorable 3d Items/Materials", typeof(Material));
        Object[] texs = Resources.LoadAll("Adorable 3d Items/Texture", typeof(Texture2D));

        for (int x = 0; x < objs.Length; x++)
        {
            GameObject obj = (GameObject)objs[x];
            Material mat = (Material)mats[x];
            Texture2D tex = (Texture2D)texs[x];

            mat.SetTexture("_BaseMap", tex);
            mat.SetFloat("_Smoothness", 0);
        }


    }

    [MenuItem("AssetDatabase/CreateAssets")]
    private static void CreateObjects()
    {
        Object[] objs = Resources.LoadAll("Adorable 3d Items/Icons", typeof(GameObject));
        Object[] mats = Resources.LoadAll("Adorable 3d Items/Materials", typeof(Material));
        Object[] texs = Resources.LoadAll("Adorable 3d Items/Texture", typeof(Texture2D));

        string assetPath = "Assets/Resources/Acorn.prefab";

        for (int x = 10; x < 13; x++)
        {
            if (!AssetDatabase.CopyAsset(assetPath, "Assets/Resources/" + ((GameObject) objs[x]).name + ".prefab"))
            {
                Debug.Log("failed to create asset");
            }

            Object[] objects = Resources.LoadAll("", typeof(GameObject));
            GameObject objPrefab = null;

            for (int a =  objects.Length - 1; a >= 0; a--) {
                if (((GameObject) objects[a]).name.Equals(((GameObject) objs[x]).name)) {
                    objPrefab = (GameObject) objects[a];
                }    
            }

            if (objPrefab != null) {
                //change the graphics 
            }
        }
    }
}
