using System;
using System.Collections.Generic;
using Fleet.Lattice.Discovery;

namespace FleetUI
{
	public partial class ShareSelectionDialog : Gtk.Dialog
	{
		private Gtk.ListStore model;
		public List<String> selectedHosts;

		public ShareSelectionDialog ()
		{
			this.Build ();

			var table = this.treeview4;
            table.Selection.Mode = Gtk.SelectionMode.Multiple;

            // Create columns
			var col1 = new Gtk.TreeViewColumn ();
			col1.Title = "Service Name";
			var col1renderer = new Gtk.CellRendererText ();
			col1.PackStart (col1renderer, true);
			col1.AddAttribute (col1renderer, "text", 0);

			var col2 = new Gtk.TreeViewColumn ();
			col2.Title = "Host Name";
			var col2renderer = new Gtk.CellRendererText ();
			col2.PackStart (col2renderer, true);
			col2.AddAttribute (col2renderer, "text", 1);

			table.AppendColumn (col1);
			table.AppendColumn (col2);

            // Create model
			var listStore = new Gtk.ListStore (typeof(String), typeof(String));
			
			table.Model = listStore;

			this.model = listStore;
			this.OnTableRefresh (null, null);

            // On close event
			this.buttonOk.Clicked += OnSelection;
		}
			
        // Table refresh event
		public void OnTableRefresh(Object o, EventArgs args) {
            // Clear model, get current records
			model.Clear();
			var records = MainClass.discovery.CurrentRecords;

            // Insert all record values into table
			foreach (var recordpair in records) {
				var record = recordpair.Value;
				this.model.AppendValues (record.ServiceName, record.Hostname);
            }
		}

        // On result selection event
		protected void OnSelection (object sender, EventArgs e)
		{
#warning This needs to accomodate multiple selection
            // Get the selection, get selection model and iterator
            var selected = this.treeview4.Selection;
			Gtk.TreeIter iter;

            // Init selection and iterator
            selectedHosts = new List<String>();
            var rows = selected.GetSelectedRows();

            // Send to each host
            foreach (var row in rows)
            {
                this.model.GetIter(out iter, row);

                var host = this.model.GetValue(iter, 1) as String;
                selectedHosts.Add(host);
            }
        }

		protected void OnCancel (object sender, EventArgs e)
		{
			
		}
	}
}
