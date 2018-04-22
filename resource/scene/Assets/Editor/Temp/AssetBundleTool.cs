using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;


public class AssetBundleTool : MonoBehaviour
{
    [MenuItem("Assets/GetAssetBundleName", false, 29)]
    static void GetAssetBundleName()
    {
        var names = AssetDatabase.GetAllAssetBundleNames();
        foreach (var name in names)
            Debug.Log("AssetBundle: " + name);
    }

    [MenuItem("Assets/AssetBundle/SetAssetBundleName", false, 30)]
    static void SetAssetBundleName()
    {
        string path = "";
        Object[] selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object obj in selectedAsset)
        {
            string tp = AssetDatabase.GetAssetPath(obj);
            if (Path.GetExtension(tp) == "")
            {
                if (path == "" || path.Length > tp.Length)
                {
                    path = tp;
                }
            }
        }

        print(path);
        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        var tfs = AssetDatabase.FindAssets("t:GameObject", new string[] { path });

        foreach (var f in tfs)
        {
            var tp = AssetDatabase.GUIDToAssetPath(f);
            var tro = AssetDatabase.LoadAssetAtPath(tp, typeof(Object));
            var ai = AssetImporter.GetAtPath(tp);
            if (string.IsNullOrEmpty(ai.assetBundleName))
            {
                string[] strs = tro.name.Split('_');
                int len = strs.Length;
                ai.assetBundleName = "scene";
                for (int i = 0; i < len - 1; i++)
                {
                    ai.assetBundleName = ai.assetBundleName + "/" + strs[i];
                }
                ai.assetBundleName = ai.assetBundleName + "/" + tro.name;
            }
        }
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Assets/AssetBundle/SetPrefabAssetBundleName", false, 30)]
    static void SetAnimationAssetBundleName()
    {
        string path = "";
        Object[] selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object obj in selectedAsset)
        {
            string tp = AssetDatabase.GetAssetPath(obj);
            if (Path.GetExtension(tp) == "")
            {
                if (path == "" || path.Length > tp.Length)
                {
                    path = tp;
                }
            }
        }

        print(path);
        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        var tfs = AssetDatabase.FindAssets("t:GamObject", new string[] { path });

        foreach (var f in tfs)
        {
            var tp = AssetDatabase.GUIDToAssetPath(f);
            var tro = AssetDatabase.LoadAssetAtPath(tp, typeof(Object));
            var ai = AssetImporter.GetAtPath(tp);
            if (string.IsNullOrEmpty(ai.assetBundleName))
            {
                ai.assetBundleName = tro.name;
                Debug.Log(tp);
            }
        }
        AssetDatabase.SaveAssets();

    }

    [MenuItem("Assets/AssetBundle/ReplaceShaderAndChangeShadowState", false, 30)]
    static void ReplaceShaderAndChangeShadowState()
    {
        string path = "";
        Object[] selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object obj in selectedAsset)
        {
            string tp = AssetDatabase.GetAssetPath(obj);
            if (Path.GetExtension(tp) == "")
            {
                if (path == "" || path.Length > tp.Length)
                {
                    path = tp;
                }
            }
        }
        print(path);
        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        Shader opaque = Shader.Find("Apowo/SceneObject/ApowoUnlitTextureShader");
        Shader transparent = Shader.Find("Apowo/SceneObject/ApowoUnlitTransparentShader");
        Shader transparentCutout = Shader.Find("Apowo/SceneObject/ApowoUnlitTransparentCutoutShader");
        List<string> shaderList = new List<string> { "Apowo/SceneObject/ApowoMaskAlphaBlendShader", "Apowo/SceneObject/ApowoAddtivePannerRolesUV2Shader", "Apowo/SceneObject/ApowoTechnologySceneObjectShader", "Apowo/SceneObject/ApowoMaskAddShader", "Apowo/SceneObject/ApowoSelectTargetShader", "Apowo/SceneObject/ApowoOffsetAddTransparentShader", "Apowo/SceneObject/ApowoOffsetDelTransparentShader" };
        var tfs = AssetDatabase.FindAssets("t:Prefab", new string[] { path });
        foreach (var f in tfs)
        {
            var tp = AssetDatabase.GUIDToAssetPath(f);
            var tro = AssetDatabase.LoadAssetAtPath(tp, typeof(Object));

            var materials = ((GameObject)tro).GetComponentsInChildren<MeshRenderer>();
            foreach (var MR in materials)
            {
                if (MR.gameObject.layer.ToString() == "11")
                {
                    foreach (var ma in MR.sharedMaterials)
                    {
                        if (ma == null) continue;
                        if (!shaderList.Contains(ma.shader.name))
                        {
                            if (ma.shader.name.Equals(opaque.name) || ma.shader.name.Equals(transparent.name) || ma.shader.name.Equals(transparentCutout.name))
                            {
                                continue;
                            }
                            if (ma.shader.name.Equals("Unlit/Transparent"))
                            {
                                MR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                                ma.shader = transparent;
                                MR.receiveShadows = false;
                                ma.SetFloat("_Selected", 1f);
                            }
                            else if (ma.shader.name.Equals("Unlit/Texture") || ma.shader.name.Equals("Standard"))
                            {
                                MR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                                ma.shader = opaque;
                                MR.receiveShadows = false;
                                ma.SetFloat("_Selected", 1f);
                            }
                            else if (ma.shader.name.Equals("Unlit/Transparent Cutout"))
                            {
                                MR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                                ma.shader = transparentCutout;
                                MR.receiveShadows = false;
                                ma.SetFloat("_Selected", 1f);
                            }
                            else
                            {
                                Debug.LogError("You set a wrong shader or layer : " + MR.name);
                            }
                        }
                    }
                }
                else if (MR.gameObject.layer.ToString() == "16" && MR.name.Contains("Shadow_"))
                {
                    MR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    MR.receiveShadows = false;
                    foreach (var ma in MR.sharedMaterials)
                    {
                        //ma.shader = transparent;
                        ma.SetFloat("_Selected", 1f);
                    }

                }

            }
        }
        AssetDatabase.SaveAssets();
        Debug.Log("end!!!!!!!!!!!!");
    }

    [MenuItem("Assets/AssetBundle/ChangeShadowPosition", false, 30)]
    static void ChangeShadowPosition()
    {
        string path = "";
        Object[] selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object obj in selectedAsset)
        {
            string tp = AssetDatabase.GetAssetPath(obj);
            if (Path.GetExtension(tp) == "")
            {
                if (path == "" || path.Length > tp.Length)
                {
                    path = tp;
                }
            }
        }
        print(path);
        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        var tfs = AssetDatabase.FindAssets("t:Prefab", new string[] { path });
        foreach (var f in tfs)
        {
            var tp = AssetDatabase.GUIDToAssetPath(f);
            var tro = AssetDatabase.LoadAssetAtPath(tp, typeof(Object));

            var materials = ((GameObject)tro).GetComponentsInChildren<MeshRenderer>();

            foreach (var MR in materials)
            {
                if (MR.name.Contains("Shadow_"))
                {
                    if(MR.gameObject.transform.position.y != 0.005f)
                    {
                        MR.gameObject.transform.position = new Vector3(0, 0.005f, 0);
                        MR.gameObject.layer = 16;
                    }
                }
                else if (MR.name.Contains("shadow_") || MR.name.Contains("shadow"))
                {
                    Debug.LogError("名字有误------>" + MR.name);
                    if (MR.gameObject.transform.position.y != 0.005f)
                    {
                        MR.gameObject.transform.position = new Vector3(0, 0.005f, 0);
                        MR.gameObject.layer = 16;
                    }
                }
            }
        }
        AssetDatabase.SaveAssets();
        Debug.Log("end!!!!!!!!!!!!");
    }


    [MenuItem("Assets/SplitTexture/1_Block_ChangeTextureToRGB", false, 10)]
    static void ChangeMaterialTextureToRGB()
    {
        string path = "";
        Object[] selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object obj in selectedAsset)
        {
            string tp = AssetDatabase.GetAssetPath(obj);
            if (Path.GetExtension(tp) == "")
            {
                if (path == "" || path.Length > tp.Length)
                {
                    path = tp;
                }
            }
        }

        print(path);
        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        var tfs = AssetDatabase.FindAssets("t:Material", new string[] { path });
        Texture2D tex1;
        Texture2D tex2;
        foreach (var f in tfs)
        {
            var tp = AssetDatabase.GUIDToAssetPath(f);
            var tro = AssetDatabase.LoadAssetAtPath(tp, typeof(Object)) as Material;
            if (!tro.shader.name.Contains("ApowoBlockProShader"))
            {
                tex1 = null;
                tex2 = null;
                continue;
            }

            tex1 = tro.GetTexture("_DetailTex") as Texture2D;
            tex2 = tro.GetTexture("_DecorateTex") as Texture2D;

            if (tex1 == null || tex2 == null)
            {
                tex1 = null;
                tex2 = null;
                continue;
            }
            if (tex1.width * tex1.height <= 16 && tex2.width * tex2.height <= 16)
            {
                Debug.LogWarning(tro.name + "纹理尺寸过小，不拆分=>" + tex1.name + ", " + tex2.name);
                tex1 = null;
                tex2 = null;
                continue;
            }
            if (tex2.width * tex2.height <= 16)
            {
                Debug.LogWarning(tro.name + "第二张纹理为透明图，不拆分=>" + tex2.name);
                ModifyOneTex(tex1);
            }
            else
            {
                if (tex1.width != tex2.width || tex1.height != tex2.height)
                {
                    Debug.LogError("将要拆分的纹理: " + tro.name + ", 大小尺寸不一致");
                    tex1 = null;
                    tex2 = null;
                    continue;
                }
                ModifyTowTex(tex1, tex2);
            }
            tex1 = null;
            tex2 = null;
        }
        AssetDatabase.SaveAssets();
        Debug.Log("ending.");
    }
    static void ModifyTowTex(Texture2D tex1, Texture2D tex2)
    {

        int w = tex1.width;
        int h = tex1.height;

        string tex_path1 = AssetDatabase.GetAssetPath(tex1);
        string[] str1 = tex_path1.Split('/');
        string dirPath1 = tex_path1.Substring(0, tex_path1.Length - str1[str1.Length - 1].Length);

        string tex_path2 = AssetDatabase.GetAssetPath(tex2);
        string[] str2 = tex_path2.Split('/');
        string dirPath2 = tex_path2.Substring(0, tex_path2.Length - str2[str2.Length - 1].Length);

        ModifyTexReadable(tex_path1);
        ModifyTexReadable(tex_path2);


        Texture2D tex_rgb1 = new Texture2D(w, h, TextureFormat.RGB24, false);
        Texture2D tex_rgb2 = new Texture2D(w, h, TextureFormat.RGB24, false);
        Texture2D tex_alpha = new Texture2D(w, h, TextureFormat.RGB24, false);

        Color[] color_rgb1 = new Color[w * h];
        Color[] color_rgb2 = new Color[w * h];
        Color[] color_alpha = new Color[w * h];

        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                color_rgb1[i * w + j].r = tex1.GetPixel(j, i).r;
                color_rgb1[i * w + j].g = tex1.GetPixel(j, i).g;
                color_rgb1[i * w + j].b = tex1.GetPixel(j, i).b;

                color_rgb2[i * w + j].r = tex2.GetPixel(j, i).r;
                color_rgb2[i * w + j].g = tex2.GetPixel(j, i).g;
                color_rgb2[i * w + j].b = tex2.GetPixel(j, i).b;

                color_alpha[i * w + j].r = tex1.GetPixel(j, i).a;
                color_alpha[i * w + j].g = tex2.GetPixel(j, i).a;
            }
        }
        RecoveryTex(tex_path1);
        RecoveryTex(tex_path2);

        tex_rgb1.SetPixels(color_rgb1);
        tex_rgb2.SetPixels(color_rgb2);
        tex_alpha.SetPixels(color_alpha);

        string path_rgb1 = dirPath1 + tex1.name + "_rgb.png";
        string path_rgb2 = dirPath2 + tex2.name + "_rgb.png";
        string path_alpha = dirPath1 + tex1.name + "_" + tex2.name + "_a.png";

        SaveTex2Local(tex_rgb1, path_rgb1);
        SaveTex2Local(tex_rgb2, path_rgb2);
        SaveTex2Local(tex_alpha, path_alpha);

        Texture2D.DestroyImmediate(tex_rgb1);
        Texture2D.DestroyImmediate(tex_rgb2);
        Texture2D.DestroyImmediate(tex_alpha);
        tex_rgb1 = null;
        tex_rgb2 = null;
        tex_alpha = null;
    }
    static void ModifyOneTex(Texture2D tex)
    {
        int w = tex.width;
        int h = tex.height;

        string tex_path = AssetDatabase.GetAssetPath(tex);
        string[] str = tex_path.Split('/');
        string dirPath = tex_path.Substring(0, tex_path.Length - str[str.Length - 1].Length);

        ModifyTexReadable(tex_path);
        if (tex.format != TextureFormat.ARGB32)
        {
            Debug.LogError("图片: " + tex_path + "格式不正确!!!!!");
            return;
        }

        Texture2D tex_rgb = new Texture2D(w, h, TextureFormat.RGB24, false);
        Texture2D tex_alpha = new Texture2D(w, h, TextureFormat.RGB24, false);

        Color[] color_rgb = new Color[w * h];
        Color[] color_alpha = new Color[w * h];

        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                color_rgb[i * w + j].r = tex.GetPixel(j, i).r;
                color_rgb[i * w + j].g = tex.GetPixel(j, i).g;
                color_rgb[i * w + j].b = tex.GetPixel(j, i).b;

                color_alpha[i * w + j].r = tex.GetPixel(j, i).a;
            }
        }
        RecoveryTex(tex_path);

        tex_rgb.SetPixels(color_rgb);
        tex_alpha.SetPixels(color_alpha);

        string path_rgb1 = dirPath + tex.name + "_rgb.png";
        string path_alpha = dirPath + tex.name + "_a.png";

        SaveTex2Local(tex_rgb, path_rgb1);
        SaveTex2Local(tex_alpha, path_alpha);

        Texture2D.DestroyImmediate(tex_rgb);
        Texture2D.DestroyImmediate(tex_alpha);
        tex_rgb = null;
        tex_alpha = null;
    }
    [MenuItem("Assets/SplitTexture/2_Block_ReplaceShaderAndTexture", false, 10)]
    static void ReplaceShaderAndTexture()
    {
        string path = "";
        Object[] selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object obj in selectedAsset)
        {
            string tp = AssetDatabase.GetAssetPath(obj);
            if (Path.GetExtension(tp) == "")
            {
                if (path == "" || path.Length > tp.Length)
                {
                    path = tp;
                }
            }
        }

        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        var tfs = AssetDatabase.FindAssets("t:Material", new string[] { path });


        Shader RGBShader = Shader.Find("Apowo/SceneComponent/ApowoBlockProRGBShader");

        foreach (var f in tfs)
        {
            var tp = AssetDatabase.GUIDToAssetPath(f);
            var tro = AssetDatabase.LoadAssetAtPath(tp, typeof(Object)) as Material;
            Debug.Log(tro.shader.name);
            if (!tro.shader.name.Equals("Apowo/SceneComponent/ApowoBlockProShader"))
            {
                continue;
            }

            Texture2D tex1 = tro.GetTexture("_DetailTex") as Texture2D;
            Texture2D tex2 = tro.GetTexture("_DecorateTex") as Texture2D;
            if (tex1 == null && tex2 == null)
            {
                continue;
            }
            if (tex1 == null)
            {
                continue;
            }
            var dirPath1 = GetDirPath(tex1);
            string path_rgb1 = dirPath1 + tex1.name + "_rgb.png";

            var dirPath2 = GetDirPath(tex2);
            string path_rgb2 = dirPath2 + tex2.name + "_rgb.png";

            string path_a = dirPath1 + tex1.name + "_" + tex2.name + "_a.png";

            var tex_rgb1 = ModifyShaderAndTex(path_rgb1);
            var tex_rgb2 = ModifyShaderAndTex(path_rgb2);
            if (tex_rgb1 == null && tex_rgb2 == null) continue;
            if (tex_rgb2 == null)
            {
                tex_rgb2 = tex2;
                path_a = dirPath1 + tex1.name + "_a.png";
            }

            var tex_alpha = ModifyShaderAndTex(path_a);
            tro.shader = RGBShader;
            tro.SetTexture("_DetailTex_rgb", tex_rgb1);
            tro.SetTexture("_DecorateTex_rgb", tex_rgb2);
            tro.SetTexture("_Detail_DecorateTex_a", tex_alpha);

        }
        AssetDatabase.SaveAssets();
        Debug.Log("ending.");
    }


    [MenuItem("Assets/SplitTexture/1_SceneObject_ChangeTextureToRGB", false, 20)]
    static void ChangeSceneObjectTextureToRGB()
    {
        string path = "";
        Object[] selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object obj in selectedAsset)
        {
            string tp = AssetDatabase.GetAssetPath(obj);
            if (Path.GetExtension(tp) == "")
            {
                if (path == "" || path.Length > tp.Length)
                {
                    path = tp;
                }
            }
        }
        Debug.Log(path);
        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        var tfs = AssetDatabase.FindAssets("t:Prefab", new string[] { path });
        List<string> ShaderNames = new List<string>()
        {
            "Apowo/SceneObject/ApowoUnlitTransparentShader",
            "Apowo/SceneObject/ApowoUnlitTransparentCutoutShader"
        };
        Texture2D tex = null;
        foreach (var f in tfs)
        {
            var tp = AssetDatabase.GUIDToAssetPath(f);
            var tro = AssetDatabase.LoadAssetAtPath(tp, typeof(Object));

            var materials = ((GameObject)tro).GetComponentsInChildren<MeshRenderer>();
            foreach (var MR in materials)
            {
                if (MR.gameObject.layer.ToString() == "11" || MR.gameObject.layer.ToString() == "16")//
                {
                    foreach (var ma in MR.sharedMaterials)
                    {
                        tex = ma.mainTexture as Texture2D;
                        if (!ShaderNames.Contains(ma.shader.name) || tex == null)
                        {
                            continue;
                        }

                        int w = tex.width;
                        int h = tex.height;
                        string tex_path = AssetDatabase.GetAssetPath(tex);
                        string[] str = tex_path.Split('/');
                        string dirPath = tex_path.Substring(0, tex_path.Length - str[str.Length - 1].Length);

                        ModifyTexReadable(tex_path);

                        if (tex.format != TextureFormat.ARGB32)
                        {
                            Debug.LogError("图片: " + tex_path + "格式不正确!!!!!");
                            RecoveryTex(tex_path);
                            continue;
                        }

                        Color[] color_rgb = new Color[w * h];
                        Color[] color_alpha = new Color[w * h];

                        for (int i = 0; i < h; i++)
                        {
                            for (int j = 0; j < w; j++)
                            {
                                color_rgb[i * w + j].r = tex.GetPixel(j, i).r;
                                color_rgb[i * w + j].g = tex.GetPixel(j, i).g;
                                color_rgb[i * w + j].b = tex.GetPixel(j, i).b;

                                color_alpha[i * w + j].r = tex.GetPixel(j, i).a;
                            }
                        }
                        RecoveryTex(tex_path);

                        Texture2D tex_rgb = new Texture2D(w, h, TextureFormat.RGB24, false);
                        tex_rgb.SetPixels(color_rgb);
                        string path_rgb = dirPath + tex.name + "_rgb.png";
                        SaveTex2Local(tex_rgb, path_rgb);
                        Texture2D.DestroyImmediate(tex_rgb);
                        tex_rgb = null;

                        Texture2D tex_alpha = new Texture2D(w, h, TextureFormat.RGB24, false);
                        tex_alpha.SetPixels(color_alpha);
                        string path_alpha = dirPath + tex.name + "_a.png";
                        SaveTex2Local(tex_alpha, path_alpha);
                        Texture2D.DestroyImmediate(tex_alpha);
                        tex_alpha = null;

                        tex = null;
                    }
                }
                else
                {
                    Debug.LogError("Pleace check " + MR.name + " layer.");
                }
            }
        }
        Debug.Log("ending.");
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Assets/SplitTexture/2_SceneObject_ReplaceShaderAndTexture", false, 20)]
    static void ReplaceSceneObjectShaderAndTexture()
    {
        string path = "";
        Object[] selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object obj in selectedAsset)
        {
            string tp = AssetDatabase.GetAssetPath(obj);
            if (Path.GetExtension(tp) == "")
            {
                if (path == "" || path.Length > tp.Length)
                {
                    path = tp;
                }
            }
        }
        Debug.Log(path);
        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        var tfs = AssetDatabase.FindAssets("t:Prefab", new string[] { path });
        List<string> ShaderNames = new List<string>()
        {
            "Apowo/SceneObject/ApowoUnlitTransparentShader",
            "Apowo/SceneObject/ApowoUnlitTransparentCutoutShader"
        };
        Shader TraShade = Shader.Find("Apowo/SceneObject/ApowoUnlitTransparentRGBShader");
        Shader CutShade = Shader.Find("Apowo/SceneObject/ApowoUnlitTransparentCutoutRGBShader");
        Texture2D tex = null;
        foreach (var f in tfs)
        {
            var tp = AssetDatabase.GUIDToAssetPath(f);
            var tro = AssetDatabase.LoadAssetAtPath(tp, typeof(Object));
            var materials = ((GameObject)tro).GetComponentsInChildren<MeshRenderer>();
            foreach (var MR in materials)
            {
                if (MR.gameObject.layer.ToString() == "11" || MR.gameObject.layer.ToString() == "16")
                {
                    foreach (var ma in MR.sharedMaterials)
                    {
                        tex = ma.mainTexture as Texture2D;
                        if (!ShaderNames.Contains(ma.shader.name) || tex == null)
                        {
                            continue;
                        }

                        var dirPath = GetDirPath(tex);

                        string path_rgb = dirPath + tex.name + "_rgb.png";
                        string path_a = dirPath + tex.name + "_a.png";

                        var tex_rgb = ModifyShaderAndTex(path_rgb);
                        var tex_a = ModifyShaderAndTex(path_a);

                        if (tex_rgb == null && tex_a == null) continue;
                        
                        if (ma.shader.name.Equals("Apowo/SceneObject/ApowoUnlitTransparentShader"))
                        {
                            ma.shader = TraShade;
                        }
                        else if (ma.shader.name.Equals("Apowo/SceneObject/ApowoUnlitTransparentCutoutShader"))
                        {
                            ma.shader = CutShade;
                        }

                        ma.SetTexture("_MainTex", tex_rgb);
                        ma.SetTexture("_MainTex_alpha", tex_a);
                    }
                }
            }
        }
        Debug.Log("ending.");
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Assets/SplitTexture/OverrideTexture", false, 20)]
    static void OverrideTexture()
    {
        string path = "";
        Object[] selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object obj in selectedAsset)
        {
            string tp = AssetDatabase.GetAssetPath(obj);
            if (Path.GetExtension(tp) == "")
            {
                if (path == "" || path.Length > tp.Length)
                {
                    path = tp;
                }
            }
        }
        Debug.Log(path);
        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        var tfs = AssetDatabase.FindAssets("t:Texture", new string[] { path });
        foreach (var f in tfs)
        {
            var tp = AssetDatabase.GUIDToAssetPath(f);
            var tro = AssetDatabase.LoadAssetAtPath(tp, typeof(Object)) as Texture2D;

            string texPath = AssetDatabase.GetAssetPath(tro);
            if (texPath.EndsWith("_a.png") || texPath.EndsWith("_rgb.png"))
            {
                
                TextureImporter textureImporter = AssetImporter.GetAtPath(texPath) as TextureImporter;
                textureImporter.isReadable = false;

                TextureImporterPlatformSettings t0 = new TextureImporterPlatformSettings();
                t0.overridden = true;
                t0.name = "Android";
                t0.textureCompression = TextureImporterCompression.Compressed;
                t0.compressionQuality = 100;
                t0.format = TextureImporterFormat.ETC_RGB4;
                textureImporter.SetPlatformTextureSettings(t0);

                TextureImporterPlatformSettings t1 = new TextureImporterPlatformSettings();
                t1.overridden = true;
                t1.name = "iPhone";
                t1.textureCompression = TextureImporterCompression.Compressed;
                t1.compressionQuality = 100;
                t1.format = TextureImporterFormat.PVRTC_RGB4;
                textureImporter.SetPlatformTextureSettings(t1);
                textureImporter.mipmapEnabled = false;
                textureImporter.alphaIsTransparency = true;
                AssetDatabase.ImportAsset(texPath);
            }

        }
    }
    //[MenuItem("Assets/SplitTexture/PrintShaderName", false, 20)]
    static void PrintShaderName()
    {
        string path = "";
        Object[] selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object obj in selectedAsset)
        {
            string tp = AssetDatabase.GetAssetPath(obj);
            if (Path.GetExtension(tp) == "")
            {
                if (path == "" || path.Length > tp.Length)
                {
                    path = tp;
                }
            }
        }
        Debug.Log(path);
        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        var tfs = AssetDatabase.FindAssets("t:Material", new string[] { path });
        foreach (var f in tfs)
        {
            var tp = AssetDatabase.GUIDToAssetPath(f);
            var tro = AssetDatabase.LoadAssetAtPath(tp, typeof(Object)) as Material;
            Debug.Log("------" + tro.shader.name);
        }
    }
    #region SplitTexture Tools
    static void ModifyTexReadable(string tex_path)
    {
        TextureImporter textureImporter = AssetImporter.GetAtPath(tex_path) as TextureImporter;
        textureImporter.isReadable = true;
        TextureImporterPlatformSettings t = new TextureImporterPlatformSettings();
        t.format = TextureImporterFormat.ARGB32;
        t.textureCompression = TextureImporterCompression.Compressed;
        textureImporter.SetPlatformTextureSettings(t);
        AssetDatabase.ImportAsset(tex_path);
    }
    static void RecoveryTex(string tex_path)
    {
        TextureImporter textureImporter = AssetImporter.GetAtPath(tex_path) as TextureImporter;
        TextureImporterPlatformSettings t = new TextureImporterPlatformSettings();
        t.format = TextureImporterFormat.DXT5Crunched;
        t.textureCompression = TextureImporterCompression.Compressed;
        textureImporter.SetPlatformTextureSettings(t);
        textureImporter.isReadable = false;
        AssetDatabase.ImportAsset(tex_path);
    }
    static void SaveTex2Local(Texture2D tex, string path)
    {
        byte[] bytes = tex.EncodeToPNG();
        FileStream file = File.Open(path, FileMode.Create);
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(bytes);
        file.Close();
    }

    
    static string GetDirPath(Texture tex)
    {
        string tex_path = AssetDatabase.GetAssetPath(tex);
        string[] str = tex_path.Split('/');
        string dirPath = tex_path.Substring(0, tex_path.Length - str[str.Length - 1].Length);
        return dirPath;
    }
    static Texture ModifyShaderAndTex(string path)
    {
        var tex = AssetDatabase.LoadAssetAtPath(path, typeof(Object)) as Texture2D;
        if (tex != null)
        {
            ModifyImageSettings(path);
        }
        return tex;
    }
    static void ModifyImageSettings(string texPath)
    {
        if (texPath.EndsWith("_a.png") || texPath.EndsWith("_rgb.png"))
        {
            TextureImporter textureImporter = AssetImporter.GetAtPath(texPath) as TextureImporter;
            textureImporter.isReadable = false;

            TextureImporterPlatformSettings t0 = new TextureImporterPlatformSettings();
            t0.overridden = true;
            t0.name = "Android";
            t0.textureCompression = TextureImporterCompression.Compressed;
            t0.compressionQuality = 100;
            t0.format = TextureImporterFormat.ETC_RGB4;
            textureImporter.SetPlatformTextureSettings(t0);

            TextureImporterPlatformSettings t1 = new TextureImporterPlatformSettings();
            t1.overridden = true;
            t1.name = "iPhone";
            t1.textureCompression = TextureImporterCompression.Compressed;
            t1.compressionQuality = 100;
            t1.format = TextureImporterFormat.PVRTC_RGB4;
            textureImporter.SetPlatformTextureSettings(t1);
            textureImporter.mipmapEnabled = false;
            textureImporter.alphaIsTransparency = true;
            AssetDatabase.ImportAsset(texPath);
        }
    }

    #endregion

    [MenuItem("Assets/AssetBundle/ClearBundleName", false, 50)]
    static void ClearBundleName()
    {
        string path = "";
        Object[] selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object obj in selectedAsset)
        {
            string tp = AssetDatabase.GetAssetPath(obj);
            if (Path.GetExtension(tp) == "")
            {
                if (path == "" || path.Length > tp.Length)
                {
                    path = tp;
                }
            }
        }

        print(path);
        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        //var dir = new DirectoryInfo(path);
        var tfs = AssetDatabase.FindAssets("*", new string[] { path });


        foreach (var f in tfs)
        {
            var tp = AssetDatabase.GUIDToAssetPath(f);
            //var tro = AssetDatabase.LoadAssetAtPath(tp, typeof(Object));
            var ai = AssetImporter.GetAtPath(tp);
            if (!string.IsNullOrEmpty(ai.assetBundleName))
            {
                ai.assetBundleName = "";
                Debug.Log(tp);
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.RemoveUnusedAssetBundleNames();
    }

    [MenuItem("Assets/AssetBundle/SliceMultipleSpriteToSingle", false, 60)]
    public static void SliceMultipleSpriteToSingle()
    {

        foreach (var o in Selection.objects)
        {
            if (o is Sprite)
            {
                var s = o as Sprite;
                string tp = AssetDatabase.GetAssetPath(o);
                FileInfo fi = new FileInfo(tp);
                Debug.Log(fi.Directory.FullName);
                var finalPath = Path.Combine(fi.Directory.FullName, s.name + ".png");

                var tex = new Texture2D((int)s.rect.width, (int)s.rect.height);
                Debug.Log(s.rect);
                tex.SetPixels(s.texture.GetPixels((int)s.rect.x, (int)s.rect.y, (int)s.rect.width, (int)s.rect.height));
                //var p=s.texture.GetPixels();
                File.WriteAllBytes(finalPath, tex.EncodeToPNG());
                Debug.Log(o.name + " " + o.GetType() + " " + tp);
            }
        }
    }

    [MenuItem("Assets/UpSceneObjectShadowLittle", false, 60)]
    public static void UpSceneObjectShadowLittle()
    {
        Object[] gameObjects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (var gameObject in gameObjects)
        {
            GameObject go = gameObject as GameObject;
            if (go != null)
            {
                if (go.name.StartsWith("Furniture_"))
                {
                    for (int i = 0; i < go.transform.childCount; i++)
                    {
                        if (go.transform.GetChild(i).name.StartsWith("Shadow_"))
                        {
                            //Debug.Log(go.transform.GetChild(i).name);
                            Vector3 oldTransform = go.transform.GetChild(i).localPosition;
                            go.transform.GetChild(i).localPosition = oldTransform + new Vector3(0, 0.01f, 0);
                        }
                    }
                }
            }
        }
    }
}
