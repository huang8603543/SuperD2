using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class UniRXTestOne : ExcuteTestClass
{
    public UniRXTestOne(string name, ILRuntime.Runtime.Enviorment.AppDomain appDomain, Transform root, Button button, bool showTime) : base(name, appDomain, root, button, showTime)
    {

    }

    public override void Excute()
    {
        base.Excute();
        AppDomain.Invoke("GameModelTest.UniRXTest", "TestOne", null, null);
    }
}

public class UniRXTestTwo : ExcuteTestClass
{
    public UniRXTestTwo(string name, ILRuntime.Runtime.Enviorment.AppDomain appDomain, Transform root, Button button, bool showTime) : base(name, appDomain, root, button, showTime)
    {

    }

    public override void Excute()
    {
        base.Excute();
        AppDomain.Invoke("GameModelTest.UniRXTest", "TestTwo", null, null);
    }
}

public static class GenericTest
{
    public static IObservable<T> Test<T>(this IObservable<T> source, Action<T> onNext)
    {
        return Observable.Empty<T>();
    }
}

