using System;
using System.Management;
using System.Windows.Forms;

namespace MediaButtons
{
	public partial class Configuration : Form
	{
		public Configuration()
		{
			InitializeComponent();
		}

		private void Configuration_Load(object sender, EventArgs e)
		{
			LoadData();
			FindThePort();
		}

		private void LoadData()
		{
			//showMessageCheckBox.Checked = MediaButtons.Properties.Settings.Default.ShowMessage;
			txtPort.Text = MediaButtons.Properties.Settings.Default.PortName;
		}

		private void SaveSettings(object sender, FormClosingEventArgs e)
		{
			// If the user clicked "Save" to close this dialog box
			if (this.DialogResult == DialogResult.OK)
			{
				MediaButtons.Properties.Settings.Default.PortName = txtPort.Text;
				//MediaButtons.Properties.Settings.Default.ShowMessage = showMessageCheckBox.Checked;
				MediaButtons.Properties.Settings.Default.Save();
			}
		}

		//void AudioEndpointVolume_OnVolumeNotification(AudioVolumeNotificationData data)
		//{
		//    // This shows data.MasterVolume, you can do whatever you want here
		//    int volume = (int)Math.Round(Device.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
		//    port.Write("<Vol: |" + (volume.ToString()) + ">");
		//}
		//void Form1_FormClosed(object sender, FormClosedEventArgs e)
		//{

		//}
		//private void button1_Click(object sender, EventArgs e)
		//{
		//    PortWrite("1");
		//}

		//private void button2_Click(object sender, EventArgs e)
		//{
		//    PortWrite("<HelloWorld| 12| 24.7>");
		//}
		private void PortWrite(string message)
		{
			//port.Write(message);
		}

		private void button3_Click(object sender, EventArgs e)
		{
			//int volume = (int)Math.Round(Device.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
			//port.Write("<Vol: |" + (volume.ToString()) + ">");
		}

		private void FindThePort()
		{
			ManagementScope connectionScope = new ManagementScope();
			SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_SerialPort");
			ManagementObjectSearcher searcher = new ManagementObjectSearcher(connectionScope, serialQuery);
			while (lvPorts.Items.Count > 0) lvPorts.Items.RemoveAt(0);
			try
			{
				foreach (ManagementObject device in searcher.Get())
				{
					var SerialPort = device["DeviceID"].ToString();
					var serialName = device["Name"].ToString();
					//"Arduino" may be in here:
					var SerialDescription = device["Description"].ToString();
					//VID and PID in here:
					var SerialPNPDevice = device["PNPDeviceID"].ToString();
					/*
                    From the boards.text file:
                    teensy31.vid.0=0x16C0
                    teensy31.vid.1=0x16C0
                    teensy31.vid.2=0x16C0
                    teensy31.vid.3=0x16C0
                    teensy31.vid.4=0x16C0
                    teensy31.pid.0=0x0483
                    teensy31.pid.1=0x0487
                    teensy31.pid.2=0x0489
                    teensy31.pid.3=0x048A
                    teensy31.pid.4=0x0476
                    */
					var DeviceType = "";
					if (SerialDescription.Contains("Arduino"))
					{
						DeviceType = "Arduino";
					}
					else if (SerialPNPDevice.Contains("16C0"))
					{
						DeviceType = "Teensy";
					}
					else
					{
						DeviceType = "Unknown";
					}

					string[] row = { SerialPort, DeviceType, SerialPNPDevice, SerialDescription };
					var listViewItem = new ListViewItem(row);
					lvPorts.Items.Add(listViewItem);
				}
			}
			catch (ManagementException e)
			{
				/* Do Nothing */
			}
		}

		private void lvPorts_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lvPorts.SelectedItems.Count < 1)
				return;
			txtPort.Text = lvPorts.SelectedItems[0].Text;
		}
	}
}