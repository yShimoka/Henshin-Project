// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Collections.Generic;
using Henshin.Editor.SceneEditor.GraphArea;
using Henshin.Editor.SceneEditor.Header;
using Henshin.Editor.SceneEditor.Inspector;
using JetBrains.Annotations;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor {

/// <summary>
/// State class holding the info about the specified scene editor window.
/// 
/// </summary>
[SerializableAttribute]
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
            public HeaderState Header = new HeaderState();
            
        // -- Public Attributes --
            // - References -
            /// <summary>
            /// Reference to the owner's instance.
            /// </summary>
            public SceneEditorController Instance => SceneEditorState.WindowList[index: this.Index];
            
            /// <summary>
            /// Reference to the window's GraphArea object.
            /// </summary>
            [NonSerialized, CanBeNull]
            public GraphAreaState GraphArea;
            
            /// <summary>
            /// Reference to the window's InspectorState object.
            /// </summary>
            [NonSerialized]
            public InspectorState Inspector = new InspectorState();
            
            // - Flags -
            /// <summary>
            /// Flag set if the window is initialized.
            /// </summary>
            [NonSerialized]
            public bool IsInitialized = false;
            
            // - Rect -
            /// <summary>
            /// Rect used to represent the entire window's area.
            /// This is used in the view classes to render in their proper areas.
            /// </summary>
            [NonSerialized]
            public Rect WindowRect = new Rect();
            
            // - Static References -
            /// <summary>
            /// List of all the window that are currently opened in the editor.
            /// This list is updated every time a window is opened or closed.
            /// </summary>
            public static List<SceneEditorController> WindowList = new List<SceneEditorController>();
        // -- Private Attributes --
    // --- /Attributes ---
}
}