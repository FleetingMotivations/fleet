using System;
using System.IO;
using Fleet.Lattice;
using Fleet.Lattice.Broadcast;
using Fleet.Lattice.Network;
using Fleet.Lattice.IPC;
using System.Drawing;

namespace LatticeDaemon
{
    class Program
    {
        static void Main(string[] args)
        {
            String serviceName = "Lattice Service Daemon";
            Int16 port = 80;
            String regtype = "_lattice._tcp";
            String replydomain = "local";

            // Parse args if present
            if (args.Length >= 1)
                serviceName = args[0];

            if (args.Length >= 2)
                port = Convert.ToInt16(args[1]);

            if (args.Length >= 3)
                regtype = args[2];

            if (args.Length >= 4)
                replydomain = args[3];

            // Start IPC
            var ipcHost = LatticeUtil.MakeIPCHost();
            ipcHost.Open();
            Console.WriteLine("Server is accepting IPC Clients");

            // Start WCF
            LatticeServiceHost.DidReceiveText += (text, arg) => {
                Console.WriteLine("Received Text: {0}", text);

                foreach (var ippair in LatticeIPCHost.CurrentClients)
                {
                    try
                    {
                        var pipe = ippair.Key;
                        var client = LatticeUtil.MakeIPCClient(pipe);
                        client.PassText(text);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            };

            LatticeServiceHost.DidReceiveImage += (bmp, arg) => {
                Console.WriteLine("Recevied Image: {0}", bmp);

                var filename = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                filename += Path.DirectorySeparatorChar + "recevied-" + DateTime.Now.ToString("yyyyMMDD-hh-mm-ss") + ".jpg";
                
                Console.WriteLine("Saving to: {0}", filename);
                
                try
                {
                    bmp.Save(filename);
                } catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                foreach (var ippair in LatticeIPCHost.CurrentClients)
                {
                    try
                    {
                        var pipe = ippair.Key;
                        var client = LatticeUtil.MakeIPCClient(pipe);
                        client.PassImage(bmp as Bitmap);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            };

            var wcfHost = LatticeUtil.MakeLatticeHost();
            wcfHost.Open();
            Console.WriteLine("Server is accepting sharing Clients");

            // Start Zeroconf
            var broadcast = new LatticeBroadcast(serviceName, port);
            broadcast.RegisterZeroconfService("Lattice", regtype, replydomain);

            

            Console.WriteLine("Press any key to stop the server");
            Console.ReadLine();

            Environment.Exit(0);
        }
    }
}
