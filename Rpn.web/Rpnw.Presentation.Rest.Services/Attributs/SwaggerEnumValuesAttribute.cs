using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Presentation.Rest.Services.Attributs
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SwaggerEnumValuesAttribute:  Attribute
    {
        public Type EnumType { get; }
        public SwaggerEnumValuesAttribute(Type enumType)
        {

            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type must be an enum", nameof(enumType));
            }
            EnumType = enumType;
        }
    }
}
