using System;
using System.Drawing;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Fleet.Lattice.Network
{
    //  ==  ==  ==  ==  ==  ==
    //  Service Contract    ==
    //  ==  ==  ==  ==  ==  ==

    [ServiceContract]
    public interface ILatticeService
    {
        [OperationContract]
        void SendText(String text);

        [OperationContract]
        void SendImage(Bitmap bmp);
    }

    //  ==  ==  ==  ==  ==
    //  Lattice Server  ==
    //  ==  ==  ==  ==  ==

    public class LatticeServiceHost : ILatticeService
    {
        public static event DidReceiveEvent<String> DidReceiveText = delegate { };
        public static event DidReceiveEvent<Image> DidReceiveImage = delegate { };

        public void SendText(String text)
        {
            DidReceiveText(text, new EventArgs());
        }

        public void SendImage(Bitmap img)
        {
            DidReceiveImage(img, new EventArgs());
        }
    }

    //  ==  ==  ==  ==  ==
    //  Lattice Client  ==
    //  ==  ==  ==  ==  ==

    public class LatticeServiceClient : ClientBase<ILatticeService>, ILatticeService
    {
        public LatticeServiceClient(Binding binding, EndpointAddress address) : base(binding, address) { }

        public void SendText(String text)
        {
            Channel.SendText(text);
        }

        public void SendImage(Bitmap img)
        {
            Channel.SendImage(img);
        }
    }
}

