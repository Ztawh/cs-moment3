using System;
using System.Collections.Generic;
using System.IO;

// Skrivet av Amanda Hwatz björkholm 2021

namespace Moment3
{
    public class Entry
    {
        private string _name;
        private string _message;

        public Entry(string name, string message)
        {
            this._name = name;
            this._message = message;
        }

        public string GetName()
        {
            return this._name;
        }

        public string GetMessage()
        {
            return this._message;
        }
    }

    public class Guestbook
    {
        public void AddEntry(Entry entry)
        {
            // Skriv till fil
            StreamWriter sw = new StreamWriter("users.txt", true);

            sw.WriteLine($"{entry.GetName()} - {entry.GetMessage()}");
            sw.Close();
        }

        public void WriteEntries()
        {
            StreamReader sr = new StreamReader("users.txt");
            string line = "";

            List<Entry> entryList = new List<Entry>();

            while ((line = sr.ReadLine()) != null)
            {
                if (line == "")
                {
                    continue;
                }
                
                string[] lineArr = line.Split(" - ");

                Entry readEntry = new Entry(lineArr[0], lineArr[1]);
                
                entryList.Add(readEntry);
            }
            
            sr.Close();

            int i = 0;
            foreach (var entry in entryList)
            {
                Console.WriteLine($"[{i}]Författare: {entry.GetName()}, Meddelande: {entry.GetMessage()}");
                i += 1;
            }
        }

        public bool DeleteEntry(int id)
        {
            StreamReader sr = new StreamReader("users.txt");
            // Hämta inlägg
            string line = "";
            List<Entry> entryList = new List<Entry>();

            while ((line = sr.ReadLine()) != null)
            {
                if (line == "")
                {
                    continue;
                }
                
                string[] lineArr = line.Split(" - ");

                Entry readEntry = new Entry(lineArr[0], lineArr[1]);
                
                entryList.Add(readEntry);
            }
            sr.Close();

            // Ta bort inlägg med ID
            try
            {
                entryList.RemoveAt(id);
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
            
            //Spara ny lista
            StreamWriter sw = new StreamWriter("users.txt");
            
            foreach (var entry in entryList)
            {
                sw.WriteLine($"{entry.GetName()} - {entry.GetMessage()}");
            }
            
            sw.Close();
            return true;
        }
    }
    
    internal static class Program
    {
        private static void Main(string[] args)
        {
            // Deklarera lista
            //List<Entry> entries = new List<Entry>();
            Guestbook guestbook = new Guestbook();

            while (true)
            {
                Console.WriteLine("A M A N D A S  G Ä S T B O K");
                
                 // Meny
                 Console.WriteLine("1. Skriv i gästboken");
                 Console.WriteLine("2. Ta bort inlägg");
                 Console.WriteLine();
                 Console.WriteLine("X. Avsluta");
                 Console.WriteLine();

                 guestbook.WriteEntries();
                
                 string key = Console.ReadLine();
                
                 // Nytt inlägg
                 if (key == "1")
                 {
                     // Hämta namn och meddelande
                     Console.WriteLine("Skriv ditt namn");
                     string name = Console.ReadLine();
                     // Kontrollera om namn är tomt
                     while (name == "")
                     {
                         Console.WriteLine("Du måste ange ett namn!");
                         name = Console.ReadLine();
                     }
                
                     Console.WriteLine("Skriv ett inlägg"); 
                     string message = Console.ReadLine();
                     // Kontrollera om meddelandet är tomt
                     while (message == "")
                     {
                         Console.WriteLine("Du måste skriva ett inlägg!");
                         message = Console.ReadLine();
                     }
                
                     Entry newEntry = new Entry(name, message);
                
                     // Lägg till gäst i lista
                     //entries.Add(newEntry);
                     
                     // Spara inlägg i gästbok
                     guestbook.AddEntry(newEntry);
                     guestbook.WriteEntries();
                 } else if (key == "2")
                 {
                     // Ta bort
                     Console.WriteLine("Ange ett ID för det inlägg som ska tas bort");
                     string id = Console.ReadLine();

                     try
                     {
                         int idInt = Convert.ToInt32(id);
                         Console.WriteLine(idInt);

                         if (guestbook.DeleteEntry(idInt))
                         {
                             Console.WriteLine("Inlägget togs bort.");
                         }
                         else
                         {
                             Console.WriteLine("Felaktigt ID.");
                         }
                     }
                     catch (FormatException)
                     {
                         Console.WriteLine("Du måste ange en siffra");
                     }
                     catch (OverflowException)
                     {
                         Console.WriteLine("Det var lite överdrivet va.. Testa igen");
                     }

                     guestbook.WriteEntries();
                 } else if (key == "x")
                 {
                     break;
                 }
                 else
                 {
                     Console.WriteLine("Du kan endast välja mellan alternativen i listan. 1 för nytt inlägg, 2 för att ta bort ett inlägg, 'x' för att avsluta programmet.");
                 }
            }
        }
    }
}