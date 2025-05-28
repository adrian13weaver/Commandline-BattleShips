public class Board
{

    string[,] GameGrid;
    string[] Carrier = new string[5];
    string[] Battleship = new string[4];
    string[] Cruiser = new string[3];
    string[] Submarine = new string[2];
    string[] Destroyer = new string[2];

    public Board()
    {
        GameGrid = new string[10, 10]; //Initializes a 10x10 grid matrix
        BoardReset();
    }

    public void BoardReset()
    {
        for (int i = 0; i < GameGrid.GetLength(0); i++)
        {
            for (int j = 0; j < GameGrid.GetLength(1); j++)
            {
                GameGrid[i, j] = "Water";
            }
        }
    }

    public void PrintBoard()
    {
        Console.WriteLine("   A   B   C   D   E   F   G   H   I   J"); //Column Names
        for (int i = 0; i < GameGrid.GetLength(0); i++)
        {
            if (i + 1 != 10) { Console.Write(" " + (i + 1) + " "); } //Row Names
            else { Console.Write(10 + " "); } //Resize for 10
            for (int j = 0; j < GameGrid.GetLength(1); j++)
            {
                Tools.GetColor(GameGrid[i, j]); //Puts each tile into the print with their own color
            }
            Console.WriteLine("\n"); //Writes entire line after assembling
        }
    }

    public string GetTile(string coordinateLetter, string coordinateNumber) //Returns the tile type
    {
        int coordinateConverted = Tools.LetterToNumber(coordinateLetter);
        return GameGrid[int.Parse(coordinateNumber) - 1, coordinateConverted];
    }

    public void SetTile(string coordinateLetter, string coordinateNumber, string type) //Change a tile type
    {
        int coordinateConverted = Tools.LetterToNumber(coordinateLetter);
        GameGrid[int.Parse(coordinateNumber) - 1, coordinateConverted] = type;
    }

    public bool CheckForShipLetter(string[] tile, int size) //Checks tiles along Letter Column to see of theres already a ship placed
    {
        int index = 0;
        while (index < size)
        {
            int tempNumber = Tools.LetterToNumber(tile[0]) + index; //Starting tile letter + index ex) A + 2 = C
            if (GetTile(Tools.NumberToLetter(tempNumber), tile[1]) == "Ship") { return true; }
            else { index += 1; }
        }
        return false; //No ship
    }

    public bool CheckForShipNumber(string[] tile, int size) //Checks tiles along Number Row to see of theres already a ship placed
    {
        int index = 0;
        while (index < size)
        {
            int tempNumber = int.Parse(tile[1]) + index; //Number plus the index to loop through row
            string newCoord = tempNumber.ToString();
            if (GetTile(tile[0], newCoord) == "Ship") { return true; }
            index += 1;
        }
        return false; //No Ship
    }

    public void PlaceShipLetter(string[] tile, int size, string ship) //Changes type to ship on different letters, looping until all tiles are placed for the ship
    {
        int index = 0;
        while (index < size)
        {
            int tempNumber = Tools.LetterToNumber(tile[0]) + index; //lop through all the letters bounded by size
            SetTile(Tools.NumberToLetter(tempNumber), tile[1], "Ship"); //sets tile value to ship
            string[] newTile = {Tools.NumberToLetter(tempNumber),tile[1]};
            RegisterTileToShipType(ship, newTile, index); //registers each tile to their corresponding ship
            index += 1;
        }
    }

    public void PlaceShipNumber(string[] tile, int size, string ship) //Changes type to ship on different numbers, looping until all tiles are placed for the ship
    {
        int index = 0;
        while (index < size)
        {
            int tempNumber = int.Parse(tile[1]) + index; //loops through each number bounded by ship size
            string newCoord = tempNumber.ToString();
            SetTile(tile[0], newCoord, "Ship"); //Sets tile type to ship
            string[] newTile = { tile[0], newCoord };
            RegisterTileToShipType(ship, newTile, index); //registers each tile to their corresponding ship
            index += 1;
        }
    }

    public void RegisterTileToShipType(string ship, string[] tile, int tileNumber) //Puts the tile coord in the ship array
    {
        string stringTile = tile[0] + tile[1];
        switch (ship) //takes the ship type and sets the tile to that ship
        {
            case "Carrier":
                Carrier[tileNumber] = stringTile;
                break;
            case "Battleship":
                Battleship[tileNumber] = stringTile;
                break;
            case "Cruiser":
                Cruiser[tileNumber] = stringTile;
                break;
            case "Submarine":
                Submarine[tileNumber] = stringTile;
                break;
            case "Destroyer":
                Destroyer[tileNumber] = stringTile;
                break;
        }

    }

    public void SetShip(int size, string shipType) //Gets user input and sets value to Ship
    {
        //Gets the first tile
        string[] startTile = Tools.GetUserCoordinate($"Please select a Starting Tile for your ship of length {size}. ex (A5): ");

        //Checks for ship on initial tile
        if (GetTile(startTile[0], startTile[1]) == "Ship")
        {
            Console.WriteLine($"Cannot place a ship ontop of another. Input: {startTile[0] + startTile[1]}");
            SetShip(size, shipType);
        }

        else
        {
            while (true) //Grabs end tile and checks for viability then places tiles or repeats
            {
                //Gets the last tile, then check for diagonal
                string[] endTile = Tools.GetUserCoordinate($"Please select a Ending Tile for your ship of length {size}. ex (A5): ");
                if (Tools.CheckNotDiagonal(startTile, endTile) == false) { Console.WriteLine($"Diagonals aren't allowed"); }

                //Checks Horizontal Left to Right
                else if (Tools.CheckInRangeLetter(startTile[0], endTile[0], size) == true)
                {
                    if (CheckForShipLetter(startTile, size) == false)
                    {
                        PlaceShipLetter(startTile, size, shipType);
                        break;
                    }
                    else { Console.WriteLine("Cannot place ship ontop of another"); }

                }

                //Checks Horizonatal Right to Left
                else if (Tools.CheckInRangeLetterReverse(startTile[0], endTile[0], size) == true)
                {
                    if (CheckForShipLetter(endTile, size) == false)
                    {
                        PlaceShipLetter(endTile, size, shipType);
                        break;
                    }
                    else { Console.WriteLine("Cannot place ship ontop of another"); }
                }

                //Checks Verticle Top to Bottom
                else if (Tools.CheckInRangeNumber(startTile[1], endTile[1], size) == true)
                {
                    if (CheckForShipNumber(startTile, size) == false)
                    {
                        PlaceShipNumber(startTile, size, shipType);
                        break;
                    }
                    else { Console.WriteLine("Cannot place ship ontop of another"); }
                }

                //Checks Verticle Bottom To Top
                else if (Tools.CheckInRangeNumberReverse(startTile[1], endTile[1], size) == true)
                {
                    if (CheckForShipNumber(endTile, size) == false)
                    {
                        PlaceShipNumber(endTile, size, shipType);
                        break;
                    }
                    else { Console.WriteLine("Cannot place ship ontop of another"); }
                }

                //Out of range
                else { Console.WriteLine($"Please choose a tile within range. Input: {endTile[0] + endTile[1]}"); }
            }
        }


    }

    public void SetShipComputer(int size, string shipType) //Gets user input and sets value to Ship
    {
        //Gets the first tile
        string[] startTile = Tools.GetRandomCoordinate();

        //Checks for ship on initial tile
        if (GetTile(startTile[0], startTile[1]) == "Ship")
        {
            SetShipComputer(size, shipType);
        }

        else
        {

            bool placedShip = false;
            while (placedShip == false) //Grabs end tile and checks for viability then places tiles or repeats
            {
                //Different Coordinate parts for End Coordinate
                string smallerLetterCoord = Tools.NumberToLetter(Tools.LetterToNumber(startTile[0]) - (size-1)); //Letter - Size

                int smallerNumberCoordInt = int.Parse(startTile[1]) - (size-1); //Number - Size
                string smallerNumberCoord = smallerNumberCoordInt.ToString();


                //Different Possible Tile Choices, only needing earlier tiles - farther tiles can be infered with the size of the ship (i think)
                string[] leftTile = [smallerLetterCoord, startTile[1]];
                string[] topTile = [startTile[0], smallerNumberCoord];

                List<int> possibleTiles = new List<int> { 0, 1, 2, 3 }; //List of the different cases so the random doesn't repeat options 

                while (placedShip == false)
                {
                    int random = Tools.GetRandomNumber(0, possibleTiles.Count()); //Gets random number for a different case for the switch based on the remaining size of the list
                    if (possibleTiles.Count() == 0) {SetShipComputer(size,shipType);break;} //If the list is empty then it starts again with a new random number
                    switch (possibleTiles[random])
                    {
                        case 0:
                            if (Tools.ValidTile(startTile) && 11 - Tools.LetterToNumber(startTile[0]) >= size && CheckForShipLetter(startTile, size) == false) //Checks if the letter isnt invalid, if the ship of its size can fit and there arent ships already on those tiles
                            {
                                PlaceShipLetter(startTile, size, shipType);
                                placedShip = true;
                                break;
                            }
                            else //If it can't be placed there, then remove the case from the list so that it's not tried again
                            {
                                possibleTiles.Remove(random);
                                break;
                            }
                        case 1:
                            if (Tools.ValidTile(leftTile) && CheckForShipLetter(leftTile, size) == false) //Checks if the smaller letter is valid, and no ships
                            {
                                PlaceShipLetter(leftTile, size, shipType);
                                placedShip = true;
                                break;
                            }
                            else
                            {
                                possibleTiles.Remove(random);
                                break;
                            }
                        case 2:
                            if (Tools.ValidTile(startTile) && 11 - int.Parse(startTile[1]) >= size && CheckForShipNumber(startTile, size) == false) //Checks if the number is greater than 0, Checks if the ship can fit and if there is no ships already there
                            {
                                PlaceShipNumber(startTile, size, shipType);
                                placedShip = true;
                                break;
                            }
                            else
                            {
                                possibleTiles.Remove(random);
                                break;
                            }
                        case 3:
                            if (Tools.ValidTile(topTile) && CheckForShipNumber(topTile, size) == false) //Checks if the number is on the grid, then of theres a ship already there
                            {
                                PlaceShipNumber(topTile, size, shipType);
                                placedShip = true;
                                break;
                            }
                            else
                            {
                                possibleTiles.Remove(random);
                                break;
                            }
                        default: //No tiles are valid to place ships, try again with a new starting tile
                            SetShipComputer(size,shipType);
                            break;
                    }
                }

            }
        }


    }

    public void CheckIfShipDestroyed() //Tells the player which ships are alive or destroyed
    {
        Console.WriteLine("");

        // Check if each ship is alive, then display
        if (IsShipDestroyed(Carrier) == false)
        {
            Console.WriteLine("Carrier    |  Alive");
        }
        if (IsShipDestroyed(Battleship) == false)
        {
            Console.WriteLine("BattleShip |  Alive");
        }
        if (IsShipDestroyed(Cruiser) == false)
        {
            Console.WriteLine("Cruiser    |  Alive");
        }
        if (IsShipDestroyed(Submarine) == false)
        {
            Console.WriteLine("Submarine  |  Alive");
        }
        if (IsShipDestroyed(Destroyer) == false)
        {
            Console.WriteLine("Destroyer  |  Alive");
        }

        // Check if each ship has been destroyed, then display
        if (IsShipDestroyed(Carrier))
        {
            Console.WriteLine("Carrier    |  Destroyed");
        }
        if (IsShipDestroyed(Battleship))
        {
            Console.WriteLine("BattleShip |  Destroyed");
        }
        if (IsShipDestroyed(Cruiser))
        {
            Console.WriteLine("Cruiser    |  Destroyed");
        }
        if (IsShipDestroyed(Submarine))
        {
            Console.WriteLine("Submarine  |  Destroyed");
        }
        if (IsShipDestroyed(Destroyer))
        {
            Console.WriteLine("Destroyer  |  Destroyed");
        }
        Console.WriteLine("");
    }

    private bool IsShipDestroyed(string[] ship) // Helper method to check if a ship is destroyed
    {
        int numOfHits = 0;

        // Loop through each coordinate in the ship's array
        for (int i = 0; i < ship.Length; i++)
        {
            // Split the coordinate into letter and number parts
            string coordinate = ship[i];
            string letterPart = "";
            string numberPart = "";

            // Separate the letter and number using a loop
            int index = 0;
            while (index < coordinate.Length && !char.IsDigit(coordinate[index]))
            {
                letterPart += coordinate[index];
                index++;
            }
            numberPart = coordinate.Substring(index);

            // Use GetTile() function to check if this part is hit
            if (GetTile(letterPart, numberPart) == "Hit")
            {
                numOfHits++;
            }
        }

        // If the number of hits equals the length of the ship, it's destroyed
        return numOfHits == ship.Length;
    }

    public bool AllShipsDestroyed() //Checks if all ships are destroyed
    {
        int shipDestroyedCounter = 0;
        if (IsShipDestroyed(Carrier)) { shipDestroyedCounter += 1; }
        if (IsShipDestroyed(Battleship)) { shipDestroyedCounter += 1; }
        if (IsShipDestroyed(Cruiser)) { shipDestroyedCounter += 1; }
        if (IsShipDestroyed(Submarine)) { shipDestroyedCounter += 1; }
        if (IsShipDestroyed(Destroyer)) { shipDestroyedCounter += 1; }

        if (shipDestroyedCounter == 5) { return true; }

        else { return false; }
    }

}