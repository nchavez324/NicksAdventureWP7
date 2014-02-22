using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using NicksAdventure7.GameCore.Model.Graphics;
using NicksAdventure7;
using NicksAdventure7.GameCore.Util;

namespace NicksAdventure7.GameCore.Scenes{
    public enum Scene{
        LOAD, INTRO, ACTION, EDITOR
    }

    abstract class GameScene{
        public Runner core;
        public Scene kind;
        public HashSet<Component> components;
        public ContentManager Content;
        public bool running;

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
        public abstract void Activate();
        public abstract void Suspend();
        public void Stop() { running = false; }
        public void SwitchScenes(Scene newScene) { core.SwitchScenes(newScene); }
    }
}
