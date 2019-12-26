using System;
using System.Windows.Forms;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace Auto_File_Transfer_Program
{
    public class SystemTraySet
    {
        private NotifyIcon notify = null;
        private ContextMenu menu = null;
        private MainWindow mainwindow = null;

        public SystemTraySet()
        {
            notify = new NotifyIcon();
            menu = new ContextMenu();

            try
            {
                MenuItem item1 = new MenuItem();
                MenuItem item2 = new MenuItem();

                menu.MenuItems.Add(item1);
                menu.MenuItems.Add(item2);

                item1.Index = 0;
                item1.Text = "열기";
                item1.Click += OpenProg;

                item2.Index = 1;
                item2.Text = "종료";
                item2.Click += ExitProg;
               
                notify.Icon = Properties.Resources.icon;
                notify.Visible = true;
                notify.DoubleClick += OpenProg;
                notify.ContextMenu = menu;
                notify.Text = "Auto File Transfer Program :)";
            }
            catch (Exception ex) { MessageBox.Show(ex.StackTrace); }
        }

        public void SetMainwindow(MainWindow _mainwindow)
        {
            mainwindow = _mainwindow;
        }

        private void OpenProg(object click, EventArgs eClick)
        {
            mainwindow.WindowState = WindowState.Maximized;            
            mainwindow.Show();
            mainwindow.WindowState = WindowState.Normal;
        }

        private void ExitProg(object click, EventArgs eClick)
        {
            trayClose();
            Environment.Exit(0);           
        }

        public void tooltip_show(string str)
        {
            notify.BalloonTipIcon = ToolTipIcon.Info;
            notify.BalloonTipText = str;
            notify.ShowBalloonTip(5);
        }

        public void trayClose()
        {
            notify.Visible = false;
            notify.Icon = null;
            notify.Dispose();
        }

    }
}
