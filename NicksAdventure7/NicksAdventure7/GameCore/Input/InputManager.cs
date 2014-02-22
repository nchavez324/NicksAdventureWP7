using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NicksAdventure7.GameCore.Scenes;
using NicksAdventure7.GameCore.Input;

namespace NicksAdventure7.GameCore.Input{
    class InputManager{
        KeyManager keyMgr;
        PointerManager ptrMgr;
        public GamePad pad;

        GameScene scene;
        LoadScene load;
        IntroScene intro;
        ActionScene action;
        EditorScene editor;

        public InputManager(GameScene scene){
            SetScene(scene);
            keyMgr = new KeyManager(scene);
            ptrMgr = new PointerManager(scene);
            pad = new GamePad();
        }
        public void Update(){
            keyMgr.Update();
            ptrMgr.Update();
            pad.Query(scene);
        }
        public void SwitchScenes(GameScene newScene){
            SetScene(newScene);
            keyMgr.SwitchScenes(newScene);
            ptrMgr.SwitchScenes(newScene);
        }
        private void SetScene(GameScene scene){
            this.scene = scene;
            if (scene.kind == Scene.LOAD){
                load = (LoadScene)scene;
            }else if (scene.kind == Scene.INTRO){
                intro = (IntroScene)scene;
            }else if(scene.kind == Scene.ACTION){
                action = (ActionScene)scene;
            }else if (scene.kind == Scene.EDITOR){
                editor = (EditorScene)scene;
            }
        }
    }
}
