using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Payline
{
    /// <summary>
    /// The name of the payline (hand)
    /// </summary>
    public string Name = "Hand Name";

    /// <summary>
    /// The multiplier to pay back if the payline (hand) is won (achieved)
    /// </summary>
    public double Prize = 1;

    /// <summary>
    /// Construct with relevant values
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_prize"></param>
    public Payline( string _name, double _prize )
    {
        Name = _name;
        Prize = _prize;
    }
}
