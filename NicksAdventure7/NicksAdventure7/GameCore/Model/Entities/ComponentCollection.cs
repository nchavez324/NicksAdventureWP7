using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NicksAdventure7.GameCore.Model.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NicksAdventure7.GameCore.Util;

namespace NicksAdventure7.GameCore.Model.Entities.Collections{
    class ComponentCollection : Component{
        public HashSet<Component> components;

        public ComponentCollection(){
            components = new HashSet<Component>();
        }
        public void Add(Component c){
            components.Add(c);
        }
        public void Remove(Component c){
            components.Remove(c);
        }
        public void Update(GameTime gameTime){
            for (int i = 0; i < components.Count; i++)
                components.ElementAt(i).Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch){
            for (int i = 0; i < components.Count; i++)
                components.ElementAt(i).Draw(spriteBatch);
        }
    }
}
