using Fleet.Lattice;
using Fleet.Lattice.Discovery;
using Fleet.Lattice.IPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace File_O_Matic
{ 
    static class Program
    {
        public static LatticeDiscovery discovery;
        public static ServiceHost ipcHost;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                discovery = new LatticeDiscovery();
                discovery.StartBrowsing();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            LatticeIPCHost.DidPassFilename += (filename, arg) =>
            {
                Console.WriteLine("Recevied File Reference: {0}", filename);

                var mainform = Application.OpenForms[0];
                if (mainform.InvokeRequired)
                {
                    mainform.Invoke((MethodInvoker) delegate
                    {
                        var form = new FileRepresentationWindow(filename);
                        form.Show();
                    });
                } else
                {
                    var form = new FileRepresentationWindow(filename);
                    form.Show();
                }
            };

            try
            {
                ipcHost = LatticeUtil.MakeIPCHost("File-O-Matic");
                ipcHost.Open();

                var client = LatticeUtil.MakeIPCClient();
                client.RegisterClient("File-O-Matic");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not initialise IPC");
                Console.WriteLine(ex.ToString());
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
