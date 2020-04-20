// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.Controller.SceneEditor {

/// <summary>
/// Static header controller class.
/// Manipulates the state of the scene editor's header.
/// </summary>
public static class Header {
    // ---  Attributes ---
        // -- Public Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>Initializes the state of the header.</summary>
            public static void Initialize() {
                // Clear the state's scene list.
                State.SceneEditor.Header.SceneList  = new List<Henshin.State.Directions.Scene>();
                
                // Load all the scene assets from the project.
                string[] sceneGuids = AssetDatabase.FindAssets(filter: $"t:{typeof(Henshin.State.Directions.Scene).FullName}", searchInFolders: new []{ "Assets/Resources/Serialized" });
                
                // Load all the scenes with the specified paths.
                foreach (string guid in sceneGuids) {
                    // Load the scene object.
                    Henshin.State.Directions.Scene scene = AssetDatabase.LoadAssetAtPath<Henshin.State.Directions.Scene>(
                        assetPath: AssetDatabase.GUIDToAssetPath(guid: guid)
                    );
                    
                    // Add it to the list.
                    State.SceneEditor.Header.SceneList .Add(item: scene);
                }
                
                // Get all the names of the scenes.
                State.SceneEditor.Header.SceneNames = State.SceneEditor.Header.SceneList.Select(selector: scene => scene.identifier).ToArray();
            }
            
            /// <summary>Updates the <see cref="State.SceneEditor.Header"/>'s rect element.</summary>
            /// <param name="container">The rect of the container.</param>
            public static void UpdateRect(Rect container) {
                // Store the container rect's values.
                State.SceneEditor.Header.ContainerRect.Set(x: container.x, y: container.y, width: container.width, height: container.width);
                
                // Compute the size of the header rect.
                State.SceneEditor.Header.Rect.Set(
                    x:         State.SceneEditor.Header.RATIO.x      * container.width,
                    y:         State.SceneEditor.Header.RATIO.y      * container.height,
                    width:     State.SceneEditor.Header.RATIO.width  * container.width,
                    height:    State.SceneEditor.Header.RATIO.height * container.height
                );
            }
        // -- Private Methods --
    // --- /Methods ---
}
}