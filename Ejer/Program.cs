using System;
using System.Threading;
using System.Threading.Tasks;

namespace _01_Ejemplo_HolaMundo
{
    class Program
    {
        const string DEFAULT_BLOCK = "▄";
        const string JUMPING_BLOCK = "▀";
        const string WALL = "▓";
        const string SKY_SPACE = " ";
        const string FLOOR_SPACE = "_";
        const string CLOUD = "~";


        static string prevSky = "  ";
        static string nextSky = "                            ";
        static string prevFloor = "__";
        static string nextFloor = "____________________________";

        static string topPosition = SKY_SPACE;
        static string bottomPosition = DEFAULT_BLOCK;
        static string backFloorSpace = FLOOR_SPACE;
        static string backSkySpace = SKY_SPACE;

        static int parallaxCont = 0;


        static void Main(string[] args)
        {

            Console.CursorVisible = false;


            bool exit = false;

            do
            {
                EmptyRoad();
                RenderGame();
                Console.WriteLine("Space to Start playing. Esc to Exit.");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Spacebar:
                        Console.Clear();
                        StartPlaying();
                        break;

                    case ConsoleKey.Escape:
                        Console.Clear();
                        Console.WriteLine("RunN'Jump was closed.");
                        exit = true;
                        break;
                }

            } while (!exit);
        }
        static void StartPlaying()
        {
            //int initialSleepTime = 75; //so slow

            int initialSleepTime = 50;

            var rand = new Random();
            int jump = -1;
            int speedBoost = 0;

            //int point = 0;

            bool exit = false;

            do
            {
                TimeSpan Sleep = TimeSpan.FromMilliseconds(initialSleepTime);
                int random = rand.Next(101);
                while (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.Spacebar:
                            if (bottomPosition == DEFAULT_BLOCK)
                            {
                                jump = 0;
                            }
                            break;
                    }
                }

                if (jump != -1)
                {
                    JumpRender(jump);
                    jump++;

                    if (jump == 6)
                    {
                        jump = -1;
                    }
                }

                if (random < 5)
                {
                    WallRender();
                }

                if (random > 90)
                {
                    CloudRender();
                }

                Thread.Sleep(Sleep);

                speedBoost++;

                if(speedBoost == 50)
                {
                    if (initialSleepTime < 20)
                    {
                        initialSleepTime--;
                    }

                    speedBoost = 0;
                }

                exit = HorizontalScroll();
                RenderGame();
            } while (!exit);
        }

        static void WallRender()
        {
            if (nextFloor.Substring(nextFloor.Length - 5).IndexOf(WALL) == -1)
            {
                nextFloor = nextFloor.Substring(1) + WALL;
            }
        }

        static void CloudRender()
        {
            nextSky = nextSky.Substring(1) + CLOUD;
        }

        static bool HorizontalScroll()
        {
            bool exit = false;

            if (nextFloor.Substring(0, 1) == WALL && bottomPosition != FLOOR_SPACE)
            {
                exit = true;
                Console.WriteLine("\nGAME OVER");

                Thread.Sleep(TimeSpan.FromMilliseconds(1500));

                Console.Clear();

            }
            else
            {
                prevFloor = prevFloor.Substring(1) + backFloorSpace;
                backFloorSpace = nextFloor.Substring(0, 1);
                nextFloor = nextFloor.Substring(1) + FLOOR_SPACE;

                if(backFloorSpace == WALL)
                {
                    bottomPosition = WALL;
                }

                if(parallaxCont == 0)
                {
                    prevSky = prevSky.Substring(1) + backSkySpace;
                    backSkySpace = nextSky.Substring(0, 1);
                    nextSky = nextSky.Substring(1) +SKY_SPACE;
                }
                parallaxCont++;
                if(parallaxCont == 5)
                {
                    parallaxCont = 0;
                }
            }
            return exit;
        }

        static void RenderGame()
        {
            if (Console.CursorTop > 0)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                ClearCurrentConsoleLine();
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                ClearCurrentConsoleLine();
            }
            else
            {
                Console.Clear();
            }
            Console.WriteLine(prevSky + topPosition + nextSky);
            Console.WriteLine(prevFloor + bottomPosition + nextFloor);
        }

        static void JumpRender(int jumpOnTime)
        {
            switch (jumpOnTime)
            {
                case 0:
                    topPosition = SKY_SPACE;
                    bottomPosition = JUMPING_BLOCK;
                    break;

                case 1:
                    topPosition = DEFAULT_BLOCK;
                    bottomPosition = backFloorSpace;
                    break;

                case 2:
                    topPosition = JUMPING_BLOCK;
                    bottomPosition = backFloorSpace;
                    break;

                case 3:
                    topPosition = DEFAULT_BLOCK;
                    bottomPosition = backFloorSpace;
                    break;

                case 4:
                    topPosition = SKY_SPACE;
                    bottomPosition = JUMPING_BLOCK;
                    break;

                case 5:
                    topPosition = SKY_SPACE;
                    bottomPosition = DEFAULT_BLOCK;
                    break;

                default:
                    break;
            }
        }

        static void EmptyRoad()
        {
            prevFloor = "__";
            nextFloor = "____________________________";
            prevSky = "  ";
            nextSky = "                            ";

            topPosition = SKY_SPACE;
            bottomPosition = DEFAULT_BLOCK;
            backFloorSpace = FLOOR_SPACE;
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}