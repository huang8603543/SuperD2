using Framework.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameModelTest
{
    public class Inheritance : TestClassBase
    {
        public override void TestAbstract(int gg)
        {
            Debug.Log("Inheritance.TestAbstract() and gg: " + gg);
            LoggerProvider.Debug.Write("Inheritance.TestAbstract() and gg: " + gg);
        }

        public override void TestVirtual(string str)
        {
            base.TestVirtual(str);
            Debug.Log("Inheritance.TestVirtual() and str: " + str);
            LoggerProvider.Debug.Write("Inheritance.TestVirtual() and str: " + str);
        }

        public static Inheritance NewObject()
        {
            return new Inheritance();
        }
    }
}
