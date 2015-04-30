using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace Lab4_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var h1 = new HttpChannel(0);
            ChannelServices.RegisterChannel(h1);
            Object remoteOb = RemotingServices.Connect(
            typeof(IDatabaseObject),
            "http://localhost:54321/SOEndPoint");
            Console.WriteLine("Enter lastname");
            var lastname = Console.ReadLine();
            var so = remoteOb as IDatabaseObject;
            var list = so.QueryResult(lastname);
            Console.Write(lastname);
            Console.Write(list);
            Console.ReadLine();
        }
    }
}
