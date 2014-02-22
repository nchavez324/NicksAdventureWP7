using NicksAdventure7.GameCore.Model.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NicksAdventure7.GameCore.Model.Entities.Terrain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NicksAdventure7.GameCore.Model.Entities.Battle{
    class Battler : Billboard{
        protected readonly static float speed = 0.01f;
        public Vector3 position;
        protected Vector3 velocity, acceleration;

        public Battler(string texturePath, Vector3 position){
            this.texturePath = texturePath;
            this.position = position;
        }
        public void Initialize(){
            MiniInit();
            position = new Vector3(0, -.5f, 3.75f);
            for (int i = 0; i < vertices.Length; i++){
                VertexPositionNormalTexture vertex = vertices[i];
                float s = 2f;
                vertex.Position.X = ((i > 1) ? (position.X + 1) * s * Tile.SIZE : (position.X) * s * Tile.SIZE);
                vertex.Position.Y = (i % 2 != 0) ? (position.Y + 1.75f) * s * Tile.SIZE : (position.Y) * s * Tile.SIZE;
                vertex.Position.Z = (i % 2 != 0) ? (position.Z) * s * HEIGHT : (position.Z) * s * HEIGHT;
                vertices[i] = vertex;
            }
            //config animations
            animations = new Dictionary<string, AdvancedAnimation>();
            animations["Idle"] = new AdvancedAnimation(texture, new Rectangle(0,0, 248, 35), 8);
            animations["Walking"] = new AdvancedAnimation(texture, new Rectangle(0, 35, 232, 34), 8);
            animations["Running"] = new AdvancedAnimation(texture, new Rectangle(0, 70, 192, 32), 6);
            animations["Walking"].frameTime = 70;
            animations["Walking"].Start();
            animation = animations["Idle"];

            animation.running = true;
            UpdateUVCoords();
        }
        public void Update(GameTime gameTime, Battler[] battlers){
            UpdateMotion(gameTime);
            UpdateVerts(gameTime);
            if (animation != null){
                int frameNum = animation.sceneIndex;
                animation.Update(gameTime);
                UpdateUVCoords();
            }
        }
        protected override void UpdateVerts(GameTime gameTime){
            for (int i = 0; i < vertices.Length; i++){
                VertexPositionNormalTexture vertex = vertices[i];
                float s = 2f;
                vertex.Position.X = ((i > 1) ? (position.X + 1) * s * Tile.SIZE : (position.X) * s * Tile.SIZE);
                vertex.Position.Y = (i % 2 != 0) ? (position.Y + 1.75f) * s * Tile.SIZE : (position.Y) * s * Tile.SIZE;
                vertex.Position.Z = (i % 2 != 0) ? (position.Z) * s * HEIGHT : (position.Z) * s * HEIGHT;
                vertices[i] = vertex;
            }
        }
        protected override void UpdateMotion(GameTime gameTime){
            velocity.X += acceleration.X * gameTime.ElapsedGameTime.Milliseconds;
            velocity.Y += acceleration.Y * gameTime.ElapsedGameTime.Milliseconds;
            velocity.Z += acceleration.Z * gameTime.ElapsedGameTime.Milliseconds;
            position.X += velocity.X * gameTime.ElapsedGameTime.Milliseconds;
            position.Y += velocity.Y * gameTime.ElapsedGameTime.Milliseconds;
            position.Z += velocity.Z * gameTime.ElapsedGameTime.Milliseconds;


        }
        public void Move(Tile.Direction dir){
            if (dir == Tile.Direction.NORTH){
                //jump
            }else if (dir == Tile.Direction.SOUTH){
                //crouch
            }else if (dir == Tile.Direction.EAST){
                velocity.X = speed;
                animation = animations["Walking"];
                flipped = false;
            }else if (dir == Tile.Direction.WEST){
                velocity.X = -speed;
                animation = animations["Walking"];
                flipped = true;
            }
        }
        public void MoveReleased(){
            velocity.X = 0;
            velocity.Y = 0;
            animation = animations["Idle"];
        }
        public override void DebugDraw(SpriteBatch spriteBatch){
        }
    }
}
