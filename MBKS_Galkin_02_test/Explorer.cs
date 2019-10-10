using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace MBKS_Galkin_02_test
{
    class Explorer
    {
        static bool DateChecker(string date)
        {
            DateTime DT = new DateTime();

            if (DateTime.TryParse(date, out DT))
                return true;
            else
                return false;
        }
        static DateTime DateParser(DateTime DC)
        {
            DateTime NewDC = DC;
            string date = DC.ToShortDateString();
            string time = DC.ToShortTimeString();
            do
            {
                Console.Write("Введите новую дату создания: ");
                date = Console.ReadLine();
            }
            while (!DateChecker(date));
            NewDC = DateTime.Parse(date+" "+time);
            return NewDC;
        }
        static void DateChanger(ref FileStream FS)
        {
            DateTime DC = File.GetCreationTime(FS.Name);
            Console.WriteLine("Дата создания: "+DC.ToShortDateString());
            DateTime NewDC = DateParser(DC);
            FS.Close();
            //FS.
            File.SetCreationTime(FS.Name, NewDC);
            FS = File.Open(FS.Name, FileMode.Open, FileAccess.Read, FileShare.None);
            DC = File.GetCreationTime(FS.Name);
            Console.WriteLine("Дата создания: " + DC.ToLongDateString());
            ConsoleKeyInfo KK;
            Console.WriteLine("   Esc для выхода");
            do
            {
                KK = Console.ReadKey(true);
            } while (KK.Key != ConsoleKey.Escape);
            
        }
        static FileStream Adminka(ConsoleKeyInfo K, ref FileStream FS)
        {
            Console.Clear();
            Console.WriteLine("1.   Изменение даты создания");
            Console.WriteLine("2.   Посмотреть содержимое файла");
            Console.WriteLine("Esc. Выход");
            Console.WriteLine();
            bool check = true;
            while (check)
            {
                ConsoleKeyInfo KK;
                KK = Console.ReadKey(true);
                if (KK.KeyChar == '1')
                {
                    Console.Clear();
                    DateChanger(ref FS);
                    Console.Clear();
                    Console.WriteLine("1.   Изменение даты создания");
                    Console.WriteLine("2.   Посмотреть содержимое файла");
                    Console.WriteLine("Esc. Выход");
                    Console.WriteLine();
                }
                if(KK.KeyChar == '2')
                {
                    Console.Clear();
                    Read(K, ref FS);
                    Console.Clear();
                    Console.WriteLine("1.   Изменение даты создания");
                    Console.WriteLine("2.   Посмотреть содержимое файла");
                    Console.WriteLine("Esc. Выход");
                    Console.WriteLine();  
                }
                if (KK.Key == ConsoleKey.Escape)
                { 
                   
                    Console.Clear();
                    check = false;
                    
                }
            }
            return FS;
        }
        static void Read(ConsoleKeyInfo K,ref  FileStream SR)
        {
            Console.Clear();
            Console.WriteLine("   Esc для закрытия");
            Console.WriteLine();
            char ch = ' ';
            string o = "";
            for (long i = 0; i < SR.Length; i++)
                    o += Convert.ToChar(SR.ReadByte());
            SR.Seek(0, SeekOrigin.Begin);
            if (SR.Name.Substring(SR.Name.Length - 6) == "matrix") { Console.WriteLine(o.Substring(3)); }
            else
                Console.WriteLine(o);
            ConsoleKeyInfo Ke = K;
            while (Ke.Key != ConsoleKey.Escape) { Ke = Console.ReadKey(true); }
        }
        static string Trimmer(string a)
        {
            if (a == Program.PATH)
                return a;
            if (a != "")
            {
                while (a[a.Length - 1] != '\\')
                {
                    a = a.Substring(0, a.Length - 1);
                }
            }
            a = a.Substring(0, a.Length - 1);
            return a;
        }
        static bool Checker(string a, string b)
        {
            if (a == Trimmer(b))
                return true;
            else
                return false;
        }
        public static void Journey(string pth,string role)
        {
            Console.CursorVisible = false;
            string[] AllFilesPathes = Directory.GetFiles(pth);
            string[] AllDirPathes = Directory.GetDirectories(pth);
            List<string> AllStrings = new List<string> { };
            foreach (var item in AllFilesPathes)
            {
                if ((role == "admin" || (role == "user" && (Program.Rulles[item] == "110" || Program.Rulles[item] == "111")) || (role == "guest" && Program.Rulles[item] == "111")))
                    AllStrings.Add(item);
            }
            foreach (var item in AllDirPathes)
            {
                AllStrings.Add(item);
            }
            AllStrings.Add(Trimmer(pth));
            AllStrings.Add(Program.PATH);
            List<FileStream> Fls = new List<FileStream> { };
            foreach (var item in Program.FLS)
            {
                if (Checker(pth, item.Name) && (role == "admin" || (role == "user" && (Program.Rulles[item.Name] == "110" || Program.Rulles[item.Name] == "111")) || (role == "guest" && Program.Rulles[item.Name] == "111")))
                    Fls.Add(item);
            }
            int counter = 0;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("   Текущая директория: "+ pth+ "  Пользователь: " + role + "  '0' Для выхода из программы");
                Console.WriteLine();
                Console.WriteLine();
                for (int i = 0; i < AllStrings.Count; i++)
                {
                    if (i == counter)
                    {
                        if (i == AllStrings.Count - 2)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("");
                            Console.WriteLine(" >> Предыдущая директория");
                        }
                        else
                       if (i == AllStrings.Count - 1)
                        {
                            Console.WriteLine(" >> Домашняя директория");
                        }else
                        Console.WriteLine(" >>  " + AllStrings[i]);
                    }
                    else
                    {
                        if (i == AllStrings.Count - 2)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("");
                            Console.WriteLine("   Предыдущая директория");
                        }
                        else
                        if (i == AllStrings.Count - 1)
                        {
                            
                            Console.WriteLine("   Домашняя директория");
                        }else
                        Console.WriteLine(AllStrings[i]);
                    }
                }
                Console.SetCursorPosition(0, 0);
                ConsoleKeyInfo K = new ConsoleKeyInfo();
                K = Console.ReadKey(true);
                Thread.Sleep(50);
                if (K.Key == ConsoleKey.DownArrow) { if (counter < AllStrings.Count - 1) counter++; }
                if (K.Key == ConsoleKey.UpArrow) { if (counter > 0) counter--; }
                if (K.Key == ConsoleKey.Escape) { return; }
                if (K.KeyChar == '0') { throw new Exception(); }
                if (K.Key == ConsoleKey.Enter)
                {
                    if (Directory.Exists(AllStrings[counter]))
                    {
                        Explorer.Journey(AllStrings[counter], role);
                    }
                    else
                    if(role!="admin")
                    {

                        FileStream FSS = Fls[counter];

                        Read(K,ref FSS);
                        Fls[counter] = FSS;
                    }
                    else
                    {
                        FileStream FSS = Fls[counter];

                        Adminka(K,ref FSS);

                        Fls[counter] = FSS;
                        for (int i = 0; i < Program.FLS.Count; i++)
                        {
                            if (Program.FLS[i].Name == Fls[counter].Name)
                                Program.FLS[i] = FSS;
                        }
                        
                    }
                }
                else{}
            }
        }
    }
}
