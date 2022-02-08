using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;


namespace FileManager_v2._0
{

    class MainJob
    {
        public string Path { get; set; }
        List<FileSystemInfo> dirs;

        const int fileLeft = 25,
            fileBoard = 25,
            namePos = 25,
            sizePos = namePos + 15,
            dataPos = sizePos + 20,
            typePos = dataPos + 25;

        int endPosY,
            ind = 0;

        
        string info = "Информация\n" +
            "\nФайловый менеджер\n" +
            "\nСоздать:нажмите N" +
            "\nОткрыть:нажмите Enter" + 
            "\nКопировать:нажмите C"+
            "\nУдалить:нажмите Delete";           
            
        public MainJob()
        {
            endPosY = Console.WindowWidth -2;
            Path = @"";
            dirs = new List<FileSystemInfo>();
        }

      
        void PrintFileInfo()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(namePos, 2);
            Console.Write("Имя");
            Console.SetCursorPosition(sizePos + 4, 2);
            Console.Write('|');
            Console.SetCursorPosition(sizePos , 2);
            Console.Write("Размер");
            Console.SetCursorPosition(dataPos - 2, 2);
            Console.Write('|');
            Console.SetCursorPosition(dataPos, 2);
            Console.Write("Дата редактирования");
            Console.SetCursorPosition(typePos , 2);
            Console.Write('|');
            Console.SetCursorPosition(typePos, 2);
            Console.Write("Тип ");
        }
        void PrintDriverInfo()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(namePos, 2);
            Console.Write("Имя");
            Console.SetCursorPosition(sizePos, 2);
            Console.Write("Размер");
            Console.SetCursorPosition(sizePos - 2, 2);
            Console.Write('|');
            Console.SetCursorPosition(dataPos, 2);
            Console.Write("Свободного места");
            Console.SetCursorPosition(dataPos - 2, 2);
            Console.Write('|');
            Console.SetCursorPosition(typePos, 2);
            Console.Write("Тип");
            Console.SetCursorPosition(typePos - 2, 2);
            Console.Write('|');
        }
      
        /// <summary>
        /// окантовка консоли
        /// </summary>
        void PrintEdging()
        {
            for (int i = fileLeft - 2; i < endPosY; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(i, 1);
                Console.Write("------------------------------");
                Console.SetCursorPosition(i, 3);
                Console.Write("------------------------------");
            }
            for (int i = 0; i < fileBoard; i++)
            {
                Console.SetCursorPosition(fileLeft - 3, i);
                Console.Write('|');
            }
            for (int i = 0; i < endPosY; i++)
            {
                Console.SetCursorPosition(i, fileBoard);
                Console.Write("------------------------------");
            }
        }
        /// <summary>
        /// очистка файлов
        /// </summary>
        public void ClearFiles()
        {
            for (int i = fileLeft - 2; i < endPosY+2; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write(' ');
                for (int j = 2; j < fileBoard; j++)
                {
                    Console.SetCursorPosition(i, j);
                    Console.Write(' ');
                }
            }
        }
        public void PrintDirectoryList(int index )
        {           
            PrintFileInfo();
            UpgradeDirectoryList();
            Console.SetCursorPosition(0, 0);         
            Console.WriteLine(info);
            Console.SetCursorPosition(fileLeft, 0);
            Console.Write(Path);
            if (index == 0)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.SetCursorPosition(fileLeft, 4);
            Console.Write("...");
            Console.ResetColor();

            for (int i = 0; i < dirs.Count && i < fileBoard - 5; i++)
            {
                if (i + ind == index - 1)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                Console.SetCursorPosition(namePos, 5 + i);
                if (dirs[i + ind].Name.Length > 23)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write("{0,20}...  ", dirs[i + ind].Name);
                }
                else
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"{dirs[i + ind].Name}   ");
                Console.SetCursorPosition(dataPos, 5 + i);
                Console.Write($"{dirs[i + ind].LastWriteTimeUtc}           ");
                if (dirs[i + ind] is FileInfo)
                {
                    Console.SetCursorPosition(sizePos, 5 + i);
                    Console.Write("{0,15} KB", (dirs[i + ind] as FileInfo)?.Length / 1024);
                    Console.SetCursorPosition(typePos, 5 + i);
                    Console.Write($"                    ");
                }
                else
                {
                    Console.SetCursorPosition(sizePos, 5 + i);
                    Console.Write($"                    ");
                    Console.SetCursorPosition(typePos, 5 + i);
                    var vs = dirs[i + ind].Attributes.ToString();
                    var inde = vs.LastIndexOf(' ');
                    if (inde >= 0)
                    {
                        Console.Write("{0,20}", vs.Substring(inde + 1, vs.Length - inde - 1));
                    }
                    else
                        Console.Write("{0,20}", vs);
                }
                Console.ResetColor();
            }
        }

        public void PrintDrivers(int index = -1)
        {
            
            PrintDriverInfo();
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(info);
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            for (int i = 0; i < allDrives.Length; i++)
            {
                if (i == index - 1)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.SetCursorPosition(namePos, 5 + i);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(allDrives[i].Name);

                Console.SetCursorPosition(dataPos, 5 + i);
                Console.Write($"{Math.Round((double)allDrives[i].TotalFreeSpace / 1024 / 1024 / 1024, 2)} GB");

                Console.SetCursorPosition(sizePos, 5 + i);
                Console.Write($"{Math.Round((double)allDrives[i].TotalSize / 1024 / 1024 / 1024, 2)} GB");

                Console.SetCursorPosition(typePos, 5 + i);
                Console.Write("{0,20}", allDrives[i].DriveType);

                Console.ResetColor();
            }
        }

        public void DriversControl()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            int i = 1;
            ConsoleKey key = ConsoleKey.Spacebar;
            while (key != ConsoleKey.Escape)
            {               
                if (key == ConsoleKey.DownArrow)
                    i++;
                if (key == ConsoleKey.UpArrow)
                    i--;
                if (i < 1)
                    i = allDrives.Length;
                if (i > allDrives.Length)
                    i = 1;
                
                PrintDrivers(i);

                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Enter)
                {
                    Path += allDrives[i - 1].Name;
                    break;
                }             
            }
            ClearFiles();
        }

        public void UpgradeDirectoryList()
        {
            dirs.Clear();
            dirs.AddRange(new DirectoryInfo(Path).GetDirectories().Where(file =>
                (file.Attributes & FileAttributes.Hidden) == 0));
            dirs.AddRange(new DirectoryInfo(Path).GetFiles().Where(file =>
                (file.Attributes & FileAttributes.Hidden) == 0));
        }

        public void RunExe(FileSystemInfo file)
        {
            Process.Start(file.FullName);
        }

        /// <summary>
        /// Путь назад
        /// </summary>
        /// <returns></returns>
        public string OpenBackFolder()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Path = Path.TrimEnd('\\');
            int position = Path.LastIndexOf(@"\");
            Path = Path.Substring(0, position + 1);
            return Path;
        }

        /// <summary>
        /// Переход к папкам и файлам
        /// </summary>
        /// <param name="index"></param>
        public void Choose(int index)
        {
            
            if (index > 0)
            {
                if (dirs[index - 1].Extension == "" || dirs[index - 1].Attributes == FileAttributes.Directory)
                {
                    ClearFiles();
                    Path += $@"{dirs[index - 1].Name}\";
                    UpgradeDirectoryList();
                }
                else if (dirs[index - 1].Extension == ".exe")
                {
                    RunExe(dirs[index - 1]);
                }
                else if (dirs[index - 1].Extension == ".txt")
                {
                    
                }              
            }
            else if (index == 0)
            {
                ClearFiles();
                OpenBackFolder();
            }
        }

        /// <summary>
        /// Очистка ввода консоли
        /// </summary>
        public static void ClearInput()
        {
            for (int i = 0; i < Console.WindowHeight - fileBoard; i++)
            {
                for (int j = 0; j < Console.WindowWidth - 1; j++)
                {
                    Console.SetCursorPosition(j, fileBoard + i);
                    Console.Write(" ");
                }
            }
        }


        /// <summary>
        /// Метод,создающий файл/папку
        /// </summary>
        public void CreateFile()
        {
            string name = "";

            Console.SetCursorPosition(0, fileBoard + 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Введите имя и расширение (без расширения создастся папка): ");
            name = Console.ReadLine();

            if (name.IndexOf(".") == -1)
                Directory.CreateDirectory(Path + name);
            else
            {
                FileStream f = File.Create(Path + name);
                f.Close();
            }
            ClearFiles();
            ClearInput();
        }

        /// <summary>
        /// Метод,удаляющий файл/папку
        /// </summary>
        /// <param name="index"></param>
        public void DeleteFile(int index)
        {
            ClearFiles();
            if (dirs[index - 1].Extension == "" || dirs[index - 1].Attributes == FileAttributes.Directory)
                Directory.Delete(dirs[index - 1].FullName, true);
            else
                File.Delete(dirs[index - 1].FullName);
        }

        /// <summary>
        /// Метод копирующий файл из одной директории в другую.
        /// </summary>
        /// <param name="pathFrom">Строка, путь, откуда копируется файл.</param>
        /// <param name="pathTo">Строка, путь, куда копируется файл.</param>
        private  static void CopyFile(string pathFrom, string pathTo)
        {
            try
            {
                if (pathTo == null || pathTo.Length == 0 || pathFrom == null || pathFrom.Length == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Имя файла не может быть пустым!!!");
                    return;
                }
                if (!File.Exists(pathFrom))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Копируемый файл не найден, операция не была выполнена.");
                    return;
                }
                if (new DirectoryInfo(pathFrom).FullName == new DirectoryInfo(pathTo).FullName)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Пути откуда копируется файл и куда копируется" +
                        " файл совпадают, копирование не будет произведено.");
                    return;
                }
                if (File.Exists(pathTo))
                {
                    int rewrite = -1;
                    string inputString;
                    do
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("В директории, в которую вы хотите скопировать" +
                            " файл, уже есть файл с таким именем. Заменить его(yes/no)?>>>");
                        inputString = Console.ReadLine();
                        if (inputString == "yes")
                        {
                            rewrite = 1;
                        }
                        if (inputString == "no")
                        {
                            rewrite = 0;
                        }
                    }
                    while (rewrite == -1);
                    if (rewrite == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Файл не будет заменен, операция закончена.");
                        return;
                    }
                }               
                string upperPathTo = Directory.GetParent(pathTo).ToString();
                if (!Directory.Exists(upperPathTo))
                {
                    Directory.CreateDirectory(upperPathTo);
                }
                File.Copy(pathFrom, pathTo, true);
                Console.SetCursorPosition(0, fileBoard + 2);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nФайл успешно скопирован.");
            }
            catch (IOException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Произошла ошибка при копировании:{e.Message}");
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        private static void TerminalCopy()
        {
            try
            {
                Console.SetCursorPosition(0, fileBoard + 2);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Директория из которой копируется файл(вместе с именем файла)>>>");
                string pathFrom = Console.ReadLine();
                Console.SetCursorPosition(0, fileBoard + 4);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Директория в которую копируется файл" +
                    "(вместе с именем файла, можно задать новое имя)>>>");
                string pathTo = Console.ReadLine();
                CopyFile(pathFrom, pathTo);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        public void Start()
        {
#pragma warning disable CA1416 // Проверка совместимости платформы
            Console.SetWindowSize(120, 50);
#pragma warning restore CA1416 // Проверка совместимости платформы
            try
            {
                int i = 0;
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, fileBoard + 1);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ввод: ");
                ConsoleKey key = ConsoleKey.Spacebar;
                while (key != ConsoleKey.Escape)
                {
                    PrintEdging();                   
                    if (key == ConsoleKey.DownArrow)
                    {
                        i++;
                        if (i > dirs.Count)
                            i = dirs.Count;
                        else if (i - ind >= fileBoard - 4)
                        {
                            ind++;
                        }
                    }
                    if (key == ConsoleKey.UpArrow)
                    {
                        i--;
                        if (i < 0)
                            i = 0;
                        else if (i < ind + 1)
                        {
                            ind--;
                            if (ind < 0)
                                ind = 0;
                        }
                    }

                    if (Path == @"")
                        DriversControl();
                    PrintDirectoryList(i);

                    key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Enter)
                    {
                        Choose(i);
                        i = 0;
                        ind = 0;
                    }
                    if (key == ConsoleKey.N)
                        CreateFile();                                    
                    if (key == ConsoleKey.Delete)
                        DeleteFile(i);
                    if (key==ConsoleKey.C)                  
                        TerminalCopy();                  
                }
            }
            catch (Exception ex)
            {
                Console.SetCursorPosition(0, fileBoard + 1);                
                Console.SetCursorPosition(0, fileBoard + 2);
                Console.WriteLine(ex.Message);
                Console.ReadKey(true);
                OpenBackFolder();
                ClearInput();
                Start();
            }
        }
    }
}
