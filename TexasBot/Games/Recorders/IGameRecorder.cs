using System.Collections.Generic;
using TexasBot.Games.Players;
using TexasBot.Models;

namespace TexasBot.Games.Recorders
{
    public interface IGameRecorder
    {
        void StartNewGame(List<string> playerNames);
        void RecordDecision(PlayerDecision playerDecision);
        void FinishStage(GameStage gameStage);
        void RecordFlopsDealt(List<Card> flops);
        void RecordTurnDealt(Card card);
        void RecordRiverDealt(Card card);
        void RecordHolesDealt(string playerName, List<Card> holes);
        GameStatus GetGameStatus();
        CardStatus GetCardStatus(string playerName);
        bool AllPlayerAgreed();
        void RecordBestHand(string playerName, HandOf5 bestHand);
        void RecordPlayerPrize(string playerName, int outcome);
    }
}
