using NodeNetwork.SDK.Models;
using NodeNetworkSDK.Models.Serializer;
using NodeNetworkSDK.Models.Values;
using NodeNetworkSDK.Services;
using System;
using System.Net.NetworkInformation;
using System.Runtime.ConstrainedExecution;

public class Program
{
    static void Main()
    {
        Console.WriteLine("Node Network 시작\n");


        var gm = new GraphManager();
        var ctx = new Context();
        var codecs = new ValueCodecRegistry();
        codecs.Register(new NumberCodec());
        codecs.Register(new BoolCodec());

        var ser = new GraphSerializer(gm, codecs);

        var subG = gm.CreateGraph("SubCalc");


        var sSum = gm.AddNode(subG, "SumNode", "Sub.Sum");
        var sSub = gm.AddNode(subG, "SubNode", "Sub.Sub");
        var sIf = gm.AddNode(subG, "IfNode", "Sub.If");


        gm.SetInput(ctx, sSum, "a", NumberValue.Of(3), subG);
        gm.SetInput(ctx, sSum, "b", NumberValue.Of(4), subG);


        gm.SetInput(ctx, sSub, "a", NumberValue.Of(20), subG);
        gm.SetInput(ctx, sSub, "b", NumberValue.Of(5), subG);

        gm.SetInput(ctx, sIf, "cond", BoolValue.True, subG);

        gm.Connect(subG, sSum, "out", sIf, "then");
        gm.Connect(subG, sSub, "out", sIf, "else");


        gm.Execute(subG, ctx);


        if (ctx.TryGetOutput(sIf, "out", out var subOut) && subOut is NumberValue subNv)
            Console.WriteLine($"[SubGraph] If.out = {subNv}"); // 기대: 7 (then 선택)


        Console.WriteLine();


        var mainG = gm.CreateGraph("MainCalc");


        var mSum = gm.AddNode(mainG, "SumNode", "Main.Sum");
        var mSub = gm.AddNode(mainG, "SubNode", "Main.Sub");
        var mDiv = gm.AddNode(mainG, "DivNode", "Main.Div");
        var mIf = gm.AddNode(mainG, "IfNode", "Main.If");

        gm.SetInput(ctx, mSum, "a", NumberValue.Of(10), mainG);
        gm.SetInput(ctx, mSum, "b", NumberValue.Of(7), mainG);
        gm.SetInput(ctx, mSub, "a", NumberValue.Of(10), mainG);
        gm.SetInput(ctx, mSub, "b", NumberValue.Of(7), mainG);


        gm.SetInput(ctx, mIf, "cond", BoolValue.True, mainG);


        gm.Connect(mainG, mSum, "out", mIf, "then");
        gm.Connect(mainG, mSub, "out", mIf, "else");


        gm.Connect(mainG, mIf, "out", mDiv, "a");
        gm.Connect(mainG, mSub, "out", mDiv, "b");


        gm.Execute(mainG, ctx);


        if (ctx.TryGetOutput(mDiv, "out", out var v) && v is NumberValue nv)
            Console.WriteLine($"[MainGraph] Div.out = {nv}");


        if (ctx.TryGetOutput(mIf, "out", out var ifOut) && ifOut is NumberValue ifNv)
            Console.WriteLine($"[MainGraph] If.out = {ifNv}"); // 기대: 17 (cond=true)

        string json = ser.Serialize(mainG);
        Console.WriteLine(json);

        var g2 = ser.Deserialize(json);
        gm.Execute(g2, new Context());

        Console.WriteLine("\n끝.");
    }
}
///////////////////////////// Subgraph
/*
 * Console.WriteLine("Node Network 시작\n");


        var gm = new GraphManager();
        var ctx = new Context();
        var codecs = new ValueCodecRegistry();
        codecs.Register(new NumberCodec());
        codecs.Register(new BoolCodec());

        var ser = new GraphSerializer(gm, codecs);

        var subG = gm.CreateGraph("SubCalc");


        var sSum = gm.AddNode(subG, "SumNode", "Sub.Sum");
        var sSub = gm.AddNode(subG, "SubNode", "Sub.Sub");
        var sIf = gm.AddNode(subG, "IfNode", "Sub.If");


        gm.SetInput(ctx, sSum, "a", NumberValue.Of(3), subG);
        gm.SetInput(ctx, sSum, "b", NumberValue.Of(4), subG); 


        gm.SetInput(ctx, sSub, "a", NumberValue.Of(20), subG);
        gm.SetInput(ctx, sSub, "b", NumberValue.Of(5), subG); 

        gm.SetInput(ctx, sIf, "cond", BoolValue.True, subG);

        gm.Connect(subG, sSum, "out", sIf, "then");
        gm.Connect(subG, sSub, "out", sIf, "else");


        gm.Execute(subG, ctx);


        if (ctx.TryGetOutput(sIf, "out", out var subOut) && subOut is NumberValue subNv)
            Console.WriteLine($"[SubGraph] If.out = {subNv}"); // 기대: 7 (then 선택)


        Console.WriteLine();


        var mainG = gm.CreateGraph("MainCalc");


        var mSum = gm.AddNode(mainG, "SumNode", "Main.Sum");
        var mSub = gm.AddNode(mainG, "SubNode", "Main.Sub");
        var mDiv = gm.AddNode(mainG, "DivNode", "Main.Div");
        var mIf = gm.AddNode(mainG, "IfNode", "Main.If");

        gm.SetInput(ctx, mSum, "a", NumberValue.Of(10), mainG);
        gm.SetInput(ctx, mSum, "b", NumberValue.Of(7), mainG);
        gm.SetInput(ctx, mSub, "a", NumberValue.Of(10), mainG);
        gm.SetInput(ctx, mSub, "b", NumberValue.Of(7), mainG);


        gm.SetInput(ctx, mIf, "cond", BoolValue.True, mainG);


        gm.Connect(mainG, mSum, "out", mIf, "then");
        gm.Connect(mainG, mSub, "out", mIf, "else"); 


        gm.Connect(mainG, mIf, "out", mDiv, "a");
        gm.Connect(mainG, mSub, "out", mDiv, "b");


        gm.Execute(mainG, ctx);


        if (ctx.TryGetOutput(mDiv, "out", out var v) && v is NumberValue nv)
            Console.WriteLine($"[MainGraph] Div.out = {nv}");


        if (ctx.TryGetOutput(mIf, "out", out var ifOut) && ifOut is NumberValue ifNv)
            Console.WriteLine($"[MainGraph] If.out = {ifNv}"); // 기대: 17 (cond=true)

        string json = ser.Serialize(mainG);
        Console.WriteLine(json);

        var g2 = ser.Deserialize(json);
        gm.Execute(g2, new Context());

        Console.WriteLine("\n끝.");
 */


///////////////////////////// If 사용 예제
/*
 * var page = new Page("Demo1", resultKey: "result");
        var m    = new NodeManager();

        var ctx = Ctx.Of();

        // Ctx.Of(("a", 10.0), ("b", 7.0)); 대신 input 값을 넣는다.
        page.AddNode(new InputNode("LoadA", "a", () => 10.0));
        page.AddNode(new InputNode("LoadB", "b", () => 16.0));

        // AddNode("name", "inputA", "inputB", "outKey");
        // sum = a + b
        page.AddNode(m.Operation("Add", "a", "b", "sum", Operation.Add));
        // diff = a - b
        page.AddNode(m.Operation("Sub", "a", "b", "diff", Operation.Sub));
        // cond = sum >= diff ? true : false
        page.AddNode(m.Compare("AGtB", "sum", "diff", "cond", Comparator.Gt));
        // result = cond ? sum : diff
        page.AddNode(m.If("Pick", "cond", "sum", "diff", "result"));

        ctx = page.Exec(ctx);
        if(ctx.TryGetResult<double>(out var res))
            Console.WriteLine($"result = {res}"); 
 */

//////////////////////// 서브 그래프 예제
/*
        Console.WriteLine("Node Network 시작");
        var m = new NodeManager();

        // 서브 그래프
        var child = new Page("ChildSum", resultKey: "sum");

        child.AddNode(new InputNode("LoadA", "a", () => 10.0));
        child.AddNode(new InputNode("LoadB", "b", () => 7.0));

        child.AddNode(m.Operation("Add", "a", "b", "sum", Operation.Add));

        var ctx1 = Ctx.Of();
        ctx1 = child.Exec(ctx1);

        if (ctx1.TryGetResult<double>(out var res1))
            Console.WriteLine($"a+b = {res1}");

        // 메인 그래프
        var parent = new Page("Parent", resultKey: "final");

        // child 의 resultKey 를 부모의 out 키로 복사.
        parent.AddNode(new PageNode("ChildNode", child, "sum"));

        parent.AddNode(new InputNode("LoadTwo", "two", () => 2.0));
        parent.AddNode(m.Operation("Times2", "sum", "two", "t2", Operation.Mul));
        parent.AddNode(m.Operation("Sum4", "t2", "a", "final", Operation.Add));

        var ctx = Ctx.Of();
        ctx = parent.Exec(ctx);

        if (ctx.TryGetResult<double>(out var res))
            Console.WriteLine($"(a+b) x 2 = {res}");
 */

//////////////////////// If node + SubGraph 예제
/*
 * 
 * Console.WriteLine("Node Network 시작");
        var m = new NodeManager();

        // 서브 그래프
        var child = new Page("ChildSum", resultKey: "childResult1");

        child.AddNode(new InputNode("C", "c", () => 5.0));
        child.AddNode(new InputNode("D", "d", () => 10.0));
        child.AddNode(m.Operation("sum", "c", "d", "childResult1", Operation.Add));
        child.AddNode(m.Operation("sub", "c", "d", "childResult2", Operation.Sub));

        // 메인 그래프
        var parent = new Page("Parent", resultKey: "final");
        parent.AddNode(new InputNode("A", "a", () => 4.0));
        parent.AddNode(new InputNode("B", "b", () => 4.0));
        parent.AddNode(m.Compare("compareAB", "a", "b", "compAB", Comparator.Eq));

        parent.AddNode(m.If("ifNode", "compAB", "childResult1", "childResult2", "final"));
        // child 의 resultKey 를 부모의 out 키로 복사.
        parent.AddNode(new PageNode("ChildNode", child, "childResult1"));

        // ========================== Execute =================================
        var ctx = Ctx.Of();
        ctx = parent.Exec(ctx);

        if (ctx.TryGetResult<double>(out var res))
            Console.WriteLine($"(a+b) x 2 = {res}");
 */

//////////////////////// Connect 예제
/*
 * Console.WriteLine("Node Network 시작");
        var m = new NodeManager();

        // 서브 그래프

        var child = new Page("ChildSum", resultKey: "childResult1");

        // 노드 추가하고 id 받기
        var idC = child.AddNode(new InputNode("C", "c", () => 5.0));
        var idD = child.AddNode(new InputNode("D", "d", () => 10.0));
        var idSum = child.AddNode(m.Operation("sum", "c", "d", "childResult1", Operation.Add));
        var idSub = child.AddNode(m.Operation("sub", "c", "d", "childResult2", Operation.Sub));

        // 포트 ↔ 포트 연결
        child.Connect(idC.Id, "out", idSum.Id, "a");  // C → sum.a
        child.Connect(idD.Id, "out", idSum.Id, "b");  // D → sum.b

        child.Connect(idC.Id, "out", idSub.Id, "a");  // C → sub.a
        child.Connect(idD.Id, "out", idSub.Id, "b");  // D → sub.b


        // ========================== Execute =================================
        var ctx = Ctx.Of();
        ctx = child.Exec(ctx);

        if (ctx.TryGetResult<double>(out var res))
            Console.WriteLine($"(a+b) x 2 = {res}");
 */

//////////////////////// If node + SubGraph + Connect 예제
/*
 * Console.WriteLine("Node Network 시작");
        var m = new NodeManager();

        // 서브 그래프

        var child = new Page("ChildSum", resultKey: "childResult1");
        var parent = new Page("Parent", resultKey: "childResult2");

        // 노드 추가하고 id 받기
        var idC = child.AddNode(new InputNode("C", "c", () => 5.0));
        var idD = child.AddNode(new InputNode("D", "d", () => 10.0));
        var idSum = child.AddNode(m.Operation("sum", "c", "d", "childResult1", Operation.Add));
        var idSub = child.AddNode(m.Operation("sub", "c", "d", "childResult2", Operation.Sub));

        // 포트 ↔ 포트 연결
        child.Connect(idC.Id, "out", idSum.Id, "a");  // C → sum.a
        child.Connect(idD.Id, "out", idSum.Id, "b");  // D → sum.b

        child.Connect(idC.Id, "out", idSub.Id, "a");  // C → sub.a
        child.Connect(idD.Id, "out", idSub.Id, "b");  // D → sub.b

        parent.AddNode(new PageNode("chilf", child, "childResult1"));

        // ========================== Execute =================================
        var ctx = Ctx.Of();
        ctx = parent.Exec(ctx);

        if (ctx.TryGetResult<double>(out var res))
            Console.WriteLine($"a - b  = {res}");
 */

////////// 구조 변경 전
/*
 * Console.WriteLine("Node Network 시작");
        var m = new NodeManager();

        // sub graph
        var child = new Page("ChildSum", resultKey: "childResult1");
        // main graph
        var parent = new Page("Parent", resultKey: "childResult2");

        // 노드 추가, node return
        var c = child.AddNode(new InputNode("C", "c", () => 5.0));
        var d = child.AddNode(new InputNode("D", "d", () => 10.0));
        var Sum2 = child.AddNode(m.Operation("sum", "c", "d", "childResult1", Operation.Add));
        var Sub2 = child.AddNode(m.Operation("sub", "c", "d", "childResult2", Operation.Sub));

        // page 에서 노드간 connect
        child.Connect(c.Id, "out", Sum2.Id, "a");  
        child.Connect(d.Id, "out", Sum2.Id, "b"); 
        child.Connect(c.Id, "out", Sub2.Id, "a"); 
        child.Connect(d.Id, "out", Sub2.Id, "b"); 

        parent.AddNode(new PageNode("child", child, "childResult1"));

        // ========================== Execute =================================
        var ctx = Ctx.Of();
        ctx = parent.Exec(ctx);

        if (ctx.TryGetResult<double>(out var res))
            Console.WriteLine($"a - b  = {res}");
 */