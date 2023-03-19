using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Diagnostics;
using System.Threading;

namespace ConsoleSnakeGame
{
    class Snake
    {
        public int SnakeX, SnakeY;

        public Snake()
        {
            SnakeX = 0;
            SnakeY = 0;
        }

        public int GetX
        {
            set
            {
                SnakeX = value;
            }
            get
            {
                return SnakeX;
            }
        }

        public int GetY
        {
            set
            {
                SnakeY = value;
            }
            get
            {
                return SnakeY;
            }
        }
    }

    public class Food
    {
        public int FruitsX, FruitsY;

        public Food()
        {
            FruitsX = 0;
            FruitsY = 0;
        }

        public int GetX
        {
            set
            {
                FruitsX = value;
            }
            get
            {
                return FruitsX;
            }
        }

        public int GetY
        {
            set
            {
                FruitsY = value;
            }
            get
            {
                return FruitsY;
            }
        }
    }

    public class MapSetting
    {
        private List<Snake> Usersnake = new List<Snake>();
        private Food Somefruits = new Food();

        private bool Gameover = false;

        Stopwatch Survivesec = new Stopwatch();
        TimeSpan ts;
        String Viewsec;
        System.Timers.Timer timer = new System.Timers.Timer();

        private int Choice = 6;
        static int SizeX = 20;
        static int SizeY = 20;
        char[,] Map = new char[SizeX, SizeY];

        private void GameSetting()
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    if (i == 0 || i == SizeX - 1 || j == 0 || j == SizeY - 1)
                    {
                        Map[i, j] = '#';
                    }
                }
            }
        }

        private void PrintMap()
        {
            int TempX, TempY;

            ts = Survivesec.Elapsed;
            Viewsec = String.Format("{00}", ts.Seconds);

            Console.WriteLine("Now Your Snake's body size: " + Usersnake.Count);
            Console.WriteLine("Sec: " + Viewsec);

            TempX = Somefruits.FruitsX;
            TempY = Somefruits.FruitsY;

            Map[TempX, TempY] = '@';

            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    for (int k = 0; k < Usersnake.Count; k++)
                    {
                        if (Usersnake.Any())
                        {
                            TempX = Usersnake[k].GetX;
                            TempY = Usersnake[k].GetY;

                            if (k == 0)
                            {
                                Map[TempX, TempY] = 'o';
                            }
                            else
                            {
                                Map[TempX, TempY] = 'o';
                            }
                        }
                    }
                    Console.Write(Map[i, j]);
                }
                Console.WriteLine();
            }
        }

        private void GameFunc()
        {
            Usersnake.Clear();

            Snake sn = new Snake();

            sn.GetX = 9;
            sn.GetY = 9;

            GenerateFood();
            Usersnake.Add(sn);

            timer.Interval = 80;
            timer.Elapsed += timer_Elapsed;

            timer.Start();
            Survivesec.Start();

            while (Gameover == false)
            {
                ControlSnake();
            }
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            MapSetting Map = this;

            Console.Clear();

            Console.WriteLine("ArrowKey = Moving, A Key = Pause, Q Key = Exit");
            CheckSnake();

            if (Gameover == true)
            {
                return;
            }
            else if (Gameover == false)
            {
                if (Choice == 4)
                {
                    Usersnake[0].GetY--;
                }
                else if (Choice == 6)
                {
                    Usersnake[0].GetY++;
                }
                else if (Choice == 8)
                {
                    Usersnake[0].GetX--;
                }
                else if (Choice == 2)
                {
                    Usersnake[0].GetX++;
                }

                GameSetting();
                PrintMap();
            }
        }

        private void ControlSnake()
        {
            if (Gameover == true)
            {
                return;
            }
            ConsoleKeyInfo InputKey;

            InputKey = Console.ReadKey(true);
            
            switch (InputKey.Key)
            {
                case ConsoleKey.LeftArrow:
                    if (Choice == 6)
                    {
                        break;
                    }
                    Choice = 4;
                    break;

                case ConsoleKey.RightArrow:
                    if (Choice == 4)
                    {
                        break;
                    }
                    Choice = 6;
                    break;

                case ConsoleKey.UpArrow:
                    if (Choice == 2)
                    {
                        break;
                    }
                    Choice = 8;
                    break;

                case ConsoleKey.DownArrow:
                    if (Choice == 8)
                    {
                        break;
                    }
                    Choice = 2;
                    break;

                case ConsoleKey.A:
                    Survivesec.Stop();
                    timer.Stop();

                    Thread.Sleep(50);
                    Console.Clear();

                    Console.WriteLine("Snake's status");
                    for (int i = 0; i < Usersnake.Count; i++)
                    {
                        Console.WriteLine(i + "'s status");
                        Console.WriteLine("     X : " + Usersnake[i].GetX);
                        Console.WriteLine("     Y : " + Usersnake[i].GetY);
                    }
                    Console.ReadKey();

                    timer.Start();
                    Survivesec.Start();
                    break;

                case ConsoleKey.Q:
                    Die();
                    break;
            }
        }

        private void CheckSnake()
        {
            int OldX = 0, OldY = 0;
            int TempX = 0, TempY = 0;

            if (Usersnake[0].GetX == 0 || Usersnake[0].GetX == SizeX - 1 || Usersnake[0].GetY == 0 || Usersnake[0].GetY == SizeY - 1)
            {
                Die();
                return;
            }

            for (int i = 1; i < Usersnake.Count; i++)
            {
                if (Usersnake[0].GetX == Usersnake[i].GetX && Usersnake[0].GetY == Usersnake[i].GetY)
                {
                    Die();
                    return;
                }
            }

            TempX = Usersnake[0].GetX;
            TempY = Usersnake[0].GetY;

            if (Usersnake.Count == 1)
            {
                OldX = TempX;
                OldY = TempY;
            }
            else
            {
                for (int i = 1; i < Usersnake.Count; i++)
                {
                    if (i % 2 != 0)
                    {
                        OldX = Usersnake[i].GetX;
                        OldY = Usersnake[i].GetY;

                        Usersnake[i].GetX = TempX;
                        Usersnake[i].GetY = TempY;
                    }
                    else if (i % 2 == 0)
                    {
                        TempX = Usersnake[i].GetX;
                        TempY = Usersnake[i].GetY;

                        Usersnake[i].GetX = OldX;
                        Usersnake[i].GetY = OldY;
                    }
                }
                if (Usersnake.Count % 2 != 0)
                {
                    OldX = TempX;
                    OldY = TempY;
                }
            }

            Map[OldX, OldY] = ' ';

            if (Usersnake[0].GetX == Somefruits.FruitsX && Usersnake[0].GetY == Somefruits.FruitsY)
            {
                EatFruits(OldX, OldY);
                GenerateFood();
            }
        }

        private void EatFruits(int OldX, int OldY)
        {
            Snake Newbody = new Snake
            {
                SnakeX = OldX,
                SnakeY = OldY
            };

            Usersnake.Add(Newbody);
        }

        private void GenerateFood()
        {
            int LimitX = SizeX - 1;
            int LimitY = SizeY - 1;

            Random random = new Random();
            Somefruits = new Food();

            Somefruits.FruitsX = random.Next(1, LimitX);
            Somefruits.FruitsY = random.Next(1, LimitY);

            for (int i = 0; i < Usersnake.Count; i++)
            {
                if (Somefruits.FruitsX == Usersnake[i].GetX && Somefruits.FruitsY == Usersnake[i].GetY)
                {
                    Somefruits.FruitsX = random.Next(1, LimitX);
                    Somefruits.FruitsY = random.Next(1, LimitY);

                    i--;
                    continue;
                }
            }
        }

        private void Die()
        {
            Gameover = true;

            timer.Stop();
            Survivesec.Stop();

            Console.Clear();
            Console.WriteLine("Your Survived Time is {0} sec.", Viewsec);
            Console.WriteLine("and Your Snake's size is {0}.", Usersnake.Count);
            Console.WriteLine("Game Over");
        }

        public static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("############################################################");
            Console.WriteLine("######+++*+*+##+%#####+%###+*+*+%###+*#####+*#@+*+*++*+=####");
            Console.WriteLine("######*=*:*:*##*@#####*%###*:*:*@###*:#####*:##*:*:**:*=####");
            Console.WriteLine("####=*#########=%#####==#*=#####%=##+*###*=###@=############");
            Console.WriteLine("####*:#########*@#####*@#:*#####@*##*:###:*####*############");
            Console.WriteLine("####=*#########=+=####=%#*=#####%=##+*#%=#####@=############");
            Console.WriteLine("####+==@=%=####+@%%=##+@#=+=%=%=@+##+==@%######+%=%==%######");
            Console.WriteLine("####=*=+=*=####=%#*+##=%#*=+*=*=+=##+*==######@=*=*=+*######");
            Console.WriteLine("######@#@#@++##+####+=+@#++##@#@@+##++@@+######+#@##########");
            Console.WriteLine("###########*+##=%###+*=%#*=#####%=##+*#%=#####@=############");
            Console.WriteLine("###########+*##*@#####*@#+*#####@*##*+###**####*############");
            Console.WriteLine("###########*+##=%#####=%#*=#####%=##+*###*=###@=############");
            Console.WriteLine("####***%***####*@#####*@#**#####@*##**#####**##********%####");
            Console.WriteLine("####=*=+=*=####=%#####=%#*+#####%=##+*#####+*#@=*=*=+*=+####");
            Console.WriteLine("############################################################");
            Console.WriteLine("Happy enjoy with my game!! push the any button to Start");
            Console.ReadKey(true);

            while(true)
            {
                MapSetting Map = new MapSetting();
                Map.GameFunc();

                if (Map.Gameover == true)
                {
                    Console.WriteLine("Retry?? Y/N");
                    back:switch(Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.Y:
                            continue;

                        case ConsoleKey.N:
                            Console.WriteLine("Ok!! See ya!");
                            Thread.Sleep(1000);
                            Environment.Exit(0);
                            break;

                        default:
                            Console.WriteLine("What?? Just push the Y or N button.");
                            goto back;
                    }
                }
            }
        }
    }
}
