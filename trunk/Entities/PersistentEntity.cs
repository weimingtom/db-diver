using System;
using System.Collections.Generic;
using System.Text;

namespace DB.DoF.Entities
{
    public abstract class PersistentEntity : Entity
    {
        public string GetConstructorString()
        {
            StringBuilder sb = new StringBuilder(this.GetType().Name + "(");
            string[] args = GetConstructorArguments();
            for (int i = 0; i < args.Length; i++)
            {
                if (i != 0)
                {
                    sb.Append(", ");
                }

                sb.Append(args[i]);
            }
            sb.Append(")");
            return sb.ToString();
        }

        protected abstract string[] GetConstructorArguments();

        protected static string Quote(string s)
        {
            return '"' + s + '"';
        }
    }
}
