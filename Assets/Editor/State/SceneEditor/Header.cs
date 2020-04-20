// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System.Collections.Generic;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.State.SceneEditor {

/// <summary>
/// Static header state class.
/// This is a static class since there can only be one <see cref="Window"/>element.
/// </summary>
public static class Header {
    // ---  Attributes ---
        // -- Public Attributes --
            // - Constants -
            /// <summary>Ratio of the header relative to its container.</summary>
            public static readonly Rect RATIO = new Rect{ x = 0f, y = 0f, width = 1f, height = 0.1f };
            
            // - Render Area -
            /// <summary>Rect of this object's container.</summary>
            public static Rect ContainerRect = new Rect();
            
            /// <summary>Rect of the header</summary>
            public static Rect Rect = new Rect();
            
            // - Indices -
            /// <summary>Reference to the scene that is currently edited.</summary>
            public static int CurrentSceneIndex;
            
            // - Events -
            /// <summary>Event triggered when the selected scene has been updated.</summary>
            public static UnityEngine.Events.UnityEvent OnSceneChange = new UnityEngine.Events.UnityEvent();
            
            // - References -
            /// <summary>List of all the <see cref="Henshin.State.Directions.Scene"/> objects found in the project.</summary>
            public static List<Henshin.State.Directions.Scene> SceneList;
            
            /// <summary>List of all the names of the scenes found in the project.</summary>
            /// <seealso cref="SceneList"/>
            public static string[] SceneNames;
            
            // - Accessors -
            /// <summary>Accessor helper to the current scene object.</summary>
            public static Henshin.State.Directions.Scene CurrentScene => Header.SceneList[index: Header.CurrentSceneIndex];
    // --- /Attributes ---
}
}