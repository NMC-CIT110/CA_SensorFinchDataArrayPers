using FinchAPI;
using System;
using System.IO;

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
        /// display menu
        /// </summary>
        static void DisplayMainMenu(Finch freddy)
        {
            string menuChoice;
            bool exiting = false;
            int numberOfDataPoints = 0;
            double secondsBetweenDataPoints = 0;
            double[] temperatures = null;

            //
            // define the path to the data file
            //
            string dataPath = @"Data\Data.txt";

            while (!exiting)
            {
                DisplayHeader("Main Menu");

                Console.WriteLine("\tA) Connect to Finch Robot");
                Console.WriteLine("\tB) Setup Application");
                Console.WriteLine("\tC) Acquire Data");
                Console.WriteLine("\tD) Display Data");
                Console.WriteLine("\tE) Save Data");
                Console.WriteLine("\tF) Retrieve Data");
                Console.WriteLine("\tQ) Quit");
                Console.WriteLine();
                Console.Write("\tEnter Choice:");
                menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case "A":
                    case "a":
                        DisplayConnectToFinch(freddy);
                        break;

                    case "B":
                    case "b":
                        numberOfDataPoints = DisplayGetNumberOfDataPoints();
                        secondsBetweenDataPoints = DisplayGetSecondsBetweenDataPoints();
                        break;

                    case "C":
                    case "c":
                        temperatures = DisplayAcquireDataSet(numberOfDataPoints, secondsBetweenDataPoints, freddy);
                        break;

                    case "D":
                    case "d":
                        DisplayDataSet(temperatures);
                        break;

                    case "E":
                    case "e":
                        DisplaySaveDataSet(dataPath, numberOfDataPoints, temperatures);
                        break;

                    case "F":
                    case "f":
                        temperatures = DisplayRetrieveDataSet(dataPath);
                        break;

                    case "Q":
                    case "q":
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
        /// retrieve the data to a text file
        /// </summary>
        static double[] DisplayRetrieveDataSet(string dataPath)
        {
            string[] temperaturesAsString;
            double[] temperatures;
            int numberOfDataPoints;

            DisplayHeader("Retrieve Data");

            Console.WriteLine("Ready to retrieve data. Press Enter to continue.");
            Console.ReadLine();

            //
            // read the data file into a string array
            // note that the ReadAllLines methods sets the length of the array
            //
            temperaturesAsString = File.ReadAllLines(dataPath);

            //
            // get the number of data points using the Length property of the array
            //
            numberOfDataPoints = temperaturesAsString.Length;

            //
            // create (instantiate) the temperatures array
            //
            temperatures = new double[numberOfDataPoints];

            //
            // convert the temperatures string array to an array of doubles
            //
            for (int index = 0; index < numberOfDataPoints; index++)
            {
                temperatures[index] = double.Parse(temperaturesAsString[index]);
            }

            Console.WriteLine("Data retrieved correctly.");

            DisplayContinuePrompt();

            return temperatures;
        }

        /// <summary>
        /// save the data to a text file
        /// </summary>
        static void DisplaySaveDataSet(string dataPath, int numberOfDataPoints, double[] temperatures)
        {
            DisplayHeader("Save Data");

            //
            // create (instantiate) the string array
            //
            string[] temperaturesAsString = new string[numberOfDataPoints];

            //
            // convert the temperatures array to an array of strings
            //
            for (int index = 0; index < numberOfDataPoints; index++)
            {
                temperaturesAsString[index] = temperatures[index].ToString();
            }

            Console.WriteLine("Ready to save data. Press Enter to continue.");
            Console.ReadLine();

            //
            // write the string array to the data file
            //
            File.WriteAllLines(dataPath, temperaturesAsString);

            Console.WriteLine("Data saved correctly.");

            DisplayContinuePrompt();
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
                freddy.setLED(0, 255, 0);
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
