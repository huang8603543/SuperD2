using System;
using UnityEngine;

namespace Framework.Core
{
    public class GameApplication : MonoBehaviour
    {
        private static GameApplication _instance;
        public static GameApplication Instance
        {
            get
            {
                return _instance;
            }
        }

        public IHotFixMain hotFix;

        public HotFixILRunTime ILHotFix
        {
            get
            {
                return (HotFixILRunTime)hotFix;
            }
        }

        public HotFixReflector ReHotFix
        {
            get
            {
                return (HotFixReflector)hotFix;
            }
        }

        public bool useILHotFix = true;

        public Action LoadAssemblyOver;

        void Awake()
        {
            _instance = this;

            if (useILHotFix)
            {
                hotFix = HotFixILRunTime.GetInstance();
            }
            else
            {
                hotFix = HotFixReflector.GetInstance();
                ReHotFix.loadAssemblyOver += () => ReflectTest.GetInstance();
            }
        }

        void Start()
        {
            if (useILHotFix)
            {
                ILRuntimeTest.GetInstance();
            }
        }

        private void Update()
        {

        }

        private void LateUpdate()
        {
            
        }

        private void OnApplicationQuit()
        {
            
        }

    }
}
