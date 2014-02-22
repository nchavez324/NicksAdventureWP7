using Microsoft.Xna.Framework;
using NicksAdventure7.GameCore.Model.Entities.Terrain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NicksAdventure7.GameCore.Model.Entities.Events{
    public class Movement{
        public enum Type{//Implement these!
            FIXED, WALK_AROUND, LOOK_AROUND
        };
        public enum Trigger{
            ACTION, TOUCH, AUTO
        };
        public Type type;
        public Trigger trigger;
        public bool turnable;
        public int frequency;//Ranges from 1 to 5.
        public int freqTimer = -1;
        public static int freqTimeBase = 750;
        public Movement(Type type, Trigger trigger, bool turnable, int frequency){
            this.type = type;
            this.trigger = trigger;
            this.turnable = turnable;
            this.frequency = frequency;
        }
    };
    class EventPage : Character{
        Movement movement;
        public delegate void EventCommands(OWPlayer player);
        public EventCommands script;
        public bool touched = false;
        public bool sentRun = false;

        public EventPage(string texturePath, int[] mapPos, Tile.Direction dir, Movement movement, EventCommands script, string name)
            : base(texturePath, mapPos, dir, name){
                this.movement = movement;
                this.script = script;
        }
        public override void Trigger(OWPlayer player){
            if (movement.trigger == Movement.Trigger.ACTION){
                RunEventCommand(script, player);
            }
        }
        public void RunEventCommand(EventCommands commands, OWPlayer player){
            if (movement.turnable){
                if (player.GetDir() == Tile.Direction.NORTH)
                    SetDir(Tile.Direction.SOUTH);
                else if (player.GetDir() == Tile.Direction.SOUTH)
                    SetDir(Tile.Direction.NORTH);
                else if (player.GetDir() == Tile.Direction.EAST)
                    SetDir(Tile.Direction.WEST);
                else if (player.GetDir() == Tile.Direction.WEST)
                    SetDir(Tile.Direction.EAST);
            }
            sentRun = true;
            commands(player);
        }
        public new void Update(GameTime gameTime, OWPlayer player){
            base.Update(gameTime, player);
            if (movement.trigger == Movement.Trigger.AUTO){
                RunEventCommand(script, player);
            }
            if (movement.trigger == Movement.Trigger.TOUCH){
                bool touching = (player.travel.X == 0 && player.travel.Y == 0 && player.GetMapPos()[Character.Z] == mapZ && (
                    (player.GetDir() == Tile.Direction.NORTH && player.GetMapPos()[Character.X] == mapX && player.GetMapPos()[Character.Y] == mapY - 1) ||
                    (player.GetDir() == Tile.Direction.SOUTH && player.GetMapPos()[Character.X] == mapX && player.GetMapPos()[Character.Y] == mapY + 1) ||
                    (player.GetDir() == Tile.Direction.EAST && player.GetMapPos()[Character.X] == mapX - 1 && player.GetMapPos()[Character.Y] == mapY) ||
                    (player.GetDir() == Tile.Direction.WEST && player.GetMapPos()[Character.X] == mapX + 1 && player.GetMapPos()[Character.Y] == mapY)));
                if (!touching) touching = false;
                if (touching && !touched){
                    touched = true;
                    RunEventCommand(script, player);
                }
            }
            if (!Global.action.dialog.visible)
                sentRun = false;
            if(!sentRun)
                UpdateMovement(gameTime);
        }
        public void UpdateMovement(GameTime gameTime){
            if (movement.type != Movement.Type.FIXED){
                movement.freqTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (movement.freqTimer >= Movement.freqTimeBase / movement.frequency){
                    Random r = new Random(System.DateTime.Now.Millisecond % Int32.MaxValue * this.GetHashCode());
                    SetDir((Tile.Direction)(r.Next(4)));
                    movement.freqTimer = 0;
                }
            }
            if (movement.type == Movement.Type.WALK_AROUND)
              if (movement.freqTimer == 0){
                        Move(GetDir());
              }
        }
    }
}
