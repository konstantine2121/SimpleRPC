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
using System.Runtime.InteropServices;


namespace SimpleRPC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            RPC_Example r = new RPC_Example();

            string str = "hello!";

            /*ObjectInfo oo = new ObjectInfo();
            oo.typeName = "sds";
            oo.objectData = new byte[10];
            for (int f = 0; f < 10; f++)
            {
                oo.objectData[f] =(byte) f;
            }*
             */

            ff asfsa = new ff();
           

            ObjectInfo o1 = new ObjectInfo(asfsa);



            object d1 = o1.GetObject();


            //           string hi =(string) o1.GetObject();
            
            int b = 5;

        }

        struct ff
        {

            public int[] fff;
        }

    }
}
