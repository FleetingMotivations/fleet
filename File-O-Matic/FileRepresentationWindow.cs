using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace File_O_Matic
{
    public partial class FileRepresentationWindow : Form
    {
        private String filename;

        public FileRepresentationWindow(String filename)
        {
            InitializeComponent();

            this.filename = filename;
            this.filenameLabel.Text = filename;

            
        }



        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start(filename);
        }

        private void OnShare(object sender, EventArgs e)
        {
            var filewindow = new Form1(this.filename);
            filewindow.Show();
            this.Close();
        }

        private void CheckSave(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to save this file?", "Save File", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var savedialog = new SaveFileDialog();
                
                if (savedialog.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.Move(this.filename, savedialog.FileName);
                }
            }
        }
    }
}
