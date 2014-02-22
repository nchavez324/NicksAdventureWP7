using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NicksAdventure7.GameCore.Model.Entities.Terrain{
    public enum Name{
        BlockHouse
    };
    class Building{
        static int[][] dims = {
                                  new int[]{3,1}
                              };
        static bool[][][] collMaps = {
                                         new bool[][]{
                                             new bool[]{false,true,false}
                                         }
                                     };
        Microsoft.Xna.Framework.Graphics.Model model;
        Name name;
        public int mapX, mapY, mapZ, mapW, mapL;

        public Building(Name name, int[] mapPos){
            this.name = name;
            mapX = mapPos[0];
            mapY = mapPos[1];
            mapZ = mapPos[2];
            mapW = dims[(int)name][0];
            mapL = dims[(int)name][1];
        }
        public void Initialize(){
            model = Global.Content.Load<Microsoft.Xna.Framework.Graphics.Model>("Models/Buildings/" + name.ToString());
        }
        public void Draw(Matrix world, BasicEffect basicEffect, bool lighting){
            foreach (ModelMesh mesh in model.Meshes){
                foreach (BasicEffect effect in mesh.Effects){
                    effect.World = world * Matrix.CreateTranslation((mapX * Tile.SIZE) + mapW, (mapY * Tile.SIZE) - mapL, mapZ*Tile.HEIGHT) * Matrix.CreateRotationX(-0.3f);
                    effect.View = Global.view;
                    effect.Projection = Global.projection * Global.screenScale;
                    effect.LightingEnabled = true;
                    effect.DirectionalLight0.Direction = new Vector3(1, .5f, -1);
                    effect.DirectionalLight0.Enabled = true;
                    effect.DirectionalLight0.DiffuseColor = new Vector3(.5f);
                    effect.DirectionalLight1.Direction = new Vector3(-1, .5f, -1);
                    effect.DirectionalLight1.Enabled = true;
                    effect.DirectionalLight1.DiffuseColor = new Vector3(.35f);
                    effect.DirectionalLight2.Enabled = false;

                    if (lighting){
                        effect.AmbientLightColor = new Vector3(0f, 0f, 0f);
                        effect.DirectionalLight0.DiffuseColor = new Vector3(.25f);
                        effect.DirectionalLight1.DiffuseColor = new Vector3(.17f);
                    }
                    else
                        effect.AmbientLightColor = basicEffect.AmbientLightColor;
                }
                mesh.Draw();
            }
        }
        public bool GetRelativeColl(int x, int y){
            return collMaps[(int)name][collMaps[(int)name].Length - 1 - y][x];
        }
    }
}
