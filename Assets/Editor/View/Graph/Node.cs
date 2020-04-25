// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.View.Graph {

/// <summary>
/// Controller class used to manipulate <see cref="Henshin.Editor.State.Graph.Node"/> objects.
/// </summary>
public static class Node {
    // ---  Attributes ---
        // -- Public Attributes --
        // -- Private Attributes --
            // - Textures -
            /// <summary>Texture used by the regular node body.</summary>
            private static Texture2D _msNodeTexture;
            /// <summary>Texture used by the regular node objects.</summary>
            private static Texture2D _msRegularTexture;
            /// <summary>Texture used by the start node objects.</summary>
            private static Texture2D _msStartTexture;
            /// <summary>Texture used by the end node objects.</summary>
            private static Texture2D _msEndTexture;
            
            /// <summary>Texture used for the node title.</summary>
            private static Texture2D _msTitleTexture;
            
            // - Contents -
            
            // - Styles -
            /// <summary>Style used to render the regular node body.</summary>
            private static GUIStyle _msNodeStyle;
            /// <summary>Style used to render the regular node objects.</summary>
            private static GUIStyle _msRegularStyle;
            /// <summary>Style used to render the start node objects.</summary>
            private static GUIStyle _msStartStyle;
            /// <summary>Style used to render the end node objects.</summary>
            private static GUIStyle _msEndStyle;
            
            /// <summary>Style used for the node title.</summary>
            private static GUIStyle _msTitleStyle;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Initializes the view class.
            /// Loads all required objects.
            /// </summary>
            public static void Initialize() {
                // Create all the texture objects.
                Node._CreateTextures();
                // Create all the content objects.
                Node._CreateContents();
                // Create all the style objects.
                Node._CreateStyles();
                
                // Initialize the socket view class.
                Socket.Initialize();
            }
            
            /// <summary>
            /// Renders the specified node.
            /// </summary>
            /// <param name="node">The node object to render.</param>
            public static void Render(State.Graph.Node node) {
                // Update the title's font size.
                Node._msTitleStyle.fontSize = Mathf.FloorToInt(f: 16 * (1 / node.owner.scale));
                
                // Render the header.
                GUI.Box(
                    position: node.HeaderRect, 
                    content: GUIContent.none, 
                    style: node.Transformation.GetType() == typeof(Henshin.Controller.Directions.Transformations.Scene.Start) ? 
                        Node._msStartStyle :
                        node.Transformation.GetType() == typeof(Henshin.Controller.Directions.Transformations.Scene.End) ?
                            Node._msEndStyle :
                            Node._msRegularStyle 
                );
                // Render the body.
                GUI.Box(
                    position: node.BodyRect, 
                    content: GUIContent.none, 
                    style: Node._msNodeStyle
                );
                
                // Render the name of the transformation.
                GUI.Label(position: node.TextRect, text: node.Transformation.GetType().Name, style: Node._msTitleStyle);
            }
            
        // -- Private Methods --
            /// <summary>Creates all the required texture objects.</summary>
            private static void _CreateTextures() {
                // Load the node texture.
                Node._msNodeTexture = Resources.Load<Texture2D>(path: "Editor/Graph/UI_NODE_Node");
                // Load the node texture.
                Node._msRegularTexture = Resources.Load<Texture2D>(path: "Editor/Graph/UI_NODE_Regular");
                // Load the start node texture.
                Node._msStartTexture = Resources.Load<Texture2D>(path: "Editor/Graph/UI_NODE_Start");
                // Load the end node texture.
                Node._msEndTexture = Resources.Load<Texture2D>(path: "Editor/Graph/UI_NODE_End");
                
                // Load the node title texture.
                Node._msTitleTexture = new Texture2D(width: 1, height: 1, textureFormat: TextureFormat.ARGB32, mipChain: false);
                Node._msTitleTexture.SetPixel(x: 0, y: 0, color: Color.clear);
                Node._msTitleTexture.wrapMode = TextureWrapMode.Repeat;
                Node._msTitleTexture.filterMode = FilterMode.Point;
                Node._msTitleTexture.Apply();
            }
            
            /// <summary>Creates all the required <see cref="GUIContent"/> objects.</summary>
            private static void _CreateContents() {}
            
            /// <summary>Creates all the required <see cref="GUIStyle"/> objects.</summary>
            private static void _CreateStyles() {
                // Load the regular node style.
                Node._msNodeStyle = new GUIStyle {
                    border = { left = 12, top = 0, right = 12, bottom = 12 },
                    normal = { background = Node._msNodeTexture, textColor = Color.white }
                };
                // Load the regular node style.
                Node._msRegularStyle = new GUIStyle {
                    border = { left = 12, top = 24, right = 12, bottom = 0 },
                    normal = { background = Node._msRegularTexture, textColor = Color.white }
                };
                // Load the start node style.
                Node._msStartStyle = new GUIStyle {
                    border = { left = 12, top = 24, right = 12, bottom = 0 },
                    normal = { background = Node._msStartTexture, textColor = Color.white }
                };
                // Load the end node style.
                Node._msEndStyle = new GUIStyle {
                    border = { left = 12, top = 24, right = 12, bottom = 0 },
                    normal = { background = Node._msEndTexture, textColor = Color.white }
                };
                
                // Load the node's title style.
                Node._msTitleStyle = new GUIStyle {
                    normal = { background = Node._msTitleTexture, textColor = Color.white },
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 16,
                    fontStyle = FontStyle.Bold
                };
            }
    // --- /Methods ---
}
}