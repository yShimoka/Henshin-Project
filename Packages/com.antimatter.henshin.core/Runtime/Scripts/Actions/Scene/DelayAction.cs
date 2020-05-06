// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using Henshin.Runtime.Actions.Base;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Actions.Scene {

/// <summary>
/// </summary>
[ActionControllerType(stateType: typeof(DelayState)), ActionControllerCategory(category: EActionCategory.Scene)]
public class DelayAction: TimedAction {
    // ---  SubObjects ---
        // -- Public Classes --
            /// <summary>
            /// State class used to represent a <see cref="StartAction"/>.
            /// </summary>
            [Serializable]
            public class DelayState: TimedState { }
    // --- /SubObjects ---
    
    // ---  Methods ---
        // -- Protected Methods --
            /// <summary>
            /// Does nothing but delaying.
            /// </summary>
            protected override void Update(float normalizedTime) { }
    // --- /Methods ---
}
}