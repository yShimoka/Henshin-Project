// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Henshin.Runtime.Actions;
using Henshin.Runtime.Actions.Actor;
using Henshin.Runtime.Actor;
using UnityEditor;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.Inspector.Editor.Actor {

/// <summary>
/// Class used to render the pose action.
/// </summary>
[ActionEditor(actionType: typeof(PoseAction))]
public class Pose: Base {
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="Base._Render"/>
            protected override void _Render(ActionState action, InspectorState inspector) {
                if (inspector.Owner.Header.EditedScene == null) return;
                
                // Cast the action to a visible action.
                PoseAction.PoseState state = (PoseAction.PoseState)action;
                
                // Render the actor selector.
                Base.Render<Actor>(action: action, inspector: inspector);
                
                // Get the actor that is referenced.
                if (!(inspector.Owner.Header.EditedScene.ActorList[index: state.ActorIndex] is ActorState actor)) return;
                
                // Get the actor's pose store.
                if (!(actor.PoseStore is PoseStore store)) return;
                
                // Get the actor's pose list.
                if (!(store.PoseList is List<Sprite> list)) return;
                
                // Check the index of the actor's pose.
                if (state.PoseIndex < 0 || state.PoseIndex >= list.Count) {
                    state.PoseIndex = 0;
                }
                
                // Render the pose list.
                state.PoseIndex = EditorGUILayout.Popup(
                    selectedIndex: state.PoseIndex,
                    displayedOptions: 
                    list.Select(selector: pose => pose == null ? "empty" : pose.name.Substring(
                            startIndex: pose.name.LastIndexOf(value: "_", comparisonType: StringComparison.Ordinal)
                                        + 1
                        ))
                        .ToArray()
                );
                
                // Check if the texture is set.
                if (!(list[index: state.PoseIndex] is Sprite sprite)) return;
                if (!(sprite.texture is Texture texture)) return;
            
                // Get the height of the image.
                float tW = texture.width;
                float tH = texture.height;
                float ratio = tW / tH;
                float iH = inspector.ActionRect.height / 2f;
                float iW = iH * ratio;
                // Get the texture rect.
                Rect imageRect = new Rect {
                    position = new Vector2(
                        x: inspector.ActionRect.center.x - iW / 2, 
                        y: inspector.ActionRect.height - iH - 2
                    ),
                    width = iW,
                    height = iH
                };

                // Draw the selected pose.
                GUI.DrawTexture(position: imageRect, image: texture);
            }
    // --- /Methods ---
}
}