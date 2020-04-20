// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.Controller.SceneEditor {

/// <summary>
/// 
/// </summary>
public static class Inspector {
    // ---  Attributes ---
        // -- Serialized Attributes --
        // -- Public Attributes --
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>Initialize the inspector controller.</summary>
            public static void Initialize() {}
            
            /// <summary>
            /// Updates the state of the inspector.
            /// Called right before the view's Render method.
            /// </summary>
            /// <param name="container">The container's rect</param>
            public static void BeforeRender(Rect container) {
                // Update the rect of the inspector.
                State.SceneEditor.Inspector.Rect.Set(
                    x:      State.SceneEditor.Inspector.RATIO.x      * container.width,
                    y:      State.SceneEditor.Inspector.RATIO.y      * container.height,
                    width:  State.SceneEditor.Inspector.RATIO.width  * container.width,
                    height: State.SceneEditor.Inspector.RATIO.height * container.height
                );
            }
        // -- Private Methods --
    // --- /Methods ---
}
}