// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */
namespace Runtime.Application {

/// <summary>
/// State class used to represent the application.
/// Stores all the relevant information into a single area.
/// </summary>
[UnityEngine.CreateAssetMenuAttribute(fileName = "APP_State", menuName = "Henshin State", order = 0)]
public class ApplicationState: UnityEngine.ScriptableObject, UnityEngine.ISerializationCallbackReceiver {
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - Parameters -
            /// <summary>
            /// Serializable list of all the acts in the application.
            /// </summary>
            [UnityEngine.SerializeField] 
            public Directions.Act.ActState[] ActList;
            
#if UNITY_EDITOR
            /// <summary>
            /// Flag set if this is a debug application state.
            /// Used only within the editor.
            /// </summary>
            [UnityEngine.SerializeField]
            public bool IsDebugState;
#endif
            
            // - Visual Parameters -
            /// <summary>
            /// Color used when the camera is cleared.
            /// </summary>
            public UnityEngine.Color ClearColor;
            
            /// <summary>
            /// Icon displayed on the error scene.
            /// </summary>
            public UnityEngine.Sprite ErrorIcon; 
        // -- Public Attributes --
            // - Runtime Parameters -
            /// <summary>
            /// Reference to the current application's state.
            /// </summary>
            public static ApplicationState Own;
        
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
            // - Serialization Events -
            /// <summary>
            /// Event fired right before Unity serializes the objects of the project.
            /// Calls the <see cref="ApplicationController.Serialize"/> method on the instance.
            /// </summary>
            public void OnBeforeSerialize() { ApplicationController.Serialize(state: this); }

            /// <summary>
            /// Event fired right before Unity serializes the objects of the project.
            /// Calls the <see cref="ApplicationController.Deserialize"/> method on the instance.
            /// </summary>
            public void OnAfterDeserialize() { ApplicationController.Deserialize(state: this); }
    // --- /Methods ---
}
}