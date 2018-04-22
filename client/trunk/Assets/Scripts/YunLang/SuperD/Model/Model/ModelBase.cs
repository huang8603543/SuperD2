using YunLang.SuperD.Data;
using UnityEngine;

namespace YunLang.SuperD.Model
{
    /// <summary>
    /// 模型基类///
    /// </summary>
    public abstract class ModelBase
    {
        public GameObject GameObject
        {
            get;
            private set;
        }

        public Transform Transform
        {
            get
            {
                return GameObject.transform;
            }
        }

        public ModelData Data
        {
            get;
            private set;
        }

        public ModelBase(ModelData data)
        {
            Data = data;
        }
    }
}
