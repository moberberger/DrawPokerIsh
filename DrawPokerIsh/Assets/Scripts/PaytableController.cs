using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Morpheus;
using TMPro;

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
    private Dictionary<Payline, TextMeshProUGUI> m_lookup = new Dictionary<Payline, TextMeshProUGUI>();

    void Start()
    {
        TextTemplate = TextTemplate ?? GetComponentInChildren<TextMeshProUGUI>();

        TextTemplate.enabled = false;
        foreach (var paylineCode in Paylines)
        {
            var split = paylineCode
                .Split( '=' )
                .Select( _code => _code.RemoveDuplicateWhitespace() )
                .ToArray();

            var payline = m_paytable.AddPayline( split[0], double.Parse( split[1] ) );

            var uiLine = Instantiate<TextMeshProUGUI>( TextTemplate, this.transform );
            SetPaylineText( uiLine, payline );
            uiLine.GetComponent<PaylineClickHandler>().Payline = payline;
            uiLine.enabled = true;

            m_lookup[payline] = uiLine;
        }
    }

    [AEventHandler]
    public void OnPaylineClicked( PaylineClickedMessage _msg )
    {
        var uiLine = m_lookup[_msg.Payline];
        SetPaylineText( uiLine, _msg.Payline );
    }

    private void SetPaylineText( TextMeshProUGUI _uiLine, Payline _payline )
    {
        _uiLine.text = $"{_payline.Name}<pos=60%>{_payline.Prize}";
    }
}
