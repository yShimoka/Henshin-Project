// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEngine;
using System.Collections.Generic;
using Enumerable = System.Linq.Enumerable;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.Controller.Graph {

/// <summary>
/// Controller class for the node objects.
/// </summary>
public static class Node {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Creates a new <see cref="Henshin.Editor.State.Graph.Node"/> object.
            /// This node is then immediately renderable in a <see cref="Henshin.Editor.State.Graph.RenderArea"/>.
            /// </summary>
            /// <param name="transformationIndex">The index of the transformation in the scene's transformations.</param>
            /// <param name="at">The position of the created node.</param>
            /// <param name="owner">The owner of the node.</param>
            /// <typeparam name="TTransformationController">The type of the transformation controller for the manipulated transformation.</typeparam>
            /// <returns>The generated <see cref="State.Graph.Node"/> object.</returns>
            public static State.Graph.Node CreateNode<TTransformationController>(int transformationIndex, Vector2 at, State.Graph.RenderArea owner) where TTransformationController: Henshin.Controller.Directions.Transformation {
                return Node.CreateNode(type: typeof(TTransformationController), transformationIndex: transformationIndex, at: at, owner: owner);
            }
            
            /// <summary>
            /// Creates a new <see cref="Henshin.Editor.State.Graph.Node"/> object.
            /// This node is then immediately renderable in a <see cref="Henshin.Editor.State.Graph.RenderArea"/>.
            /// </summary>
            /// <param name="type">The type of the transformation controller for the manipulated transformation.</param>
            /// <param name="transformationIndex">The index of the transformation in the scene's transformations.</param>
            /// <param name="at">The position of the created node.</param>
            /// <param name="owner">The owner of the node.</param>
            /// <param name="nodeIndices">The node output's indices.</param>
            /// <returns>The generated <see cref="State.Graph.Node"/> object.</returns>
            public static State.Graph.Node CreateNode(System.Type type, int transformationIndex, Vector2 at, State.Graph.RenderArea owner, IEnumerable<int> nodeIndices = null) {
                // Create a new node instance.
                State.Graph.Node node = new State.Graph.Node {
                    position = at,
                    owner = owner,
                    transformationIndex = transformationIndex,
                    targetIndices = nodeIndices == null ? new List<int>() : new List<int>(collection: nodeIndices),
                };
                
                // Generate the input socket.
                if (type != typeof(Henshin.Controller.Directions.Transformations.Scene.Start)) {
                    node.Input = Socket.CreateSocket(owner: node, isInput: true);
                }
                
                // Generate the output socket.
                if (type != typeof(Henshin.Controller.Directions.Transformations.Scene.End)) {
                    node.Output = Socket.CreateSocket(owner: node, isInput: false);
                }
                
                // Return the node.
                return node;
            }
            
            /// <summary>
            /// Method called right before the specified node is rendered.
            /// </summary>
            public static void BeforeRender(Vector2 canvasPosition, State.Graph.Node node) {
                // Update the size of the node's header.
                node.HeaderRect.size = State.Graph.Node.HEADER_SIZE * (1 / node.owner.scale);
                node.BodyRect.size = State.Graph.Node.BODY_SIZE * (1 / node.owner.scale);
                
                // Update the position of the node.
                node.HeaderRect.position = node.position * (1 / node.owner.scale) * State.Graph.RenderArea.GUI_CELL_SIZE + canvasPosition;
                node.BodyRect.position = new Vector2(x: node.HeaderRect.position.x, y: node.HeaderRect.y + node.HeaderRect.height);
                
                node.TextRect.size = State.Graph.Node.HEADER_SIZE * (1 / node.owner.scale);
                node.TextRect.position = node.HeaderRect.position;
                
                // Update the sockets of the node.
                if (node.Input != null) {
                    Socket.BeforeRender(container: node.BodyRect, socket: node.Input);
                } 
                if (node.Output != null) {
                    Socket.BeforeRender(container: node.BodyRect, socket: node.Output);
                }
            }
            
            /// <summary>
            /// Method called right after the node is rendered.
            /// </summary>
            /// <param name="ev">The currently handled event.</param>
            /// <param name="node">The node to handle events for.</param>
            public static void AfterRender(Event ev, State.Graph.Node node) {
                // Update the sockets of the node.
                if (node.Input != null) {
                    Socket.AfterRender(ev: ev, socket: node.Input);
                } 
                if (node.Output != null) {
                    Socket.AfterRender(ev: ev, socket: node.Output);
                }
                
                // Check if the node has been grabbed.
                if (node.IsDragged) {
                    switch (ev.type) {
                    // Check if the mouse was released.
                    case EventType.MouseUp:
                        // Stop the dragging.
                        node.IsDragged = false;
                        break;
                    // Check if the mouse was moved.
                    case EventType.MouseDrag:
                        // Move the node.
                        node.position = node.owner.scale * (ev.mousePosition - node.FullRect.size / 2 - node.owner.Position - State.SceneEditor.Canvas.Rect.position);
                        
                        // Get the top left position of the node.
                        const int GRID = State.Graph.RenderArea.GUI_CELL_SIZE;
                        
                        // Bind the node to the grid.
                        node.position.x = Mathf.FloorToInt(f: node.position.x / GRID);  
                        node.position.y = Mathf.FloorToInt(f: node.position.y / GRID);
                        
                        // Use the event.
                        ev.Use();
                        
                        // Repaint the window.
                        RenderArea.ON_SHOULD_REPAINT.Invoke();
                        break;
                    }
                } else {
                    // Check if the node was pressed.
                    if (node.FullRect.Contains(point: ev.mousePosition - State.SceneEditor.Canvas.Rect.position)) {
                        // Check if the mouse was pressed.
                        if (ev.type == EventType.MouseDown && ev.button == (int)UnityEngine.UIElements.MouseButton.LeftMouse) {
                            // Set the drag flag.
                            node.IsDragged = true;
                            
                            // Set the selected node.
                            State.Graph.Node.CurrentNode = node;
                            
                            // Consume the event.
                            ev.Use();
                        }
                    
                        // Check if the mouse made a context click.
                        if (ev.type == EventType.ContextClick && (ev.modifiers & EventModifiers.Shift) != 0) {
                            // Clear the node connections.
                            if (node.Input != null) {
                                foreach (State.Graph.Node target in node.Input.Targets) {
                                    // Delete the node.
                                    target.Output.Targets.Remove(item: node);
                                }
                                node.Input.Targets.Clear();
                            }
                            if (node.Output != null) { 
                                foreach (State.Graph.Node target in node.Output.Targets) {
                                    // Delete the node.
                                    target.Input.Targets.Remove(item: node);
                                }
                                node.Output.Targets.Clear();
                            }
                            
                            // Update all the other node's indices.
                            for (int i = node.transformationIndex + 1; i < node.owner.nodes.Count; i++) {
                                // Get the node object.
                                State.Graph.Node other = node.owner.nodes[index: i];
                                
                                // Decrement the index of the transformation.
                                other.transformationIndex--;
                            }
                            // Delete the node's transformation.
                            node.owner.describedScene.Transformations.Remove(item: node.Transformation);
                            
                            // Remove the node itself.
                            node.owner.nodes.Remove(item: node);
                            
                            // Clear the current node.
                            State.Graph.Node.CurrentNode = null;
                            
                            // Use the event.
                            ev.Use();
                            
                            // Repaint the window.
                            RenderArea.ON_SHOULD_REPAINT.Invoke();
                        }
                    }
                }
            }
            
            /// <summary>
            /// Deserializes the specified node object.
            /// </summary>
            public static void Deserialize(State.Graph.Node node, List<State.Graph.Node> allNodes) {
                // Loop through the target ids of the node.
                foreach (Henshin.Editor.State.Graph.Node child in Enumerable.Select(source: node.targetIndices, selector: index => allNodes[index: index])) {
                    // Add the node as the target of the node output.
                    node.Output.Targets.Add(item: child);
                    
                    // Add the node as the target of the child input.
                    child.Input.Targets.Add(item: node);
                }
            }
            
            /// <summary>
            /// Serializes the specified node object.
            /// </summary>
            public static void Serialize(State.Graph.Node node, List<State.Graph.Node> allNodes) {
                // Clear the target index list.
                node.targetIndices = new List<int>();
                node.Transformation.State.nodeIndices = new List<int>();
                
                // If the node has an output.
                if (node.Output != null) {
                    // Loop through the target ids of the node.
                    foreach (int nodeIndex in Enumerable.Select(source: node.Output.Targets, selector: child => allNodes.IndexOf(item: child))) {
                        node.targetIndices.Add(item: nodeIndex);
                        node.Transformation.State.nodeIndices.Add(item: nodeIndex);
                    }
                }
            }
        // -- Private Methods --
    // --- /Methods ---
}
}