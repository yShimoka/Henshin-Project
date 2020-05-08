// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Actions.Gameplay {

/// <summary>
/// Action used to wait for the next step of the snowball to take place.
/// </summary>
[ActionControllerType(stateType: typeof(StepState)), ActionControllerCategory(category: EActionCategory.Gameplay)]
public class StepAction: ActionController {
    // ---  SubObjects ---
         // -- Public Classes --
            /// <summary>
            /// State class used for the step actions.
            /// </summary>
            public class StepState: ActionState {}
    // --- /SubObjects ---
     
    // ---  Attributes ---
        // -- Public Attributes --
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
        // -- Protected Methods --
            protected override void Apply() {
                // TODO: Wait for the snowball step.
                //SnowballController.NextStep(callback: this.Finish);
            }
            
            protected override void SaveParameters() {}
            protected override void LoadParameters() {}
        // -- Private Methods --
    // --- /Methods ---
}
}