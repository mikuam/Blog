using Colorful;
using System.Drawing;
using System.Threading;
using Console = Colorful.Console;

namespace MichalBialecki.com.ConsoleAppMagic
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Hello World!");
            Console.ReadKey();

            // ShowSimplePercentage();
            // ShowSpinner();
            // MultiLineAnimation();
            // ColorfulText();
            // ColorfulAnimation();
            AsciiText();
            
            Console.ReadKey();
        }

        static void ShowSimplePercentage()
        {
            for (int i = 0; i <= 100; i++)
            {
                Console.Write($"\rProgress: {i}%   ");
                Thread.Sleep(25);
            }

            Console.Write("\rDone!          ");
        }

        static void ShowSpinner()
        {
            var counter = 0;
            for (int i = 0; i < 50; i++)
            {
                switch (counter % 4)
                {
                    case 0: Console.Write("/"); break;
                    case 1: Console.Write("-"); break;
                    case 2: Console.Write("\\"); break;
                    case 3: Console.Write("|"); break;
                }
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                counter++;
                Thread.Sleep(100);
            }
        }

        static void MultiLineAnimation()
        {
            var counter = 0;
            for (int i = 0; i < 30; i++)
            {
                Console.Clear();

                switch (counter % 4)
                {
                    case 0:
                        {
                            Console.WriteLine("╔════╤╤╤╤════╗");
                            Console.WriteLine("║    │││ \\   ║");
                            Console.WriteLine("║    │││  O  ║");
                            Console.WriteLine("║    OOO     ║");
                            break;
                        };
                    case 1:
                        {
                            Console.WriteLine("╔════╤╤╤╤════╗");
                            Console.WriteLine("║    ││││    ║");
                            Console.WriteLine("║    ││││    ║");
                            Console.WriteLine("║    OOOO    ║");
                            break;
                        };
                    case 2:
                        {
                            Console.WriteLine("╔════╤╤╤╤════╗");
                            Console.WriteLine("║   / │││    ║");
                            Console.WriteLine("║  O  │││    ║");
                            Console.WriteLine("║     OOO    ║");
                            break;
                        };
                    case 3:
                        {
                            Console.WriteLine("╔════╤╤╤╤════╗");
                            Console.WriteLine("║    ││││    ║");
                            Console.WriteLine("║    ││││    ║");
                            Console.WriteLine("║    OOOO    ║");
                            break;
                        };
                }

                counter++;
                Thread.Sleep(200);
            }
        }

        static void ColorfulText()
        {
            Console.WriteLine("This is a standard info message");
            Console.WriteLine("This is an error!", Color.Red);
        }

        static void ColorfulAnimation()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    Console.Clear();

                    // steam
                    Console.Write("       . . . . o o o o o o", Color.LightGray);
                    for (int s = 0; s < j / 2; s++)
                    {
                        Console.Write(" o", Color.LightGray);
                    }
                    Console.WriteLine();

                    var margin = "".PadLeft(j);
                    Console.WriteLine(margin + "                _____      o", Color.LightGray);
                    Console.WriteLine(margin + "       ____====  ]OO|_n_n__][.", Color.DeepSkyBlue);
                    Console.WriteLine(margin + "      [________]_|__|________)< ", Color.DeepSkyBlue);
                    Console.WriteLine(margin + "       oo    oo  'oo OOOO-| oo\\_", Color.Blue);
                    Console.WriteLine("   +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+", Color.Silver);

                    Thread.Sleep(200);
                }
            }
        }

        static void AsciiText()
        {
            Console.WriteAscii("MichalBialecki.com", Color.FromArgb(131, 184, 214));

            Console.WriteLine();

            var font = FigletFont.Load("larry3d.flf");
            Figlet figlet = new Figlet(font);
            Console.WriteLine(figlet.ToAscii("MichalBialecki.com"), Color.FromArgb(67, 144, 198));
        }
    }
}
