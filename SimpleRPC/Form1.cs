using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;


namespace SimpleRPC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            RPC_Example r = new RPC_Example();

            r.puckk((Int32)5, "fuck!");

            var o1= 4;
            var o2= 3;
            Type t = o1.GetType();
            s(o1, o2);
        }

        

        int s(int o1, int o2)
        {
            return o1 + o2;
        }
    }
}
