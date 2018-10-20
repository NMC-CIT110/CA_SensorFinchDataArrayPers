using FinchAPI;
using System;

namespace CA_SensorFinchDataArrayPers
{
    class Program
    {
        /// <summary>
        /// Main method - application starting point
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Finch freddy = new Finch();

            DisplayOpeningScreen();
            DisplayMainMenu(freddy);
            DisplayClosingScreen(freddy);
        }

        /// <summary>
        /// display main menu
        /// </summary>
        static void DisplayMainMenu(Finch freddy)
        {
            string menuChoice;
            bool exiting = false;
            int numberOfDataPoints = 0;
            double secondsBetweenDataPoints = 0;
            double[] temperatures = null;

            while (!exiting)
            {
                DisplayHeader("Main Menu");

                //
                // display main menu
                //
                Console.WriteLine("\tA) Connect to Finch Robot");
                Console.WriteLine("\tB) Setup Application");
                Console.WriteLine("\tC) Acquire Data");
                Console.WriteLine("\tD) Display Data");
                Console.WriteLine("\tQ) Quit");
                Console.WriteLine();
                Console.Write("\tEnter Choice:");
                menuChoice = Console.ReadLine().ToUpper();

                //
                // process main menu
                //
                switch (menuChoice)
                {
                    case "A":
                        DisplayConnectToFinch(freddy);
                        break;

                    case "B":
                        numberOfDataPoints = DisplayGetNumberOfDataPoints();
                        secondsBetweenDataPoints = DisplayGetSecondsBetweenDataPoints();

                        break;

                    case "C":
                        temperatures = DisplayAcquireDataSet(numberOfDataPoints, secondsBetweenDataPoints, freddy);
                        break;

                    case "D":
                        DisplayDataSet(temperatures);
                        break;

                    case "Q":
                        exiting = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("Please enter the letter of your menu choice.");

                        DisplayContinuePrompt();
                        break;
                }
            }
        }

        /// <summary>
        /// display a list of the data points
        /// </summary>
        static void DisplayDataSetList(double[] temperatures)
        {
            Console.WriteLine();
            Console.WriteLine("Data Points Recorded");
            for (int index = 0; index < temperatures.Length; index++)
            {
                Console.WriteLine($"\t\tData Point {index + 1}: {temperatures[index]}");
            }
        }

        /// <summary>
        /// display the data set
        /// </summary>
        static void DisplayDataSet(double[] temperatures)
        {
            DisplayHeader("Current Data Set");

            DisplayDataSetList(temperatures);

            DisplayContinuePrompt();
        }

        /// <summary>
        /// acquire data 
        /// </summary>
        static double[] DisplayAcquireDataSet(int numberOfDataPoints, double secondsBetweenDataPoints, Finch freddy)
        {
            double[] temperatures = new double[numberOfDataPoints];

            DisplayHeader("Acquire Data Set");

            //
            // pause for user
            //
            Console.WriteLine("The Finch Robot is ready. Press Enter to begin.");
            Console.ReadLine();

            //
            // acquire data
            //
            for (int index = 0; index < numberOfDataPoints; index++)
            {
                temperatures[index] = freddy.getTemperature();
                Console.WriteLine($"Data Point {index + 1}: {temperatures[index]}");
                freddy.wait((int)(secondsBetweenDataPoints * 1000));
            }

            DisplayDataSetList(temperatures);

            DisplayContinuePrompt();

            return temperatures;
        }

        /// <summary>
        /// get number of data points
        /// </summary>
        static int DisplayGetNumberOfDataPoints()
        {
            int numberOfDataPoints;

            DisplayHeader("Number of Data Points");

            Console.Write("Enter the number of data points:");
            numberOfDataPoints = int.Parse(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine($"Number of Data Points: {numberOfDataPoints}");

            DisplayContinuePrompt();

            return numberOfDataPoints;
        }

        /// <summary>
        /// get seconds between data points
        /// </summary>
        static double DisplayGetSecondsBetweenDataPoints()
        {
            double secondsBetweenDataPoints;

            DisplayHeader("Seconds Between Data Points");

            Console.Write("Enter the seconds between data points:");
            secondsBetweenDataPoints = double.Parse(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine("Seconds Between Data Points {0}", secondsBetweenDataPoints);

            DisplayContinuePrompt();

            return secondsBetweenDataPoints;
        }

        /// <summary>
        /// connect to Finch Robot
        /// </summary>
        static void DisplayConnectToFinch(Finch freddy)
        {
            DisplayHeader("Connect to Finch");

            if (freddy.connect())
            {
                Console.WriteLine("Finch Robot is connected and ready.");
            }
            else
            {
                Console.WriteLine("Finch Robot is not connect. Please check the wires.");
            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display opening screen
        /// </summary>
        static void DisplayOpeningScreen()
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("\t\tWelcome to the finch Recorder App");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display closing screen
        /// </summary>
        static void DisplayClosingScreen(Finch freddy)
        {
            Console.Clear();

            freddy.disConnect();

            Console.WriteLine();
            Console.WriteLine("\t\tThanks for using the Finch Recorder App");
            Console.WriteLine();

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        #region  HELPER METHODS

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display header
        /// </summary>
        static void DisplayHeader(string headerTitle)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerTitle);
            Console.WriteLine();
        }

        #endregion
    }
}
