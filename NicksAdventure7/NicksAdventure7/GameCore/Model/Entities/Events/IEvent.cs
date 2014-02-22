using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NicksAdventure7.GameCore.Model.Entities.Events{
    public interface IEvent{

        void Initialize();
        VertexPositionNormalTexture[] GetVertices();
        void Update(GameTime gameTime, OWPlayer player);
        Texture2D GetTexture();
        bool GetPassage();
        int[] GetMapPos();
        void DebugDraw(SpriteBatch spriteBatch);
        void Trigger(OWPlayer player);
        string GetName();
        bool IsForPlayerOnly();
        bool Moving();
    }
}
