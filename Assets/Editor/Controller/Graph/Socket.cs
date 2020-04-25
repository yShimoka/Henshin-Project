// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEngine;
using System.Collections.Generic;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.Controller.Graph {

/// <summary>
/// 
/// </summary>
public static class Socket {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Create a new Socket object.
            /// </summary>
            /// <returns>The instantiated socket object.</returns>
            public static State.Graph.Socket CreateSocket(State.Graph.Node owner, bool isInput) {
                // Create a new instance.
                return new State.Graph.Socket {
                    Rect = { size = State.Graph.Socket.SIZE }, 
                    Owner = owner, 
                    IsInput = isInput, 
                    Targets = new List<Henshin.Editor.State.Graph.Node>() 
                };
            }
            
            /// <summary>
            /// Method called right before the specified socket is rendered.
            /// </summary>
            /// <param name="container">The rect of the parent node.</param>
            /// <param name="socket">The socket to render.</param>
            public static void BeforeRender(Rect container, State.Graph.Socket socket) {
                // Place the socket at the right position.
                socket.Rect.center = new Vector2(
                    x: container.center.x + (socket.IsInput ? -1 : 1) * (container.width / 2 - (16 * (1 / socket.Owner.owner.scale))), 
                    y: container.position.y + 16 * (1 / socket.Owner.owner.scale)
                );
                
                socket.Rect.size = State.Graph.Socket.SIZE * (1 / socket.Owner.owner.scale);
            }
            
            /// <summary>
            /// Method called right after the socket is rendered.
            /// </summary>
            public static void AfterRender(Event ev, State.Graph.Socket socket) {
                // Check if the socket is an input socket.
                if (socket.IsInput) {
                    // Check if the mouse was released.
                    if (ev.type == EventType.MouseUp) {
                        // Check if there is a dragged socket.
                        if (State.Graph.Socket.CurrentSource != null) {
                            // Check if the mouse is over the current socket.
                            if (socket.Rect.Contains(point: ev.mousePosition - State.SceneEditor.Canvas.Rect.position)) {
                                // Check if the current source's owner is the same as the current socket.
                                if (State.Graph.Socket.CurrentSource.Owner != socket.Owner) {
                                    // Set the socket as the target of the current source.
                                    State.Graph.Socket.CurrentSource.Targets.Add(item: socket.Owner);
                                    socket.Targets.Add(item: State.Graph.Socket.CurrentSource.Owner);
                                }
                            }
                        }
                    }
                    
                    // If the mouse was right clicked.
                    if (ev.type == EventType.MouseDown && ev.button == (int)UnityEngine.UIElements.MouseButton.RightMouse) {
                        // Check if the mouse is over the current socket.
                        if (socket.Rect.Contains(point: ev.mousePosition - State.SceneEditor.Canvas.Rect.position)) {
                            // Clear all the targets.
                            foreach (State.Graph.Node node in socket.Targets) {
                                if (!node.Output.Targets.Remove(item: socket.Owner)) {
                                    Debug.LogWarning(message: "Owner removal failed !");
                                }
                            }
                            socket.Targets.Clear();
                            
                            // Repaint the window.
                            Controller.Graph.RenderArea.ON_SHOULD_REPAINT.Invoke();
                            
                            // Consume the event.
                            ev.Use();
                        }
                    }
                } else {
                    // Check if the mouse was clicked.
                    if (ev.type == EventType.MouseDown) {
                        // Check if the socket was pressed.
                        if (socket.Rect.Contains(point: ev.mousePosition - State.SceneEditor.Canvas.Rect.position)) {
                            switch (ev.button) {
                            // Check if the mouse was left clicked.
                            case (int)UnityEngine.UIElements.MouseButton.LeftMouse:
                                // Set the socket as the current source.
                                State.Graph.Socket.CurrentSource = socket;
                                
                                // Use the event.
                                ev.Use();
                                break;
                            // Check if the mouse was right clicked.
                            case (int)UnityEngine.UIElements.MouseButton.RightMouse:
                                // Clear all the targets.
                                foreach (State.Graph.Node node in socket.Targets) {
                                    node.Input.Targets.Remove(item: socket.Owner);
                                }
                                socket.Targets.Clear();
                                
                                // Repaint the window.
                                Controller.Graph.RenderArea.ON_SHOULD_REPAINT.Invoke();
                                
                                // Consume the event.
                                ev.Use();
                                break;
                            }
                        }
                    }
                }
            }
            
        // -- Private Methods --
    // --- /Methods ---
}
}