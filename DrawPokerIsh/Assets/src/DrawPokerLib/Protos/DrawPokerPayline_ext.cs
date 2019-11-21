using System;
using System.Collections.Generic;
using System.Linq;
using Morpheus;

using DrawPoker;
namespace Protobuf.DrawPoker
{
    public partial class Payline
    {
        private string m_paylineIdKey = null;
        public string IdKey => m_paylineIdKey ?? (m_paylineIdKey = PaylineEvaluatorCache.NormalizedPaylineId( Id ));

        private Func<Evaluation, bool> m_evaluator;
        public Func<Evaluation, bool> Evaluator => m_evaluator ?? (m_evaluator = PaylineEvaluatorCache.FindPayline( IdKey ));

        public string WinAmountString => WinAmounts.JoinAsString( ", " );

        /// <summary>
        /// The application should use this method to determine what to multiply the wager by.
        /// This relies on the INDEX of the wager within the enabledWagers.
        /// </summary>
        /// <param name="wagerIndex">
        /// The index into the enabled wagers list to figure out the win for.
        /// </param>
        /// <param name="enabledWagerAmounts">The list of enabled wagers for the game.</param>
        /// <returns>The number of credits that have been won with this payline.</returns>
        public long GetWinAmount( int wagerIndex, IList<long> enabledWagerAmounts )
        {
            if (wagerIndex < 0)
                throw new ArgumentOutOfRangeException( "wagerIndex", "WagerIndex cannot be negative" );
            if (wagerIndex > enabledWagerAmounts.Count)
                throw new ArgumentOutOfRangeException( "wagerIndex", $"WagerIndex of {wagerIndex} cannot be larger than the number of enabled wagers {enabledWagerAmounts.Count}" );

            if (WinAmounts.Count == 1)
                return enabledWagerAmounts[wagerIndex] * WinAmounts[0];

            if (wagerIndex >= WinAmounts.Count)
                throw new ArgumentOutOfRangeException( "wagerIndex", $"WagerIndex of {wagerIndex} is too large for the number of win multiples in paytable {WinAmounts.Count}" );

            return enabledWagerAmounts[wagerIndex] * WinAmounts[wagerIndex];
        }
    }
}
