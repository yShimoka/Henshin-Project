// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Editor.Skin;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.Inspector {

/// <summary>
/// Static controller class that will manipulate the <see cref="InspectorState"/> objects.
/// </summary>
public static class InspectorController {
    // ---  Methods ---
        // -- Public Methods --
            // - Initialization -
            /// <summary>
            /// Initializes the specified <see cref="InspectorState"/> object.
            /// </summary>
            /// <param name="inspector">The object to initialize.</param>
            /// <param name="owner">The owner of this inspector.</param>
            public static void Initialize(InspectorState inspector, SceneEditorController owner) {
                // Store the owner of the inspector.
                inspector.Owner = owner.State;
            }
            
            // - Events -
            /// <summary>
            /// Prepares the inspector for rendering.
            /// </summary>
            /// <param name="inspector">The inspector to render.</param>
            public static void Prepare(InspectorState inspector) {
                // Apply the inspector's ratio.
                SkinState.RatioStruct.ApplyRatio(
                    from: inspector.Owner.WindowRect,
                    ratio: SkinState.Ratios.SceneEditorInspectorRatio,
                    to: ref inspector.Rect
                );
                
                // Compute the scene editor and action editor rects.
                 inspector.SceneRect.Set(
                    x: 0, 
                    y: 0, 
                    width: inspector.Rect.width, 
                    height: Mathf.Ceil(f: inspector.Rect.height * 0.4f)
                 );
                 inspector.ActionRect.Set(
                    x: 0, 
                    y: inspector.SceneRect.height, 
                    width: inspector.Rect.width, 
                    height: inspector.Rect.height * 0.6f
                );
            }
            
            /// <summary>
            /// Handles the events for the inspector.
            /// For now, does nothing.
            /// </summary>
            /// <param name="inspector">The inspector to handle events for.</param>
            public static void HandleEvents(InspectorState inspector) {}
        // -- Private Methods --
    // --- /Methods ---
}
}