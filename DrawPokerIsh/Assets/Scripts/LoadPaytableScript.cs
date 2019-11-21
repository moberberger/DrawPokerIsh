using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Morpheus;

public class LoadPaytableScript : MonoBehaviour
{
    public Text TextTemplate;
    public string[] Paylines;

    private Paytable m_paytable = new Paytable();
    private Dictionary<Payline, Text> m_lookup = new Dictionary<Payline, Text>();

    void Start()
    {
        TextTemplate.enabled = false;
        foreach (var paylineCode in Paylines)
        {
            var split = paylineCode
                .Split( '=' )
                .Select( _code => _code.RemoveDuplicateWhitespace() )
                .ToArray();

            var payline = m_paytable.AddPayline( split[0], double.Parse( split[1] ) );

            var uiLine = Instantiate<Text>( TextTemplate, this.transform );
            uiLine.text = $"{split[0]} .......... {split[1]}";
            uiLine.GetComponent<PaylineClickHandler>().Payline = payline;
            uiLine.enabled = true;

            m_lookup[payline] = uiLine;
        }
    }

    [AEventHandler]
    public void OnPaylineClicked( PaylineClickedMessage _msg )
    {
        var uiLine = m_lookup[_msg.Payline];
        uiLine.text = $"{_msg.Payline.Name} .......... {_msg.Payline.Prize}";
    }
}
