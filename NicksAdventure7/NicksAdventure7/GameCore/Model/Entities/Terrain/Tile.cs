using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NicksAdventure7.GameCore.Model.Entities.Terrain{
    public class Tile{
        public enum TileStyle{
            FLAT, SLOPE,
            INNER_CORNER,
            OUTER_CORNER,
            VERTICAL, SV_CORNER
        };
        public enum Direction { NORTH, SOUTH, EAST, WEST, NONE};
        public static int SIZE = 2;
        public static float HEIGHT = 2.5f;

        VertexPositionNormalTexture[] vertices;
        Texture2D texture;
        public string texturePath;
        public TileStyle style;
        public int mapX, mapY, mapZ;
        public bool[] passage;
        public Direction dir, secondDir;

        public Tile(TileStyle style, string texturePath, int[] mapPos, bool[] passage, Nullable<Direction> dir, Nullable<Direction> secondDir){
            if (style == TileStyle.FLAT){
                this.style = style;
            }else if (style == TileStyle.SLOPE || style == TileStyle.VERTICAL){
                if (dir == null) throw new ArgumentNullException("dir", "Cannot instantiate SLOPE tile without any orientation.");
                this.style = style;
                this.dir = (Direction)dir;
            }else if(style == TileStyle.OUTER_CORNER || style == TileStyle.INNER_CORNER || style == TileStyle.SV_CORNER){
                if (dir == null) throw new ArgumentNullException("dir", "Cannot instantiate *_CORNER tile without any orientation.");
                if (secondDir == null) throw new ArgumentNullException("secondDir", "Cannot instantiate *_CORNER tile without both orientations.");
                this.style = style;
                this.dir = (Direction)dir;
                this.secondDir = (Direction)secondDir;
            }
            this.texturePath = texturePath;
            mapX = mapPos[0];
            mapY = mapPos[1];
            mapZ = mapPos[2];

            this.passage = passage;
        }
        public Tile(TileStyle style, int[] mapPos, Nullable<Direction> dir, Nullable<Direction> secondDir)
            : this(style,
            (style == TileStyle.FLAT) ? "Images/Textures/snowFlat" : "Images/Textures/snowSlope",
            mapPos, new bool[4] { true, true, true, true }, dir, secondDir){
                passage = GetPassage(style, dir, secondDir);
         }
        public void Initialize(){
            GraphicsDevice g = Global.GraphicsDevice;
            texture = Global.Content.Load<Texture2D>(texturePath);

            vertices = new VertexPositionNormalTexture[4];

            vertices[0] = new VertexPositionNormalTexture(new Vector3(0), new Vector3(0, 0, 1), new Vector2(0, 1));
            vertices[1] = new VertexPositionNormalTexture(new Vector3(0), new Vector3(0, 0, 1), new Vector2(0, 0));
            vertices[2] = new VertexPositionNormalTexture(new Vector3(0), new Vector3(0, 0, 1), new Vector2(1, 1));
            vertices[3] = new VertexPositionNormalTexture(new Vector3(0), new Vector3(0, 0, 1), new Vector2(1, 0));
            //0 is bottom left
            //1 is top left
            //2 is bottom right
            //3 is top right
            //Assign vertices normals and positions according to TileStyles, orientation, and mapCoords
            for(int i = 0; i < vertices.Length; i++){
                VertexPositionNormalTexture vertex = vertices[i];
                /***FLAT***/
                if (style == TileStyle.FLAT){
                    vertex.Position.Z = mapZ * HEIGHT;
                    vertex.Position.X = (i <= 1) ? mapX * SIZE : (mapX + 1) * SIZE;
                    vertex.Position.Y = (i % 2 == 0) ? mapY * SIZE : (mapY + 1) * SIZE;
                }
                /***VERTICAL***/
                if (style == TileStyle.VERTICAL){
                    if (dir == Direction.NORTH){
                        vertex.Position.X = (i > 1) ? mapX * SIZE : (mapX + 1) * SIZE;
                        vertex.Position.Y = (mapY + 1) * SIZE;
                    }
                    if (dir == Direction.SOUTH){
                        vertex.Position.X = (i <= 1) ? mapX * SIZE : (mapX + 1) * SIZE;
                        vertex.Position.Y = mapY * SIZE;
                    }
                    if (dir == Direction.EAST){
                        vertex.Position.X = (mapX + 1) * SIZE;
                        vertex.Position.Y = (i <= 1) ? mapY * SIZE : (mapY + 1) * SIZE;
                    }
                    if (dir == Direction.WEST){
                        vertex.Position.X = (mapX * SIZE);
                        vertex.Position.Y = (i > 1) ? mapY * SIZE : (mapY + 1) * SIZE;
                    }
                    vertex.Position.Z = (i % 2 == 0)?mapZ * HEIGHT:(mapZ + 1) * HEIGHT;
                }
                /***SLOPE***/
                if (style == TileStyle.SLOPE){
                    if (dir == Direction.NORTH){
                        vertex.Position.X = (i <= 1) ? (mapX + 1) * SIZE : mapX * SIZE;
                        vertex.Position.Y = (i % 2 == 0) ? (mapY + 1) * SIZE : mapY * SIZE;
                    }
                    if (dir == Direction.SOUTH){
                        vertex.Position.X = (i > 1) ? (mapX + 1) * SIZE : mapX * SIZE;
                        vertex.Position.Y = (i % 2 != 0) ? (mapY + 1) * SIZE : mapY * SIZE;
                    }
                    if (dir == Direction.EAST){
                        vertex.Position.X = (i % 2 == 0) ? (mapX + 1) * SIZE : mapX * SIZE;
                        vertex.Position.Y = (i <= 1)?mapY * SIZE:(mapY + 1) * SIZE;
                    }
                    if (dir == Direction.WEST){
                        vertex.Position.X = (i % 2 != 0) ? (mapX + 1) * SIZE : mapX * SIZE;
                        vertex.Position.Y = (i > 1) ? mapY * SIZE : (mapY + 1) * SIZE;
                    }
                    vertex.Position.Z = (i % 2 != 0) ? (mapZ + 1) * HEIGHT : mapZ * HEIGHT;
                }
                /***OUTER_CORNER***/
                if (style == TileStyle.OUTER_CORNER){
                    vertex.Position.Z = (i % 2 == 0) ? mapZ * HEIGHT : (mapZ + 1) * HEIGHT;
                    if (dir == Direction.NORTH){
                        vertex.Position.X = (i % 2 == 0) ? ((i > 1) ? mapX * SIZE : (mapX + 1) * SIZE) : ((secondDir == Direction.EAST) ? mapX * SIZE : (mapX + 1) * SIZE);
                        vertex.Position.Y = (i % 2 != 0) ? mapY * SIZE : (mapY + 1) * SIZE;
                        vertex.TextureCoordinate.X = (i % 2 != 0) ? ((secondDir == Direction.EAST) ? 1 : 0) : vertex.TextureCoordinate.X;
                    }
                    if (dir == Direction.SOUTH){
                        vertex.Position.X = (i % 2 == 0) ? ((i <= 1) ? mapX * SIZE : (mapX + 1) * SIZE) : ((secondDir == Direction.EAST) ? mapX * SIZE : (mapX + 1) * SIZE);
                        vertex.Position.Y = (i % 2 == 0) ? mapY * SIZE : (mapY + 1) * SIZE;
                        vertex.TextureCoordinate.X = (i % 2 != 0) ? ((secondDir == Direction.EAST) ? 0 : 1) : vertex.TextureCoordinate.X;
                    }
                    if (dir == Direction.EAST){
                        vertex.Position.X = (i % 2 != 0) ? mapX * SIZE : (mapX + 1) * SIZE;
                        vertex.Position.Y = (i % 2 == 0) ? ((i <= 1) ? mapY * SIZE : (mapY + 1) * SIZE) : ((secondDir == Direction.NORTH) ? mapY * SIZE : (mapY + 1) * SIZE);
                        vertex.TextureCoordinate.X = (i % 2 != 0) ? ((secondDir == Direction.NORTH) ? 0 : 1) : vertex.TextureCoordinate.X;
                    }
                    if (dir == Direction.WEST){
                        vertex.Position.X = (i % 2 == 0) ? mapX * SIZE : (mapX + 1) * SIZE;
                        vertex.Position.Y = (i % 2 == 0) ? ((i > 1) ? mapY * SIZE : (mapY + 1) * SIZE) : ((secondDir == Direction.NORTH) ? mapY * SIZE : (mapY + 1) * SIZE);
                        vertex.TextureCoordinate.X = (i % 2 != 0) ? ((secondDir == Direction.NORTH) ? 1 : 0) : vertex.TextureCoordinate.X;
                    }
                }
                /***INNER_CORNER***/
                if (style == TileStyle.INNER_CORNER){
                    if (dir == Direction.NORTH){
                        vertex.Position.X = (secondDir == Direction.EAST) ?
                            ((i % 2 == 0 || i <= 1) ? mapX * SIZE : (mapX + 1) * SIZE):
                            ((i % 2 != 0 && i <= 1) ? mapX * SIZE : (mapX + 1) * SIZE);
                        vertex.Position.Y = (i % 2 == 0) ? mapY * SIZE : (mapY + 1) * SIZE;
                    }
                    if (dir == Direction.SOUTH){
                        vertex.Position.X = (secondDir == Direction.EAST) ?
                            ((i % 2 == 0 || i > 1) ? mapX * SIZE : (mapX + 1) * SIZE) :
                            ((i % 2 != 0 && i > 1) ? mapX * SIZE : (mapX + 1) * SIZE);
                        vertex.Position.Y = (i % 2 != 0) ? mapY * SIZE : (mapY + 1) * SIZE;
                    }
                    if (dir == Direction.WEST){
                        vertex.Position.Y = (secondDir == Direction.NORTH) ?
                            ((i % 2 == 0 || i <= 1) ? mapY * SIZE : (mapY + 1) * SIZE) :
                            ((i % 2 != 0 && i <= 1) ? mapY * SIZE : (mapY + 1) * SIZE);
                        vertex.Position.X = (i % 2 != 0) ? mapX * SIZE : (mapX + 1) * SIZE;
                    }
                    if (dir == Direction.EAST){
                        vertex.Position.Y = (secondDir == Direction.NORTH) ?
                            ((i % 2 == 0 || i > 1) ? mapY * SIZE : (mapY + 1) * SIZE) :
                            ((i % 2 != 0 && i > 1) ? mapY * SIZE : (mapY + 1) * SIZE);
                        vertex.Position.X = (i % 2 == 0) ? mapX * SIZE : (mapX + 1) * SIZE;
                    }
                    vertex.Position.Z = (i % 2 == 0) ? mapZ * HEIGHT : (mapZ + 1) * HEIGHT;
                }
                /***SV_CORNER***/
                if (style == TileStyle.SV_CORNER){
                    if (dir == Direction.NORTH){
                        vertex.Position.X = (secondDir == Direction.EAST) ?
                            ((i == 3) ? (mapX + 1) * SIZE : (mapX) * SIZE) :
                            ((i == 1) ? mapX * SIZE : (mapX + 1) * SIZE);
                        vertex.Position.Y = (mapY + 1) * SIZE;
                    }
                    if (dir == Direction.SOUTH){
                        vertex.Position.X = (secondDir == Direction.EAST) ?
                            ((i == 3) ? (mapX + 1) * SIZE : (mapX) * SIZE) :
                            ((i == 1) ? mapX * SIZE : (mapX + 1) * SIZE);
                        vertex.Position.Y = mapY * SIZE;
                    }
                    if (dir == Direction.WEST){
                        vertex.Position.Y = (secondDir == Direction.NORTH) ?
                            ((i == 3) ? (mapY + 1) * SIZE : (mapY) * SIZE) :
                            ((i == 1) ? mapY * SIZE : (mapY + 1) * SIZE);
                        vertex.Position.X = mapX * SIZE;
                    }
                    if (dir == Direction.EAST){
                        vertex.Position.Y = (secondDir == Direction.NORTH) ?
                            ((i == 3) ? (mapY + 1) * SIZE : (mapY) * SIZE) :
                            ((i == 1) ? mapY * SIZE : (mapY + 1) * SIZE);
                        vertex.Position.X = (mapX + 1) * SIZE;
                    }
                    vertex.Position.Z = (i % 2 == 0) ? mapZ * HEIGHT : (mapZ + 1) * HEIGHT;
                }
                
                vertices[i] = vertex;
            }
        }
        public Texture2D GetTexture(){
            return texture;
        }
        public VertexPositionNormalTexture[] GetVertices(){
            return vertices;
        }
        private bool[] GetPassage(TileStyle style, Nullable<Direction> dir, Nullable<Direction> secondDir){
            bool[] p = new bool[4];

            if (style == TileStyle.FLAT){
                for (int i = 0; i < p.Length; i++)
                    p[i] = true;
            }else if (style == TileStyle.SLOPE){
                for (int i = 0; i < p.Length; i++)
                    p[i] = false;
            }else if(style == TileStyle.INNER_CORNER){
                for (int i = 0; i < p.Length; i++)
                    p[i] = false;
            }else if (style == TileStyle.OUTER_CORNER){
                for (int i = 0; i < p.Length; i++)
                    p[i] = false;
            }else if (style == TileStyle.VERTICAL){
                for (int i = 0; i < p.Length; i++)
                    if (i == (int)dir)
                        p[i] = false;
                    else
                        p[i] = true;
            }else if(style == TileStyle.SV_CORNER){
                for (int i = 0; i < p.Length; i++)
                    p[i] = false;
            }
            return p;
        }
    }
}