using System;
using System.Linq;
using System.Collections.Generic;
using Morpheus;
using Protobuf.Cards;

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
    private bool[] m_dealtCardFlags;
    private int[] m_availableCardListTemp;

    public DeckOfCards( int size, IEnumerable<PlayingCard> cardsDealt = null )
    {
        m_dealtCardFlags = new bool[size];
        m_availableCardListTemp = new int[size];

        m_size = size;

        if (cardsDealt != null)
        {
            foreach (var c in cardsDealt)
                m_dealtCardFlags[c.CardId] = true;
        }
    }

    public PlayingCard GetRandomCard()
    {
        lock (m_dealtCardFlags)
        {
            for (int i = 0; i < 100; i++)
            {
                var id = Rng.Default.Next( m_size );
                if (!m_dealtCardFlags[id]) // not set yet
                {
                    m_dealtCardFlags[id] = true;
                    return new PlayingCard( id );
                }
            }

            int availableCardCount = 0;
            for (int i = 0; i < m_dealtCardFlags.Length; i++)
            {
                if (!m_dealtCardFlags[i])
                    m_availableCardListTemp[availableCardCount++] = i;
            }
            if (availableCardCount == 0)
                return null;

            var selIdx = Rng.Default.Next( availableCardCount );
            var cardId = m_availableCardListTemp[selIdx];
            m_dealtCardFlags[cardId] = true;
            return new PlayingCard( cardId );
        }
    }

    public bool RemoveCard( int cardId )
    {
        var wasInDeck = m_dealtCardFlags[cardId];
        m_dealtCardFlags[cardId] = true;
        return wasInDeck;
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
        lock (m_dealtCardFlags)
            Array.Clear( m_dealtCardFlags, 0, m_dealtCardFlags.Length );
    }
}
