using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Paytable : IEnumerable<Payline>
{
    private List<Payline> m_pays=new List<Payline>();

    public Paytable() { }
    public Paytable( IEnumerable<Payline> _pays ) => m_pays.AddRange( _pays );

    public Payline AddPayline( string _name, double _prize )
    {
        var payline = new Payline( _name, _prize );
        m_pays.Add( payline );
        return payline;
    }

    public IEnumerator<Payline> GetEnumerator() => m_pays.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => m_pays.GetEnumerator();
}
