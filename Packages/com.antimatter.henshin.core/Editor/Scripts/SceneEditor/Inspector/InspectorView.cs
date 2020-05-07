// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */

using System.Collections.Generic;
using Henshin.Editor.SceneEditor.GraphArea;
using Henshin.Editor.SceneEditor.Inspector.Editor;
using Henshin.Editor.Skin;
using Henshin.Runtime.Actor;
using Henshin.Runtime.Directions.Scene;
using UnityEditor;
using UnityEngine;

namespace Henshin.Editor.SceneEditor.Inspector {

/// <summary>
/// Static class that is used to render the <see cref="InspectorState"/> objects.
/// </summary>
public static class InspectorView {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Renders the specified <see cref="InspectorState"/> object.
            /// </summary>
            /// <param name="inspector">The inspector object to render.</param>
            public static void Render(InspectorState inspector) {
                // Start a new GUI area.
                GUILayout.BeginArea(screenRect: inspector.Rect);
                
                // Draw the scene editor section.
                InspectorView._DrawSceneSection(inspector: inspector);
                
                // Draw the action editor section.
                InspectorView._DrawActionSection(inspector: inspector);
                
                // End the inspector area.
                GUILayout.EndArea();
            }
            
        // -- Private Methods --
            // - Renderers -
            /// <summary>
            /// Draws the scene section of the inspector.
            /// </summary>
            private static void _DrawSceneSection(InspectorState inspector) {
                // Start a new gui area.
                GUILayout.BeginArea(screenRect: inspector.SceneRect);
                
                // Draw the title of the area.
                GUILayout.Label(
                    content: SkinState.Contents.SceneInspectorSceneSectionTitle, 
                    style: SkinState.Styles.SceneInspectorTitle
                );
                
                // Get a reference to the scene.
                SceneState current = inspector.Owner.Header.EditedScene;
                
                // If the scene is set.
                if (current != null) {
                    // Draw the scene's name.
                    GUILayout.Label(text: current.Identifier, style: SkinState.Styles.SceneInspectorSubtitle);
                    
                    // Allow updating the scene's index.
                    int newIndex = EditorGUILayout.IntField(
                        label: "Scene index: ",
                        value: current.Index + 1
                    ) - 1;
                    
                    // Check if the index has changed.
                    if (newIndex != current.Index) {
                        // Get the list of all the act's graphs.
                        List<GraphAreaState> others = GraphAreaController.FindGraphAreaStore(actState: current.Owner)?.GraphList;
                        if (others != null) {
                            // Get the current graph object.
                            GraphAreaState currentGraph = others[index: current.Index];
                            
                            // Insert the current graph at its new location.
                            others.Remove(item: currentGraph);
                            others.Insert(index: newIndex, item: currentGraph);
                            // Loop through the incremented graphs.
                            for (int index = 0; index < others.Count; index++) {
                                others[index: index].SceneIndex = index;
                            }
                            
                            // Insert the scene at its new location.
                            current.Owner.SceneList.Remove(item: current);
                            current.Owner.SceneList.Insert(index: newIndex, item: current);
                            // Update all the scene's indices.
                            for (int i = 0; i < current.Owner.SceneList.Count; i++) {
                                current.Owner.SceneList[index: i].Index = i;
                            }
                            
                            // Update the current scene's index.
                            inspector.Owner.Header.EditedSceneIndex = newIndex;
                        }
                        
                        
                        // Reload the graph area.
                        inspector.Owner.Instance.UpdateGraphArea(
                            graphArea: GraphAreaController.FindGraphArea(sceneState: inspector.Owner.Header.EditedScene)
                        );
                    }
                    
                    // Draw the scene's background.
                    current.Background = EditorGUILayout.ObjectField(
                        label: "Background sprite",
                        obj: current.Background,
                        objType: typeof(Sprite),
                        allowSceneObjects: false
                    ) as Sprite;
                    
                    // Draw a foldout for the actor's list.
                    inspector.IsActorListVisible = EditorGUILayout.Foldout(
                        foldout: inspector.IsActorListVisible,
                        content: "Actor List"
                    );
                    if (inspector.IsActorListVisible) {
                        // Start a new scrollable area.
                        inspector.ActorListPosition = GUILayout.BeginScrollView(
                            scrollPosition: inspector.ActorListPosition,
                            alwaysShowHorizontal: false,
                            alwaysShowVertical: true
                        );
                        
                        // Draw the list of all the actors in the scene.
                        for (int index = 0; index < current.ActorList.Count; index++) {
                            // Start a new horizontal area.
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(pixels: 4);

                            // Draw the actor's foldout.
                            bool isShown = EditorGUILayout.Foldout(
                                content: current.ActorList[index: index].Identifier,
                                foldout: index == inspector.ShownActorIndex
                            );
                            // Draw the delete button.
                            if (GUILayout.Button(
                                image: SkinState.Textures.Delete, 
                                style: SkinState.Styles.DeleteMini
                            )) {
                                // Remove the actor from the list.
                                current.ActorList.RemoveAt(index: index);
                            }
                            
                            // Set the index if the actors is shown.
                            if (isShown) {
                                inspector.ShownActorIndex = index;
                            }
                            
                            // End the horizontal area.
                            GUILayout.EndHorizontal();
                            
                            // Check if the actor is drawn.
                            if (index == inspector.ShownActorIndex) {
                                // If the actor was hidden.
                                if (!isShown) {
                                    inspector.ShownActorIndex = -1;
                                } else {
                                    // Begin a vertical area.
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Space(pixels: 12);
                                    GUILayout.BeginVertical();
                                    
                                    // Get the actor object.
                                    ActorState actor = current.ActorList[index: index];
                                    
                                    // Allow edition of the actor's identifier.
                                    actor.Identifier = EditorGUILayout.TextField(label: "Identifier", text: actor.Identifier);
                                    // Allow edition of the actor's prefab.
                                    actor.Prefab = EditorGUILayout.ObjectField(
                                        label: "Prefab",
                                        obj: actor.Prefab,
                                        objType: typeof(GameObject),
                                        allowSceneObjects: false
                                    ) as GameObject;
                                    // Allow edition of the actor's pose store.
                                    actor.PoseStore = EditorGUILayout.ObjectField(
                                        label: "Poses",
                                        obj: actor.PoseStore,
                                        objType: typeof(PoseStore),
                                        allowSceneObjects: false
                                    ) as PoseStore;
                                    
                                    // End the vertical area.
                                    GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();
                                }
                            }
                        }

                        // End the scrollable area.
                        GUILayout.EndScrollView();
                        
                        // Draw the add button.
                        if (GUILayout.Button(text: "Create new Actor")) {
                            // Create a new actor in the list.
                            current.ActorList.Add(item: new ActorState{ Identifier = $"Actor {current.ActorList.Count}" });
                        }
                        
                        // Add a bottom margin.
                        GUILayout.Space(pixels: 4);
                    }
                } else {
                    // Show an error message.
                    EditorGUILayout.HelpBox(message: "There is no edited scene.", type: MessageType.Info);
                }
                
                // End the scene area.
                GUILayout.EndArea();
            }
            
            /// <summary>
            /// Draws the action section of the inspector.
            /// </summary>
            private static void _DrawActionSection(InspectorState inspector) {
                // Start a new gui area.
                GUILayout.BeginArea(screenRect: inspector.ActionRect);
                
                // Draw the title of the area.
                GUILayout.Label(
                    content: SkinState.Contents.SceneInspectorActionSectionTitle, 
                    style: SkinState.Styles.SceneInspectorTitle
                );
                
                // Check if there is a selected action.
                if (inspector.EditedNode != null) {
                    // Render the name of the action.
                    GUILayout.Label(
                        text: inspector.EditedNode.Action?.ControllerType.Name.Replace(oldValue: "Action", newValue: ""),
                        style: SkinState.Styles.SceneInspectorSubtitle
                    );
                    // Render the current action.
                    Base.Render(action: inspector.EditedNode.Action, inspector: inspector);
                }
                
                // End the scene area.
                GUILayout.EndArea();
            }
    // --- /Methods ---
}
}