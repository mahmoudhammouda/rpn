using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Domain.Enum
{
    public static class EnumConverter<T> where T : struct
    {

        public static string GetDescription(T value)
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

            return value.ToString(); // Retourne le nom de l'enum si aucune description n'est trouvée
        }
        public static T ConvertFromString(string value)
        {
            if (System.Enum.TryParse(value, out T result))
            {
                return result;
            }

            throw new ArgumentException($"Not able to convert enum {typeof(T).Name}: {value}");
        }

        public static bool TryConvertFromString(string value, out T? result)
        {
            if (System.Enum.TryParse(value, out T val))
            {
                result=val;
                return true;
            }
            else
            {
                result= default(T);
                return false;
            }
            
        }


        public static T? ConvertStringToNullableEnum(string value) 
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            if (System.Enum.TryParse<T>(value, out var result))
            {
                return result;
            }

            return null;
        }

        public static string ConvertToString<T>(T enumValue) 
        {
            return enumValue.ToString();
        }


    }
}
