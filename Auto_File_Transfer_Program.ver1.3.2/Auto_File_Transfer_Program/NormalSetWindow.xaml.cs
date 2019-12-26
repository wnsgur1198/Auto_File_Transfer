using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using MessageBox = System.Windows.MessageBox;
using System.ComponentModel;
using System.Windows.Input;
using System;

namespace Auto_File_Transfer_Program
{
    public partial class NormalSetWindow : Window
    {
        private string[] day_of_week_Eng = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        private bool[] dayFlag = new bool[7];
        private bool[] temp_dayFlag = new bool[7];
        private int tempHour = 0;
        private int tempMin = 0;
        private NowSetWindow nowSet = null;
        private DatabaseWorker dbworker = null;
        private static ProgressProcess process = null;
        private MainWindow mainwindow = null;

        public NormalSetWindow(NowSetWindow _nowSet)
        {
            InitializeComponent();

            nowSet = _nowSet;

            ComboBoxAddItem();

            for (int i = 0; i < dayFlag.Length; i++) { dayFlag[i] = false; temp_dayFlag[i] = false; }
        }

        public void SetDBWorker(DatabaseWorker _dbworker)
        {
            dbworker = _dbworker;

            DBLink_NormalSet(DateTime.Now);             // DB 연동
        }

        public void SetProgressProcess(ProgressProcess _process)
        {
            process = _process;

            process.SetNormalSetting(ScheduleDay_sun, ScheduleDay_mon, ScheduleDay_tue, ScheduleDay_wed, ScheduleDay_thr, ScheduleDay_fri, ScheduleDay_sat);
        }

        public void SetMainwindow(MainWindow _mainwindow) { mainwindow = _mainwindow; }

        public List<string> SchedulePath_sun { get; set; } = new List<string>();
        public List<string> SchedulePath_mon { get; set; } = new List<string>();
        public List<string> SchedulePath_tue { get; set; } = new List<string>();
        public List<string> SchedulePath_wed { get; set; } = new List<string>();
        public List<string> SchedulePath_thr { get; set; } = new List<string>();
        public List<string> SchedulePath_fri { get; set; } = new List<string>();
        public List<string> SchedulePath_sat { get; set; } = new List<string>();

        public List<string> ScheduleDay_sun { get; set; } = new List<string>();
        public List<string> ScheduleDay_mon { get; set; } = new List<string>();
        public List<string> ScheduleDay_tue { get; set; } = new List<string>();
        public List<string> ScheduleDay_wed { get; set; } = new List<string>();
        public List<string> ScheduleDay_thr { get; set; } = new List<string>();
        public List<string> ScheduleDay_fri { get; set; } = new List<string>();
        public List<string> ScheduleDay_sat { get; set; } = new List<string>();
        
        public void DBLink_NormalSet(DateTime dateTime)
        {
            if (dbworker != null)
            {
                ScheduleDay_sun.Clear();
                ScheduleDay_mon.Clear();
                ScheduleDay_tue.Clear();
                ScheduleDay_wed.Clear();
                ScheduleDay_thr.Clear();
                ScheduleDay_fri.Clear();
                ScheduleDay_sat.Clear();

                switch (dateTime.DayOfWeek.ToString())
                {
                    case "Sunday":
                        dateTime = dateTime.AddDays(0);
                        DayOfWeek_Linkage(dateTime);  
                        break;
                    case "Monday":
                        dateTime = dateTime.AddDays(-1);
                        DayOfWeek_Linkage(dateTime);
                        break;
                    case "Tuesday":
                        dateTime = dateTime.AddDays(-2);
                        DayOfWeek_Linkage(dateTime);
                        break;
                    case "Wednesday":
                        dateTime = dateTime.AddDays(-3);
                        DayOfWeek_Linkage(dateTime);
                        break;
                    case "Thursday":
                        dateTime = dateTime.AddDays(-4);
                        DayOfWeek_Linkage(dateTime);
                        break;
                    case "Friday":
                        dateTime = dateTime.AddDays(-5);
                        DayOfWeek_Linkage(dateTime);
                        break;
                    case "Saturday":
                        dateTime = dateTime.AddDays(-6);
                        DayOfWeek_Linkage(dateTime);
                        break;
                }

                for(int i=0; i<6; i++)
                {
                    dateTime = dateTime.AddDays(1);
                    DayOfWeek_Linkage(dateTime);
                }
            }

            else { MessageBox.Show("DB 연동 오류"); }
        }

        private void DayOfWeek_Linkage(DateTime dateTime)
        {
            LocalDB load_db = dbworker.LoadTable_Normal(dateTime.Year, dateTime.Month, dateTime.Day);

            if (load_db.HasRows)
            {
                while (load_db.Read())
                {
                    for (int i = 0; i < load_db.FieldCount; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                string xName = load_db.GetData(i).ToString();
                                switch (dateTime.DayOfWeek.ToString())
                                {
                                    case "Sunday":
                                        ScheduleDay_sun.Add(xName);
                                        break;
                                    case "Monday":
                                        ScheduleDay_mon.Add(xName);
                                        break;
                                    case "Tuesday":
                                        ScheduleDay_tue.Add(xName);
                                        break;
                                    case "Wednesday":
                                        ScheduleDay_wed.Add(xName);
                                        break;
                                    case "Thursday":
                                        ScheduleDay_thr.Add(xName);
                                        break;
                                    case "Friday":
                                        ScheduleDay_fri.Add(xName);
                                        break;
                                    case "Saturday":
                                        ScheduleDay_sat.Add(xName);
                                        break;
                                }
                                break;
                            case 1:
                                string xPath = load_db.GetData(i).ToString();
                                switch (dateTime.DayOfWeek.ToString())
                                {
                                    case "Sunday":
                                        SchedulePath_sun.Add(xPath);
                                        break;
                                    case "Monday":
                                        SchedulePath_mon.Add(xPath);
                                        break;
                                    case "Tuesday":
                                        SchedulePath_tue.Add(xPath);
                                        break;
                                    case "Wednesday":
                                        SchedulePath_wed.Add(xPath);
                                        break;
                                    case "Thursday":
                                        SchedulePath_thr.Add(xPath);
                                        break;
                                    case "Friday":
                                        SchedulePath_fri.Add(xPath);
                                        break;
                                    case "Saturday":
                                        SchedulePath_sat.Add(xPath);
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
        }        

        private void Normal_Apply(DateTime date)
        {            
            int dayLimit = 0;

            if (dayRepeat.IsChecked != true)     // 한번만일 때
            {
                for (int i = 0; i < 7; i++)
                {
                    if (date.DayOfWeek.ToString().Equals(day_of_week_Eng[i]))
                    {
                        dayLimit = date.Day + 7 - i;
                    }
                }

                for (int k = date.Day; k < dayLimit; k++)
                {
                    for (int l = 0; l < 7; l++)
                    {
                        Normal_Apply_REAL(date, l);
                    }

                    date = date.AddDays(1);
                }

            }

            else                                 // 반복일 때
            {              
                int tempyear = date.Year;
                int tempyear2 = date.Year + 1;

                while (tempyear <= tempyear2)
                {
                    int tempmonth = date.Month;
                    int monthLimit = 12 - tempmonth;
                    
                    for (int j = 0; j <= monthLimit; j++)
                    {
                        switch (date.Month)
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
                                if (date.Year % 4 == 0) dayLimit = 29;
                                break;
                            default:
                                dayLimit = 30;
                                break;
                        }
                        for (int k = date.Day; k <= dayLimit; k++)
                        {
                            for (int l = 0; l < 7; l++)
                            {
                                Normal_Apply_REAL(date, l);
                            }

                            date = date.AddDays(1);
                        }

                        date = new DateTime(date.Year, date.Month, 1);

                        if (tempmonth.ToString().Equals(date.Month.ToString()))
                        {
                            date = date.AddMonths(1);
                            tempmonth = date.Month;
                        }
                    }

                    tempyear++;
                    date = new DateTime(tempyear, 1, 1);
                }
                
            }
            
        }

        private void Normal_Apply_REAL(DateTime date, int flag)
        {
            int xhour = int.Parse(hour.SelectedItem.ToString());
            int xmin = int.Parse(min.SelectedItem.ToString());
            string setStr;
            string conStr;

            if (date.DayOfWeek.ToString().Equals(day_of_week_Eng[flag]))
            {
                if (dayFlag[flag])
                {
                    try
                    {
                        setStr = string.Format("{0} = '{1}', {2} = '{3}'", "xName", scheduleName.Text, "xPath", pathList.SelectedItem.ToString());
                        conStr = string.Format("({0} = {1}) AND ({2} = {3}) AND ({4} = {5}) AND ({6} = {7}) AND ({8} = {9})",
                                               "xYear", date.Year, "xMonth", date.Month, "xDay", date.Day, "xHour", xhour, "xMinute", xmin);
                        dbworker.UpdateTable("Normal_Table", setStr, conStr);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            if (scheduleName.Text.Equals(""))
            {
                MessageBox.Show("설정이름은 공백일 수 없습니다.");
                return;
            }
            if (pathList.SelectedItem == null)
            {
                MessageBox.Show("키워드를 선택하십시오");
                return;
            }

            if (apply.Content.ToString().Equals("적용"))
            {
                Normal_Apply(mainwindow.GetSelectedDate());
            }
            else                  // 수정
            {
                Normal_Delete(mainwindow.GetSelectedDate());
                Normal_Apply(mainwindow.GetSelectedDate());
            }

            DBLink_NormalSet(mainwindow.GetSelectedDate());             // DB 연동
            Visibility = Visibility.Hidden;
        }

        private void Entire_apply_Click(object sender, RoutedEventArgs e)
        {
            if (scheduleName.Text.Equals(""))
            {
                MessageBox.Show("설정이름은 공백일 수 없습니다.");
                return;
            }
            if (pathList.SelectedItem == null)
            {
                MessageBox.Show("키워드를 선택하십시오");
                return;
            }

            Normal_Entire_Delete();
            Normal_Apply(mainwindow.GetSelectedDate());

            DBLink_NormalSet(mainwindow.GetSelectedDate());             // DB 연동
            Visibility = Visibility.Hidden;
        }

        private void Normal_Delete(DateTime date)
        {
            int year = date.Year; int month = date.Month; int day = date.Day;
            
            switch (date.DayOfWeek.ToString())
            {
                case "Sunday":                                              // 오늘 요일
                    if (temp_dayFlag[0])
                    {
                        day = date.AddDays(0).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[1])
                    {
                        day = date.AddDays(1).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[2])
                    {
                        day = date.AddDays(2).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[3])
                    {
                        day = date.AddDays(3).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[4])
                    {
                        day = date.AddDays(4).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[5])
                    {
                        day = date.AddDays(5).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[6])
                    {
                        day = date.AddDays(6).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    break;
                case "Monday":                                              // 오늘 요일
                    if (temp_dayFlag[0])
                    {
                        day = date.AddDays(-1).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[1])
                    {
                        day = date.AddDays(0).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[2])
                    {
                        day = date.AddDays(1).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[3])
                    {
                        day = date.AddDays(2).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[4])
                    {
                        day = date.AddDays(3).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[5])
                    {
                        day = date.AddDays(4).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[6])
                    {
                        day = date.AddDays(5).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    break;
                case "Tuesday":                                              // 오늘 요일
                    if (temp_dayFlag[0])
                    {
                        day = date.AddDays(-2).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[1])
                    {
                        day = date.AddDays(-1).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[2])
                    {
                        day = date.AddDays(0).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[3])
                    {
                        day = date.AddDays(1).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[4])
                    {
                        day = date.AddDays(2).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[5])
                    {
                        day = date.AddDays(3).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[6])
                    {
                        day = date.AddDays(4).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    break;
                case "Wednesday":                                              // 오늘 요일
                    if (temp_dayFlag[0])
                    {
                        day = date.AddDays(-3).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[1])
                    {
                        day = date.AddDays(-2).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[2])
                    {
                        day = date.AddDays(-1).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[3])
                    {
                        day = date.AddDays(0).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[4])
                    {
                        day = date.AddDays(1).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[5])
                    {
                        day = date.AddDays(2).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[6])
                    {
                        day = date.AddDays(3).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    break;
                case "Thursday":                                              // 오늘 요일
                    if (temp_dayFlag[0])
                    {
                        day = date.AddDays(-4).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[1])
                    {
                        day = date.AddDays(-3).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[2])
                    {
                        day = date.AddDays(-2).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[3])
                    {
                        day = date.AddDays(-1).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[4])
                    {
                        day = date.AddDays(0).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[5])
                    {
                        day = date.AddDays(1).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[6])
                    {
                        day = date.AddDays(2).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    break;
                case "Friday":                                              // 오늘 요일
                    if (temp_dayFlag[0])
                    {
                        day = date.AddDays(-5).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[1])
                    {
                        day = date.AddDays(-4).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[2])
                    {
                        day = date.AddDays(-3).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[3])
                    {
                        day = date.AddDays(-2).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[4])
                    {
                        day = date.AddDays(-1).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[5])
                    {
                        day = date.AddDays(0).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[6])
                    {
                        day = date.AddDays(1).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    break;
                case "Saturday":                                              // 오늘 요일
                    if (temp_dayFlag[0])
                    {
                        day = date.AddDays(-6).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[1])
                    {
                        day = date.AddDays(-5).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[2])
                    {
                        day = date.AddDays(-4).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[3])
                    {
                        day = date.AddDays(-3).Day;
                        Normal_Delete_REAL(year, month, day);                       
                    }
                    else if (temp_dayFlag[4])
                    {
                        day = date.AddDays(-2).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[5])
                    {
                        day = date.AddDays(-1).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    else if (temp_dayFlag[6])
                    {
                        day = date.AddDays(0).Day;
                        Normal_Delete_REAL(year, month, day);
                    }
                    break;
            }

        }

        private void Normal_Delete_REAL(int year, int month, int day)
        {
            int xhour = tempHour;
            int xmin = tempMin;
            string setStr; string conStr;

            try
            {
                setStr = string.Format("{0} = '{1}', {2} = '{3}'", "xName", "", "xPath", "");
                conStr = string.Format("({0} = {1}) AND ({2} = {3}) AND ({4} = {5}) AND ({6} = {7}) AND ({8} = {9})",
                                       "xYear", year, "xMonth", month, "xDay", day, "xHour", xhour, "xMinute", xmin);
                dbworker.UpdateTable("Normal_Table", setStr, conStr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void Cancel_delete_Click(object sender, RoutedEventArgs e)
        {
            if (cancel_delete.Content.Equals("취소")) { this.Visibility = Visibility.Hidden; }

            else    // 삭제
            {
                MessageBoxResult dr = MessageBox.Show("삭제하시겠습니까?", "", MessageBoxButton.YesNo);

                if (dr == MessageBoxResult.Yes)
                {
                    Normal_Delete(mainwindow.GetSelectedDate());

                    DBLink_NormalSet(mainwindow.GetSelectedDate());             // DB 연동
                    Visibility = Visibility.Hidden;
                }
                else { return; }
            }
        }

        private void Normal_Entire_Delete()
        {
            try
            {
                string setStr = string.Format("{0} = '{1}', {2} = '{3}'", "xName", "", "xPath", "");
                string conStr = string.Format("{0} = '{1}'", "xName", scheduleName.Text);
                dbworker.UpdateTable("Normal_Table", setStr, conStr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private void Entire_delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dr = MessageBox.Show("전체 삭제하시겠습니까?", "", MessageBoxButton.YesNo);

            if (dr == MessageBoxResult.Yes)
            {
                Normal_Entire_Delete();

                DBLink_NormalSet(mainwindow.GetSelectedDate());             // DB 연동
                Visibility = Visibility.Hidden;
            }
            else { return; }
        }

        public void Initialize_NormalTable()
        {
            int year = DateTime.Now.Year; 
            int yearlimit = DateTime.Now.Year + 1;

            while(year <= yearlimit)
            {
                int month = 1; int day = 1; int hour = 0; int min = 0;

                for (int i = 0; i < 12; i++)         //------------------------------------ 월
                {
                    if (month == 1 && day == 1 && year != yearlimit)                // 그 해 첫번째 주
                    {
                        DateTime presentdate = new DateTime(year, 1, 1);
                        int startday = 31;
                        switch (presentdate.DayOfWeek.ToString())
                        {
                            case "Sunday":
                                startday = 32;
                                break;
                            case "Monday":
                                startday = 31;
                                break;
                            case "Tuesday":
                                startday = 30;
                                break;
                            case "Wednesday":
                                startday = 29;
                                break;
                            case "Thursday":
                                startday = 28;
                                break;
                            case "Friday":
                                startday = 27;
                                break;
                            case "Saturday":
                                startday = 26;
                                break;
                        }

                        while (startday <= 31)
                        {
                            for (int k = 0; k < 144; k++)    // 144 = 24(시) * 6(10분간격)           // 시분
                            {
                                string queryStr = string.Format("{0}, {1}, {2}, {3}, {4}, '{5}', '{6}'",
                                                                presentdate.Year - 1, 12, startday, hour, min, "", "");
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
                            startday++;
                        }
                    }//---------------------------------------------

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

                    if (month == 12 && day == 32 && year == yearlimit)                // 그 해 마지막 주
                    {
                        DateTime presentdate = new DateTime(year, 12, 31);
                        int limit = 2;
                        switch (presentdate.DayOfWeek.ToString())
                        {
                            case "Sunday":
                                limit = 6;
                                break;
                            case "Monday":
                                limit = 5;
                                break;
                            case "Tuesday":
                                limit = 4;
                                break;
                            case "Wednesday":
                                limit = 3;
                                break;
                            case "Thursday":
                                limit = 2;
                                break;
                            case "Friday":
                                limit = 1;
                                break;
                            case "Saturday":
                                limit = 0;
                                break;
                        }

                        for (int l = 1; l <= limit; l++)
                        {
                            for (int k = 0; k < 144; k++)    // 144 = 24(시) * 6(10분간격)           // 시분
                            {
                                string queryStr = string.Format("{0}, {1}, {2}, {3}, {4}, '{5}', '{6}'",
                                                                presentdate.Year + 1, 1, l, hour, min, "", "");
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

                year++;
            }
            
        }

        public void ExistNormalSet(string name, string h, string m, bool _sun, bool _mon, bool _tue, bool _wed, bool _thr, bool _fri, bool _sat)
        {
            apply.Content = "수정";
            apply.Width = 100;
            entire_apply.Width = 100;
            entire_apply.Margin = new Thickness(10, 0, 0, 0);
            entire_apply.Visibility = Visibility.Visible;

            cancel_delete.Content = "삭제";
            cancel_delete.Width = 100;
            entire_delete.Width = 100;
            entire_delete.Margin = new Thickness(10, 0, 0, 0);
            entire_delete.Visibility = Visibility.Visible;

            scheduleName.Text = name;
            scheduleName.IsEnabled = false;
            dayRepeat.IsEnabled = false;

            NameKeyPathList_Initialize();

            for (int i = 0; i < hour.Items.Count; i++)
            {
                if (hour.Items[i].Equals(h))
                {
                    hour.SelectedIndex = i;
                    tempHour = int.Parse(hour.Items[i].ToString());
                }
            }

            for (int i = 0; i < min.Items.Count; i++)
            {
                if (min.Items[i].Equals(m))
                {
                    min.SelectedIndex = i;
                    tempMin = int.Parse(min.Items[i].ToString());
                }
            }

            if (!_sun)
            {
                sun.Background = Brushes.White;
                dayFlag[0] = false;
                temp_dayFlag[0] = false;
            }
            else
            {
                sun.Background = Brushes.Gray;
                dayFlag[0] = true;
                temp_dayFlag[0] = true;
            }

            if (!_mon)
            {
                mon.Background = Brushes.White;
                dayFlag[1] = false;
                temp_dayFlag[1] = false;
            }
            else
            {
                mon.Background = Brushes.Gray;
                dayFlag[1] = true;
                temp_dayFlag[1] = true;
            }

            if (!_tue)
            {
                tue.Background = Brushes.White;
                dayFlag[2] = false;
                temp_dayFlag[2] = false;
            }
            else
            {
                tue.Background = Brushes.Gray;
                dayFlag[2] = true;
                temp_dayFlag[2] = true;
            }

            if (!_wed)
            {
                wed.Background = Brushes.White;
                dayFlag[3] = false;
                temp_dayFlag[3] = false;
            }
            else
            {
                wed.Background = Brushes.Gray;
                dayFlag[3] = true;
                temp_dayFlag[3] = true;
            }

            if (!_thr)
            {
                thr.Background = Brushes.White;
                dayFlag[4] = false;
                temp_dayFlag[4] = false;
            }
            else
            {
                thr.Background = Brushes.Gray;
                dayFlag[4] = true;
                temp_dayFlag[4] = true;
            }

            if (!_fri)
            {
                fri.Background = Brushes.White;
                dayFlag[5] = false;
                temp_dayFlag[5] = false;
            }
            else
            {
                fri.Background = Brushes.Gray;
                dayFlag[5] = true;
                temp_dayFlag[5] = true;
            }

            if (!_sat)
            {
                sat.Background = Brushes.White;
                dayFlag[6] = false;
                temp_dayFlag[6] = false;
            }
            else
            {
                sat.Background = Brushes.Gray;
                dayFlag[6] = true;
                temp_dayFlag[6] = true;
            }
        }

        public void ResetControl()
        {
            scheduleName.Focus();
            scheduleName.Text = "";
            hour.SelectedIndex = 0;
            min.SelectedIndex = 0;

            nowNameList.Items.Clear();
            nowKeyList.Items.Clear();
            pathList.Items.Clear();

            sun.Background = Brushes.White;
            dayFlag[0] = false;
            mon.Background = Brushes.White;
            dayFlag[1] = false;
            tue.Background = Brushes.White;
            dayFlag[2] = false;
            wed.Background = Brushes.White;
            dayFlag[3] = false;
            thr.Background = Brushes.White;
            dayFlag[4] = false;
            fri.Background = Brushes.White;
            dayFlag[5] = false;
            sat.Background = Brushes.White;
            dayFlag[6] = false;
        }

        private void ComboBoxAddItem()
        {
            for (int i = 0; i < 24; i++) { hour.Items.Add(i.ToString()); }
            hour.SelectedIndex = 0;

            for (int i = 0; i < 60; i += 10) { min.Items.Add(i.ToString()); }
            min.SelectedIndex = 0;
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e) { NameKeyPathList_Initialize(); }

        private void NameKeyPathList_Initialize()
        {
            List<string> _nameList = nowSet.GetNameList();
            List<string> _keyList = nowSet.GetKeyList();
            List<string> _pathList = nowSet.GetPathList();

            for (int i = 0; i < _nameList.Count; i++)
            {
                if (scheduleName.Text.Contains(_nameList[i]))
                {
                    nowNameList.Items.Clear();
                    nowKeyList.Items.Clear();
                    pathList.Items.Clear();

                    nowNameList.Items.Add(_nameList[i]);
                    nowKeyList.Items.Add(_keyList[i]);
                    pathList.Items.Add(_pathList[i]);
                    
                }
            }

        }

        private void ScheduleName_KeyDown(object sender, KeyEventArgs e) { if (e.Key == Key.Enter) { SearchBtn_Click(sender, e); } }

        private void Sun_Click(object sender, RoutedEventArgs e)
        {
            if (dayFlag[0] == true)
            {
                sun.Background = Brushes.White;
                dayFlag[0] = false;
            }
            else
            {
                sun.Background = Brushes.Gray;
                dayFlag[0] = true;
            }
        }

        private void Mon_Click(object sender, RoutedEventArgs e)
        {
            if (dayFlag[1] == true)
            {
                mon.Background = Brushes.White;
                dayFlag[1] = false;
            }
            else
            {
                mon.Background = Brushes.Gray;
                dayFlag[1] = true;
            }
        }

        private void Tue_Click(object sender, RoutedEventArgs e)
        {
            if (dayFlag[2] == true)
            {
                tue.Background = Brushes.White;
                dayFlag[2] = false;
            }
            else
            {
                tue.Background = Brushes.Gray;
                dayFlag[2] = true;
            }
        }

        private void Wed_Click(object sender, RoutedEventArgs e)
        {
            if (dayFlag[3] == true)
            {
                wed.Background = Brushes.White;
                dayFlag[3] = false;
            }
            else
            {
                wed.Background = Brushes.Gray;
                dayFlag[3] = true;
            }
        }

        private void Thr_Click(object sender, RoutedEventArgs e)
        {
            if (dayFlag[4] == true)
            {
                thr.Background = Brushes.White;
                dayFlag[4] = false;
            }
            else
            {
                thr.Background = Brushes.Gray;
                dayFlag[4] = true;
            }
        }

        private void Fri_Click(object sender, RoutedEventArgs e)
        {
            if (dayFlag[5] == true)
            {
                fri.Background = Brushes.White;
                dayFlag[5] = false;
            }
            else
            {
                fri.Background = Brushes.Gray;
                dayFlag[5] = true;
            }
        }

        private void Sat_Click(object sender, RoutedEventArgs e)
        {
            if (dayFlag[6] == true)
            {
                sat.Background = Brushes.White;
                dayFlag[6] = false;
            }
            else
            {
                sat.Background = Brushes.Gray;
                dayFlag[6] = true;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }

        private void NowNameList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            nowKeyList.SelectedIndex = nowNameList.SelectedIndex;
            pathList.SelectedIndex = nowNameList.SelectedIndex;
        }

        private void NowKeyList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            nowNameList.SelectedIndex = nowKeyList.SelectedIndex;
            pathList.SelectedIndex = nowKeyList.SelectedIndex;
        }

        private void PathList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            nowKeyList.SelectedIndex = pathList.SelectedIndex;
            nowNameList.SelectedIndex = pathList.SelectedIndex;
        }

    }
}
