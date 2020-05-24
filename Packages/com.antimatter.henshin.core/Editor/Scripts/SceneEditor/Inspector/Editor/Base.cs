// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Henshin.Runtime.Actions;
using JetBrains.Annotations;
using UnityEditor;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.Inspector.Editor {

/// <summary>
/// Attribute used to associate an editor with an action state's type.
/// </summary>
[AttributeUsage(validOn: AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class ActionEditorAttribute: Attribute {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Type of the action that is rendered by this class. 
            /// </summary>
            public Type ActionType;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Creates a new instance.
            /// Stores the <see cref="actionType"/>.
            /// </summary>
            public ActionEditorAttribute(Type actionType) {
                if (!actionType.IsSubclassOf(c: typeof(ActionController))) {
                    throw new InvalidOperationException(message: "actionType must be an ActionController.");
                }
                
                this.ActionType = actionType;
            }
    // --- /Methods ---
}

/// <summary>
/// Base class used for all the inspector editor objects.
/// </summary>
public abstract class Base {
    // ---  Attributes ---
        // -- Private Attributes --
            /// <summary>
            /// Stores all the constructors for the specified action types.
            /// </summary>
            private static Dictionary<Type, ConstructorInfo> _msConstructors;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Calls the <see cref="Render"/> method on the specified <see cref="TEditorType"/> instance.
            /// </summary>
            /// <param name="action">The action to render.</param>
            /// <param name="inspector">The inspector instance where the object is rendered.</param>
            /// <typeparam name="TEditorType">The type of the editor</typeparam>
            public static void Render<TEditorType>(ActionState action, InspectorState inspector) where TEditorType: Base, new() {
                // Render the inspector.
                new TEditorType()._Render(action: action, inspector: inspector);
            }
            
            /// <summary>
            /// Calls the <see cref="_Render"/> method for the action state object.
            /// </summary>
            /// <param name="action">The action to render.</param>
            /// <param name="inspector">The inspector instance where the object is rendered.</param>
            public static void Render([CanBeNull]ActionState action, InspectorState inspector) {
                // If the action is null, do nothing.
                if (action == null) return;
                
                // If the dictionary is not created.
                if (Base._msConstructors == null) {
                    // Create the dictionary.
                    Base._msConstructors = new Dictionary<Type, ConstructorInfo>();
                    
                    // Get all the attribute instances.
                    foreach (Type childClass in typeof(Base).Assembly
                        .GetTypes()
                        .Where(predicate: type => type.IsSubclassOf(c: typeof(Base)))
                    ) {
                        foreach (ActionEditorAttribute attribute in childClass
                            .GetCustomAttributes(attributeType: typeof(ActionEditorAttribute), inherit: false)
                            .Cast<ActionEditorAttribute>()
                        ) {
                            // Store the pair in the dictionary.
                            Base._msConstructors.Add(
                                key: attribute.ActionType,
                                value: childClass.GetConstructor(types: new Type[] {})
                            );
                        }
                    }
                }
                
                // Check if the key is in the dictionary.
                if (Base._msConstructors.ContainsKey(key: action.ControllerType)) {
                    // Invoke the constructor and call the render method.
                    (Base._msConstructors[key: action.ControllerType].Invoke(parameters: new object[] {}) as Base)
                        ?._Render(action: action, inspector: inspector);
                } else {
                    EditorGUILayout.HelpBox(
                        message: $"There is no renderer for action of type {action.ActionControllerName}",
                        type: MessageType.Error
                    );
                }
            }
        
        // -- Protected Methods --
            /// <summary>
            /// Renders the specified action state object.
            /// </summary>
            /// <param name="action">The action to render.</param>
            /// <param name="inspector">The inspector instance where the object is rendered.</param>
            protected abstract void _Render(ActionState action, InspectorState inspector);
    // --- /Methods ---
}
}