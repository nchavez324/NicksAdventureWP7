using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

using NicksAdventure7.GameCore.Scenes;
using NicksAdventure7.GameCore.Model.Entities;

using NicksAdventure7;
using Microsoft.Xna.Framework;

namespace NicksAdventure7.GameCore.Input{
    class KeyManager{

        KeyboardState currentState, oldState;

        GameScene scene;
        LoadScene load;
        IntroScene intro;
        ActionScene action;
        EditorScene editor;

        public KeyManager(GameScene scene){
            SetScene(scene);
        }
        public void Update(){
            if (currentState == null){
                currentState = Keyboard.GetState();
            }
            oldState = currentState;
            currentState = Keyboard.GetState();
            if (scene.kind == Scene.LOAD){
            }else if (scene.kind == Scene.INTRO){
            }else if(scene.kind == Scene.ACTION){
            }else if(scene.kind == Scene.EDITOR){
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
            }else if(scene.kind == Scene.ACTION){
                action = (ActionScene)scene;
            }else if(scene.kind == Scene.EDITOR){
                editor = (EditorScene)scene;
            }
        }   
    }
}
