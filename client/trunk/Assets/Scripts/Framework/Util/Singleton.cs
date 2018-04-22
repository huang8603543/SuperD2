using UnityEngine;

namespace Framework.Util
{
    public class Singleton<T> where T : class, new()
    {
        protected static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }

        public static T GetInstance()
        {
            return Instance;
        }
    }

    public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<T>();
                    obj.name = _instance.GetType().Name;
                }
                return _instance;
            }
        }

        public static T GetInstance()
        {
            return Instance;
        }

        public static void DestoryInstance()
        {
            if (_instance == null)
                return;
            GameObject obj = _instance.gameObject;
            //ResourceMgr.Instance.DestroyObject(obj);
        }
    }
}
