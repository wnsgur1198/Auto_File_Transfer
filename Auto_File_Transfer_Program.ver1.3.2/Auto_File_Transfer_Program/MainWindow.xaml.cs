using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ListBox = System.Windows.Controls.ListBox;
using SelectionMode = System.Windows.Controls.SelectionMode;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;
using System.Threading;

namespace Auto_File_Transfer_Program
{
    public partial class MainWindow : Window
    {
        private string[] day_of_week_kor = { "일", "월", "화", "수", "목", "금", "토" };
        private string[] day_of_week_Eng = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        private int[] identifier_Minute = { 0, 10, 20, 30, 40, 50 };
        private ListBox[] processList_Sun = new ListBox[24];
        private ListBox[] processList_Mon = new ListBox[24];
        private ListBox[] processList_Tue = new ListBox[24];
        private ListBox[] processList_Wed = new ListBox[24];
        private ListBox[] processList_Thr = new ListBox[24];
        private ListBox[] processList_Fri = new ListBox[24];
        private ListBox[] processList_Sat = new ListBox[24];

        private List<string> dayList = null;
        private NowSetWindow nowSet = null;
        private NormalSetWindow normalset = null;
        private SystemTraySet traySet = null;
        private Thread weekpageThread = null;
        private string selectedDayOfWeek = DateTime.Now.DayOfWeek.ToString();
        private DateTime selectedDate = DateTime.Now;
        private DatabaseWorker dbworker = null;


        public MainWindow(NowSetWindow _nowset, NormalSetWindow _normalset, SystemTraySet _traySet)
        {
            InitializeComponent();

            StateChanged += Window_Resize;

            dayList = new List<string>();

            nowSet = _nowset;

            normalset = _normalset;

            traySet = _traySet;

            presentMonth.Text = DateTime.Today.Month.ToString();
            presentYear.Text = DateTime.Today.Year.ToString();

            CreateTable_Column(DateTime.Today.DayOfWeek.ToString(), DateTime.Today.Date);

            Create_processList();

            CreateTable_Row();

            weekpageThread = new Thread(Refresh);
            weekpageThread.Start();
        }

        public DateTime GetSelectedDate() { return selectedDate; }

        private void Create_processList()
        {
            for (int i = 0; i < 24; i++)
            {
                processList_Sun[i] = new ListBox { SelectionMode = SelectionMode.Single };
                processList_Sun[i].SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
                processList_Sun[i].MouseDoubleClick += new MouseButtonEventHandler(ProcessList_MouseDoubleClick);

                processList_Mon[i] = new ListBox { SelectionMode = SelectionMode.Single };
                processList_Mon[i].SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
                processList_Mon[i].MouseDoubleClick += new MouseButtonEventHandler(ProcessList_MouseDoubleClick);

                processList_Tue[i] = new ListBox { SelectionMode = SelectionMode.Single };
                processList_Tue[i].SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
                processList_Tue[i].MouseDoubleClick += new MouseButtonEventHandler(ProcessList_MouseDoubleClick);

                processList_Wed[i] = new ListBox { SelectionMode = SelectionMode.Single };
                processList_Wed[i].SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
                processList_Wed[i].MouseDoubleClick += new MouseButtonEventHandler(ProcessList_MouseDoubleClick);

                processList_Thr[i] = new ListBox { SelectionMode = SelectionMode.Single };
                processList_Thr[i].SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
                processList_Thr[i].MouseDoubleClick += new MouseButtonEventHandler(ProcessList_MouseDoubleClick);

                processList_Fri[i] = new ListBox { SelectionMode = SelectionMode.Single };
                processList_Fri[i].SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
                processList_Fri[i].MouseDoubleClick += new MouseButtonEventHandler(ProcessList_MouseDoubleClick);

                processList_Sat[i] = new ListBox { SelectionMode = SelectionMode.Single };
                processList_Sat[i].SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
                processList_Sat[i].MouseDoubleClick += new MouseButtonEventHandler(ProcessList_MouseDoubleClick);
            }
                        
            for (int rowNumber = 1; rowNumber < 25; rowNumber++)         // 그리드에 스택패널 배치
            {
                for(int columnNumber=1; columnNumber<8; columnNumber++)
                {
                    switch (columnNumber)
                    {
                        case 1:
                            Grid.SetRow(processList_Sun[rowNumber - 1], rowNumber);
                            Grid.SetColumn(processList_Sun[rowNumber - 1], columnNumber);
                            myGrid.Children.Add(processList_Sun[rowNumber - 1]);
                            break;
                        case 2:
                            Grid.SetRow(processList_Mon[rowNumber - 1], rowNumber);
                            Grid.SetColumn(processList_Mon[rowNumber - 1], columnNumber);
                            myGrid.Children.Add(processList_Mon[rowNumber - 1]);
                            break;
                        case 3:
                            Grid.SetRow(processList_Tue[rowNumber - 1], rowNumber);
                            Grid.SetColumn(processList_Tue[rowNumber - 1], columnNumber);
                            myGrid.Children.Add(processList_Tue[rowNumber - 1]);
                            break;
                        case 4:
                            Grid.SetRow(processList_Wed[rowNumber - 1], rowNumber);
                            Grid.SetColumn(processList_Wed[rowNumber - 1], columnNumber);
                            myGrid.Children.Add(processList_Wed[rowNumber - 1]);
                            break;
                        case 5:
                            Grid.SetRow(processList_Thr[rowNumber - 1], rowNumber);
                            Grid.SetColumn(processList_Thr[rowNumber - 1], columnNumber);
                            myGrid.Children.Add(processList_Thr[rowNumber - 1]);
                            break;
                        case 6:
                            Grid.SetRow(processList_Fri[rowNumber - 1], rowNumber);
                            Grid.SetColumn(processList_Fri[rowNumber - 1], columnNumber);
                            myGrid.Children.Add(processList_Fri[rowNumber - 1]);
                            break;
                        case 7:
                            Grid.SetRow(processList_Sat[rowNumber - 1], rowNumber);
                            Grid.SetColumn(processList_Sat[rowNumber - 1], columnNumber);
                            myGrid.Children.Add(processList_Sat[rowNumber - 1]);
                            break;
                    }
                }
               
            }

        }

        private void CreateTable_Column(string _todayDoW, DateTime _todayDate)
        {
            string todayDoW = _todayDoW;
            DateTime todayDate = _todayDate;
            dayList.Clear();
            
            if (todayDoW.Equals(day_of_week_Eng[0]))    // 열 추가
            {
                string[] str = todayDate.AddDays(-0).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+1).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+2).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+3).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+4).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+5).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+6).ToShortDateString().Split('-');
                dayList.Add(str[2]);
            }
            else if (todayDoW.Equals(day_of_week_Eng[1]))
            {
                string[] str = todayDate.AddDays(-1).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+0).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+1).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+2).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+3).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+4).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+5).ToShortDateString().Split('-');
                dayList.Add(str[2]);
            }
            else if (todayDoW.Equals(day_of_week_Eng[2]))
            {
                string[] str = todayDate.AddDays(-2).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(-1).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+0).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+1).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+2).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+3).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+4).ToShortDateString().Split('-');
                dayList.Add(str[2]);
            }
            else if (todayDoW.Equals(day_of_week_Eng[3]))
            {
                string[] str = todayDate.AddDays(-3).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(-2).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(-1).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+0).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+1).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+2).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+3).ToShortDateString().Split('-');
                dayList.Add(str[2]);
            }
            else if (todayDoW.Equals(day_of_week_Eng[4]))
            {
                string[] str = todayDate.AddDays(-4).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(-3).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(-2).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(-1).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+0).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+1).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+2).ToShortDateString().Split('-');
                dayList.Add(str[2]);
            }
            else if (todayDoW.Equals(day_of_week_Eng[5]))
            {
                string[] str = todayDate.AddDays(-5).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(-4).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(-3).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(-2).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(-1).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+0).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+1).ToShortDateString().Split('-');
                dayList.Add(str[2]);
            }
            else
            {
                string[] str = todayDate.AddDays(-6).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(-5).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(-4).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(-3).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(-2).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(-1).ToShortDateString().Split('-');
                dayList.Add(str[2]);

                str = todayDate.AddDays(+0).ToShortDateString().Split('-');
                dayList.Add(str[2]);
            }

            colName0.Text = dayList.ElementAt(0) + "일 (" + day_of_week_kor[0] + ")";
            colName1.Text = dayList.ElementAt(1) + "일 (" + day_of_week_kor[1] + ")";
            colName2.Text = dayList.ElementAt(2) + "일 (" + day_of_week_kor[2] + ")";
            colName3.Text = dayList.ElementAt(3) + "일 (" + day_of_week_kor[3] + ")";
            colName4.Text = dayList.ElementAt(4) + "일 (" + day_of_week_kor[4] + ")";
            colName5.Text = dayList.ElementAt(5) + "일 (" + day_of_week_kor[5] + ")";
            colName6.Text = dayList.ElementAt(6) + "일 (" + day_of_week_kor[6] + ")";
        }

        private void CreateTable_Row()
        {
            for (int _hour = 0; _hour < 24; _hour++)
            {
                for (int i = 0; i < 144; i++)
                {
                    if (i / 6 == _hour)
                    {
                        if(normalset.ScheduleDay_sun[i].Equals(""))
                        {
                            processList_Sun[_hour].Items.Add(normalset.ScheduleDay_sun[i]);
                            RemoveBlank_InList(processList_Sun);
                        }
                        else
                        {
                            processList_Sun[_hour].Items.Add(normalset.ScheduleDay_sun[i] + "                  -0-" + i / 6 + "-" + identifier_Minute[i % 36]);
                            RemoveBlank_InList(processList_Sun);
                        }

                        if (normalset.ScheduleDay_mon[i].Equals(""))
                        {
                            processList_Mon[_hour].Items.Add(normalset.ScheduleDay_mon[i]);
                            RemoveBlank_InList(processList_Mon);
                        }
                        else
                        {
                            processList_Mon[_hour].Items.Add(normalset.ScheduleDay_mon[i] + "                  -1-" + i / 6 + "-" + identifier_Minute[i % 6]);
                            RemoveBlank_InList(processList_Mon);
                        }

                        if (normalset.ScheduleDay_tue[i].Equals(""))
                        {
                            processList_Tue[_hour].Items.Add(normalset.ScheduleDay_tue[i]);
                            RemoveBlank_InList(processList_Tue);
                        }
                        else
                        {
                            processList_Tue[_hour].Items.Add(normalset.ScheduleDay_tue[i] + "                  -2-" + i / 6 + "-" + identifier_Minute[i % 6]);
                            RemoveBlank_InList(processList_Tue);
                        }

                        if (normalset.ScheduleDay_wed[i].Equals(""))
                        {
                            processList_Wed[_hour].Items.Add(normalset.ScheduleDay_wed[i]);
                            RemoveBlank_InList(processList_Wed);
                        }
                        else
                        {
                            processList_Wed[_hour].Items.Add(normalset.ScheduleDay_wed[i] + "                  -3-" + i / 6 + "-" + identifier_Minute[i % 6]);
                            RemoveBlank_InList(processList_Wed);
                        }

                        if (normalset.ScheduleDay_thr[i].Equals(""))
                        {
                            processList_Thr[_hour].Items.Add(normalset.ScheduleDay_thr[i]);
                            RemoveBlank_InList(processList_Thr);
                        }
                        else
                        {
                            processList_Thr[_hour].Items.Add(normalset.ScheduleDay_thr[i] + "                  -4-" + i / 6 + "-" + identifier_Minute[i % 6]);
                            RemoveBlank_InList(processList_Thr);
                        }

                        if (normalset.ScheduleDay_fri[i].Equals(""))
                        {
                            processList_Fri[_hour].Items.Add(normalset.ScheduleDay_fri[i]);
                            RemoveBlank_InList(processList_Fri);
                        }
                        else
                        {
                            processList_Fri[_hour].Items.Add(normalset.ScheduleDay_fri[i] + "                  -5-" + i / 6 + "-" + identifier_Minute[i % 6]);
                            RemoveBlank_InList(processList_Fri);
                        }
                         
                        if (normalset.ScheduleDay_sat[i].Equals(""))
                        {
                            processList_Sat[_hour].Items.Add(normalset.ScheduleDay_sat[i]);
                            RemoveBlank_InList(processList_Sat);
                        }
                        else
                        {
                            processList_Sat[_hour].Items.Add(normalset.ScheduleDay_sat[i] + "                  -6-" + i / 6 + "-" + identifier_Minute[i % 6]);
                            RemoveBlank_InList(processList_Sat);
                        }
                        
                    }
                }
            }

        }

        private void RemoveBlank_InList(ListBox[] list)
        {
            for (int t1 = 0; t1 < list.Length; t1++)
            {
                for (int t2 = 0; t2 < list[t1].Items.Count; t2++)
                {
                    if (list[t1].Items[t2].Equals(""))
                    {
                        list[t1].Items.RemoveAt(t2);
                        break;
                    }
                }
            }
        }

        public void Refresh()
        {
            while (true)
            {
                Dispatcher.Invoke(new Action(delegate
                {
                    for (int i = 0; i < 24; i++)
                    {
                        processList_Sun[i].Items.Clear();
                        processList_Mon[i].Items.Clear();
                        processList_Tue[i].Items.Clear();
                        processList_Wed[i].Items.Clear();
                        processList_Thr[i].Items.Clear();
                        processList_Fri[i].Items.Clear();
                        processList_Sat[i].Items.Clear();
                    }
                    CreateTable_Row();
                }));

                Thread.Sleep(1000);
            }
        }

        public void SetDBWorker(DatabaseWorker _dbworker)
        {
            dbworker = _dbworker;
        }

        private void ResDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            int year = ResDatePicker.SelectedDate.Value.Year;
            int month = ResDatePicker.SelectedDate.Value.Month;

            selectedDayOfWeek = ResDatePicker.SelectedDate.Value.DayOfWeek.ToString();
            selectedDate = ResDatePicker.SelectedDate.Value.Date;

            LocalDB load_db = dbworker.LoadTable_Normal(year, month, selectedDate.Day);
            if (!load_db.HasRows)
            {
                MessageBox.Show("유효하지 않은 날짜 지정입니다.");
                return;
            }

            presentMonth.Text = month.ToString();
            presentYear.Text = year.ToString();

            CreateTable_Column(selectedDayOfWeek, selectedDate);

            normalset.DBLink_NormalSet(selectedDate);
        }

        private void ProcessList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;
            
            bool sun = false;
            bool mon = false;
            bool tue = false;
            bool wed = false;
            bool thr = false;
            bool fri = false;
            bool sat = false;

            if (item != null)
            {
                string[] splitRowColumn = item.Content.ToString().Split('-');
                string name = splitRowColumn[0];
                int colNumber = int.Parse(splitRowColumn[1]);
                string rowNumber_hour = splitRowColumn[2];
                string rowNumber_min = splitRowColumn[3];

                switch (colNumber)
                {
                    case 0:
                        sun = true;
                        break;
                    case 1:
                        mon = true;
                        break;
                    case 2:
                        tue = true;
                        break;
                    case 3:
                        wed = true;
                        break;
                    case 4:
                        thr = true;
                        break;
                    case 5:
                        fri = true;
                        break;
                    case 6:
                        sat = true;
                        break;
                }

                normalset.ExistNormalSet(name, rowNumber_hour, rowNumber_min, sun, mon, tue, wed, thr, fri, sat);

                normalset.Visibility = Visibility.Visible;
            }
        }

        private void AddSchedule_Click(object sender, RoutedEventArgs e)
        {
            normalset.apply.Content = "적용";
            normalset.apply.Width = 200;
            normalset.entire_apply.Width = 0;
            normalset.entire_apply.Margin = new Thickness(0);
            normalset.entire_apply.Visibility = Visibility.Hidden;

            normalset.cancel_delete.Content = "취소";
            normalset.cancel_delete.Width = 200;
            normalset.entire_delete.Width = 0;
            normalset.entire_delete.Margin = new Thickness(0);
            normalset.entire_delete.Visibility = Visibility.Hidden;

            normalset.scheduleName.IsEnabled = true;
            normalset.dayRepeat.IsEnabled = true;

            normalset.Visibility = normalset.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
            normalset.ResetControl();
        }

        private void NowSetMode_Click(object sender, RoutedEventArgs e)
        {
            nowSet.Exist_nowSet();
            nowSet.Show();
        }

        private void Window_Resize(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized) { Hide(); }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult dr = MessageBox.Show("종료하시겠습니까?", "", MessageBoxButton.OKCancel);

            if (dr == MessageBoxResult.OK)
            {
                traySet.trayClose();

                weekpageThread.Abort();

                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;

                return;
            }
        }

    }
}
