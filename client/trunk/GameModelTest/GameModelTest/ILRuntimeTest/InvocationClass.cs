using Framework.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameModelTest
{
    public class InvocationClass
    {
        private int _id;

        public int ID
        {
            get
            {
                Debug.Log("Call InvocationClass.get_ID and ID: " + _id);
                LoggerProvider.Debug.Write("Call InvocationClass.get_ID and ID: " + _id);
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public string Des
        {
            get;
            set;
        } = "hey";

        public InvocationClass()
        {
            Debug.Log("Instantiate InvocationClass");
            LoggerProvider.Debug.Write("Instantiate InvocationClass");
        }

        public InvocationClass(int id, string des)
        {
            ID = id;
            Des = des;
            Debug.Log(string.Format("Instantiate InvocationClass and ID:{0} and Des:{1}", ID, Des));
            LoggerProvider.Debug.Write(string.Format("Instantiate InvocationClass and ID:{0} and Des:{1}", ID, Des));
        }

        public static void StaticFunTest()
        {
            Debug.Log("Call InvocationClass.StaticFunTest()");
            LoggerProvider.Debug.Write("Call InvocationClass.StaticFunTest()");
        }

        public static void StaticFunTest2(int a)
        {
            Debug.Log("Call InvocationClass.StaticFunTest2() and a: " + a);
            LoggerProvider.Debug.Write("Call InvocationClass.StaticFunTest2() and a: " + a);
        }

        public void InstanceMethod()
        {
            Debug.Log("Call InvocationClass.InstanceMethod() and Des: " + Des);
            LoggerProvider.Debug.Write("Call InvocationClass.InstanceMethod() and Des: " + Des);
        }

        public static void GenericMethod<T>(T a)
        {
            Debug.Log("Call InvocationClass.GenericMethod() and a: " + a);
            LoggerProvider.Debug.Write("Call InvocationClass.GenericMethod() and a: " + a);
        }


    }
}
