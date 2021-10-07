using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Util
{
    public static class TypeNameExtensions
    {
        public static string GetGenericTypeName(this Type type)
        {
            string genericName = type.Name;
            if (type.IsGenericType)
            {
                int iBacktick = genericName.IndexOf('`');
                if (iBacktick > 0)
                {
                    genericName = genericName.Remove(iBacktick);
                }
                genericName += "<";
                Type[] typeParameters = type.GetGenericArguments();
                for (int i = 0; i < typeParameters.Length; ++i)
                {
                    string typeParamName = GetGenericTypeName(typeParameters[i]);
                    genericName += (i == 0 ? typeParamName : "," + typeParamName);
                }
                genericName += ">";
            }

            return genericName;
        }
    }
}
