using Google.Protobuf;
using Grpc.Core;
using System;
using static Etcdserverpb.KV;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Channel c = new Channel("localhost", 2379, ChannelCredentials.Insecure);
            KVClient kv = new KVClient(c);

            Console.WriteLine("client");

            while (true)
            {
                Console.WriteLine("...");

                var sss = Console.ReadLine();
                if (sss.StartsWith("get "))
                {
                    sss = sss.Replace("get ", "").TrimStart();

                    var lis = kv.Range(new Etcdserverpb.RangeRequest() { Key = ByteString.CopyFromUtf8(sss) });
                    Console.WriteLine(lis.ToString());
                }
                else if (sss.StartsWith("set "))
                {
                    sss = sss.Replace("set ", "").TrimStart();
                    var sasa = sss.Split(' ');
                    var res = kv.Put(new Etcdserverpb.PutRequest() { Key = Google.Protobuf.ByteString.CopyFromUtf8(sasa[0]), Value = ByteString.CopyFromUtf8(sasa[1]) });
                    Console.WriteLine(res.ToString());

                }
            }

        }
    }
}