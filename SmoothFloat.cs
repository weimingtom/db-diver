using System;
using System.Collections.Generic;
using System.Text;

namespace DB.DoF
{
    class SmoothFloat
    {
        float value;
        public float Value { get { return value; } }

        public float Target;

        public float Diff { get { return Target - value; } }

        float divisor;

        public SmoothFloat(float value, float divisor)
        {
            this.value = value;
            this.divisor = divisor;
        }

        public void Update()
        {
            this.value += Diff/divisor;
        }
    }
}
