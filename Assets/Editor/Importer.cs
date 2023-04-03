using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Assets.Editor
{
    internal class Importer : AssetPostprocessor
    {
        void OnPreprocessTexture()
        {
            TextureImporter importer = (TextureImporter)assetImporter;

            importer.mipmapEnabled = false;
            importer.filterMode = UnityEngine.FilterMode.Point;
        }
    }
}
