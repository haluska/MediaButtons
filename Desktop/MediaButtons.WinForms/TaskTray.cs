using NAudio.CoreAudioApi;
using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace MediaButtons
{
	public class TaskTray : ApplicationContext
	{
		private string _portName;
		private SerialPort _port = new SerialPort();
		private NotifyIcon notifyIcon = new NotifyIcon();
		private MenuItem menuItemStart;
		private MenuItem menuItemStop;
		private MenuItem menuItemConfig = new MenuItem();
		private MenuItem menuItemExit = new MenuItem();
		private static MMDeviceEnumerator enumer = new MMDeviceEnumerator();
		private MMDevice Device = enumer.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
		Configuration configuration = new Configuration();

		public TaskTray()
		{
			Application.ApplicationExit += new EventHandler(this.OnApplicationExit);

			menuItemStart = new MenuItem("Run", new EventHandler(menuItemToggle_Click));
			menuItemStop = new MenuItem("Stop", new EventHandler(menuItemToggle_Click));
			menuItemConfig = new MenuItem("Configuration", new EventHandler(ShowConfig));
			menuItemExit = new MenuItem("Exit", new EventHandler(Exit));

			notifyIcon.Icon = MediaButtons.Properties.Resources.AppIcon;
			notifyIcon.DoubleClick += new EventHandler(notifyIcon_DoubleClick);
			notifyIcon.ContextMenu = new ContextMenu(new MenuItem[] { menuItemStart, menuItemStop, new MenuItem("-"), menuItemConfig, menuItemExit });
			notifyIcon.Visible = true;

			ToggleCommunications();
		}

		private void menuItemToggle_Click(object sender, EventArgs e)
		{
			ToggleCommunications();
		}

		private void ToggleCommunications()
		{

			if (_port.IsOpen)
			{
				CloseConnection();
			}
			else
			{
				if (OpenPort())
				{
					UpdateVolume();
					Device.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
					menuItemStart.Text = string.Format("Running on {0}", _portName);
					menuItemStart.Checked = true;
					menuItemStop.Text = "Click to Stop";
					menuItemStop.Checked = false;
					notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
					notifyIcon.BalloonTipTitle = "Talking to MediaButtons";
					notifyIcon.BalloonTipText = "Double click the tray icon to change this.";
					notifyIcon.ShowBalloonTip(2000);
				}
				else
				{
					menuItemStart.Text = "Connection Error";
					menuItemStart.Checked = false;
					menuItemStop.Text = "Stopped";
					menuItemStop.Checked = true;
					notifyIcon.BalloonTipIcon = ToolTipIcon.Error;
					notifyIcon.BalloonTipTitle = "Connection Error";
					notifyIcon.BalloonTipText = "There was an error talking to MediaButtons. Is it plugged in? Double click the tray icon to try again.";
					notifyIcon.ShowBalloonTip(2000);
				}
			}

		}

		private void CloseConnection()
		{
			try
			{
				_port.Close();
			}
			catch { }
			Device.AudioEndpointVolume.OnVolumeNotification -= AudioEndpointVolume_OnVolumeNotification;
			menuItemStart.Text = "Click to Start";
			menuItemStart.Checked = false;
			menuItemStop.Text = "Stopped";
			menuItemStop.Checked = true;
			notifyIcon.BalloonTipIcon = ToolTipIcon.Warning;
			notifyIcon.BalloonTipTitle = "Stopped talking to MediaButtons";
			notifyIcon.BalloonTipText = "Double click the tray icon to try again.";
			notifyIcon.ShowBalloonTip(2000);
		}

		private bool OpenPort()
		{
			var returnValue = false;
			if (MediaButtons.Properties.Settings.Default.PortName == "")
			{
				ViewConfiguration();
			}
			try
			{
				_portName = MediaButtons.Properties.Settings.Default.PortName;
				_port = new SerialPort(_portName, 9600);
				_port.Open();
				returnValue = true;
			}
			catch (Exception ex)
			{

			}
			return returnValue;
		}

		void notifyIcon_DoubleClick(object sender, EventArgs e)
		{
			ToggleCommunications();
		}

		void ShowConfig(object sender, EventArgs e)
		{
			ViewConfiguration();
		}

		private void ViewConfiguration()
		{
			if (configuration.Visible)
				configuration.Focus();
			else
				configuration.ShowDialog();
		}

		void AudioEndpointVolume_OnVolumeNotification(AudioVolumeNotificationData data)
		{
			UpdateVolume();
		}

		private void UpdateVolume()
		{
			int volume = (int)Math.Round(Device.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
			// < = begining of data
			// > = end of data
			// | = data delimiter
			// Arduino code is initially set up to receive 3 pieces data: a string, an int, a float
			// Currently ignoring the float... this was done just for fun.
			try
			{
				_port.Write("<Vol: |" + (volume.ToString()) + ">");
			}
			catch (System.InvalidOperationException iex)
			{
				if (iex.Message == "The port is closed.")
				{
					CloseConnection();
				}
			}
		}

		void Exit(object sender, EventArgs e)
		{
			// We must manually tidy up and remove the icon before we exit.
			// Otherwise it will be left behind until the user mouses over.
			notifyIcon.Visible = false;

			Application.Exit();
		}
		private void OnApplicationExit(object sender, EventArgs e)
		{
			if (_port != null)
			{
				if (_port.IsOpen) _port.Close();
				_port.Dispose();
			}
		}
	}
}
