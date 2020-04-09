// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System.Collections.Generic;
using Henshin.Core.Scene.Directions;
using Henshin.Core.Scene.Scenery.Transformation;
using Henshin.Editor.Misc;
using Henshin.Editor.Scene.Transformation.Graph;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.Scene.Transformation {

/// <summary>
/// Class used to render the <see cref="Graph.GraphCanvas"/> objects.
/// </summary>
public static class TransformationEditorCanvas {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>Size of the canvas texture.</summary>
            public readonly static Vector2 CellSize = new Vector2(x: 25f, y: 25f);

            /// <summary>Number of cells on the canvas.</summary>
            public const int CELL_COUNT = 100;
            
            /// <summary>Helper used to access the full canvas size.</summary>
            public static Vector2 CanvasSize => TransformationEditorCanvas.CellSize * TransformationEditorCanvas.CELL_COUNT;

            /// <summary>Current position of the canvas in the world.</summary>
            public static Vector2 CanvasPosition { get; set; }
        // -- Private Attributes --
            // - Canvas State -
            /// <summary>List of all the nodes rendered on the canvas.</summary>
            private static List<GraphNode> _msNodes = null;
            
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            public static void OnSceneChanged() { CanvasPosition = Vector2.zero; TransformationEditorCanvas._ReloadGraphNodes(); }
        
            public static void OnGUI() {
                // If the nodes are not loaded.
                if (TransformationEditorCanvas._msNodes == null) {
                    // Load all the nodes.
                    TransformationEditorCanvas._ReloadGraphNodes();
                }
                
                // Get the canvas rect.
                Rect canvasContainer = TransformationEditorState.GetCanvasRect();
                Rect canvas = new Rect {
                    position = 
                        TransformationEditorCanvas.CanvasPosition +
                        new Vector2(x: canvasContainer.width / 2, y: canvasContainer.height / 2) -
                        TransformationEditorCanvas.CanvasSize / 2,
                        size = TransformationEditorCanvas.CanvasSize
                };
                
                // Start a scrollable area.
                GUI.BeginGroup(position: canvasContainer);
                
                // Draw the background.
                GUI.DrawTextureWithTexCoords(position: canvas, image: TextureStore.GraphBackground, texCoords: new Rect{ size = Vector2.one * TransformationEditorCanvas.CELL_COUNT / 4 });
                
                // Draw the nodes.
                foreach (GraphNode node in TransformationEditorCanvas._msNodes) {
                    node.Draw();
                }
                
                // Handle mouse events.
                TransformationEditorCanvas._HandleMouseEvent();
                
                // End the scrollable area.
                GUI.EndGroup();
            }
            
        // -- Private Methods --
            /// <summary>Handles the current mouse event.</summary>
            private static void _HandleMouseEvent() {
                // Check if the canvas is being moved.
                if (Event.current.type == EventType.ScrollWheel) {
                    // Update the canvas position.
                    TransformationEditorCanvas.CanvasPosition -= Event.current.delta * 1e1f;
                    
                    // Repaint the canvas.
                    TransformationEditorWindow.Repaint();
                }
                
                // If the user started a context click.
                if (Event.current.type == EventType.ContextClick) {
                    // Create the contextual menu.
                    GenericMenu menu = new GenericMenu();
                    
                    // Create the "Add Line" item.
                    Vector2 at = Event.current.mousePosition;
                    menu.AddItem(content: new GUIContent{ text = "Add line" }, on: false, func: () => {
                        // Create a new line object.
                        Line line = new Line();
                        
                        // Create a new Start and End node.
                        Start start = new Start();
                        End end = new End();
                        
                        // Set their positions.
                        start.Position = new Vector2(x: 0, y: TransformationEditorState.CurrentScene.lines.Count * 100);
                        end.Position = new Vector2(x: 100, y: TransformationEditorState.CurrentScene.lines.Count * 100);
                        
                        // Define the names of the elements.
                        line.identifier = $"{TransformationEditorState.CurrentScene.identifier}_Line#{TransformationEditorState.CurrentScene.lines.Count + 1}";
                        start.Identifier = $"{line.identifier}_StartTransformation";
                        end.Identifier = $"{line.identifier}_EndTransformation";
                        
                        // Set the line owner of the elements.
                        start.Owner = line;
                        end.Owner = line;
                        
                        // Add a start and end transformation object to the list.
                        line.RootTransformation = start;
                        line.RootTransformation.Nodes.Add(item: end);
                        
                        // Add the line to the scene.
                        TransformationEditorState.CurrentScene.lines.Add(item: line);
                        
                        // Center the canvas on the object.
                        TransformationEditorCanvas.CanvasPosition = at;
                        
                        // Refresh the graph nodes.
                        TransformationEditorCanvas._ReloadGraphNodes();
                    });
                    
                    // Draw the menu.
                    menu.ShowAsContext();
                }
            }
            
            /// <summary>Reloads all the <see cref="GraphNode"/> objects.</summary>
            private static void _ReloadGraphNodes() {
                // Create the node list.
                TransformationEditorCanvas._msNodes = new List<GraphNode>();
                
                // Loop through the lines of the current scene.
                foreach (Line line in TransformationEditorState.CurrentScene.lines) {
                    // Load the nodes of the line.
                    TransformationEditorCanvas._LoadNode(node: line.RootTransformation);
                }
            }
            
            /// <summary>Loads the specified node and its children.</summary>
            /// <param name="node">The node to load.</param>
            private static void _LoadNode(Base node) {
                // If the node is null, do nothing.
                if (node == null) return;
                
                // Create a new graph node.
                TransformationEditorCanvas._msNodes.Add(item: new GraphNode(referenced: node));
                
                // Loop through the children of the node.
                foreach (Base child in node.Nodes) {
                    // Load that node.
                    TransformationEditorCanvas._LoadNode(node: child);
                }
            }
    // --- /Methods ---
}
}