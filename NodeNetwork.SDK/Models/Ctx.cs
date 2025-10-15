using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetwork.SDK.Models
{
    public class Ctx : IContext
    {
        private readonly Dictionary<string, object?> _map = new();
        private object? _result;

        public static IContext Of(params (string key, object value)[] pairs)
        {
            var c = new Ctx();
            foreach (var (k, v) in pairs) 
                c._map[k] = v;
            return c;
        }

        // key, value setter.
        public IContext Set<T>(string key, T value) { _map[key] = value; return this; }

        // key, value 값이 있는 지 validate
        public bool TryGet<T>(string key, out T value)
        {
            if (_map.TryGetValue(key, out var v) && v is T t) { value = t; return true; }
            value = default!; return false;
        }


        public T Get<T>(string key) =>
            TryGet<T>(key, out var v) ? v : throw new InvalidOperationException($"missing '{key}'");
        public IContext SetResult<T>(T value) { _result = value; return this; }
        public bool TryGetResult<T>(out T value)
        {
            if (_result is T t) { value = t; return true; }
            value = default!; return false;
        }

        public bool TryGet(string key, out object? value)
        {
            if (_map.TryGetValue(key, out var v)) { value = v; return true; }
            value = null;
            return false;
        }

    }
}
