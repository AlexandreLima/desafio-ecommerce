using System;

namespace Catalogo.Domain.Extensoes
{
    public static class ConversionExtensions
    {
        public static object Convert(this object value, Type t)
        {
            Type underlyingType = Nullable.GetUnderlyingType(t);

            if (underlyingType != null && value == null)
            {
                return null;
            }
            Type basetype = underlyingType == null ? t : underlyingType;
            return System.Convert.ChangeType(value, basetype);
        }

        public static T Convert<T>(this object value)
        {
            if (typeof(T) == typeof(bool) && value.GetType() == typeof(string))
            {
                int booleanValue = int.Parse((string)value);

                return (T)booleanValue.Convert(typeof(T));
            }

            return (T)value.Convert(typeof(T));
        }
    }
}
