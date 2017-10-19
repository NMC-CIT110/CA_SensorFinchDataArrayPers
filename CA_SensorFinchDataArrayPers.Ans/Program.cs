using FinchAPI;
using System;
using System.IO;

namespace CA_SensorFinchDataArrayPers
{
    class Program
    {
        //
        // global variables
        //
        static Finch freddy = new Finch();
        static int numberOfDataPoints;
        static double secondsBetweenDataPoints;
        static double[] temperatures;
        static string[] temperaturesAsString;
        static string dataPath;

        /// <summary>
        /// Main method - application starting point
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            DisplayOpeningScreen();
            DisplayMainMenu();
            DisplayClosingScreen();
        }

        /// <summary>
        /// display menu
        /// </summary>
        static void DisplayMainMenu()
        {
            string menuChoice;
            bool exiting = false;

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
                Console.Write("Enter Choice:");
                menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case "A":
                    case "a":
                        DisplayConnectToFinch();
                        break;

                    case "B":
                    case "b":
                        DisplaySetupApplication();
                        break;

                    case "C":
                    case "c":
                        DisplayAcquireDataSet();
                        break;

                    case "D":
                    case "d":
                        DisplayDataSet();
                        break;

                    case "E":
                    case "e":
                        DisplaySaveDataSet();
                        break;

                    case "F":
                    case "f":
                        DisplayRetrieveDataSet();
                        break;

                    case "Q":
                    case "q":
                        exiting = true;
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// retrieve the data to a text file
        /// </summary>
        static void DisplayRetrieveDataSet()
        {
            DisplayHeader("Retrieve Data");

            //
            // set the data path
            //
            dataPath = "Data\\Data.txt";

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
        }

        /// <summary>
        /// save the data to a text file
        /// </summary>
        static void DisplaySaveDataSet()
        {
            DisplayHeader("Save Data");

            //
            // set the data path
            //
            dataPath = "Data\\Data.txt";

            //
            // create (instantiate) the string array
            //
            temperaturesAsString = new string[numberOfDataPoints];

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
        static void DisplayDataSetList()
        {
            //
            // display data
            //
            Console.WriteLine();
            Console.WriteLine("Data Points");
            for (int index = 0; index < numberOfDataPoints; index++)
            {
                Console.WriteLine($"\t\tData Point {index + 1}: {temperatures[index]}");
            }
        }

        /// <summary>
        /// display the data set
        /// </summary>
        static void DisplayDataSet()
        {
            DisplayHeader("Current Data Set");

            DisplayDataSetList();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// acquire data 
        /// </summary>
        static void DisplayAcquireDataSet()
        {
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

            DisplayDataSetList();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// get the application parameters
        /// </summary>
        static void DisplaySetupApplication()
        {
            DisplayHeader("Setup Application");

            Console.Write("Enter the number of data points:");
            numberOfDataPoints = int.Parse(Console.ReadLine());

            Console.Write("Enter the seconds between data points:");
            secondsBetweenDataPoints = double.Parse(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine($"Number of Data Points: {numberOfDataPoints}");
            Console.WriteLine("Seconds Between Data Points {0}", secondsBetweenDataPoints);

            //
            // create (instantiate) the array
            //
            temperatures = new double[numberOfDataPoints];

            DisplayContinuePrompt();
        }

        /// <summary>
        /// connect to Finch Robot
        /// </summary>
        static void DisplayConnectToFinch()
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
        static void DisplayClosingScreen()
        {
            Console.Clear();

            freddy.disConnect();

            Console.WriteLine();
            Console.WriteLine("\t\tThanks for using the Finch Recorder App");
            Console.WriteLine();

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

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
    }
}
