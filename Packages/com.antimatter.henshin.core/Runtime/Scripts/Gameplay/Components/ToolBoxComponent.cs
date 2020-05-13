// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Runtime.Application;
using UnityEngine;
using UnityEngine.UI;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay.Components {

/// <summary>
/// Component used on the toolbox object.
/// Allows simple adding of children objects.
/// </summary>
[RequireComponent(requiredComponent: typeof(RectTransform)), AddComponentMenu(menuName: "Henshin/Gameplay/Tool Box", order: 11)]
public class ToolBoxComponent: MonoBehaviour {
    // ---  Attributes ---
        // -- Serialized Attributes --
            /// <summary>
            /// Margin that is applied between items and the bounding box of the tool box.
            /// </summary>
            public float Margin;
            
            /// <summary>
            /// Size of the toolbox.
            /// </summary>
            public Vector2 Size;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
            /// <summary>
            /// Event called when the object is enabled.
            /// </summary>
            private void OnEnable() {
                // Update the size and position of the parent.
                RectTransform transform = this.transform.parent.GetComponent<RectTransform>(); 
                transform.sizeDelta = this.Size;
                transform.anchoredPosition = Vector2.right * ApplicationView.VIEW_WIDTH;
            }
        
        // -- Public Methods --
            /// <summary>
            /// Attaches a new <see cref="RectTransform"/> to the toolbox.
            /// </summary>
            /// <param name="child">The child to attach.</param>
            public void AttachChild(RectTransform child) {
                // Prepare the y position store.
                float y;
                
                // Check if the toolbox has any children.
                if (this.transform.childCount > 0) {
                    // Get the rect transform of the latest child object.
                    RectTransform childRect = this.transform
                        .GetChild(index: this.transform.childCount - 1)
                        .GetComponent<RectTransform>();
                    
                    // Get the y position of the last child in the list.
                    y = childRect.anchoredPosition.y - childRect.sizeDelta.y;
                } else {
                    // Start at the end of the sprite's border.
                    y = -this.transform.parent.GetComponent<Image>().sprite.border.y;
                }
                
                // Attach the child to ourselves.
                child.SetParent(parent: this.transform, worldPositionStays: false);
                child.SetAsLastSibling();
                
                // Update its position.
                child.anchoredPosition = new Vector2(
                    x: this.Margin + child.sizeDelta.x / 2, 
                    y: y - this.Margin
                );
            }
            
            public void Clear() {
                while (this.transform.childCount > 0) {
                    Object.DestroyImmediate(obj: this.transform.GetChild(index: 0).gameObject);
                }
            }
    // --- /Methods ---
}
}