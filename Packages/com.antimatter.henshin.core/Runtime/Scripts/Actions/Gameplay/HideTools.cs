// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Runtime.Actions.Base;
using Henshin.Runtime.Gameplay.Components.Textbox;
using Henshin.Runtime.Gameplay.Components.Toolbox;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Actions.Gameplay {

/// <summary>
/// Action used to hide the <see cref="TextboxController"/> object.
/// </summary>
[ActionControllerType(stateType: typeof(HideToolsState)), ActionControllerCategory(category: EActionCategory.Gameplay)]
public class HideToolsAction: TimedAction {
    // ---  SubObjects --
        // -- Public Classes --
            public class HideToolsState: TimedState {}
    // --- /SubObjects --
    
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="ActionController.Apply"/>
            protected override void Apply() {
                // Reveal the text box.
                ToolboxView.Hide(callback: this.Finish, time: this.State.Time);
            }
            
    // --- /Methods ---
}
}