using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using NicksAdventure7.GameCore.Scenes;
using NicksAdventure7;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace NicksAdventure7.GameCore.Input{
    class PointerManager{
        
        MouseState oldMState, currentMState;

        GameScene scene;
        LoadScene load;
        IntroScene intro;
        ActionScene action;
        EditorScene editor;

        Vector2 lastPos;

        public PointerManager(GameScene scene){
            SetScene(scene);
            lastPos = new Vector2(0.0f);
        }
        public void Update(){
            if (currentMState == null){
                currentMState = Mouse.GetState();
            }
            oldMState = currentMState;
            currentMState = Mouse.GetState();

            if(scene.kind == Scene.LOAD){
            }else if (scene.kind == Scene.INTRO){
            }else if (scene.kind == Scene.ACTION){
                
            }else if (scene.kind == Scene.EDITOR){
                /*
                if (currentMState.X >= 0 && currentMState.X < editor.tilePaneWidth &&
                    currentMState.Y >= Global.paddleHeight && currentMState.Y < Global.paddleHeight + editor.tilePaneHeight){
                        editor.hoverX = currentMState.X / (editor.tileSize * 2);
                        editor.hoverY = (currentMState.Y - Global.paddleHeight) / editor.tileSize;
                }
                if (currentMState.LeftButton == ButtonState.Pressed){
                    editor.selX = editor.hoverX;
                    editor.selY = editor.hoverY;
                }
                */
            }
        }
        public void SwitchScenes(GameScene newScene){
            SetScene(newScene);
        }
        private void SetScene(GameScene scene){
            this.scene = scene;
            if(scene.kind == Scene.LOAD){
                load = (LoadScene)scene;
            }else if (scene.kind == Scene.INTRO){
                intro = (IntroScene)scene;
            }else if (scene.kind == Scene.ACTION){
                action = (ActionScene)scene;
            }else if (scene.kind == Scene.EDITOR){
                editor = (EditorScene)scene;
            }
        }
        public float? IntersectDistance(BoundingSphere sphere, Vector2 mouseLocation){
            Matrix view = Global.view;
            Matrix projection = Global.projection;
            Viewport viewport = Global.GraphicsDevice.Viewport;
            Vector3 nearPoint = viewport.Unproject(new Vector3(mouseLocation.X,
            mouseLocation.Y, 0.0f),
            projection,
            view,
            Matrix.Identity);

            Vector3 farPoint = viewport.Unproject(new Vector3(mouseLocation.X,
            mouseLocation.Y, 1.0f),
            projection,
            view,
            Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            Ray mouseRay = new Ray(nearPoint, direction);
            return mouseRay.Intersects(sphere);
            //bounding box or frustum?
        }

    }
}
