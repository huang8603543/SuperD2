using UnityEngine;
using UnityEngine.UI;

public class MVVMTestOne : ExcuteTestClass
{
    public MVVMTestOne(string name, ILRuntime.Runtime.Enviorment.AppDomain appDomain, Transform root, Button button, bool showTime) : base(name, appDomain, root, button, showTime)
    {

    }

    public override void Excute()
    {
        base.Excute();
        AppDomain.Invoke("GameModelTest.MVVMTest", "TestOne", null, null);
    }
}

public class MVVMTestTwo : ExcuteTestClass
{
    public MVVMTestTwo(string name, ILRuntime.Runtime.Enviorment.AppDomain appDomain, Transform root, Button button, bool showTime) : base(name, appDomain, root, button, showTime)
    {

    }

    public override void Excute()
    {
        base.Excute();
        AppDomain.Invoke("GameModelTest.MVVMTest", "TestTwo", null, null);
    }
}

public class MVVMPanelTestOne : ExcuteTestClass
{
    public MVVMPanelTestOne(string name, ILRuntime.Runtime.Enviorment.AppDomain appDomain, Transform root, Button button, bool showTime) : base(name, appDomain, root, button, showTime)
    {

    }

    public override void Excute()
    {
        base.Excute();
        AppDomain.Invoke("GameModelTest.MVVMTest", "TestThree", null, null);
    }
}

