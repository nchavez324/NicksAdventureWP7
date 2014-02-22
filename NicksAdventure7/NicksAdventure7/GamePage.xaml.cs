using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NicksAdventure7.GameCore.Scenes;
using NicksAdventure7.GameCore.Input;
using NicksAdventure7.GameCore.Util;
using Microsoft.Xna.Framework.Audio;

namespace NicksAdventure7{
    public partial class Runner : PhoneApplicationPage{
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;
        int gW, gH, fps;
        public DataLoader dataLoader;
        InputManager input;

        GameScene scene;
        LoadScene load;
        IntroScene intro;
        ActionScene action;
        EditorScene editor;

        public Runner(){
            InitializeComponent();

            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;
            Global.loaded = false;
            Global.loading = false;
            Global.Running = false;
            // Create a timer for this page
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;
            SetWindowSize();
            Global.Content = contentManager;
        }
        private void SetWindowSize(){
            gW = (int)(Application.Current as App).RootFrame.ActualWidth;
            gH = (int)(Application.Current as App).RootFrame.ActualHeight;
            Global.camera = new Microsoft.Xna.Framework.Rectangle(0, 0, gW, gH);
            float aspectRatio = (float)Global.virtualScreen.Width / Global.virtualScreen.Height;
            float origRatio = (float)Global.camera.Width / Global.camera.Height;
            Global.screenScale = Matrix.CreateScale(
                Global.camera.Height / Global.virtualScreen.Height,
                Global.camera.Height / Global.virtualScreen.Height,
                1f);
            Global.paddleHeight = (int)(Global.camera.Height - aspectRatio * Global.camera.Width);
            Global.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(85), (float)Global.camera.Width / (float)Global.camera.Height, 0.001f, 100f);
            if (scene != null && scene.kind == Scene.EDITOR)
                ((EditorScene)scene).Resize();
            if (spriteBatch != null) Global.GraphicsDevice = spriteBatch.GraphicsDevice;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e){
            // Set the sharing mode of the graphics device to turn on XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);
            Global.GraphicsDevice = spriteBatch.GraphicsDevice;
            SetWindowSize();

            load = new LoadScene(this);
            scene = load;
            load.Initialize();
            load.running = true;

            timer.Start();
            base.OnNavigatedTo(e);
        }
        private void CustomLoad(){
            dataLoader = new DataLoader();
            Global.dataLoader = dataLoader;
            dataLoader.LoadAllContent(contentManager);

            intro = new IntroScene(this);
            action = new ActionScene(this);
            editor = new EditorScene(this);
            input = new InputManager(scene);
            Global.Running = true;

            Global.p = contentManager.Load<Texture2D>("Images/UI/white");
            //MediaPlayer.Play(Content.Load<Song>("Audio/Music/KnuxTheme"));
        }
        private void Load(System.Windows.Threading.Dispatcher dispatcher){
            //async?//await dispatcher.RunAsync(System.Windows.Core.CoreDispatcherPriority.High, () => CustomLoad());
            Action a = new Action(CustomLoad);
            dispatcher.BeginInvoke(a);

        }
        public void LoadBar(){
            /*
            loadingBar.Visibility = System.Windows.Visibility.Visible;
            loadingBar.Width = Global.camera.Width;
            loadingBar.Height = (int)(Global.camera.Height / 76.8f);
            loadingBar.UseLayoutRounding = true;
            loadingBar.Value = 0;
            loadingBar.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 6, 176, 37));
            loadingBar.Margin = new Thickness(0, (int)(Global.camera.Height * (9.87f / 10f) - loadingBar.Height), 0, 0);
            */
        }
        public void SwitchScenes(Scene newScene){
            SoundEffect s = contentManager.Load<SoundEffect>("Audio/SFX/switch");
            s.Play();
            if (scene.kind != newScene){
                scene.Stop();
                scene.Suspend();
                if (newScene == Scene.LOAD){
                    scene = load;
                }else if (newScene == Scene.INTRO){
                    scene = intro;
                }else if (newScene == Scene.ACTION){
                    scene = action;
                }else if (newScene == Scene.EDITOR){
                    scene = editor;
                }
                scene.running = true;
                scene.Activate();
                input.SwitchScenes(scene);
            }

        }
        protected override void OnNavigatedFrom(NavigationEventArgs e){
            // Stop the timer
            timer.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }
        /// <summary>
        /// Allows the page to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e){
            GameTime gameTime = new GameTime(e.TotalTime, e.ElapsedTime);
            if (!Global.loaded && !Global.loading){
                Global.loading = true;
                if (spriteBatch == null) spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);
                LoadBar();
                Load(Deployment.Current.Dispatcher);
            }
            if (load != null && Global.loading){
                load.Update(gameTime);
            }
            if (intro != null && !Global.loading){
                if (scene.kind == Scene.LOAD){
                    intro.Initialize();
                    action.Initialize();
                    editor.Initialize();
                    input.pad.Initialize();

                    SwitchScenes(Scene.INTRO);
                }
                input.Update();
                scene.Update(gameTime);
                if (scene.kind == Scene.INTRO)
                    SwitchScenes(Scene.ACTION);
            }
            fps = 1000 / e.ElapsedTime.Milliseconds;
        }
        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e){
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.CornflowerBlue);
            if (Global.loaded){
                Global.GraphicsDevice.Clear(Color.CornflowerBlue);
                spriteBatch.Begin();
                scene.Draw(spriteBatch);

                spriteBatch.Draw(Global.p, new Microsoft.Xna.Framework.Rectangle(0, Global.camera.Height - Global.paddleHeight, Global.camera.Width, Global.paddleHeight), Color.Black);
                //spriteBatch.Draw(Global.p, new Microsoft.Xna.Framework.Rectangle(0, Global.camera.Height - Global.barWidth,  Global.camera.Height, Global.barWidth), Color.Black);
                //spriteBatch.DrawString(contentManager.Load<SpriteFont>("Fonts/font"), padStatus + "", new Vector2(0, 0), Color.White);
                input.pad.Draw(spriteBatch);
                
                spriteBatch.DrawString(Global.Content.Load<SpriteFont>("Fonts/font"), Global.debugMessage + "\nFPS: " + fps, new Vector2(0, 0), Color.White);
                spriteBatch.End();
            }
            //On Load...
            if (Global.loading && load != null && !Global.loaded){
                spriteBatch.Begin();
                Global.GraphicsDevice.Clear(Color.CornflowerBlue);
                load.Draw(spriteBatch);
                spriteBatch.End();
            }
        }
    }
}