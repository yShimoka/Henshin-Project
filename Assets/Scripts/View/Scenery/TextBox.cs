// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEngine;
using UnityEngine.UI;

/* Wrap the class within the local namespace. */
namespace Henshin.View.Scenery {

/// <summary>
/// 
/// </summary>
public static class TextBox {
    // ---  Attributes ---
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Creates the view for the specified text box state.
            /// </summary>
            /// <param name="state">The state object to create.</param>
            public static void CreateView(Henshin.State.Scenery.TextBox state) {
                // Create the text box's ui renderer.
                Image renderer = state.GameObject.GetComponent<Image>();
                
                // Set the sprite renderer's properties.
                renderer.sprite = Henshin.State.Application.Current.textBox;
                renderer.type = Image.Type.Sliced;
            }
    // --- /Methods ---
}
}