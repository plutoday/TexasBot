namespace TexasBot.Games
{
    public enum GameStage
    {
        Initialized,    //no holes have been dealt yet
        HolesReceived,  //all players received their holes
        FlopsSeen,      //flops seen
        TurnSeen,       //turn seen
        RiverSeen,      //river seen
        Finished,       //game finished
    }
}
