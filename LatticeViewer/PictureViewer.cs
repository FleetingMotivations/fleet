using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LatticeViewer
{
    public partial class PictureViewer : Form
    {
        public PictureViewer(Image img)
        {
            InitializeComponent();

            this.imageView.Image = img;
        }
    }
}
