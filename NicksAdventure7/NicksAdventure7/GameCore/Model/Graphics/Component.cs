using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace NicksAdventure7.GameCore.Model.Graphics{
    interface Component{
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
    }
}