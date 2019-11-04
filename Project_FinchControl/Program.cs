using FinchAPI;
using System;
using System.Collections.Generic;
using System.IO;

namespace Project_FinchControl
{

    //_____________________________________________________
    // Title: Finch Control
    // Description: 
    // Application Type: Console
    // Author: Kendrick
    // Dated Created: 
    // Last Modified: 
    // ____________________________________________________

    class Program
    {

        private enum FinchCommand
        {
            NONE,
            MOVEFORWARD,
            MOVEBACKWARD,
            STOPMOTORS,
            WAIT,
            TURNRIGHT,
            TURNLEFT,
            LEDON,
            LEDOFF,
            DONE

        }
        static void Main(string[] args)
        {
            setTheme();
            DisplayWelcomeScreen();
            DisplayMainMenu();
            DisplayClosingScreen();
        }

        #region Login


        //static bool DisplayLogin()
        //{
        //    string dataPath = @"Data\login.txt";
        //    string menuChoice;

        //    menuChoice = Console.ReadLine().ToUpper();

        //    switch (menuChoice)
        //    {
        //        case "A":

        //            break;
        //        case "B":

        //            break;
        //        default:
        //            break;
        //    }



        //}
        #endregion

        static void setTheme()
        {
            string dataPath = @"Data\Theme.txt";
            string foregroundColorString;
            ConsoleColor foregroundColor;
            foregroundColorString = File.ReadAllText(dataPath);

            Enum.TryParse(foregroundColorString, out foregroundColor);

            Console.ForegroundColor = foregroundColor;
        }

        static void DisplayMainMenu()
        {
            Finch finchRobot = new Finch();

            bool finchRobotConnected = false;
            bool quitApplication = false;
            string menuChoice;


            do
            {
                DisplayScreenHeader("Main Menu");
                Console.WriteLine("a.) Connect Finch Robot");
                Console.WriteLine("b.) Talent Show");
                Console.WriteLine("c.) Data Recorder");
                Console.WriteLine("d.) Alarm System");
                Console.WriteLine("e.) User Programing");
                Console.WriteLine("f.) Disconnect Finch Robot");
                Console.WriteLine("q.) Quit");
                Console.Write("Enter Choice: ");
                menuChoice = Console.ReadLine().ToUpper();


                switch (menuChoice)
                {
                    case "A":

                        if (finchRobotConnected)
                        {
                            Console.Clear();
                            Console.WriteLine("Finch robot already connected. Returning to main menu.");

                            DisplayContinuePrompt();

                        }
                        else
                        {
                            finchRobotConnected = DisplayConnectFinchRobot(finchRobot);
                        }
                        break;

                    case "B":
                        if (finchRobotConnected)
                        {
                            DisplayTalentShow(finchRobot);
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Finch robot is not connected, please go back and connect it.");
                            DisplayContinuePrompt();

                        }

                        break;

                    case "C":

                        displayDataRecorder(finchRobot);


                        break;

                    case "D":
                        if (finchRobotConnected)
                        {
                            DisplayAlarmSystem(finchRobot);
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Finch Robot not connected. Return to main menu and connect.");
                            DisplayContinuePrompt();
                        }
                        break;

                    case "E":

                        DisplayUserProgramming(finchRobot);

                        break;

                    case "F":
                        DisplayDisconnectFinchRobot(finchRobot);

                        break;

                    case "Q":
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\t===================================");
                        Console.WriteLine("\tSorry, please enter a menu choice");
                        Console.WriteLine("\t===================================");
                        DisplayContinuePrompt();
                        break;
                }


            } while (!quitApplication);
        }

        static void DisplayFinchCommands(List<FinchCommand> commands)
        {
            DisplayScreenHeader("Dyisplay Finch Commands");
            foreach (FinchCommand command in commands)
            {
                Console.WriteLine(command);
            }
        }

        static void DisplayGetFinchCommands(List<FinchCommand> commands)
        {
            FinchCommand command = FinchCommand.NONE;
            string userResponse;

            DisplayScreenHeader("Finch Robot Commands");

            while (command != FinchCommand.DONE)
            {
                Console.Write("enter command:");
                userResponse = Console.ReadLine().ToUpper();
                Enum.TryParse(userResponse, out command);

                commands.Add(command);

            }
            DisplayContinuePrompt();
        }

        static void DisplayUserProgramming(Finch finchRobot)

        {

            string menuChoice;

            bool quitApplication = false;

            (int motorSpeed, int ledBrightness, int waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;


            List<FinchCommand> commands = new List<FinchCommand>();

            do

            {

                DisplayScreenHeader("Main Menu");



                //

                // get user menu choice

                //

                Console.WriteLine("a) Set Command Parameters");

                Console.WriteLine("b) Add Commands");

                Console.WriteLine("c) View Commands");

                Console.WriteLine("d) Execute Commands");
                Console.WriteLine("e) save Commands to Teext File");
                Console.WriteLine("f)");
                Console.WriteLine("q) Quit");

                Console.Write("Enter Choice:");

                menuChoice = Console.ReadLine().ToLower();



                //
                // process user menu choice
                //

                switch (menuChoice)

                {

                    case "a":

                        commandParameters = DisplayGetCommandParameters();

                        break;



                    case "b":

                        DisplayGetFinchCommands(commands);

                        break;



                    case "c":

                        DisplayFinchCommands(commands);

                        break;



                    case "d":

                        DisplayExecuteCommands(finchRobot, commands, commandParameters);

                        break;

                    case "e":
                        DisplayWriteUserProgramingData(commands);
                        break;

                    case "f":
                        commands = DisplayReadUserProgrammingData();
                        break;



                    case "q":

                        quitApplication = true;

                        break;



                    default:

                        Console.WriteLine();

                        Console.WriteLine("Please enter a letter for the menu choice.");

                        DisplayContinuePrompt();

                        break;

                }



            } while (!quitApplication);

        }
        static void DisplayWriteUserProgramingData(List<FinchCommand> commands)
        {
            string dataPath = @"Data\Data.txt";
            List<string> commandsString = new List<string>();

            DisplayScreenHeader("Write Commands to the data file");


            foreach (FinchCommand command in commands)
            {
                commandsString.Add(command.ToString());
            }

            Console.WriteLine("All ready to save");

            File.WriteAllLines(dataPath, commandsString.ToArray());

            DisplayContinuePrompt();
        }

        static void DisplayExecuteCommands(Finch finchrobot, List<FinchCommand> commands, (int motorSpeed, int ledBrightness, int waitSeconds) commandParameters)
        {
            int motorSpeed = commandParameters.motorSpeed;
            int ledBrightness = commandParameters.ledBrightness;
            int waitSeconds = commandParameters.waitSeconds * 1000;


            DisplayScreenHeader("Execte Finch Commands");

            // info and pause
            foreach (FinchCommand command in commands)
            {
                Console.WriteLine(command);
                switch (command)
                {
                    case FinchCommand.NONE:
                        break;
                    case FinchCommand.MOVEFORWARD:
                        finchrobot.setMotors(motorSpeed, motorSpeed);
                        finchrobot.wait(waitSeconds);
                        finchrobot.setMotors(0, 0);
                        break;
                    case FinchCommand.MOVEBACKWARD:
                        finchrobot.setMotors(-motorSpeed, -motorSpeed);
                        finchrobot.wait(waitSeconds);
                        finchrobot.setMotors(0, 0);
                        break;
                    case FinchCommand.STOPMOTORS:
                        break;
                    case FinchCommand.WAIT:
                        finchrobot.wait(waitSeconds);
                        break;
                    case FinchCommand.TURNRIGHT:
                        finchrobot.setMotors(motorSpeed, -motorSpeed);
                        finchrobot.wait(waitSeconds);
                        finchrobot.setMotors(0, 0);
                        break;
                    case FinchCommand.TURNLEFT:
                        finchrobot.setMotors(-motorSpeed, motorSpeed);
                        finchrobot.wait(waitSeconds);
                        finchrobot.setMotors(0, 0);
                        break;
                    case FinchCommand.LEDON:
                        finchrobot.setLED(ledBrightness, ledBrightness, ledBrightness);
                        finchrobot.wait(waitSeconds);
                        break;
                    case FinchCommand.LEDOFF:
                        finchrobot.setLED(0, 0, 0);
                        break;
                    case FinchCommand.DONE:
                        finchrobot.setLED(0, 0, 0);
                        finchrobot.setMotors(0, 0);
                        break;
                    default:
                        break;
                }


            }


            DisplayContinuePrompt();
        }
        static List<FinchCommand> DisplayReadUserProgrammingData()
        {
            string dataPath = @"Data\Data.txt";
            List<FinchCommand> commands = new List<FinchCommand>();
            string[] commandsString;

            DisplayScreenHeader("Read commands from the data file");

            Console.WriteLine("Ready to read commands from the data file");
            Console.WriteLine();

            commandsString = File.ReadAllLines(dataPath);

            FinchCommand command;

            foreach (string commandString in commandsString)
            {
                Enum.TryParse(commandString, out command);

                commands.Add(command);
            }

            Console.WriteLine();
            Console.WriteLine("Commands read from data file done");

            DisplayContinuePrompt();

            return commands;

        }

        static (int motorSpeed, int ledBrightness, int waitSeconds) DisplayGetCommandParameters()
        {
            (int motorSpeed, int ledBrightness, int waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;

            DisplayScreenHeader("Command Paramaters");

            Console.Write("Enter Motor Speed [1 - 255]: ");
            int.TryParse(Console.ReadLine(), out commandParameters.motorSpeed);

            Console.Write("Enter LED brightness [1-255]");
            int.TryParse(Console.ReadLine(), out commandParameters.ledBrightness);

            Console.Write("Enter wait time ");
            int.TryParse(Console.ReadLine(), out commandParameters.waitSeconds);


            return commandParameters;
        }


        #region Temp monitoring/and Light
        static bool MonitorTemperature(Finch finchrobot, double threshold, int maxseconds)
        {
            bool thresholdExceeded = false;
            double currentTemperature;
            double seconds = 0;

            while (!thresholdExceeded && seconds <= maxseconds)
            {
                finchrobot.setLED(0, 255, 0);
                DisplayScreenHeader("Monitoring Temperature");
                currentTemperature = finchrobot.getTemperature() * (9 / 5) + 32;
                Console.Write($"What is The Max Temperature:  {threshold}\u00B0F");
                Console.Write($" \t\t Current Temperature: {currentTemperature}\u00B0F");


                if (currentTemperature > threshold)
                {
                    thresholdExceeded = true;
                }

                finchrobot.wait(500);
                seconds += 0.5;
            }

            return thresholdExceeded;
        }

        static void DisplayAlarmSystem(Finch finchrobot)
        {
            DisplayScreenHeader("Alarm System");

            int maxseconds;
            double threshold;
            bool thresholdExceeded;

            maxseconds = DisplayGetMaxSeconds();
            threshold = DisplayGetThreshold(finchrobot);

            thresholdExceeded = MonitorTemperature(finchrobot, threshold, maxseconds);

            if (thresholdExceeded)
            {
                for (int i = 0; i < 5; i++)
                {
                    finchrobot.setLED(255, 0, 0);
                    finchrobot.noteOn(255);
                    finchrobot.wait(800);
                    finchrobot.setLED(0, 0, 0);
                    finchrobot.noteOff();
                    finchrobot.wait(800);
                }

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Max Temperature Exceeded");
            }
            else
            {
                Console.WriteLine("Ran out of time");
            }


            DisplayContinuePrompt();
        }
        static int DisplayGetMaxSeconds()
        {
            Console.WriteLine("how long will you want to run this?:");
            return int.Parse(Console.ReadLine());
        }

        static double DisplayGetThreshold(Finch finchRobot)
        {
            double threshold = 0;

            DisplayScreenHeader("Max temp");
            //Console.WriteLine("how cold do you want to stop: ");

            Console.WriteLine($"Current Temperature: {finchRobot.getTemperature() * (9 / 5) + 32}\u00B0F");
            Console.Write("Enter Max Temperature:");
            threshold = double.Parse(Console.ReadLine());

            DisplayContinuePrompt();

            return threshold;
        }
        static bool MonitorLightLevel(Finch finchrobot, double threshold, int maxseconds)
        {
            bool thresholdExceeded = false;
            int currentLightLevel;
            double seconds = 0;

            while (!thresholdExceeded && seconds <= maxseconds)
            {
                finchrobot.setLED(0, 255, 0);
                DisplayScreenHeader("Monitoring Light Levels");
                currentLightLevel = finchrobot.getLeftLightSensor();
                Console.WriteLine($"Maximum Light Level: {threshold}");
                Console.WriteLine($"Current Light Level: {currentLightLevel}");

                if (currentLightLevel > threshold)
                {
                    thresholdExceeded = true;
                }

                finchrobot.wait(500);
                seconds += 0.5;
            }

            return thresholdExceeded;
        }
        #endregion

        #region DATA REC
        static double DisplayGetDataPointFrequency()
        {
            DisplayScreenHeader("Data point frequency");

            double dataPointFrequency;

            Console.Write("Enter data point frequency: ");
            double.TryParse(Console.ReadLine(), out dataPointFrequency);
            DisplayContinuePrompt();



            return dataPointFrequency;
        }

        static int DisplayGetNumberOfDataPoints()
        {
            int numberOfDataPoints;

            DisplayScreenHeader("Number of data points");

            Console.Write("Enter number of data points: ");
            int.TryParse(Console.ReadLine(), out numberOfDataPoints);

            DisplayContinuePrompt();

            return numberOfDataPoints;


        }


        static void displayDataRecorder(Finch finchRobot)
        {
            double dataPointFrequency;
            int numberOfDataPoints;

            DisplayScreenHeader("Data Recorder");
            DisplayContinuePrompt();

            //tell the user whats going to happen 

            Console.WriteLine("we are recourding the data");

            dataPointFrequency = DisplayGetDataPointFrequency();
            numberOfDataPoints = DisplayGetNumberOfDataPoints();

            //
            // Create the array
            //
            double[] temperature = new double[numberOfDataPoints];

            DisplayGetData(numberOfDataPoints, dataPointFrequency, temperature, finchRobot);
            displayData(temperature);

        }
        static void displayData(double[] temperature)
        {
            DisplayScreenHeader("temperature data");

            for (int index = 0; index < temperature.Length; index++)
            {
                Console.WriteLine($"temperature {index + 1}: {temperature[index]}");
            }

            DisplayContinuePrompt();
        }

        static void DisplayGetData(int numberOfDataPoints, double DataPointFrequency, double[] temperatures, Finch finchRobot)
        {
            Console.WriteLine("================");
            DisplayScreenHeader("Get data set");
            Console.WriteLine("================");
            //
            // Provide the user info and prompt
            //
            for (int Index = 0; Index < numberOfDataPoints; Index++)
            {
                //
                //recored data 
                //

                temperatures[Index] = (finchRobot.getTemperature() * (9 / 5)) + (32);
                int milliseconds = (int)(DataPointFrequency * 1000);
                finchRobot.wait(milliseconds);

                //
                // echo
                //
                Console.WriteLine($"Temperature {Index + 1}: {temperatures[Index]}\u00B0F ");

            }

            DisplayContinuePrompt();
        }
        #endregion

        #region Talent Show
        static void DisplayTalentShow(Finch finchRobot)
        {
            string userResponse;


            DisplayScreenHeader("Talent Show");
            Console.WriteLine("do you want a dance or a song?");
            userResponse = Console.ReadLine().Trim().ToLower();

            if (userResponse == "dance")
            {
                Console.WriteLine("are you READY FOR THIS");
                lightShow(finchRobot);
                DisplayContinuePrompt();
                Dance(finchRobot);
            }
            if (userResponse == "song")
            {
                Console.WriteLine("are you READY FOR THIS");
                lightShow(finchRobot);
                DisplayContinuePrompt();
                song(finchRobot);
            }
            else
                Console.WriteLine();
            Console.WriteLine("Please enter a vaild responce");
            DisplayContinuePrompt();





        }

        static void Dance(Finch finchRobot)
        {
            bool Dance = false;
            do
            {
                for (int i = 0; i < 5; i++)
                {
                    finchRobot.setMotors(left: -100, right: 100);
                    finchRobot.wait(1000);
                    finchRobot.setMotors(left: 0, right: 0);
                    finchRobot.setMotors(left: 100, right: -100);
                    finchRobot.wait(1000);
                    finchRobot.setMotors(left: 0, right: 0);
                }
                finchRobot.setMotors(left: 100, right: 100);
                finchRobot.wait(1000);
                finchRobot.setMotors(left: 0, right: 0);

                for (int i = 0; i < 5; i++)
                {
                    finchRobot.setMotors(left: -100, right: 100);
                    finchRobot.wait(1000);
                    finchRobot.setMotors(left: 0, right: 0);
                    finchRobot.setMotors(left: 100, right: -100);
                    finchRobot.wait(1000);
                    finchRobot.setMotors(left: 0, right: 0);
                }
                finchRobot.setMotors(left: -100, right: -100);
                finchRobot.wait(1000);
                finchRobot.setMotors(left: 0, right: 0);

                Dance = true;

            } while (Dance);

        }

        static void lightShow(Finch finchRobot)
        {
            finchRobot.setLED(255, 0, 0);
            finchRobot.wait(500);
            finchRobot.setLED(0, 255, 0);
            finchRobot.wait(500);
            finchRobot.setLED(0, 0, 255);
            finchRobot.wait(500);
            finchRobot.setLED(255, 0, 300);
        }

        static void song(Finch finchRobot)
        {

            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(167); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(400); finchRobot.wait(375); finchRobot.noteOff();
            finchRobot.setLED(255, 0, 0);
            finchRobot.noteOn(392); finchRobot.wait(375); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(392); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(330); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(440); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(494); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(466); finchRobot.wait(42); finchRobot.noteOff();
            finchRobot.noteOn(440); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(392); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(400); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(698); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(400); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(587); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(494); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(392); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(330); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(440); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(494); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(440); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(392); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(400); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(698); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(400); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(167); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(400); finchRobot.wait(375); finchRobot.noteOff();
            finchRobot.noteOn(392); finchRobot.wait(375); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(392); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(330); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(440); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(494); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(466); finchRobot.wait(42); finchRobot.noteOff();
            finchRobot.noteOn(440); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(392); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(400); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(698); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(400); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(587); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(494); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(392); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(330); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(440); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(494); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(466); finchRobot.wait(42); finchRobot.noteOff();
            finchRobot.noteOn(440); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(392); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(400); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(698); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(400); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(300); finchRobot.noteOff();

        }

        #endregion

        #region HELPER METHODS
        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("Ready to disconnect the finch robot");
            DisplayContinuePrompt();

            finchRobot.disConnect();
            Console.WriteLine();
            Console.WriteLine("Finch bot is now disconnected");

            DisplayContinuePrompt();
        }

        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            bool finchRobotConnected;

            DisplayScreenHeader("Connect to Finch Robot");

            Console.WriteLine("Ready to connect to Finch robot. Be sure to connect the USB cable to the robot and the computer.");
            DisplayContinuePrompt();

            finchRobotConnected = finchRobot.connect();

            if (finchRobotConnected)
            {
                finchRobot.setLED(0, 255, 0);
                finchRobot.noteOn(15000);
                finchRobot.wait(1000);
                finchRobot.setLED(0, 0, 0);
                finchRobot.noteOff();
                Console.WriteLine();
                Console.WriteLine("Finch robot is now connected.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Unable to connect to the Finch robot.");
            }
            return finchRobotConnected;

        }

        /// <summary>
        /// display welcome screen
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tFinch Control");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display closing screen
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayContinuePrompt();
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
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion
    }
}
