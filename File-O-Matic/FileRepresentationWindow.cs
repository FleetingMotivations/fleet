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
using System.IO;

namespace File_O_Matic
{
    public partial class FileRepresentationWindow : Form
    {
        private String filename;
        private Boolean saved = false;

        public FileRepresentationWindow(String filename)
        {
            InitializeComponent();

            this.filename = filename;
            this.filenameLabel.Text = filename;

            try
            {
                this.pictureBox.Image = Image.FromFile(filename);
            } catch (Exception)
            {

                this.pictureBox.Image = Image.FromFile("File.jpeg");
            }
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
            if (!this.saved)
            {
                try
                {
                    this.DoSave();
                }
                catch (Exception)
                {

                }
            }
        }

        private void DoSave()
        {
            if (MessageBox.Show("Do you want to save this file?", "Save File", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var savedialog = new SaveFileDialog();

                if (savedialog.ShowDialog() == DialogResult.OK)
                {
                    if (System.IO.File.Exists(savedialog.FileName))
                    {
                        File.Delete(this.filename);
                    }

                    File.Move(this.filename, savedialog.FileName);
                    this.saved = true;
                    this.filename = savedialog.FileName;
                    this.filenameLabel.Text = filename;
                }
            }
        }

        private void FileRepresentationWindow_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                this.DoSave();
            }
            catch (Exception)
            {

            }

        }
    }
}
