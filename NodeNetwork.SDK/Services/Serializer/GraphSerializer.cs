using NodeNetwork.SDK.Models;
using NodeNetworkSDK.Models.Dtos;
using NodeNetworkSDK.Models.Nodes;
using NodeNetworkSDK.Models.Values;
using NodeNetworkSDK.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Services.Serializer
{
    public class GraphSerializer
    {
        private readonly GraphManager _gm;
        private readonly ValueCodecRegistry _codecs;
        public int SchemaVersion => 1;

        private readonly JsonSerializerOptions _json = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        // 직렬화
        public GraphSerializer(GraphManager gm, ValueCodecRegistry codecs)
        { _gm = gm; _codecs = codecs; }

        public string Serialize(GraphId gid)
        {
            var g = _gm.Debug_GetGraphSnapshot(gid); 
            var dto = new GraphDto { Name = g.Name, SchemaVersion = SchemaVersion };

            // Node
            foreach (var (id, n) in g.Nodes)
                dto.Nodes.Add(new NodeDto { Id = id, TypeName = n.GetType().FullName!, Name = n.Name });

            // Edge
            foreach (var e in g.Edges)
                dto.Edges.Add(new EdgeDto { From = e.From, Out = e.Out, To = e.To, In = e.In });

            // Literal
            foreach (var kv in g.Literals)
            {
                var (nodeId, input) = kv.Key;
                var val = kv.Value;
                if (!_codecs.TryGet(val.Type.Id, out var codec) || codec is null)
                    throw new InvalidOperationException($"No codec for type '{val.Type.Id}'");
                dto.Literals.Add(new LiteralDto
                {
                    Node = nodeId,
                    Input = input,
                    TypeId = val.Type.Id,
                    Payload = codec.ToPayload(val)
                });
            }
            return JsonSerializer.Serialize(dto, _json);
        }

        // 역직렬화
        public GraphId Deserialize(string json)
        {
            var dto = JsonSerializer.Deserialize<GraphDto>(json, _json)
                      ?? throw new InvalidOperationException("Invalid json");
            if (dto.SchemaVersion != SchemaVersion)
                throw new NotSupportedException($"Schema {dto.SchemaVersion} != {SchemaVersion}");

            var gid = _gm.CreateGraph(dto.Name);


            var idMapOldToNew = new Dictionary<Guid, Guid>();

            // 새로운 GUID 를 발급해준다.
            foreach (var nd in dto.Nodes)
            {
                var nh = _gm.AddNode(gid, nd.TypeName, nd.Name);
                idMapOldToNew[nd.Id] = nh.Value;
            }


            foreach (var e in dto.Edges)
            {
                var fromNew = new NodeHandle(idMapOldToNew[e.From]);
                var toNew = new NodeHandle(idMapOldToNew[e.To]);
                _gm.Connect(gid, fromNew, e.Out, toNew, e.In);
            }


            var ctx = new Context();
            foreach (var lit in dto.Literals)
            {
                if (!_codecs.TryGet(lit.TypeId, out var codec) || codec is null)
                    throw new InvalidOperationException($"No codec for type '{lit.TypeId}'");

                var val = codec.FromPayload(lit.Payload!);
                var nodeNew = new NodeHandle(idMapOldToNew[lit.Node]);
                _gm.SetInput(ctx, nodeNew, lit.Input, val, gid);
            }

            return gid;
        }
    }
}
