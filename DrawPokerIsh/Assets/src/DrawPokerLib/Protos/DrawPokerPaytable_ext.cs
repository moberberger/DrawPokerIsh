using System;

using DrawPoker;

namespace Protobuf.DrawPoker
{
    public partial class Paytable
    {
        /// <summary>
        /// Apply the appropriate logic to the evaluation given the specifications found in the
        /// paytable. It is invalid for multiple PayLines to evaluate to TRUE for a given hand.
        /// </summary>
        /// <param name="paytable">The Paytable to be used for the evaluation</param>
        public Payline Apply( Evaluation eval )
        {
            if (eval == null)
                throw new ArgumentNullException( "eval" );

            Payline retval = null;
            foreach (var payline in Paylines)
            {
                if (payline.Evaluator( eval )) // If it is indeed this type of Payline...
                {
                    if (retval == null)
                        retval = payline;
                    else
                        throw new InvalidOperationException( $"Duplicate Paylines found for evaluation:\n\t'{payline}'\n\t'{retval}'" );
                }
            }

            return retval;
        }


        /// <summary>
        /// Return a proper evaluation logic object based on this paytable's configuration. A
        /// sort of Factory method.
        /// </summary>
        /// <returns></returns>
        public PokerEvaluationLogic GetPokerLogic()
        {
            if (HasWildCards)
            {
                if (NumberOfCardsInDeck == 52) // sad way of detecting deuces-wild vs. jokers
                    return new FiveCardDeucesWildLogic();
                else
                    return new FiveCardJokerWildLogic();
            }
            else
            {
                return new FiveCardNoWildsLogic();
            }
        }

        public Payline AddPayline( string _desc, double _prize )
        {
            var line = new Payline() { EnglishDescription = _desc };
            line.WinAmounts.Add( (int)_prize );
            Paylines.Add( line );
            return line;
        }

    }
}
