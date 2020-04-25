// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using System.Collections.Generic;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.State.Graph {

/// <summary>
/// State class used to represent individual sockets of the <see cref="Node"/> objects.
/// Sockets can be linked together and represent the flow of the application.
/// </summary>
public class Socket {
    // ---  Attributes ---
        // - Serialized Attributes -
            // - Flag -
            /// <summary>Flag set if this is the input socket.</summary>
            public bool IsInput;
            
            // - Socket Info -
            /// <summary>
            /// List of all the targets of the socket.
            /// </summary>
            public List<Node> Targets;
            
            /// <summary>Reference to the owner of this socket.</summary>
            public Node Owner;
            
        // -- Public Attributes --
            // - Reusable Assets -
            /// <summary>Rect representing the position and size of the socket.</summary>
            [NonSerialized]
            public Rect Rect;
            
            /// <summary>Reference to the socket object that is the current source.</summary>
            public static Socket CurrentSource = null; 
            
            public static readonly Vector2 SIZE = Vector2.one * 16; 
            
    // --- /Attributes ---
}
}