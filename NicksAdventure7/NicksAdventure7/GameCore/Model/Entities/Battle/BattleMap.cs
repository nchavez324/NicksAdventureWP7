using NicksAdventure7.GameCore.Model.Graphics;
using NicksAdventure7.GameCore.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NicksAdventure7.GameCore.Model.Entities.Battle{
    class BattleMap : Component{
        string mapName;
        Microsoft.Xna.Framework.Graphics.Model map;
        public Battler[] battlers;
        AttackEffect[] attackFx;
        static int maxNumAttackFx = 10;
        int player;

        BasicEffect basicEffect;
        VertexBuffer vertexBuffer;
        VertexPositionNormalTexture[] vertices;
        RasterizerState rasterizerState;
        Matrix world = Matrix.CreateTranslation(0, 0, 0);
        public bool nightTime;

        public BattleMap(string mapName, Battler[] battlers, int player){
            this.mapName = mapName;
            this.battlers = battlers;
            this.player = player;
        }
        public void Initialize(){
            GraphicsDevice g = Global.GraphicsDevice;
            basicEffect = new BasicEffect(g);

            foreach (Battler b in battlers)
                b.Initialize();
            attackFx = new AttackEffect[maxNumAttackFx];
            map = Global.Content.Load<Microsoft.Xna.Framework.Graphics.Model>("Models/BattleMaps/" + mapName);

            vertices = new VertexPositionNormalTexture[(battlers.Length + maxNumAttackFx) * 4];
            for (int i = 0; i < battlers.Length; i++)
                for (int j = 0; j < 4; j++)
                    vertices[(i * 4) + j] = battlers[i].GetVertices()[j];
            vertexBuffer = new VertexBuffer(g, typeof(VertexPositionNormalTexture), vertices.Length, BufferUsage.WriteOnly);

            rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            rasterizerState.FillMode = FillMode.Solid;

            basicEffect.View = Global.view;
            basicEffect.VertexColorEnabled = false;
            basicEffect.TextureEnabled = true;

            basicEffect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
            nightTime = false;
        }
        public void Update(GameTime gameTime){
            basicEffect.LightingEnabled = nightTime;
            foreach (Battler b in battlers)
                b.Update(gameTime, battlers);
            foreach (AttackEffect afx in attackFx)
                if(afx != null)
                    afx.Update(gameTime, battlers);
            for (int i = 0; i < battlers.Length; i++)
                for (int j = 0; j < 4; j++)
                    vertices[(i * 4) + j] = battlers[i].GetVertices()[j];
            for (int i = 0; i < attackFx.Length; i++)
                for (int j = 0; j < 4; j++)
                    if(attackFx[i] != null)
                        vertices[((battlers.Length + i) * 4) + j] = attackFx[i].GetVertices()[j];

        }
        public void Draw(SpriteBatch spriteBatch){
            GraphicsDevice g = spriteBatch.GraphicsDevice;
            basicEffect.World = world * Matrix.CreateRotationX(0.2625f);
            basicEffect.View = Global.view;
            basicEffect.Projection = Global.projection * Global.screenScale;

            DrawMap();

            vertexBuffer.SetData<VertexPositionNormalTexture>(vertices);
            g.SetVertexBuffer(vertexBuffer);
            g.RasterizerState = rasterizerState;

            for (int i = 0; i < battlers.Length; i++){
                basicEffect.Texture = battlers[i].GetTexture();
                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes){
                    pass.Apply();
                    g.DrawPrimitives(PrimitiveType.TriangleStrip, (i * 4), 2);
                }
                battlers[i].DebugDraw(spriteBatch);
            }
            for (int i = 0; i < attackFx.Length; i++){
                if (attackFx[i] != null){
                    basicEffect.Texture = attackFx[i].GetTexture();
                    foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes){
                        pass.Apply();
                        g.DrawPrimitives(PrimitiveType.TriangleStrip, ((battlers.Length + i) * 4), 2);
                    }
                    attackFx[i].DebugDraw(spriteBatch);
                }
            }
        }
        public void DrawMap(){
            foreach (ModelMesh mesh in map.Meshes){
                foreach (BasicEffect effect in mesh.Effects){
                    effect.World = world * Matrix.CreateRotationX(0.2625f);
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

                    if (nightTime){
                        effect.AmbientLightColor = new Vector3(0f, 0f, 0f);
                        effect.DirectionalLight0.DiffuseColor = new Vector3(.25f);
                        effect.DirectionalLight1.DiffuseColor = new Vector3(.17f);
                        effect.EmissiveColor = new Vector3(.3f, .3f, .3f);
                    }else{
                        effect.AmbientLightColor = basicEffect.AmbientLightColor;
                        effect.EmissiveColor = new Vector3(.75f, .75f, .75f);
                    }

                }
                mesh.Draw();
            }
        }
        public void SetNightTime(bool nightTime){
            this.nightTime = nightTime;
        }
        public bool AddAttackFx(AttackEffect attack){
            for (int i = 0; i < attackFx.Length; i++){
                if (attackFx[i] == null){
                    attackFx[i] = attack;
                    return true;
                }
            }
            return false;
        }
        public Battler GetPlayer(){
            return battlers[player];
        }
    }
}
