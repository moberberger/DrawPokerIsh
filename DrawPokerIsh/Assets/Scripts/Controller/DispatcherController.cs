using System;
using System.Collections.Generic;

using Morpheus;

using UnityEngine;


public class DispatcherController : MonoBehaviour
{
    private List<MessageHandler> m_forDeregistering = new List<MessageHandler>();

    /// <summary>
    /// What is the default post mode for the dispatcher
    /// </summary>
    public EDispatchMode DefaultDispatchMode = EDispatchMode.Inline;

    /// <summary>
    /// When FALSE, the application must handle
    /// <see cref="Dispatcher.ExecuteBatch(int)"/> if it wants batch mode.
    /// </summary>
    public bool ExecuteBatchInUpdate = true;


    void Awake()
    {
        Debug.Log( "Awake- Registering Handlers" );

        Dispatcher.Default.DefaultDispatchMode = DefaultDispatchMode;

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
        m_forDeregistering.Clear();
    }

    void Update()
    {
        if (ExecuteBatchInUpdate)
            Dispatcher.Default.ExecuteBatch();
    }

    [AEventHandler]
    public void OnDispatcherException( DispatcherException ex )
    {
        Debug.LogError( $"Exception from Dispatcher: {ex}" );
    }
}
