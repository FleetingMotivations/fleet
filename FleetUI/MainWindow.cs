using System;
using System.Linq;
using Gdk;
using Gtk;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

using FleetUI;

public partial class MainWindow: Gtk.Window
{
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
	    //this.Opacity = 0.79;
	    //this.Decorated = false;
        //this.Fullscreen();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
	    Gtk.Application.Quit ();
		a.RetVal = true;
	}

    /// <summary>
    /// View an image with the file dialogue
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
			    DisplayImage(new Pixbuf(fileDialog.Filename)); 
	        }

	        fileDialog.Destroy();
	    }
	}


	protected void OnExit (object sender, EventArgs e)
	{
		Gtk.Application.Quit();
	}


	protected void OnAbout (object sender, EventArgs e)
	{
		throw new NotImplementedException ();
	}

    protected void OnCaptureDesktop(object sender, EventArgs e)
    {
	    var screen = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
        CaptureScreenSection(0, 0, screen.X, screen.Y);
    }

	private void CaptureScreenSection(int x, int y, int width, int height) 
	{
	    using (var screenCapture = new Bitmap(width, height))
	    {
	        using (var graphics = Graphics.FromImage(screenCapture))
	        {
                // Copy the image from the screen
                graphics.CopyFromScreen(width, height,
                    x, y,
                    screenCapture.Size,
                    CopyPixelOperation.SourceCopy);

                // Change the format of the image
                var bitStream = new MemoryStream();
                screenCapture.Save(bitStream, ImageFormat.Jpeg);
	            bitStream.Position = 0;
                this.DisplayImage(new Pixbuf(bitStream));
	        }
	    }
	}

    private void DisplayImage(Pixbuf buffer)
    {
        displayImage?.Pixbuf?.Dispose();
        displayImage.Pixbuf = buffer;       
    }

	protected void OnPortion (object sender, EventArgs e)
	{
        // Hide the main application
        this.Hide();
        // Throw up fullscreen overlay
		var capSelector = new FleetUI.ScreenCaptureSelector(delegate(Tuple<double, double, double, double> coords)
		{
		    CaptureScreenSection((int)coords.Item1, (int)coords.Item2, (int)coords.Item3, (int)coords.Item4);
            this.Show();
		    return true;
		});

		capSelector.Show ();

	}
}