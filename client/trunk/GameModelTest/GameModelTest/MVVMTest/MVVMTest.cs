using Framework.MVVM;
using UnityEngine;

namespace GameModelTest
{
    public class MVVMTest
    {
        public static void TestOne()
        {
            BindableProperty<int> testInt = new BindableProperty<int>();

            testInt.OnValueChanged += (oldValue, newValue) => Debug.Log(string.Format("oldValue:{0}, newValue:{1}", oldValue, newValue));

            testInt.Value = 10;
            //Debug.Log("testInt: " + testInt);
            testInt.Value = 20;
            //Debug.Log("testInt: " + testInt);
            testInt.Value = 30;
            //Debug.Log("testInt: " + testInt);
            testInt.Value = 40;
        }

        public static void TestTwo()
        {
            ObservableList<int> testList = new ObservableList<int>();
            testList.OnValueChanged += (oldValue, newValue) => Debug.Log("Changed");
            testList.OnAdd += (addValue) => Debug.Log("addValue" + addValue);
            testList.OnRemove += (removeValue) => Debug.Log("removeValue" + removeValue);

            testList.Add(100);
            testList.Add(200);
            testList.Remove(100);
            testList.Add(300);
        }

        public static void TestThree()
        {
            TestOneModule oneModule = new TestOneModule();
            oneModule.Excute();
        }
    }


}
