using Fleet.Lattice;
using Fleet.Lattice.Discovery;
using Fleet.Lattice.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace File_O_Matic
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var table = this.listView;

            table.View = View.Details;
            table.FullRowSelect = true;
            table.GridLines = true;

            ColumnHeader serviceName = new ColumnHeader();
            serviceName.Text = "Service Name";
            ColumnHeader hostName = new ColumnHeader();
            hostName.Text = "Host name";

            table.Columns.AddRange(new ColumnHeader[] { serviceName, hostName });

            var records = Program.discovery.CurrentRecords;

            foreach (var recordpair in records)
            {
                var record = recordpair.Value;

                var row = new RepresentedListViewItem<ServiceRecord>();
                row.RepresentedObject = record;
                row.Text = record.ServiceName;
                row.SubItems.Add(record.Hostname);

                table.Items.Add(row);
            }

            //OnRefreshList(null, null);
        }

        private void OnOpenFile(object sender, EventArgs e)
        {
            var picker = new OpenFileDialog();
            picker.Multiselect = false;

            if (picker.ShowDialog() == DialogResult.OK)
            {
                this.fileLabel.Text = picker.FileName;
                this.shareButton.Enabled = true;
                  
            }
        }

        private void OnRefreshList(object sender, EventArgs e)
        {
            listView.Items.Clear();
            var records = Program.discovery.CurrentRecords;

            foreach (var recordpair in records)
            {
                var record = recordpair.Value;

                var row = new RepresentedListViewItem<ServiceRecord>();
                row.RepresentedObject = record;
                row.Text = record.ServiceName;
                row.SubItems.Add(record.Hostname);

                listView.Items.Add(row);
            }
        }

        private void OnShare(object sender, EventArgs e)
        {
            var indicies = listView.SelectedIndices;
            var selectedrecords = new List<ServiceRecord>();

            foreach (int index in indicies)
            {
                var item = listView.Items[index] as RepresentedListViewItem<ServiceRecord>;
                selectedrecords.Add(item.RepresentedObject);
            }

            var file = new LatticeFile();
            file.FileContents = File.ReadAllBytes(this.fileLabel.Text);

            var components = this.fileLabel.Text.Split('\\');
            file.FileName = components[components.Length - 1];

            foreach (var service in selectedrecords)
            {
                var client = LatticeUtil.MakeLatticeClient(service.Hostname);
                client.SendFile(file);
            }
        }
    }

    public class RepresentedListViewItem<T> : ListViewItem
    {
        public T RepresentedObject { get; set; }
    }
}
