using System;
using UnityEditor;
using UnityEngine;
using System.IO;

public class KSPTextureTools : AssetPostprocessor
{
    // what file type to allow
    private const string EXTENSION = ".png";

    
    /* SPECULAR */
    // enable specular map copying
    private const bool SPEC_ENABLE = true;

    // what the file name should end with for specular maps
    private const string SPEC_TAG = "-spec";


    /* NORMALS */
    // enable normal map processing 
    private const bool NORMAL_ENABLE = true;
    
    // what the file name should end with for normal maps
    private const string NORMAL_TAG = "-n";

    // enable normal map generation from heightmaps
    private const bool NORMAL_GENERATE = true;
    
   

    private Texture2D _otherTexture = null;
    private bool _isSpecular = false;
    private bool _hasSpecular = false;

    void OnPreprocessTexture()
    {
        var importer = (TextureImporter) assetImporter;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.isReadable = true;

        var extension = Path.GetExtension(assetPath);
        if (extension != EXTENSION) return;

        var basename = Path.GetFileNameWithoutExtension(assetPath);
        var directory = Path.GetDirectoryName(assetPath);
        
        if (NORMAL_ENABLE)
        {
            if (basename.EndsWith(NORMAL_TAG))
            {
                importer.textureType = TextureImporterType.NormalMap;
                if (NORMAL_GENERATE) importer.convertToNormalmap = true;
            }
        }
        
        if (SPEC_ENABLE) {
            if (basename.EndsWith(SPEC_TAG))
            {
                var othername = basename.Replace(SPEC_TAG, "");
                var otherpath = directory + "/" + othername + extension;
                _otherTexture = (Texture2D) AssetDatabase.LoadAssetAtPath<Texture2D>(otherpath);
                if (_otherTexture == null)
                {
                    LogError("Diffuse file " + otherpath + " not found. Aborting");
                    return;
                }

                _isSpecular = true;
                _hasSpecular = false;
            }
            else
            {
                var othername = basename + SPEC_TAG;
                var otherpath = directory + "/" + othername + extension;
                _otherTexture = (Texture2D) AssetDatabase.LoadAssetAtPath<Texture2D>(otherpath);
                if (_otherTexture == null)
                {
                    return;
                }

                _hasSpecular = true;
                importer.alphaSource = TextureImporterAlphaSource.FromGrayScale;
            }
        }
    }

    void OnPostprocessTexture(Texture2D texture)
    {

        if (_isSpecular)
        {
            CopyAlphaChannel(texture, _otherTexture);
            return;
        }

        if (_hasSpecular)
        {
            CopyAlphaChannel(_otherTexture, texture);
            return;
        }
        AssetDatabase.SaveAssets();
    }

    private void CopyAlphaChannel(Texture2D source, Texture2D target)
    {
        var width = source.width;
        var height = source.height;
        var depth = source.mipmapCount;

        if (width != target.width || height != target.height)
        {
            LogError("Specular and Diffuse texture sizes to not match in texture " + assetPath);
            return;
        }

        for (int level = 0; level < depth; level++)
        {
            var sourcePixels = source.GetPixels32(level);
            var targetPixels = target.GetPixels32(level);

            var length = sourcePixels.Length;

            for (var i = 0; i < length; i++)
            {
                targetPixels[i].a = sourcePixels[i].r;
            }

            target.SetPixels32(targetPixels, level);
        }

        target.Apply();
    }
}