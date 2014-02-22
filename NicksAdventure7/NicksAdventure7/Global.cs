using NicksAdventure7.GameCore.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NicksAdventure7.GameCore.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NicksAdventure7{
    class Global{
        public static Rectangle camera, virtualScreen = new Rectangle(0, 0, 768, 768);
        public static int paddleHeight = 10;
        public static Texture2D p;
        public static Boolean loaded, loading, Running;
        public static ContentManager Content;
        public static GraphicsDevice GraphicsDevice;
        public static DataLoader dataLoader;
        public static ActionScene action;
        public static string debugMessage = "DEBUG";

        //Updated in ActionScene anyway
        public static Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 30), new Vector3(0, 0, 0), new Vector3(1, 0, 0));
        public static Matrix projection;
        public static Matrix screenScale;

        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rect, Color color){
            spriteBatch.Draw(Global.p, rect, color);
        }
        public static void DrawRectangleLine(SpriteBatch spriteBatch, Rectangle rect, int thickness, Color color){
            spriteBatch.Draw(Global.p, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
            spriteBatch.Draw(Global.p, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
            spriteBatch.Draw(Global.p, new Rectangle(rect.X + rect.Width - thickness, rect.Y, thickness, rect.Height), color);
            spriteBatch.Draw(Global.p, new Rectangle(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), color);
        }
        public static float Distance(Vector2 origin, Vector2 terminus){
            return (float)Math.Sqrt((Math.Pow((terminus.X - origin.X), 2)) + (Math.Pow((terminus.Y - origin.Y), 2)));
        }
        public static float Angle(Vector2 origin, Vector2 terminus){
            float a = (float)(-Math.Atan2(terminus.Y - origin.Y, terminus.X - origin.X));
            return (a < 0) ? (a += 2 * (float)Math.PI) : a;
        }
    }
}
