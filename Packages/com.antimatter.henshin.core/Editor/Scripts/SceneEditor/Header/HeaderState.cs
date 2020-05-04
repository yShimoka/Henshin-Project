// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.Header {

/// <summary>
/// State class used to represent the header of a <see cref="SceneEditorController"/> window.
/// Stores all the necessary parameters within itself.
/// </summary>
[System.SerializableAttribute]
public class HeaderState {
    // ---  Attributes ---
        // -- Serialized Attributes --
            /// <summary>
            /// Index of the application object that is being edited.
            /// </summary>
            public int EditedApplicationIndex;
            
            /// <summary>
            /// Index of the act object that is being edited.
            /// </summary>
            public int EditedActIndex;
            
            /// <summary>
            /// Index of the scene object that is being edited.
            /// </summary>
            public int EditedSceneIndex;
            
        // -- Public Attributes --
            // - References -
            /// <summary>
            /// Reference to the owner of this header instance.
            /// </summary>
            [System.NonSerializedAttribute]
            public SceneEditorState Owner;
            
            /// <summary>
            /// Reference to the instance of the application that is being edited.
            /// </summary>
            [JetBrains.Annotations.CanBeNullAttribute]
            public Runtime.Application.ApplicationState EditedApplication
            {
                get {
                    // Check if the index's value is correct.
                    if (this.EditedApplicationIndex >= 0 && this.EditedApplicationIndex < HeaderState.EditableApplications.Length) {
                        return HeaderState.EditableApplications[this.EditedApplicationIndex];
                    } else {
                        return null;
                    }
                }
            }
            
            /// <summary>
            /// Reference to the instance of the act that is being edited.
            /// </summary>
            [JetBrains.Annotations.CanBeNullAttribute]
            public Runtime.Directions.Act.ActState EditedAct
            {
                get {
                    // Check if the application is correct.
                    if (this.EditedApplication != null) {
                        // Check if the index's value is correct.
                        if (this.EditedActIndex >= 0 && this.EditedActIndex < this.EditedApplication.ActList.Length) {
                            return this.EditedApplication.ActList[this.EditedActIndex];
                        }
                    }
                    
                    // Return a null on failure.
                    return null;
                }
            }
            
            /// <summary>
            /// Reference to the instance of the scene that is being edited.
            /// </summary>
            [JetBrains.Annotations.CanBeNullAttribute]
            public Runtime.Directions.Scene.SceneState EditedScene
            {
                get {
                    // Check if the application is correct.
                    if (this.EditedAct != null) {
                        // Check if the index's value is correct.
                        if (this.EditedSceneIndex >= 0 && this.EditedSceneIndex < this.EditedAct.SceneList.Length) {
                            return this.EditedAct.SceneList[this.EditedSceneIndex];
                        }
                    }
                    
                    // Return a null on failure.
                    return null;
                }
            }
            
            // - Static References -
            /// <summary>
            /// Stores a list of all the application objects that can be edited.
            /// Updated on reserialization of the project.
            /// </summary>
            public static Runtime.Application.ApplicationState[] EditableApplications;
            
            // - Rects -
            /// <summary>
            /// Stores the rect of the header.
            /// </summary>
            [System.NonSerializedAttribute]
            public UnityEngine.Rect Rect = new UnityEngine.Rect();
            
            /// <summary>
            /// Rect set to the size that is allowed for the content of the header.
            /// </summary>
            [System.NonSerializedAttribute]
            public UnityEngine.Rect ContentRect = new UnityEngine.Rect();
    // --- /Attributes ---
}
}