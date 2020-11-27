using PangyaAPI.Json.Interface;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
namespace PangyaAPI.Json
{
    public class JsonObject : DynamicObject, IJson
    {
        private readonly IDictionary<string, object> _hash = new Dictionary<string, object>();

        public JsonObject(IDictionary<string, object> hash)
        {
            _hash = hash;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var name = Underscored(binder.Name);
            _hash[name] = value;
            return _hash[name] == value;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var name = Underscored(binder.Name);
            return YieldMember(name, out result);
        }

        private bool YieldMember(string name, out object result)
        {
            if (_hash.ContainsKey(name))
            {
                result = _hash[name];

                if (result is IDictionary<string, object>)
                {
                    result = new JsonObject((IDictionary<string, object>)result);
                    return true;
                }

                return _hash[name] == result;
            }
            result = null;
            return false;
        }

        private static string Underscored(IEnumerable<char> pascalCase)
        {
            var sb = new StringBuilder();
            var i = 0;
            foreach (var c in pascalCase)
            {
                if (char.IsUpper(c) && i > 0)
                {
                    sb.Append("_");
                }
                sb.Append(c);
                i++;
            }
            return sb.ToString().ToLowerInvariant();
        }
    }
}