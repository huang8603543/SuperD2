using UnityEngine;
using ILRuntime.Runtime.Enviorment;
using System;
using UnityEngine.UI;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using System.Collections.Generic;
using ILRuntime.Runtime.Stack;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.CLR.Utils;
using Framework.Core;
using Framework.Util;
using System.Reflection;

public class ILRuntimeTest : SingletonMono<ILRuntimeTest>
{
    public Transform contentRoot;
    public Button button;

    void Start()
    {
        contentRoot = GameObject.Find("Content").transform;
        button = GameObject.Find("Button").GetComponent<Button>();
        TestCase();
    }

    unsafe void TestCase()
    {
        #region TestOne

        //new NativeTestOne("NativeTestOne", appDomain, contentRoot, button, true);
        //new ILRunTimeTestOne("ILRunTimeTestOne", appDomain, contentRoot, button, true);

        #endregion

        #region Invocation

        new ILRuntimeInvocationClass("InvocationClass", GameApplication.Instance.ILHotFix.appDomain, contentRoot, button, false);

        #endregion

        #region Delegate

        new ILDelegateClass("ILDelegateClass", GameApplication.Instance.ILHotFix.appDomain, contentRoot, button, false);

        #endregion

        #region Inheritance

        new ILInheritanceClass("ILInheritance", GameApplication.Instance.ILHotFix.appDomain, contentRoot, button, false);

        #endregion

        #region CLRRedirection

        new ILCLRRedirectionClass("CLRRedirection", GameApplication.Instance.ILHotFix.appDomain, contentRoot, button, false);

        #endregion

        #region CLRBinding

        new ILCLRBindingClass("ILCLRBinding", GameApplication.Instance.ILHotFix.appDomain, contentRoot, button, true);

        #endregion

        #region ValueTypeBinder

        new NativeValueTypeBinderClass("NativeValueTypeBinderClass", GameApplication.Instance.ILHotFix.appDomain, contentRoot, button, true);
        new ILRunTimeValueTypeBinderClass("ILValueTypeBinderClass", GameApplication.Instance.ILHotFix.appDomain, contentRoot, button, true);

        #endregion


        #region UniRX

        new UniRXTestOne("UniRXTestOne", GameApplication.Instance.ILHotFix.appDomain, contentRoot, button, false);
        new UniRXTestTwo("UniRXTestTwo", GameApplication.Instance.ILHotFix.appDomain, contentRoot, button, false);

        #endregion

        #region MVVM

        new MVVMTestOne("MVVMTestOne", GameApplication.Instance.ILHotFix.appDomain, contentRoot, button, false);
        new MVVMTestTwo("MVVMTestTwo", GameApplication.Instance.ILHotFix.appDomain, contentRoot, button, false);


        new MVVMPanelTestOne("MVVMPanelTestOne", GameApplication.Instance.ILHotFix.appDomain, contentRoot, button, true);

        #endregion
    }
}

#region TestCase

#region Inheritance

public abstract class TestClassBase
{
    public virtual int Value
    {
        get
        { return 0; }
    }

    public virtual void TestVirtual(string str)
    {
        Debug.Log("TestClassBase.TestVirtual() and str = " + str);
        LoggerProvider.Debug.Write("TestClassBase.TestVirtual() and str = " + str);
    }

    public abstract void TestAbstract(int gg);
}

#endregion

#region CLRBinding

public class CLRBindingTestClass
{
    public static float DoSomeTest(int a, float b)
    {
        return a + b;
    }
}

#endregion

#endregion

public class ExcuteTestClass
{
    public string TestName
    {
        get;
        set;
    }

    public Button button;

    public ILRuntime.Runtime.Enviorment.AppDomain AppDomain
    {
        get;
        set;
    }

    public Assembly Assembly
    {
        get;
        set;
    }

    public ExcuteTestClass(string testName, ILRuntime.Runtime.Enviorment.AppDomain appDomain, Transform root, Button button, bool showTime)
    {
        TestName = testName;
        AppDomain = appDomain;
        button = UnityEngine.Object.Instantiate(button);
        button.transform.SetParent(root);
        button.GetComponentInChildren<Text>().text = TestName;
        button.onClick.AddListener(() => Start(showTime));
    }

    public ExcuteTestClass(string testName, Assembly assemly, Transform root, Button button, bool showTime)
    {
        TestName = testName;
        Assembly = assemly;
        button = UnityEngine.Object.Instantiate(button);
        button.transform.SetParent(root);
        button.GetComponentInChildren<Text>().text = TestName;
        button.onClick.AddListener(() => Start(showTime));
    }

    public void Start(bool showTime)
    {
        if (showTime)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            Excute();
            sw.Stop();

            Debug.Log(string.Format(TestName + " CostTime: {0}ms",sw.ElapsedMilliseconds));
            LoggerProvider.Debug.Write(string.Format(TestName + " CostTime: {0}ms", sw.ElapsedMilliseconds));
        }
        else
        {
            Excute();
        }
    }

    public virtual void Excute()
    {
        Debug.Log("Start Test " + TestName + ":");
        LoggerProvider.Debug.Write("Start Test " + TestName + ":");
    }
}

public class NativeTestOne : ExcuteTestClass
{
    public NativeTestOne(string name, ILRuntime.Runtime.Enviorment.AppDomain appDomain, Transform root, Button button, bool showTime) : base(name, appDomain, root, button, showTime)
    {

    }

    public override void Excute()
    {
        base.Excute();
        Vector3[] vArray = new Vector3[100000];
        for (int i = 0; i < 100000; i++)
        {
            vArray[i] = new Vector3(i, i, i);
        }
    }
}

public class ILRunTimeTestOne : ExcuteTestClass
{
    public ILRunTimeTestOne(string name, ILRuntime.Runtime.Enviorment.AppDomain appDomain, Transform root, Button button, bool showTime) : base(name, appDomain, root, button, showTime)
    {

    }

    public override void Excute()
    {
        base.Excute();
        AppDomain.Invoke("GameModelTest.TestClass", "StaticFunTest", null, null);
    }
}

public class ILRuntimeInvocationClass : ExcuteTestClass
{
    public ILRuntimeInvocationClass(string name, ILRuntime.Runtime.Enviorment.AppDomain appDomain, Transform root, Button button, bool showTime) : base(name, appDomain, root, button, showTime)
    {

    }

    public override void Excute()
    {
        base.Excute();

        //调用无参数静态方法，appdomain.Invoke("类名", "方法名", 对象引用, 参数列表);
        AppDomain.Invoke("GameModelTest.InvocationClass", "StaticFunTest", null, null);

        //调用带参数的静态方法
        AppDomain.Invoke("GameModelTest.InvocationClass", "StaticFunTest2", null, 123);

        //预先获得IMethod，可以减低每次调用查找方法耗用的时间
        IType type = AppDomain.LoadedTypes["GameModelTest.InvocationClass"];
        //根据方法名称和参数个数获取方法
        IMethod method = type.GetMethod("StaticFunTest", 0);
        AppDomain.Invoke(method, null, null);

        //指定参数类型来获得IMethod
        IType intType = AppDomain.GetType(typeof(int));
        //参数类型列表
        List<IType> paramList = new List<IType>();
        paramList.Add(intType);
        //根据方法名称和参数类型列表获取方法
        method = type.GetMethod("StaticFunTest2", paramList, null);
        AppDomain.Invoke(method, null, 456);


        //实例化热更里的类
        object obj = AppDomain.Instantiate("GameModelTest.InvocationClass", new object[] { 233, "HelloWorld"});
        //第二种方式
        object obj2 = ((ILType)type).Instantiate();

        //调用成员方法
        AppDomain.Invoke("GameModelTest.InvocationClass", "get_ID", obj, null);
        AppDomain.Invoke("GameModelTest.InvocationClass", "InstanceMethod", obj2, null);

        //调用泛型方法
        IType stringType = AppDomain.GetType(typeof(string));
        IType[] genericArguments = new IType[] { stringType };
        AppDomain.InvokeGenericMethod("GameModelTest.InvocationClass", "GenericMethod", genericArguments, null, "TestString");

        //获取泛型方法的IMethod
        paramList.Clear();
        paramList.Add(intType);
        genericArguments = new IType[] { intType };
        method = type.GetMethod("GenericMethod", paramList, genericArguments);
        AppDomain.Invoke(method, null, 33333);
    }
}

public class ILDelegateClass : ExcuteTestClass
{
    public delegate void TestDelegateMethod(int a);
    public delegate string TestDelegateFunction(int a);

    public static TestDelegateMethod TestMethodDelegate;
    public static TestDelegateFunction TestFunctionDelegate;
    public static Action<string> TestActionDelegate;

    public ILDelegateClass(string name, ILRuntime.Runtime.Enviorment.AppDomain appDomain, Transform root, Button button, bool showTime) : base(name, appDomain, root, button, showTime)
    {

    }

    public override void Excute()
    {
        base.Excute();
        AppDomain.Invoke("GameModelTest.Delegate", "Initialize2", null, null);
        AppDomain.Invoke("GameModelTest.Delegate", "RunTest2", null, null);
        TestMethodDelegate(789);
        var str = TestFunctionDelegate(098);
        Debug.Log("!! OnHotFixLoaded str = " + str);
        TestActionDelegate("Hello From Unity Main Project");
    }
}

public class NativeValueTypeBinderClass : ExcuteTestClass
{
    public NativeValueTypeBinderClass(string name, ILRuntime.Runtime.Enviorment.AppDomain appDomain, Transform root, Button button, bool showTime) : base(name, appDomain, root, button, showTime)
    {

    }

    public override void Excute()
    {
        base.Excute();
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

public class ILRunTimeValueTypeBinderClass : ExcuteTestClass
{
    public ILRunTimeValueTypeBinderClass(string name, ILRuntime.Runtime.Enviorment.AppDomain appDomain, Transform root, Button button, bool showTime) : base(name, appDomain, root, button, showTime)
    {

    }

    public override void Excute()
    {
        base.Excute();
        AppDomain.Invoke("GameModelTest.ValueTypeBinder", "RunTest", null, null);
    }
}

public class ILInheritanceClass : ExcuteTestClass
{
    public ILInheritanceClass(string name, ILRuntime.Runtime.Enviorment.AppDomain appDomain, Transform root, Button button, bool showTime) : base(name, appDomain, root, button, showTime)
    {

    }

    public override void Excute()
    {
        base.Excute();
        //现在我们来注册适配器
        AppDomain.RegisterCrossBindingAdaptor(new TestClassBaseeAdapter());
        TestClassBase obj = AppDomain.Instantiate<TestClassBase>("GameModelTest.Inheritance");
        //现在来调用成员方法
        obj.TestAbstract(123);
        obj.TestVirtual("Hello");

        //现在换个方式创建实例
        obj = AppDomain.Invoke("GameModelTest.Inheritance", "NewObject", null, null) as TestClassBase;
        obj.TestAbstract(456);
        obj.TestVirtual("Hello2");
    }
}

public class ILCLRRedirectionClass : ExcuteTestClass
{
    public ILCLRRedirectionClass(string name, ILRuntime.Runtime.Enviorment.AppDomain appDomain, Transform root, Button button, bool showTime) : base(name, appDomain, root, button, showTime)
    {

    }

    unsafe public override void Excute()
    {
        base.Excute();
        //经过CLR重定向之后可以做到输出DLL内堆栈，接下来进行CLR重定向注册
        var mi = typeof(Debug).GetMethod("Log", new Type[] { typeof(object) });
        //AppDomain.RegisterCLRMethodRedirection(mi, Log_11);
        AppDomain.Invoke("GameModelTest.CLRRedirection", "RunTest", null, null);
    }

    unsafe static StackObject* Log_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
    {
        //ILRuntime的调用约定为被调用者清理堆栈，因此执行这个函数后需要将参数从堆栈清理干净，并把返回值放在栈顶，具体请看ILRuntime实现原理文档
        ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
        StackObject* ptr_of_this_method;
        //这个是最后方法返回后esp栈指针的值，应该返回清理完参数并指向返回值，这里是只需要返回清理完参数的值即可
        StackObject* __ret = ILIntepreter.Minus(__esp, 1);
        //取Log方法的参数，如果有两个参数的话，第一个参数是esp - 2,第二个参数是esp -1, 因为Mono的bug，直接-2值会错误，所以要调用ILIntepreter.Minus
        ptr_of_this_method = ILIntepreter.Minus(__esp, 1);

        //这里是将栈指针上的值转换成object，如果是基础类型可直接通过ptr->Value和ptr->ValueLow访问到值，具体请看ILRuntime实现原理文档
        object message = typeof(object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
        //所有非基础类型都得调用Free来释放托管堆栈
        __intp.Free(ptr_of_this_method);

        //在真实调用Debug.Log前，我们先获取DLL内的堆栈
        var stacktrace = __domain.DebugService.GetStackTrance(__intp);

        //我们在输出信息后面加上DLL堆栈
        UnityEngine.Debug.Log(message + "\n" + stacktrace);

        return __ret;
    }
}

public class ReflectionClass : ExcuteTestClass
{
    public ReflectionClass(string name, ILRuntime.Runtime.Enviorment.AppDomain appDomain, Transform root, Button button, bool showTime) : base(name, appDomain, root, button, showTime)
    {

    }

    public override void Excute()
    {
        base.Excute();

    }
}

public class ILCLRBindingClass : ExcuteTestClass
{
    public ILCLRBindingClass(string name, ILRuntime.Runtime.Enviorment.AppDomain appDomain, Transform root, Button button, bool showTime) : base(name, appDomain, root, button, showTime)
    {

    }

    public override void Excute()
    {
        base.Excute();
        var type = AppDomain.LoadedTypes["GameModelTest.CLRBinding"];
        var m = type.GetMethod("RunTest", 0);
        AppDomain.Invoke(m, null, null);
    }
}


