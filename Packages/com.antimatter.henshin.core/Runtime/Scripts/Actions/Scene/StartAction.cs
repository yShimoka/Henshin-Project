// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using Henshin.Runtime.Application;
using Henshin.Runtime.Data;
using Henshin.Runtime.Gameplay;
using Henshin.Runtime.Gameplay.Components.Answer;
using Henshin.Runtime.Gameplay.Components.Target;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Actions.Scene {

/// <summary>
/// Controller class used to manipulate <see cref="StartAction.State"/> objects.
/// This action marks the beginning of the specified action.
/// </summary>
[ActionControllerType(stateType: typeof(StartState)), ActionControllerCategory(category: EActionCategory.Start)]
public class StartAction: ActionController {
    // ---  SubObjects ---
        // -- Public Classes --
            /// <summary>
            /// State class used to represent a <see cref="StartAction"/>.
            /// </summary>
            [Serializable]
            public class StartState: ActionState { }
    // --- /SubObjects ---
    
    // ---  Attributes ---
        // -- Public Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
        // -- Public Methods --
        // -- Protected Methods --
            /// <summary>
            /// Starts the scene.
            /// Just finishes the action immediately.
            /// </summary>
            protected override void Apply() {
                DataController.LoadGameplay(identifier: "Neku Presentation");
                if (TargetController.Instantiate(parent: ApplicationView.GUI) is TargetState target) {
                    if (AnswerController.Instantiate(parent: ApplicationView.GUI) is AnswerState answer) {
                        answer.Transform.localPosition = Vector2.up * 250;
                        target.Transform.localPosition = Vector2.down * 250;
                        answer.Value = DataState.Options[0][0];
                        target.Value = DataState.Options[0][0];
                        AnswerView.UpdateText(answer: answer);
                        answer.Callback = this.Finish;
                    }
                }
                //this.Finish();
            }
    
            // - Serialization Events -
            /// <inheritdoc cref="ActionController.SaveParameters"/>
            protected override void SaveParameters() { }

            /// <inheritdoc cref="ActionController.LoadParameters"/>
            protected override void LoadParameters() { }
        // -- Private Methods --
    // --- /Methods ---
}
}