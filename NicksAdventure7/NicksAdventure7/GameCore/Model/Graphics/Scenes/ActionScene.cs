using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NicksAdventure7;
using NicksAdventure7.GameCore.Model.Graphics;
using NicksAdventure7.GameCore.Scenes;
using NicksAdventure7.GameCore.Model.Entities.Terrain;
using NicksAdventure7.GameCore.Model.Entities;
using NicksAdventure7.GameCore.Util;
using NicksAdventure7.GameCore.Model.Entities.Battle;
using NicksAdventure7.GameCore.Model.Scripting;
using NicksAdventure7.GameCore.Model.Entities.Events;

namespace NicksAdventure7.GameCore.Scenes{
    class ActionScene : NicksAdventure7.GameCore.Scenes.GameScene{

        public enum Mode{
            OVERWORLD, BATTLE
        };

        Dictionary<string, TileMap> tileMaps;
        Dictionary<string, BattleMap> battleMaps;
        public TileMap tileMap;
        public BattleMap battleMap;
        public Dialog dialog;

        public Mode mode;
        public Vector3 cameraPos = new Vector3(0, 0, 30);

        public ActionScene(NicksAdventure7.Runner core){
            this.core = core;
            kind = Scene.ACTION;
            components = new HashSet<Component>();
            tileMaps = new Dictionary<string, TileMap>();
            Content = Global.Content;
            Global.action = this;
            running = false;
            mode = Mode.OVERWORLD;
        }
        public void Initialize(){
            Global.camera.X = 0;
            Global.camera.Y = 0;
            //the first two indices are obviously x and y, although the third is NOT z!
            //the third stores any number of tiles when z can be queryed.
            //Note that this is not inefficient for the GraphicsCard since
            //the TileMap takes the three-dimensional and skips the null cells
            //when copying the vertices.
            TileMapParser mapLoader = new TileMapParser();
            TileMap demoMap = mapLoader.LoadMap("DemoMap");
            demoMap.player = new OWPlayer("Images/Characters/nickOW", new int[3]{1,1,0}, Tile.Direction.SOUTH);
            demoMap.Initialize();
            tileMaps.Add("DemoMap", demoMap);
            TileMap stairMap = mapLoader.LoadMap("StairMap");
            tileMaps.Add("StairMap", stairMap);
            tileMap = demoMap;

            battleMaps = new Dictionary<string, BattleMap>();
            Battler[] battlers = new Battler[1];
            battlers[0] = new Battler("Images/Characters/nickBattle", new Vector3(0, 0, 0));
            BattleMap testMap = new BattleMap("Ice_Path", battlers, 0);
            testMap.Initialize();
            battleMaps.Add("Ice_Path", testMap);
            battleMap = testMap;

            dialog = new Dialog();
            dialog.Initialize();
        }
        public override void Activate(){
        }
        public override void Suspend(){
        }
        public void SetTileMap(string mapName, int[] destPos, Tile.Direction dir){
            TileMap nextMap = tileMaps[mapName];
            nextMap.player = tileMap.player;
            nextMap.player.SetMapPos(destPos);
            nextMap.Initialize();
            nextMap.player.SetDir(dir);
            tileMap = nextMap;
        }
        public override void Update(GameTime gameTime){
            if (mode == Mode.OVERWORLD){
                tileMap.Update(gameTime);
                VertexPositionNormalTexture[] charVerts = tileMap.player.GetVertices();
                cameraPos.X = (charVerts[0].Position.X + charVerts[2].Position.X) / 2; cameraPos.Y = charVerts[0].Position.Y - Global.camera.Height / 120;
                cameraPos.Z = charVerts[0].Position.Z;
                /**long term camera issues here?**/
                Global.view = Matrix.CreateLookAt(new Vector3(cameraPos.X, cameraPos.Y, cameraPos.Z + 20 - .66f * (tileMap.player.GetMapPos()[Character.Y] + tileMap.player.travel.Y)), new Vector3(cameraPos.X, cameraPos.Y, cameraPos.Z), Vector3.UnitY);
                dialog.Update(gameTime);
            }else if (mode == Mode.BATTLE){
                battleMap.Update(gameTime);
                VertexPositionNormalTexture[] charVerts = battleMap.GetPlayer().GetVertices();
                cameraPos.X = (charVerts[0].Position.X + charVerts[2].Position.X) / 2; cameraPos.Z = charVerts[0].Position.Z - Global.camera.Height / 120;
                cameraPos.Y = charVerts[0].Position.Y;
                Global.view = Matrix.CreateLookAt(new Vector3(cameraPos.X, cameraPos.Y + 2, cameraPos.Z + 40), new Vector3(cameraPos.X, cameraPos.Y - 10, cameraPos.Z), Vector3.UnitY);
            }
        }
        public override void Draw(SpriteBatch spriteBatch){
            GraphicsDevice g = spriteBatch.GraphicsDevice;
            g.Clear(new Color(0 / (tileMap.nightTime ? 3 : 1), 122 / (tileMap.nightTime ? 3 : 1), 224 / (tileMap.nightTime ? 3 : 1)));
            g.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            if (mode == Mode.OVERWORLD){
                g.Clear(new Color(0 / (tileMap.nightTime ? 3 : 1), 122 / (tileMap.nightTime ? 3 : 1), 224 / (tileMap.nightTime ? 3 : 1)));
                tileMap.Draw(spriteBatch);
                dialog.Draw(spriteBatch);
            }else if (mode == Mode.BATTLE){
                g.Clear(new Color(0 / (battleMap.nightTime ? 3 : 1), 122 / (battleMap.nightTime ? 3 : 1), 224 / (battleMap.nightTime ? 3 : 1)));
                battleMap.Draw(spriteBatch);
            }

            //SpriteFont font = Global.Content.Load<SpriteFont>("Fonts/font");
            //spriteBatch.DrawString(font, "ActionScene", new Vector2(0, 25), Color.White);
        }
    }
}
