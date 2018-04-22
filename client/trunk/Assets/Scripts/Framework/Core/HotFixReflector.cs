using Framework.Util;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Framework.Core
{
    public class HotFixReflector : SingletonMono<HotFixReflector>, IHotFixMain
    {
        public Assembly assembly;
        byte[] dllBytes;
        byte[] pdbBytes;

        public bool usePdb = true;

        public Action loadAssemblyOver;

        public Type LoadType(string typeName)
        {
            Type[] types = assembly.GetTypes();
            Type type = assembly.GetTypes().FirstOrDefault(t => t.FullName == typeName);
            if (type == null)
            {
                throw new Exception(string.Format("Cant't find Class by class name:'{0}'", typeName));
            }
            return type;
        }

        public object CreateInstance(string typeName)
        {
            return Activator.CreateInstance(LoadType(typeName));
        }

        void Awake()
        {
            StartCoroutine(LoadHotFixAssembly());
        }

        IEnumerator LoadHotFixAssembly()
        {
#if UNITY_ANDROID
        WWW www = new WWW(Application.streamingAssetsPath + "/GameModelTest.dll");
#else
            WWW www = new WWW("file:///" + Application.streamingAssetsPath + "/GameModelTest.dll");
#endif
            while (!www.isDone)
            {
                yield return null;
            }
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError(www.error);
            }
            dllBytes = www.bytes;
            www.Dispose();

            if (usePdb)
            {
#if UNITY_ANDROID
        www = new WWW(Application.streamingAssetsPath + "/GameModelTest.pdb");
#else
                www = new WWW("file:///" + Application.streamingAssetsPath + "/GameModelTest.pdb");
#endif
                while (!www.isDone)
                {
                    yield return null;
                }
                if (!string.IsNullOrEmpty(www.error))
                {
                    Debug.LogError(www.error);
                }
                pdbBytes = www.bytes;
            }

            using (MemoryStream fs = new MemoryStream(dllBytes))
            {
                if (pdbBytes != null)
                {
                    using (MemoryStream p = new MemoryStream(pdbBytes))
                    {
                        assembly = Assembly.Load(dllBytes, pdbBytes);
                    }
                }
                else
                {
                    assembly = AppDomain.CurrentDomain.Load(dllBytes);
                }
                if (loadAssemblyOver != null)
                {
                    loadAssemblyOver();
                }
            }
        }
    }
}
