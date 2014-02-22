using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NicksAdventure7.GameCore.Model.Graphics;
using NicksAdventure7.GameCore.Scenes;

using NicksAdventure7;
using NicksAdventure7.GameCore.Util;

namespace NicksAdventure7.GameCore.Scenes{
    class LoadScene : GameScene{
        Texture2D logo;
        float scale;
         public LoadScene(NicksAdventure7.Runner core){
            this.core = core;
            kind = Scene.LOAD;
            components = new HashSet<Component>();
            Content = Global.Content;
            running = false;
        }
        public void Initialize(){
                Global.camera.X = 0;
                Global.camera.Y = 0;
                logo = Content.Load<Texture2D>("Images/UI/companyLogo");
                float w = Global.camera.Width / logo.Width;
                float h = Global.camera.Height / logo.Height;
                scale = (w < h) ? w : h;
        }
        public override void Activate(){
        }
        public override void Suspend(){
        }
        public override void Update(GameTime gameTime){
            for (int i = 0; i < components.Count; i++)
                components.ElementAt(i).Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch){
            spriteBatch.GraphicsDevice.Clear(Color.Black);
            //spriteBatch.Draw(logo, new Rectangle((int)((Global.camera.Width - (logo.Width * scale)) / 2f), (int)((Global.camera.Height - (logo.Height * scale)) / 2f), (int)(logo.Width * scale), (int)(logo.Height * scale)), Color.White);
        }

    }
}
