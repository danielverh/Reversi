using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;  

namespace Reversi
{
    public partial class Form1 : Form
    {
        private ReversiGame game;
        public Form1()
        {
            InitializeComponent();
            game = new ReversiGame(reversiBoard1);
        }
    }
}
