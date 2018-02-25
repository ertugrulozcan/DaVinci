using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Shared.Helpers
{
    public static class ReflectionHelper
    {
        public class PropertySource
        {
            public PropertyInfo Property { get; private set; }
            public object Source { get; private set; }

            public PropertySource(PropertyInfo prop, object src)
            {
                this.Property = prop;
                this.Source = src;
            }
        }

        public static PropertySource GetProp(this object src, string fullPropName)
        {
            try
            {
                int seperatorIndex = fullPropName.IndexOf('.');
                if (seperatorIndex >= fullPropName.Length)
                    throw new Exception("ReflectionHelper : Syntax hatası!");

                if (seperatorIndex >= 0)
                {
                    string propName = fullPropName.Substring(0, seperatorIndex);
                    var prop = src.GetType().GetProperty(propName);
                    if (prop != null)
                    {
                        return prop.GetValue(src, null).GetProp(fullPropName.Substring(seperatorIndex + 1));
                    }
                    else
                        System.Diagnostics.Debug.WriteLine(string.Format("ReflectionHelper : {0} icerisinde {1} isminde bir property yok!", src.GetType().Name, propName));
                }

                var property = src.GetType().GetProperty(fullPropName);
                if (property != null)
                {
                    return new PropertySource(property, src);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("ReflectionHelper : {0} icerisinde {1} isminde bir property yok!", src.GetType().Name, fullPropName));
                    return null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("ReflectionHelper : {0}", ex.Message));
                throw;
            }
        }

        public static void ExecuteMethod(this object src, string methodName)
        {
            ExecuteMethod(src, methodName, null);
        }

        public static void ExecuteMethod(this object src, string methodName, object[] parameters)
        {
            try
            {
                int seperatorIndex = methodName.IndexOf('.');
                if (seperatorIndex >= methodName.Length)
                    throw new Exception("ReflectionHelper : Syntax hatası!");

                if (seperatorIndex >= 0)
                {
                    string propName = methodName.Substring(0, seperatorIndex);
                    var prop = src.GetType().GetProperty(propName);
                    if (prop != null)
                    {
                        prop.GetValue(src, null).ExecuteMethod(methodName.Substring(seperatorIndex + 1));
                        return;
                    }
                    else
                        System.Diagnostics.Debug.WriteLine(string.Format("ReflectionHelper : {0} icerisinde {1} isminde bir property yok!", src.GetType().Name, propName));
                }

                MethodInfo method = src.GetType().GetMethod(methodName);
                if (method != null)
                {
                    method.Invoke(src, ConvertParametersType(parameters));
                }
                else
                    System.Diagnostics.Debug.WriteLine(string.Format("ReflectionHelper : {0} icerisinde {1} isminde bir method yok!", src.GetType().Name, methodName));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("ReflectionHelper : {0}", ex.Message));
                throw;
            }
        }

        public static bool HasProperty(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }

        public static bool HasMethod(this object obj, string methodName)
        {
            return obj.GetType().GetMethod(methodName) != null;
        }

        public static object GetPropValue(this object src, string propName)
        {
            var prop = src.GetProp(propName);
            if (prop != null)
            {
                try
                {
                    return prop.Property.GetValue(prop.Source);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(string.Format("ReflectionHelper GetPropValue() : {0} icerisinde {1} isminde bir property yok!", src.GetType().Name, propName));
                return null;
            }
        }

        public static void SetPropValue(this object target, string propName, object value)
        {
            PropertySource propSource = target.GetProp(propName);
            PropertyInfo propertyInfo = propSource.Property;
            object source = propSource.Source;

            if (propertyInfo != null && propertyInfo.CanWrite)
            {

                propertyInfo.SetValue(source, ConvertValueType(propertyInfo, value));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(string.Format("ReflectionHelper SetPropValue() : {0} icerisinde {1} isminde bir property yok!", target.GetType().Name, propName));
            }
        }

        private static object[] ConvertParametersType(object[] parameters)
        {
            if (parameters == null)
                return new object[] { };

            List<object> newParams = new List<object>();
            foreach (var param in parameters)
            {
                newParams.Add(ConvertValueType(param));
            }

            return newParams.ToArray();
        }

        private static object ConvertValueType(PropertyInfo propertyInfo, object value)
        {
            if (propertyInfo.PropertyType == typeof(string))
            {
                return value.ToString();
            }
            else if (propertyInfo.PropertyType == typeof(bool))
            {
                bool boolean;
                bool isParsed = Boolean.TryParse(value.ToString(), out boolean);
                if (isParsed)
                    return boolean;
                else
                    throw new Exception("BooleanParsingException");
            }
            else if (IsNumericType(propertyInfo.PropertyType))
            {
                double number;
                bool isParsed = Double.TryParse(value.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out number);
                if (isParsed)
                    return number;
                else
                    throw new Exception("DoubleParsingException");
            }
            else
                throw new Exception("Setter property type is must be primitive.");
        }

        private static object ConvertValueType(object value)
        {
            bool boolean;
            bool isParsedToBoolean = Boolean.TryParse(value.ToString(), out boolean);
            if (isParsedToBoolean)
                return boolean;

            double number;
            bool isParsedToDouble = Double.TryParse(value.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out number);
            if (isParsedToDouble)
                return number;

            return value.ToString();
        }

        public static bool IsNumericType(object o)
        {
            return IsNumericType(o.GetType());
        }

        public static bool IsNumericType(System.Type type)
        {
            switch (System.Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}
