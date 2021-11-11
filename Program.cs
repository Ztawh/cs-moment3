using System;
using System.Collections.Generic;
using System.IO;

/* Skrivet av Amanda Hwatz Björkholm 2021
 Moment 3 i kursen Programmering i C# .NET*/

namespace Moment3
{
    // Klass för inlägg
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

    // Klass för gästbok
    public class Guestbook
    {
        private List<Entry> _entryList;

        public Guestbook()
        {
            // Hämta alla rader från textfilen och skapa objekt
            StreamReader sr = new StreamReader("users.txt");
            string line = "";
            this._entryList = new List<Entry>();

            // Loopa igenom alla rader
            while ((line = sr.ReadLine()) != null)
            {
                if (line == "")
                {
                    continue;
                }
                
                string[] lineArr = line.Split(" - "); // Separera rader på " - " för att få ut namn och meddelande separat
                Entry readEntry = new Entry(lineArr[0], lineArr[1]);
                
                // Spara objekt till listan
                this._entryList.Add(readEntry);
            }
            sr.Close(); // Stäng StreamReader
        }
        
        public void AddEntry(Entry entry)
        {
            // Lägg till i filen
            StreamWriter sw = new StreamWriter("users.txt", true);

            sw.WriteLine($"{entry.GetName()} - {entry.GetMessage()}");
            sw.Close();
            
            // Lägg till i listan
            this._entryList.Add(entry);
        }

        public void WriteEntries()
        {
            // Skriv ut allt i listan
            int i = 0;
            foreach (var entry in this._entryList)
            {
                Console.WriteLine($"[{i}] {entry.GetName()} - {entry.GetMessage()}");
                i += 1;
            }
        }

        public bool DeleteEntry(int id)
        {
            // Ta bort inlägg med ID
            try
            {
                this._entryList.RemoveAt(id);
            }
            catch (ArgumentOutOfRangeException) // Om inmatat id är för stort eller för litet
            {
                return false;
            }
            
            //Spara ny lista
            StreamWriter sw = new StreamWriter("users.txt");
            
            foreach (var entry in this._entryList)
            {
                sw.WriteLine($"{entry.GetName()} - {entry.GetMessage()}");
            }
            
            sw.Close(); // Stäng StreamWriter
            return true;
        }
    }
    
    internal static class Program
    {
        private static void Main(string[] args)
        {
            // Skapa instans av klass
            Guestbook guestbook = new Guestbook();

            // Kör meny
            while (true)
            {
                Console.WriteLine("A M A N D A S  G Ä S T B O K");
                
                 // Meny
                 Console.WriteLine("1. Skriv i gästboken");
                 Console.WriteLine("2. Ta bort inlägg");
                 Console.WriteLine();
                 Console.WriteLine("X. Avsluta");
                 Console.WriteLine();

                 // Skriv ut inlägg
                 guestbook.WriteEntries();
                
                 // Läs in inmatat tecken
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
                         // Hämta namn tills det inte är tomt
                         Console.WriteLine("Du måste ange ett namn!");
                         name = Console.ReadLine();
                     }
                
                     Console.WriteLine("Skriv ett inlägg"); 
                     string message = Console.ReadLine();
                     // Kontrollera om meddelandet är tomt
                     while (message == "")
                     {
                         // Hämta meddelande tills det inte är tomt
                         Console.WriteLine("Du måste skriva ett inlägg!");
                         message = Console.ReadLine();
                     }
                
                     // Skapa inläggs-objekt av de inmatade värdena
                     Entry newEntry = new Entry(name, message);

                     // Spara inlägg i gästbok
                     guestbook.AddEntry(newEntry);
                     guestbook.WriteEntries();
                 } else if (key == "2")
                 {
                     // Ta bort
                     // Hämta ett ID från användaren
                     Console.WriteLine("Ange ett ID för det inlägg som ska tas bort");
                     string id = Console.ReadLine();

                     try // Testa konvertera string till integer
                     {
                         int idInt = Convert.ToInt32(id); // Konvertera string till int

                         // Om det gick att ta bort inlägget
                         if (guestbook.DeleteEntry(idInt))
                         {
                             Console.WriteLine("Inlägget togs bort.");
                         }
                         else // Om det inte gick att ta bort inlägget
                         {
                             Console.WriteLine("Felaktigt ID.");
                         }
                     }
                     catch (FormatException) // Om en bokstav har matats in
                     {
                         Console.WriteLine("Du måste ange en siffra");
                     }
                     catch (OverflowException) // Om siffran tar upp för mycket minne
                     {
                         Console.WriteLine("Det var lite överdrivet va.. Testa igen");
                     }
                     
                 } else if (key == "x")
                 {
                     break; // Avsluta programmet
                 }
                 else // Om användaren matar in något annat än menyalternativen
                 {
                     Console.WriteLine("Du kan endast välja mellan alternativen i listan. 1 för nytt inlägg, 2 för att ta bort ett inlägg, 'x' för att avsluta programmet.");
                 }
            }
        }
    }
}