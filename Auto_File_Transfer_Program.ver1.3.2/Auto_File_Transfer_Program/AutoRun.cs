using Microsoft.Win32;
using System.Windows.Forms;

namespace Auto_File_Transfer_Program
{
    class AutoRun
    {
        public AutoRun() {  }

        public void autoRunning()
        {
            try
            {
                string runKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

                RegistryKey strUpKey = Registry.LocalMachine.OpenSubKey(runKey);

                if (strUpKey.GetValue("AFTprog") == null)
                {
                    strUpKey.Close();
                    strUpKey = Registry.LocalMachine.OpenSubKey(runKey, true);
                    strUpKey.SetValue("AFTprog", Application.ExecutablePath);
                }
            }
            catch { }
        }

        public void autoCancel()
        {
            try
            {
                string runKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

                RegistryKey strUpKey = Registry.LocalMachine.OpenSubKey(runKey, true);

                strUpKey.DeleteValue("AFTprog");
            }
            catch { }
        }

    }
}
