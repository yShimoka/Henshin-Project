/*
 * Copyright © 2020 - Zimproov.
 */

using System;
using System.Collections.Generic;
using System.Linq;

using Henshin.Core.App;
using Henshin.Core.Scene.Directions;
using Henshin.Editor.Misc;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using static Henshin.Core.Scene.Directions.Act;
using Object = UnityEngine.Object;


/* Wrap the class within the local namespace. */
namespace Henshin.Editor.App {

/// <summary>
/// Main editor window for the <see cref="Manager"/> class.
/// Edits <see cref="State"/> instances to make them usable by the developer.
/// </summary>
public class ManagerEditorWindow: EditorWindow {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Helper property.
            /// Facilitates access to the current <see cref="State"/> instance.
            /// </summary>
            public static State CurrentState => ManagerEditorState.CurrentState;

        // -- Private Attributes --
            // - State Attributes -
            /// <summary>Flag set if the act foldout is opened.</summary>
            private bool _mActFoldout;
            
            // - Reusable Assets -
            /// <summary><see cref="GUIContent"/> used for the header's refresh button.</summary>
            private static GUIContent _msRefreshContent;
             
            /// <summary><see cref="GUIContent"/> used for the state's locate button.</summary>
            private static GUIContent _msLocateState;
             
            /// <summary><see cref="GUIContent"/> used for the remove act button.</summary>
            private static GUIContent _msDeleteAct; 
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
            /// <summary>Unity event fired the first time that the window is created.</summary>
            private void OnEnable() {
                // Generate the gui content objects.
                ManagerEditorWindow._msRefreshContent  = new GUIContent { tooltip = "Refresh the available state list.", image = TextureStore.Refresh };
                ManagerEditorWindow._msLocateState = new GUIContent { tooltip = "Find the State in the project view.", image = TextureStore.Pointer };
                ManagerEditorWindow._msDeleteAct = new GUIContent { tooltip = "Remove this act from the list.", image = TextureStore.Delete };
            }
            
            /// <summary>Unity event fired when the GUI should be redrawn.</summary>
            private void OnGUI() {
                // Draw the header.
                this._DrawHeader();
                
                // Check if the indexed object is still valid.
                if (ManagerEditorWindow.CurrentState != null) {
                    // Draw the state selector.
                    this._DrawStateSelector();
                    
                    // Draw the contents of the state.
                    this._DrawStateContents();
                } else {
                    // Draw the warning message.
                    this._DrawNoStateWarning();
                }
                
                // Add the create state button.
                EditorGUILayout.Space();
                this._DrawCreateStateButton();
            }
            
        // -- Public Methods --
        // -- Private Methods --
            // - Window Management -
            /// <summary>
            /// Method used to open the editor window.
            /// </summary>
            [MenuItem(itemName: "Henshin/Manager Editor")]
            private static void _OpenEditor() {
                // Check if the window already exists.
                if (EditorWindow.HasOpenInstances<ManagerEditorWindow>()) {
                    // Get the editor window.
                    EditorWindow.GetWindow<ManagerEditorWindow>(title: nameof(ManagerEditorWindow), focus: true);
                } else {
                    // Create a new window.
                    ManagerEditorWindow window = EditorWindow.CreateWindow<ManagerEditorWindow>();

                    // Update the window's title and icon.
                    window.titleContent = new GUIContent {
                        text = nameof(ManagerEditorWindow),
                        tooltip = "Window used to edit the application's State instances.",
                        image = TextureStore.ManagerEditor
                    };
                }
                
                // Reload the states.
                ManagerEditorState.ForceReloadStates();
            }
            
            // - Window Content -
            /// <summary>
            /// Draws the header of the window.
            /// This includes its title and the refresh button.
            /// </summary>
            private void _DrawHeader() {
                EditorGUILayout.BeginHorizontal();
                
                // Draw the title.
                EditorGUILayout.LabelField(label: "Manager Editor", style: EditorStyles.boldLabel);
                
                // Draw the refresh button.
                if (GUILayout.Button(content: ManagerEditorWindow._msRefreshContent, style: StyleStore.IconButton)) {
                    // Reload the states.
                    ManagerEditorState.ForceReloadStates();
                }
                
                EditorGUILayout.EndHorizontal();
            }
            
            /// <summary>
            /// Draws the dropdown selector used to choose which state is being altered.
            /// </summary>
            private void _DrawStateSelector() {
                // Start an horizontal space.
                EditorGUILayout.BeginHorizontal();
                
                // Draw the dropdown.
                ManagerEditorState.CurrentStateIndex = EditorGUILayout.Popup(
                    selectedIndex: ManagerEditorState.CurrentStateIndex,
                    displayedOptions: ManagerEditorState.States.Select(selector: state => state.identifier).ToArray()
                );
                
                // Draw the locate button.
                if (GUILayout.Button(content: ManagerEditorWindow._msLocateState, style: StyleStore.IconButton)) {
                    // Select the asset.
                    EditorGUIUtility.PingObject(obj: ManagerEditorWindow.CurrentState);
                }
                
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }
            
            /// <summary>
            /// Draws a simple warning that there are no State objects in the project.
            /// </summary>
            private void _DrawNoStateWarning() {
                // Draw a message box.
                EditorGUILayout.HelpBox(
                    message: "There is no State object in the project.",
                    type: MessageType.Warning
                );
            }
            
            /// <summary>
            /// Draws the properties of the state.
            /// </summary>
            private void _DrawStateContents() {
                // Get the serialized representation of the state.
                SerializedObject state = new SerializedObject(obj: ManagerEditorWindow.CurrentState);
                
                // Draw the identifier of the state.
                EditorGUILayout.PropertyField(property: state.FindProperty(propertyPath: nameof(State.identifier)));
                
                // Draw the scene field asset.
                EditorGUIHelper.AssetField<SceneAsset>(property: state.FindProperty(propertyPath: nameof(State.theatreScene)), "Scenes");
                
                // Draw the prefab asset used by the state.
                EditorGUILayout.PropertyField(property: state.FindProperty(propertyPath: nameof(State.spectator)));
                
                // Draw the act foldout.
                this._mActFoldout = EditorGUILayout.Foldout(foldout: this._mActFoldout, content: "Acts");
                
                // If the foldout is opened.
                if (this._mActFoldout) {
                    // Loop through all the acts.
                    for (int actIndex = 0; actIndex < ManagerEditorWindow.CurrentState.acts.Count; actIndex++) {
                        EditorGUILayout.BeginHorizontal();
                        
                        // Get a reference to the act.
                        Act current = ManagerEditorWindow.CurrentState.acts[index: actIndex];
                        
                        // If the current object is valid.
                        if (current != null) {
                            // Draw the act's index.
                            int newIndex = EditorGUILayout.IntField(value: actIndex, style: StyleStore.SmallNumberField, GUILayout.Width(width: StyleStore.SmallNumberField.fixedWidth));
                            
                            // If the index was altered.
                            if (newIndex != actIndex) {
                                // Update the position of the act in the array.
                                ManagerEditorWindow.CurrentState.acts.Remove(item: current);
                                ManagerEditorWindow.CurrentState.acts.Insert(index: newIndex, item: current);
                            }
                            
                            // Add a small space in front of the label.
                            GUILayout.Space(pixels: 8);
                            // Draw the act name.
                            GUILayout.Label(text: current.identifier);
                            
                            // Draw the delete button.
                            if (GUILayout.Button(content: ManagerEditorWindow._msDeleteAct, style: StyleStore.IconButton)) {
                                ManagerEditorWindow.CurrentState.acts.RemoveAt(index: actIndex);
                            }
                        } else {
                            // Remove the act from the list.
                            ManagerEditorWindow.CurrentState.acts.RemoveAt(index: actIndex);
                            
                            // Redraw the current item.
                            actIndex--;
                        }
                        
                        EditorGUILayout.EndHorizontal();
                    }
                    
                    // Add a drop area for a new act.
                    Act newAct = EditorGUIHelper.AssetDropArea<Act>(fieldName: "Add act");
                    if (newAct != null) {
                        // Add the act to the list.
                        ManagerEditorWindow.CurrentState.acts.Add(item: newAct);
                    }
                    
                    // Add the create act button.
                    if (GUILayout.Button(text: "Create act")) {
                        // Create a new act.
                        ManagerEditorState.CreateAct();
                    }
                }
                
                // Apply the property edition.
                state.ApplyModifiedProperties();
            }
            
            /// <summary>
            /// Draws a button that creates a new <see cref="State"/> asset.
            /// </summary>
            private void _DrawCreateStateButton() {
                // Add the option to create a new state object.
                if (GUILayout.Button(text: "Create new State")) {
                    ManagerEditorState.CreateState();
                }
            }
    // --- /Methods ---
}
}