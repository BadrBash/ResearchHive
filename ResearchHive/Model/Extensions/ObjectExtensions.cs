using System.Collections;

namespace Model.Extensions
{
    public static class ObjectExtensions
    {
        public static IDictionary<string, string> ToDictionary(this object source, IEnumerable<string> propertiesToSelect = null)
        {
            var exclusiveList = propertiesToSelect ?? new List<string>();
            var result = new Dictionary<string, string>();
            Map(exclusiveList, result, source);
            return result;
        }

        static void AddValue(this IDictionary<string, string> obj, IEnumerable<string> propertiesToSelect, string key, object value)
        {
            var type = value.GetType();
            if (propertiesToSelect.Contains(key) || !propertiesToSelect.Any())
            {
                var _value = type.IsEnum ? ((Enum)value).ToString() :
                    type.IsEquivalentTo(typeof(DateTime)) ? DateTime.Parse(value.ToString()).ToString("MMM dd, yyyy") :
                    (type.IsEquivalentTo(typeof(decimal)) || type.IsEquivalentTo(typeof(decimal))) ? ((decimal)value).ToString("N") :
                    value.ToString();

                obj.Add(key, _value);
            }

        }

        private static void Map(IEnumerable<string> exclusiveList, IDictionary<string, string> result, object source, string prefix = null)
        {
            var properties = source.GetType().GetProperties();

            foreach (var x in properties)
            {
                var key = $"{prefix}.{x.Name}".TrimStart('.');
                var value = x.GetValue(source, null);
                if (value != null)
                {
                    var type = value.GetType();
                    if (type.IsValueType || type.IsEquivalentTo(typeof(String)))
                    {
                        if (type.IsGenericType)
                        {
                            Map(exclusiveList, result, value, key);
                        }
                        else
                        {
                            result.AddValue(exclusiveList, key, value);
                        }
                    }
                    else if (typeof(IEnumerable).IsAssignableFrom(type))
                    {
                        var elementType = type.GetElementType();
                        var isGeneric = elementType != null && elementType.IsGenericType;
                        var items = ((IEnumerable)value);

                        var i = 0;
                        foreach (var itm in items)
                        {
                            var iKey = $"{key}[{i}]";
                            if (isGeneric)
                            {
                                Map(exclusiveList, result, itm, iKey);
                            }
                            else
                            {
                                result.AddValue(exclusiveList, iKey, itm);
                            }
                            i++;
                        }
                    }
                    else
                    {
                        Map(exclusiveList, result, value, key);
                    }
                }
            }
        }

        public static void SetProperty(this object obj, string key, object value)
        {
            object InitializeProperty(string _key, object _source)
            {
                var _property = _source.GetType().GetProperty(_key);
                var _value = _property.GetValue(_key, null);
                if (_value == null && _property.PropertyType.IsClass)
                {
                    _value = Activator.CreateInstance(_property.PropertyType);
                    _property.SetValue(_key, _value);
                }

                return _value;
            }

            Stack<string> keyStack = new Stack<string>(key.Split(new char[] { '.' }, StringSplitOptions.None).Reverse());

            var targetKey = keyStack.Pop();
            object targetObject = obj;

            keyStack.Reverse().ToList().ForEach(_key => targetObject = InitializeProperty(_key, targetObject));

            targetObject.GetType().GetProperty(targetKey).SetValue(targetObject, value, null);

        }

        public static IList<T> ToList<T>(this ICollection collection)
        {
            var resp = new List<T>();
            foreach (var item in collection)
            {
                resp.Add((T)(Convert.ChangeType(item, typeof(T))));
            }
            return resp;
        }
    }
}
