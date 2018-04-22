using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Util;
using System.IO;
using System;
using Framework.MVVM;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.CLR.TypeSystem;

namespace Framework.Core
{
    public class HotFixILRunTime : SingletonMono<HotFixILRunTime>, IHotFixMain
    {
        public ILRuntime.Runtime.Enviorment.AppDomain appDomain;
        byte[] dllBytes;
        byte[] pdbBytes;

        public bool usePdb = true;

        public Type LoadType(string typeName)
        {
            if (appDomain.LoadedTypes.ContainsKey(typeName))
            {
                return appDomain.LoadedTypes[typeName].ReflectionType;
            }
            return null;
        }

        public object CreateInstance(string typeName)
        {
            IType type = GameApplication.Instance.ILHotFix.appDomain.LoadedTypes[typeName];
            var instance = ((ILType)type).Instantiate();
            return instance;
        }

        void Awake()
        {
            StartCoroutine(LoadHotFixAssembly());
        }

        IEnumerator LoadHotFixAssembly()
        {
            appDomain = new ILRuntime.Runtime.Enviorment.AppDomain();
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
                        appDomain.LoadAssembly(fs, p, new Mono.Cecil.Pdb.PdbReaderProvider());
                    }
                }
                else
                {
                    appDomain.LoadAssembly(fs, null, new Mono.Cecil.Pdb.PdbReaderProvider());
                }
            }

            InitializeILRuntime();
        }

        void InitializeILRuntime()
        {
            #region Delegate

            appDomain.DelegateManager.RegisterMethodDelegate<int>();
            appDomain.DelegateManager.RegisterFunctionDelegate<int, string>();
            appDomain.DelegateManager.RegisterMethodDelegate<string>();
            appDomain.DelegateManager.RegisterMethodDelegate<int, int>();
            appDomain.DelegateManager.RegisterMethodDelegate<List<int>, List<int>>();
            appDomain.DelegateManager.RegisterMethodDelegate<string, string>();
            appDomain.DelegateManager.RegisterMethodDelegate<object, MessageArgs<object>>();
            appDomain.DelegateManager.RegisterMethodDelegate<object, MessageArgs<ILTypeInstance>>();

            //appDomain.DelegateManager.RegisterDelegateConvertor<TestDelegateMethod>((action) =>
            //{
            //    //转换器的目的是把Action或者Func转换成正确的类型，这里则是把Action<int>转换成TestDelegateMethod
            //    return new TestDelegateMethod((a) =>
            //    {
            //        //调用委托实例
            //        ((Action<int>)action)(a);
            //    });
            //});

            //appDomain.DelegateManager.RegisterDelegateConvertor<TestDelegateFunction>((action) =>
            //{
            //    return new TestDelegateFunction((a) =>
            //    {
            //        return ((Func<int, string>)action)(a);
            //    });
            //});

            appDomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<float>>((action) =>
            {
                return new UnityEngine.Events.UnityAction<float>((a) =>
                {
                    ((Action<float>)action)(a);
                });
            });

            #endregion

            #region CLRBinding

            //ILRuntime.Runtime.Generated.CLRBindings.Initialize(appDomain);

            #endregion

            #region Adaptor

            appDomain.RegisterCrossBindingAdaptor(new ViewModelBaseAdapter());
            appDomain.RegisterCrossBindingAdaptor(new UnityGuiViewAdapter());
            appDomain.RegisterCrossBindingAdaptor(new ModuleBaseAdapter());

            #endregion

            appDomain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());


            #region UniRX

            appDomain.DelegateManager.RegisterMethodDelegate<Exception>();
            appDomain.DelegateManager.RegisterMethodDelegate<UniRx.Unit>();

            #endregion
        }
    }
}
