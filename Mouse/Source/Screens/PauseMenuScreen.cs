using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using Mouse.ScreenSystem;
using Mouse.GraphicsElement;



namespace Mouse.Screens
{


     ///=====================================================================================
    ///
    /// <summary>
    /// Меню с паузой
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class APauseMenuScreen : AMenuScreen
    {
        ///-------------------------------------------------------------------------------------
        private AScreen m_gameScreen = null;//указатель на экран с игрой (чтобы если чо его можно было закрыть)
        private ABackground m_back = null;
        private Song m_mediaMenu = null;
        ///-------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор меню пауза
        /// </summary>
        /// 
        ///-------------------------------------------------------------------------------------
        public APauseMenuScreen(AScreen gameScreen)
            : base("Pause")
        {
            m_gameScreen = gameScreen;
            isPopup = true;
        }
        ///-------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Также показывается что обрабатывает экран
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override EScreenContent getTypeContent()
        {
            return EScreenContent.Pause;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Инциализация заднего буфера
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onInitialized()
        {
            base.onInitialized();
            Viewport viewport = screenManager.GraphicsDevice.Viewport;
            Vector2 size = new Vector2(viewport.Width, viewport.Height);
            m_back = new ABackground(size, 50.0f);
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Обрабатываем кнопку бак, выход из меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onCancel()
        {
            exitScreen();
        }
        ///--------------------------------------------------------------------------------------












         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка меню "пауза"
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onLoadContent(ContentManager content)
        {
            base.onLoadContent(content);
            m_mediaMenu = content.Load<Song>("Common/Music/menu");

            /* добавим пункты меню
             */
            items.addItemExitMenu("Return game");
            items.addItem(new AOptionsMenuScreen());
            
            AMenuEntry item = items.addItem("Menu");
            item.selectEvent += returnMenu;

            items.addItemExitGame();
            items.loadContent(content);

            m_back.loadContent(content, "Common/Background/tileMenuPause");
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Выгрузка игрового контента
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onUnloadContent()
        {
            items.clear();
            base.onUnloadContent();
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// выбор пункта меню, возвратится в главное меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void returnMenu(AMenuScreen menu, AMenuEntry item)
        {
            exitScreen();
            m_gameScreen.exitScreen();
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка фонового экрана
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onDraw(TimeSpan gameTime)
        {
            ASpriteBatch spriteBatch = screenManager.spriteBatch;
            spriteBatch.begin();
            m_back.draw(gameTime, spriteBatch);
            spriteBatch.End();
            base.onDraw(gameTime);
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// активация фокуса у экрана
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onFocusActivation(AScreen focus)
        {
            screenManager.music.playRepeat(m_mediaMenu);
        }
        ///--------------------------------------------------------------------------------------





    }//AMainMenuScreen
}