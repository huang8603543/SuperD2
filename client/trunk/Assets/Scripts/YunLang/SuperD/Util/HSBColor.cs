using System;
using UnityEngine;

namespace YunLang.SuperD.Util
{
    [Serializable]
    public class HSBColor
    {
        public float A;
        public float B;
        public float H;
        public float S;

        public HSBColor()
        {
        }

        public HSBColor(float h, float s, float b)
        {
            this.H = h;
            this.S = s;
            this.B = b;
            this.A = 1f;
        }

        public HSBColor(float h, float s, float b, float a)
        {
            this.H = h;
            this.S = s;
            this.B = b;
            this.A = a;
        }

        public HSBColor Clone()
        {
            return new HSBColor(this.H, this.S, this.B, this.A);
        }

        public bool compare(HSBColor other)
        {
            return ((Mathf.Approximately(this.H, other.H) && Mathf.Approximately(this.S, other.S)) && Mathf.Approximately(this.B, other.B));
        }

        public static HSBColor RGBToHSB(Color color)
        {
            float num;
            float num2;
            float num3;
            Color.RGBToHSV(color, out num, out num2, out num3);
            return new HSBColor(num, num2, num3, color.a);
        }

        public Color ToColor()
        {
            Color color = Color.HSVToRGB(this.H, this.S, this.B);
            color.a = this.A;
            return color;
        }

        public override string ToString()
        {
            object[] objArray1 = new object[] { "H:", this.H, " S:", this.S, " B:", this.B };
            return string.Concat(objArray1);
        }

        public static HSBColor white
        {
            get
            {
                return new HSBColor(0f, 0f, 1f, 1f);
            }
        }

        public string ToJsonString()
        {
            return JsonUtility.ToJson(this);
        }
    }
}

