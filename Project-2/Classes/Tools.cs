public static class Tools
{

    public static int LetterToNumber(string letter) //Converts the letter to a number for coordinates
    {
        switch (letter.ToLower())
        {
            case "a":
                return 0;
            case "b":
                return 1;
            case "c":
                return 2;
            case "d":
                return 3;
            case "e":
                return 4;
            case "f":
                return 5;
            case "g":
                return 6;
            case "h":
                return 7;
            case "i":
                return 8;
            case "j":
                return 9;
            default:
                return 15;
        }
    }

    public static string NumberToLetter(int number) //Converts the number to letter
    {
        switch (number)
        {
            case 0:
                return "A";
            case 1:
                return "B";
            case 2:
                return "C";
            case 3:
                return "D";
            case 4:
                return "E";
            case 5:
                return "F";
            case 6:
                return "G";
            case 7:
                return "H";
            case 8:
                return "I";
            case 9:
                return "J";
            default:
                return "Invalid";  // Return a placeholder if the number is out of range
        }
    }


    public static bool CheckLetter(string letter) //Checks if letter is on grid
    {
        switch (letter.ToLower())
        {
            case "a":
                return true;
            case "b":
                return true;
            case "c":
                return true;
            case "d":
                return true;
            case "e":
                return true;
            case "f":
                return true;
            case "g":
                return true;
            case "h":
                return true;
            case "i":
                return true;
            case "j":
                return true;
            default:
                return false;
        }
    }

    public static bool CheckNumber(string number) //Checks if number is on grid
    {
        switch (number)
        {
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
            case "7":
            case "8":
            case "9":
            case "10":
                return true;
            default:
                return false;
        }
    }

    public static bool ValidTile(string[] tile) //Checks if both the letter and number values are valid
    {
        if (CheckLetter(tile[0]) && CheckNumber(tile[1]))
        {
            return true;
        }
        else {return false;}
    }
    public static bool CheckNumberString(string numberString) //Checks if number is on grid
    {
        int number;
        bool success = int.TryParse(numberString, out number);

        if (success)
        {
            if (number < 11 && number > 0) { return true; }
            else { return false; }
        }
        else { return false; }
    }

    public static bool CheckNotDiagonal(string[] startTile, string[] endTile) //Checks if the selected tile isn't diagonal
    {
        if (startTile[0].ToLower() == endTile[0].ToLower()) { return true; }
        else if (startTile[1] == endTile[1]) { return true; }
        else { return false; }
    }

    public static string[] GetUserCoordinate(string message) //Need to ass the check number and split it up so its not a bunch of nested if statements
    {
        while (true)
        {
            Console.WriteLine(message);
            string input = Console.ReadLine();
            if (input == "") { Console.WriteLine($"Input not in expected range. Given input: {input}"); }
            else
            {
                string letter = input.Substring(0, 1); //Grabs first character aka letter coord
                if (CheckLetter(letter) == false) //Checks to make sure letter is in range
                { Console.WriteLine($"Input not in expected range. Given input: {input}"); }
                else
                {
                    string number;
                    if (input.Length == 2) //For numbers 1-9
                    {
                        number = input.Substring(1, 1);
                        if (CheckNumberString(number) == true)
                        {
                            string[] Coord = [letter, number];
                            return Coord;
                        }
                        else { Console.WriteLine($"Input not in expected range. Given input: {input}"); }
                    }
                    else if (input.Length == 3) //For number 10
                    {
                        number = input.Substring(1, 2);
                        if (CheckNumberString(number) == true)
                        {
                            string[] Coord = [letter, number];
                            return Coord;
                        }
                        else { Console.WriteLine($"Input not in expected range. Given input: {input}"); }
                    }
                    else
                    {
                        Console.WriteLine($"Input not in expected range. Given input: {input}");
                    }
                }

            }


        }
    }

    public static bool CheckInRangeLetter(string startTile, string endTile, int size) //Checks if the ship is in Range on the Letter Column
    {
        int startInt = LetterToNumber(startTile);
        int endInt = LetterToNumber(endTile);

        if (startTile == endTile) { return false; }
        else if (endInt - startInt == size - 1)
        {
            return true;
        }
        else { return false; }
    }

    public static bool CheckInRangeLetterReverse(string startTile, string endTile, int size) //Checks if the ship is in Range on the Letter Column but in reverse
    {
        int startInt = LetterToNumber(startTile);
        int endInt = LetterToNumber(endTile);

        if (startTile == endTile) { return false; }
        else if (startInt - endInt == size - 1)
        {
            return true;
        }
        else { return false; }
    }

    public static bool CheckInRangeNumber(string startTile, string endTile, int size) //Checks if number is in range with a given size. ex) tile a5 size 3, checks a5, a6, a7
    {
        int startInt = int.Parse(startTile);
        int endInt = int.Parse(endTile);

        if (endInt - startInt == size - 1) //loops through all numbers
        {
            return true;
        }
        else { return false; }
    }

    public static bool CheckInRangeNumberReverse(string startTile, string endTile, int size) //Checks if number is in range with a given size, but reverse. ex) tile a5 size 3, checks a5, a4, a3
    {
        int startInt = int.Parse(startTile);
        int endInt = int.Parse(endTile);

        if (startInt - endInt == size - 1) //loops in reverse
        {
            return true;
        }
        else { return false; }
    }

    public static void GetColor(string tile) //Getting color based on type for board print
    {
        switch (tile)
        {
            case "Water": //Water tiles are blue
                Console.ForegroundColor = ConsoleColor.DarkBlue; //Change color
                Console.Write("██");
                Console.Write("  ");
                Console.ResetColor(); //Changes color back
                break;
            case "Ship": //Ship tiles are gray
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("██");
                Console.Write("  ");
                Console.ResetColor();
                break;
            case "Miss": //Misses are white
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("██");
                Console.Write("  ");
                Console.ResetColor();
                break;
            case "Hit": //Hits are Red
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("██");
                Console.Write("  ");
                Console.ResetColor();
                break;
        }
    }

    public static bool GetYesNo(string message) //Verifies that the answer is a yes/no or y/n response
    {
        while (true)
        {
            Console.WriteLine(message);
            string input = Console.ReadLine();
            if (input.ToLower() == "yes" || input.ToLower() == "y")
            {
                return true;
            }
            else if (input.ToLower() == "no" || input.ToLower() == "n")
            {
                return false;
            }
            else
            {
                Console.WriteLine($"Input not expected (Yes/No): {input}");
            }
        }
    }

    public static int GetRandomNumber(int lowerBound, int upperBound) //return a random number based on given bounds, lower is inclusive with the upper being exclusive
    {
        Random random = new Random();
        int randomNumber = random.Next(lowerBound, upperBound);
        return randomNumber;
    }

    public static string[] GetRandomCoordinate() //Gets a random coordinate bounded by the length and width of the grid
    {
        int firstNumber = GetRandomNumber(0, 10); //expected letters are 0-9
        int secondNumber = GetRandomNumber(1, 11); //expected numbers are 1-10

        //turns them into strings
        string letterCoord = NumberToLetter(firstNumber);
        string numberCoord = secondNumber.ToString();

        return [letterCoord, numberCoord];
    }

    

}


