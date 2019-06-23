using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Relative_Orientation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double f = Convert.ToDouble(textBox1.Text);
            double m = Convert.ToDouble(textBox7.Text);
            double[,] LP = new double[6, 2];
            double[,] RP = new double[6, 2];
            double[,] Result = new double[5, 1];
            double[,] MPoint = new double[6, 3];
            double[,] PPoint = new double[6, 3];

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                ListViewItem item1 = listView1.Items[i];
                ListViewItem item2 = listView2.Items[i];
                for (int j = 0; j < item1.SubItems.Count - 1; j++)
                {
                    LP[i, j] = Convert.ToDouble(item1.SubItems[j+1].Text);
                    RP[i, j] = Convert.ToDouble(item2.SubItems[j+1].Text);
                }                
            }            

            Result = Calculation.ROrient(LP, RP, f);
            textBox2.Text = Result[0, 0].ToString("G4");
            textBox3.Text = Result[1, 0].ToString("G4");
            textBox4.Text = Result[2, 0].ToString("G4");
            textBox5.Text = Result[3, 0].ToString("G4");
            textBox6.Text = Result[4, 0].ToString("G4");

            MPoint = MPGcoordinate.MC(LP, RP, f);
            PPoint = MPGcoordinate.PC(MPoint, m);
            for (int i = 0; i < listView6.Items.Count; i++)
            {
                ListViewItem item3 = listView3.Items[i];
                ListViewItem item6 = listView6.Items[i];
                ListViewItem item7 = listView7.Items[i];
                for (int j = 0; j < item6.SubItems.Count - 1; j++)
                {
                    item3.SubItems[j + 1].Text = Convert.ToString(PPoint[i, j]);
                    item6.SubItems[j + 1].Text = Convert.ToString(MPoint[i, j]);
                    item7.SubItems[j + 1].Text = Convert.ToString(PPoint[i, j]);                    
                }
            }
        }

        private void MPbutton_Click(object sender, EventArgs e)
        {
            double[,] GPoint = new double[6, 3];
            double[,] PPoint = new double[6, 3];
            double[] ARElem = new double[7];
            double[] Elem = new double[7];

            for (int i = 0; i < listView5.Items.Count; i++)
            {
                ListViewItem item5 = listView5.Items[i];
                Elem[i] = Convert.ToDouble(item5.SubItems[1].Text);
            }
            for (int i = 0; i < listView3.Items.Count; i++)
            {
                ListViewItem item3 = listView3.Items[i];
                for (int j = 0; j < item3.SubItems.Count - 1; j++)
                    PPoint[i, j] = Convert.ToDouble(item3.SubItems[j + 1].Text);
            }

            GPoint = MPGcoordinate.GC(PPoint, Elem);
            for (int i = 0; i < listView4.Items.Count; i++)
            {
                ListViewItem item4 = listView4.Items[i];
                for (int j = 0; j < item4.SubItems.Count - 1; j++)
                    item4.SubItems[j + 1].Text = Convert.ToString(GPoint[i, j]);
            }

            ARElem = MPGcoordinate.AOrient(PPoint, GPoint);
            for (int i = 0; i < listView8.Items.Count; i++)
            {
                ListViewItem item8 = listView8.Items[i];
                for (int j = 0; j < item8.SubItems.Count - 1; j++)
                    item8.SubItems[j + 1].Text = Convert.ToString(ARElem[i]);
            }
        }
    }
}
