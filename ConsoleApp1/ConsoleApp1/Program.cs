using System;

namespace ConsoleApp1
{

    class Cat
    {
        public string Color;
        public int LegsCount;
        
        public Cat()
        {
            Color = "black";
            LegsCount = 4;
        }

        public void Кушать()
        {
            
            Console.WriteLine("ням");
        }      
        
        public void Мяукать()
        {
            Console.WriteLine("Мяу");
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var cat = new Cat();
            
            cat.Кушать();
            cat.Мяукать();
            
        }
    }
}