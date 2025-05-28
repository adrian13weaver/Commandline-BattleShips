public class Player
{
    public Board PersonalBoard;
    public Board PublicBoard;

    public Player()
    {
        PersonalBoard = new Board();
        PublicBoard = new Board();
    }

    public void PrintPublicBoard()
    {
        PublicBoard.PrintBoard();
    }
    public void PrintPersonalBoard()
    {
        PersonalBoard.PrintBoard();
    }



}