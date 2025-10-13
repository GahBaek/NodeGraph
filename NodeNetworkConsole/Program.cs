// 사용 샘플 (Program) — Page는 비제네릭, PageManager.Run은 Guid 사용 가정
using System;
using NodeNetwork.SDK.Models;
using NodeNetwork.SDK.Services;

public class Program
{
    static void Main()
    {
        Console.WriteLine("Node Network 시작");

        IContext baseCtx = Ctx.Of(("a", 10.0), ("b", 5.0), ("c", 2.0), ("d", 7.0));

        var main = new Page("MainCalc", resultKey: "resultMain");
        var secondary = new Page("SecondaryCalc", resultKey: "final");


        var add1 = new Op("Add1", "a", "b", "sum1", Operations.Add);
        var div1 = new Op("Div1", "sum1", "c", "div1", Operations.Div);
        var add2 = new Op("Add2", "div1", "d", "resultMain", Operations.Add);

        var add1Id = main.AddNode(add1);
        var div1Id = main.AddNode(div1);
        var add2Id = main.AddNode(add2);

        main.Connect(add1Id, "sum1", div1Id, "sum1");
        main.Disconnect(add1Id, "sum1", div1Id, "sum1");
        main.Connect(add1Id, "sum1", div1Id, "sum1");      
        main.Connect(div1Id, "div1", add2Id, "div1");

        // SecondaryCalc: ((a + d) / b) + c = 5.4
        var addX = new Op("AddX", "a", "d", "x", Operations.Add);
        var divY = new Op("DivY", "x", "b", "y", Operations.Div);
        var addF = new Op("AddF", "y", "c", "final", Operations.Add);

        var addXId = secondary.AddNode(addX);
        var divYId = secondary.AddNode(divY);
        var addFId = secondary.AddNode(addF);

        secondary.Connect(addXId, "x", divYId, "x");
        secondary.Connect(divYId, "y", addFId, "y");

        var manager = new PageManager();
        manager.RegisterPage(main);
        manager.RegisterPage(secondary);

        var r1Ctx = manager.Run(main.Id, baseCtx);
        r1Ctx.TryGetResult<double>(out var r1);
        Console.WriteLine($"MainCalc = {r1}");              // 14.5

        var r2Ctx = manager.Run(secondary.Id, baseCtx);
        r2Ctx.TryGetResult<double>(out var r2);
        Console.WriteLine($"SecondaryCalc = {r2}");         // 5.4
    }
}
