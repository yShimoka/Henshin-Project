// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System.Linq;
using Henshin.Runtime.Actions.Base;
using Henshin.Runtime.Gameplay.Components.Textbox;
using Henshin.Runtime.Gameplay.Modes;
using UnityEngine;
using UnityEngine.UI;
using Selectable = Henshin.Runtime.Libraries.Selectable;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Actions.Gameplay {

/// <summary>
/// Action used to reveal the <see cref="TextboxController"/> object.
/// </summary>
[ActionControllerType(stateType: typeof(ShowTextState)), ActionControllerCategory(category: EActionCategory.Gameplay)]
public class ShowTextAction: TimedAction {
    // ---  SubObjects --
        // -- Public Classes --
            public class ShowTextState: TimedState {}
    // --- /SubObjects --
    
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="ActionController.Apply"/>
            protected override void Apply() {
                // Reveal the text box.
                TextboxView.Reveal(callback: this.Finish, time: this.State.Time);
            }
            
    // --- /Methods ---
}
}