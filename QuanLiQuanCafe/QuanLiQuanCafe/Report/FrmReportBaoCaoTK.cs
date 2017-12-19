using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace QuanLiQuanCafe.Report
{
    public partial class FrmReportBaoCaoTK : DevExpress.XtraEditors.XtraForm
    {
        private DataTable dt = null;
        public FrmReportBaoCaoTK(DataTable dts)
        {
            dt = dts;
            InitializeComponent();
        }

        private void FrmReportBaoCaoTK_Load(object sender, EventArgs e)
        {
            ReportBaoCaoTKcs rp = new ReportBaoCaoTKcs();
            rp.DataSource = dt;
            rp.BindData();
            documentViewer1.PrintingSystem = rp.PrintingSystem;
            rp.CreateDocument();
        }
    }
}