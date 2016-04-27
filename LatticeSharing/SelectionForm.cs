using Fleet.Lattice.Discovery;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LatticeSharing
{
    public class RepresentedListViewItem<T>: ListViewItem
    {
        public T RepresentedObject { get; set; }
    }

    public partial class SelectionForm : Form
    {
        public List<ServiceRecord> selectedRecords;

        public SelectionForm()
        {
            InitializeComponent();

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
        }

        private void SelectTargets(object sender, EventArgs e)
        {
            var indicies = table.SelectedIndices;
            selectedRecords = new List<ServiceRecord>();

            foreach (int index in indicies)
            {
                var item = table.Items[index] as RepresentedListViewItem<ServiceRecord>;
                selectedRecords.Add(item.RepresentedObject);
            }

            this.Close();
        }
    }
}
