using System;
using System.Collections.Generic;

using Protobuf.Cards;
using Protobuf.DrawPoker;
using Morpheus;

namespace DrawPoker
{
    /// <summary>
    /// Contains the logic for evaluating poker hands.
    /// </summary>
    public abstract class PokerEvaluationLogic
    {
        /// <summary>
        /// The results of the evaluation. Will be NULL until an evaluation is performed using
        /// the <see cref="Evaluate(IList{PlayingCard}, Paytable)"/> or
        /// <see cref="AnalyzeCards(IList{PlayingCard})"/> method.
        /// </summary>
        public Evaluation Evaluation { get; protected set; }

        /// <summary>
        /// If this is non-null, it represents the winning payline after the
        /// <see cref="Evaluate(IList{PlayingCard}, PaytableInfo)"/> method has been called.
        /// </summary>
        public Payline WinningPayline { get; private set; }

        /// <summary>
        /// The number of wild cards in the deck
        /// </summary>
        public abstract int ValidWildCardCount { get; }

        /// <summary>
        /// The number of cards in the deck
        /// </summary>
        public abstract int DeckSize { get; }

        private object m_lock = new object();

        /// <summary>
        /// Given a list of <see cref="PlayingCard"/> s, evaluate the cards then set both the
        /// <see cref="Evaluation"/> and <see cref="WinningPayline"/> properties.
        /// </summary>
        /// <param name="cards">The cards to evaluate- Must be 5 in the list.</param>
        /// <param name="paytable">
        /// The Paytable to use to evaluate the cards for a win amount
        /// </param>
        public virtual void Evaluate( IList<PlayingCard> cards, Paytable paytable )
        {
            lock (m_lock)
            {
                //Log.Debug( $"Evaluating cards: {cards.JoinAsString( " " )}" );
                Evaluation = new Evaluation();
                WinningPayline = null;

                AnalyzeCards( cards );
                WinningPayline = paytable?.Apply( Evaluation );
            }
        }

        /// <summary>
        /// Go over the cards to determine which basic poker hands are present.
        /// </summary>
        /// <param name="cards">A list of cards to transform into basic Poker data</param>
        public abstract Evaluation AnalyzeCards( IList<PlayingCard> cards );


        /// <summary>
        /// Verify that a list of cards has 5 elements and that all of them are different. This
        /// is basic to enough evaluators to include it in this base class.
        /// </summary>
        /// <remarks>
        /// It is for optimization purposes that (a) the cards are taken out of the IList only
        /// once; and (b) they are manually compared to each other instead of looping. This
        /// optimization shaved almost 20% off the running time of the algorithm, so it should
        /// not be modified without extreme caution.
        /// 
        /// The same function could be performed by this code with a severe performance penalty:
        /// <code>
        /// var set = new HashSet( cards );
        /// if (set.Count != 5) throw exception
        /// </code>
        /// </remarks>
        /// <param name="cards">The list of cards to verify</param>
        protected void VerifyFiveUniqueCards( IList<PlayingCard> cards )
        {
            if (cards.Count != 5)
                throw new ArgumentException( "You must pass exactly 5 cards to this analyzer, not " + cards.Count );

            var card0 = cards[0];
            var card1 = cards[1];
            var card2 = cards[2];
            var card3 = cards[3];
            var card4 = cards[4];

            // Accumulate through all of the cards because its going to be very rare to find a
            // "match", thus justifying leaving the accumulation early
            bool match = (card0 == card1);
            match |= (card0 == card2);
            match |= (card0 == card3);
            match |= (card0 == card4);
            match |= (card1 == card2);
            match |= (card1 == card3);
            match |= (card1 == card4);
            match |= (card2 == card3);
            match |= (card2 == card4);
            match |= (card3 == card4);
            if (match)
                throw new ArgumentException( "This analyzer does not handle duplicate cards (rank and suit), as if they were from different decks or a mistake was made" );
        }

        /// <summary>
        /// Internal / protected: Step one is to figure out if the cards all have the same suit
        /// and to put the ranks in a new sorted collection. Note- for more than 5 cards, all
        /// cards must be the same suit for "Flush" to be true; otherwise, this method should
        /// work for any number of cards.
        /// </summary>
        /// <param name="cards">The un-ordered cards that are to be evaluated.</param>
        protected virtual void InternalOrderedRanksAndFlush( IList<PlayingCard> cards )
        {
            // Profiling shows this to be a "hot" function, so its optimized for speed. The
            // first and second card optimization was worth about 5% execution time.

            // handle first card
            var firstCard = cards[0];
            var firstSuit = firstCard.Suit;
            Evaluation.UnorderedCards.Add( firstCard );
            var firstRank = firstCard.Rank;

            // handle second card
            var secondCard = cards[1];
            Evaluation.Flush = (firstSuit == secondCard.Suit);
            Evaluation.UnorderedCards.Add( secondCard );
            var secondRank = secondCard.Rank;

            if (firstRank < secondRank)
            {
                Evaluation.OrderedRanks.Add( firstRank );
                Evaluation.OrderedRanks.Add( secondRank );
            }
            else
            {
                Evaluation.OrderedRanks.Add( secondRank );
                Evaluation.OrderedRanks.Add( firstRank );
            }

            // Handle Third+ cards
            for (int i = 2; i < cards.Count; i++)
            {
                var card = cards[i];
                Evaluation.UnorderedCards.Add( card );

                // Any suit mismatch will force "Flush" bool to FALSE
                Evaluation.Flush &= (firstSuit == card.Suit);

                // Implement an Insertion Sort here- this is the fastest possible sort for an
                // existing list of no more than 4 elements
                for (int j = 0; j < Evaluation.OrderedRanks.Count; j++)
                    if (card.Rank < Evaluation.OrderedRanks[j]) // this card is less than the [j] card
                    { // note: if ==, then don't insert yet to prevent unneeded element shift for Insert
                        Evaluation.OrderedRanks.Insert( j, card.Rank );
                        break;
                    }

                // If it wasn't Inserted, then simply append it.
                if (Evaluation.OrderedRanks.Count != i + 1)
                    Evaluation.OrderedRanks.Add( card.Rank );
            }
        }

        /// <summary>
        /// Analyse the ordered ranks to determine Distinct Rank data. This also identifies the
        /// highest duplicate rank. Should work for any number of cards, but the
        /// DistinctRankCount and DistinctRankCountProduct will have to be used differently for
        /// (probably) each different card count. I.e. analysing 4 cards will be different using
        /// these two values than analysing 5 cards.
        /// </summary>
        protected virtual void InternalAnalyzeDistinctRankData()
        {
            // Set up the aggregation accumulators for duplicate ranks
            int consecutiveRankCount = 0;
            Evaluation.DistinctRankCount = 1;
            Evaluation.DistinctRankCountProduct = 1;
            Evaluation.HighestDuplicateRank = ERank.Unknown;

            for (int i = 1; i < Evaluation.OrderedRanks.Count; i++)
            {
                consecutiveRankCount++;
                // If the consecutive ranks are different, then accumulate
                if (Evaluation.OrderedRanks[i] != Evaluation.OrderedRanks[i - 1])
                {
                    Evaluation.DistinctRankCount++;
                    Evaluation.DistinctRankCountProduct *= consecutiveRankCount;
                    consecutiveRankCount = 0; // The next card will cause this to inc. by 1
                }
                else // Ranks are duplicates- Record this as the highest one
                {
                    Evaluation.HighestDuplicateRank = Evaluation.OrderedRanks[i];
                }
            }
            // Accumulate the effect of the last card.
            Evaluation.DistinctRankCountProduct *= (consecutiveRankCount + 1);
        }

    }
}
