using NicksAdventure7.GameCore.Model.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NicksAdventure7.GameCore.Model.Entities.Events;
using NicksAdventure7.GameCore.Model.Entities.Terrain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NicksAdventure7.GameCore.Model.Entities{
    public abstract class Billboard{
        protected static float HEIGHT = (float)(Tile.SIZE * Math.Sin(Math.Atan2(Tile.HEIGHT, Tile.SIZE)));
        protected Texture2D texture;
        protected string texturePath;
        protected AdvancedAnimation animation;
        protected Dictionary<String, AdvancedAnimation> animations;
        protected bool flipped;
        protected VertexPositionNormalTexture[] vertices;

        public void MiniInit(){
            GraphicsDevice g = Global.GraphicsDevice;
            texture = Global.Content.Load<Texture2D>(texturePath);

            vertices = new VertexPositionNormalTexture[4];

            vertices[0] = new VertexPositionNormalTexture(new Vector3(0), new Vector3(0, 0, 1), new Vector2(0, 1));
            vertices[1] = new VertexPositionNormalTexture(new Vector3(0), new Vector3(0, 0, 1), new Vector2(0, 0));
            vertices[2] = new VertexPositionNormalTexture(new Vector3(0), new Vector3(0, 0, 1), new Vector2(1, 1));
            vertices[3] = new VertexPositionNormalTexture(new Vector3(0), new Vector3(0, 0, 1), new Vector2(1, 0));
        }
        public Texture2D GetTexture(){
            return texture;
        }
        public VertexPositionNormalTexture[] GetVertices(){
            return vertices;
        }
        protected void UpdateUVCoords(){
            Rectangle clip = animation.GetClip();
            //make sure it is proportional
            
            if (flipped){
                vertices[2].TextureCoordinate = new Vector2((clip.X) / (float)texture.Width, (clip.Y + clip.Height) / (float)texture.Height);
                vertices[3].TextureCoordinate = new Vector2((clip.X) / (float)texture.Width, (clip.Y) / (float)texture.Height);
                vertices[0].TextureCoordinate = new Vector2((clip.X + clip.Width) / (float)texture.Width, (clip.Y + clip.Height) / (float)texture.Height);
                vertices[1].TextureCoordinate = new Vector2((clip.X + clip.Width) / (float)texture.Width, (clip.Y) / (float)texture.Height);
            }else{
                vertices[0].TextureCoordinate = new Vector2((clip.X) / (float)texture.Width, (clip.Y + clip.Height) / (float)texture.Height);
                vertices[1].TextureCoordinate = new Vector2((clip.X) / (float)texture.Width, (clip.Y) / (float)texture.Height);
                vertices[2].TextureCoordinate = new Vector2((clip.X + clip.Width) / (float)texture.Width, (clip.Y + clip.Height) / (float)texture.Height);
                vertices[3].TextureCoordinate = new Vector2((clip.X + clip.Width) / (float)texture.Width, (clip.Y) / (float)texture.Height);
            }
        }
        protected abstract void UpdateVerts(GameTime gameTime);
        protected abstract void UpdateMotion(GameTime gameTime);
        public abstract void DebugDraw(SpriteBatch spriteBatch);
    }
}
