// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Henshin.Editor.SceneEditor.GraphArea.Node;
using Henshin.Editor.SceneEditor.GraphArea.Socket;
using Henshin.Editor.SceneEditor.Header;
using Henshin.Editor.Skin;
using Henshin.Runtime.Actions;
using Henshin.Runtime.Actions.Actor;
using Henshin.Runtime.Actions.Scene;
using Henshin.Runtime.Directions.Act;
using Henshin.Runtime.Directions.Scene;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.GraphArea {

/// <summary>
/// Static class used to manipulate <see cref="GraphAreaState"/> objects.
/// Loads all the <see cref="GraphAreaStore"/> instances on initialization.
/// </summary>
public static class GraphAreaController {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Stores the current mouse's position.
            /// </summary>
            public static Vector2 CurrentMousePosition;
            
        // -- Private Attributes --
            /// <summary>
            /// Stores the position of the mouse for the <see cref="_CreateNodeMenu"/> method.
            /// </summary>
            private static Vector2 _msLastMousePosition;
    // --- /Attributes ---

    // ---  Methods ---
        // -- Public Methods --
            // - Initialization -
            /// <summary>
            /// Initializes the GraphArea controller class.
            /// Loads all the stores from the disk.
            /// </summary>
            public static void ReloadStores() {
                // Load all the stores from disk.
                List<GraphAreaStore> loadedStores = AssetDatabase
                    .FindAssets(filter: $"t:{nameof(GraphAreaStore)}")
                    .Select(selector: AssetDatabase.GUIDToAssetPath)
                    .Select(selector: AssetDatabase.LoadAssetAtPath<GraphAreaStore>)
                    .ToList();
                // Prepare the list of all the valid stores.
                List<GraphAreaStore> validStores = new List<GraphAreaStore>();
                
                // Loop through all the existing acts.
                foreach (ActState act in HeaderState.EditableApplications.SelectMany(selector: app => app.ActList)) {
                    // Check if a store exists for that act.
                    if (loadedStores.Find(match: store => store.OwnerHash == act.Hash) is GraphAreaStore actStore) {
                        // Move the store the the valid list.
                        loadedStores.Remove(item: actStore);
                        validStores.Add(item: actStore);
                        
                        // Refresh the name of the store.
                        actStore.name = $"{act.Identifier}_Store";
                        // Store the owner of the act store.
                        actStore.Owner = act;

                        // Validate the scenes of the store.
                        GraphAreaController._ValidateStore(store: actStore);
                    } else {
                        // Add a new store to the valid list.
                        validStores.Add(item: GraphAreaController._CreateStore(@for: act));
                    }
                }
                
                // Destroy all the left over stores.
                foreach (GraphAreaStore store in loadedStores) {
                    // Get the path to the store.
                    string storePath = AssetDatabase.GetAssetPath(assetObject: store);
                    
                    // Check if the path is set.
                    if (!string.IsNullOrEmpty(value: storePath)) {
                        // Delete the asset.
                        AssetDatabase.DeleteAsset(path: storePath);
                    } else {
                        Debug.LogError(message: $"Could not delete the store {store.name}");
                    }
                }
                
                // Store the valid stores.
                GraphAreaStore.StoreList = validStores.ToArray();
            }
            
            /// <summary>
            /// Initializes the specified <see cref="GraphAreaState"/> object.
            /// </summary>
            /// <param name="graphArea">The graph area object to initialize.</param>
            public static void Initialize([NotNull]GraphAreaState graphArea) {
                // Create the graph area's rects.
                graphArea.RenderRect = new Rect();
                graphArea.GraphRect = new Rect {
                    x = 0, y = 0, 
                    width  = GraphAreaState.GRAPH_SIZE_X,
                    height = GraphAreaState.GRAPH_SIZE_Y
                };
                graphArea.TextureRect = new Rect {
                    x = 0, y = 0, 
                    width  = (float)GraphAreaState.CELL_COUNT_X / GraphAreaView.TEXTURE_CELL_COUNT,
                    height = (float)GraphAreaState.CELL_COUNT_Y / GraphAreaView.TEXTURE_CELL_COUNT
                };
                
                // Get a list of all the graph area owner scene's actions.
                List<ActionState> sceneActions = new List<ActionState>(collection: graphArea.Scene.ActionList);
                // Initialize all the nodes.
                for (int index = 0; index < graphArea.NodeList.Count; index++) {
                    // Get the node at the specified index.
                    NodeState nodeState = graphArea.NodeList[index: index];
                    
                    // Set its index.
                    nodeState.NodeIndex = index;
                    
                    // Check if the pointed actions is valid.
                    if (nodeState.ActionIndex >= 0 && nodeState.ActionIndex < sceneActions.Count) {
                        // Remove the action from the list.
                        sceneActions[index: nodeState.ActionIndex] = null;

                        // Initialize the node.
                        NodeController.Initialize(node: nodeState, owner: graphArea);
                    } else {
                        // Log an error.
                        Debug.LogWarning(message: $"The node #{index} is referring to an invalid action #{nodeState.ActionIndex}");
                        
                        // Remove the invalid node.
                        graphArea.NodeList.RemoveAt(index: index);
                    }
                }

                // Log all the inaccessible actions.
                for (int index = 0; index < sceneActions.Count; index++) {
                    // Check if the action is not null.
                    if (sceneActions[index: index] != null) {
                        // Log an error.
                        Debug.LogError(message: $"Action #{index} has no node representing it ! It will be inaccessible.");
                    }
                }
            }
            
            /// <summary>
            /// Prepare the GraphArea for rendering.
            /// </summary>
            /// <param name="graphArea">The graph area to prepare.</param>
            /// <param name="owner">The owner of the graph area.</param>
            public static void Prepare([CanBeNull]GraphAreaState graphArea, [NotNull]SceneEditorController owner) {
                // Check if the graph area is set.
                if (graphArea == null) return;
                
                // Apply the graph area's ratio.
                SkinState.RatioStruct.ApplyRatio(
                    from: owner.State.WindowRect,
                    ratio: SkinState.Ratios.SceneEditorGraphAreaRatio,
                    to: ref graphArea.RenderRect
                );
                
                // Apply the scale of the area.
                graphArea.GraphRect.size = new Vector2(
                    x: GraphAreaState.GRAPH_SIZE_X * graphArea.Scale,
                    y: GraphAreaState.GRAPH_SIZE_Y * graphArea.Scale
                );
                
                // Move the graph area.
                graphArea.GraphRect.center = graphArea.Position;
                
                // Prepare all the nodes.
                foreach (NodeState nodeState in graphArea.NodeList) { NodeController.Prepare(node: nodeState); }
            }
            
            /// <summary>
            /// Handles the <see cref="Event.current"/> object.
            /// Moves the graph area around with alt+mouse and changes the scale with the scroll wheel.
            /// </summary>
            /// <param name="graphArea">The area to handle the events for.</param>
            public static void HandleEvents([CanBeNull]GraphAreaState graphArea) {
                // Check if the graph area is set.
                if (graphArea == null) return;
                
                // Store the mouse position.
                GraphAreaController.CurrentMousePosition = Event.current.mousePosition - graphArea.RenderRect.position;
                
                // Handle all the node events.
                foreach (NodeState nodeState in graphArea.NodeList) { NodeController.HandleEvents(node: nodeState); }
                
                // Check if the event happened in the graph's rect area.
                if (graphArea.RenderRect.Contains(point: Event.current.mousePosition)) {
                    // Handle the event's type.
                    switch (Event.current.type) {
                    // Check if the user clicked.
                    case EventType.MouseDown:
                        // If this is a left click.
                        if (Event.current.button == (int)MouseButton.LeftMouse) {
                            // Set the dragging flag.
                            graphArea.IsDragged = true;
                            
                            // Use the event.
                            Event.current.Use();
                        }
                        break;
                    // Check if the user released the mouse button.
                    case EventType.MouseUp:
                        // Clear the dragging flag.
                        graphArea.IsDragged = false;
                        
                        // Clear the manipulated socket.
                        SocketState.Manipulated = null;
                        
                        // Repaint the window.
                        graphArea.Owner.Repaint();
                        break;
                    // Check if the user made a right click.
                    case EventType.ContextClick:
                        // Store the mouse position.
                        GraphAreaController._msLastMousePosition = GraphAreaController.WindowToGraph(
                            graphArea: graphArea, position: Event.current.mousePosition, windowSpace: true
                        );
                        
                        // Open the create node menu.
                        GraphAreaController._CreateNodeMenu(graphArea: graphArea);
                        break;
                    // Check if the user dragged the screen.
                    case EventType.MouseDrag:
                        // If the area is being dragged.
                        if (graphArea.IsDragged) {
                            // Update the position of the area.
                            graphArea.Position += Event.current.delta;
                            
                            // Bind the position of the area.
                            GraphAreaController._BindToRenderArea(graphArea: graphArea);
                            
                            // Repaint the window.
                            graphArea.Owner.Repaint();
                            
                            // Use the event.
                            Event.current.Use();
                        // If there is a manipulated socket.
                        } else if (SocketState.Manipulated != null) {
                            // Repaint the window.
                            graphArea.Owner.Repaint();
                        }
                        break;
                    // Check if the user scrolled the wheel.
                    case EventType.ScrollWheel:
                        // Check if the control key is pressed.
                        if ((Event.current.modifiers & EventModifiers.Control) != 0) {
                            // Update the value of the scale.
                            graphArea.Ratio -= Mathf.Sign(f: Event.current.delta.y);
                            
                            // Clamp the scale value.
                            graphArea.Ratio = Mathf.Clamp(
                                value: graphArea.Ratio, 
                                min: GraphAreaState.RATIO_MIN, 
                                max: GraphAreaState.RATIO_MAX
                            );
                            
                            // Compute the position of the screen.
                            float x  = 1 - graphArea.ScrollPosition.x;
                            float gX = GraphAreaState.GRAPH_SIZE_X / 2f * graphArea.Scale;
                            float rX = graphArea.RenderRect.width;
                            float y  = 1 - graphArea.ScrollPosition.y;
                            float gY = GraphAreaState.GRAPH_SIZE_Y / 2f * graphArea.Scale;
                            float rY = graphArea.RenderRect.height;
                            graphArea.Position.Set(
                                newX: (2 * gX - rX) * x - gX + rX,
                                newY: (2 * gY - rY) * y - gY + rY
                            );
                            
                            // Use the event.
                            Event.current.Use();
                        } else {
                            // Update the position of the area.
                            graphArea.Position -= Event.current.delta * 5f;
                            
                            // Use the event.
                            Event.current.Use();
                        }
                            
                        // Bind the position of the area.
                        GraphAreaController._BindToRenderArea(graphArea: graphArea);
                        
                        // Repaint the window.
                        graphArea.Owner.Repaint();
                    
                        break;
                    }
                } else {
                    // If the user released the mouse.
                    if (Event.current.type == EventType.MouseUp) {
                        // Clear the dragging flag.
                        graphArea.IsDragged = false;
                        
                        // Clear the manipulated socket.
                        SocketState.Manipulated = null;
                            
                        // Repaint the window.
                        graphArea.Owner.Repaint();
                    }
                }
            }
            
            // - Seeking Methods -
            /// <summary>
            /// Finds the <see cref="GraphAreaState"/> that is related to the specified <see cref="SceneState"/>.
            /// </summary>
            /// <param name="sceneState">The scene state to find the <see cref="GraphAreaState"/> for.</param>
            /// <returns>The found <see cref="GraphAreaState"/> instance.</returns>
            [CanBeNull]
            public static GraphAreaState FindGraphArea([CanBeNull]SceneState sceneState) {
                // Check if the scene state is set.
                if (sceneState == null) return null;
                
                try {
                    // Find the graph area from the list of stores.
                    return GraphAreaStore.StoreList
                        ?.First(predicate: store =>  store.OwnerHash == sceneState.Owner.Hash)
                        .GraphList
                        ?.First(predicate: graph => graph.SceneIndex == sceneState.Index);
                } catch (InvalidOperationException) {
                    // Log the error.
                    Debug.LogWarning(message: $"Could not find a graph for the scene {sceneState.Identifier}");
                    
                    // Return a null.
                    return null;
                }
            }
            
            /// <summary>
            /// Checks if the specified <see cref="ActionController"/>'s type is represented in the area's nodes.
            /// </summary>
            /// <param name="graphArea">The graph area to search into.</param>
            /// <typeparam name="TActionType">The type of the searched action.</typeparam>
            /// <returns>True if the type exists in the area.</returns>
            public static bool HasActionType<TActionType>(GraphAreaState graphArea) where TActionType: ActionController {
                // Seek the action type in the scene's action list.
                return graphArea.Scene.ActionList.Any(predicate: actionState => actionState.ControllerType == typeof(TActionType));
            }
            
            // - Transformation Methods -
            /// <summary>
            /// Converts the point from the window coordinates to the graph coordinates.
            /// </summary>
            /// <param name="graphArea">The graph area to get a position relative to.</param>
            /// <param name="position">The position to convert, in world coordinates.</param>
            /// <param name="windowSpace">If true, returns the position relative to the window.</param>
            /// <returns>The position of the point in graph coordinates.</returns>
            public static Vector2 WindowToGraph([NotNull]GraphAreaState graphArea, Vector2 position, bool windowSpace = true) {
                // Compute the value.
                Vector2 output = new Vector2(
                    x: position.x - graphArea.Position.x,
                    y: position.y - graphArea.Position.y
                );
                
                // If we need the window space.
                if (windowSpace) {
                    // Remove the renderer's position.
                    output.x -= graphArea.RenderRect.x;
                    output.y -= graphArea.RenderRect.y;
                }
                
                // Apply the cell offset.
                output.x = Mathf.Floor(f: output.x / (GraphAreaState.CELL_SIZE * graphArea.Scale));
                output.y = Mathf.Floor(f: output.y / (GraphAreaState.CELL_SIZE * graphArea.Scale));
                
                // Return the value.
                return output;
            }

            /// <summary>
            /// Converts the point from the graph coordinates to the window coordinates.
            /// </summary>
            /// <param name="graphArea">The graph area to get a position relative to.</param>
            /// <param name="position">The position to convert, in graph coordinates.</param>
            /// <param name="windowSpace">If true, returns the position relative to the window.</param>
            /// <returns>The position of the point in window coordinates.</returns>
            public static Vector2 GraphToWindow([NotNull]GraphAreaState graphArea, Vector2 position, bool windowSpace = true) {
                // Compute the base vector.
                Vector2 output = new Vector2(
                    x: position.x * GraphAreaState.CELL_SIZE * graphArea.Scale + graphArea.Position.x,
                    y: position.y * GraphAreaState.CELL_SIZE * graphArea.Scale + graphArea.Position.y
                );
                
                // If we need the window space.
                if (windowSpace) {
                    // Add the render rect's position.
                    output.x += graphArea.RenderRect.x;
                    output.y += graphArea.RenderRect.y;
                }
                
                // Return the value.
                return output;
            }
            
            /// <summary>
            /// Centers the graph area in the window.
            /// </summary>
            /// <param name="graphArea">The graph area to center.</param>
            public static void Center([NotNull]GraphAreaState graphArea) {
                // Find the center of the window.
                graphArea.Position = graphArea.RenderRect.size / 2f;
                
                // Bind the position of the render area.
                GraphAreaController._BindToRenderArea(graphArea: graphArea);
            }
            
        // -- Private Methods --
            /// <summary>
            /// Creates a new <see cref="GraphAreaStore"/> instance.
            /// Stores it in the Resources/Stores folder.
            /// </summary>
            /// <param name="for">The act that owns the new store.</param>
            /// <returns>The newly created store.</returns>
            private static GraphAreaStore _CreateStore(ActState @for) {
                // Create a new store instance.
                GraphAreaStore store = ScriptableObject.CreateInstance<GraphAreaStore>();
                
                // Set the name of the store.
                store.name = $"{@for.Identifier}_Store"; 
                
                // Store the hash of the act store's owner.
                store.OwnerHash = @for.Hash;
                
                // Store its act owner.
                store.Owner = @for;
                
                // Create its object list.
                store.GraphList = new List<GraphAreaState>();
                
                // Ensure that the folder exists.
                if (!AssetDatabase.IsValidFolder(path: "Assets/Resources/Stores")) {
                    AssetDatabase.CreateFolder(parentFolder: "Assets/Resources", newFolderName: "Stores");
                }
                
                // Store it on disk.
                AssetDatabase.CreateAsset(
                    asset: store, 
                    path: $"Assets/Resources/Stores/DATA_GraphArea_{store.name}.asset"
                );
                
                // Validate the scenes of the store.
                GraphAreaController._ValidateStore(store: store);
                
                // Save the asset.
                AssetDatabase.SaveAssets();
                
                // Return the store.
                return store;
            }
            
            /// <summary>
            /// Validates the GraphArea of the specified store.
            /// If a graph area belongs to a non-existing 
            /// </summary>
            /// <param name="store"></param>
            private static void _ValidateStore([NotNull]GraphAreaStore store) {
                // Create a list of all the graphs.
                List<GraphAreaState> storeStates = new List<GraphAreaState>(collection: store.GraphList);
                // Clear the source list.
                store.GraphList = new List<GraphAreaState>();
                
                // Loop through the scenes of the store's act.
                for (int sceneIndex = 0; sceneIndex < store.Owner.SceneList.Length; sceneIndex++) {
                    // Check if there is a GraphArea for the scene.
                    if (storeStates.Find(match: graph => graph.SceneIndex == sceneIndex) is GraphAreaState sceneGraph) {
                        // Add the store the the valid list.
                        store.GraphList.Add(item: sceneGraph);
                    }
                    else {
                        // Create a new graph area object.
                        store.GraphList.Add(item: new GraphAreaState {
                            SceneIndex = sceneIndex
                        });
                    }
                }
            }
            
            /// <summary>
            /// Ensures that the <see cref="graphArea"/> object is bound
            /// within its <see cref="GraphAreaState.RenderRect"/>.
            /// </summary>
            /// <param name="graphArea">The area to bind.</param>
            private static void _BindToRenderArea([NotNull]GraphAreaState graphArea) {
                // Get the position of the top left corner.
                Vector2 topLeft = GraphAreaController.GraphToWindow(
                    graphArea: graphArea, 
                    position: new Vector2(
                        x: Mathf.Floor(f: -GraphAreaState.CELL_COUNT_X / 2f), 
                        y: Mathf.Floor(f: -GraphAreaState.CELL_COUNT_Y / 2f) 
                    )
                );
                // Get the position of the bottom right corner.
                Vector2 bottomRight = GraphAreaController.GraphToWindow(
                    graphArea: graphArea, 
                    position: new Vector2(
                        x: Mathf.Ceil(f: +GraphAreaState.CELL_COUNT_X / 2f), 
                        y: Mathf.Ceil(f: +GraphAreaState.CELL_COUNT_Y / 2f) 
                    )
                );
                
                // Check the position of the area.
                if (bottomRight.x < graphArea.RenderRect.x + graphArea.RenderRect.width) {
                    graphArea.Position.x = graphArea.RenderRect.width - GraphAreaState.GRAPH_SIZE_X / 2f * graphArea.Scale;
                }
                if (bottomRight.y < graphArea.RenderRect.y + graphArea.RenderRect.height) {
                    graphArea.Position.y = graphArea.RenderRect.height - GraphAreaState.GRAPH_SIZE_Y / 2f * graphArea.Scale;
                }
                if (topLeft.x >= graphArea.RenderRect.x) {
                    graphArea.Position.x = GraphAreaState.GRAPH_SIZE_X / 2f * graphArea.Scale;
                }
                if (topLeft.y >= graphArea.RenderRect.y) {
                    graphArea.Position.y = GraphAreaState.GRAPH_SIZE_Y / 2f * graphArea.Scale;
                }
                
                // Compute the position of the scrollbar.
                float x  = graphArea.Position.x;
                float mX = GraphAreaState.GRAPH_SIZE_X / 2f * graphArea.Scale;
                float nX = graphArea.RenderRect.width - mX;
                float y  = graphArea.Position.y;
                float mY = GraphAreaState.GRAPH_SIZE_Y / 2f * graphArea.Scale;
                float nY = graphArea.RenderRect.height - mY;
                graphArea.ScrollPosition.Set(
                    newX: 1 - (x - nX) / (mX - nX),
                    newY: 1 - (y - nY) / (mY - nY)
                );
            }
            
            /// <summary>
            /// Shows the menu that allows creation of a new node.
            /// </summary>
            private static void _CreateNodeMenu(GraphAreaState graphArea) {
                // Create a new menu.
                GenericMenu menu = new GenericMenu();
                
                // Create the miscellaneous options.
                menu.AddItem(
                    content: new GUIContent{ text = "Center" }, 
                    on: false, 
                    func: () => GraphAreaController.Center(graphArea: graphArea)
                );
                menu.AddSeparator(path: "");
                
                // Create the creation options.
                menu.AddSeparator(path: "Create Node/- Scene Actions");
                if (GraphAreaController.HasActionType<StartAction>(graphArea: graphArea)) {
                    menu.AddDisabledItem(content: new GUIContent{ text = "Create Node/Start" }, on: true);
                } else {
                    menu.AddItem(
                        content: new GUIContent{ text = "Create Node/Start" }, 
                        on: false,
                        func: () => NodeController.CreateNode<StartAction>(
                            owner: graphArea, position: GraphAreaController._msLastMousePosition
                        )
                    );
                }
                if (GraphAreaController.HasActionType<EndAction>(graphArea: graphArea)) {
                    menu.AddDisabledItem(content: new GUIContent{ text = "Create Node/End" }, on: true);
                } else {
                    menu.AddItem(
                        content: new GUIContent{ text = "Create Node/End" }, 
                        on: false,
                        func: () => NodeController.CreateNode<EndAction>(
                            owner: graphArea, position: GraphAreaController._msLastMousePosition
                        )
                    );
                }
                menu.AddItem(
                    content: new GUIContent{ text = "Create Node/Delay"}, 
                    on: false, 
                    func: () => NodeController.CreateNode<DelayAction>(
                        owner: graphArea, position: GraphAreaController._msLastMousePosition
                    )
                );
                menu.AddSeparator(path: "Create Node/");
                menu.AddSeparator(path: "Create Node/- Actor Actions");
                menu.AddItem(
                    content: new GUIContent{ text = "Create Node/Visible"}, 
                    on: false, 
                    func: () => NodeController.CreateNode<VisibleAction>(
                        owner: graphArea, position: GraphAreaController._msLastMousePosition
                    )
                );
                
                // Show the menu.
                menu.ShowAsContext();
            }
    // --- /Methods ---
}
}