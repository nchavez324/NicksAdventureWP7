using NicksAdventure7.GameCore.Model.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using NicksAdventure7.GameCore.Model.Entities.Events;
using NicksAdventure7.GameCore.Model.Entities.Terrain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NicksAdventure7.GameCore.Model.Entities{
    public class OWPlayer : Character{
        public OWPlayer(string texturePath, int[] mapPos, Tile.Direction dir) : base(texturePath, mapPos, dir, "Player"){
            this.texturePath = texturePath;
            mapX = mapPos[0];
            mapY = mapPos[1];
            mapZ = mapPos[2];
            SetDir(dir);
        }
        public void AButton(){
            SoundEffect s = Global.Content.Load<SoundEffect>("Audio/SFX/A");
            if (Global.action.dialog.visible){
                Global.action.dialog.Next();
                s.Play();
            }
            else if (travel.X == 0 && travel.Y == 0){
                IEvent e = null;
                if (GetDir() == Tile.Direction.NORTH){
                    e = Global.action.tileMap.GetEvent(mapX, mapY + 1, mapZ);
                }else if (GetDir() == Tile.Direction.SOUTH){
                    e = Global.action.tileMap.GetEvent(mapX, mapY - 1, mapZ);
                }else if (GetDir() == Tile.Direction.EAST){
                    e = Global.action.tileMap.GetEvent(mapX + 1, mapY, mapZ);
                }else if (GetDir() == Tile.Direction.WEST){
                    e = Global.action.tileMap.GetEvent(mapX - 1, mapY, mapZ);
                }
                if (e != null && !e.Moving()){
                    e.Trigger(this);
                    s.Play();
                }
            }
        }
        public void BButton(){

        }
        public new void Move(Tile.Direction dir){
            //DIRECT CLONE OF CHARACTER MOVE -- KEEP UPDATED
            if (travel.X == 0 && travel.Y == 0){
                this.dir = dir;
                animation = animations[dir.ToString()];
                if (GetTileCollision() && GetBuildingCollision() && GetEventCollision()){
                    if (!animation.running) animation.Start();
                    else animation.running = true;
                    if (dir == Tile.Direction.NORTH){
                        travel.Y = +0.01f;
                        Global.action.tileMap.UpdateEventGrid(this, GetMapPos(), new int[2] { GetMapPos()[0], GetMapPos()[1] + 1 });
                    }
                    if (dir == Tile.Direction.SOUTH){
                        travel.Y = -0.01f;
                        Global.action.tileMap.UpdateEventGrid(this, GetMapPos(), new int[2] { GetMapPos()[0], GetMapPos()[1] - 1 });
                    }
                    if (dir == Tile.Direction.EAST){
                        travel.X = +0.01f;
                        Global.action.tileMap.UpdateEventGrid(this, GetMapPos(), new int[2] { GetMapPos()[0] + 1, GetMapPos()[1] });
                    }
                    if (dir == Tile.Direction.WEST){
                        travel.X = -0.01f;
                        Global.action.tileMap.UpdateEventGrid(this, GetMapPos(), new int[2] { GetMapPos()[0] - 1, GetMapPos()[1] });
                    }
                }
            }
        }
        public new void DebugDraw(SpriteBatch spriteBatch){
        }
        protected new bool GetEventCollision(){
            Tile.Direction dir = GetDir();
            IEvent e = null;
            if (dir == Tile.Direction.NORTH){
                e = Global.action.tileMap.GetEvent(mapX, mapY + 1, mapZ);
            }if (dir == Tile.Direction.SOUTH){
                e = Global.action.tileMap.GetEvent(mapX, mapY - 1, mapZ);
            }if (dir == Tile.Direction.EAST){
                e = Global.action.tileMap.GetEvent(mapX + 1, mapY, mapZ);
            }if (dir == Tile.Direction.WEST){
                e = Global.action.tileMap.GetEvent(mapX - 1, mapY, mapZ);
            }
            return (e == null || (e != null && e.GetPassage()));
        }
        public new int[] GetMapPos(){
            return new int[3] { mapX, mapY, mapZ};
        }
    }
}
