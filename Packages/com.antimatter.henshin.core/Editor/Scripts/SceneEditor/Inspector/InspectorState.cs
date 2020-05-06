// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Editor.SceneEditor.GraphArea.Node;
using JetBrains.Annotations;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.Inspector {

/// <summary>
/// Class used to represent the current state of the scene editor's inspector.
/// </summary>
public class InspectorState {
    // ---  Attributes ---
        // -- Public Attributes --
            // - References -
            /// <summary>
            /// Reference to the <see cref="SceneEditorState"/> that owns this <see cref="InspectorState"/>.
            /// </summary>
            public SceneEditorState Owner;
            
            /// <summary>
            /// Reference to the node that is being edited.
            /// </summary>
            [CanBeNull]
            public NodeState EditedNode;
            
            // - Rects -
            /// <summary>
            /// Rect within which the inspector will be rendered.
            /// </summary>
            public Rect Rect = new Rect();
            
            /// <summary>
            /// Rect within which the scene section of the inspector will be rendered.
            /// </summary>
            public Rect SceneRect = new Rect();
            
            /// <summary>
            /// Rect within which the action section of the inspector will be rendered.
            /// </summary>
            public Rect ActionRect = new Rect();
            
            // - Positions -
            /// <summary>
            /// Position of the actor list's scroll view.
            /// </summary>
            public Vector2 ActorListPosition;
            
            // - Flags -
            /// <summary>
            /// Flag set if the list of all the actors is visible.
            /// </summary>
            public bool IsActorListVisible = false;
            
            /// <summary>
            /// Index of the actor that is shown.
            /// </summary>
            public int ShownActorIndex = -1;
    // --- /Attributes ---
}
}