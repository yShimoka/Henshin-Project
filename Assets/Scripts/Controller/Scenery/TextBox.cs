// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Controller.Scenery {

/// <summary>
/// Text box mono behaviour controller.
/// Handles the game's text boxes.
/// </summary>
[RequireComponent(requiredComponent: typeof(UnityEngine.UI.Image))]
public class TextBox: MonoBehaviour {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>Reference to the state of this object.</summary>
            [System.NonSerializedAttribute]
            public State.Scenery.TextBox State = new Henshin.State.Scenery.TextBox();
            
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
            /// <summary>Unity event fired when the object gets created.</summary>
            private void Awake() {
                // Load the parent GameObject.
                this.State.GameObject = this.gameObject;
                
                // Create the text box's view object.
                View.Scenery.TextBox.CreateView(state: this.State);
            }

            /// <summary>Unity event when the object gets enabled.</summary>
            private void OnEnable() {
                // Check the current gameplay type.
                switch (this.State.Mode) {
                case Henshin.State.Scenery.TextBox.EMode.None:
                    break;
                case Henshin.State.Scenery.TextBox.EMode.Snowball:
                    break;
                case Henshin.State.Scenery.TextBox.EMode.Comparison:
                    break;
                case Henshin.State.Scenery.TextBox.EMode.Holes:
                    break;
                }
            }
            
            /// <summary>Unity event when the object gets disabled.</summary>
            private void OnDisable() {
            }

        // -- Public Methods --
        // -- Private Methods --
            private void _PrepareSnowball() {}
            private void _PrepareComparison() {}
            private void _PrepareHoles() {}
    // --- /Methods ---
}
}