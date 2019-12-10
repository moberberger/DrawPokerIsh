using System.Collections.Generic;

public class SetHeldCards
{
    public IEnumerable<int> Indicies;

    public SetHeldCards( IEnumerable<int> _indicies )
    {
        this.Indicies = _indicies;
    }
}