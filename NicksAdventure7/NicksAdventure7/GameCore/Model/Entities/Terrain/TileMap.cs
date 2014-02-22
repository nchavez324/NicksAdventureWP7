using NicksAdventure7.GameCore.Model.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NicksAdventure7.GameCore.Model.Entities.Events;
using NicksAdventure7.GameCore.Model.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NicksAdventure7.GameCore.Model.Entities.Terrain{
    class TileMap : Component{
        Tile[, ,] tiles;
        IEvent[, ,] events;//A complete restructuring may be required here -- IEvent positions change and this essential for drawing.
        Building[] buildings;
        public bool[, ,] bCollMap;/**CAREFUL!! false indicates passage!!**/
        public OWPlayer player;
        int numNonNull, eventNonNull;
        int charVertsOffset, eventVertsOffset;
        public string name;

        BasicEffect basicEffect;
        AlphaTestEffect alphaEffect;
        VertexBuffer vertexBuffer;
        VertexPositionNormalTexture[] vertices;
        RasterizerState rasterizerState;
        Matrix world = Matrix.CreateTranslation(0, 0, 0);
        public bool nightTime;

        public TileMap(string name, Tile[,,] tiles, IEvent[,,] events, Building[] buildings, int numNonNull, int eventNonNull){
            this.name = name;
            this.tiles = tiles;
            this.events = events;
            this.buildings = buildings;
            this.numNonNull = numNonNull;
            this.eventNonNull = eventNonNull;
        }
        //player should be set
        public void Initialize(){
            GraphicsDevice g = Global.GraphicsDevice;
            basicEffect = new BasicEffect(g);
            alphaEffect = new AlphaTestEffect(g);

            foreach (Building b in buildings)
                b.Initialize();

            foreach (Tile t in tiles){
                if (t != null){
                    t.Initialize();
                }
            }
            foreach(IEvent e in events){
                if(e != null){
                    e.Initialize();
                }
            }
            bCollMap = new bool[tiles.GetLength(0), tiles.GetLength(1), tiles.GetLength(2)];
            for (int l = 0; l < buildings.Length; l++){
                Building b = buildings[l];
                for (int i = 0; i < b.mapW; i++){
                    for (int j = 0; j < b.mapL; j++){
                        if (i + b.mapX < bCollMap.GetLength(0) && i + b.mapX >= 0 && j + b.mapY < bCollMap.GetLength(1) && j + b.mapY >= 0){
                            if (!b.GetRelativeColl(i, j))
                                bCollMap[i + b.mapX, j + b.mapY -  b.mapL, b.mapZ] = !b.GetRelativeColl(i, j);
                        }
                    }
                }
            }

            player.Initialize();

            vertices = new VertexPositionNormalTexture[(numNonNull + 1 + eventNonNull) * 4];

            int m = 0;
            for (int i = 0; i < tiles.GetLength(0); i++){
                for (int j = 0; j < tiles.GetLength(1); j++){
                    for (int k = 0; k < tiles.GetLength(2); k++){
                        if (tiles[i, j, k] != null){
                            for (int l = 0; l < 4; l++){
                                vertices[m * 4 + l] = tiles[i, j, k].GetVertices()[l];
                            }
                            m++;
                        }
                    }
                }
            }
            charVertsOffset = numNonNull * 4;
            for (int j = 0; j < 4; j++){
                    vertices[charVertsOffset + j] = player.GetVertices()[j];
            }

            eventVertsOffset = (numNonNull + 1) * 4;
            m = 0;
            for (int j = events.GetLength(1) - 1; j >= 0; j--){
                for (int k = events.GetLength(2) - 1; k >= 0; k--){
                    for (int i = events.GetLength(0) - 1; i >= 0; i--){
                        if (events[i, j, k] != null){
                            for (int l = 0; l < 4; l++){
                                vertices[eventVertsOffset + l] = events[i, j, k].GetVertices()[l];
                            }
                            m++;
                        }
                    }
                }
            }

            vertexBuffer = new VertexBuffer(g, typeof(VertexPositionNormalTexture), vertices.Length, BufferUsage.WriteOnly);

            rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            rasterizerState.SlopeScaleDepthBias = -5f;
            rasterizerState.FillMode = FillMode.Solid;

            basicEffect.View = Global.view;
            basicEffect.VertexColorEnabled = false;
            basicEffect.TextureEnabled = true;

            alphaEffect.View = Global.view;
            alphaEffect.VertexColorEnabled = false;
            alphaEffect.FogEnabled = false;
            alphaEffect.AlphaFunction = CompareFunction.GreaterEqual;
            alphaEffect.ReferenceAlpha = 125;

            basicEffect.AmbientLightColor = new Vector3(0.25f, 0.25f, 0.25f);
            nightTime = false;
            
        }
        public void Update(GameTime gameTime){
            basicEffect.LightingEnabled = nightTime;
            player.Update(gameTime, null);
            foreach (IEvent e in events){
                if (e != null)
                    e.Update(gameTime, player);
            }
            for (int j = 0; j < 4; j++){
                    vertices[(numNonNull) * 4 + j] = player.GetVertices()[j];
            }
            int m = 0;
            for (int j = events.GetLength(1) - 1; j >= 0; j--){
                for (int k = events.GetLength(2) - 1; k >= 0; k--){
                    for (int i = events.GetLength(0) - 1; i >= 0; i--){
                        if (events[i, j, k] != null){
                            for (int l = 0; l < 4; l++){
                                vertices[eventVertsOffset + (m * 4) + l] = events[i, j, k].GetVertices()[l];
                            }
                            m++;
                        }
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch){
            GraphicsDevice g = spriteBatch.GraphicsDevice;
            #region EffectMath
            basicEffect.World = world * Matrix.CreateRotationX(-0.3f);
            basicEffect.View = Global.view;
            basicEffect.Projection = Global.projection * Global.screenScale;
            alphaEffect.World = world * Matrix.CreateRotationX(-0.3f);
            alphaEffect.View = Global.view;
            alphaEffect.Projection = Global.projection * Global.screenScale;
            //maybe next line only on change?
            #endregion
            foreach (Building b in buildings){
                b.Draw(world, basicEffect, nightTime);
            }
            vertexBuffer.SetData<VertexPositionNormalTexture>(vertices);
            g.SetVertexBuffer(vertexBuffer);
            g.RasterizerState = rasterizerState;
            #region TileDraw
            int m = 0;
            for (int i = 0; i < tiles.GetLength(0); i++){
                for (int j = 0; j < tiles.GetLength(1); j++){
                    for (int k = 0; k < tiles.GetLength(2); k++){
                        if (tiles[i, j, k] != null){
                            basicEffect.Texture = tiles[i, j, k].GetTexture();
                            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes){
                                pass.Apply();
                                g.DrawPrimitives(PrimitiveType.TriangleStrip, m * 4, 2);
                            }
                            m++;
                        }
                    }
                }
            }
            #endregion

            m = 0;
            //g.DepthStencilState = new DepthStencilState() { DepthBufferEnable = false };
            g.DepthStencilState = DepthStencilState.Default;
            #region EventDraw
            for (int j = events.GetLength(1) - 1; j >= 0; j--){
                for (int k = events.GetLength(2)-1; k >= 0; k--){
                    for (int i = events.GetLength(0) - 1; i >= 0; i--){
                        if ((i == player.GetMapPos()[Character.X]) && (j == player.GetMapPos()[Character.Y]) && k == player.GetMapPos()[Character.Z]){
                            alphaEffect.Texture = player.GetTexture();
                            foreach (EffectPass pass in alphaEffect.CurrentTechnique.Passes){
                                pass.Apply();
                                g.DrawPrimitives(PrimitiveType.TriangleStrip, charVertsOffset, 2);
                            }
                            player.DebugDraw(spriteBatch);
                        }
                        if (events[i, j, k] != null){
                            alphaEffect.Texture = events[i, j, k].GetTexture();
                            foreach (EffectPass pass in alphaEffect.CurrentTechnique.Passes){
                                pass.Apply();
                                g.DrawPrimitives(PrimitiveType.TriangleStrip, eventVertsOffset + m*4, 2);
                            }
                            events[i, j, k].DebugDraw(spriteBatch);
                            m++;
                        }
                    }
                }
            }
            #endregion
            //g.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
        }
        public void SetLighting(bool lighting){
            this.nightTime = lighting;
        }
        public void SetTransparency(float alpha){
            basicEffect.Alpha = alpha;
        }
        public Tile GetTile(int i, int j, int k){
            if (i < tiles.GetLength(0) && i >= 0 && j < tiles.GetLength(1) && j >= 0){
                for (int z = 0; z < tiles.GetLength(2); z++){
                    if (tiles[i, j, z] != null){
                        if (tiles[i, j, z].mapZ == k)
                            return tiles[i, j, z];
                    }else{
                        return null;
                    }
                }
            }
            return null;
        }
        public IEvent GetEvent(int i, int j, int k){
            foreach (IEvent e in events){
                if (e != null) {
                    int[] pos = e.GetMapPos();
                    if (i == pos[BasicEvent.X] && j == pos[BasicEvent.Y] && k == pos[BasicEvent.Z])
                        return e;
                }
            }
            return null;
        }
        public void UpdateEventGrid(IEvent e, int[] oldPos, int[] newPos){
            /*
            if (e != player){
                for (int k = 0; k < events.GetLength(2); k++){
                    if (events[newPos[Character.X], newPos[Character.Y], k] == null){
                        events[newPos[Character.X], newPos[Character.Y], k] = e;
                        for (int k2 = 0; k2 < events.GetLength(2); k2++){
                            if (events[oldPos[Character.X], oldPos[Character.Y], k2] != null &&
                                events[oldPos[Character.X], oldPos[Character.Y], k2].GetName().CompareTo(e.GetName()) == 0){
                                events[oldPos[Character.X], oldPos[Character.Y], k2] = null;
                                return;
                            }
                        }
                    }
                }
                throw new Exception("Not enough room for events!!");
            }
            */
        }
    }
}
