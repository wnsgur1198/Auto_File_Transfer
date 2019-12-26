using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace Auto_File_Transfer_Program
{
    public class ProgressProcess
    {
        private DispatcherTimer timer = null;
        private string[] day_of_week_Eng = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        private int[] identifier_Minute = { 0, 10, 20, 30, 40, 50 };

        // now/today 설정변수
        private List<string> do_nowNameList = new List<string>();
        private List<string> do_nowKeyList = new List<string>();
        private List<string> do_regular_work_folderList = new List<string>();
        private string do_tempPath = null;
        private string do_nowPath = null;
        private string do_todayPath = null;
        private int do_hour = 0;
        private int do_min = 0;
        private string do_backupPath = null;
        private string do_logPath = null;

        // normal 설정변수
        private List<string> do_schedulePath = new List<string>();
        private List<string> do_scheduleDay_sun = new List<string>();
        private List<string> do_scheduleDay_mon = new List<string>();
        private List<string> do_scheduleDay_tue = new List<string>();
        private List<string> do_scheduleDay_wed = new List<string>();
        private List<string> do_scheduleDay_thr = new List<string>();
        private List<string> do_scheduleDay_fri = new List<string>();
        private List<string> do_scheduleDay_sat = new List<string>();

        private string appendStr = null;

        private DatabaseWorker dbworker = null;
        public SystemTraySet trayset = null;
        private NormalSetWindow normalset = null;

        public ProgressProcess(SystemTraySet _trayset)
        {
            trayset = _trayset;

            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1000) };
            timer.Tick += new EventHandler(DoNowSetting);        
            timer.Tick += new EventHandler(DoNormalSet);
            timer.Tick += new EventHandler(UpdateDatabase);
            timer.Start();
        }

        private void UpdateDatabase(object sender, EventArgs e)
        {
            DateTime presentdate = DateTime.Now;
            if (presentdate.Month == 1 && presentdate.Day == 1 && presentdate.Hour == 0 && presentdate.Minute == 0 && presentdate.Second == 0)
            {
                // 작년꺼 마지막 주 빼고 삭제
                Log("DB 갱신 진행 중");
                trayset.tooltip_show("DB 갱신 진행 중");

                dbworker.DeleteTable_WithCondition_Without_MonthDay("Normal_Table", presentdate.Year - 2);

                for (int _month = 1; _month < 12; _month++)
                {
                    dbworker.DeleteTable_WithCondition_WithoutDay("Normal_Table", presentdate.Year - 1, _month);
                }

                int limit = 31;
                switch (presentdate.DayOfWeek.ToString())
                {
                    case "Sunday":
                        limit = 31;
                        break;
                    case "Monday":
                        limit = 30;
                        break;
                    case "Tuesday":
                        limit = 29;
                        break;
                    case "Wednesday":
                        limit = 28;
                        break;
                    case "Thursday":
                        limit = 27;
                        break;
                    case "Friday":
                        limit = 26;
                        break;
                    case "Saturday":
                        limit = 25;
                        break;
                }
                for (int _day = 1; _day <= limit; _day++)
                {
                    dbworker.DeleteTable_WithCondition("Normal_Table", presentdate.Year - 1, 12, _day);
                }
                //---------------------------------------------------


                // 내년꺼 생성, 내후년 첫째주까지
                DateTime date = new DateTime(presentdate.Year, 12, 31);
                int year = presentdate.Year + 1; int month = 1; int day = 1;
                int hour = 0; int min = 0;

                switch (date.DayOfWeek.ToString())
                {
                    case "Sunday":
                        day = 7;
                        break;
                    case "Monday":
                        day = 6;
                        break;
                    case "Tuesday":
                        day = 5;
                        break;
                    case "Wednesday":
                        day = 4;
                        break;
                    case "Thursday":
                        day = 3;
                        break;
                    case "Friday":
                        day = 2;
                        break;
                    case "Saturday":
                        day = 1;
                        break;
                }

                for (int i = 0; i < 12; i++)         //------------------------------------ 월
                {                   
                    int dayLimit;

                    switch (month)
                    {
                        case 1:
                        case 3:
                        case 5:
                        case 7:
                        case 8:
                        case 10:
                        case 12:
                            dayLimit = 31;
                            break;
                        case 2:
                            dayLimit = 28;
                            if (year % 4 == 0) dayLimit = 29;
                            break;
                        default:
                            dayLimit = 30;
                            break;
                    }

                    for (int j = 0; j < dayLimit; j++)                                      // 일
                    {
                        for (int k = 0; k < 144; k++)    // 144 = 24(시) * 6(10분간격)           // 시분
                        {
                            string queryStr = string.Format("{0}, {1}, {2}, {3}, {4}, '{5}', '{6}'",
                                year, month, day, hour, min, "", "");

                            dbworker.InsertTable("Normal_Table", queryStr);

                            if (min == 50)
                            {
                                hour++;
                                min = 0;
                            }
                            else
                            {
                                min += 10;
                            }
                        }
                        hour = 0;
                        day++;
                    }

                    if (month == 12 && day == 32)                // 그 해 마지막 주
                    {
                        DateTime newdate = new DateTime(year, 12, 31);
                        int _limit = 2;
                        switch (newdate.DayOfWeek.ToString())
                        {
                            case "Sunday":
                                _limit = 6;
                                break;
                            case "Monday":
                                _limit = 5;
                                break;
                            case "Tuesday":
                                _limit = 4;
                                break;
                            case "Wednesday":
                                _limit = 3;
                                break;
                            case "Thursday":
                                _limit = 2;
                                break;
                            case "Friday":
                                _limit = 1;
                                break;
                            case "Saturday":
                                _limit = 0;
                                break;
                        }

                        for (int l = 1; l <= _limit; l++)
                        {
                            for (int k = 0; k < 144; k++)    // 144 = 24(시) * 6(10분간격)           // 시분
                            {
                                string queryStr = string.Format("{0}, {1}, {2}, {3}, {4}, '{5}', '{6}'",
                                                                newdate.Year + 1, 1, l, hour, min, "", "");
                                dbworker.InsertTable("Normal_Table", queryStr);

                                if (min == 50)
                                {
                                    hour++;
                                    min = 0;
                                }
                                else
                                {
                                    min += 10;
                                }
                            }
                            hour = 0;
                        }
                    }//---------------------------------------------

                    day = 1;
                    month++;
                }

                Log("DB 갱신 진행 완료");
                trayset.tooltip_show("DB 갱신 진행 완료");
                MessageBox.Show("DB 갱신을 완료하였습니다.\n내후년 설정은 새로 지정해야합니다.");
            }
            
        }

        private void Backup(string path)
        {
            if (Directory.Exists(path))
            {
                DirectoryCopy(path, do_backupPath + "\\" + DateTime.Now.ToString("MMdd"), true);
            }
        }

        private void Log(string comment)
        {
            if (do_logPath == null || do_logPath.Equals("")) return;

            if (!Directory.Exists(do_logPath)) { Directory.CreateDirectory(do_logPath); }

            string savePath = do_logPath + "\\log" + ".txt";
            string textValue = DateTime.Now.ToString("MM-dd/hh:mm") + " >> " + comment + "\r\n\r\n";

            if (!File.Exists(savePath))
            {
                File.WriteAllText(savePath, textValue, Encoding.UTF8);
            }
            else
            {
                File.AppendAllText(savePath, textValue, Encoding.UTF8);
            }
        }

        public void SetDBWorker(DatabaseWorker _dbworker)
        {
            dbworker = _dbworker;

            LinkNowSet();    // DB 연동
        }

        public void SetNormalset(NormalSetWindow _normalset)
        {
            normalset = _normalset;
        }

        private void LinkNowSet()
        {
            if(dbworker != null)
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
                                    do_nowNameList.Add(load_db.GetData(i).ToString());
                                    break;
                                case 1:
                                    do_nowKeyList.Add(load_db.GetData(i).ToString());
                                    break;
                                case 2:
                                    do_schedulePath.Add(load_db.GetData(i).ToString());
                                    break;
                                case 3:
                                    string[] splitPath = load_db.GetData(i).ToString().Split('@');
                                    foreach (string s in splitPath) { do_regular_work_folderList.Add(s); }
                                    break;
                                case 4:
                                    do_tempPath = load_db.GetData(i).ToString();
                                    break;
                                case 5:
                                    do_nowPath = load_db.GetData(i).ToString();
                                    break;
                                case 6:
                                    do_todayPath = load_db.GetData(i).ToString();
                                    break;
                                case 7:
                                    string[] spltTime = load_db.GetData(i).ToString().Split(':');
                                    do_hour = int.Parse(spltTime[0]);
                                    do_min = int.Parse(spltTime[1]);
                                    break;
                            }
                        }
                    }
                }

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
                                    do_backupPath = load_db2.GetData(i).ToString();
                                    break;
                                case 1:
                                    do_logPath = load_db2.GetData(i).ToString();
                                    break;
                            }

                        }
                    }
                }

                if (load_db.HasRows && load_db2.HasRows)
                {
                    NowIntialize();
                    trayset.tooltip_show("NowSet-DB연동 완료");
                }
            }
            else { MessageBox.Show("DB Error"); }

        }

        public void NowIntialize()
        {
            // today폴더 생성 및 삭제
            string regular_work_folder_path = do_todayPath + "\\today";
            DirectoryInfo di = new DirectoryInfo(regular_work_folder_path);
            if (di.Exists == true) { di.Delete(true); }
            di.Create();
           
            // now폴더 초기화
            Backup(do_nowPath);
            Log("now 백업 완료");

            di = new DirectoryInfo(do_nowPath);
            if (di.Exists == true) { di.Delete(true); }
            di.Create();

            for (int i = 0; i < do_nowKeyList.Count; i++)
            {
                string nowFolder_path = do_nowPath + "\\" + do_nowKeyList[i];
                di = new DirectoryInfo(nowFolder_path);
                di.Create();

                string nowTextfile_path = do_nowPath + "\\" + do_nowKeyList[i] + ".txt";
                File.WriteAllText(nowTextfile_path, "", Encoding.UTF8);
            }
            Log("now 초기화 완료");

            // temp폴더 초기화   
            string month = DateTime.Now.ToString("MM");
            int dayLimit = 30;

            string content1 = "_학생";
            string content2 = "_비서";
            string[] savePath = new string[2];

            if (int.Parse(month) == 1 || int.Parse(month) == 3 || int.Parse(month) == 5 || int.Parse(month) == 7
                || int.Parse(month) == 8 || int.Parse(month) == 10 || int.Parse(month) == 12) { dayLimit = 31; }

            for (int i = 1; i <= dayLimit; i++)
            {
                if (i < 10)
                {
                    savePath[0] = do_tempPath + "\\학생\\" + month + "0" + i + content1 + ".txt";
                    savePath[1] = do_tempPath + "\\비서\\" + month + "0" + i + content2 + ".txt";
                }
                else
                {
                    savePath[0] = do_tempPath + "\\학생\\" + month + i + content1 + ".txt";
                    savePath[1] = do_tempPath + "\\비서\\" + month + i + content2 + ".txt";
                }

                for (int j = 0; j < savePath.Length; j++)
                {
                    string textValue = "temp";
                    File.WriteAllText(savePath[j], textValue, Encoding.UTF8);
                }
            }
        }

        public void SetNowSetting(List<string> _nowNameList, List<string> _nowKeyList, List<string> _pathList,
            List<string> _regular_work_folderList, string tempPath, string nowPath, string todayPath, int _nowHour, int _nowMin, string _backupPath, string _logPath)
        {
            do_nowNameList = _nowNameList;
            do_nowKeyList = _nowKeyList;
            do_schedulePath = _pathList;

            do_regular_work_folderList = _regular_work_folderList;

            do_tempPath = tempPath;
            do_nowPath = nowPath;
            do_todayPath = todayPath;

            do_hour = _nowHour;
            do_min = _nowMin;

            do_backupPath = _backupPath;
            do_logPath = _logPath;


            if(dbworker != null)
            {
                // Tables 데이터 삭제------------------혹시나 수정했을 경우 고려
                dbworker.DeleteTable("ETC_Table");
                dbworker.DeleteTable("Now_Table");
                dbworker.DeleteTable("Worker_Table");

                // ETC_Table 삽입------------------
                string queryStr_etc = string.Format("'{0}', '{1}'", do_backupPath, do_logPath);
                dbworker.InsertTable("ETC_Table", queryStr_etc);

                // Worker_Table 삽입------------------
                for (int i = 0; i < do_nowNameList.Count; i++)
                {
                    string queryStr = string.Format("'{0}', '{1}', '{2}'", do_nowNameList[i], do_nowKeyList[i], do_schedulePath[i]);

                    dbworker.InsertTable("Worker_Table", queryStr);
                }

                // Now_Table 삽입------------------
                string hourmin = do_hour.ToString() + ":" + do_min.ToString();
                List<string> nameList = new List<string>();
                List<string> workfolderList = new List<string>();

                for (int i = 0; i < do_nowNameList.Count; i++) { nameList.Add(do_nowNameList[i]); }
                var names = string.Join("@", nameList.ToArray());

                for (int i = 0; i < do_regular_work_folderList.Count; i++) { workfolderList.Add(do_regular_work_folderList[i]); }
                var workfolder = string.Join("@", workfolderList.ToArray());

                string queryStr2 = string.Format("'{0}', '{1}', '{2}', '{3}', '{4}', '{5}'", names, workfolder, do_tempPath, do_nowPath, do_todayPath, hourmin);

                dbworker.InsertTable("Now_Table", queryStr2);
            }
            else { MessageBox.Show("DB Error"); }

            Log("now 설정 완료");
        }

        private void DoNowSetting(object sender, EventArgs e)
        {
            string[] firstSplit = DateTime.Now.ToString("H:m:s").Split(':');
            int h = int.Parse(firstSplit[0]);
            int m = int.Parse(firstSplit[1]);

            // NOW / TODAY폴더 내 파일 전체 삭제
            if (h == 0 && m == 0 && firstSplit[2].Equals("0")) { NowIntialize(); }

            // today폴더에 파일들 복사--------------------------------------
            if (h == do_hour && m == do_min)
            {
                if (firstSplit[2].Equals("0"))
                {
                    Log("now 진행중");
                    for (int i = 0; i < do_regular_work_folderList.Count; i++)
                    {
                        if (Directory.Exists(do_regular_work_folderList[i]))
                        {
                            string[] splitStr = do_regular_work_folderList[i].Split('\\');
                            appendStr = splitStr[splitStr.Length - 1];
                            Directory_OR_File(do_regular_work_folderList[i]);
                        }
                        else
                        {
                            MessageBox.Show("해당 폴더가 존재하지 않습니다.: " + do_regular_work_folderList[i]);
                            return;
                        }
                    }

                    //temp폴더에서도 파일복사---------------------------------
                    if (Directory.Exists(do_tempPath))
                    {
                        string[] splitStr = do_tempPath.Split('\\');
                        appendStr = splitStr[splitStr.Length - 1];
                        Directory_OR_File(do_tempPath);
                    }
                    else
                    {
                        MessageBox.Show("해당 폴더가 존재하지 않습니다.");
                        return;
                    }
                    // today 전송완료--------------------------------------------


                    // now 프로세스 실행------------------------------------------------
                    if (Directory.Exists(do_nowPath))
                    {
                        for (int i = 0; i < do_nowNameList.Count; i++)
                        {
                            DirectoryInfo di = new DirectoryInfo(do_todayPath + "\\today");

                            // 텍스트파일
                            foreach (var item in di.GetFiles())
                            {
                                if (item.Name.Contains(do_nowKeyList[i]))
                                {
                                    string textValue = File.ReadAllText(item.FullName);

                                    string savePath = do_nowPath + "\\" + do_nowNameList[i] + ".txt";
                                    if (savePath == null) { throw new FileNotFoundException("지정된 폴더가 존재하지 않습니다."); }

                                    if (!Path.GetFileNameWithoutExtension(savePath).Equals(do_nowKeyList[i]))
                                    {
                                        File.WriteAllText(savePath, textValue, Encoding.UTF8);
                                    }
                                    else
                                    {
                                        textValue = File.ReadAllText(item.FullName) + "\r\n";
                                        File.AppendAllText(savePath, textValue, Encoding.UTF8);
                                    }
                                }
                            }

                            // 관련폴더
                            foreach (var item in di.GetDirectories())
                            {
                                if (item.Name.Contains(do_nowKeyList[i]))
                                {
                                    string savePath = do_nowPath + "\\" + do_nowNameList[i];
                                    DirectoryCopy(item.FullName, savePath, true);
                                }
                            }
                        }
                        Log("now 진행완료");
                        trayset.tooltip_show("NOW is Complete");
                    }
                    else { MessageBox.Show("지정된 폴더가 존재하지 않습니다."); }
                }
            }
        }

        private void Directory_OR_File(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);

            foreach (var item in di.GetFiles()) { IsTodayFile(item.FullName); }

            foreach (var item in di.GetDirectories())
            {
                if (IsTodayFolder(item.FullName)) { continue; }
                else { Directory_OR_File(item.FullName); }
            }
        }

        private void IsTodayFile(string s)
        {
            string date = DateTime.Now.ToString("MMdd");
            string _todayPath = do_todayPath + "\\today";

            string[] splitStr = s.Split('\\');
            string fDate = splitStr[splitStr.Length - 1].Substring(0, 4);

            if (fDate.Equals(date))
            {
                string sourceFile = Path.Combine(Path.GetDirectoryName(s), Path.GetFileName(s));
                string destFile = Path.Combine(_todayPath, Path.GetFileNameWithoutExtension(s) + "_" + appendStr + ".txt");

                if (destFile == null) { throw new FileNotFoundException("지정된 폴더가 존재하지 않습니다."); }

                File.Copy(sourceFile, destFile, true);
            }
        }

        private bool IsTodayFolder(string s)
        {
            string date = DateTime.Now.ToString("MMdd");
            string _todayPath = do_todayPath + "\\today\\";

            string[] splitStr = s.Split('\\');
            string fDate = splitStr[splitStr.Length - 1];

            if (fDate.Contains(date))
            {
                DirectoryCopy(s, _todayPath + splitStr[splitStr.Length - 1] + "_" + appendStr, true);
                return true;
            }
            else { return false; }
        }

        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists) { throw new DirectoryNotFoundException("지정된 폴더를 찾을 수 없습니다.: " + sourceDirName); }

            DirectoryInfo[] dirs = dir.GetDirectories();
            if (!Directory.Exists(destDirName)) { Directory.CreateDirectory(destDirName); }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public void SetNormalSetting( List<string> _scheduleDay_sun, List<string> _scheduleDay_mon, List<string> _scheduleDay_tue,
                                      List<string> _scheduleDay_wed, List<string> _scheduleDay_thr, List<string> _scheduleDay_fri, 
                                      List<string> _scheduleDay_sat)
        {
            do_scheduleDay_sun = _scheduleDay_sun;
            do_scheduleDay_mon = _scheduleDay_mon;
            do_scheduleDay_tue = _scheduleDay_tue;
            do_scheduleDay_wed = _scheduleDay_wed;
            do_scheduleDay_thr = _scheduleDay_thr;
            do_scheduleDay_fri = _scheduleDay_fri;
            do_scheduleDay_sat = _scheduleDay_sat;

            Log("노멀 프로세스 설정 완료");
        }
        
        private void DoNormalSet(object sender, EventArgs e)
        {
            string day = DateTime.Now.DayOfWeek.ToString();

            for (int i = 0; i < 144; i++)
            {
                if (do_scheduleDay_sun.Count > 0)
                {
                    if (!do_scheduleDay_sun[i].Equals(""))
                    {
                        if (day.Equals(day_of_week_Eng[0]))
                        {
                            CheckTime(i / 6, identifier_Minute[i % 6], 0, do_scheduleDay_sun[i], i);
                        }
                    }
                    if (!do_scheduleDay_mon[i].Equals(""))
                    {
                        if (day.Equals(day_of_week_Eng[1]))
                        {
                            CheckTime(i / 6, identifier_Minute[i % 6], 1, do_scheduleDay_mon[i], i);
                        }
                    }
                    if (!do_scheduleDay_tue[i].Equals(""))
                    {
                        if (day.Equals(day_of_week_Eng[2]))
                        {
                            CheckTime(i / 6, identifier_Minute[i % 6], 2, do_scheduleDay_tue[i], i);
                        }
                    }
                    if (!do_scheduleDay_wed[i].Equals(""))
                    {
                        if (day.Equals(day_of_week_Eng[3]))
                        {
                            CheckTime(i / 6, identifier_Minute[i % 6], 3, do_scheduleDay_wed[i], i);
                        }
                    }
                    if (!do_scheduleDay_thr[i].Equals(""))
                    {
                        if (day.Equals(day_of_week_Eng[4]))
                        {
                            CheckTime(i / 6, identifier_Minute[i % 6], 4, do_scheduleDay_thr[i], i);
                        }
                    }
                    if (!do_scheduleDay_fri[i].Equals(""))
                    {
                        if (day.Equals(day_of_week_Eng[5]))
                        {
                            CheckTime(i / 6, identifier_Minute[i % 6], 5, do_scheduleDay_fri[i], i);
                        }
                    }
                    if (!do_scheduleDay_sat[i].Equals(""))
                    {
                        if (day.Equals(day_of_week_Eng[6]))
                        {
                            CheckTime(i / 6, identifier_Minute[i % 6], 6, do_scheduleDay_sat[i], i);
                        }
                    }
                }
            }
        }

        private void CheckTime(int _hour, int _min, int dayofweekNum, string _name, int i)
        {
            string name = "";

            for (int j=0; j<do_nowKeyList.Count; j++)
            {
                if(_name.Contains(do_nowKeyList[j]))
                {
                    name = do_nowKeyList[j];
                }
            }
            
            string[] firstSplit = DateTime.Now.ToString("H:m:s").Split(':');
            int hour = int.Parse(firstSplit[0]);
            int min = int.Parse(firstSplit[1]);

            if (hour == _hour && min == _min)
            {
                if (firstSplit[2].Equals("0")) { GetFile(name, dayofweekNum, i); }
            }
        }

        private void GetFile(string name, int dayofweekNum, int i)
        {
            switch(dayofweekNum)
            {
                case 0:
                    do_schedulePath = normalset.SchedulePath_sun;
                    break;
                case 1:
                    do_schedulePath = normalset.SchedulePath_mon;
                    break;
                case 2:
                    do_schedulePath = normalset.SchedulePath_tue;
                    break;
                case 3:
                    do_schedulePath = normalset.SchedulePath_wed;
                    break;
                case 4:
                    do_schedulePath = normalset.SchedulePath_thr;
                    break;
                case 5:
                    do_schedulePath = normalset.SchedulePath_fri;
                    break;
                case 6:
                    do_schedulePath = normalset.SchedulePath_sat;
                    break;
            }

            if (Directory.Exists(do_nowPath))
            {
                Log(name + " 프로세스 진행중");
                DirectoryInfo di = new DirectoryInfo(do_nowPath);

                foreach (var item in di.GetFiles())
                {
                    if (item.Name.Contains(name))
                    {
                        string sourceFile = Path.Combine(do_nowPath, item.Name);
                        string destFile = Path.Combine(do_schedulePath[i], item.Name);

                        if (destFile == null) { throw new FileNotFoundException("지정된 폴더가 존재하지 않습니다."); }

                        File.Copy(sourceFile, destFile, true);
                    }
                }

                foreach (var item in di.GetDirectories())
                {
                    if (item.Name.Contains(name))
                    {
                        DirectoryCopy(item.FullName, do_schedulePath[i] + "\\" + name, true);
                    }
                }
                Log(name + " 프로세스 진행완료");
                trayset.tooltip_show("Process is complete");
            }
            else { MessageBox.Show("지정된 폴더가 존재하지 않습니다."); }
        }

    }
}