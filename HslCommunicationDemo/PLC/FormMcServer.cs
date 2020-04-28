﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HslCommunication.Profinet;
using HslCommunication;
using HslCommunication.ModBus;
using System.Threading;

namespace HslCommunicationDemo
{
    public partial class FormMcServer : HslFormContent
    {
        public FormMcServer( )
        {
            InitializeComponent( );
        }

        private void FormSiemens_Load( object sender, EventArgs e )
        {
            panel2.Enabled = false;

            if(Program.Language == 2)
            {
                Text = "MC Virtual Server [data support, bool: x,y,m   word: x,y,m,d,w]";
                label3.Text = "port:";
                button1.Text = "Start Server";
                button11.Text = "Close Server";
                label11.Text = "This server is not a strict mc protocol and only supports perfect communication with HSL components.";
            }
        }
        
        private void FormSiemens_FormClosing( object sender, FormClosingEventArgs e )
        {
            mcNetServer?.ServerClose( );
        }

        #region Server Start

        private HslCommunication.Profinet.Melsec.MelsecMcServer mcNetServer;

        private void button1_Click( object sender, EventArgs e )
        {
            if (!int.TryParse( textBox2.Text, out int port ))
            {
                MessageBox.Show( DemoUtils.PortInputWrong );
                return;
            }

            try
            {
                mcNetServer = new HslCommunication.Profinet.Melsec.MelsecMcServer( checkBox2.Checked );                       // 实例化对象
                mcNetServer.OnDataReceived += MelsecMcServer_OnDataReceived;
                mcNetServer.ServerStart( port );
                userControlReadWriteServer1.SetReadWriteServer( mcNetServer, "D100" );

                button1.Enabled = false;
                panel2.Enabled = true;
                button11.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message );
            }
        }

        private void button11_Click( object sender, EventArgs e )
        {
            // 停止服务
            mcNetServer?.ServerClose( );
            button1.Enabled = true;
            button11.Enabled = false;
        }

        private void MelsecMcServer_OnDataReceived( object sender, byte[] receive )
        {
            // 可以进行一些操作
        }

        #endregion
    }
}
