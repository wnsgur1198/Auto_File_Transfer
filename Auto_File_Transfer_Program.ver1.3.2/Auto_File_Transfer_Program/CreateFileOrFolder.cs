using System;
using System.IO;
using System.Text;
using System.Windows;

namespace Auto_File_Transfer_Program
{
    class CreateFileOrFolder
    {
        public CreateFileOrFolder()
        {
            int count = 0;
            string month = DateTime.Now.ToString("MM");
            int dayLimit = 30;

            string content1 = "_학생";
            string content2 = "_비서";

            string[] savePath = new string[4];
            string[] saveFolderPath = new string[4];

            if (int.Parse(month) == 1 || int.Parse(month) == 3 || int.Parse(month) == 5 || int.Parse(month) == 7
                || int.Parse(month) == 8 || int.Parse(month) == 10 || int.Parse(month) == 12) { dayLimit = 31; }

            for (int i = 1; i <= dayLimit; i++)
            {
                if (i < 10)
                {
                    savePath[0] = @"C:\Users\user\Desktop\Test_Folder\month\학생\" + month + "0" + i + content1 + ".txt";
                    savePath[1] = @"C:\Users\user\Desktop\Test_Folder\month\비서\" + month + "0" + i + content2 + ".txt";
                    savePath[2] = @"C:\Users\user\Desktop\Test_Folder\year\학생\" + month + "0" + i + content1 + ".txt";
                    savePath[3] = @"C:\Users\user\Desktop\Test_Folder\year\비서\" + month + "0" + i + content2 + ".txt";

                    saveFolderPath[0] = @"C:\Users\user\Desktop\Test_Folder\month\학생\" + month + "0" + i + content1;
                    saveFolderPath[1] = @"C:\Users\user\Desktop\Test_Folder\month\비서\" + month + "0" + i + content2;
                    saveFolderPath[2] = @"C:\Users\user\Desktop\Test_Folder\year\학생\" + month + "0" + i + content1;
                    saveFolderPath[3] = @"C:\Users\user\Desktop\Test_Folder\year\비서\" + month + "0" + i + content2;
                }
                else
                {
                    savePath[0] = @"C:\Users\user\Desktop\Test_Folder\month\학생\" + month + i + content1 + ".txt";
                    savePath[1] = @"C:\Users\user\Desktop\Test_Folder\month\비서\" + month + i + content2 + ".txt";
                    savePath[2] = @"C:\Users\user\Desktop\Test_Folder\year\학생\" + month + i + content1 + ".txt";
                    savePath[3] = @"C:\Users\user\Desktop\Test_Folder\year\비서\" + month + i + content2 + ".txt";

                    saveFolderPath[0] = @"C:\Users\user\Desktop\Test_Folder\month\학생\" + month + i + content1;
                    saveFolderPath[1] = @"C:\Users\user\Desktop\Test_Folder\month\비서\" + month + i + content2;
                    saveFolderPath[2] = @"C:\Users\user\Desktop\Test_Folder\year\학생\" + month + i + content1;
                    saveFolderPath[3] = @"C:\Users\user\Desktop\Test_Folder\year\비서\" + month + i + content2;

                }

                for (int j = 0; j < savePath.Length; j++)
                {
                    //파일생성
                    string textValue = savePath[j];
                    File.WriteAllText(savePath[j], textValue, Encoding.UTF8);

                    //폴더생성
                    DirectoryInfo di = new DirectoryInfo(saveFolderPath[j]);
                    if (di.Exists == false) { di.Create(); }

                    textValue = saveFolderPath[j] + "\\" + "관련자료" + count + ".txt";
                    File.WriteAllText(textValue, textValue, Encoding.UTF8);

                    count++;
                }
            }
            MessageBox.Show("Files and Folder are created");
            Environment.Exit(0);
        }

    }
}
