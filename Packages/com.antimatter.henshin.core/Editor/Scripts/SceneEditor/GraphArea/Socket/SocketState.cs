// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using Henshin.Editor.SceneEditor.GraphArea.Node;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.GraphArea.Socket {

/// <summary>
/// State class used to represent the state of a node's socket.
/// </summary>
public class SocketState {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Reference to the owner of this <see cref="SocketState"/>.
            /// </summary>
            public NodeState Owner;
            
            /// <summary>
            /// Flag set if the <see cref="SocketState"/> is an input socket.
            /// </summary>
            public bool IsInput;
            
            /// <summary>
            /// Flag set if the socket is enabled.
            /// Set if the socket is rendered.
            /// </summary>
            public bool IsEnabled = true;
            
            /// <summary>
            /// Rect used to render this socket state.
            /// </summary>
            public Rect Rect = new Rect();
            
            // - Static References -
            /// <summary>
            /// Reference to the socket object that is currently being manipulated.
            /// </summary>
            public static SocketState Manipulated;
            
        // -- Public Constants --
            /// <summary>
            /// Width of the socket, in pixels.
            /// </summary>
            public const int WIDTH = 16;
            
            /// <summary>
            /// Height of the socket, in pixels.
            /// </summary>
            public const int HEIGHT = 16;
    // --- /Attributes ---
}
}