// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Editor.SceneEditor.GraphArea.Node;
using Henshin.Runtime.Actions.Scene;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.GraphArea.Socket {

/// <summary>
/// Controller class used to manipulate <see cref="SocketState"/> objects.
/// These objects are not serializable.
/// </summary>
public static class SocketController {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Initializes the specified socket object.
            /// </summary>
            /// <param name="socket">The socket to initialize.</param>
            public static void Initialize(SocketState socket) {
                // Check if the parent is a start object. 
                if (socket.Owner.Action?.ControllerType == typeof(StartAction)) {
                    // Enable if this is the output socket.
                    socket.IsEnabled = !socket.IsInput;
                // Check if the parent is an end object. 
                } else if (socket.Owner.Action?.ControllerType == typeof(EndAction)) {
                    // Enable if this is the input socket.
                    socket.IsEnabled = socket.IsInput;
                } else {
                    // Enable always.
                    socket.IsEnabled = true;
                }
            }
            
            /// <summary>
            /// Prepares the <see cref="SocketState"/> before rendering it.
            /// </summary>
            /// <param name="socket">The socket object to prepare.</param>
            public static void Prepare(SocketState socket) {
                // Check if the socket is enabled.
                if (!socket.IsEnabled) return;
                
                // Compute the size of the socket.
                socket.Rect.width  = SocketState.WIDTH * socket.Owner.Owner.Scale;
                socket.Rect.height = SocketState.HEIGHT * socket.Owner.Owner.Scale;
                
                // Compute the position of the socket.
                socket.Rect.position = new Vector2(
                    x: socket.IsInput ? 
                        socket.Owner.BodyRect.position.x + socket.Rect.width / 2 :
                        socket.Owner.BodyRect.position.x + socket.Owner.BodyRect.width - 3 * socket.Rect.width / 2,
                    y: socket.Owner.BodyRect.position.y + socket.Rect.height / 2
                );
            }
            
            /// <summary>
            /// Handle the socket's events.
            /// Allows disconnecting from a specific parent/child.
            /// </summary>
            /// <param name="socket">The socket to handle the events of.</param>
            public static void HandleEvents(SocketState socket) {
                // Check if the socket is enabled.
                if (!socket.IsEnabled) return;
                
                // Get the position of the click relative to the render area.
                Vector2 mousePos = Event.current.mousePosition - socket.Owner.Owner.RenderRect.position;
                
                // Handle the kind of mouse event.
                switch (Event.current.type) {
                    // If the mouse was clicked.
                    case EventType.MouseDown:
                        // Check if the socket was pressed.
                        if (Event.current.button == (int)MouseButton.LeftMouse && socket.Rect.Contains(point: mousePos)) {
                            // Select the current socket.
                            SocketState.Manipulated = socket;
                            
                            // Use the event.
                            Event.current.Use();
                        }
                        break;
                    
                    // If a right click was made.
                    case EventType.ContextClick:
                        // Check if the socket was pressed.
                        if (socket.Rect.Contains(point: mousePos)) {
                            // Show the deletion menu.
                            SocketController._DeleteBindMenu(socketState: socket);
                            
                            // Use the event.
                            Event.current.Use();
                        }
                        break;
                    
                    // If the mouse was released.
                    case EventType.MouseUp:
                        // If there was a manipulated socket.
                        if (SocketState.Manipulated != null) {
                            // If the socket was under the mouse.
                            if (socket.Rect.Contains(point: mousePos)) {
                                // Bind the two sockets.
                                SocketController._BindSockets(a: socket, b: SocketState.Manipulated);
                            }
                        }
                        break;
                }
            }
            
        // -- Private Methods --
            /// <summary>
            /// Binds two specified sockets together.
            /// </summary>
            private static void _BindSockets(SocketState a, SocketState b) {
                // Ensure that both the socket's owners are different.
                if (a.Owner == b.Owner) return;
                
                // Check which socket is the input socket.
                if (a.IsInput && !b.IsInput) {
                    // Attach the nodes together.
                    NodeController.AddChildNode(parent: b.Owner, child: a.Owner);
                } else if (!a.IsInput && b.IsInput) {
                    // Attach the nodes together.
                    NodeController.AddChildNode(parent: a.Owner, child: b.Owner);
                }
            }
            
            /// <summary>
            /// Shows the menu that allows deleting a link to the specified object.
            /// </summary>
            /// <param name="socketState">The socket to show the menu for.</param>
            private static void _DeleteBindMenu(SocketState socketState) {
                // Create the menu instance.
                GenericMenu menu = new GenericMenu();
                
                // Add the separator.
                menu.AddSeparator(path: "Delete link to: ");
                // Check the type of the socket.
                if (socketState.IsInput) {
                    // Loop through the parents of the owner.
                    foreach (NodeState parent in socketState.Owner.ParentList.ToArray()) {
                        // Add an option to unlink to the parent.
                        menu.AddItem(
                            content: new GUIContent {
                                text = $"- {parent.Action?.ControllerType.Name.Replace(oldValue: "Action", newValue: "")}"
                            },
                            on: false,
                            func: () => NodeController.RemoveChildNode(parent: parent, child: socketState.Owner) 
                        );
                    }
                } else {
                    // Loop through the children of the owner.
                    foreach (NodeState child in socketState.Owner.ChildrenList.ToArray()) {
                        // Add an option to unlink to the parent.
                        menu.AddItem(
                            content: new GUIContent {
                                text = $"- {child.Action?.ControllerType.Name.Replace(oldValue: "Action", newValue: "")}"
                            },
                            on: false,
                            func: () => NodeController.RemoveChildNode(parent: socketState.Owner, child: child) 
                        );
                    }
                }
                
                // Add the all option.
                menu.AddSeparator(path: "");
                menu.AddItem(
                    content: new GUIContent{ text = "Delete all links"},
                    on: false,
                    func: () => {
                        // Check the type of the socket.
                        if (socketState.IsInput) {
                            // Loop through the parents of the owner.
                            foreach (NodeState parent in socketState.Owner.ParentList.ToArray()) {
                                // Remove the child.
                                NodeController.RemoveChildNode(parent: parent, child: socketState.Owner); 
                            }
                        } else {
                            // Loop through the children of the owner.
                            foreach (NodeState child in socketState.Owner.ChildrenList.ToArray()) {
                                // Remove the parent.
                                NodeController.RemoveChildNode(parent: socketState.Owner, child: child); 
                            }
                        }
                    } 
                );
                
                // Show the menu.
                menu.ShowAsContext();
            }
    // --- /Methods ---
}
}