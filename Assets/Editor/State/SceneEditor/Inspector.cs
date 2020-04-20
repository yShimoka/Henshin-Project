// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.State.SceneEditor {

/// <summary>
/// 
/// </summary>
public static class Inspector {
    // ---  Attributes ---
        // -- Public Attributes --
            // - Constants -
            /// <summary>Ratio of the inspector relative to the overall window.</summary>
            public static readonly Rect RATIO = new Rect{ x = 0f, y = Header.RATIO.height, width = 0.2f, height = 1 - Header.RATIO.height };
            
            // - Render Area -
            /// <summary>Rect in which the inspector will be rendered.</summary>
            public static Rect Rect = new Rect();
        // -- Private Attributes --
    // --- /Attributes ---
}
}