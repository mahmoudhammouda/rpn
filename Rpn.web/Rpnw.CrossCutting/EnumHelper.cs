using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.CrossCutting
{
    public static class EnumHelper
    {
        public static string GetDescription(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            if (fieldInfo != null)
            {
                var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    return attributes[0].Description;
                }
            }
            // Retourner le nom de l'enum si aucune description n'est trouvée
            return value.ToString(); 
        }
    }
}
