using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.TestBotSrv;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TestBotServiceClient cl = new TestBotServiceClient();
            cl.Open();
            bool flg = cl.GetUser(1, 709190378, "Uliana Borisovets", "2020-06-19 19:28:09.157", 30);
            cl.Close();
        }
    }
}
