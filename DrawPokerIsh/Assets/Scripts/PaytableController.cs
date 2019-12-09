using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Morpheus;
using TMPro;
using Protobuf.DrawPoker;
using System;

public class PaytableController : MonoBehaviour
{
    /// <summary>
    /// OPTIONAL! If there's a TextMeshProUGUI object that's a child of this object, it will be used
    /// if this field is left empty by the user in the IDE
    /// </summary>
    public TextMeshProUGUI TextTemplate;

    /// <summary>
    /// Whatever the paylines should look like, in "handName = prize" format, no quotes, "=" is
    /// reserved and may not be used in a hand-name. All whitespace is condensed and turned into spaces.
    /// </summary>
    public string[] Paylines;

    private Paytable m_paytable = new Paytable();
    private Dictionary<string, TextMeshProUGUI> m_lookup = new Dictionary<string, TextMeshProUGUI>();

    void Start()
    {
        TextTemplate = TextTemplate ?? GetComponentInChildren<TextMeshProUGUI>();

        TextTemplate.enabled = false;
        foreach (var paylineString in Paylines)
        {
            var split = paylineString
                .Split( '=' )
                .Select( _code => _code.RemoveDuplicateWhitespace() )
                .ToArray();

            var payline = m_paytable.AddPayline( split[0], double.Parse( split[1] ) );

            var uiLine = Instantiate<TextMeshProUGUI>( TextTemplate, this.transform );
            SetPaylineText( uiLine, payline );
            uiLine.GetComponent<PaylineClickHandler>().Payline = payline;
            uiLine.enabled = true;

            m_lookup.Add( payline.EnglishDescription, uiLine );
        }
    }

    [AEventHandler]
    public void OnPaylineClicked( PaylineClickedMessage _msg )
    {
        try
        {
            Debug.Log( $"Payline {_msg.Payline.EnglishDescription} @ {_msg.Payline.WinAmounts[0]} Handled" );

            var uiLine = m_lookup[_msg.Payline.EnglishDescription];

            SetPaylineText( uiLine, _msg.Payline );
        }
        catch (Exception ex)
        {
            Debug.LogError( ex.ToString() );
        }
    }

    private void SetPaylineText( TextMeshProUGUI _uiLine, Payline _payline )
    {
        _uiLine.text = $"{_payline.EnglishDescription}<pos=60%>{_payline.WinAmounts[0]}";
    }
}
