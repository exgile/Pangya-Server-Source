using PangyaAPI.Json.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
namespace PangyaAPI.Json
{
    public class JsonArray : DynamicObject, IEnumerable, IJson
    {
        private readonly List<IJson> _collection;

        public JsonArray(ICollection<object> collection)
        {
            _collection = new List<IJson>(collection.Count);
            foreach (var instance in collection.Cast<IDictionary<string, object>>())
            {
                _collection.Add(new JsonObject(instance));
            }
        }

        public IEnumerator GetEnumerator()
        {
            return _collection.GetEnumerator();
        }
    }
}
