using Magazin.BLL;
using Magazin.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Web.UI;
using System.Web;

namespace Magazin.UI
{
    public partial class frmComanda : Form
    {
        frmAdminDashboard frm;
        public frmComanda()
        {
            InitializeComponent();
            frm = new frmAdminDashboard();
        }
        ComandaDAL dal = new ComandaDAL();
        ComandaBLL c = new ComandaBLL();
        public void Clear()
        {
            txtAdresaID.Text = "";
            txtComandaID.Text = "";
            txtNume.Text = "";
            txtPrenume.Text = "";
            txtSearch.Text = "";
            txtDataComanda.Text = "";
            txtDataExpediere.Text = "";
            txtStare.Text = "";
            txtSuma.Text = "";
        }
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void frmComanda_Load(object sender, EventArgs e)
        {
            DataTable dt = dal.Select();
            dgvComanda.DataSource = dt;
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtSearch.Text;

            if (keywords != null)
            {
                DataTable dt = dal.Search(keywords);
                dgvComanda.DataSource = dt;
            }
            else
            {
                DataTable dt = dal.Select();
                dgvComanda.DataSource = dt;
            }
        }

        private void dgvComanda_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int RowIndex = e.RowIndex;
            txtAdresaID.Text = dgvComanda.Rows[RowIndex].Cells[1].Value.ToString(); 
            txtComandaID.Text = dgvComanda.Rows[RowIndex].Cells[0].Value.ToString();
            txtNume.Text = dgvComanda.Rows[RowIndex].Cells[2].Value.ToString();
            txtPrenume.Text = dgvComanda.Rows[RowIndex].Cells[3].Value.ToString();
            txtDataComanda.Text = dgvComanda.Rows[RowIndex].Cells[4].Value.ToString();
            txtDataExpediere.Text = dgvComanda.Rows[RowIndex].Cells[5].Value.ToString();
            txtStare.Text = dgvComanda.Rows[RowIndex].Cells[6].Value.ToString();
            txtSuma.Text = dgvComanda.Rows[RowIndex].Cells[7].Value.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            c.adresa_id=Convert.ToInt32(txtAdresaID.Text);
            c.data_comanda = Convert.ToDateTime(txtDataComanda.Text);
            c.data_expediere=Convert.ToDateTime(txtDataExpediere.Text);
            c.stare=txtStare.Text;
            bool succes = dal.Insert(c);

            if (succes)
            {
                MessageBox.Show("Comanda a fost creata cu succes.");
                Clear();
                DataTable dt = dal.Select();
                dgvComanda.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Nu s-a putut crea noua comanda.");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            c.adresa_id = Convert.ToInt32(txtAdresaID.Text);
            c.comanda_id = Convert.ToInt32(txtComandaID.Text);
            c.data_comanda = Convert.ToDateTime(txtDataComanda.Text);
            c.data_expediere = Convert.ToDateTime(txtDataExpediere.Text);
            c.stare = txtStare.Text;
            bool succes = dal.Update(c);

            if (succes)
            {
                MessageBox.Show("Comanda a fost actualizata cu succes.");
                Clear();
                DataTable dt = dal.Select();
                dgvComanda.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Nu s-a putut actualiza comanda.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            c.comanda_id = Convert.ToInt32(txtComandaID.Text);
            bool succes = dal.Delete(c);

            if (succes)
            {
                MessageBox.Show("Comanda stearsa cu succes.");
                Clear();
                DataTable dt = dal.Select();
                dgvComanda.DataSource = dt;

            }
            else
            {
                MessageBox.Show("Comanda nu a fost stearsa.");
            }
        }

        private void btnComenziMari_Click(object sender, EventArgs e)
        {
            DataTable dt = dal.Select_having();
            dgvComanda.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = dal.Select();
            dgvComanda.DataSource = dt;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            frm.Show();
        }

        protected void GenerateInvoicePDF(object sender, EventArgs e)
        {
            DataTable dt = dal.InvoiceBill(txtComandaID.Text);
            dgvComanda.DataSource = dt;

            String comandaID = dgvComanda.Rows[0].Cells[0].Value.ToString();
            String adresa = dgvComanda.Rows[0].Cells[1].Value.ToString();
            String nume = dgvComanda.Rows[0].Cells[2].Value.ToString();
            String prenume = dgvComanda.Rows[0].Cells[3].Value.ToString();
            String data_comanda = dgvComanda.Rows[0].Cells[4].Value.ToString();

            String[] nume_produs = new String[dgvComanda.Rows.Count];
            String[] cod_produs = new String[dgvComanda.Rows.Count];
            String[] pret = new String[dgvComanda.Rows.Count];
            String[] cantitate = new String[dgvComanda.Rows.Count];
            String[] suma_per_cantitate = new String[dgvComanda.Rows.Count];
            int[] suma_per_cantitate_int = new int[dgvComanda.Rows.Count];

            for (int RowIndex = 0; RowIndex < dgvComanda.Rows.Count; RowIndex++)
            {

                nume_produs[RowIndex] = dgvComanda.Rows[RowIndex].Cells[5].Value.ToString();
                cod_produs[RowIndex] = dgvComanda.Rows[RowIndex].Cells[6].Value.ToString();
                pret[RowIndex] = dgvComanda.Rows[RowIndex].Cells[7].Value.ToString();
                cantitate[RowIndex] = dgvComanda.Rows[RowIndex].Cells[8].Value.ToString();
                suma_per_cantitate[RowIndex] = dgvComanda.Rows[RowIndex].Cells[9].Value.ToString();
                suma_per_cantitate_int[RowIndex] = Int32.Parse(dgvComanda.Rows[RowIndex].Cells[9].Value.ToString());
            }
             


            /**********TO DO***********/
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    StringBuilder sb = new StringBuilder();

                    //Generate Invoice (Bill) Header.
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Order Sheet</b></td></tr>");
                    sb.Append("<tr><td colspan = '2'></td></tr>");
                    sb.Append("<tr><td><b>Order No: </b>");
                    sb.Append(orderNo);
                    sb.Append("</td><td align = 'right'><b>Date: </b>");
                    sb.Append(DateTime.Now);
                    sb.Append(" </td></tr>");
                    sb.Append("<tr><td colspan = '2'><b>Company Name: </b>");
                    sb.Append(companyName);
                    sb.Append("</td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br />");

                    //Generate Invoice (Bill) Items Grid.
                    sb.Append("<table border = '1'>");
                    sb.Append("<tr>");
                    foreach (DataColumn column in dt.Columns)
                    {
                        sb.Append("<th style = 'background-color: #D20B0C;color:#ffffff'>");
                        sb.Append(column.ColumnName);
                        sb.Append("</th>");
                    }
                    sb.Append("</tr>");
                    foreach (DataRow row in dt.Rows)
                    {
                        sb.Append("<tr>");
                        foreach (DataColumn column in dt.Columns)
                        {
                            sb.Append("<td>");
                            sb.Append(row[column]);
                            sb.Append("</td>");
                        }
                        sb.Append("</tr>");
                    }
                    sb.Append("<tr><td align = 'right' colspan = '");
                    sb.Append(dt.Columns.Count - 1);
                    sb.Append("'>Total</td>");
                    sb.Append("<td>");
                    sb.Append(dt.Compute("sum(Total)", ""));
                    sb.Append("</td>");
                    sb.Append("</tr></table>");

                    //Export HTML String as PDF.
                    StringReader sr = new StringReader(sb.ToString());
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    pdfDoc.Open();
                    htmlparser.Parse(sr);
                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=Invoice_" + orderNo + ".pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.End();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataTable dt = dal.InvoiceBill(txtComandaID.Text);
            dgvComanda.DataSource = dt;
          
         
        }

        private void txtSuma_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
