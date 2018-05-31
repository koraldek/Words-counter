using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace ZiAD_5 {

    class Program {

        static void Main()
        {
            Menu menu = new Menu();

            Console.Write("Write input:{word};{scan method[0-1]} (ESC to exit) 0-Normal scan, 1-with memory mapped file:\n");
            while (true)
            {
                Console.WriteLine("\n");
                if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                    break;
                //Console.Clear();
                //Console.Write("Write input:{word};{file to scan[0-" + menu.filePaths.Length + "]};{scan method[0-1]} (ESC to exit):\n");
                //var cr1 = Console.CursorTop;
                //var cc1 = Console.CursorLeft;
                //menu.printMenu();
                //var cr2 = Console.CursorTop;
                //var cc2 = Console.CursorLeft;
                //Console.CursorTop = cr1;
                //Console.CursorLeft = cc1;
                String[] input = Console.ReadLine().Split(';');
                //Console.CursorTop = cr2;
                //Console.CursorLeft = cc2;

                //String dir = menu.filePaths[Int32.Parse(input[1])];
                Boolean method = input[1].Equals("1") ? true : false;
                String mName = method ? "Memory mapped file" : "Default method";
                //Console.WriteLine("Method:" + mName + ".Word found " + readMyFile.findWord(input[0], dir, method) + " times.");
                readMyFile.findWord(input[0], "books", method);
                Console.WriteLine("Method:" + mName + ".Word found:" + readMyFile.count + " times. Time elapsed:" + readMyFile.time + " ms.");

            }
            Console.WriteLine("End of program.");

            Console.ReadKey();
        }

        public class Menu {
            public string[] filePaths;

            public Menu()
            {
                filePaths = Directory.GetFiles(@"c:\test\", "*.txt", SearchOption.TopDirectoryOnly);
            }
            public int printMenu()
            {
                Console.WriteLine();
                Console.WriteLine("0-Normal scan, 1- with memory mapped file");
                for (int i = 0; i < filePaths.Length; i++)
                {
                    Console.WriteLine(i + ":" + filePaths[i]);
                }
                return filePaths.Length;
            }
        }

        public class readMyFile 
		{
            static String dir;
            public static int count = 0;
            public static long time = 0;
            public static int findWord(String word, String fileName, Boolean withMapping)
            {
                dir = "c:\\test\\" + fileName + ".txt";
                if (withMapping)
                    useMapping(word);
                else
                    readNormal(word);
                return count;
            }
            private static void useMapping(String word)
            {
                count = 0;
                var watch = System.Diagnostics.Stopwatch.StartNew();
                using (var mmf = MemoryMappedFile.CreateFromFile(dir))
                {
                    using (var stream = mmf.CreateViewStream())
                    {
                        StreamReader sr = new StreamReader(stream);
                        while (!sr.EndOfStream)
                            if (sr.ReadLine().Contains(word))
                                count++;
                    }
                }
                watch.Stop();
                time = watch.ElapsedMilliseconds;
            }

            private static void readNormal(String word)
            {
                count = 0;
                var watch = System.Diagnostics.Stopwatch.StartNew();
                using (var stream = new FileStream(dir, FileMode.Open))
                {
                    StreamReader sr = new StreamReader(stream);
                    while (!sr.EndOfStream)
                        if (sr.ReadLine().Contains(word))
                            count++;
                }
                watch.Stop();
                time = watch.ElapsedMilliseconds;
            }
        }
    }
}
