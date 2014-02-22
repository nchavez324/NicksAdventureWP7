using NicksAdventure7.GameCore.Model.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NicksAdventure7.GameCore.Model.Scripting{
    public struct wordSplit{
        public string s; public int iOff;
    };
    public class Dialog : Component{
        public bool visible;
        string text;
        string visibleText;
        public string[] options;
        static int charsPerLine = 28;
        static string startOp = "<<";
        static string endOp = ">>";
        int nextVisibleChar;
        public int optionCursor;
        bool usedOp = false;

        public delegate OptionCommands OptionCommands(string option);
        OptionCommands command;
        Texture2D messageTexture, optionsTexture;

        public Dialog(){
        }
        public void Initialize(){
            messageTexture = Global.Content.Load<Texture2D>("Images/UI/txtBox1");
            optionsTexture = Global.Content.Load<Texture2D>("Images/UI/txtBox1");
        }
        public OptionCommands CallDialog(string text, OptionCommands command){
            visible = true;
            nextVisibleChar = 0;
            this.text = text;

            visibleText = "";
            options = null;
            usedOp = false;
            //method for returning...
            this.command = command;
            Next();
            return command;
        }
        public void Next(){
            string res = "";
            //Given a queue of text, need to keep track of where we're showing text from
            //and when to stop showing, as well as when we hit an options menu
            //when it comes across an open tag, it should process it and then delete it
            //from the text.
            if (nextVisibleChar == -1){
                text = "";
                visibleText = "";
                visible = false;
                if (usedOp && command != null){
                    res = options[optionCursor];
                    options = null;
                    usedOp = false;
                    optionCursor = 0;
                    //is not letting you get that option in thurr
                    if(command != null)
                        command = command(res);
                    //what happens is that the first command gets here, drops the second one, which returns (it shouldn't...)
                    //and the third command is overriden by null here... I think command should return the next command??
                }
                return;
            }
            int j = 0;
            for (int i = nextVisibleChar; i < text.Length; i++){
                //Found option tag!!
                if (i + startOp.Length < text.Length && text.Substring(i, startOp.Length).CompareTo(startOp) == 0){
                    
                    //problem is youre setting up the option but split word chops off that last end
                    //You get in here when you hit the startOp
                    //then you wanna make a temp string minus the op and split it
                    //if the string is done, use op, else, send the op
                    //to the beginning of the nextVisibleChar
                    string debug = text.Substring(nextVisibleChar);
                    int cutoutLen = text.Substring(i).IndexOf(endOp) + endOp.Length;
                    string temp = text.Substring(nextVisibleChar).Remove(i-nextVisibleChar, cutoutLen);
                    //temp has removed option data... split it!! see if it fits...

                    
                    string option = text.Substring(i + startOp.Length, text.Substring(i + startOp.Length).IndexOf(endOp));
                    string textPt1 = text.Substring(0, i);
                    visibleText = temp;
                    if (visibleText.Length > charsPerLine){
                        wordSplit s = SplitWords(visibleText, i);
                        visibleText = s.s;
                        //there is either leftover or not
                        //if leftover, then def proceed and adjust nextVisibleChar AND send option to next piece
                        //if no leftover, then you either got a clean break or really are done AND do option...
                        if (s.iOff < 0){
                            nextVisibleChar += visibleText.Length;//for newline

                            if (text.Length > i + startOp.Length + cutoutLen + endOp.Length)
                                textPt1 += text.Substring(i + startOp.Length + cutoutLen + endOp.Length);
                            text = textPt1.Trim();
                            //then add option back in..
                            text = text.Insert(nextVisibleChar, startOp + option + endOp);

                        }else{
                            string d = text.Substring(nextVisibleChar + visibleText.Length);
                            int a = nextVisibleChar + visibleText.Length - 1 + text.Count((c)=>{
                                return (c == ' ');
                            });
                            int b = text.Length - 1 - startOp.Length - cutoutLen - endOp.Length;
                            if (a < b){
                                nextVisibleChar += visibleText.Length - 1;
                                if (text.Length > i + startOp.Length + cutoutLen + endOp.Length)
                                    textPt1 += text.Substring(i + startOp.Length + cutoutLen + endOp.Length);
                                text = textPt1.Trim();
                                //then add option back in..
                                text = text.Insert(nextVisibleChar, startOp + option + endOp);
                            }else{
                                nextVisibleChar = -1;
                                usedOp = true;
                                options = option.Split(new char[1] { ';' });
                                //this is the modification of text to remove the option once it has been processed..
                                if (text.Length > i + startOp.Length + cutoutLen + endOp.Length)
                                    textPt1 += text.Substring(i + startOp.Length + cutoutLen + endOp.Length);
                                text = textPt1.Trim();
                            }
                        }
                        //it is going to break and return here to command = null check.
                        //proceed with next command here??
                        return;
                    }else{
                        usedOp = true;
                        options = option.Split(new char[1] { ';' });
                        //this is the modification of text to remove the option once it has been processed..
                        int m = i + cutoutLen;
                        if (text.Length > m)
                            textPt1 += text.Substring(i + cutoutLen);
                        text = textPt1.Trim();
                        visibleText = text.Substring(nextVisibleChar);
                        nextVisibleChar = -1;
                        break;
                    }
                
                }
                if (i >= text.Length - 1){
                    visibleText = text.Substring(nextVisibleChar);
                    if (visibleText.Length > charsPerLine){
                        wordSplit s = SplitWords(visibleText, i);
                        visibleText = s.s;
                        //there is either leftover or not
                        //if leftover, then def proceed and adjust nextVisibleChar
                        //if no leftover, then you either got a clean break or really are done
                        if (s.iOff < 0){
                            nextVisibleChar += visibleText.Length;//for newline
                        }else{
                            if (nextVisibleChar + visibleText.Length - 1 < text.Length - 1){
                                nextVisibleChar += visibleText.Length - 1;
                            }else{
                                nextVisibleChar = -1;
                            }
                        }
                    }else{
                        //DONE!!!!
                        nextVisibleChar = -1;
                        break;
                    }
                    break;
                }else if (j > (charsPerLine * 2)){
                    visibleText = text.Substring(nextVisibleChar, j);
                    nextVisibleChar = i;
                    wordSplit s = SplitWords(visibleText, i);
                    visibleText = s.s;
                    //if leftover, then def proceed and adjust nextVisibleChar
                    //if no leftover, then you either got a clean break or really are done
                    nextVisibleChar += s.iOff;
                    break;
                }
                j++;
            }

            return;
        }
        private wordSplit SplitWords(string s, int i){
            //given a string with the correct number of charsPerLine*2...
            //FIX!!
            string o, t = "", l = "";
            o = s.Substring(0, s.Substring(0, charsPerLine).LastIndexOf(' '));
            if (s.Length > o.Length){
                int c;
                if(s.Length > o.Length + charsPerLine)
                    c = s.Substring(0, o.Length + charsPerLine).LastIndexOf(' ');
                else
                    c = s.Substring(0).LastIndexOf(' ') + 1;
                int b = c - o.Length - 1;
                t = s.Substring(s.Substring(0, charsPerLine).LastIndexOf(' ') + 1, b);
                if (s.Length > o.Length + t.Length + 2){
                    l = s.Substring(s.Substring(0, o.Length + t.Length + 2).LastIndexOf(' ') + 1);
                }
            }
            s = o + "\n" + t;
            wordSplit ws = new wordSplit();
            ws.s = s;

            //return what for i????
            if (l.CompareTo("") == 0)
                ws.iOff = 0;
            else
                ws.iOff = -l.Length;
            return ws;
        }
        public void Update(GameTime gameTime){

        }
        public void Draw(SpriteBatch spriteBatch){
            if (visible){
                //Global.DrawRectangle(spriteBatch, new Rectangle(0, Global.camera.Height - Global.paddleHeight - 60, Global.camera.Width, 60), Color.White);
                int boxHeight = (int)(messageTexture.Height * 3f);
                spriteBatch.Draw(messageTexture, new Rectangle(0, Global.camera.Height - Global.paddleHeight - boxHeight, 2*(messageTexture.Width / 3), boxHeight), new Rectangle(0, 0, messageTexture.Width / 3, messageTexture.Height), Color.White);
                spriteBatch.Draw(messageTexture, new Rectangle(2*(messageTexture.Width / 3), Global.camera.Height - Global.paddleHeight - boxHeight, Global.camera.Width - (messageTexture.Width / 3), boxHeight), new Rectangle(messageTexture.Width / 3, 0, messageTexture.Width / 3, messageTexture.Height), Color.White);
                //spriteBatch.Draw(messageTexture, new Rectangle(Global.camera.Width - (messageTexture.Width / 3), Global.camera.Height - Global.paddleHeight - boxHeight, messageTexture.Width / 3, boxHeight), new Rectangle(2 * (messageTexture.Width / 3), 0, messageTexture.Width / 3, messageTexture.Height), Color.White);
                string f = visibleText, s = "";
                if (visibleText.IndexOf("\n") != -1){
                    f = visibleText.Substring(0, visibleText.IndexOf("\n"));
                    s = visibleText.Substring(visibleText.IndexOf("\n") + 1);
                }
                int vertSpace = 10;
                spriteBatch.DrawString(Global.Content.Load<SpriteFont>("Fonts/dpFont"), f, new Vector2(2 * (messageTexture.Width / 3), Global.camera.Height - Global.paddleHeight - boxHeight + (int)(boxHeight / 6f)), Color.Black);
                spriteBatch.DrawString(Global.Content.Load<SpriteFont>("Fonts/dpFont"), s, new Vector2(2 * (messageTexture.Width / 3), Global.camera.Height - Global.paddleHeight - boxHeight / 2 + vertSpace), Color.Black);
                if (options != null){
                    float charToPix = 17.5f;
                    int lenLongest = 1;
                    for (int i = 0; i < options.Length; i++)
                        if (options[i].Length > lenLongest)
                            lenLongest = options[i].Length;
                    //new Rectangle(Global.camera.Width - (int)(lenLongest * charToPix + Global.camera.Width/22 + Global.camera.Width/22), Global.camera.Height - Global.paddleHeight - boxHeight - 40 * options.Length - (int)(boxHeight / 7f), (int)(lenLongest * charToPix + Global.camera.Width/11), 40 * options.Length + (int)(boxHeight / 6f))
                    spriteBatch.Draw(optionsTexture, new Rectangle(Global.camera.Width - (int)(lenLongest * charToPix + 4 * (optionsTexture.Width / 3)), Global.camera.Height - Global.paddleHeight - boxHeight - 40 * options.Length - (int)(boxHeight / 7f), 2 * (optionsTexture.Width / 3), 40 * options.Length + (int)(boxHeight / 6f)), new Rectangle(0, 0, optionsTexture.Width / 3, optionsTexture.Height), Color.White);
                    spriteBatch.Draw(optionsTexture, new Rectangle(Global.camera.Width - (int)(lenLongest * charToPix + 2 * (optionsTexture.Width / 3)), Global.camera.Height - Global.paddleHeight - boxHeight - 40 * options.Length - (int)(boxHeight / 7f), (int)(lenLongest * charToPix) + 2*(optionsTexture.Width/3), 40 * options.Length + (int)(boxHeight / 6f)), new Rectangle((optionsTexture.Width / 3), 0, optionsTexture.Width / 3, optionsTexture.Height), Color.White);

                    for (int i = 0; i < options.Length; i++){
                        Color c = Color.Black;
                        if (i == optionCursor)
                            //160, 208, 224
                            //72, 112, 160
                            //116, 160, 192
                            //104, 191, 255
                            c = new Color(95, 182, 246);
                        spriteBatch.DrawString(Global.Content.Load<SpriteFont>("Fonts/dpFont"), options[i], new Vector2(Global.camera.Width - (int)(lenLongest * charToPix + 2 * (optionsTexture.Width / 3)), Global.camera.Height - Global.paddleHeight - boxHeight - 40 * options.Length + 40 * i), c);
                    }
                    //Global.DrawRectangleLine(spriteBatch, new Rectangle(Global.camera.Width - Global.camera.Width / 5, Global.camera.Height - Global.paddleHeight - 60 - 20 * options.Length + 20 * optionCursor, Global.camera.Width / 5, 20), 1, Color.Black);
                }
            }
        }
    }
}