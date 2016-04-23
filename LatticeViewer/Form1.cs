using System;
using System.Drawing;
using Fleet.Lattice;
using Fleet.Lattice.IPC;
using System.Windows.Forms;
using System.ServiceModel;
using System.IO;

namespace LatticeViewer
{
    public partial class Form1 : Form
    {
        ServiceHost host;

        public Form1()
        {
            InitializeComponent();

            LatticeIPCHost.DidPassText += (text, args) =>
            {
                Console.WriteLine("Was Passed Text: {0}", text);
            };

            LatticeIPCHost.DidPassImage += (bmp, args) =>
            {
                Console.WriteLine("Was Passed Image: {0}", bmp);

                var filename = Path.GetTempFileName() + ".jpg";
                bmp.Save(filename);

                var localImg = Image.FromFile(filename);

                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker) delegate {
                        try
                        {
                            var form = new PictureViewer(localImg);
                            form.Show();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    });
                } else
                {
                    var form = new PictureViewer(localImg);
                    form.Show();
                }
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            host = LatticeUtil.MakeIPCHost("LatticeViewer");
            host.Open();

            var client = LatticeUtil.MakeIPCClient();
            client.RegisterClient("LatticeViewer");
        }
    }
}
