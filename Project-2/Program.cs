class BattleShips
{
    public static void Main()
    {
        bool computerGame = Tools.GetYesNo("Do you wish to play the computer? (Yes/No)");
        Game runningGame = Game.GetInstance(computerGame);
        runningGame.Start();
    }
}