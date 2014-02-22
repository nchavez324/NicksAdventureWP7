using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NicksAdventure7.GameCore.Model.Graphics{
    public class SimpleAnimation{
        public int frameTime = 125;
        private int movieTime;
        protected int totalTime;

        public int sceneIndex;
        public int frameWidth, frameHeight;
        public int rows, cols;
        public int numFrames;

        public bool reverse = false;
        public bool once = false;
        public bool running = false;

        public Texture2D sheet;

        public SimpleAnimation(Texture2D sheet, int numFrames){
            this.sheet = sheet;
            this.numFrames = numFrames;
            frameWidth = sheet.Width / numFrames;
            frameHeight = sheet.Height;
            rows = -1;
            cols = -1;
            
            Start();
        }
        public SimpleAnimation(Texture2D sheet, int rows, int cols){
            this.sheet = sheet;
            this.rows = rows;
            this.cols = cols;
            numFrames = rows * cols;
            frameHeight = sheet.Height / rows;
            frameWidth = sheet.Width / cols;
            Start();
        }
        public void Start(){
            totalTime = numFrames * frameTime;
            movieTime = 0;
            sceneIndex = 0;
            running = true;
            reverse = false;
        }
        private void ReverseStart(){
            movieTime = totalTime;
            sceneIndex = numFrames - 1;
            running = true;
            reverse = true;
        }
        public void Update(GameTime gameTime){
                if (!reverse){
                    if (numFrames > 1 && running){
                        movieTime += gameTime.ElapsedGameTime.Milliseconds;
                        if (movieTime >= totalTime){
                            if (once){
                                running = false;
                            }else{
                                Start();
                            }
                        }
                        while (movieTime > (sceneIndex + 1) * frameTime){
                            sceneIndex++;
                        }
                    }
                }else{
                    if (numFrames > 1 && running){
                        movieTime -= gameTime.ElapsedGameTime.Milliseconds;
                        if (movieTime <= 0){
                            if (once){
                                running = false;
                            }else{
                                ReverseStart();
                            }
                        }
                        while (movieTime <= sceneIndex * frameTime){
                            sceneIndex--;
                        }
                    }
                }
        }
        public Rectangle GetClip(){
            Rectangle ans;
            if (rows != -1 && cols != -1){
                ans = new Rectangle(
                    (sceneIndex % cols) * frameWidth,
                    (sceneIndex / rows) * frameHeight,
                    frameWidth,
                    frameHeight
                    );
            }else{
                ans = new Rectangle(
                    sceneIndex * frameWidth,
                    0,
                    frameWidth,
                    frameHeight
                    );
            }
            return ans;
        }
        public void PauseSet(int sceneIndex){
            running = false;
            this.sceneIndex = sceneIndex;
        }
    }
    public class AdvancedAnimation : SimpleAnimation{
        Rectangle section;

        public AdvancedAnimation(Texture2D sheet, Rectangle section, int numFrames) : base(sheet, numFrames){
            this.section = section;
            frameWidth = section.Width / numFrames;
            frameHeight = section.Height;
        }
        public AdvancedAnimation(Texture2D sheet, Rectangle section, int rows, int cols) : base(sheet, rows, cols){
            this.section = section;
            frameHeight = section.Height / rows;
            frameWidth = section.Width / cols;
        }
        
        public new Rectangle GetClip(){
            Rectangle ans;
            if (rows != -1 && cols != -1){
                ans = new Rectangle(
                    section.X + ((sceneIndex % cols) * frameWidth),
                    section.Y + ((sceneIndex / rows) * frameHeight),
                    frameWidth,
                    frameHeight
                    );
            }else{
                ans = new Rectangle(
                    section.X + (sceneIndex * frameWidth),
                    section.Y,
                    frameWidth,
                    frameHeight
                    );
            }
            return ans;
        }
    }
}
