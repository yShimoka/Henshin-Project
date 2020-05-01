// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.State.Scenery {

/// <summary>
/// State class used by the <see cref="Henshin.Controller.Scenery.TextBox"/> object.
/// Stores all the data relative to the controller.
/// </summary>
public class TextBox {
    // ---  SubObjects ---
        // -- Public Emums --
            /// <summary>
            /// Lists all the text box modes.
            /// </summary>
            public enum EMode {
                None, Snowball, Holes, Comparison
            }
    // --- /SubObjects ---
    
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>Reference to the owner game object.</summary>
            public GameObject GameObject;
            
            /// <summary>Text renderer used to display the text in its original language.</summary>
            public UnityEngine.UI.Text OriginalText;
            
            /// <summary>Text renderer used to display the text in the translated language.</summary>
            public UnityEngine.UI.Text TranslatedText;
            
            /// <summary>Current text box mode.</summary>
            public EMode Mode;
    // --- /Attributes ---
}
}