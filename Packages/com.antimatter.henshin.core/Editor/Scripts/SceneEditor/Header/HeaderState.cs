// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.Header {

/// <summary>
/// State class used to represent the header of a <see cref="SceneEditorController"/> window.
/// Stores all the necessary parameters within itself.
/// </summary>
[System.SerializableAttribute]
public class HeaderState {
    // ---  Attributes ---
        // -- Serialized Attributes --
        // -- Public Attributes --
            // - References -
            /// <summary>
            /// Reference to the owner of this header instance.
            /// </summary>
            [System.NonSerializedAttribute]
            public SceneEditorState Owner;
            
            // - Rect -
            /// <summary>
            /// Stores the rect of the header.
            /// </summary>
            [System.NonSerializedAttribute]
            public UnityEngine.Rect Rect = new UnityEngine.Rect();
    // --- /Attributes ---
}
}