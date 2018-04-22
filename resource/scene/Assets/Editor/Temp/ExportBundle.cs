using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

public class ExportBundle : MonoBehaviour
{
    private static string tempStreamingAssetsPublishPath = "../../../client/trunk/TempStreamingAssets";
    private static bool copyZipOnly = false;

    public static string bundleExtension = ".ab";
    public static string zipExtension = ".zip";
    public static string sceneDataExtension = ".data";

    static List<AssetBundleBuild> assetBundleBuildList;

    public static string GetBundleRoot(BuildTarget bt)
    {
        string path = Path.Combine(Application.dataPath, "../TempStreamingAssets/" + GetBundleFolderName(bt));
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return path;
    }

    public static string GetPublishBundleRoot(BuildTarget bt)
    {
        string path = Path.Combine(Application.dataPath, "../TempStreamingAssetsPublish/" + GetBundleFolderName(bt));
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return path;
    }

    public static string GetBundleFolderName(BuildTarget bt)
    {
        if (bt == BuildTarget.Android)
            return "Android";
        else if (bt == BuildTarget.iOS)
            return "IOS";
        else if (bt == BuildTarget.StandaloneWindows)
            return "Windows";
        else
            return "Web";
    }

    public static BuildTarget GetBuildTarget()
    {
        BuildTarget bt = BuildTarget.iOS;
#if UNITY_ANDROID
        bt = BuildTarget.Android;
#elif UNITY_IPHONE
		bt = BuildTarget.iOS;	
#elif UNITY_STANDALONE_WIN
        bt = BuildTarget.StandaloneWindows;
#else
        bt = BuildTarget.WebPlayerStreamed;
#endif
        return bt;
    }

    #region Build

    // BEGIN ResBuilder
    static void BuildChangedBundle_ResBuilder_Win32() 
    {
        BuildChangedBundle_ResBuilder(BuildTarget.StandaloneWindows);
    }

    static void BuildChangedBundle_ResBuilder_iOS() 
    {
        BuildChangedBundle_ResBuilder(BuildTarget.iOS);
    }

    static void BuildChangedBundle_ResBuilder_Android() 
    {
        BuildChangedBundle_ResBuilder(BuildTarget.Android);
    }

    static void BuildChangedBundle_ResBuilder(BuildTarget bt) 
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(bt);
        string configStr = File.ReadAllText("BuildBundleParams.txt");
        string[] configStrAry = configStr.Split(';');
        string publishPath = configStrAry[0];
        string copyZipOnlyStr = configStrAry[1];
        tempStreamingAssetsPublishPath = publishPath;
        copyZipOnly = copyZipOnlyStr == "true";
        var root = GetBundleRoot(bt);
        BuildAllBundle(root, bt, BuildAssetBundleOptions.None);
    }
    // END ResBuilder

    [MenuItem("Assets/AssetBundle/BuildChangedBundle", false, 1)]
    static void BuildChangedBundle()
    {
        BuildTarget bt = GetBuildTarget();
        var root = GetBundleRoot(bt);
        BuildAllBundle(root, bt, BuildAssetBundleOptions.StrictMode);
    }

    [MenuItem("Assets/AssetBundle/RebuildAllBundle", false, 2)]
    static void RebuildAllBundle()
    {
        var bt = GetBuildTarget();
        BuildAllBundle(GetBundleRoot(bt), bt, BuildAssetBundleOptions.ForceRebuildAssetBundle | BuildAssetBundleOptions.StrictMode);
    }

    #endregion

    static void BuildAllBundle(string root, BuildTarget bt, BuildAssetBundleOptions options)
    {
        AssetDatabase.RemoveUnusedAssetBundleNames();
        string[] names = AssetDatabase.GetAllAssetBundleNames();
        assetBundleBuildList = new List<AssetBundleBuild>();

        int len = names.Length;
        for (int i = 0; i < len; i++)
        {
            CreateNewAssetBundleBuild(names[i]);
        }
        
        var result = BuildPipeline.BuildAssetBundles(root, assetBundleBuildList.ToArray(), options, bt);
        if (result == null) {
            Debug.Log("Error while building asset bundles, please check unity log");
            throw new System.Exception("Error while building asset bundles, please check unity log");
        }

        CopySceneGuides(Application.dataPath + "/HotFix/SceneGuides", root + "/SceneGuides");
        //EncryptAssetBundleFiles(root);
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
        {
            GenerateZipsOfSceneProject(root, GetPublishBundleRoot(bt), bt);
        }
        if (!copyZipOnly)
            CopyBundleToClient(root, bt);
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
        {
            MoveZIPToProjectPublish(bt);
            CopyBundleZIPToClient(bt);
            GenerateAssetBundleList(bt);
        }
        EditorUtility.DisplayDialog("CopyBundleToClient", "复制成功", "Ok");
    }

    static void CopySceneGuides(string srcDir, string tgtDir)
    {
        CopyDirectory(srcDir, tgtDir);
    }

    static void CreateNewAssetBundleBuild(string name)
    {
        string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(name);
        AssetBundleBuild assetBundleBuild = new AssetBundleBuild();
        assetBundleBuild.assetBundleName = name + bundleExtension;
        assetBundleBuild.assetBundleVariant = null;
        assetBundleBuild.assetNames = assetPaths;
        assetBundleBuildList.Add(assetBundleBuild);
    }

    static void EncryptAssetBundleFiles(string root)
    {
        DirectoryInfo source = new DirectoryInfo(root);
        if (source.Exists)
        {
            FileInfo[] files = source.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                EncryptAssetBundleFile(files[i]);
            }
            DirectoryInfo[] dirs = source.GetDirectories();
            for (int j = 0; j < dirs.Length; j++)
            {
                EncryptAssetBundleFiles(dirs[j].FullName);
            }
        }
    }

    static void EncryptAssetBundleFile(FileInfo fileInfo)
    {
        if (fileInfo.Extension == bundleExtension)
        {
            FileStream fileStream = new FileStream(fileInfo.ToString(), FileMode.Open, FileAccess.ReadWrite);
            byte[] buff = new byte[fileStream.Length];
            fileStream.Read(buff, 0, (int)fileStream.Length);
            if (!IsFileEncrypted(buff))
            {
                EncryptAlgorithm(fileStream, buff);
            }
            fileStream.Flush();
            fileStream.Close();
        }
    }

    static bool IsFileEncrypted(byte[] buff)
    {
        return buff[buff.Length - 1] == 14 && buff[buff.Length - 2] == 13 && buff[buff.Length - 3] == 20 && buff[buff.Length - 4] == 5;
    }

    static void EncryptAlgorithm(FileStream fileStream, byte[] buff)
    {
        byte[] bytes = new byte[fileStream.Length];
        bytes = AESHelper.AESEncrypt(buff);
        byte[] newBytes = new byte[bytes.Length + 4];
        for (int k = 0; k < bytes.Length; k++)
        {
            newBytes[k] = bytes[k];
        }
        newBytes[newBytes.Length - 4] = 5;
        newBytes[newBytes.Length - 3] = 20;
        newBytes[newBytes.Length - 2] = 13;
        newBytes[newBytes.Length - 1] = 14;
        fileStream.Position = 0;
        fileStream.Write(newBytes, 0, newBytes.Length);
    }

    static void CopyBundleToClient(string root, BuildTarget bt)
    {
        string clientDir = Path.Combine(Application.dataPath, Path.Combine(tempStreamingAssetsPublishPath, GetBundleFolderName(bt)));
        CopyDirectory(root, clientDir);
        //GenerateAssetBundleList(GetBundleRoot(bt));
        GenerateAssetBundleList(clientDir);
    }

    static void MoveZIPToProjectPublish(BuildTarget bt)
    {
        DirectoryInfo source = new DirectoryInfo(Path.Combine(Application.dataPath, "../"));
        FileInfo[] files = source.GetFiles();
        int len = files.Length;
        for (int i = 0; i < len; i++)
        {
            if (files[i].Extension == zipExtension)
            {
                if (File.Exists(Path.Combine(GetPublishBundleRoot(bt), files[i].Name)))
                {
                    File.Delete(Path.Combine(GetPublishBundleRoot(bt), files[i].Name));
                }
                File.Move(files[i].FullName, Path.Combine(GetPublishBundleRoot(bt), files[i].Name));
            }
        }
    }

    static void CopyBundleZIPToClient(BuildTarget bt)
    {
        string clientPublishDir = Path.Combine(Application.dataPath, Path.Combine(tempStreamingAssetsPublishPath, GetBundleFolderName(bt)));

        DirectoryInfo source = new DirectoryInfo(GetPublishBundleRoot(bt));
        FileInfo[] files = source.GetFiles();
        int len = files.Length;
        for (int i = 0; i < len; i++)
        {
            if (files[i].Extension == zipExtension)
            {
                if (File.Exists(Path.Combine(clientPublishDir, files[i].Name)))
                {
                    File.Delete(Path.Combine(clientPublishDir, files[i].Name));
                }
                File.Copy(files[i].FullName, Path.Combine(clientPublishDir, files[i].Name));
            }
        }
    }

    static void GenerateAssetBundleList(BuildTarget bt)
    {
        string clientPublishDir = Path.Combine(Application.dataPath, Path.Combine(tempStreamingAssetsPublishPath, GetBundleFolderName(bt)));
        GenerateAssetBundleList(clientPublishDir);
    }

    static void GenerateAssetBundleList(string path)
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] files = dir.GetFiles("*", SearchOption.AllDirectories);
        string tpath = dir.FullName.Replace("\\", "/");
        string ls = string.Empty;
        foreach (var f in files)
        {
            if (f.FullName.EndsWith(bundleExtension) || f.FullName.EndsWith(zipExtension) || f.FullName.EndsWith(sceneDataExtension))
            {
                using (var s = new StreamReader(f.FullName))
                {
                    var ms = CryptoHelp.MD5(s.BaseStream);
                    var fullFillName = f.FullName.Replace("\\", "/");

                    ls += fullFillName.Replace(tpath + "/", "") + "," + ms + "," + s.BaseStream.Length + "\n";
                }
            }
        }

        string str = Path.Combine(path, "ResourceList.txt");

        if (str.Length != 0)
        {
            FileStream cache = new FileStream(str, FileMode.Create);
            var ec = new System.Text.UTF8Encoding();
            var bytes = ec.GetBytes(ls);
            cache.Write(bytes, 0, bytes.Length);
            cache.Close();
        }
        EditorUtility.DisplayDialog("GenerateAssetBundleListSuccess ", "清单地址 " + str, "Ok");
    }

    static void CopyDirectory(string srcDir, string tgtDir)
    {
        DirectoryInfo source = new DirectoryInfo(srcDir);
        DirectoryInfo target = new DirectoryInfo(tgtDir);

        if (target.FullName.StartsWith(source.FullName, System.StringComparison.CurrentCultureIgnoreCase))
        {
            Debug.LogError("父目录不能拷贝到子目录！");
            return;
        }
        if (!source.Exists)
        {
            return;
        }
        if (!target.Exists)
        {
            target.Create();
        }
        FileInfo[] files = source.GetFiles();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Extension == bundleExtension || files[i].Extension == sceneDataExtension)
            {
                File.Copy(files[i].FullName, Path.Combine(target.FullName, files[i].Name), true);
            }
        }
        DirectoryInfo[] dirs = source.GetDirectories();
        for (int j = 0; j < dirs.Length; j++)
        {
            CopyDirectory(dirs[j].FullName, Path.Combine(target.FullName, dirs[j].Name));
        }
    }

    static void GenerateZipsOfSceneProject(string srcDir, string publishDir, BuildTarget bt)
    {
        DirectoryInfo source = new DirectoryInfo(srcDir);
        DirectoryInfo publish = new DirectoryInfo(publishDir);
        if (!source.Exists)
        {
            return;
        }
        DirectoryInfo[] dictsOfSource = source.GetDirectories();
        foreach (var dict in dictsOfSource)
        {
            if (dict.Name == "SceneGuides")
            {
                FileInfo[] files = dict.GetFiles();
                if (files.Length > 0)
                {
                    GenerateZIPFile(files, "SceneGuides", bt);
                }
            }

            if (dict.Name == "scene")
            {
                FileInfo[] files = dict.GetFiles();
                if (files.Length > 0)
                {
                    GenerateZIPFile(files, "scene", bt);
                }

                DirectoryInfo[] dictsOfScene = dict.GetDirectories();
                foreach (var dict2 in dictsOfScene)
                {
                    if (dict2.Name == "scene")
                    {
                        GenerateZIPFile(dict2, "scene_", bt);
                    }
                    else
                    {
                        DirectoryInfo[] dictsOfSceneChild = dict2.GetDirectories();
                        foreach (var dict3 in dictsOfSceneChild)
                        {
                            CreateZipAlgorithmOfScene(dict3, "scene_" + dict2.Name + "_" + dict3.Name, bt);
                        }                        
                    }
                }
            }
        }
    }

    static void CreateZipAlgorithmOfScene(DirectoryInfo root, string perString, BuildTarget bt)
    {
        FileInfo[] files = root.GetFiles();
        
        Dictionary<int, List<FileInfo>> subFiles = new Dictionary<int, List<FileInfo>>();
        foreach (FileInfo file in files)
        {
            if (file.Extension == bundleExtension)
            {
                int num = int.Parse(file.Name.Substring(file.Name.Length - 7, 3));
                if (!subFiles.ContainsKey(num))
                {
                    List<FileInfo> dictList = new List<FileInfo>();
                    dictList.Add(file);
                    subFiles.Add(num, dictList);
                }
                else
                {
                    subFiles[num].Add(file);
                }
            }
        }
        GenerateZIPFile(subFiles, root.Name, perString, bt);
    }

    static void GenerateZIPFile(Dictionary<int, List<FileInfo>> subDict, string tag, string perString, BuildTarget bt)
    {
        foreach (var dictList in subDict)
        {
            string path = perString + "_" + tag + string.Format("{0:D3}", dictList.Key) + "0_" + string.Format("{0:D3}", dictList.Key) + "9" + zipExtension;
            ZipOutputStream outPutStream = new ZipOutputStream(File.Open(path, FileMode.OpenOrCreate));
            AddZIPFile(outPutStream, dictList.Value.ToArray(), bt);
            outPutStream.Finish();
            outPutStream.Close();
        }
    }

    static void GenerateZIPFile(DirectoryInfo root, string perString, BuildTarget bt)
    {
        ZipOutputStream outPutStream = new ZipOutputStream(File.Open(perString + "_" + root.Name + zipExtension, FileMode.OpenOrCreate));
        AddZIPFile(outPutStream, root, bt);
        outPutStream.Finish();
        outPutStream.Close();
    }

    static void GenerateZIPFile(FileInfo[] root, string perString, BuildTarget bt)
    {
        ZipOutputStream outPutStream = new ZipOutputStream(File.Open(perString + zipExtension, FileMode.OpenOrCreate));
        AddZIPFile(outPutStream, root, bt);
        outPutStream.Finish();
        outPutStream.Close();
    }

    static void AddZIPFile(ZipOutputStream outPutStream, DirectoryInfo[] dirInfos, BuildTarget bt)
    {
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            AddZIPFile(outPutStream, dirInfo, bt);
        }
    }

    static void AddZIPFile(ZipOutputStream outPutStream, FileInfo[] fileInfos, BuildTarget bt)
    {
        foreach (FileInfo fileInfo in fileInfos)
        {
            AddZIPFile(outPutStream, fileInfo, bt);
        }
    }

    static void AddZIPFile(ZipOutputStream outPutStream, DirectoryInfo dirInfo, BuildTarget bt)
    {
        FileInfo[] fileInfos = dirInfo.GetFiles();
        AddZIPFile(outPutStream, fileInfos, bt);
        DirectoryInfo[] dirInfos = dirInfo.GetDirectories();
        AddZIPFile(outPutStream, dirInfos, bt);
    }

    static void AddZIPFile(ZipOutputStream outPutStream, FileInfo fileInfo, BuildTarget bt)
    {
        outPutStream.SetLevel(6);
        if (fileInfo.Extension == bundleExtension || fileInfo.Extension == sceneDataExtension)
        {
            string cutStr = GetCutString(bt);
            int index = fileInfo.FullName.LastIndexOf(cutStr) + cutStr.Length + 1;
            string path = fileInfo.FullName.Substring(index);
            string[] strs = path.Split('\\');
            path = string.Empty;
            for (int i = 0; i < strs.Length; i++)
            {
                if (i == strs.Length - 1)
                {
                    path = path + strs[i];
                }
                else
                {
                    path = path + strs[i] + "/";
                }
            }
            ZipEntry entry = new ZipEntry(path);
            entry.CompressionMethod = CompressionMethod.Stored;
            entry.DateTime = new System.DateTime(10000);
            outPutStream.PutNextEntry(entry);
            FileStream fileStream = fileInfo.OpenRead();
            byte[] buffer = new byte[fileInfo.Length];
            fileStream.Read(buffer, 0, buffer.Length);
            outPutStream.Write(buffer, 0, buffer.Length);
            fileStream.Close();
        }
    }

    static string GetCutString(BuildTarget bt)
    {
        string cutStr;
        if (bt == BuildTarget.Android)
            cutStr = "Android";
        else if (bt == BuildTarget.iOS)
            cutStr = "IOS";
        else if (bt == BuildTarget.StandaloneWindows)
            cutStr = "Windows";
        else
            cutStr = "Web";
        return cutStr;
    }

    [MenuItem("Assets/AssetBundle/HZCSetAssetBundleNameForScene", false, 50)]
    static void HZCSetAssetBundleNameForScene()
    {
        string path = string.Empty;
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

        string[] materils = AssetDatabase.FindAssets("t:material", new string[] { path });
        foreach (var f in materils)
        {
            string tp = AssetDatabase.GUIDToAssetPath(f);
            Object tro = AssetDatabase.LoadAssetAtPath(tp, typeof(Object));
            AssetImporter ai = AssetImporter.GetAtPath(tp);

            string tempTp = tp.Substring(20);
            if (tempTp.StartsWith("SceneComponent"))
            {
                string tempTp2 = tempTp.Substring(15);

                string[] strs = tempTp2.Split('/');
                int len = strs.Length;
                ai.assetBundleName = "scene/scenecomponent";
                for (int i = 0; i < len - 1; i++)
                {
                    ai.assetBundleName = ai.assetBundleName + "/" + strs[i];
                }
                ai.assetBundleName = ai.assetBundleName + "/" + tro.name;
            }           
        }

        string[] prefabs = AssetDatabase.FindAssets("t:Prefab", new string[] { path });
        foreach (var f in prefabs)
        {
            string tp = AssetDatabase.GUIDToAssetPath(f);
            Object tro = AssetDatabase.LoadAssetAtPath(tp, typeof(Object));
            AssetImporter ai = AssetImporter.GetAtPath(tp);

            string tempTp = tp.Substring(20);

            if (tempTp.StartsWith("SceneObject"))
            {
                string tempTp2 = tempTp.Substring(12);

                string[] strs = tempTp2.Split('/');
                int len = strs.Length;
                ai.assetBundleName = "scene";
                for (int i = 0; i < len - 1; i++)
                {
                    ai.assetBundleName = ai.assetBundleName + "/" + strs[i];
                }
                ai.assetBundleName = ai.assetBundleName + "/" + tro.name;
            }
        }
    }
}
