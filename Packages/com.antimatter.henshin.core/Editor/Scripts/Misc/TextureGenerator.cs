// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.Misc {

/// <summary>
/// Static class used to generate textures on the fly.
/// It has an internal cache to avoid recreating textures that have been saved to disk. 
/// </summary>
public static class TextureGenerator {
    // ---  Attributes ---
        // -- Public Attributes --
            // - Constants -
            /// <summary>
            /// Base name of a texture element.
            /// </summary>
            public const string TEXTURE_BASE_NAME = "UI_EDITOR_TEXTURE_";
            
            /// <summary>
            /// The path of the texture folder.
            /// This is relative to the <see cref="Skin.SkinState.EditorPath"/>.
            /// </summary>
            public const string TEXTURE_FOLDER = "UI/Textures/Generated";
        // -- Private Attributes --
            /// <summary>
            /// Cache of all the loaded textures.
            /// </summary>
            private static System.Collections.Generic.Dictionary<string, UnityEngine.Texture2D> _msTextureCache =
                new System.Collections.Generic.Dictionary<string, UnityEngine.Texture2D>(); 
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Creates a new simple texture.
            /// Loads it from cache if its was already generated.
            /// The cache is busted if the <see cref="colour"/> is different.
            /// </summary>
            /// <param name="name">The name of the texture in the cache.</param>
            /// <param name="colour">The expected color of the texture.</param>
            /// <returns>The generated texture object.</returns>
            public static UnityEngine.Texture2D CreateSimple(string name, UnityEngine.Color colour) {
                // Search in the cache if the texture exists.
                UnityEngine.Texture2D cachedTexture = TextureGenerator._FindTextureInCache(name: name);
                if (TextureGenerator._ValidateColours(texture: cachedTexture, innerColour: colour)) {
                    // Return the cached texture.
                    return cachedTexture;
                }
                
                // Search on disk if the texture exists.
                UnityEngine.Texture2D diskTexture = TextureGenerator._FindTextureOnDisk(name: name);
                if (TextureGenerator._ValidateColours(texture: diskTexture, innerColour: colour)) {
                    // Cache the texture.
                    TextureGenerator._msTextureCache.Add(key: name, value: diskTexture);
                    
                    // Return the disk texture.
                    return diskTexture;
                }
                
                // Create a new texture.
                UnityEngine.Texture2D generatedTexture = TextureGenerator.CreateSimple(colour: colour);
                
                // Store the texture in the cache.
                TextureGenerator._msTextureCache[key: name] = generatedTexture;
                
                // Store the texture on the disk.
                UnityEditor.AssetDatabase.CreateAsset(
                    asset: generatedTexture, 
                    path: $"{Skin.SkinState.EditorPath}/" +
                          $"{TextureGenerator.TEXTURE_FOLDER}/" +
                          $"{TextureGenerator.TEXTURE_BASE_NAME}{name}.asset"
                );
                
                // Return the texture.
                return generatedTexture;
            }
            
            /// <summary>
            /// Generates a new simple texture object.
            /// The texture will be 1x1 pixels in size and have
            /// the color specified by the <see cref="colour"/> argument.
            /// </summary>
            /// <param name="colour">The colour that the texture must have.</param>
            /// <returns>The generated texture.</returns>
            public static UnityEngine.Texture2D CreateSimple(UnityEngine.Color colour) {
                // Create a new texture.
                UnityEngine.Texture2D texture = new UnityEngine.Texture2D(
                    width: 1, 
                    height: 1,
                    textureFormat: UnityEngine.Mathf.Approximately(a: colour.a, b: 1f) ? 
                        UnityEngine.TextureFormat.RGB24 : 
                        UnityEngine.TextureFormat.ARGB32,
                    mipChain: false
                );
                
                // Set its colour.
                texture.SetPixel(x: 0, y: 0, color: colour);
                
                // Set its wrap and filter modes.
                texture.wrapMode = UnityEngine.TextureWrapMode.Repeat;
                texture.filterMode = UnityEngine.FilterMode.Point;
                
                // Apply the changes.
                texture.Apply();
                
                // Return the texture.
                return texture;
            }

            /// <summary>
            /// Creates a new rectangle texture.
            /// Loads it from cache if its was already generated.
            /// The cache is busted if any argument is different from the stored version.
            /// </summary>
            /// <param name="name">The name of the texture in the cache.</param>
            /// <param name="innerColour">The colour used for the inside of the rectangle.</param>
            /// <param name="borderSize">The size of the border.</param>
            /// <param name="borderColour">The colour of the border.</param>
            /// <param name="borderRadius">(Optional) The radius of the rounded corners.</param>
            /// <returns>The generated texture object.</returns>
            public static UnityEngine.Texture2D CreateBordered(
                string name,
                UnityEngine.Color innerColour,
                float borderSize,
                UnityEngine.Color borderColour,
                float borderRadius = 0
            ) {
                // Search in the cache if the texture exists.
                UnityEngine.Texture2D cachedTexture = TextureGenerator._FindTextureInCache(name: name);
                if (TextureGenerator._ValidateColours(
                    texture: cachedTexture, 
                    innerColour: innerColour,
                    borderSize: borderSize,
                    borderColour: borderColour,
                    borderRadius: borderRadius
                )) {
                    // Return the cached texture.
                    return cachedTexture;
                }
                
                // Search on disk if the texture exists.
                UnityEngine.Texture2D diskTexture = TextureGenerator._FindTextureOnDisk(name: name);
                if (TextureGenerator._ValidateColours(
                    texture: diskTexture, 
                    innerColour: innerColour,
                    borderSize: borderSize,
                    borderColour: borderColour,
                    borderRadius: borderRadius
                )) {
                    // Cache the texture.
                    TextureGenerator._msTextureCache.Add(key: name, value: diskTexture);
                    
                    // Return the disk texture.
                    return diskTexture;
                }
                
                // Create a new texture.
                UnityEngine.Texture2D generatedTexture = TextureGenerator.CreateBordered(
                    innerColour: innerColour, 
                    borderSize: borderSize, 
                    borderColour: borderColour, 
                    borderRadius: borderRadius
                );
                
                // Store the texture in the cache.
                TextureGenerator._msTextureCache[key: name] = generatedTexture;
                
                // Store the texture on the disk.
                UnityEditor.AssetDatabase.CreateAsset(
                    asset: generatedTexture, 
                    path: $"{Skin.SkinState.EditorPath}/" +
                          $"{TextureGenerator.TEXTURE_FOLDER}/" +
                          $"{TextureGenerator.TEXTURE_BASE_NAME}{name}.asset"
                );
                
                // Return the texture.
                return generatedTexture;
            }
            
            /// <summary>
            /// Generates a new bordered texture object.
            /// The texture will be <see cref="borderRadius"/> + <see cref="borderSize"/> + <code>1</code>
            /// pixels high and wide.
            /// </summary>
            /// <param name="innerColour">The colour used for the inside of the texture.</param>
            /// <param name="borderSize">The size (in pixels) of the border.</param>
            /// <param name="borderColour">The colour of the border.</param>
            /// <param name="borderRadius">(Optional) The radius of the rounded corners.</param>
            /// <returns>The generated texture.</returns>
            public static UnityEngine.Texture2D CreateBordered(
                UnityEngine.Color innerColour,
                float borderSize,
                UnityEngine.Color borderColour,
                float borderRadius = 0
            ) {
                // Get some helper properties.
                int b = UnityEngine.Mathf.RoundToInt(f: borderSize);
                int r = UnityEngine.Mathf.RoundToInt(f: borderRadius);
                int w = 2 * b + 2 * r + 1;
                float d;
                
                UnityEngine.Color iCol = innerColour;
                UnityEngine.Color bCol = borderColour;
                UnityEngine.Color oCol = UnityEngine.Color.clear;
                
                // Create a new texture.
                UnityEngine.Texture2D texture = new UnityEngine.Texture2D(
                    width: w, 
                    height: w,
                    textureFormat: UnityEngine.TextureFormat.ARGB32,
                    mipChain: false
                );
                
                // Loop through the texture.
                for (int x = 0; x < w; x++) for (int y = 0; y < w; y++) {
                    // Get the distance of x and y relative to the center of the corner's radius.
                    int dX = x < r ? r - x : x > w - r ? r - (w - x) : -1;
                    int dY = y < r ? r - y : y > w - r ? r - (w - y) : -1;
                    
                    // Check if both x and y are not -1.
                    if (dX != -1 && dY != -1) {
                        // Get the distance to that center.
                        d = new UnityEngine.Vector2(x: dX, y: dY).magnitude; 
                        
                        // Get the color to apply at that location.
                        texture.SetPixel(x: x, y: y, color: TextureGenerator._GetColorFromDistance(
                            d: d, r: r, b: b, iCol: iCol, bCol: bCol, oCol: oCol
                        ));
                    // Check if we are on a border.
                    } else if (x < b || y < b || x >= w - b || y >= w - b) {
                        // Set the border color.
                        texture.SetPixel(x: x, y: y, color: bCol);
                    // If we are in the center of the texture.
                    } else {
                        // Set the inner color.
                        texture.SetPixel(x: x, y: y, color: iCol);
                    }
                }
                
                // Set its wrap and filter modes.
                texture.wrapMode = UnityEngine.TextureWrapMode.Repeat;
                texture.filterMode = UnityEngine.FilterMode.Point;
                
                // Apply the changes.
                texture.Apply();
                
                // Return the texture.
                return texture;
            }
            
        // -- Private Methods --
            /// <summary>
            /// Finds the specified texture in the cache.
            /// </summary>
            /// <param name="name">The name of the cached texture.</param>
            /// <returns>The texture object if it exists, otherwise a <code>null</code> reference.</returns>
            [JetBrains.Annotations.CanBeNullAttribute]
            private static UnityEngine.Texture2D _FindTextureInCache(string name) {
                // Search for the texture in the cache then return it.
                return TextureGenerator._msTextureCache.ContainsKey(key: name) ? 
                    TextureGenerator._msTextureCache[key: name] : 
                    null;
            }
            
            /// <summary>
            /// Searches for the specified texture on disk.
            /// </summary>
            /// <param name="name">The name of the texture.</param>
            /// <returns>The texture if it was found, a <code>null</code> reference otherwise.</returns>
            [JetBrains.Annotations.CanBeNullAttribute]
            private static UnityEngine.Texture2D _FindTextureOnDisk(string name) {
                // Search for the GUID of the texture.
                string[] textureGuid = UnityEditor.AssetDatabase.FindAssets(
                    filter: $"{TextureGenerator.TEXTURE_BASE_NAME}{name} t:{nameof(UnityEngine.Texture2D)}",
                    searchInFolders: new [] {
                        $"{Skin.SkinState.EditorPath}/{TextureGenerator.TEXTURE_FOLDER}"
                    }
                );
                
                // Check if a texture object was found, and return it.
                return textureGuid.Length > 0 ?
                    UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(
                        assetPath: UnityEditor.AssetDatabase.GUIDToAssetPath(guid: textureGuid[0])
                    ) :
                    null;
            }
            
            /// <summary>
            /// Selects which colour should be applied based on the supplied parameters.
            /// </summary>
            /// <param name="d">The distance to the circle's center.</param>
            /// <param name="r">The radius of the circle.</param>
            /// <param name="b">The size of the border.</param>
            /// <param name="oCol">The colour of the outside.</param>
            /// <param name="bCol">The colour of the border.</param>
            /// <param name="iCol">The colour of the inside.</param>
            /// <returns></returns>
            private static UnityEngine.Color _GetColorFromDistance(
                float d,
                float r, 
                float b,
                UnityEngine.Color iCol, 
                UnityEngine.Color bCol, 
                UnityEngine.Color oCol 
            ) {
                // Check if we are on the inside of the object.
                if (d <= r - b) {
                    return iCol;
                // Check if we are on the border.
                } else if (d <= r) {
                    return bCol;
                // Check if we are on the outside of the object.
                } else {
                    return oCol;
                }
            }
            
            /// <summary>
            /// Validates the colours of the specified texture.
            /// Checks both the border and the inner color.
            /// </summary>
            /// <param name="texture">The texture element to check.</param>
            /// <param name="innerColour">The expected color of the center of the texture.</param>
            /// <param name="borderSize">The expected size of the border.</param>
            /// <param name="borderColour">The expected colour of the border.</param>
            /// <param name="borderRadius">The expected radius of the border.</param>
            /// <returns>True if the texture is valid according to the specified arguments.</returns>
            private static bool _ValidateColours(
                [JetBrains.Annotations.CanBeNullAttribute]UnityEngine.Texture2D texture, 
                UnityEngine.Color innerColour, 
                float borderSize = 0, 
                UnityEngine.Color borderColour = default,
                float borderRadius = 0
            ) {
                // Check if the texture is set.
                if (texture == null) { return false; }
                
                // Check the color at the center of the texture.
                if (texture.GetPixel(
                    x: UnityEngine.Mathf.FloorToInt(f: texture.width  / 2f),
                    y: UnityEngine.Mathf.FloorToInt(f: texture.height / 2f)
                ) != innerColour) {
                    return false;
                }
                
                // Check the bottom border.
                int botX = UnityEngine.Mathf.FloorToInt(f: texture.width  / 2f);
                for (int botY = 0; botY < borderSize; botY++) {
                    if (texture.GetPixel(x: botX, y: botY) != borderColour) {
                        return false;
                    }
                }
                
                // Check the border radius of the bottom left corner.
                for (int bot = 0; bot < borderRadius; bot++) {
                    // Get the distance to the center of the circle.
                    float d = UnityEngine.Mathf.Pow(f: borderRadius - bot, p: 2);
                    d = UnityEngine.Mathf.Sqrt(f: 2 * d);
                    d *= UnityEngine.Mathf.Sign(f: borderRadius - bot);
                    
                    // Check the colour of the specified pixel.
                    if (texture.GetPixel(x: bot, y: bot) != TextureGenerator._GetColorFromDistance(
                        d: d, 
                        r: borderRadius, 
                        b: borderSize, 
                        iCol: innerColour, 
                        bCol: borderColour, 
                        oCol: UnityEngine.Color.clear
                    )) {
                        return false;
                    }
                }
                
                // If the function is still running, return a success.
                return true;
            }
    // --- /Methods ---
}
}