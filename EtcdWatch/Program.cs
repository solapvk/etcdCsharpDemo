using Etcdserverpb;
using Grpc.Core;
using System;
using static Etcdserverpb.Watch;
using static Etcdserverpb.WatchCreateRequest.Types;

namespace Etcd
{
    class Program
    {

        static void Main(string[] args)
        {
            Channel c = new Channel("localhost", 2379, ChannelCredentials.Insecure);
            WatchClient cli = new WatchClient(c);

            var x = cli.Watch();
            Console.WriteLine("input key....");
            var key = Console.ReadLine();

            var watchAction = new Action(async () =>
            {
                await x.RequestStream.WriteAsync(new WatchRequest() { CreateRequest = new WatchCreateRequest() { ProgressNotify = false, Key = Google.Protobuf.ByteString.CopyFromUtf8(key), PrevKv = true } });
                await x.RequestStream.CompleteAsync();

            });
            watchAction();
            
            var eventOn = new Action(async () =>
            {
                while (await x.ResponseStream.MoveNext(new System.Threading.CancellationToken()))
                {
                    Console.WriteLine(x.ResponseStream.Current.ToString());
                    foreach (var item in x.ResponseStream.Current.Events)
                    {
                        Console.WriteLine(item.Kv.ToString());
                    }

                }
            });
            eventOn();

            Console.WriteLine("...");
            Console.ReadLine();
        }
    }
}