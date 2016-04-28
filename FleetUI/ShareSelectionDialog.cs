using System;
using System.Collections.Generic;
using Fleet.Lattice.Discovery;

namespace FleetUI
{
	public partial class ShareSelectionDialog : Gtk.Dialog
	{
		private Gtk.ListStore model;
		public String selectedHost;

		public ShareSelectionDialog ()
		{
			this.Build ();


			var table = this.treeview4;

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

			var listStore = new Gtk.ListStore (typeof(String), typeof(String));
			
			table.Model = listStore;

			this.model = listStore;
			this.OnTableRefresh (null, null);

			this.buttonOk.Clicked += OnSelection;
		}
			
		public void OnTableRefresh(Object o, EventArgs args) {
			model.Clear();
			var records = MainClass.discovery.CurrentRecords;

			foreach (var recordpair in records) {
				var record = recordpair.Value;

				this.model.AppendValues (record.ServiceName, record.Hostname);
			}
		}

		protected void OnSelection (object sender, EventArgs e)
		{
			var selected = this.treeview4.Selection;
			Gtk.TreeModel model;
			Gtk.TreeIter iter;

			selected.GetSelected (out model, out iter);
			this.selectedHost = model.GetValue (iter, 1) as String;
		}

		protected void OnCancel (object sender, EventArgs e)
		{
			
		}
	}
}
