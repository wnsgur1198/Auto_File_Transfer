using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using ListBox = System.Windows.Controls.ListBox;
using MessageBox = System.Windows.MessageBox;

namespace Auto_File_Transfer_Program
{
    public partial class NowSetWindow : Window
    {
        private List<string> _nowNameList = null;
        private List<string> _nowKeyList = null;
        private List<string> _pathList = null;
        private List<string> _regular_work_folderList = null;

        private ListBox nowName_lb = null;
        private ListBox nowKey_lb = null;
        private ListBox path_lb = null;

        private int _nowHour = 0;
        private int _nowMin = 0;

        private string select_path_Today;// = @"C:\Users\user\Desktop\Test_Folder";
        private string select_path_Now;// = @"C:\Users\user\Desktop\Test_Folder\now";
        private string select_path_Temp;// = @"C:\Users\user\Desktop\Test_Folder\temp";
        private string select_send_path;// = "";        
        private string _backupPath;// = @"C:\Users\user\Desktop\Test_Folder\backup";
        private string _logPath;// = @"C:\Users\user\Desktop\Test_Folder\log";

        private bool autoRunningFlag = false;
        private bool autoCancelFlag = false;

        private DatabaseWorker dbworker = null;
        private AutoRun autoRun = null;
        private ProgressProcess process = null;
        private NormalSetWindow normalSet = null;

        public NowSetWindow(ProgressProcess _process)
        {
            InitializeComponent();

            // 임의 설정----------------------------
            //tempPath.Text = select_path_Temp;
            //todayPath.Text = select_path_Today;
            //nowPath.Text = select_path_Now;
            //backupPath.Text = _backupPath;
            //logPath.Text = _logPath;

            _nowNameList = new List<string>();
            _nowKeyList = new List<string>();
            _pathList = new List<string>();
            _regular_work_folderList = new List<string>();

            autoRun = new AutoRun();
            process = _process;

            HourMin_Initialize();

            input_nowName.Focus();
            nowNameList.SelectionChanged += new SelectionChangedEventHandler(NowNameList_Selected);
            nowKeyList.SelectionChanged += new SelectionChangedEventHandler(NowKeyList_Selected);
            pathList.SelectionChanged += new SelectionChangedEventHandler(PathList_Selected);
                       
        }

        public List<string> GetNameList() { return _nowNameList; }
        public List<string> GetKeyList() { return _nowKeyList; }
        public List<string> GetPathList() { return _pathList; }

        public void SetDBWorker(DatabaseWorker _dbworker)
        {
            dbworker = _dbworker;
            DBLink_NowSet();
        }

        private void DBLink_NowSet()
        {
            LocalDB load_db = dbworker.LoadTable_Now();

            if (load_db.HasRows)
            {
                while (load_db.Read())
                {
                    for (int i = 0; i < load_db.FieldCount; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                _nowNameList.Add(load_db.GetData(i).ToString());
                                break;
                            case 1:
                                _nowKeyList.Add(load_db.GetData(i).ToString());
                                break;
                            case 2:
                                _pathList.Add(load_db.GetData(i).ToString());
                                break;
                            case 3:
                                string[] splitPath = load_db.GetData(i).ToString().Split('@');
                                foreach (string s in splitPath) { _regular_work_folderList.Add(s); }
                                break;
                            case 4:
                                select_path_Temp = load_db.GetData(i).ToString();
                                break;
                            case 5:
                                select_path_Now = load_db.GetData(i).ToString();
                                break;
                            case 6:
                                select_path_Today = load_db.GetData(i).ToString();
                                break;
                            case 7:
                                string[] spltTime = load_db.GetData(i).ToString().Split(':');
                                _nowHour = int.Parse(spltTime[0]);
                                _nowMin = int.Parse(spltTime[1]);
                                break;
                        }
                    }
                }
            }

            Refresh_KeywordList();
            Refresh_folderList();


            LocalDB load_db2 = dbworker.LoadTable_ETC();

            if (load_db2.HasRows)
            {
                while (load_db2.Read())
                {
                    for (int i = 0; i < load_db2.FieldCount; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                _backupPath = load_db2.GetData(i).ToString();
                                break;
                            case 1:
                                _logPath = load_db2.GetData(i).ToString();
                                break;
                        }
                    }
                }
            }
            backupPath.Text = _backupPath;
            logPath.Text = _logPath;
        }

        private void HourMin_Initialize()
        {
            for (int i = 0; i < 24; i++)
            {
                nowSet_hour.Items.Add(i.ToString());
            }
            nowSet_hour.SelectedIndex = 0;

            for (int i = 0; i < 60; i += 10)
            {
                nowSet_min.Items.Add(i.ToString());
            }
            nowSet_min.SelectedIndex = 0;
        }

        private void Keyword_Add_Button_Click(object sender, RoutedEventArgs e)
        {
            input_nowName.Focus();

            if (!input_nowName.Text.Equals("") || !input_nowKey.Text.Equals(""))
            {
                for (int i = 0; i < _nowNameList.Count; i++)
                {
                    if (_nowNameList[i].Equals(input_nowName.Text) || _nowKeyList[i].Equals(input_nowKey.Text))
                    {
                        MessageBox.Show("중복된 설정입니다.");
                        return;
                    }
                }
                _nowNameList.Add(input_nowName.Text);
                input_nowName.Text = "";

                _nowKeyList.Add(input_nowKey.Text);
                input_nowKey.Text = "";

                _pathList.Add(input_path.Text);
                input_path.Text = "";

            }
            else { MessageBox.Show("공백은 불가합니다."); }

            Refresh_KeywordList();
        }

        private void Refresh_KeywordList()
        {
            nowNameList.Items.Clear();
            nowKeyList.Items.Clear();
            pathList.Items.Clear();

            for (int i = 0; i < _nowNameList.Count; i++)
            {
                nowNameList.Items.Add(_nowNameList[i]);
                nowKeyList.Items.Add(_nowKeyList[i]);
                pathList.Items.Add(_pathList[i]);
            }
        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { Keyword_Add_Button_Click(sender, e); }
        }

        private void NowNameList_Selected(object sender, RoutedEventArgs e)
        {
            nowName_lb = (ListBox)e.Source;
            int selectIndex = nowName_lb.SelectedIndex;
            nowKeyList.SelectedIndex = selectIndex;
        }

        private void NowKeyList_Selected(object sender, RoutedEventArgs e)
        {
            nowKey_lb = (ListBox)e.Source;
            int selectIndex = nowKey_lb.SelectedIndex;
            nowNameList.SelectedIndex = selectIndex;
        }

        private void PathList_Selected(object sender, SelectionChangedEventArgs e)
        {
            path_lb = (ListBox)e.Source;
            int selectIndex = path_lb.SelectedIndex;
            pathList.SelectedIndex = selectIndex;
        }


        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            select_send_path = dialog.SelectedPath;

            if (!select_send_path.Equals("")) { input_path.Text = select_send_path; }

            input_path.Focus();
        }

        private void Keyword_Remove_Button_Click(object sender, RoutedEventArgs e)
        {
            int selectIndex = nowNameList.SelectedIndex;

            if (selectIndex > -1)
            {
                if (nowNameList.Items.Count > 0 || _nowNameList.Count > 0)
                {
                    nowNameList.Items.RemoveAt(selectIndex);
                    _nowNameList.RemoveAt(selectIndex);

                    nowKeyList.Items.RemoveAt(selectIndex);
                    _nowKeyList.RemoveAt(selectIndex);

                    pathList.Items.RemoveAt(selectIndex);
                    _pathList.RemoveAt(selectIndex);

                    Refresh_KeywordList();
                }
                else { return; }
            }
        }
        private void Refresh_folderList()
        {
            regular_work_folderList.Items.Clear();

            for (int i = 0; i < _regular_work_folderList.Count; i++)
            {
                regular_work_folderList.Items.Add(_regular_work_folderList[i]);
            }
        }

        private void AddPath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            string select_path = dialog.SelectedPath;

            if (!select_path.Equals("")) { _regular_work_folderList.Add(select_path); }

            Refresh_folderList();
        }

        private void DelPath_Click(object sender, RoutedEventArgs e)
        {
            int selectIndex = regular_work_folderList.SelectedIndex;
            if (selectIndex > -1)
            {
                if (regular_work_folderList.Items.Count > 0 || _regular_work_folderList.Count > 0)
                {
                    regular_work_folderList.Items.RemoveAt(selectIndex);
                    _regular_work_folderList.RemoveAt(selectIndex);

                    Refresh_folderList();
                }
                else { return; }
            }
        }

        private void TodayPath_Set_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            select_path_Today = dialog.SelectedPath;

            if (!select_path_Today.Equals("")) { todayPath.Text = select_path_Today; }
        }

        private void NowPath_Set_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            select_path_Now = dialog.SelectedPath;

            if (!select_path_Now.Equals("")) { nowPath.Text = select_path_Now; }
        }

        private void TempPath_Set_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            select_path_Temp = dialog.SelectedPath;

            if (!select_path_Temp.Equals("")) { tempPath.Text = select_path_Temp; }
        }

        private void NowSet_apply_Click(object sender, RoutedEventArgs e)
        {
            _nowHour = nowSet_hour.SelectedIndex;
            _nowMin = nowSet_min.SelectedIndex;

            process.SetNowSetting(_nowNameList, _nowKeyList, _pathList, _regular_work_folderList,
                                        tempPath.Text, nowPath.Text, todayPath.Text, _nowHour, _nowMin, _backupPath, _logPath);
            process.NowIntialize();

            if (autoRunningFlag == true)
            {
                autoRun.autoRunning();
            }

            if (autoCancelFlag == true)
            {
                autoRun.autoCancel();
            }

            Visibility = Visibility.Hidden;
        }

        private void NowSet_cancel_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
        }

        public void Exist_nowSet()
        {
            Refresh_KeywordList();
            Refresh_folderList();
            nowSet_hour.SelectedIndex = _nowHour;
            nowSet_min.SelectedIndex = _nowMin;
            tempPath.Text = select_path_Temp;
            todayPath.Text = select_path_Today;
            nowPath.Text = select_path_Now;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {            
            Visibility = Visibility.Hidden;
        }

        private void NowNameList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            nowKeyList.SelectedIndex = nowNameList.SelectedIndex;
            pathList.SelectedIndex = nowNameList.SelectedIndex;
        }

        private void NowKeyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            nowNameList.SelectedIndex = nowKeyList.SelectedIndex;
            pathList.SelectedIndex = nowKeyList.SelectedIndex;
        }

        private void PathList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            nowKeyList.SelectedIndex = pathList.SelectedIndex;
            nowNameList.SelectedIndex = pathList.SelectedIndex;
        }

        private void BackupPath_btn_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            _backupPath = dialog.SelectedPath;

            if (!_backupPath.Equals("")) { backupPath.Text = _backupPath; }
        }

        private void LogPath_btn_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            _logPath = dialog.SelectedPath;

            if (!_logPath.Equals("")) { logPath.Text = _logPath; }
        }

        private void AutoRunning_Set_Click(object sender, RoutedEventArgs e)
        {
            autoRunningFlag = true;
            autoCancelFlag = false;

            MessageBox.Show("자동실행 설정");
        }

        private void AutoCanel_Set_Click(object sender, RoutedEventArgs e)
        {
            autoRunningFlag = false;
            autoCancelFlag = true;

            MessageBox.Show("자동실행 취소");
        }
        
        public void SetNormalSet(NormalSetWindow _normalSet) { normalSet = _normalSet; }

        private void DB_Initialize_Normal_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dr = MessageBox.Show("초기화하시겠습니까?", "", MessageBoxButton.OKCancel);

            if (dr == MessageBoxResult.OK)
            {
                dbworker.DeleteTable("Normal_Table");
                normalSet.Initialize_NormalTable();
                normalSet.DBLink_NormalSet(DateTime.Now);
                MessageBox.Show("초기화 완료");
                Visibility = Visibility.Hidden;
            }
            else { return; }            
        }

        private void DB_Initialize_Now_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dr = MessageBox.Show("초기화하시겠습니까?", "", MessageBoxButton.OKCancel);

            if (dr == MessageBoxResult.OK)
            {
                dbworker.DeleteTable("Now_Table");
                dbworker.DeleteTable("ETC_Table");

                Refresh_KeywordList();
                Refresh_folderList();
                nowSet_hour.SelectedIndex = 0;
                nowSet_min.SelectedIndex = 0;
                tempPath.Text = "";
                todayPath.Text = "";
                nowPath.Text = "";
                MessageBox.Show("초기화 완료");
                Visibility = Visibility.Hidden;
            }
            else { return; }
        }
    }
}