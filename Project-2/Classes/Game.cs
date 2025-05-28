using System.IO.Compression;
using System.Net.Http.Headers;

public sealed class Game //Using singleton | Only one game
{
    private Player Player1;
    private Player Player2;
    private int CurrentTurn;
    private bool ComputerGame;
    private bool GameOver;
    private string ComputerShotTile; //holds the tile the computer last shot 


    private Game(bool input)
    {
        Player1 = new Player(); //Always human player
        Player2 = new Player(); //Human or computer player
        CurrentTurn = 1;
        ComputerGame = input;
        GameOver = false;
        ComputerShotTile = "N/A";
    }

    private static Game _instance; //Singleton instance holder

    public static Game GetInstance(bool input) //Makes new game if there isn't one
        {
            if (_instance == null)
            {
                _instance = new Game(input);
            }
            return _instance;
        }

    public void Start()
    {
        if (ComputerGame == false) //Game between People
        {
            PlaceShipsHuman();

            while (true) //Loop for each turn
            {
                PlayerReady();
                PlayTurn();
                if (this.GameOver == true) //Checks if game is over
                {
                    Console.WriteLine("You Win!");
                    Console.ReadLine();
                    break;
                }
                ChangeTurn();
                Console.WriteLine("Press anything to end turn");
                Console.ReadLine(); //Lets you see what happened before it switches turns
            }
        }
        else if (ComputerGame == true) //Game against the computer
        {
            PlaceShipsComputer();
            //PlaceShipsComputerDebug(); //Only computer places ships for debugging purposes

            while (true) //Loop for each turn
            {
                PlayTurnComputerGame(); //Turn functionality

                if (GameOver) {break;} //Stops the loop if the game is over
                Console.WriteLine("Press anything to end turn");
                Console.ReadLine(); //Lets you see what happened before it switches turns
            }
        }



    }

    public void ChangeTurn()
    {
        if (this.CurrentTurn == 1) { this.CurrentTurn = 2; }
        else if (this.CurrentTurn == 2) { this.CurrentTurn = 1; }
    }

    public void DisplayBoards() //Prints the boards out
    {
        Console.Clear();
        if (this.ComputerGame == true || this.CurrentTurn == 1) //Always chooses this option if its a computer game
        {
            Player2.PrintPublicBoard();
            Console.WriteLine("Player 2 Board\n");
            Player1.PrintPersonalBoard();
            Console.WriteLine("Player 1 Board\n");
        }
        if (this.CurrentTurn == 2)
        {
            Player1.PrintPublicBoard();
            Console.WriteLine("Player 1 Board\n");
            Player2.PrintPersonalBoard();
            Console.WriteLine("Player 2 Board\n");
        }
    }

    public void PlayerReady() //Hides the screen and waits for other player
    {
        Console.Clear();
        Console.WriteLine($"Is Player {this.CurrentTurn} ready?");
        Console.ReadLine();
        Console.Clear();
    }

    public void PlayTurn() //Turn functionality
    {
        DisplayBoards();
        if (this.CurrentTurn == 1)
        {
            PlayerShoot(Player2.PublicBoard, Player2.PersonalBoard);
            Player2.PersonalBoard.CheckIfShipDestroyed();
        }
        else if (this.CurrentTurn == 2)
        {
            PlayerShoot(Player1.PublicBoard, Player1.PersonalBoard);
            Player1.PersonalBoard.CheckIfShipDestroyed();

        }
    }


    public void PlayTurnComputerGame() //Turn functionality for a computer game
    {
        DisplayBoards();
        Console.WriteLine($"Computer shot this tile: {this.ComputerShotTile}");
        PlayerShoot(Player2.PublicBoard, Player2.PersonalBoard); //Player shoots
        Player2.PersonalBoard.CheckIfShipDestroyed();
        CheckGameOverComputer(2);
        if (this.GameOver == true) //Checks if game is over
            {
                Console.WriteLine("You Win!");
                Console.ReadLine();
            }
        ComputerShootRandom(); //Computer shoots
        CheckGameOverComputer(2);
        if (this.GameOver == true) //Checks if game is over
            {
                Console.WriteLine("Computer Wins!");
                Console.ReadLine();
            }
    }
    public void PlayerShoot(Board publicBoard, Board personalBoard) //Shooting fuctionality
    {
        while (true) //Loop so you can enter again if your shot is invalid
        {
            string[] tile = Tools.GetUserCoordinate($"Please select a tile to shoot. ex (A5): ");
            if (publicBoard.GetTile(tile[0], tile[1]) == "Water") //If you haven't shot the tile before
            {
                string personalTileType = personalBoard.GetTile(tile[0], tile[1]); //Grabs the personal tile

                //Miss functionality
                if (personalTileType == "Water")
                {
                    publicBoard.SetTile(tile[0], tile[1], "Miss");
                    personalBoard.SetTile(tile[0], tile[1], "Miss");
                    DisplayBoards();
                    Console.WriteLine($"The shot on {tile[0] + tile[1]} was a miss");
                    break;
                }

                //Hit fuctionality
                else if (personalTileType == "Ship")
                {
                    publicBoard.SetTile(tile[0], tile[1], "Hit");
                    personalBoard.SetTile(tile[0], tile[1], "Hit");
                    DisplayBoards();
                    Console.WriteLine($"The shot on {tile[0] + tile[1]} was a hit");
                    CheckGameOver();
                    break;
                }

            }

            //If they select a tile thats already shot
            else { Console.WriteLine($"Please select a tile that hasn't been shot. Input {tile[0] + tile[1]}"); }
        }
    }

    public void ComputerShootRandom() //Computer Shoots Random Tile
    {
        while (true) //Loop for invalid shots
        {
            string[] tile = Tools.GetRandomCoordinate();
            if (Player1.PublicBoard.GetTile(tile[0], tile[1]) == "Water") //If you haven't shot the tile before
            {
                string personalTileType = Player1.PersonalBoard.GetTile(tile[0], tile[1]); //Grabs the personal tile

                //Miss functionality
                if (personalTileType == "Water")
                {
                    Player1.PublicBoard.SetTile(tile[0], tile[1], "Miss");
                    Player1.PersonalBoard.SetTile(tile[0], tile[1], "Miss");
                    this.ComputerShotTile = tile[0]+tile[1];
                    break;
                }

                //Hit fuctionality
                else if (personalTileType == "Ship")
                {
                    Player1.PublicBoard.SetTile(tile[0], tile[1], "Hit");
                    Player1.PersonalBoard.SetTile(tile[0], tile[1], "Hit");
                    this.ComputerShotTile = tile[0]+tile[1];
                    break;
                }

            }
        }
    }

    public void CheckGameOver() //Checks if the game is over based on turn
    {
        if (this.CurrentTurn == 1)
        {
            this.GameOver = Player2.PersonalBoard.AllShipsDestroyed();
        }
        else if (this.CurrentTurn == 2)
        {
            this.GameOver = Player1.PersonalBoard.AllShipsDestroyed();
        }
    }
    public void CheckGameOverComputer(int player) //Checks if the game is over, but without turn change for computer games
    {
        if (player == 1)
        {
            this.GameOver = Player2.PersonalBoard.AllShipsDestroyed();
        }
        else if (player == 2)
        {
            this.GameOver = Player1.PersonalBoard.AllShipsDestroyed();
        }
    }


    public void PlaceShipsHuman()
    {
        //Initial Board Prints
        DisplayBoards();

        //Player 1 Sets Ships
        Player1.PersonalBoard.SetShip(5, "Carrier");
        DisplayBoards();
        Player1.PersonalBoard.SetShip(4, "Battleship");
        DisplayBoards();
        Player1.PersonalBoard.SetShip(3, "Cruiser");
        DisplayBoards();
        Player1.PersonalBoard.SetShip(2, "Submarine");
        DisplayBoards();
        Player1.PersonalBoard.SetShip(2, "Destroyer");

        ChangeTurn();

        PlayerReady();

        //Player 2 Sets Ships
        DisplayBoards();
        Player2.PersonalBoard.SetShip(5, "Carrier");
        DisplayBoards();
        Player2.PersonalBoard.SetShip(4, "Battleship");
        DisplayBoards();
        Player2.PersonalBoard.SetShip(3, "Cruiser");
        DisplayBoards();
        Player2.PersonalBoard.SetShip(2, "Submarine");
        DisplayBoards();
        Player2.PersonalBoard.SetShip(2, "Destroyer");

        ChangeTurn();
    }

    public void PlaceShipsComputer()
    {
        //Initial Board Prints
        DisplayBoards();

        //Player 1 Sets Ships
        Player1.PersonalBoard.SetShip(5, "Carrier");
        DisplayBoards();
        Player1.PersonalBoard.SetShip(4, "Battleship");
        DisplayBoards();
        Player1.PersonalBoard.SetShip(3, "Cruiser");
        DisplayBoards();
        Player1.PersonalBoard.SetShip(2, "Submarine");
        DisplayBoards();
        Player1.PersonalBoard.SetShip(2, "Destroyer");

        //Player 2 Comupter Sets Ships

        Player2.PersonalBoard.SetShipComputer(5, "Carrier");
        Player2.PersonalBoard.SetShipComputer(4, "Battleship");
        Player2.PersonalBoard.SetShipComputer(3, "Cruiser");
        Player2.PersonalBoard.SetShipComputer(2, "Submarine");
        Player2.PersonalBoard.SetShipComputer(2, "Destroyer");
    }

    public void PlaceShipsComputerDebug() //Debug version of PlaceShipsComputer that skips the player setting ships
    {
        //Initial Board Prints
        DisplayBoards();

        //Player 2 Comupter Sets Ships

        Player2.PersonalBoard.SetShipComputer(5, "Carrier");
        Player2.PersonalBoard.SetShipComputer(4, "Battleship");
        Player2.PersonalBoard.SetShipComputer(3, "Cruiser");
        Player2.PersonalBoard.SetShipComputer(2, "Submarine");
        Player2.PersonalBoard.SetShipComputer(2, "Destroyer");
    }

}