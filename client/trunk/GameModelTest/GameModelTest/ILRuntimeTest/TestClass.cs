using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameModelTest
{
    public class TestClass
    {
        public static void StaticFunTest()
        {
            Vector3[] vArray = new Vector3[100000];
            for (int i = 0; i < 100000; i++)
            {
                vArray[i] = new Vector3(i, i, i);
            }
        }
    }
}
