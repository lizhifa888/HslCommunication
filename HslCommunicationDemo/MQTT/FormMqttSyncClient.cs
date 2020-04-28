﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HslCommunication.MQTT;
using HslCommunication;

namespace HslCommunicationDemo
{
	#region FormMqttSyncClient


	public partial class FormMqttSyncClient : HslFormContent
	{
		public FormMqttSyncClient( )
		{
			InitializeComponent( );
		}

		private void FormClient_Load( object sender, EventArgs e )
		{
			panel2.Enabled = false;
			button2.Enabled = false;

			

			Language( Program.Language );
		}

		private void Language( int language )
		{
			if (language == 1)
			{
				Text = "Mqtt同步客户端";
				label1.Text = "Ip地址：";
				label3.Text = "端口号：";
				button1.Text = "连接";
				button2.Text = "断开连接";
				label7.Text = "Topic：";
				label8.Text = "主题";
				label9.Text = "Payload：";
				button4.Text = "清空";
				label12.Text = "接收：";
			}
			else
			{
				Text = "Mqtt Sync Client Test";
				label1.Text = "Ip:";
				label3.Text = "Port:";
				button1.Text = "Connect";
				button2.Text = "Disconnect";
				label7.Text = "Topic:";
				label8.Text = "";
				label9.Text = "Payload:";
				button3.Text = "most one";
				button4.Text = "Clear";
				label12.Text = "Receive:";
				button3.Text = "Read";
			}
		}

		private MqttSyncClient mqttSyncClient;

		private async void button1_Click( object sender, EventArgs e )
		{
			// 连接
			MqttConnectionOptions options = new MqttConnectionOptions( )
			{
				IpAddress = textBox1.Text,
				Port = int.Parse( textBox2.Text ),
				ClientId = textBox3.Text,
			};
			if(!string.IsNullOrEmpty(textBox9.Text) || !string.IsNullOrEmpty( textBox10.Text ))
			{
				options.Credentials = new MqttCredential( textBox9.Text, textBox10.Text );
			}

			button1.Enabled = false;
			mqttSyncClient = new MqttSyncClient( options );
			OperateResult connect = await mqttSyncClient.ConnectServerAsync( );

			if(connect.IsSuccess)
			{
				panel2.Enabled = true;
				button1.Enabled = false;
				button2.Enabled = true;
				panel2.Enabled = true;
				MessageBox.Show( StringResources.Language.ConnectServerSuccess );
			}
			else
			{
				button1.Enabled = true;
				MessageBox.Show( connect.Message );
			}
		}

		private void button2_Click( object sender, EventArgs e )
		{
			// 断开连接
			button1.Enabled = true;
			button2.Enabled = false;
			panel2.Enabled = false;

			mqttSyncClient.ConnectClose( );
		}

		private async void button3_Click( object sender, EventArgs e )
		{
			DateTime start = DateTime.Now;
			button3.Enabled = false;
			OperateResult<string, byte[]> read = await mqttSyncClient.ReadAsync( textBox5.Text, Encoding.UTF8.GetBytes( textBox4.Text ) );
			button3.Enabled = true;

			textBox7.Text = (int)(DateTime.Now - start).TotalMilliseconds + " ms";
			if (!read.IsSuccess) { MessageBox.Show( "Rend Failed:" + read.Message ); return; }

			textBox6.Text = read.Content1;
			string msg = Encoding.UTF8.GetString( read.Content2 );
			if (radioButton4.Checked)
			{
				try
				{
					msg = System.Xml.Linq.XElement.Parse( msg ).ToString( );
				}
				catch
				{

				}
			}
			else if (radioButton5.Checked)
			{
				try
				{
					msg = Newtonsoft.Json.Linq.JObject.Parse( msg ).ToString( );
				}
				catch
				{

				}
			}

			textBox8.Text = msg;
		}


		private void button4_Click( object sender, EventArgs e )
		{
			// 清空
			textBox8.Clear( );
		}

	}


	#endregion
}
