using System;


namespace Protobuf.Cards
{
    /// <summary>
    /// Extends the PlayingCard class to add helpful access methods.
    /// </summary>
    public partial class PlayingCard
    {
        public PlayingCard( int cardId ) { CardId = cardId; }

        /// <summary>
        /// Get the Suit of the card. Assumes the card is valid.
        /// 
        /// Note- It is probably not a good idea to use this value to map to a Card Image- Use
        /// <see cref="CardIndex"/> instead.
        /// </summary>
        public ESuit Suit => (ESuit) (CardId & 3);

        /// <summary>
        /// Get the Rank of the card. Assumes the card is valid.
        /// 
        /// Note- It is probably not a good idea to use this value to map to a Card Image- Use
        /// <see cref="CardIndex"/> instead.
        /// </summary>
        public ERank Rank => (ERank) ((CardId >> 2) + 1);

        /// <summary>
        /// The index of the card in a deck. Assumes cards are ordered like this:
        /// 
        /// 2H 2C 2D 2S 3H 3C 3D 3S ... KS AH AC AD AS
        /// 
        /// Order of suits should match order found in <see cref="ESuit"/> Order of ranks should
        /// match order found in <see cref="ERank"/>
        /// </summary>
        public int CardIndex => CardId;

        const string suits = "CDHS";
        const string ranks = "234567890JQKA";

        public static PlayingCard From2String( string _str )
        {
            if (_str?.Length != 2) throw new ArgumentOutOfRangeException( "May only pass in 2-character strings" );
            _str = _str.ToUpper();

            int rank = ranks.IndexOf( _str[0] );
            if (rank < 0) throw new ArgumentException( $"Rank is invalid: {_str[0]}" );

            int suit = suits.IndexOf( _str[1] );
            if (suit < 0) throw new ArgumentException( $"Suit is invalid: {_str[1]}" );

            return new PlayingCard( rank * 4 + suit );
        }

        /// <summary>
        /// Get either a 2-char or a full string description of this card.
        /// </summary>
        /// <param name="concise"></param>
        /// <returns></returns>
        public string AsString( bool concise = true )
        {
            // Check for anything greater than the original 52 cards- If so, assume its a joker.
            // This may require review in the future- i'm thinking maybe multi-deck
            if (CardIndex >= 52)
                return "JK";

            var s = Suit.ToString().Substring( 0, 1 );
            var r = Rank.ToString(); // skip underscore
            if (r.StartsWith( "_" ))
                r = r.Substring( 1 );
            if (r != "10")
                r = r.Substring( 0, 1 );

            return r + s;
        }

        public static implicit operator PlayingCard( int cardId ) => new PlayingCard( cardId );
    }
}
