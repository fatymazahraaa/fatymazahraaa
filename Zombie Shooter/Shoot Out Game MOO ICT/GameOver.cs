using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zombie_Shooter
{
    public partial class GameOver : Form
    {
        public GameOver(Form1 form)
        {
            InitializeComponent();
            mainForm = form;
        }
   

    private Form1 mainForm;

    private void btnPlayAgain_Click(object sender, EventArgs e)
    {
        mainForm.RestartGame();   
        mainForm.Show();         
        this.Close();            
    }

}

                 
    }
