using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceModel;
using Fleet.Lattice.Network;

namespace LatticeSharing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void SelectImage(object sender, MouseEventArgs e)
        {
            var open = new OpenFileDialog();
            open.Filter = "Image Files (*.jpg, *.jpeg) | *.jpg; *.jpeg;";
            if (open.ShowDialog() == DialogResult.OK)
            {
                var image = Image.FromFile(open.FileName);
                this.pictureBox1.Image = image;
            }
        }

        private void ShareImage(object sender, MouseEventArgs e)
        {
            var selector = new SelectionForm();
            selector.ShowDialog(this);

            var image = this.pictureBox1.Image;

            var targets = selector.selectedRecords;
            foreach (var service in targets)
            {
                var address = new EndpointAddress("net.tcp://" + service.Hostname + "/Lattice");
                var binding = new NetTcpBinding();
                binding.Security.Mode = SecurityMode.None;

                var client = new LatticeServiceClient(binding, address);
                client.SendImage(image as Bitmap);
            }
        }
    }
}
