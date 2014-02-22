using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NicksAdventure7.GameCore.Model.Entities.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NicksAdventure7.GameCore.Model.Entities{
    public abstract class BasicEvent : Billboard, IEvent{
        public static int X = 0;
        public static int Y = 1;
        public static int Z = 2;
        public Vector2 travel; //limited from -1 to 1.
        protected int mapX, mapY, mapZ;
        public bool passage = false;
        public string name;

        public abstract void Initialize();
        public abstract void Update(GameTime gameTime, OWPlayer player);
        public abstract void Trigger(OWPlayer player);
        public abstract bool IsForPlayerOnly();
        public bool GetPassage(){
            return passage;
        }
        public int[] GetMapPos(){
            return new int[3] { mapX + ((travel.X > 0)?(1):((travel.X < 0)?(-1):(0))),
                mapY + ((travel.Y > 0)?(1):((travel.Y < 0)?(-1):(0))),
                mapZ};
        }
        public string GetName(){
            return name;
        }
        public bool Moving(){
            return !(travel.X == 0 && travel.Y == 0);
        }
    }
}
