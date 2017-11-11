using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiQuanCafe
{
    public partial class FrmHome : Form
    {
        public FrmHome()
        {
            InitializeComponent();
        }

        private void FrmHome_Load(object sender, EventArgs e)
        {
            for(int j =1; j<=20; j++)
            {
                 ListViewItem im = new ListViewItem(j.ToString());
                for (int i = 0; i <3; i++)
                {
                    im.SubItems.Add("Đây là "+j.ToString());
                }
                listView1.Items.Add(im);
            }        }

        private void panelControl2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void quảnTrịToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
