using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NicksAdventure7.GameCore.Model.Entities.Terrain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NicksAdventure7.GameCore.Model.Entities.Events{
    abstract class Door : BasicEvent{
        protected Tile.Direction dir, playerDir;
        protected Rectangle section;
        protected string mapDestKey;
        protected int[] destPos;
        public override void Trigger(OWPlayer player){
        }
        public override bool IsForPlayerOnly(){
            return true;
        }
    }
}
