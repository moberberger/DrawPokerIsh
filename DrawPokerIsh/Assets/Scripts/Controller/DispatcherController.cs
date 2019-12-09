using System.Collections.Generic;

using Morpheus;

using UnityEngine;


public class DispatcherController : MonoBehaviour
{
    private List<MessageHandler> m_forDeregistering = new List<MessageHandler>();

    void Awake()
    {
        Debug.Log( "Awake- Registering Handlers" );
        var discovery = new MessageHandlerDiscovery( Dispatcher.Default );

        foreach (var mb in Resources.FindObjectsOfTypeAll<MonoBehaviour>())
        {
            var registeredHandlers = discovery.RegisterInstanceHandlers( mb );
            m_forDeregistering.AddRange( registeredHandlers );
        }
    }

    void OnDestroy()
    {
        Debug.Log( "OnDestroy- Deregistering Handlers" );
        foreach (var handler in m_forDeregistering)
            Dispatcher.Default.DeregisterHandler( handler );
    }
}
