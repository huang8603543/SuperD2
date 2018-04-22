using Framework.MVVM;
using Framework.Core;
using UnityEngine;

namespace GameModelTest
{
    public class TestOneModule : ModuleBase
    {
        public MVVMTestPanel mvvmTestPanel;
        public ServiceLocator serviceLocator = new ServiceLocator();

        public override void OnInitialize()
        {
            InjectDepends();
            GameObject obj = Resources.Load("MVVMTestPanel") as GameObject;
            if (obj != null)
            {
                GameObject panel = Object.Instantiate(obj);
                panel.name = "MVVMTestPanel";
                panel.transform.SetParent(GameObject.Find("Canvas").transform, false);
                mvvmTestPanel = new MVVMTestPanel();
                mvvmTestPanel.BindingContext = new MVVMTestModel();
            }
        }

        public override void Excute()
        {
            OnInitialize();
            mvvmTestPanel.Reveal();
            serviceLocator.Resolve<TestOneModuleDepend>(typeof(TestOneModuleDepend).FullName).LogTest();
            serviceLocator.Resolve<ITestOneModuleDepend>(typeof(ITestOneModuleDepend).FullName).LogTest();
        }

        void InjectDepends()
        {
            serviceLocator.RegisterSingleton(typeof(TestOneModuleDepend).FullName);
            serviceLocator.RegisterSingleton(typeof(ITestOneModuleDepend).FullName, typeof(TestOneModuleDependInterfece).FullName);
        }
    }

    public class TestOneModuleDepend
    {
        public void LogTest()
        {
            Debug.Log("Injected Depend");
        }
    }

    public interface ITestOneModuleDepend
    {
        void LogTest();
    }

    public class TestOneModuleDependInterfece : ITestOneModuleDepend
    {
        public void LogTest()
        {
            Debug.Log("Injected DependInterface");
            float dot = 0;
            Vector3 a = new Vector3(1, 2, 3);
            Vector3 b = Vector3.one;
            for (int i = 0; i < 100000; i++)
            {
                a += Vector3.one;
                dot += Vector3.Dot(a, Vector3.zero);
            }
        }
    }
}
