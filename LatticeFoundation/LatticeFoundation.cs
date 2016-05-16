using System;
using System.Net;
using System.ServiceModel;
using Fleet.Lattice.Network;
using Fleet.Lattice.IPC;

namespace Fleet.Lattice
{
    //  ==  ==  ==
    //  Event   ==
    //  ==  ==  ==

    public delegate void DidReceiveEvent<T>(T data, EventArgs args);

    //  ==  ==  ==  ==  ==  ==  ==
    //  Convenience Utilities   ==
    //  ==  ==  ==  ==  ==  ==  ==

    public static class LatticeUtil
    {
        private const String protocol = "net.tcp";

        public static String GetLocalHost()
        {
            return Dns.GetHostName();
        }

        //  ==  ==  ==  ==
        //  WCF Helpers ==
        //  ==  ==  ==  ==

        public static ServiceHost MakeLatticeHost(String servicename = "Lattice")
        {
            var address = new Uri(String.Format("{0}://{1}/{2}", protocol, GetLocalHost(), servicename));
            var binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.None;
            binding.MaxReceivedMessageSize = 204003200;
            binding.MaxBufferSize = 204003200;

            var host = new ServiceHost(typeof(LatticeServiceHost));
            
            host.AddServiceEndpoint(typeof(ILatticeService), binding, address);
            return host;
        }

        public static ILatticeService MakeLatticeClient(String remotehost, String servicename = "Lattice")
        {
            var address = new EndpointAddress(String.Format("{0}://{1}/{2}", protocol, remotehost, servicename));
            var binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.None;
            binding.MaxReceivedMessageSize = 204003200;
            binding.MaxBufferSize = 204003200;

            var client = new LatticeServiceClient(binding, address);
            return client;
        }

        //  ==  ==  ==  ==
        //  IPC Helpers ==
        //  ==  ==  ==  ==

        public static ServiceHost MakeIPCHost(String pipename = "Lattice-IPC")
        {
            var address = new Uri(String.Format("net.pipe://localhost/{0}", pipename));
            var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            binding.MaxReceivedMessageSize = 204003200;
            binding.MaxBufferSize = 204003200;

            var host = new ServiceHost(typeof(LatticeIPCHost));
            host.AddServiceEndpoint(typeof(ILatticeIPC), binding, address);
            return host;
        }

        public static ILatticeIPC MakeIPCClient(String pipename = "Lattice-IPC")
        {
            var address = new EndpointAddress(String.Format("net.pipe://locahost/{0}", pipename));
            var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            binding.MaxReceivedMessageSize = 204003200;
            binding.MaxBufferSize = 204003200;

            var client = new LatticeIPCClient(binding, address);
            return client;
        }
    }
}
