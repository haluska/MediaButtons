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
		private NotifyIcon _notifyIcon = new NotifyIcon();
		private MenuItem _menuItemStart;
		private MenuItem _menuItemStop;
		private MenuItem _menuItemConfig = new MenuItem();
		private MenuItem _menuItemExit = new MenuItem();
		private static MMDeviceEnumerator _deviceEnumerator = new MMDeviceEnumerator();
		private MMDevice _device = _deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
		private Configuration _configuration = new Configuration();

		public TaskTray()
		{
			Application.ApplicationExit += new EventHandler(this.OnApplicationExit);

			_menuItemStart = new MenuItem("Run", new EventHandler(menuItemToggle_Click));
			_menuItemStop = new MenuItem("Stop", new EventHandler(menuItemToggle_Click));
			_menuItemConfig = new MenuItem("Configuration", new EventHandler(ShowConfig));
			_menuItemExit = new MenuItem("Exit", new EventHandler(Exit));

			_notifyIcon.Icon = MediaButtons.Properties.Resources.AppIcon;
			_notifyIcon.DoubleClick += new EventHandler(notifyIcon_DoubleClick);
			_notifyIcon.ContextMenu = new ContextMenu(new MenuItem[] { _menuItemStart, _menuItemStop, new MenuItem("-"), _menuItemConfig, _menuItemExit });
			_notifyIcon.Visible = true;

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
					_device.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
					_menuItemStart.Text = string.Format("Running on {0}", _portName);
					_menuItemStart.Checked = true;
					_menuItemStop.Text = "Click to Stop";
					_menuItemStop.Checked = false;
					_notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
					_notifyIcon.BalloonTipTitle = "Talking to MediaButtons";
					_notifyIcon.BalloonTipText = "Double click the tray icon to change this or right click the icon to open the menu.";
					_notifyIcon.ShowBalloonTip(2000);
				}
				else
				{
					_menuItemStart.Text = "Connection Error";
					_menuItemStart.Checked = false;
					_menuItemStop.Text = "Stopped";
					_menuItemStop.Checked = true;
					_notifyIcon.BalloonTipIcon = ToolTipIcon.Error;
					_notifyIcon.BalloonTipTitle = "Connection Error";
					_notifyIcon.BalloonTipText = "There was an error talking to MediaButtons. Is it plugged in? Right click on the icon to open the menu.";
					_notifyIcon.ShowBalloonTip(2000);
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
			_device.AudioEndpointVolume.OnVolumeNotification -= AudioEndpointVolume_OnVolumeNotification;
			_menuItemStart.Text = "Click to Start";
			_menuItemStart.Checked = false;
			_menuItemStop.Text = "Stopped";
			_menuItemStop.Checked = true;
			_notifyIcon.BalloonTipIcon = ToolTipIcon.Warning;
			_notifyIcon.BalloonTipTitle = "Stopped talking to MediaButtons";
			_notifyIcon.BalloonTipText = "Double click the icon to start it up again.";
			_notifyIcon.ShowBalloonTip(2000);
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
			if (_configuration.Visible)
				_configuration.Focus();
			else
				_configuration.ShowDialog();
		}

		void AudioEndpointVolume_OnVolumeNotification(AudioVolumeNotificationData data)
		{
			UpdateVolume();
		}

		private void UpdateVolume()
		{
			int volume = (int)Math.Round(_device.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
			
			// < = beginning of data
			// > = end of data
			// | = data delimiter
			try
			{
				string temp = "<";
				temp += volume.ToString();
				temp += "|";
				temp += GetMuteStatus();
				temp += "|";
				temp += System.Net.Dns.GetHostName();
				temp += ">";
				_port.Write(temp);
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
			//We must manually tidy up and remove the icon before we exit.
			//Otherwise it will be left behind until the user mouses over.
			_notifyIcon.Visible = false;
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

		private string GetMuteStatus()
		{
			if (_device.AudioEndpointVolume.Mute) return "MUTE";
			//The Arduino does better with a space vs an empty string.
			return " ";
		}
	}
}
