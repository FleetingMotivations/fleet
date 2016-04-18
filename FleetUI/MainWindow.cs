using System;
using System.Linq;
using Gdk;
using Gtk;

public partial class MainWindow: Gtk.Window
{
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}


	protected void OnOpen (object sender, EventArgs e)
	{
        var fileDialog = new FileChooserDialog (
            "Choose an Image to View",
            this,
            FileChooserAction.Open,
            "Cancel",
            ResponseType.Cancel,
            "Open",
            ResponseType.Accept
        );

	    if (fileDialog.Run() == (int) ResponseType.Accept)
	    {
	        var fileExtension = fileDialog.Filename.Split('.')
                .LastOrDefault();
	        var types = new [] {"png", "gif", "jpg"};

	        if (types.Contains(fileExtension))
	        {
			    displayImage?.Pixbuf?.Dispose();
	            displayImage.Pixbuf = new Pixbuf(fileDialog.Filename);
	        }

	        fileDialog.Destroy();
	    }
	}


	protected void OnExit (object sender, EventArgs e)
	{
		Application.Quit();
	}


	protected void OnAbout (object sender, EventArgs e)
	{
		throw new NotImplementedException ();
	}

}