using NodeNetwork.SDK.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NodeNetworkSDK.Models.Nodes
{
    // page 를 가질 수 있다.
    public class PageNode : INode
    {
        public string Name { get; }
        private IPage _parent;
        private readonly IPage _child;
        private readonly string _outKey;


        // child.ResultKey에서 읽어 부모의 outKey로 복사
        public PageNode(IPage parent, string name, IPage child, string outKey)
        {
            _parent = parent;
            Name = name;
            _child = child;
            _outKey = outKey;
        }

        public IContext Exec(IContext ctx)
        {
            ctx = _child.Exec(ctx);
            // 자식 페이지의 결과를 부모 outKey로 복사
            var v = ctx.Get<object>(_child.ResultKey);
            return ctx.Set(_outKey, v);

            
        }

        public INode CloneWithKeyRemap(Func<string, string> remap)
        {
            return new PageNode(_parent, Name, _child, _outKey);
            
        }
    }
    /*
    public sealed class PageNode : INode
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; }
        private readonly Page _child;
        private readonly string _outputKey; // 부모 컨텍스트에 쓸 키(지정 없으면 child.ResultKey 사용)
        private readonly Dictionary<string, string> _inputBindings;
        //  key = childInputKey, value = parentKey

        public PageNode(string name, Page child, string? outputKey = null,
                        Dictionary<string, string>? inputBindings = null)
        {
            Name = name;
            _child = child ?? throw new ArgumentNullException(nameof(child));
            _outputKey = outputKey ?? child.ResultKey;
            _inputBindings = inputBindings ?? new Dictionary<string, string>();
        }

        public GraphValidationResult Validate()
        {
            // 최소한의 검증: child 존재/ResultKey 유효성
            if (string.IsNullOrWhiteSpace(_child.ResultKey))
                return GraphValidationResult.Fail($"Child page '{_child.Name}' has no ResultKey.");
            return GraphValidationResult.Ok();
        }

        public IContext Exec(IContext parentCtx)
        {
            var childCtx = Ctx.Of();

            if (_inputBindings.Count > 0)
            {
                foreach (var (childKey, parentKey) in _inputBindings)
                {
                    if (parentCtx.TryGet(parentKey, out var val))
                        childCtx = childCtx.Set(childKey, val);
                    else
                        throw new InvalidOperationException(
                            $"Parent context missing key '{parentKey}' for child input '{childKey}'.");
                }
            }
            childCtx = _child.Exec(childCtx);

            if (!childCtx.TryGet(_child.ResultKey, out var res))
                throw new InvalidOperationException(
                    $"Child page '{_child.Name}' did not produce result for key '{_child.ResultKey}'.");

            return parentCtx.Set(_outputKey, res);
        }
        public INode CloneWithKeyRemap(Func<string, string> remap)
        {
            return new PageNode(Name, _child, _outputKey);

        }
    }*/
}
