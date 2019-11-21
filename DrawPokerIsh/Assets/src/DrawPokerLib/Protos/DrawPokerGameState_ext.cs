using Protobuf.Cards;
using System.Collections.Generic;

namespace Protobuf.DrawPoker
{
    public partial class GameState
    {
        /// <summary>
        /// Set to TRUE when the Draw Poker game is completed. If FALSE, the game engine is
        /// expecting the player to draw/discard in order to complete the game.
        /// </summary>
        public bool IsComplete => FinalCards.Count > 0;

        /// <summary>
        /// The "Current" cards- the InitialCards immediately after a deal, the FinalCards after
        /// a draw.
        /// </summary>
        public IList<PlayingCard> CurrentCards => IsComplete ? FinalCards : InitialCards;

        public Evaluation CurrentEvaluation { get => IsComplete ? FinalEvaluation : InitialEvaluation; }
        public Payline CurrentWinningPayline { get => IsComplete ? FinalWinningPayline : InitialWinningPayline; }
    }
}
