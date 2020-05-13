// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using UnityEngine;
using UnityEngine.UI;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay.Components {

/// <summary>
/// Component used as the target of a drag and drop action.
/// Uses Physics2D to compute collisions.
/// </summary>
[RequireComponent(requiredComponent: typeof(Image)), AddComponentMenu(menuName: "Henshin/Gameplay/Drop Target", order: 02)]
public class DropTargetComponent : MonoBehaviour {
    // ---  Attributes ---
        // -- Public Attributes --
            // - Components -
            /// <summary>
            /// Reference to the <see cref="UnityEngine.RectTransform"/> on this game object.
            /// </summary>
            [NonSerialized]
            public RectTransform RectTransform;
            
            /// <summary>
            /// Reference to the <see cref="UnityEngine.UI.Image"/> on this game object.
            /// </summary>
            [NonSerialized]
            public Image Image;
            
            // - Comparator -
            /// <summary>
            /// Expected value for the identifier that will enter this component's trigger area.
            /// </summary>
            [NonSerialized]
            public string ExpectedIdentifier;
            
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
            /// <summary>
            /// Called right after the object is created.
            /// </summary>
            private void Awake() {
                // Get the components of the game object.
                this.RectTransform = this.GetComponent<RectTransform>();
                this.Image = this.GetComponent<Image>();
                
                // Ensure that the pivot is located on the top center of the component.
                this.RectTransform.anchorMin = Vector2.up;
                this.RectTransform.anchorMax = Vector2.up;
                this.RectTransform.pivot = Vector2.up;
            }
    // --- /Methods ---
}
}