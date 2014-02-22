using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NicksAdventure7;

namespace NicksAdventure7.GameCore.Model.Graphics{
    class Sprite : Component{
        public Dictionary<string, SimpleAnimation> animations;
        public SimpleAnimation animation;
        public string curAnim;
        public Texture2D singleImg;

        public Vector2 position, velocity, acceleration;

        public bool hud = false;
        public bool visible = true;

        public Sprite(){
            position = new Vector2(0, 0);
            velocity = new Vector2(0, 0);
            acceleration = new Vector2(0, 0);
            hud = false;
        }
        public Sprite(Dictionary<string, SimpleAnimation> animations, string def) : this(){
            this.animations = animations;
            animation = animations[def];
            curAnim = def;
        }
        public Sprite(SimpleAnimation animation) : this(){
            this.animation = animation;
        }
        public Sprite(Texture2D singleImg) : this(){
            this.singleImg = singleImg;
        }
        public void Initialize(){
            
        }
        public void Update(GameTime gameTime){
            position.X += velocity.X * gameTime.ElapsedGameTime.Milliseconds;
            position.Y += velocity.Y * gameTime.ElapsedGameTime.Milliseconds;
            if (animation != null){
                animation.Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch){
            if (visible){
                if (animation != null){
                    if (hud){
                        spriteBatch.Draw(
                            animation.sheet,
                            new Rectangle((int)position.X, (int)position.Y, animation.frameWidth, animation.frameHeight),
                            animation.GetClip(),
                            Color.White
                            );
                    }else{
                        spriteBatch.Draw(animation.sheet,
                            new Rectangle((int)(position.X - Global.camera.X), (int)(position.Y - Global.camera.Y), animation.frameWidth, animation.frameHeight),
                            animation.GetClip(),
                            Color.White
                            );
                    }
                }else{
                    if (hud){
                        spriteBatch.Draw(singleImg, new Vector2(position.X, position.Y), Color.White);
                    }else{
                        spriteBatch.Draw(singleImg, new Vector2(position.X - Global.camera.X, position.Y - Global.camera.Y), Color.White);
                    }
                }
            }
        }
    }
}
