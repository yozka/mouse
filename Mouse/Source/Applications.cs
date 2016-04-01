using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Microsoft.Phone.Shell;


using Mouse.ScreenSystem;
using Mouse.Screens;
using Mouse.GameProgramms;
using Mouse.App;

namespace Mouse
{
    //---------------------------------------------------------------------------------------





    /* //===================================================================================*
    * //                                                                                    *
    * // Это главный тип игры																*
    * //------------------------------------------------------------------------------------*
    */
    public class AAplications : Microsoft.Xna.Framework.Game
    {
        //---------------------------------------------------------------------------------------
        public GraphicsDeviceManager graphics;
        public AScreenManager screenManager { get; set; }
        //---------------------------------------------------------------------------------------





        /* //===================================================================================*
        * //                                                                                    *
        * // Constructor            															*
        * //------------------------------------------------------------------------------------*
        */
        public AAplications()
        {
            //PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;

            graphics = new GraphicsDeviceManager(this);
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.PreferMultiSampling = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.IsFullScreen = true;

            Content.RootDirectory = "Content";

            // Частота кадра на Windows Phone по умолчанию — 30 кадров в секунду.
            //TargetElapsedTime = TimeSpan.FromTicks(333333);
            //TargetElapsedTime = TimeSpan.FromTicks(133333);

            // Дополнительный заряд аккумулятора заблокирован.
           // InactiveSleepTime = TimeSpan.FromMinutes(1);




            
            screenManager = new AScreenManager(this);
            Components.Add(screenManager);

            /*
             * Создание экранов
             */
            AGameLevel_1 gameScreen = new AGameLevel_1();



            /*
             * Создаем меню
             */
            AMainMenuScreen menu = new AMainMenuScreen();
            menu.items.addItem(gameScreen);
            menu.items.addItem(new AOptionsMenuScreen());
            menu.items.addItemExitGame();


            /*
             * Добавляем окна менеджеру
             */
            screenManager.addScreen(new ABackgroundScreen());
            screenManager.addScreen(menu);
            screenManager.addScreen(new ALogoScreen(TimeSpan.FromSeconds(2.0)));
        }
        //---------------------------------------------------------------------------------------












        /* //===================================================================================*
        * //                                                                                    *
        * // Позволяет игре выполнить инициализацию, необходимую перед запуском.                *
        * // Здесь можно запросить нужные службы и загрузить неграфический                      *
        * // контент.  Вызов base.Initialize приведет к перебору всех компонентов и             *
        * // их инициализации.                                                                  *
        * //------------------------------------------------------------------------------------*
        */
        protected override void Initialize()
        {
            // ЗАДАЧА: добавьте здесь логику инициализации

            base.Initialize();
        }
        //---------------------------------------------------------------------------------------




        //
        // Сводка:
        //     Создает событие Activated. Переопределите этот метод, чтобы добавить код
        //     для обработки при получении игрой фокуса.
        //
        // Параметры:
        //   sender:
        //     Объект Game.
        //
        //   args:
        //     Аргументы события Activated.
        protected override void OnActivated(object sender, EventArgs args)
        {
            base.OnActivated(sender, args);
            screenManager.music.resume();

        }

        
        
        
        //
        // Сводка:
        //     Создает событие Deactivated. Переопределите этот метод, чтобы добавить код
        //     для обработки при потере игрой фокуса.
        //
        // Параметры:
        //   sender:
        //     Объект Game.
        //
        //   args:
        //     Аргументы события Deactivated.
        protected override void OnDeactivated(object sender, EventArgs args)
        {
            screenManager.music.pause();
            base.OnDeactivated(sender, args);
        }



    }
}//namespace Mouse
