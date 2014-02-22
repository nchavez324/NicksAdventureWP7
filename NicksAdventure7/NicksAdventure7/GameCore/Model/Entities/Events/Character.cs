using NicksAdventure7.GameCore.Model.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NicksAdventure7.GameCore.Model.Entities.Terrain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NicksAdventure7.GameCore.Model.Entities.Events{
    public class Character : BasicEvent{
        protected readonly static float speed = 0.003f;
        protected Tile.Direction dir;

        public Character(string texturePath, int[] mapPos, Tile.Direction dir, string name){
            this.texturePath = texturePath;
            mapX = mapPos[0];
            mapY = mapPos[1];
            mapZ = mapPos[2];
            this.dir = dir;
            this.name = name;
        }
        public override void Initialize(){
            MiniInit();
            
            for (int i = 0; i < vertices.Length; i++){
                VertexPositionNormalTexture vertex = vertices[i];

                vertex.Position.X = ((i > 1) ? (mapX + 1) * Tile.SIZE : mapX * Tile.SIZE);
                vertex.Position.Y = ((i % 2 != 0) ? (mapY + 1) * Tile.SIZE : mapY * Tile.SIZE);
                vertex.Position.Z = ((i % 2 != 0) ? (mapZ * Tile.HEIGHT) + HEIGHT : (mapZ) * Tile.HEIGHT);

                vertices[i] = vertex;
            }
            //config animations
            animations = new Dictionary<string, AdvancedAnimation>();
            for (int i = 0; i < 4; i++){
                AdvancedAnimation a = new AdvancedAnimation(texture, new Rectangle(0, i * (texture.Height / 4), texture.Width, texture.Height / 4), 4);
                a.frameTime = (i <= (int)Tile.Direction.SOUTH) ? 75 : 90;
                a.Start();
                animations[Enum.GetName(typeof(Tile.Direction), i)] = a;
            }
            animation = animations[dir.ToString()];
            
            animation.running = false;
            UpdateUVCoords();
        }
        public override void Update(GameTime gameTime, OWPlayer player){
            UpdateMotion(gameTime);
            UpdateVerts(gameTime);
            if (animation != null){
                int frameNum = animation.sceneIndex;
                animation.Update(gameTime);
                UpdateUVCoords();
            }
        }
        /// <summary>
        /// Updates the UVCoords of the vertices so that the correct frame of the animation is applied.
        /// </summary>
        protected override void UpdateVerts(GameTime gameTime){
            for (int i = 0; i < vertices.Length; i++){
                VertexPositionNormalTexture vertex = vertices[i];
                vertex.Position.X = ((i > 1) ? (mapX + 1 + travel.X) * Tile.SIZE : (mapX + travel.X) * Tile.SIZE);
                vertex.Position.Y = (i % 2 != 0) ? (mapY + 1 + travel.Y) * Tile.SIZE : (mapY + travel.Y) * Tile.SIZE;
                vertex.Position.Z = (i % 2 != 0) ? (mapZ * Tile.HEIGHT) + HEIGHT : (mapZ) * Tile.HEIGHT;
                vertices[i] = vertex;
            }
        }
        protected override void UpdateMotion(GameTime gameTime){
            if (travel.X != 0 || travel.Y != 0){
                //we got motion!
                if (dir == Tile.Direction.NORTH){
                    travel.Y += gameTime.ElapsedGameTime.Milliseconds * speed;
                    travel.X = 0;
                }
                if (dir == Tile.Direction.SOUTH){
                    travel.Y -= gameTime.ElapsedGameTime.Milliseconds * speed;
                    travel.X = 0;
                }
                if (dir == Tile.Direction.EAST){
                    travel.X += gameTime.ElapsedGameTime.Milliseconds * speed;
                    travel.Y = 0;
                }
                if (dir == Tile.Direction.WEST){
                    travel.X -= gameTime.ElapsedGameTime.Milliseconds * speed;
                    travel.Y = 0;
                }
                if (Math.Abs(travel.X) >= 1f || Math.Abs(travel.Y) >= 1f){//um should check for continuous directionality
                    if (dir == Tile.Direction.NORTH){
                        mapY++;
                    }
                    if (dir == Tile.Direction.SOUTH){
                        mapY--;
                    }
                    if (dir == Tile.Direction.EAST){
                        mapX++;
                    }
                    if (dir == Tile.Direction.WEST){
                        mapX--;
                    }
                    travel.X = 0;
                    travel.Y = 0;
                    animation.PauseSet(0);
                }
            }
        }
        public override void Trigger(OWPlayer player){
        }
        /// <summary>
        /// Called when a directional button is being held.
        /// </summary>
        /// <param name="dir">The direction requested for motion</param>
        public void Move(Tile.Direction dir){
            //DIRECT CLONE IN OWPLAYER...
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
        public override void DebugDraw(SpriteBatch spriteBatch){  
        }
        public void SetDir(Tile.Direction dir){
            this.dir = dir;
            if (animations != null){
                animation = animations[dir.ToString()];
                animation.PauseSet(0);
            }
            travel = new Vector2(0f);
        }
        public Tile.Direction GetDir(){
            return dir;
        }
        //true if passage is allowed, false otherwise
        protected bool GetTileCollision(){
            return ((dir == Tile.Direction.NORTH && (Global.action.tileMap.GetTile(mapX, mapY + 1, mapZ) != null && Global.action.tileMap.GetTile(mapX, mapY + 1, mapZ).passage[(int)Tile.Direction.SOUTH])) ||
                    (dir == Tile.Direction.SOUTH && (Global.action.tileMap.GetTile(mapX, mapY - 1, mapZ) != null && Global.action.tileMap.GetTile(mapX, mapY - 1, mapZ).passage[(int)Tile.Direction.NORTH])) ||
                    (dir == Tile.Direction.EAST && (Global.action.tileMap.GetTile(mapX + 1, mapY, mapZ) != null && Global.action.tileMap.GetTile(mapX + 1, mapY, mapZ).passage[(int)Tile.Direction.WEST])) ||
                    (dir == Tile.Direction.WEST && (Global.action.tileMap.GetTile(mapX - 1, mapY, mapZ) != null && Global.action.tileMap.GetTile(mapX - 1, mapY, mapZ).passage[(int)Tile.Direction.EAST])));
        }
        protected bool GetBuildingCollision(){
            return (
                (dir == Tile.Direction.NORTH && !Global.action.tileMap.bCollMap[mapX, mapY + 1, mapZ]) ||
                (dir == Tile.Direction.SOUTH && !Global.action.tileMap.bCollMap[mapX, mapY - 1, mapZ]) ||
                (dir == Tile.Direction.EAST && !Global.action.tileMap.bCollMap[mapX + 1, mapY, mapZ]) ||
                (dir == Tile.Direction.WEST && !Global.action.tileMap.bCollMap[mapX - 1, mapY, mapZ])
                );
        }
        protected bool GetEventCollision(){
            int[] playerPos = Global.action.tileMap.player.GetMapPos();
            bool isPlayerThere = false;
            Tile.Direction dir = GetDir();
            IEvent e = null;
            if (dir == Tile.Direction.NORTH){
                e = Global.action.tileMap.GetEvent(mapX, mapY + 1, mapZ);
                isPlayerThere = (playerPos[BasicEvent.X] == mapX && playerPos[BasicEvent.Y] == mapY + 1 && playerPos[BasicEvent.Z] == mapZ);
            }
            if (dir == Tile.Direction.SOUTH){
                e = Global.action.tileMap.GetEvent(mapX, mapY - 1, mapZ);
                isPlayerThere = (playerPos[BasicEvent.X] == mapX && playerPos[BasicEvent.Y] == mapY - 1 && playerPos[BasicEvent.Z] == mapZ);
            }
            if (dir == Tile.Direction.EAST){
                e = Global.action.tileMap.GetEvent(mapX + 1, mapY, mapZ);
                isPlayerThere = (playerPos[BasicEvent.X] == mapX + 1 && playerPos[BasicEvent.Y] == mapY && playerPos[BasicEvent.Z] == mapZ);
            }
            if (dir == Tile.Direction.WEST){
                e = Global.action.tileMap.GetEvent(mapX - 1, mapY, mapZ);
                isPlayerThere = (playerPos[BasicEvent.X] == mapX - 1 && playerPos[BasicEvent.Y] == mapY && playerPos[BasicEvent.Z] == mapZ);
            }
            return !isPlayerThere && (e == null || (e != null && e.GetPassage() && !e.IsForPlayerOnly()));
        }
        public void SetMapPos(int[] pos){
            mapX = pos[Character.X];
            mapY = pos[Character.Y];
            mapZ = pos[Character.Z];
            travel = new Vector2(0f);
        }
        public override bool IsForPlayerOnly(){
            return false;
        }
    }
}
