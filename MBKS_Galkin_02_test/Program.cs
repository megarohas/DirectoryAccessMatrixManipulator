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
    public class Program
    {
        public static string ChekRuller(string name)
        {
            //if()
            if(name.Substring(name.Length-6) == "matrix") return "100";
            DateTime DC = File.GetCreationTime(name);
            DateTime DCD = DC.AddDays(1);
            DateTime DCH = DC.AddHours(1);
            if (DCD < DateTime.Now) return "100";
            if (DCD > DateTime.Now && DCH < DateTime.Now) return "110";
            else
                return "111";
        }
        public static void Login()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("   Введите имя пользователя: ");
                CurUser = Console.ReadLine();
            }
            while (!Usrs.Contains(CurUser));
        }
        public static List<FileStream> FLS = new List<FileStream> { };
        public static string CurUser = "";
        public static Dictionary<string,string> Rulles = new Dictionary<string, string> { };
        public static string PATH = "C:\\Matrix";
        static List<string> Usrs = new List<string> { "admin", "user", "guest" };

        static void Main(string[] args)
        {
            
            int c = 0;
            int r = 1;
            
            FileStream fs = File.Open(PATH + "\\matrix", FileMode.OpenOrCreate);
            //fs.Close();
            string[] AllFilesPathes = Directory.GetFiles(PATH, "*", SearchOption.AllDirectories);
           // int filecount = AllFilesPathes.Length / 100 + 1;
            foreach (var item in AllFilesPathes)
            {
                Rulles.Add(item, ChekRuller(item));
            }

            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            foreach (var item in Rulles)
            {
                sw.WriteLine(item.Key + " " + item.Value);
            }
            sw.Close();
            fs.Close();

            foreach (var item in AllFilesPathes)
            {
                if (item.Substring(item.Length - 6) == "matrix")
                    FLS.Add(File.Open(item, FileMode.Open, FileAccess.ReadWrite,FileShare.None));
                else
                FLS.Add(File.Open(item, FileMode.Open, FileAccess.ReadWrite, FileShare.None));
                c++;
               // if (c % (filecount) == 0)
             //   {

                //    Console.Clear();
                //    Console.WriteLine("Загрузка файлов: " + r + "%");
               //     r++;
             //   }
            }
                     
            if (FLS.FindAll(x => x.Name == PATH + "\\matrix").Count == 0)
                FLS.Add(File.Open(PATH + "\\matrix", FileMode.Open, FileAccess.Read, FileShare.None));


            StreamWriter sw2 = new StreamWriter(FLS.Find(x => x.Name == PATH + "\\matrix"), Encoding.UTF8);
            try
            {
                while (true)
                {
                    Console.SetWindowSize(90, 30);
                    Login();
                    Rulles.Clear();
                    foreach (var item in AllFilesPathes)
                    {
                        Rulles.Add(item, ChekRuller(item));
                    }
                   
                    foreach (var item in Rulles)
                    {
                        sw2.WriteLine(item.Key + " " + item.Value);
                    }
                    Explorer.Journey(PATH, CurUser);
                }
            }
            catch (Exception e)
            {
                sw2.Close();
                FLS.Find(x => x.Name == PATH + "\\matrix").Close();
                File.Delete(PATH + "\\matrix");
                return;
            }
        }
    }
}
