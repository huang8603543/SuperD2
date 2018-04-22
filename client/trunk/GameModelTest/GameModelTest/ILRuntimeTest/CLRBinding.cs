using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModelTest
{
    public class CLRBinding
    {
        public static void RunTest()
        {
            for (int i = 0; i < 100000; i++)
            {
                CLRBindingTestClass.DoSomeTest(i, i);
            }
        }
    }
}
