using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NicksAdventure7;
using NicksAdventure7.GameCore.Model.Graphics;
using NicksAdventure7.GameCore.Scenes;
using NicksAdventure7.GameCore.Model.Entities.Terrain;
using NicksAdventure7.GameCore.Model.Entities;
using NicksAdventure7.GameCore.Util;

namespace NicksAdventure7.GameCore.Scenes{
    class EditorScene : NicksAdventure7.GameCore.Scenes.GameScene{

        public EditorScene(NicksAdventure7.Runner core){
            this.core = core;
            kind = Scene.EDITOR;
            components = new HashSet<Component>();
            Content = Global.Content;
            running = false;
        }
        public void Initialize(){
                Global.camera.X = 0;
                Global.camera.Y = 0;
                //the first two indices are obviously x and y, although the third is NOT z!
                //the third stores any number of tiles when z can be queryed.
                //Note that this is not inefficient for the GraphicsCard since
                //the TileMap takes the three-dimensional and skips the null cells
                //when copying the vertices.
        }
        public override void Activate(){
        }
        public override void Suspend(){
        }
        public void Resize(){
        }
        public override void Update(GameTime gameTime){
           
        }
        public override void Draw(SpriteBatch spriteBatch){
        }
    }
}
