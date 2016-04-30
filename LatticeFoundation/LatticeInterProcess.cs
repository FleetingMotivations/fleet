using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Fleet.Lattice.IPC
{
    //  ==  ==  ==  ==
    //  Contract    ==
    //  ==  ==  ==  ==

    [ServiceContract]
    public interface ILatticeIPC
    {
        [OperationContract(IsOneWay=true)]
        void PassText(String text);

        [OperationContract(IsOneWay = true)]
        void PassImage(Bitmap bmp);

        [OperationContract(IsOneWay = true)]
        void PassFilename(String filename);

        [OperationContract]
        Boolean RegisterClient(String pipename);

        [OperationContract]
        Boolean DeregisterClient(String pipename);
    }

    //  ==  ==  ==
    //  Host    ==
    //  ==  ==  ==

    public class LatticeIPCHost: ILatticeIPC
    {
        public static Dictionary<String, String> clients = new Dictionary<string, string>();
        public static IReadOnlyDictionary<String, String> CurrentClients {
            get {
                return new ReadOnlyDictionary<String, String>(clients);
            }
        }

        public static event DidReceiveEvent<String> DidPassText = delegate { };
        public static event DidReceiveEvent<Image> DidPassImage = delegate { };
        public static event DidReceiveEvent<String> DidPassFilename = delegate { };

        public void PassText(String text)
        {
            DidPassText(text, new EventArgs());
        }

        public void PassImage(Bitmap img)
        {
            DidPassImage(img, new EventArgs());
        }

        public void PassFilename(String filename)
        {
            DidPassFilename(filename, new EventArgs());
        }

        public Boolean RegisterClient(String pipename)
        {
            clients[pipename] = pipename;
            Console.WriteLine("Registered Pipe: {0}", pipename);
            return true;
        }

        public Boolean DeregisterClient(String pipename)
        {
            clients.Remove(pipename);
            Console.WriteLine("Client: {0}, has deregistered", pipename);
            return true;
        }
    }

    //  ==  ==  ==
    //  Client  ==
    //  ==  ==  ==

    public class LatticeIPCClient: ClientBase<ILatticeIPC>, ILatticeIPC
    {
        public LatticeIPCClient(Binding binding, EndpointAddress address) : base(binding, address) { }

        public void PassText(String text)
        {
            Channel.PassText(text);
        }

        public void PassImage(Bitmap img)
        {
            Channel.PassImage(img);
        }

        public void PassFilename(String filename)
        {
            Channel.PassFilename(filename);
        }

        public Boolean RegisterClient(String pipename)
        {
            return Channel.RegisterClient(pipename);
        }

        public Boolean DeregisterClient(String pipename)
        {
            return Channel.DeregisterClient(pipename);
        }
    }
}
