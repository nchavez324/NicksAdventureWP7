using NicksAdventure7.GameCore.Model.Graphics;
using NicksAdventure7.GameCore.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NicksAdventure7.GameCore.Input{
    class GamePad : Component{

        public enum Buttons{
            Up, Down, Left, Right, A, B, Enter, Alt, Home
        };
        Texture2D DPad, A, B, Enter, Alt, Home;
        Vector2 DPadCenter, ACenter, BCenter;
        Rectangle DPadRect, ARect, BRect, EnterRect, AltRect, HomeRect;
        bool[] currentState;
        bool[] oldState;

        public GamePad(){
        }
        public void Initialize(){
                DPad = Global.Content.Load<Texture2D>("Images/UI/DPad");
                A = Global.Content.Load<Texture2D>("Images/UI/A");
                B = Global.Content.Load<Texture2D>("Images/UI/B");
                Enter = Global.Content.Load<Texture2D>("Images/UI/Enter");
                Alt = Global.Content.Load<Texture2D>("Images/UI/Alt");
                Home = Global.Content.Load<Texture2D>("Images/UI/Home");

                float scale = (float)0.9f * ((float)Global.camera.Width / (2f * DPad.Width));

                DPadRect = new Rectangle(0, (int)(Global.camera.Height - Global.paddleHeight), (int)(DPad.Width * scale), (int)(DPad.Height * scale));
                DPadCenter = new Vector2(DPadRect.X + DPadRect.Width / 2f, DPadRect.Y + DPadRect.Height / 2f);

                ARect = new Rectangle((int)(Global.camera.Width - A.Width * scale), (int)(Global.camera.Height - Global.paddleHeight), (int)(A.Width * scale), (int)(A.Height * scale));
                ACenter = new Vector2(ARect.X + ARect.Width / 2f, ARect.Y + ARect.Height / 2f);

                BRect = new Rectangle((int)(Global.camera.Width - 2 * B.Width * scale), (int)(Global.camera.Height - 4 * Global.paddleHeight / 7 - scale * (B.Height / 2)), (int)(B.Width * scale), (int)(B.Height * scale));
                BCenter = new Vector2(BRect.X + BRect.Width / 2f, BRect.Y + BRect.Height / 2f);

                EnterRect = new Rectangle(0, (int)(Global.camera.Height - Enter.Height * scale), (int)(Enter.Width * scale), (int)(Enter.Height * scale));
                AltRect = new Rectangle((int)(Global.camera.Width - (Alt.Width * scale)), (int)(Global.camera.Height - Alt.Height * scale), (int)(Alt.Width * scale), (int)(Alt.Height * scale));
                HomeRect = new Rectangle((int)((Global.camera.Width - (Home.Width * scale)) / 2f), (int)(Global.camera.Height - (Home.Height * scale)), (int)(Home.Width * scale), (int)(Home.Height * scale));

                currentState = new bool[9];
                oldState = new bool[9];
        }
        public void Query(GameScene scene){
            //Gets Input
            if (TouchPanel.GetCapabilities().IsConnected){
                TouchCollection coll = TouchPanel.GetState();
                oldState = currentState;
                bool[] tempState = new bool[9];
                foreach (TouchLocation loc in coll){
                    bool touchDPad = (Global.Distance(DPadCenter, loc.Position) <= DPadRect.Width / 2f);
                    if (touchDPad && (loc.Position.Y < -Math.Abs(loc.Position.X - DPadCenter.X) + DPadCenter.Y)) tempState[0] = true;
                    if (touchDPad && (loc.Position.Y > Math.Abs(loc.Position.X - DPadCenter.X) + DPadCenter.Y)) tempState[1] = true;
                    if (touchDPad && (loc.Position.X < -Math.Abs(loc.Position.Y - DPadCenter.Y) + DPadCenter.X)) tempState[2] = true;
                    if (touchDPad && (loc.Position.X > Math.Abs(loc.Position.Y - DPadCenter.Y) + DPadCenter.X)) tempState[3] = true;
                    if (Global.Distance(ACenter, loc.Position) <= ARect.Width / 2f) tempState[4] = true;
                    if (Global.Distance(BCenter, loc.Position) <= BRect.Width / 2f) tempState[5] = true;
                    if (loc.Position.X >= EnterRect.X && loc.Position.X < EnterRect.X + EnterRect.Width && loc.Position.Y >= EnterRect.Y && loc.Position.Y < EnterRect.Y + EnterRect.Height) tempState[6] = true;
                    if (loc.Position.X >= AltRect.X && loc.Position.X < AltRect.X + AltRect.Width && loc.Position.Y >= AltRect.Y && loc.Position.Y < AltRect.Y + AltRect.Height) tempState[7] = true;
                    if (loc.Position.X >= HomeRect.X && loc.Position.X < HomeRect.X + EnterRect.Width && loc.Position.Y >= HomeRect.Y && loc.Position.Y < HomeRect.Y + HomeRect.Height) tempState[8] = true;
                }
                currentState = tempState;
            }
            //Query for it here...
            if (scene.kind == Scene.INTRO){
            }else if(scene.kind == Scene.ACTION){
                ActionScene action = (ActionScene)scene;
                if (currentState[(int)Buttons.Up]){
                    if (action.mode == ActionScene.Mode.OVERWORLD){
                            action.tileMap.player.Move(Model.Entities.Terrain.Tile.Direction.NORTH);
                    }else if (action.mode == ActionScene.Mode.BATTLE)
                        action.battleMap.GetPlayer().Move(Model.Entities.Terrain.Tile.Direction.NORTH);
                }else if (currentState[(int)Buttons.Down]){
                    if (action.mode == ActionScene.Mode.OVERWORLD){
                            action.tileMap.player.Move(Model.Entities.Terrain.Tile.Direction.SOUTH);
                    }else if (action.mode == ActionScene.Mode.BATTLE)
                        action.battleMap.GetPlayer().Move(Model.Entities.Terrain.Tile.Direction.SOUTH);
                }else if (currentState[(int)Buttons.Left]){
                    if (action.mode == ActionScene.Mode.OVERWORLD)
                        action.tileMap.player.Move(Model.Entities.Terrain.Tile.Direction.WEST);
                    else if (action.mode == ActionScene.Mode.BATTLE)
                        action.battleMap.GetPlayer().Move(Model.Entities.Terrain.Tile.Direction.WEST);
                }
                else if (currentState[(int)Buttons.Right]){
                    if (action.mode == ActionScene.Mode.OVERWORLD)
                        action.tileMap.player.Move(Model.Entities.Terrain.Tile.Direction.EAST);
                    else if (action.mode == ActionScene.Mode.BATTLE)
                        action.battleMap.GetPlayer().Move(Model.Entities.Terrain.Tile.Direction.EAST);
                }
                else{
                    if (action.mode == ActionScene.Mode.BATTLE)
                        action.battleMap.GetPlayer().MoveReleased();
                }
                if (!oldState[(int)Buttons.Down] && currentState[(int)Buttons.Down]){
                    if (action.dialog.visible){
                        if (action.dialog.options != null)
                            if (action.dialog.optionCursor >= action.dialog.options.Length - 1)
                                action.dialog.optionCursor = 0;
                            else
                                action.dialog.optionCursor++;
                    }
                }
                if (!oldState[(int)Buttons.Up] && currentState[(int)Buttons.Up]){
                    if (action.dialog.visible){
                        if (action.dialog.options != null)
                            if (action.dialog.optionCursor <= 0)
                                action.dialog.optionCursor = action.dialog.options.Length - 1;
                            else
                                action.dialog.optionCursor--;
                    }
                }
                if (!oldState[(int)Buttons.A] && currentState[(int)Buttons.A]){
                    if (action.mode == ActionScene.Mode.OVERWORLD){
                        action.tileMap.player.AButton();
                    }
                }
                if (currentState[(int)Buttons.B]){
                    
                }
                if (currentState[(int)Buttons.Enter]){
                    if (action.mode == ActionScene.Mode.OVERWORLD)
                        action.tileMap.SetLighting(true);
                    else if (action.mode == ActionScene.Mode.BATTLE)
                        action.battleMap.SetNightTime(true);
                }
                if (currentState[(int)Buttons.Alt]){
                    if (action.mode == ActionScene.Mode.OVERWORLD)
                        action.tileMap.SetLighting(false);
                    else if (action.mode == ActionScene.Mode.BATTLE)
                        action.battleMap.SetNightTime(false);
                }
                if (currentState[(int)Buttons.Home]){
                }
            }
            else if(scene.kind == Scene.LOAD){
            }else if(scene.kind == Scene.EDITOR){
            }
        }
        public void Update(GameTime gameTime){
        }
        public void Draw(SpriteBatch spriteBatch){
            spriteBatch.Draw(DPad, DPadRect, Color.White);
            spriteBatch.Draw(A, ARect, Color.White);
            spriteBatch.Draw(B, BRect, Color.White);
            spriteBatch.Draw(Enter, EnterRect, Color.White);
            spriteBatch.Draw(Alt, AltRect, Color.White);
            spriteBatch.Draw(Home, HomeRect, Color.White);
        }

    }
}
