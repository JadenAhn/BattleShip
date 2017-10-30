using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    class Game
    {
        //Size of the game board and the square
        const int boardX = 5;
        const int boardY = 5;
        const int initialState = 1;
        const int answer = 0;

        static void ShowTitle()
        {
            Console.WriteLine(@"
        BBBBBBBBBB    AAAAA   TTTTTTTTTT TTTTTTTTT LLL       EEEEEEEEE SSSSSSSSS HHH     HHH III PPPPPPPPP
        BBB    BBB   AAAAAA       TTT       TTT    LLL       EEE       SSS       HHH     HHH III PP    PPP
        BBB BBBBB    AAA AAA      TTT       TTT    LLL       EEEEEEEEE SSSSSS    HHH HHHHHHH III PPP   PPP
        BBBBBBBBB   AAA  AAA      TTT       TTT    LLL       EEEEEEEEE   SSSSSSS HHH HHHHHHH III PP PPPPPP
        BBB    BBB  AAAAAAAAA     TTT       TTT    LLL       EEE              SS HHH     HHH III PP PPPP
        BBB    BBB AAA AAAAAA     TTT       TTT    LLL       EEE       SSS    SSEHHH     HHH III PPP
        BBBBBBBBBB AAA     AAA    TTT       TTT   LLLLLLLLLL EEEEEEEEE  SSSSSSSS HHH     HHH III PPP

                    ** HIT THE BATTLESHIP 3 TIMES! **

Press Any Key to Continue...
");
            Console.ReadLine();
            Console.Clear();
        }
        //Convert the coordinate of X to alphabet, or vice versa
        static int CheckPosName(string input)
        {
            string[] posName = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I" };
            //Console.WriteLine(posName[0]);
            int result = Array.IndexOf(posName, input.ToUpper());
            return result;
        }
        static string CheckPosName(int input)
        {
            string[] posName = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I" };
            string result = posName[input];
            return result;
        }

        //Check the validity of user input
        static void CheckValidity(int boardX, int boardY, out int guessXpos, out int guessYpos)
        {
            string userInput;
            int result;
            Console.WriteLine("Enter XY coordinate (ex.) A1");
            string maxPosX = CheckPosName(boardX - 1);
            Console.WriteLine("(X:A~" + maxPosX + ", Y:1~" + boardY + ")");
            Console.Write("XY = ");
            userInput = Console.ReadLine();
            ;
            while (
                userInput.Length != 2 // Check the total length
                || CheckPosName(userInput.Substring(0, 1)) == -1 // No such name in the Position Name
                || CheckPosName(userInput.Substring(0, 1)) > boardX - 1 // The X is bigger than the maximum range
                || !int.TryParse(userInput.Substring(1, 1), out result) // Try parse the Y value then if false, invalid
                || int.Parse(userInput.Substring(1, 1)) > boardY // The Y is bigger than the maximum range
                )
            {
                Console.WriteLine("Invalid data.");
                Console.Write("XY = ");
                userInput = Console.ReadLine();
            }
            guessXpos = CheckPosName(userInput.Substring(0, 1)); //Get the number of uer input X
            guessYpos = int.Parse(userInput.Substring(1, 1)) - 1; //Set the Y number equal with index and the input number
        }

        static void checkHit(int state, out int returnedState, out string retuenedMessage)
        {
            /*
            0 answer. no marking
              when checked(hit) becomes 3(markType 3)
            1 initial. (markType 2)
              when checked becomes 2
            2 already missed
              when checked remain 2
            3 hit
              when checked becomes 4
            4 already hit
              when checked remain 4
             */
            returnedState = 1;
            switch (state)
            {
                case 0: //answer
                    retuenedMessage = "++ HIT ++";
                    returnedState = 3;
                    break;
                case 1: //miss
                    retuenedMessage = ".. MISSED ..";
                    returnedState = 2;
                    break;
                case 2: //miss
                    retuenedMessage = ".. ALREADY MISSED ..";
                    returnedState = 2;
                    break;
                default: //already hit
                    retuenedMessage = ".. ALREADY HIT ..";
                    returnedState = 4;
                    break;
            }
        }

        static void Main(string[] args)
        {

            ShowTitle();
            string continueY = "";
            do
            {
                Console.Clear();
                //Make basic game board
                int[,] board = new int[boardX, boardY];
                for (int i = 0; i < boardY; i++)
                {
                    for (int j = 0; j < boardX; j++)
                    {
                        board[j, i] = initialState;
                    }
                }

                //Set the size of the battleship
                Random rnd = new Random();
                int squareX = rnd.Next(2, 4);
                int squareY = rnd.Next(1, 2);

                //Set the left-up position of the square               
                int squarePosX = rnd.Next(0, boardX - squareX);
                int squarePosY = rnd.Next(0, boardY - squareY);

                //Console.WriteLine("Answer : (" + CheckPosName(squarePosX) + "," + (squarePosY + 1) + ")");

                for (int i = 0; i < squareY; i++)
                {
                    for (int j = 0; j < squareX; j++)
                    {
                        board[squarePosX + j, squarePosY + i] = answer;
                    }
                }

                //Show the board
                Console.Write("  ");
                for (int i = 0; i < boardX; i++)
                {
                    Console.Write(" " + CheckPosName(i));

                }//Column name
                Console.WriteLine("\n");

                //Show Basic board
                string[] markType = new string[] { " .", " .", " x", " o", " o" };
                /*
                0 -> answers, same mark with initial
                1 -> initial
                2 -> missed mark
                3 -> hit mark
                4 -> hit mark
                */

                string[] markTypeAnswer = new string[] { " o", " .", " x", " o", " o" };
                /*
                0 -> answers, same mark with initial
                1 -> initial
                2 -> missed mark
                3 -> hit mark
                4 -> hit mark
                */
                for (int i = 0; i < boardY; i++)
                {
                    Console.Write(i + 1 + " ");
                    for (int j = 0; j < boardX; j++)
                    {
                        Console.Write(markType[1]);
                    }
                    Console.WriteLine("\n");
                }


                int returnedState;
                string returnedMessage;
                int numOfTrial = 0;
                int numOfAnswers = 0;
                int guessXpos, guessYpos;
                while (numOfAnswers < (squareX * squareY))//repeat until you hit 3 times
                {
                    //Convert user input into guessXpos, guessYpos coordinate
                    CheckValidity(boardX, boardY, out guessXpos, out guessYpos);
                    //Console.WriteLine(guessXpos + " and " + guessYpos);
                    //Console.WriteLine("The value in the board is " + board[guessXpos, guessYpos]);

                    //check hit
                    checkHit(board[guessXpos, guessYpos], out returnedState, out returnedMessage);
                    numOfTrial++;
                    Console.Clear();

                    //change the state of guessed coordinate
                    board[guessXpos, guessYpos] = returnedState;

                    //If hit, increase the number of answers
                    if (returnedState == 3)
                    {
                        numOfAnswers++;
                    }

                    //Show the board with returnedState
                    Console.Write("  ");
                    for (int i = 0; i < boardX; i++)
                    {
                        Console.Write(" " + CheckPosName(i));

                    }
                    Console.WriteLine("\n");
                    for (int i = 0; i < boardY; i++)
                    {
                        Console.Write(i + 1 + " ");
                        for (int j = 0; j < boardX; j++)
                        {
                            Console.Write(markType[board[j, i]]);
                        }
                        Console.WriteLine("\n");
                    }
                    //Console.WriteLine("returnedState : " + returnedState); 
                    Console.WriteLine(returnedMessage);
                    Console.WriteLine("Number of Hits : " + numOfAnswers);
                    Console.WriteLine("Number of Trial : " + numOfTrial);
                }
                Console.Clear();
                //Show the board with answers
                Console.Write("  ");
                for (int i = 0; i < boardX; i++)
                {
                    Console.Write(" " + CheckPosName(i));

                }
                Console.WriteLine("\n");
                for (int i = 0; i < boardY; i++)
                {
                    Console.Write(i + 1 + " ");
                    for (int j = 0; j < boardX; j++)
                    {
                        Console.Write(markTypeAnswer[board[j, i]]);
                    }
                    Console.WriteLine("\n");
                }

                //Show the result
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("+ Congratulations! You sunk the BATTLESHIP!  +");
                Console.WriteLine("+ Number of Trial : " + numOfTrial.ToString("00") + "                       +");
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("Continue? (Y/N)");
                continueY = Console.ReadLine();
            } while (continueY.ToLower() == "y");

        }
    }
}
