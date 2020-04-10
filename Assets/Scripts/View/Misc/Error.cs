// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using UnityEngine;
using UnityEngine.UI;

/* Wrap the class within the local namespace. */
namespace Henshin.View.Misc {

/// <summary>
/// Component used for custom error messaging.
/// </summary>
/// <seealso cref="Controller.Application.Error"/>
[AddComponentMenu(menuName: "Henshin/Misc/Error", order: 101), RequireComponent(requiredComponent: typeof(Text))]
public class Error: MonoBehaviour {
    // ---  Attributes ---
        // -- Private Attributes --
            /// <summary>Reference to the sibling <see cref="UnityEngine.UI.Text"/> component.</summary>
            private Text _mText;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
            /// <summary>Unity event fired when the object's scene gets loaded.</summary>
            private void Awake() {
                // Load the text component.
                this._mText = this.GetComponent<Text>();
            }
            
        // -- Public Methods --
            /// <summary>
            /// Sets the message that is drawn on the <see cref="UnityEngine.UI.Text"/> component.
            /// </summary>
            /// <param name="message">The message to draw.</param>
            public void SetMessage(string message) {
                // Update the text of the component.
                this._mText.text = message;
            }
    // --- /Methods ---
}
}