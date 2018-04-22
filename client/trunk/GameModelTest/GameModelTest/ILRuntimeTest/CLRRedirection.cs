using Framework.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameModelTest
{
    public class CLRRedirection
    {
        public static void RunTest()
        {
            Debug.Log("看看这行的详细Log信息");
            LoggerProvider.Debug.Write("看看这行的详细Log信息");
        }
    }
}
