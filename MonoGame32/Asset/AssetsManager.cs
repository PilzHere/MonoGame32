using System;
using System.Net.Mime;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame32.Asset
{
    public class AssetsManager
    {
        private ContentManager _contentManager;

        public AssetsManager(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public void LoadAllAssets()
        {
            Console.WriteLine("Load all assets.");
            LoadTextures();
        }

        public string DefaultTexture = "Textures/default";
        
        private void LoadTextures()
        {
            _contentManager.Load<Texture2D>(DefaultTexture);
        }

        public Texture2D GetTexture(string location)
        {
            // If it's not loaded yet, it willl load.
            // If it IS loaded, it will reuse the loaded.
            //Texture2D texture2D = _contentManager.Load<Texture2D>(location);

            return _contentManager.Load<Texture2D>(location);
        }

        public void Dispose()
        {
            Console.WriteLine("Unloading all assets.");
            _contentManager.Unload(); // Disposes all loaded assets.

            _contentManager.Dispose();
        }
    }
}