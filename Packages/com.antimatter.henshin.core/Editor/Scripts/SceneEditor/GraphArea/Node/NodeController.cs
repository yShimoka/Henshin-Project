// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Henshin.Editor.SceneEditor.GraphArea.Socket;
using Henshin.Runtime.Actions;
using Henshin.Runtime.Actions.Actor;
using Henshin.Runtime.Actions.Scene;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.GraphArea.Node {

/// <summary>
/// Controller class used to manipulate <see cref="NodeState"/> objects.
/// </summary>
public static class NodeController {
    // ---  Attributes ---
        // -- Private Attributes --
            /// <summary>
            /// Menu object that is rendered when a node is right clicked.
            /// </summary>
            private static GenericMenu _msDeleteMenu;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            // - Initialization -
            /// <summary>
            /// Initializes the specified <see cref="NodeState"/> object.
            /// </summary>
            /// <param name="node">The node object to initialize.</param>
            /// <param name="owner">The owner of the initialized node.</param>
            public static void Initialize(NodeState node, GraphAreaState owner) {
                // Store the owner of the node.
                node.Owner = owner;
                
                // Load all the children of the node.
                foreach (NodeState child in node.ChildrenIndexList
                    .Select(selector: childIndex => owner.NodeList[index: childIndex])
                ) {
                    // Add the child to the node.
                    node.ChildrenList.Add(item: child);
                    
                    // Add the parent to the node.
                    child.ParentList.Add(item: node);
                }
                
                // Create the node's sockets.
                node.Input  = new SocketState{ Owner = node, IsInput = true  };
                node.Output = new SocketState{ Owner = node, IsInput = false };
                
                // Initialize them.
                SocketController.Initialize(socket: node.Input);
                SocketController.Initialize(socket: node.Output);
            }
            
            /// <summary>
            /// Creates a new <see cref="NodeState"/> object alongside a new <see cref="ActionState"/>.
            /// Stores the new objects in their respective arrays.
            /// </summary>
            /// <param name="owner">The <see cref="GraphAreaState"/> that will own the new instance.</param>
            /// <param name="position">The position to place the node at.</param>
            /// <typeparam name="TActionType">The type of the underlying <see cref="ActionController"/>.</typeparam>
            public static void CreateNode<TActionType>(GraphAreaState owner, Vector2 position) 
                where TActionType: ActionController {
                // Create a new action controller instance.
                ActionState newAction = ActionController.CreateController<TActionType>().State;
                
                // Store the action in the owner's scene.
                owner.Scene.ActionList.Add(item: newAction);
                
                // Create a new node object.
                NodeState node = new NodeState {
                    ActionIndex = owner.Scene.ActionList.Count - 1,
                    Position = position,
                    NodeIndex = owner.NodeList.Count
                };
                // Initialize it.
                NodeController.Initialize(node: node, owner: owner);
                
                // Store the node in the owner.
                owner.NodeList.Add(item: node);
            }
            
            // - Event Handling -
            /// <summary>
            /// Prepares the <see cref="NodeState"/> object before it will be rendered.
            /// </summary>
            /// <param name="node">The node object to prepare.</param>
            public static void Prepare(NodeState node) {
                // Compute the position of the node.
                node.HeaderRect.position = GraphAreaController
                    .GraphToWindow(graphArea: node.Owner, position: node.Position, windowSpace: false);
                node.BodyRect.position = new Vector2(
                    x: Mathf.Ceil(f: node.HeaderRect.position.x),
                    y: Mathf.Floor(f: node.HeaderRect.position.y + NodeState.HEADER_HEIGHT * node.Owner.Scale)
                );
                
                // Compute the size of the node.
                node.HeaderRect.size = new Vector2(x: NodeState.WIDTH, y: NodeState.HEADER_HEIGHT) * node.Owner.Scale;
                node.BodyRect.size = new Vector2(x: NodeState.WIDTH, y: NodeState.BODY_HEIGHT) * node.Owner.Scale;
                
                // Prepare the sockets.
                SocketController.Prepare(socket: node.Input);
                SocketController.Prepare(socket: node.Output);
            }
            
            /// <summary>
            /// Handles the <see cref="Event.current"/> object for the specified <see cref="NodeState"/>.
            /// </summary>
            /// <param name="node">The node to handle the events for.</param>
            public static void HandleEvents(NodeState node) {
                // Check if the node's owner is set.
                if (node.Owner == null) return;
                
                // Handle the events for the sockets.
                SocketController.HandleEvents(socket: node.Input);
                SocketController.HandleEvents(socket: node.Output);
                
                // Get the area-relative position of the mouse position.
                Vector2 renderMousePos = Event.current.mousePosition - node.Owner.RenderRect.position;
                
                // Handle the event type.
                switch (Event.current.type) {
                    // If the object was clicked.
                    case EventType.MouseDown:
                        // Check if the event happened within the area of the node.
                        if (
                            Event.current.button == (int)MouseButton.LeftMouse && (
                            node.BodyRect.Contains(point: renderMousePos) || 
                            node.HeaderRect.Contains(point: renderMousePos)
                        )) {
                            // Grab the object.
                            node.IsDragged = true;
                            
                            // Select the node.
                            node.Owner.Owner.State.Inspector.EditedNode = node;
                            
                            // Store the mouse offset.
                            node.MouseOffset = renderMousePos - GraphAreaController
                                .GraphToWindow(graphArea: node.Owner, position: node.Position);
                            
                            // Use the event.
                            Event.current.Use();
                        }
                        break;
                    // If the object was right clicked.
                    case EventType.ContextClick:
                        // Check if the event happened within the area of the node.
                        if (node.BodyRect.Contains(point: renderMousePos) || 
                            node.HeaderRect.Contains(point: renderMousePos)
                        ) {
                            // Create the menu.
                            NodeController._ShowMenu(node: node);
                            
                            // Use the event.
                            Event.current.Use();
                        }
                        break;
                    // If the object was released.
                    case EventType.MouseUp:
                        // Release the object.
                        node.IsDragged = false;
                        break;
                    // If the object was moved.
                    case EventType.MouseDrag:
                        // If the object is dragged.
                        if (node.IsDragged) {
                            // Move the object.
                            Vector2 newPos = renderMousePos - node.MouseOffset;
                            
                            // Convert the position in graph scale.
                            node.Position = GraphAreaController.WindowToGraph(graphArea: node.Owner, position: newPos);
                            
                            // Use the event.
                            Event.current.Use();
                        }
                        break;
                    }
            }
            
            // - Tree Behaviour -
            /// <summary>
            /// Adds a new child to the <see cref="parent"/> node.
            /// </summary>
            /// <param name="parent">The new parent of the <see cref="child"/> node.</param>
            /// <param name="child">The new child of the <see cref="parent"/> node.</param>
            public static void AddChildNode([NotNull]NodeState parent, [NotNull]NodeState child) {
                // Check if the action is set.
                if (parent.Action != null && child.Action != null) {
                    // Update the underlying action object.
                    parent.Action.ChildrenList.Add(item: child.Action);
                    parent.Action.ChildrenIndexList.Add(item: child.ActionIndex);
                } else {
                    // Throw an exception.
                    throw new InvalidOperationException(message: "Cannot add a child to an invalid node.");
                }

                // Add the child to the parent's lists.
                parent.ChildrenList.Add(item: child);
                parent.ChildrenIndexList.Add(item: child.NodeIndex);
                
                // Add the parent to the child's lists.
                child.ParentList.Add(item: parent);
            }
            
            /// <summary>
            /// Removes the <see cref="child"/> node from te <see cref="parent"/>'s lists.
            /// </summary>
            /// <param name="parent">The parent node to remove the <see cref="child"/> from.</param>
            /// <param name="child">The child node to remove.</param>
            public static void RemoveChildNode([NotNull]NodeState parent, [NotNull]NodeState child) {
                // Check if the action is set.
                if (parent.Action != null && child.Action != null) {
                    // Update the underlying action object.
                    parent.Action.ChildrenList.Remove(item: child.Action);
                    parent.Action.ChildrenIndexList.Remove(item: child.ActionIndex);
                } else {
                    // Throw an exception.
                    throw new InvalidOperationException(message: "Cannot remove a child from an invalid node.");
                }
                
                // Remove the child from the parent's lists.
                parent.ChildrenList.Remove(item: child);
                parent.ChildrenIndexList.Remove(item: child.NodeIndex);
                
                // Remove the parent from the child's list.
                child.ParentList.Remove(item: parent);
            }
            
            /// <summary>
            /// Deletes the specified <see cref="NodeState"/> instance.
            /// </summary>
            /// <param name="node">The node item to delete.</param>
            private static void DeleteNode([NotNull]NodeState node) {
                // Remove the node from all of its parents.
                foreach (NodeState parent in node.ParentList.ToArray()) {
                    NodeController.RemoveChildNode(parent: parent, child: node);
                }
                // Remove the node from all its children.
                foreach (NodeState child in node.ChildrenList.ToArray()) {
                    NodeController.RemoveChildNode(parent: node, child: child);
                }
                
                // Decrement the indices of all the next actions.
                for (int i = node.ActionIndex + 1; i < node.Owner.NodeList.Count; i++) {
                    // Get a reference to the node.
                    NodeState other = node.Owner.NodeList[index: i]; 
                    
                    // Decrement its indices.
                    other.ActionIndex--;
                    other.NodeIndex--;
                }
                
                // Remove the pointed action.
                node.Owner.Scene.ActionList.RemoveAt(index: node.ActionIndex);
                
                // Remove the node.
                node.Owner.NodeList.Remove(item: node);
            }
            
        // -- Private Methods --
            /// <summary>
            /// Shows the <see cref="GenericMenu"/> that is related to the specified <see cref="NodeState"/>.
            /// </summary>
            /// <param name="node">The node to get the menu for.</param>
            private static void _ShowMenu(NodeState node) {
                // Re-Create the menu.
                NodeController._msDeleteMenu = new GenericMenu();
                
                // Add the conversion options.
                NodeController._msDeleteMenu.AddSeparator(path: "Convert/- Scene Actions");
                if (GraphAreaController.HasActionType<StartAction>(graphArea: node.Owner)) {
                    NodeController._msDeleteMenu.AddDisabledItem(
                        content: new GUIContent{ text = "Convert/Start" }, 
                        on: true
                    );
                } else {
                    NodeController._msDeleteMenu.AddItem(
                        content: new GUIContent{ text = "Convert/Start" }, 
                        on: false, 
                        func: () => NodeController._ConvertNode<StartAction>(node: node));
                }
                if (GraphAreaController.HasActionType<EndAction>(graphArea: node.Owner)) {
                    NodeController._msDeleteMenu.AddDisabledItem(
                        content: new GUIContent{ text = "Convert/End" }, 
                        on: true
                    );
                } else {
                    NodeController._msDeleteMenu.AddItem(
                        content: new GUIContent{ text = "Convert/End" }, 
                        on: false, 
                        func: () => NodeController._ConvertNode<EndAction>(node: node));
                }
                NodeController._msDeleteMenu.AddSeparator(path: "Convert/");
                NodeController._msDeleteMenu.AddSeparator(path: "Convert/- Actor Actions");
                NodeController._msDeleteMenu.AddItem(
                    content: new GUIContent{ text = "Convert/Visible" }, 
                    on: false, 
                    func: () => NodeController._ConvertNode<VisibleAction>(node: node)
                );
                NodeController._msDeleteMenu.AddItem(
                    content: new GUIContent{ text = "Convert/Layer" }, 
                    on: false, 
                    func: () => NodeController._ConvertNode<LayerAction>(node: node)
                );
                NodeController._msDeleteMenu.AddItem(
                    content: new GUIContent{ text = "Convert/Flip" }, 
                    on: false, 
                    func: () => NodeController._ConvertNode<FlipAction>(node: node)
                );
                NodeController._msDeleteMenu.AddItem(
                    content: new GUIContent{ text = "Convert/Pose" }, 
                    on: false, 
                    func: () => NodeController._ConvertNode<PoseAction>(node: node)
                );
                NodeController._msDeleteMenu.AddItem(
                    content: new GUIContent{ text = "Convert/MoveTo" }, 
                    on: false, 
                    func: () => NodeController._ConvertNode<MoveToAction>(node: node)
                );
                NodeController._msDeleteMenu.AddItem(
                    content: new GUIContent{ text = "Convert/Rotate" }, 
                    on: false, 
                    func: () => NodeController._ConvertNode<RotateAction>(node: node)
                );
                NodeController._msDeleteMenu.AddItem(
                    content: new GUIContent{ text = "Convert/Scale" }, 
                    on: false, 
                    func: () => NodeController._ConvertNode<ScaleAction>(node: node)
                );
                NodeController._msDeleteMenu.AddItem(
                    content: new GUIContent{ text = "Convert/Colour" }, 
                    on: false, 
                    func: () => NodeController._ConvertNode<ColourAction>(node: node)
                );
                NodeController._msDeleteMenu.AddSeparator(path: "Convert/");
                NodeController._msDeleteMenu.AddSeparator(path: "Convert/- Gameplay Actions");
                
                // Add the delete button.
                NodeController._msDeleteMenu.AddSeparator(path: "");
                NodeController._msDeleteMenu.AddItem(
                    content: new GUIContent{ text = "Delete" },
                    on: false,
                    func: () => NodeController.DeleteNode(node: node)
                );
                
                // Show the menu.
                NodeController._msDeleteMenu.ShowAsContext();
            }
            
            /// <summary>
            /// Converts the specified <see cref="NodeState"/> instance to the specified action type.
            /// </summary>
            /// <param name="node">The node item to convert.</param>
            /// <typeparam name="TActionType">The new type of the node's action.</typeparam>
            private static void _ConvertNode<TActionType>(NodeState node) where TActionType: ActionController {
                // Create a new action state instance.
                ActionState newState = ActionController.CreateController<TActionType>().State;
                
                // Replace the pointed action.
                node.Owner.Scene.ActionList[index: node.ActionIndex] = newState;
            }
    // --- /Methods ---
}
}