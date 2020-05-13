// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Collections.Generic;
using Henshin.Runtime.Application;
using Henshin.Runtime.Gameplay.Components;
using Henshin.Runtime.Libraries;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Actions.Gameplay {

/// <summary>
/// Shows either the text or the toolbox (or both).
/// Uses MoveTo-like actions the actors.
/// </summary>
[ActionControllerType(stateType: typeof(GuiVisibleState)), ActionControllerCategory(category: EActionCategory.Gameplay)]
public class GuiVisibleAction: ActionController {
    // ---  SubObjects ---
        // -- Public Enumerators --
            /// <summary>
            /// Flags used to determine which actor should be shown or hidden.
            /// </summary>
            [Flags]
            public enum FActorType {
                ToolBox = 0b01, TextBox = 0b10
            }
            
        // -- Public Classes --
            /// <summary>
            /// State class used for the show action.
            /// </summary>
            public class GuiVisibleState: ActionState {
                // ---  Attributes ---
                    // -- Public Attributes --
                        /// <summary>
                        /// Stores which actor should be revealed.
                        /// </summary>
                        public FActorType Actor;
                        
                        /// <summary>
                        /// Flag set if the object should be shown or hidden.
                        /// </summary>
                        public bool Visible;
                        
                        /// <summary>
                        /// The easing function applied to the movement.
                        /// </summary>
                        public EasingFunction.Ease EaseMode;
                        
                        /// <summary>
                        /// The time that should be taken to show the object.
                        /// </summary>
                        public float Time;
                // --- /Attributes ---
            }
    // --- /SubObjects ---
    
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Casts the state of the controller to a <see cref="GuiVisibleState"/> object.
            /// </summary>
            public new GuiVisibleState State => (GuiVisibleState)base.State;
        
        // -- Private Attributes --
            /// <summary>
            /// Reference to the transform of the toolbox.
            /// </summary>
            private Transform _mToolbox;
            
            /// <summary>
            /// Reference to the transform of the text box.
            /// </summary>
            private Transform _mTextbox;
            
            /// <summary>
            /// Easing function used for the normalized computation.
            /// </summary>
            private EasingFunction.Function _mEasingFunction;
            
            /// <summary>
            /// Timer used to compute the time.
            /// </summary>
            private float _mTimer;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="ActionController.Apply"/>
            protected override void Apply() {
                // If the toolbox is concerned.
                if ((this.State.Actor & FActorType.ToolBox) != 0) {
                    // Seek the toolbox in the scene.
                    this._mToolbox = Object.FindObjectOfType<ToolBoxComponent>().transform.parent;
                    
                    // Show the tool box.
                    Image toolboxImg = this._mToolbox.GetComponent<Image>(); 
                    toolboxImg.color = Color.white;
                    toolboxImg.canvas.sortingLayerID = ApplicationView.SortingLayers.GUI;
                }
                // If the text box is concerned.
                if ((this.State.Actor & FActorType.TextBox) != 0) {
                    // Seek the textbox in the scene.
                    this._mTextbox = Object.FindObjectOfType<TextBoxComponent>().transform.parent;
                    
                    // Show the text box.
                    Image textboxImg = this._mTextbox.GetComponent<Image>(); 
                    textboxImg.color = Color.white;
                    textboxImg.canvas.sortingLayerID = ApplicationView.SortingLayers.GUI;
                }
                
                // Clear the timer.
                this._mTimer = 0;
                
                // Load the easing function.
                this._mEasingFunction = EasingFunction.GetEasingFunction(easingFunction: this.State.EaseMode);
                
                // Subscribe to the update call.
                ApplicationController.OnTick.AddListener(call: this._Update);
            }
            
            /// <inheritdoc cref="ActionController.Finish"/>
            protected override void Finish() {
                // Remove the update listener.
                ApplicationController.OnTick.RemoveListener(call: this._Update);
                
                // Call the base method.
                base.Finish();
            }

            /// <inheritdoc cref="ActionController.SaveParameters"/>
            protected override void SaveParameters() {
                // Save the actor type.
                this.AddSerializedData(data: this.State.Actor);
                
                // Save the visible flag.
                this.AddSerializedData(data: this.State.Visible);
                
                // Save the time and easing method.
                this.AddSerializedData(data: this.State.EaseMode);
                this.AddSerializedData(data: this.State.Time);
            }
            
            /// <inheritdoc cref="ActionController.LoadParameters"/>
            protected override void LoadParameters() {
                // Load the actor type.
                this.State.Actor = this.NextSerializedData<FActorType>();
                
                // Load the visible flag.
                this.State.Visible = this.NextSerializedData<bool>();
                
                // Load the time and easing method.
                this.State.EaseMode = this.NextSerializedData<EasingFunction.Ease>();
                this.State.Time = this.NextSerializedData<float>();
            }
        
        // -- Private Methods --
            private void _Update(float deltaTime) {
                // Update the timer.
                this._mTimer += deltaTime;
                
                // Get the normalized time.
                float norm = this.State.Time == 0 ? 1 : this._mTimer / this.State.Time;
                // If the object is hidden, invert the value.
                if (!this.State.Visible) norm = 1 - norm;
                
                // Apply the easing function.
                norm = this._mEasingFunction(s: 0, e: 1, v: norm);
                
                // If the toolbox is revealed.
                if ((this.State.Actor & FActorType.ToolBox) != 0) {
                    // Apply the movement to the toolbox.
                    this._mToolbox.localPosition = new Vector2(x: Mathf.Lerp(
                        a: ApplicationView.VIEW_WIDTH,
                        b: ApplicationView.VIEW_WIDTH / 2f,
                        t: norm
                    ), y: 100);
                }
                // If the textbox is revealed.
                if ((this.State.Actor & FActorType.TextBox) != 0) {
                    // Apply the movement to the textbox.
                    this._mTextbox.localPosition = new Vector2(x: 0, y: -Mathf.Lerp(
                        a: ApplicationView.VIEW_HEIGHT,
                        b: ApplicationView.VIEW_HEIGHT / 2f,
                        t: norm
                    ));
                }
                
                
                // If the timer reached the end.
                if (this._mTimer >= this.State.Time) {
                    // Finish the action.
                    this.Finish();
                }
            }
    // --- /Methods ---
}
}