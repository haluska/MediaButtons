using System;
using System.Threading;
using System.Windows.Forms;

namespace MediaButtons
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			using (Mutex mutex = new Mutex(false, "MediaButtons"))
			{
				if (!mutex.WaitOne(0, true))
				{
					MessageBox.Show("Unable to run multiple instances of this program.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				else
				{
					if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();
					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);
					//Instead of running a form, we are running an ApplicationContext:
					Application.Run(new TaskTray());
				}
			}
		}

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern bool SetProcessDPIAware();
	}
}