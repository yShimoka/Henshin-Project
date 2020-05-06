// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */

using Henshin.Editor.SceneEditor.GraphArea;
using Henshin.Editor.SceneEditor.Header;
using Henshin.Editor.SceneEditor.Inspector;
using JetBrains.Annotations;
using UnityEngine;

namespace Henshin.Editor.SceneEditor {

/// <summary>
/// Controller class used to manipulate the scene editor window.
/// Receives all the window-related unity events.
/// </summary>
public class SceneEditorController: UnityEditor.EditorWindow {
    // ---  Attributes ---
        // -- Serializable Attributes --
            /// <summary>
            /// Reference to the state of this editor window.
            /// </summary>
            public SceneEditorState State = new SceneEditorState();
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
            /// <summary>
            /// Opens a new SceneEditor window.
            /// </summary>
            [UnityEditor.MenuItem(itemName: "Window/Henshin Scene Editor")]
            private static void OpenWindow() {
                // Open the editor window.
                SceneEditorController window = UnityEditor.EditorWindow.CreateWindow<SceneEditorController>(
                    desiredDockNextTo: typeof(UnityEditor.SceneView)
                );
                
                // Set the title of the editor window.
                window.titleContent = new GUIContent(
                    src: Skin.SkinState.Contents.SceneEditorTitle
                );
                
                // Refresh the window indices.
                SceneEditorController._RefreshWindowIndices();
            }

            /// <summary>
            /// Event called when the window gets created.
            /// </summary>
            private void OnEnable() {
                // Add the window to the list of all windows.
                SceneEditorState.WindowList.Add(item: this);
                
                // Refresh the window indices.
                SceneEditorController._RefreshWindowIndices();
            }

            /// <summary>
            /// Event called when the window gets closed.
            /// </summary>
            private void OnDisable() {
                // Remove the window from the list.
                SceneEditorState.WindowList.Remove(item: this);
                
                // Refresh the window indices.
                SceneEditorController._RefreshWindowIndices();
            }

            /// <summary>
            /// Event called every time the window should be drawn.
            /// </summary>
            private void OnGUI() {
                // If the window is not initialized.
                if (!this.State.IsInitialized) {
                    // Initialize the window.
                    this._Initialize();
                }
                
                // Prepare the controller.
                this._Prepare();
                
                // Call the view's Render method.
                SceneEditorView.Render(state: this.State);
            
                // Call the event handlers.
                this._HandleEvents();
            }

            /// <summary>
            /// Event called when an element of the project has changed.
            /// </summary>
            private void OnProjectChange() {
                // Reload the application states.
                HeaderController.ReloadApplicationStates();
            }
            
        // -- Public Methods --
            /// <summary>
            /// Updates the reference to the graph area.
            /// </summary>
            public void UpdateGraphArea([CanBeNull]GraphAreaState graphArea) {
                // Store the reference to the graph area.
                this.State.GraphArea = graphArea;
                
                // Check if the graph area is set.
                if (graphArea != null) {
                    // Set the graph area's window reference.
                    this.State.GraphArea.Owner = this;
                    // Initialize the graph area object.
                    GraphAreaController.Initialize(graphArea: this.State.GraphArea);
                }
            }
            
        // -- Private Methods --
            // - Events -
            /// <summary>
            /// Initializes the scene editor.
            /// Initializes all the components of the window.
            /// </summary>
            private void _Initialize() {
                // Initialize the header object.
                HeaderController.Initialize(header: this.State.Header, owner: this);
                // Initialize the inspector object.
                InspectorController.Initialize(inspector: this.State.Inspector, owner: this);
                
                // Set the flag.
                this.State.IsInitialized = true;
            }
            
            /// <summary>
            /// Prepares the scene editor.
            /// Calls the prepare method on all the components of the window.
            /// </summary>
            private void _Prepare() {
                // Prepare the scene editor's rect.
                this.State.WindowRect.Set(
                    x: this.position.x,
                    y: this.position.y,
                    width: this.position.width,
                    height: this.position.height
                );
                
                // Call the prepare method on the header.
                HeaderController.Prepare(header: this.State.Header);
                // Call the prepare method on the inspector.
                InspectorController.Prepare(inspector: this.State.Inspector);
                // Call the prepare method on the graph area.
                GraphAreaController.Prepare(graphArea: this.State.GraphArea, owner: this);
            }
            
            /// <summary>
            /// Prepares the scene editor.
            /// Calls the HandleEvent method on all the components of the window.
            /// </summary>
            private void _HandleEvents() {
                // Call the handle events method on the header.
                HeaderController.HandleEvents(header: this.State.Header);
                // Call the handle events method on the inspector.
                InspectorController.HandleEvents(inspector: this.State.Inspector);
                // Call the handle events method on the graph area.
                GraphAreaController.HandleEvents(graphArea: this.State.GraphArea);
            }
            
            // - Helpers -
            /// <summary>
            /// Refreshes the indices of all the windows.
            /// </summary>
            private static void _RefreshWindowIndices() {
                // Loop through all the windows.
                for (int index = 0; index < SceneEditorState.WindowList.Count; index++) {
                    // Get the window instance.
                    SceneEditorController window = SceneEditorState.WindowList[index: index];
                    
                    // Update the index of the window.
                    window.State.Index = index;
                    
                    // Update the title of the window.
                    window.titleContent.text = 
                        Skin.SkinState.Contents.SceneEditorTitle.text 
                        + $" #{index + 1}";
                }
            }
    // --- /Methods ---
}
}