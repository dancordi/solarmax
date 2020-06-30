using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SolarMaxClient.Protocol
{
    internal static class Helper
    {
        public static string GetValueAsString(this SolarMaxCommandEnum scEnum)
        {
            // get the field 
            var field = scEnum.GetType().GetField(scEnum.ToString());
            if (field == null)
            {
                return scEnum.ToString();
            }
            var customAttributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (customAttributes == null || customAttributes.Length == 0)
            {
                return scEnum.ToString();
            }
            return (customAttributes[0] as DescriptionAttribute).Description;
        }

        public static T GetEnumValueFromDescription<T>(this string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", nameof(description));
            // or return default(T);
        }

        public static string ToHexString(this int i)
        {
            return i.ToString("X2").ToUpper();
        }

        public static int CalculateChecksum(string s)
        {
            return CalculateChecksum(s, Encoding.UTF8);
        }

        public static int CalculateChecksum(string s, Encoding encoding)
        {
            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }
            byte[] bytes = encoding.GetBytes(s);
            int crc = 0;
            foreach (var b in bytes)
            {
                crc += (int)b;
            }
            return crc;
        }
    }
}
