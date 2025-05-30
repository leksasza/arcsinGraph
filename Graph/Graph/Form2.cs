using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graph
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        // здесь мы просто при нажатии на соответсвующую кнопку,
        // инициализируем и открываем нужное окно (соответсвенно форма1 для виндоус апликейшнс и форма3 для зедграфа)
        private void button2_Click(object sender, EventArgs e)
        {
            Form1 newForm = new Form1(); 
            newForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 newForm1 = new Form3();
            newForm1.Show();
        }
    }
}
