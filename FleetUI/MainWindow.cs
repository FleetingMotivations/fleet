using System;
using System.ComponentModel;
using System.Linq;
using Gdk;
using Gtk;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using FleetUI;
using TypeConverter = GLib.TypeConverter;

public partial class MainWindow: Gtk.Window
{
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
	    Build();
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
        CaptureScreenSection(0, 0, screen.Width, screen.Height);
    }

	private void CaptureScreenSection(int x1, int y1, int x2, int y2)
	{
	    int temp;
	    if (x1 > x2)
	    {
	        temp = x2;
	        x2 = x1;
	        x1 = temp;
	    }
	    if (y1 > y2)
	    {
            temp = y2;
            y2 = y1;
            y1 = temp;    
	    }
	    var xWidth = x2 - x1;
	    var yWidth = y2 - y1;
	    using (var screenCapture = new Bitmap(xWidth, yWidth))
	    {
	        using (var graphics = Graphics.FromImage(screenCapture))
	        {
                // Copy the image from the screen
                graphics.CopyFromScreen(x1, y1,
                    0, 0,
                    screenCapture.Size,
                    CopyPixelOperation.SourceCopy);

                // Change the format of the image
                var bitStream = new MemoryStream();
                screenCapture.Save(bitStream, ImageFormat.Bmp);
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
		var capSelector = new ScreenCaptureSelector(delegate(ScreenCaptureSelector.ScreenCaptureCoords coords)
		{
		    CaptureScreenSection(coords.TouchdownX, 
                coords.TouchdownY, 
                coords.CompleteX, 
                coords.CompleteY);
            Show();
		    return true;
		});

		capSelector.Show ();

	}

	protected void OnSaveImage (object sender, EventArgs e)
	{
	    var bitmap = displayImage.Pixbuf.ToBitmap();
	    var outputFile = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
	    outputFile += "/img.bmp";
        Console.WriteLine(outputFile);

	    using (var memory = new MemoryStream())
	    {
	        using (var fileStream = new FileStream(outputFile, FileMode.Create, FileAccess.ReadWrite))
	        {
	            bitmap.Save(memory, ImageFormat.Bmp);
	            byte[] bytes = memory.ToArray();
                fileStream.Write(bytes, 0, bytes.Length);
	        }
	    }
        bitmap.Dispose();
	}



}

public static class Extensions
{
    public static System.Drawing.Bitmap ToBitmap(this Pixbuf pix)
    {
        var converter = TypeDescriptor.GetConverter(typeof(Bitmap));
        return (Bitmap) converter.ConvertFrom(pix.SaveToBuffer("bmp"));
    }
}