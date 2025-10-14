using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetwork.SDK.Models
{
    // data / 실행 상태
    public interface IContext
    {
        // out 키워드를 사용한 매개변수는 무조건 함수 내부에서 값을 세팅해줘야한다.
        bool TryGet<T>(string key, out T value);

        // 현재 context 를 변경하지 않고 새 레이어로 덧씌운 새 context를 반환.
        IContext With(string key, object? value);        
        bool TryGetResult<T>(out T value);

        T Get<T>(string a);
        IContext Set<T>(string key, T value);
        IContext SpawnScoped();             
        IContext SetResult<T>(T value);
    }
}
