using System;
using System.Threading;
using System.Windows;

namespace Auto_File_Transfer_Program
{
    class Start : Application
    {
        private static DatabaseWorker dbworker = null;

        private static SystemTraySet trayset = null;
        private static ProgressProcess process = null;

        private static MainWindow mainwindow = null;
        private static NowSetWindow nowset = null;
        private static NormalSetWindow normalset = null;

        [STAThread]
        static void Main()
        {
            //createTextFiles.createFile();     // 파일&폴더 만들기
            dbworker = new DatabaseWorker();
            //dbworker.Test_SQL();
            //Drop_Tables();                    // 테이블 삭제
            //Create_Tables();                    // 테이블 생성

            trayset = new SystemTraySet();

            process = new ProgressProcess(trayset);

            process.SetDBWorker(dbworker);      // DB 객체 전달
            

            nowset = new NowSetWindow(process);

            nowset.SetDBWorker(dbworker);       // DB 객체 전달


            normalset = new NormalSetWindow(nowset);            

            normalset.SetDBWorker(dbworker);    // DB 객체 전달

            normalset.SetProgressProcess(process);


            nowset.SetNormalSet(normalset);     // DB 초기화 위함

            //dbworker.DeleteTable("Normal_Table");
            //normalset.Initialize_NormalTable();


            process.SetNormalset(normalset);    // 노멀프로세스 진행 위함

            mainwindow = new MainWindow(nowset, normalset, trayset);

            mainwindow.SetDBWorker(dbworker);

            trayset.SetMainwindow(mainwindow);      // 시스템트레이 객체 전달

            normalset.SetMainwindow(mainwindow);

            new Application().Run(mainwindow);            
        }

        private static void Drop_Tables()
        {
            dbworker.DropTable("Normal_Table");
            //dbworker.DropTable("ETC_Table");
            //dbworker.DropTable("Now_Table");
            //dbworker.DropTable("Worker_Table");
        }

        private static void Create_Tables()
        {
            //dbworker.CreateTable("Worker_Table",
            //                     @"xName varchar(20) not null, xKeyword varchar(20) not null, 
            //                     xSendPath varchar(50) not null, primary key(xName)"
            //);
            //dbworker.CreateTable("Now_Table",
            //                     @"xNames varchar(50) not null, xWorkFolderPath varchar(100) not null, 
            //                     xTempPath varchar(50) not null, xNowPath varchar(50) not null,
            //                     xTodayPath varchar(50) not null, xTime varchar(10) not null,
            //                     primary key(xNames)"
            //);
            //dbworker.CreateTable("ETC_Table",
            //                     @"xBackupPath varchar(50) not null, 
            //                     xLogPath varchar(50) not null,
            //                     primary key(xBackupPath, xLogPath)"
            //);
            dbworker.CreateTable("Normal_Table",
                                 @"xYear int not null, xMonth int not null, xDay int not null,
                                 xHour int, xMinute int, xName varchar(20), xPath varchar(50),
                                 primary key(xYear, xMonth, xDay, xHour, xMinute)"
            );
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            string mutexName = "AFTProg";
            bool isCreatedNew = false;
            Mutex mutex = new Mutex(true, mutexName, out isCreatedNew);

            try { mutex = new Mutex(false, mutexName); }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Current.Shutdown();
            }

            if (mutex.WaitOne(0, false)) { base.OnStartup(e); }
            else
            {
                MessageBox.Show("이미 실행중입니다.");
                Current.Shutdown();
            }
        }

    }
}
