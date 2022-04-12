// See https://aka.ms/new-console-template for more information

using System.Net;
using AppInvesting;
using Newtonsoft.Json;
using WebSocket.Core;
using WebSocket.Core.Interfaces;

class Program
{
    static async Task Main( /* string[] args */)
    {
        var config = Configuration.Factory.BuildDefault("https://argaamws.tickerchart.net/connect?key=e293389e74845dc81827b8937524e519&symbols=DJI,SPX,IXIC,XAGUSD,XAUUSD,UKOil,NG,CL");
        // config.Logger = new ConsoleLogger();
        config.DefaultHeaders = new WebHeaderCollection
        {
            {HttpRequestHeader.UserAgent, "Custom User Agent"},
            {"application-key", "foo-bar"}
        };

        var sockJs = (IClient)new WebsocketClient(config);
        sockJs.Connected += async (sender, e) =>
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Connected  Exception: {ex.Message} ");
            }
        };
        sockJs.Message += async (sender, msg) =>
        {
            Console.WriteLine(msg);
        };
        sockJs.Disconnected += (sender, e) =>
        {
            Console.WriteLine($" Disconnected  ");
        };
        await sockJs.Connect();
        Console.Read();
    }
}
/*
 *  var config = Configuration.Factory.BuildDefault("https://stream183.forexpros.com:443/echo");
       // config.Logger = new ConsoleLogger();
        config.DefaultHeaders = new WebHeaderCollection
        {
            {HttpRequestHeader.UserAgent, "Custom User Agent"},
            {"application-key", "foo-bar"}
        };

        var sockJs = (IClient)new WebsocketClient(config);
        sockJs.Connected += async (sender, e) =>
                {
                    try
                    {
                        Console.WriteLine("****************** Main: Open");
                        //["{\"_event\":\"UID\",\"UID\":0}"]
                        await sockJs.Send(JsonConvert.SerializeObject(new {_event = "UID", UID = 0}));

                        await sockJs.Send(JsonConvert.SerializeObject(new { _event = "bulk-subscribe", tzID= 8,message = "pid-1057391:%%pid-1061443:%%pid-1061453:%%pid-1061448:%%pid-1114630:%%pid-169:%%pid-166:%%pid-172:%%pid-24441:%%pid-178:%%pid-171:%%pid-14958:%%pid-8830:%%pid-8849:%%pid-1:%%pid-13994:%%pid-23705:%%pid-8874:%%pid-8873:%%pid-2:%%pid-3:%%pid-7:%%pid-5:%%pid-4:%%pid-8839:%%pid-20:%%pid-27:%%pid-179:%%pid-1175152:%%pid-1175153:%%pid-44336:%%pid-8827:%%event-448947:%%event-448948:%%event-448957:%%event-448944:%%event-448952:%%event-448945:%%event-448955:%%isOpenExch-1:%%isOpenExch-2:%%isOpenPair-1175152:%%isOpenPair-1175153:%%isOpenPair-44336:%%isOpenPair-8827:%%domain-1:"  }));
                        await sockJs.Send(JsonConvert.SerializeObject(new {_event = "heartbeat", data = "h"}));

                    }
                    catch (Exception ex)
                    {
                       Console.WriteLine($" Connected  Exception: {ex.Message} ");
                    }
                };
                sockJs.Message += async (sender, msg) =>
                {
                    try
                    {
                        if (msg.Contains("message"))
                        {
                            var msgDef = new { message = ""};
                            var message = JsonConvert.DeserializeAnonymousType(msg, msgDef);
                            string tmsg = message.message.Replace("::", "=");
                            string[] tms = tmsg.Split("=".ToCharArray());
                            Console.WriteLine(tms[1]);
                           // string cqs = tms[1].Replace(",", "").Replace("%", "");
                            CommodityQout cq = JsonConvert.DeserializeObject<CommodityQout>(tms[1]);
                            Console.WriteLine(cq.ask);
                        }

                       // Console.WriteLine($"****************** Main: Message: {msg}");
                        if (msg != "test") return;
                        Console.WriteLine("****************** Main: Got back echo -> sending shutdown");
                        await sockJs.Disconnect();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($" Message Exception: {ex.Message} ");
                    }
                };
                sockJs.Disconnected += (sender, e) =>
                {
                    try
                    {
                        Console.WriteLine("****************** Main: Closed");
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($" Disconnected  Exception: {ex.Message} ");
                    }
                };

                await sockJs.Connect();
                while (true)
                {
                    Thread.Sleep(2000);
                    await sockJs.Send(JsonConvert.SerializeObject(new {_event = "heartbeat", data = "h"}));
                }

                Console.Read();
 */