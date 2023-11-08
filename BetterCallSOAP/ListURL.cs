using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetterCallSOAP
{
    public partial class ListURL : Form
    {
        public string selectedURL { get { return lbURL.SelectedItem.ToString(); } }
        public ListURL()
        {
            InitializeComponent();
        }

        private void ListURL_Load(object sender, EventArgs e)
        {
            LoadURiFromFile();
        }

        private void LoadURiFromFile()
        {
            int counter = 0;
            string line;
            //var path = Path.Combine(Directory.GetCurrentDirectory(), "\\lien.config");
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\lien.config";
            List<string> listURI = new List<string>();
            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                listURI.Add(line);
                lbURL.Items.Add(line);
                counter++;
            }

            file.Close();

            // Suspend the screen.
            // cbListLien.Items.Add(listURI);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lbURL.SelectedIndex != -1) Form.ActiveForm.Close();
        }

        private void lbURL_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Form.ActiveForm.Close();
        }

        private void lbURL_DoubleClick(object sender, EventArgs e)
        {
            Form.ActiveForm.Close();
        }
    }
}
