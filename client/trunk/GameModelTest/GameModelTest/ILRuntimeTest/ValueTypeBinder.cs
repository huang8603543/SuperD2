using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameModelTest
{
    public class ValueTypeBinder
    {
        public static void RunTest()
        {
            float dot = 0;
            Vector3 a = new Vector3(1, 2, 3);
            Vector3 b = Vector3.one;
            for (int i = 0; i < 1000000; i++)
            {
                a += Vector3.one;
                dot += Vector3.Dot(a, Vector3.zero);
            }
        }
    }
}
