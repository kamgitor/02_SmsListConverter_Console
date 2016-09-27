using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Diagnostics;


namespace KamConsole
{
    // ******************************************************
    // Proba zrobienia tego na pojedynczych znakach ale na stringu
    class Program
    {

        // ****************************************************************
        // ret: 1 - poprawny poczatek wersa, 0 - kontynuacja
        static bool VerseTestStartLine(string verse)
        {
            int size;

            if (verse == "Date,Time,Type,Number,Name,Message")      // linijka specialna poczatkowa
                return true;

            size = verse.Length;

            if (size < 20)
                return false;

            if (verse[0] != '2')
                return false;           // nie poczatek

            if (verse[1] != '0')
                return false;

            if (verse[4] != '-')
                return false;

            if (verse[7] != '-')
                return false;

            if (verse[10] != ',')
                return false;

            if (verse[13] != ':')
                return false;

            if (verse[16] != ':')
                return false;

            if (verse[19] != ',')
                return false;

            return true;

        }   // VerseTestStartLine


        // ****************************************************************
        // ret: 1 - special znak, 0 - nit special znak
        static bool TestSpecial(char znak)
        {
            byte i;
            char[] tab = {   (char)0xD83C, (char)55357, (char)0xDE0A, (char)0xDE09,
                             (char)0xDE18, (char)0xDE42, (char)0xDE0D, (char)0xDE03,
                             (char)0xDE00, (char)0xDE13, (char)0xDE12, (char)0xDCA9,
                             (char)0xDC8F, (char)0xDC8B, (char)0xDC69, (char)0xDC36,
                             (char)0xDC25, 
                             (char)0 };

            for (i = 0; i < 100; i++)
            {
                if (tab[i] == 0)
                    break;  // return false;

                if (tab[i] == znak)
                    return true;
            }

            if (znak >= 0xDC23)
            // if (znak >= 0xDE00)
                return true;

            return false;

        }   // TestSpecial

        
        // ****************************************************************
        static void Main(string[] args)
        {
            string filename;
            string verse = null;
            string verse_out = null;
            // string verse_next = null;
            string blabla;
            short i, index = 0;
            char znak;
            byte comma_cnt;

            // byte[] buff = null;

            Console.WriteLine("Hello World");
            //Tworzenie new stream-writer i otwarcie pliku.
            /*
                        Console.WriteLine("Podaj nazwe pliku");
                        filename = Console.ReadLine();
            */
            filename = "sms.csv";

            // otwarcie pliku do odczytu
            StreamReader plik_we = File.OpenText(filename);

            // otwarcie pliku do zapisu
            TextWriter plik_wy = new StreamWriter("wyjscie.txt");

            verse_out = null;
            comma_cnt = 0;

            for (;;)        // przelot po wersach
            {
                verse = plik_we.ReadLine();
                if (verse == null)               // koniec pliku
                {
                    if (verse_out != null)
                        plik_wy.WriteLine(verse_out);
                    break;
                }

                index = 0;

                if (VerseTestStartLine(verse))      // verse poprawnie sie zaczyna
                {
                    if (verse_out != null)
                        plik_wy.WriteLine(verse_out);

                    verse_out = null;
                    comma_cnt = 0;
                }
                else      // vers jest kontynuacja
                {
                    verse_out += " ";       // dodanie spacji zeby wersy byly oddzielone
                }

                for (i = 0; i < verse.Length; i++)       // przelot po poszczegolnych znakach
                {
                    znak = verse[index++];

                    if (znak == ',')
                    {
                        if (comma_cnt < 5)
                        {
                            verse_out += ";";
                            ++comma_cnt;
                        }
                        else
                        {
                            verse_out += znak;
                        }
                    }
                    else if (znak == ';')       // jak w wejsciu jest srednik
                    {
                        verse_out += '_';
                    }
                    else if (znak == '\"')
                    {
                        verse_out += " ";
                    }
                    else if (znak == '\n')
                    {
                        break;
                    }
                    else if (TestSpecial(znak))
                        verse_out += " ";
                    else
                    {
                        verse_out += znak;
                    }

                }

            }

            plik_we.Close();
            plik_wy.Close();


            // koniec - break
            blabla = verse;
        }
    }


/*
    // ******************************************************
    // Proba zrobienia tego na pojedynczych bajtach - masakra
    class Program1
    {

        static bool TestAscii(char znak)
        {
            if (znak < ' ')
                return false;
            if (znak > '~')
                return false;

            return false;
        }

        static void Main1(string[] args)
        {
            string filename;
            string verse = null;
            // string verse_next = null;
            string blabla;
            // int verse_cnt;
            int status, index = 0;
            char[] znak = null;
            byte comma_cnt;

            // byte[] buff = null;

            filename = "sms.csv";

            // otwarcie pliku do odczytu
            StreamReader plik_we = File.OpenText(filename);

            // otwarcie pliku do zapisu
            TextWriter plik_wy = new StreamWriter("wyjscie.txt");

            comma_cnt = 0;
            verse = "";
            for (; ;)
            {
                // verse = plik_we.ReadLine();
                status = plik_we.Read(znak, index, 1);
                ++index;

                if (status == null)
                    break;

                if (znak[0] == ',')
                {
                    znak[0] = ';';
                    if (comma_cnt >= 5)       // 
                    {
                        
                    }
                    else
                        ++comma_cnt;
                }
                else if (znak[0] == '\n')
                {
                    plik_wy.WriteLine(verse);
                }
                else if (znak[0] == null)
                {
                    
                    
                }
                else if (TestAscii(znak[0]))
                {

                }
            }

            plik_we.Close();
            plik_we.Close();


            // koniec - break
            blabla = verse;
        }
    }
*/


    // ******************************************************
    // To dziala pobierajac calego stringa
    // No i jest problem jak sa krzaki wewnatrz
    class Program2
    {
        static void Main2(string[] args)
        {
            string filename;
            string verse = null;
            // string verse_next = null;
            string blabla;
            // int verse_cnt;
            int char_cnt;
            char [] verse_buff;
            byte comma_cnt;
            
            // byte[] buff = null;

            Console.WriteLine("Hello World");
            //Tworzenie new stream-writer i otwarcie pliku.
/*
            Console.WriteLine("Podaj nazwe pliku");
            filename = Console.ReadLine();
*/
            filename = "sms.csv";

            // otwarcie pliku do odczytu
            StreamReader plik_we = File.OpenText(filename);

            // otwarcie pliku do zapisu
            TextWriter plik_wy = new StreamWriter("wyjscie.txt");

            // for (verse_cnt = 0; verse_cnt < 10; verse_cnt++)
            for(;;)
            {
/*
                if (verse_next != null)
                {
                    verse = verse_next;
                    verse_next = null;
                }
                else*/
                    verse = plik_we.ReadLine();

                if (verse == null)
                    break;
/*
                for(;;)
                {
                    verse_next = plik_we.ReadLine();
                    if (verse_next == "\"")
                        continue;
                    if (verse_next.Contains("2016"))
                        break;
                    else
                    {
                        verse.Insert(verse.Length, verse_next);
                        verse_next = null;
                    }
                }
*/

                verse_buff = verse.ToCharArray(0, verse.Length);

                comma_cnt = 0;

                for (char_cnt = 0; char_cnt < verse.Length; char_cnt++)
                {
                    if (verse_buff[char_cnt] == ',')
                    {
                        verse_buff[char_cnt] = ';';
                        if (++comma_cnt >= 5)
                            break;
                    }
                }
                verse = new String(verse_buff);
                plik_wy.WriteLine(verse);
            }

            plik_we.Close();
            plik_we.Close();
    

            // koniec - break
            blabla = verse;
        }
    }
}
