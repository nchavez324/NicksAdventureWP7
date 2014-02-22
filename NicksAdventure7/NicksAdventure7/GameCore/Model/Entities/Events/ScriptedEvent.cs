using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NicksAdventure7.GameCore.Model.Entities.Events{
    class ScriptedEvent : IEvent{
        EventPage[] pages;
        public EventPage page;

        public ScriptedEvent(EventPage[] pages, int[] mapPos, string name){
            this.pages = pages;
            foreach(EventPage p in pages){
                p.SetMapPos(mapPos);
            }
            foreach (EventPage p in pages)
                if (p != null)
                    p.name = name;
            page = pages[0];
        }
        public void Initialize(){
            foreach (EventPage p in pages){
                p.Initialize();
            }
        }
        public void Update(GameTime gameTime, OWPlayer player){
            foreach (EventPage p in pages){
                p.Update(gameTime, player);
            }
        }
        public VertexPositionNormalTexture[] GetVertices(){
            return page.GetVertices();
        }
        public Texture2D GetTexture(){
            return page.GetTexture();
        }
        public bool GetPassage(){
            return page.passage;
        }
        public int[] GetMapPos(){
            return new int[3] { page.GetMapPos()[Character.X], page.GetMapPos()[Character.Y], page.GetMapPos()[Character.Z] };
        }
        public void DebugDraw(SpriteBatch spriteBatch){

        }
        public void Trigger(OWPlayer player){
            page.Trigger(player);
        }
        public string GetName(){
            return page.name;
        }
        public bool Moving(){
            return page.Moving();
        }
        public bool IsForPlayerOnly(){
            return false;
        }
    }
}
