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

        public IContext Set<T>(string key, T value) { _map[key] = value; return this; }
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
        /*readonly List<IReadOnlyDictionary<string, object?>> _layers;
        object? _result;

        Ctx(List<IReadOnlyDictionary<string, object?>> layers,  object? result) { _layers = layers; _result = result; }

        public static Ctx Empty()
        {
            return new Ctx(new(), null);
        }

        // 초기 레이어, 가변 인자를 받는다. 튜플을 여러 개 받을 수 있도록 한다.
        public static Ctx Of(params (string key, object? value)[]pairs)
            // StringComparer 클래스는 문자열 비교 방법을 정의한 추상화 객체이다.
            => new(new() { pairs.ToDictionary(x => x.key, x => x.value, StringComparer.Ordinal) }, null);

        public bool TryGet<T>(string key, out T value)
        {
            for(int i = _layers.Count-1; i>=0; i--)
            {
                if (_layers[i].TryGetValue(key, out var raw))
                {
                    if(raw is T t) { value = t; return true; }
                    try
                    {
                        var target = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
                        if (raw is IConvertible)
                        { value = (T)Convert.ChangeType(raw, target); return true; }
                        if (raw is string s)
                        {
                            if (target == typeof(double) && double.TryParse(s, out var d))
                            { value = (T)(object)d; return true; }
                            if (target == typeof(int) && int.TryParse(s, out var n))
                            { value = (T)(object)n; return true; }
                        }
                    }
                    catch { }
                }
            }
            // null 허용 경고 억제 + 기본값 할당
            value = default!;
            return false;
        }

        // 불변 오버레이 -> 항상 새 context 반환으로 안전하게 합성
        public IContext With(string key, object? value)
        {
            var dict = new Dictionary<string, object?>(StringComparer.Ordinal) { [key] = value };
            var layers = new List<IReadOnlyDictionary<string, object?>>(_layers) { dict };
            return new Ctx(layers, _result);
        }
        public void SetResult(object? value) => _result = value;
        public bool TryGetResult<T>(out T value) { if (_result is T t) { value = t; return true; } value = default!; return false; }*/
    }
}
