// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System.Linq;
using UnityEngine;
using UnityEditor;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.View.SceneEditor {

/// <summary>
/// View class used to render the <see cref="Henshin.Editor.View.SceneEditor"/>'s inspector section.
/// Allows editing of the selected <see cref="Henshin.State.Directions.Scene"/> and <see cref="Henshin.State.Directions.Transformation"/>.
/// </summary>
public static class Inspector {
    // ---  Attributes ---
        // -- Public Attributes --
        // -- Private Attributes --
            // - Constants -
            /// <summary></summary>
            private const float _mBACKGROUND_GRAY_LEVEL = 0.7f;
            
            // - Textures -
            /// <summary>Texture used for the inspector's background.</summary>
            private static Texture2D _msBackgroundTexture;
            
            // - Contents -
            
            // - Styles -
            /// <summary>Style used for the inspector's background.</summary>
            private static GUIStyle _msBackgroundStyle;
            /// <summary>Style used for the inspector's container.</summary>
            private static GUIStyle _msInspectorContainer;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>Initialize the inspector view.</summary>
            public static void Initialize() {
                // Create all the required textures.
                Inspector._CreateTextures();
                // Create all the required GUIContent objects.
                Inspector._CreateContents();
                // Create all the required GUIStyle objects.
                Inspector._CreateStyles();
            }
            
            /// <summary>Renders the inspector window on the screen.</summary>
            public static void Render() {
                // Draw the inspector background.
                GUI.Box(position: State.SceneEditor.Inspector.Rect, content: GUIContent.none, style: Inspector._msBackgroundStyle);
                
                // Begin a new GUILayout area.
                GUILayout.BeginArea(screenRect: State.SceneEditor.Inspector.Rect, style: Inspector._msInspectorContainer);
                
                // Check if a node is selected.
                if (State.Graph.Node.CurrentNode != null) {
                    // Check the type of the node.
                    switch (State.Graph.Node.CurrentNode.Transformation.GetType().Name) {
                        case "Start":
                        case "End":
                            // Draw an info box.
                            EditorGUILayout.HelpBox(message: "This transformation has no parameters.", type: MessageType.None);
                            break;
                        case nameof(Henshin.State.Directions.Transformations.Scene.Delay):
                            // Print the delay duration value.
                            if (State.Graph.Node.CurrentNode.Transformation is Henshin.Controller.Directions.Transformations.Scene.Delay delay) {
                                delay.State.Time = EditorGUILayout.FloatField(label: "Delay time", value: delay.State.Time);
                            }
                            break;
                        case nameof(Henshin.State.Directions.Transformations.Actor.Scale):
                            if (State.Graph.Node.CurrentNode.Transformation is Henshin.Controller.Directions.Transformations.Actor.Scale scale) {
                                // Print the actor selector.
                                scale.State.actor = Inspector._DrawActorPopup();
                                
                                // Print the position.
                                scale.State.Target = EditorGUILayout.Vector2Field(label: "Target", value: scale.State.Target);
                            }
                            goto case nameof(Henshin.State.Directions.Transformations.Scene.Delay);
                        case nameof(Henshin.State.Directions.Transformations.Actor.MoveTo):
                            if (State.Graph.Node.CurrentNode.Transformation is Henshin.Controller.Directions.Transformations.Actor.MoveTo moveTo) {
                                // Print the actor selector.
                                moveTo.State.actor = Inspector._DrawActorPopup();
                                
                                // Print the position.
                                moveTo.State.Target = EditorGUILayout.Vector2Field(label: "Target", value: moveTo.State.Target);
                            }
                            goto case nameof(Henshin.State.Directions.Transformations.Scene.Delay);
                        case nameof(Henshin.State.Directions.Transformations.Actor.Colour):
                            if (State.Graph.Node.CurrentNode.Transformation is Henshin.Controller.Directions.Transformations.Actor.Colour colour) {
                                // Print the actor selector.
                                colour.State.actor = Inspector._DrawActorPopup();
                                
                                // Print the colour.
                                colour.State.Target = EditorGUILayout.ColorField(label: "Target", value: colour.State.Target);
                            }
                            goto case nameof(Henshin.State.Directions.Transformations.Scene.Delay);
                        case nameof(Henshin.State.Directions.Transformations.Actor.Visible):
                            if (State.Graph.Node.CurrentNode.Transformation is Henshin.Controller.Directions.Transformations.Actor.Visible active) {
                                // Print the all actors toggler.
                                active.State.AllActors = EditorGUILayout.Toggle(label: "All actors", value: active.State.AllActors);
                                
                                // Check the state of the all flag.
                                if (!active.State.AllActors) {
                                    // Print the actor selector.
                                    active.State.actor = Inspector._DrawActorPopup();
                                }
                                
                                // Print the active flag.
                                active.State.Activate = EditorGUILayout.Toggle(label: "Set active", value: active.State.Activate);
                            }
                            break;
                    default:
                            // Draw an error.
                            EditorGUILayout.HelpBox(message: $"Unsupported transformation type: {State.Graph.Node.CurrentNode.Transformation.GetType().Name}", type: MessageType.Error);
                            break;
                            
                    }
                } else {
                    // Draw a help box.
                    EditorGUILayout.HelpBox(message: "There is no selected transformation.", type: MessageType.Info);
                }
                
                // End the area.
                GUILayout.EndArea();
            }
            
        // -- Private Methods --
            /// <summary>Creates all the textures required for the inspector.</summary>
            private static void _CreateTextures() {
                // Create the background texture.
                Inspector._msBackgroundTexture = new Texture2D(width: 1, height: 1, textureFormat: TextureFormat.RGB24, mipChain: false);
                Inspector._msBackgroundTexture.SetPixel(
                    x: 0, y: 0, color: new Color(
                        r: Inspector._mBACKGROUND_GRAY_LEVEL, 
                        g: Inspector._mBACKGROUND_GRAY_LEVEL, 
                        b: Inspector._mBACKGROUND_GRAY_LEVEL 
                    )
                );
                Inspector._msBackgroundTexture.wrapMode = TextureWrapMode.Repeat;
                Inspector._msBackgroundTexture.filterMode = FilterMode.Point;
                Inspector._msBackgroundTexture.Apply();
            }
            
            /// <summary>Creates all the <see cref="GUIContent"/>s required for the inspector.</summary>
            private static void _CreateContents() {}
            
            /// <summary>Creates all the <see cref="GUIStyle"/>s required for the inspector.</summary>
            private static void _CreateStyles() {
                // Create the background style.
                Inspector._msBackgroundStyle = new GUIStyle {
                    normal = { background = Inspector._msBackgroundTexture },
                };
                // Create the inspector container style.
                Inspector._msInspectorContainer = new GUIStyle {
                    padding = { left = 8, top = 16, right = 8, bottom = 16 },
                };
            }
            
            /// <summary>
            /// Draws the actor popup for the specified scene.
            /// </summary>
            private static Henshin.State.Scenery.Actor _DrawActorPopup() {
                // Get the actor list.
                System.Collections.Generic.List<Henshin.State.Scenery.Actor> actors = State.SceneEditor.Header.CurrentScene.actors;
                // Get the node object.
                Henshin.Editor.State.Graph.Node node = State.Graph.Node.CurrentNode;
                
                // Draw the popup.
                int actorIndex = EditorGUILayout.Popup(
                    label: "Actor",
                    selectedIndex: node.Transformation.State.actor == null ? 0 : actors.IndexOf(item: node.Transformation.State.actor),
                    displayedOptions: actors.Select(selector: actor => actor.name).ToArray()
                );
                
                // Return the new actor.
                return actorIndex >= actors.Count ? null : actors[index: actorIndex];
            }
    // --- /Methods ---
}
}