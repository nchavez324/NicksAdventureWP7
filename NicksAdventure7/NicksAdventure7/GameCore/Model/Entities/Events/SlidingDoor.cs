using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NicksAdventure7.GameCore.Model.Entities.Terrain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NicksAdventure7.GameCore.Model.Entities.Events{
    class SlidingDoor : Door{
        float percentOpen;
        int state = 0;

        public SlidingDoor(string texturePath, int[] mapPos, int[] destPos, Tile.Direction dir, Tile.Direction playerDir, string mapDestKey){
            this.texturePath = texturePath;
            mapX = mapPos[0];
            mapY = mapPos[1];
            mapZ = mapPos[2];
            this.destPos = destPos;
            this.dir = dir;
            this.playerDir = playerDir;
            this.mapDestKey = mapDestKey;
            passage = true;
        }
        public SlidingDoor(string texturePath, int[] mapPos, int[] destPos, Tile.Direction dir, Tile.Direction playerDir, Rectangle section, string mapDestKey) : this(texturePath, mapPos, destPos, dir, playerDir, mapDestKey){
            this.section = section;
        }
        public override void Initialize(){
            MiniInit();

            for (int i = 0; i < vertices.Length; i++){
                VertexPositionNormalTexture vertex = vertices[i];

                if (dir == Tile.Direction.NORTH){
                    vertex.Position.X = (i > 1) ? mapX * Tile.SIZE : (mapX + 1) * Tile.SIZE;
                    vertex.Position.Y = (mapY + 1) * Tile.SIZE;
                }
                if (dir == Tile.Direction.SOUTH){
                    vertex.Position.X = (i <= 1) ? mapX * Tile.SIZE : (mapX + 1) * Tile.SIZE;
                    vertex.Position.Y = mapY * Tile.SIZE;
                }
                if (dir == Tile.Direction.EAST){
                    vertex.Position.X = (mapX + 1) * Tile.SIZE;
                    vertex.Position.Y = (i <= 1) ? mapY * Tile.SIZE : (mapY + 1) * Tile.SIZE;
                }
                if (dir == Tile.Direction.WEST){
                    vertex.Position.X = (mapX * Tile.SIZE);
                    vertex.Position.Y = (i > 1) ? mapY * Tile.SIZE : (mapY + 1) * Tile.SIZE;
                }
                vertex.Position.Z = (i % 2 == 0) ? mapZ * Tile.HEIGHT : (mapZ + 1) * Tile.HEIGHT;

                vertices[i] = vertex;
            }

            if (section.Width == 0)
                section = new Rectangle(0, 0, texture.Width, texture.Height);

            percentOpen = 1;

            UpdateVerts(null);
        }
        public override void Update(GameTime gameTime, OWPlayer player){
            UpdateTrigger(player);
            float old = percentOpen;
            UpdateMotion(gameTime);
            if(old != percentOpen)
                UpdateVerts(gameTime);
        }
        public void UpdateTrigger(OWPlayer player){
            if (player.GetMapPos()[Character.Z] == mapZ && (((dir == Tile.Direction.NORTH || dir == Tile.Direction.SOUTH) && player.GetMapPos()[Character.X] == mapX) || (dir == Tile.Direction.WEST && player.GetMapPos()[Character.X] + 1 == mapX) || (dir == Tile.Direction.EAST && player.GetMapPos()[Character.X] - 1 == mapX)) && (((dir == Tile.Direction.WEST || dir == Tile.Direction.EAST) && player.GetMapPos()[Character.Y] == mapY) || (dir == Tile.Direction.NORTH && player.GetMapPos()[Character.Y] - 1 == mapY) || (dir == Tile.Direction.SOUTH && player.GetMapPos()[Character.Y] + 1 == mapY)))
            {
                state = 1;
            }else{
                state = -1;
            }
            if (player.GetMapPos()[Character.X] == mapX && player.GetMapPos()[Character.Y] == mapY && player.GetMapPos()[Character.Z] == mapZ && ((player.GetDir() == Tile.Direction.NORTH && dir == Tile.Direction.SOUTH) || (player.GetDir() == Tile.Direction.SOUTH && dir == Tile.Direction.NORTH) || (player.GetDir() == Tile.Direction.EAST && dir == Tile.Direction.WEST) || (player.GetDir() == Tile.Direction.WEST && dir == Tile.Direction.EAST)))
            {
                state = -1;
                Global.action.SetTileMap(mapDestKey, destPos, (playerDir != Tile.Direction.NONE)?playerDir:player.GetDir());
                player.Move(playerDir);
            }
            if (player.GetDir() == dir && player.GetMapPos()[Character.X] == mapX && player.GetMapPos()[Character.Y] == mapY && player.GetMapPos()[Character.Z] == mapZ)
            {
                state = 1;
            }
        }
        protected override void UpdateVerts(GameTime gameTime){
            for (int i = 0; i < vertices.Length; i++){
                VertexPositionNormalTexture vertex = vertices[i];
                if (dir == Tile.Direction.NORTH){
                    vertex.Position.X = ((i > 1) ? mapX * Tile.SIZE : (mapX + 1) * Tile.SIZE);
                }
                if (dir == Tile.Direction.SOUTH){
                    vertex.Position.X = ((i <= 1) ? (mapX * Tile.SIZE) : (mapX + percentOpen) * Tile.SIZE);
                }
                if (dir == Tile.Direction.EAST){
                    vertex.Position.Y = ((i <= 1) ? mapY * Tile.SIZE : (mapY + 1) * Tile.SIZE);
                }
                if (dir == Tile.Direction.WEST){
                    vertex.Position.Y = ((i > 1) ? mapY * Tile.SIZE : (mapY + 1) * Tile.SIZE);
                }
                vertices[i] = vertex;
            }
            vertices[0].TextureCoordinate = new Vector2(section.X/texture.Width, section.Y/texture.Height);
            vertices[1].TextureCoordinate = new Vector2(section.X/texture.Width, (section.Y + section.Height)/texture.Height);
            vertices[2].TextureCoordinate = new Vector2((section.X + (percentOpen*section.Width))/texture.Width, section.Y/texture.Height);
            vertices[3].TextureCoordinate = new Vector2((section.X + (percentOpen * section.Width)) / texture.Width, (section.Y + section.Height) / texture.Height);
        }
        protected override void UpdateMotion(GameTime gameTime){
            if (percentOpen <= 1 && percentOpen >= 0)
                percentOpen -= state * 0.15f;
            if (percentOpen > 1) { percentOpen = 1; state = 0; }
            if (percentOpen < 0) { percentOpen = 0; state = 0; }
        }
        public override void DebugDraw(SpriteBatch spriteBatch){
        }
    }

}
