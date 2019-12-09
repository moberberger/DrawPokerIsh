using System;
using System.Collections.Generic;
using Morpheus;

/// <summary>
/// Model a deck of cards. Optimize drawing random cards from a deck even when
/// given an initial set of drawn cards. Designed to be lightweight in
/// construction despite the memory allocation.
/// </summary>
/// <remarks>
/// This is NOT a general purpose deck- it is currently optimized for draw
/// poker. Feel free to add or optimize functionality.
/// </remarks>
public class DeckOfCards
{
    private readonly int m_size;
    bool[] m_deck;

    public DeckOfCards( int size, IEnumerable<PlayingCard> cardsDealt = null )
    {
        m_deck = new bool[size];
        m_size = size;

        if (cardsDealt != null)
        {
            foreach (var c in cardsDealt)
                m_deck[c.CardId] = true;
        }
    }

    public PlayingCard GetRandomCard()
    {
        lock (m_deck)
            while (true)
            {
                var id = Rng.Default.Next( m_size );
                if (!m_deck[id]) // not set yet
                {
                    m_deck[id] = true;
                    return new PlayingCard( id );
                }
            }
    }

    public IEnumerable<PlayingCard> GetRandomCards( int count )
    {
        var list = new List<PlayingCard>();
        while (count-- > 0)
            list.Add( GetRandomCard() );
        return list;
    }

    public void Shuffle()
    {
        lock (m_deck)
            Array.Clear( m_deck, 0, m_deck.Length );
    }
}
