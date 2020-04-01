/*
 * Copyright © 2020 - Zimproov.
 */

using System;
using System.Collections.Generic;
using System.Linq;

using Henshin.Core.App;

using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


/* Wrap the class within the local namespace. */
namespace Henshin.Editor.App {

/// <summary>
/// Main editor window for the <see cref="Manager"/> class.
/// Edits <see cref="State"/> instances to make them usable by the developer.
/// </summary>
public class ManagerEditor: EditorWindow {
    // ---  Attributes ---
        // -- Public Attributes --
        // -- Protected Attributes --
        // -- Private Attributes --
            // - State Attributes -
            /// <summary>
            /// List of all the <see cref="State"/> objects found in the project.
            /// </summary>
            private State[] _mStates;
            
            /// <summary>
            /// Index of the <see cref="State"/> object currently edited.
            /// </summary>
            private int _mCurrentStateIndex;
            
            /// <summary>
            /// Helper property.
            /// Facilitates access to the current <see cref="State"/> instance.
            /// </summary>
            private State CurrentState => this._mStates[this._mCurrentStateIndex];
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
            /// <summary>
            /// Unity event triggered when the window gets opened.
            /// </summary>
            private void Awake() {
                // Reload the states of the window.
                this._ReloadStates();
            }
            
            /// <summary>
            /// Unity event fired when the GUI should be redrawn.
            /// </summary>
            private void OnGUI() {
                // Draw the header.
                this._DrawHeader();
                
                // Check if the indexed object is still valid.
                if (this.CurrentState != null) {
                    // Check if there are some states found.
                    if (this._mStates.Length > 0) {
                        // Draw the state selector.
                        this._DrawStateSelector();
                        
                        // Draw the contents of the state.
                        this._DrawStateContents();
                    } else {
                        // Draw the warning message.
                        this._DrawNoStateWarning();
                    }
                } else {
                    // Reload the assets.
                    this._ReloadStates();
                }
                
                // Add the create state button.
                EditorGUILayout.Space();
                this._DrawCreateStateButton();
            }
            
        // -- Public Methods --
        // -- Protected Methods --
        // -- Private Methods --
            // - State Management -
            /// <summary>
            /// Reloads all the <see cref="State"/> instances from their default folder.
            /// </summary>
            private void _ReloadStates() {
                // Load all the State instances in the AssetDatabase.
                string[] stateGuids = AssetDatabase.FindAssets(filter: $"t:{nameof(State)}");
                
                // If the array is empty.
                if (stateGuids.Length == 0) {
                    // Reset the array.
                    this._mStates = new State[] { };
                    
                    // Log a warning.
                    Debug.LogWarning(message: $"There is no State object in the Resources/{State.DEFAULT_PATH} folder");
                    
                    // Stop the method.
                    return;
                }
                
                // Load all the found assets.
                this._mStates = stateGuids
                    .Select(selector: stateGuid => 
                        AssetDatabase.LoadAssetAtPath<State>(assetPath: AssetDatabase.GUIDToAssetPath(guid: stateGuid)))
                    .ToArray();
                
                // Get the currently active state.
                for (int i = 0; i < this._mStates.Length; i++) {
                    State state = this._mStates[i];
                    
                    // If the state is active.
                    if (state.isAppState) {
                        // Store the index of the state.
                        this._mCurrentStateIndex = i;
                        
                        // Stop the loop.
                        break;
                    }
                }
                
                // Ensure that the index is still in the bounds of the array.
                if (this._mCurrentStateIndex >= this._mStates.Length) {
                    this._mCurrentStateIndex = this._mStates.Length - 1;
                }
            }
            
            // - Window Management -
            /// <summary>
            /// Method used to open the editor window.
            /// </summary>
            [MenuItem(itemName: "Henshin/App/Manager Editor")]
            private static void _OpenEditor() {
                ManagerEditor window;
                // Check if the window already exists.
                if (EditorWindow.HasOpenInstances<ManagerEditor>()) {
                    // Get the editor window.
                    window = EditorWindow.GetWindow<ManagerEditor>();
                    
                    // Take the focus.
                    window.Focus();
                } else {
                    // Create a new window.
                    window = EditorWindow.CreateWindow<ManagerEditor>();

                    // Update the window's title and icon.
                    window.titleContent = new GUIContent {
                        text = nameof(ManagerEditor),
                        tooltip = "Window used to edit the application's State instances.",
                        image = Resources.Load<Texture>(path: "Editor/Icons/ManagerEditor")
                    };
                }
                
                // Reload the window's states.
                window._ReloadStates();
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
                GUIStyle refreshStyle = EditorStyles.miniButton;
                refreshStyle.fixedWidth = refreshStyle.fixedHeight;
                refreshStyle.stretchHeight = false;
                refreshStyle.stretchWidth = false;
                refreshStyle.padding = new RectOffset { left = 2, top = 2, right = 2, bottom = 2 };
                
                if (GUILayout.Button(image: Resources.Load<Texture>(path: "Editor/Icons/Refresh"), style: refreshStyle)) {
                    // Reload the states.
                    this._ReloadStates();
                }
                
                EditorGUILayout.EndHorizontal();
                
                // Create the style for the line.
                GUIStyle lineStyle = new GUIStyle {
                    normal = {
                        background = EditorGUIUtility.whiteTexture
                    },
                    stretchWidth = true,
                    fixedHeight = 2,
                    margin = new RectOffset { top = 1, bottom = 8, left = 4, right = 4}
                };
                
                GUI.color = Color.grey;
                GUILayout.Box(text: "", style: lineStyle);
                GUI.color = Color.white;
            }
            
            /// <summary>
            /// Draws the dropdown selector used to choose which state is being altered.
            /// </summary>
            private void _DrawStateSelector() {
                // Start an horizontal space.
                EditorGUILayout.BeginHorizontal();
                
                // Draw the dropdown.
                int newState = EditorGUILayout.Popup(
                    selectedIndex: this._mCurrentStateIndex,
                    displayedOptions: this._mStates.Select(selector: state => state.identifier).ToArray()
                );
                
                // Update the state flag.
                this.CurrentState.isAppState = false;
                this._mCurrentStateIndex = newState;
                this.CurrentState.isAppState = true;
                
                // Draw the locate button.
                if (GUILayout.Button(image: Resources.Load<Texture2D>(path: "Editor/Icons/Pointer"), style: EditorStyles.miniButton)) {
                    // Select the asset.
                    EditorGUIUtility.PingObject(obj: this.CurrentState);
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
                SerializedObject state = new SerializedObject(obj: this.CurrentState);
                
                // Draw the identifier of the state.
                EditorGUILayout.PropertyField(property: state.FindProperty(propertyPath: nameof(State.identifier)));
                
                // Draw the scene field asset.
                try {
                    this._AssetField(
                        property: state.FindProperty(propertyPath: nameof(State.theatreScene)),
                        assetName: this.CurrentState.theatreScene,
                        assetType: typeof(SceneAsset),
                        baseFolder: "Scenes"
                    );
                } catch (AssetNotFoundException) {
                    Debug.LogWarning(message: $"Could not find a Theatre Scene named {this.CurrentState.theatreScene}.");
                } catch (MultipleAssetException) {
                    Debug.LogWarning(message: $"There are multiple Theatre Scenes named {this.CurrentState.theatreScene}.");
                }
                
                // Draw the prefab asset used by the state.
                EditorGUILayout.PropertyField(property: state.FindProperty(propertyPath: nameof(State.spectator)));
                
                // DEBUG: Draw the act.
                EditorGUILayout.PropertyField(property: state.FindProperty(propertyPath: nameof(State.debugAct)));
                
                // Save the changes to the state object.
                state.ApplyModifiedProperties();
            }
            
            /// <summary>
            /// Draws a button that creates a new <see cref="State"/> asset.
            /// </summary>
            private void _DrawCreateStateButton() {
                // Add the option to create a new state object.
                if (GUILayout.Button(text: "Create new State")) {
                    // Create a new State object.
                    State newState = ScriptableObject.CreateInstance<State>();
                    newState.identifier = $"State#{this._mStates.Length + 1}";
                    
                    // Ensure that the folder exists.
                    if (!AssetDatabase.IsValidFolder(path: $"Assets/Resources/{State.DEFAULT_PATH}")) {
                        // Create the Resources folder.
                        AssetDatabase.CreateFolder(parentFolder: "Assets", newFolderName: "Resources");
                        
                        // Create the default path for the state.
                        string currentParent = "Assets/Resources";
                        foreach (string folder in State.DEFAULT_PATH.Split('/')) {
                            // Create the folder.
                            AssetDatabase.CreateFolder(parentFolder: currentParent, newFolderName: folder);
                            
                            // Add the folder to the current parent path.
                            currentParent += $"/{folder}";
                        }
                    }
                    
                    // Serialize the state.
                    AssetDatabase.CreateAsset(asset: newState, path: $"Assets/Resources/{State.DEFAULT_PATH}/APP_STATE_{newState.identifier}.asset");
                    
                    // Reload the assets.
                    this._ReloadStates();
                }
            }
            
            // - Helper Methods -
            private IEnumerable<Object> LoadAssetsWithFilter(string filter, string baseFolder = null) {
                // Prepend the "Assets" folder to the base folder.
                baseFolder = "Assets" + (string.IsNullOrEmpty(value: baseFolder) ? null : $"/{baseFolder}");
                
                // Load the asset's GUIDs from the database.
                string[] assetGuid = AssetDatabase.FindAssets(
                    filter: filter, 
                    searchInFolders: new [] { 
                        baseFolder
                    }
                );
                
                // Check if a resource was found.
                if (assetGuid.Length > 0) {
                    // Load the assets.
                    return assetGuid.Select(selector: guid => AssetDatabase.LoadAssetAtPath<Object>(assetPath: AssetDatabase.GUIDToAssetPath(guid: guid)));
                } else {
                    // Throw an exception.
                    throw new AssetNotFoundException(message: $"Filter {filter} returned no assets within the {baseFolder} folder.");
                }
            }
            
            private string _CamelToPretty(string camel) {
                // If the string is null or less than 1 letter, just return it.
                if (camel == null) { return null; }
                if (camel.Length < 2) { return camel; }
                
                // Capitalize the first character.
                string result = camel.Substring(startIndex: 0, length: 1).ToUpper();
                
                // Loop through the string.
                for (int i = 1; i < camel.Length; i++) {
                    // If the character is an uppercase character.
                    if (char.IsUpper(c: camel[index: i])) {
                        // Add a white space.
                        result += ' ';
                    }
                    
                    // Add the char to the result.
                    result += camel[index: i];
                }
                
                // Return the result.
                return result;
            }
            
            private void _AssetField(SerializedProperty property, string assetName, Type assetType, string baseFolder = null) {
                // Reference used for the property rendering.
                Object asset = null;
                Exception error = null;
                
                // Check if the asset name is set.
                if (!string.IsNullOrEmpty(value: assetName)) {
                    try {
                        // Load the assets.
                        Object[] assets = this.LoadAssetsWithFilter(filter: $"{assetName} t:{assetType.Name}", baseFolder: baseFolder).ToArray();
                        
                        // Check if there is only one asset.
                        if (assets.Length == 1) {
                            // Load the asset.
                            asset = assets[0];
                        } else {
                            // Throw an exception.
                            error = new MultipleAssetException(message:  "Tried to render an asset field with multiple corresponding assets.\n" +
                                                                        $"(Name:{assetName}, Type:{assetType.FullName})");
                        }
                    } catch (AssetNotFoundException) {
                        // Throw an exception.
                        error = new AssetNotFoundException(message:  "Failed to find an asset when rendering an asset field.\n" +
                                                                    $"(Name:{assetName}, Type:{assetType.FullName})");
                    }
                }
                
                // Render the field.
                asset = EditorGUILayout.ObjectField(label: this._CamelToPretty(camel: property.name), obj: asset, objType: assetType, allowSceneObjects: false);
                
                // Update the property.
                switch (property.propertyType) {
                    case SerializedPropertyType.String:
                        property.stringValue = asset ? asset.name : null;
                        break; 
                    case SerializedPropertyType.ObjectReference:
                        property.objectReferenceValue = asset;
                        break;
                default:
                    throw new ArgumentOutOfRangeException(paramName: nameof(property), message: $"Tried to draw an asset field with incompatible type: {property.propertyType}");
                }
                
                // Throw the error, if needed.
                if (error != null) {
                    throw error;
                }
            }
    // --- /Methods ---
}
}