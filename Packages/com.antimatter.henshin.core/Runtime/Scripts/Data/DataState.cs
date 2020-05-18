// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System.Xml;
using Henshin.Runtime.Gameplay.Components.Answer;
using JetBrains.Annotations;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Data {

/// <summary>
/// State class used to handle the game data's xml file.
/// Loaded and manipulated by the <see cref="DataController"/> class.
/// </summary>
public static class DataState {
    // ---  Attributes ---
        // -- Public Attributes --
            // - Xml Data -
            /// <summary>
            /// Reference to the xml document that is being manipulated.
            /// </summary>
            public static XmlDocument Document;
            
            /// <summary>
            /// Instance of the namespace manager.
            /// Used to simplify XPath requests.
            /// </summary>
            public static XmlNamespaceManager NamespaceManager;
            
            /// <summary>
            /// Reference to the text file on disk.
            /// </summary>
            public static TextAsset File;
            
#if UNITY_EDITOR
            /// <summary>
            /// Hash of the file contents.
            /// Used only in editor, to check if the contents have changed. 
            /// </summary>
            public static byte[] Hash;
            
            /// <summary>
            /// Index of the current act.
            /// Stored only in editor mode.
            /// </summary>
            public static int ActIndex;
            
            /// <summary>
            /// Index of the current scene.
            /// Stored only in editor mode.
            /// </summary>
            public static int SceneIndex;
#endif
            
            // - Data Containers -
            /// <summary>
            /// Reference to the node that contains the current scene's data.
            /// </summary>
            [CanBeNull]
            public static XmlNode CurrentScene;
            
            /// <summary>
            /// Stores the Xml Gameplay node's 'kind' attribute value.
            /// This is used to determine which gameplay is being currently read.
            /// </summary>
            [CanBeNull]
            public static string Kind;
            
            /// <summary>
            /// Stores the contents of the Xml Original nodes.
            /// This should be the text before any translation.
            /// </summary>
            public static string[] Original; 
            
            /// <summary>
            /// Stores the contents of the Xml Translated nodes.
            /// This should be the translated text.
            /// </summary>
            public static string[] Translated;
            
            /// <summary>
            /// Stores the contents of the Xml Option nodes.
            /// This is a 2D array because of the 'snowball' gameplay.
            /// </summary> 
            public static string[][] Options; 
            
            /// <summary>
            /// Default size of the <see cref="AnswerState"/> objects.
            /// </summary>
            public static readonly Vector2 DEFAULT_ANSWER_OBJECT_SIZE = new Vector2(x: 250, y: 50);
            
            /// <summary>
            /// Expected size of the <see cref="AnswerState"/> objects.
            /// Defaults to <see cref="DEFAULT_ANSWER_OBJECT_SIZE"/> if none is specified.
            /// </summary>
            public static Vector2 AnswerObjectSize;
            
            /// <summary>
            /// Flag set if the answer objects should be duplicated upon pickup.
            /// This allows the usage of the same answer for multiple targets.
            /// </summary>
            public static bool DuplicateAnswers;
    // --- /Attributes ---
}
}