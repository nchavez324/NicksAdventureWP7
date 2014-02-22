using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NicksAdventure7;
using NicksAdventure7.GameCore.Model.Graphics;
using NicksAdventure7.GameCore.Scenes;
using NicksAdventure7.GameCore.Util;

namespace NicksAdventure7.GameCore.Scenes{
    class IntroScene : NicksAdventure7.GameCore.Scenes.GameScene{
        public IntroScene(NicksAdventure7.Runner core){
            this.core = core;
            kind = Scene.INTRO;
            components = new HashSet<Component>();
            Content = Global.Content;
            running = false;
        }
        public void Initialize(){
                Global.camera.X = 0;
                Global.camera.Y = 0;
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
            spriteBatch.GraphicsDevice.Clear(Color.Green);
            for (int i = 0; i < components.Count; i++)
                components.ElementAt(i).Draw(spriteBatch);

            SpriteFont font = Global.Content.Load<SpriteFont>("Fonts/font");
            spriteBatch.DrawString(font, "IntroScene", new Vector2(0, Global.paddleHeight), Color.White);
        }
    }
}
