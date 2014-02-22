using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

using NicksAdventure7;



namespace NicksAdventure7.GameCore.Util{
    public class DataLoader{
        Dictionary<string, HashSet<string>> cache = null;
        public string log;
        public bool showBar;

        //Asset names
        string [] music = {
                              "KnuxTheme"
                          };
        string [] sfx = {
                            "switch", "A", "B", "EnterAlt"
                        };
        string [] fonts ={
                             "font", "dpFont"
                         };
        string [] images = {
                               "Characters/nickOW", "Characters/nickBattle",
                               "Characters/Jasmine",

                               "Events/slidingDoor",

                               "Textures/cliffFlat", "Textures/cliffSlope",
                               "Textures/snowFlat", "Textures/snowSlope",
                               "Textures/grassFlat", "Textures/grassSlope",

                               "Textures/Block",

                               "UI/RipStrike", "UI/white",
                               "UI/DPad", "UI/A", "UI/B",
                               "UI/Enter","UI/Alt", "UI/Home",
                               "UI/AnalogBase", "UI/AnalogPad",
                               "UI/txtBox0", "UI/txtBox1"
                                                              
                           };
        string[] models = {
                              "Buildings/BlockHouse",

                              "BattleMaps/Depot", "BattleMaps/Ice_Path"
                           };
        int numLoaded, allItems;

        public DataLoader(){
           log = "";
           allItems = music.Length + sfx.Length + fonts.Length + images.Length + models.Length;
           showBar = true;
        }
        private HashSet<string> Parse(string[] array){
            HashSet<string> ans = new HashSet<string>();
            foreach (string line in array){
                ans.Add(line);
            }
            return ans;
        }
        public HashSet<string> GetData(string fname){
            if (cache.ContainsKey(fname))
                return cache[fname];
            else
                return null;
        }
        /// <summary>
        /// Preload all the Content provided by string arrays.
        /// </summary>
        /// <param name="Content">The Content Manager.</param>
        public void LoadAllContent(ContentManager Content){
            
            cache = new Dictionary<string,HashSet<string>>();

            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            /*Windows.ApplicationModel.Package.Current.InstalledLocation*/
            string l = "Load Log:\n";
            foreach (string musicName in music){
                Content.Load<Song>("Audio/Music/" + musicName);
                l += "<" + musicName + ">\n";
                LoadContentIteration(timer);
            }
            foreach (string sfxName in sfx){
                Content.Load<SoundEffect>("Audio/SFX/" + sfxName);
                l += "<" + sfxName + ">\n";
                LoadContentIteration(timer);
            }
            foreach (string fontName in fonts){
                Content.Load<SpriteFont>("Fonts/" + fontName);
                l += "<" + fontName + ">\n";
                LoadContentIteration(timer);
            }
            foreach (string imageName in images){
                Content.Load<Texture2D>("Images/" + imageName);
                l += "<" + imageName + ">\n";
                LoadContentIteration(timer);
            }
            foreach (string modelName in models){
                Content.Load<Microsoft.Xna.Framework.Graphics.Model>("Models/" + modelName);
                l += "<" + modelName + ">\n";
                LoadContentIteration(timer);
            }
            Global.loaded = true;
            Global.loading = false;

            l += "============End Load=================";
            log = l;
            timer.Stop();
            long timeToLoad = timer.ElapsedMilliseconds;
            log += "\n Took " + timeToLoad + "ms to load\n";
        }
        private void LoadContentIteration(System.Diagnostics.Stopwatch timer){
            numLoaded++;
        }
        public int GetStatus(){
            return (int)(((float)numLoaded / (float)allItems) * 100f);
        }
    }
}
