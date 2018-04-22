using Framework.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GameModelTest
{
    public class UniRXTest
    {
        public static void TestOne()
        {
            ObservableWWW.Get("http://www.baidu.com")
                .Subscribe(
                    x => {
                        string str = x.Substring(0, 100);
                        Debug.Log(str);
                        LoggerProvider.Debug.Write(str);
                    },
                    ex =>
                    {
                        Debug.LogException(ex);
                        LoggerProvider.Debug.Write(ex.ToString());
                    }
                );
        }

        public static Button testButton;

        public static void TestTwo()
        {
            testButton = GameObject.Find("TestButton").GetComponent<Button>();
            testButton.onClick.AsObservable()
                .Throttle(TimeSpan.FromSeconds(2))
                .Do(_ => Debug.Log("111"))
                .Throttle(TimeSpan.FromSeconds(2))
                .Subscribe(_ => Debug.Log("222"));
        }
    }
}
