// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor {

/// <summary>
/// State class holding the info about the specified scene editor window.
/// 
/// </summary>
[System.SerializableAttribute]
public class SceneEditorState {
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - Identifiers -
            /// <summary>
            /// Stores the index of the current window.
            /// This is used to differentiate between multiple window instances.
            /// </summary>
            public int Index;
            
            // - Components -
            /// <summary>
            /// Reference to the window's header state.
            /// </summary>
            public Header.HeaderState Header = new Header.HeaderState();
            
        // -- Public Attributes --
            // - References -
            /// <summary>
            /// Reference to the owner's instance.
            /// </summary>
            public SceneEditorController Instance => SceneEditorState.WindowList[index: this.Index];
            
            // - Rect -
            /// <summary>
            /// Rect used to represent the entire window's area.
            /// This is used in the view classes to render in their proper areas.
            /// </summary>
            [System.NonSerializedAttribute]
            public UnityEngine.Rect WindowRect = new UnityEngine.Rect();
            
            // - Static References -
            /// <summary>
            /// List of all the window that are currently opened in the editor.
            /// This list is updated every time a window is opened or closed.
            /// </summary>
            public static System.Collections.Generic.List<SceneEditorController> WindowList =
                new System.Collections.Generic.List<SceneEditorController>();
        // -- Private Attributes --
    // --- /Attributes ---
}
}