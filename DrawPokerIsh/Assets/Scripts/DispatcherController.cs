
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using DG;
using DG.Tweening;
using DG.Tweening.Core;
using Morpheus;
using System.Collections;
public class DispatcherController : MonoBehaviour
{
    private List<MessageHandler> m_forDeregistering = new List<MessageHandler>();
    private void Awake()
    {
        Debug.Log( "Awake- Registering Handlers" );
        var discovery = new MessageHandlerDiscovery( Dispatcher.Default );

        foreach (var mb in Resources.FindObjectsOfTypeAll<MonoBehaviour>())
        {
            m_forDeregistering.AddRange( discovery.RegisterInstanceHandlers( mb ) );
        }
    }

    private void OnDestroy()
    {
        Debug.Log( "OnDestroy- Deregistering Handlers" );
        foreach (var handler in m_forDeregistering)
            Dispatcher.Default.DeregisterHandler( handler );
    }
}
