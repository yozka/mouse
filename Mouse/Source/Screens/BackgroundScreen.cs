using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mouse.ScreenSystem;
using Mouse.GraphicsElement;

namespace Mouse.Screens
{



     ///=====================================================================================
    ///
    /// <summary>
    /// Неподвижное фоновый экран, всегда рисуется независемо от ситуации
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class ABackgroundScreen : AScreen
    {
        private bool m_showLayer = true;
        private Texture2D m_layerTexture;
       
        private ABackground m_backMain = null;
        private ABackground m_backOptions = null;

        private ABackground m_currentBackground = null;
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public ABackgroundScreen()
        {
            transitionOnTime = TimeSpan.FromSeconds(0.1);
            transitionOffTime = TimeSpan.FromSeconds(0.1);
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
            Viewport viewport = screenManager.GraphicsDevice.Viewport;
            Vector2 size = new Vector2(viewport.Width, viewport.Height);
            m_backMain = new ABackground(size);
            m_backOptions = new ABackground(size);
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка картинок заднего фона игры
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onLoadContent(ContentManager content)
        {
            m_currentBackground = null;

            m_backMain.loadContent(content, "Common/Background/tileMenuMain");
            m_backOptions.loadContent(content, "Common/Background/tileMenuOptions");
            m_layerTexture = content.Load<Texture2D>("Common/Background/backLayer_1");

            m_showLayer = true;
            m_currentBackground = m_backMain;//поумолчанию всегда крутится главный тайл меню
        }
        ///--------------------------------------------------------------------------------------














         ///=====================================================================================
        ///
        /// <summary>
        /// Обновления фона экрана. В отличие от большинства экранов, этот не должен
        /// перейти в стотяниы "скрыто", даже если он был перекрыт другим экраном
        /// Эта перегрузка вынуждает
        /// CoveredByOtherScreen параметру значение false, чтобы остановить базу
        /// обновления.
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onUpdate(TimeSpan gameTime, bool otherScreenHasFocus,
                                    bool coveredByOtherScreen)
        {
            bool hideScreen = coveredByOtherScreen;
            switch (screenManager.focusContent)
            {
                case EScreenContent.Menu:
                    {
                        m_currentBackground = m_backMain;
                        hideScreen = false;
                        m_showLayer = true;
                        break;
                    }

                case EScreenContent.Options:
                    {
                        m_currentBackground = m_backOptions;
                        hideScreen = false;
                        m_showLayer = false;
                        break;
                    }
            }
            base.onUpdate(gameTime, otherScreenHasFocus, hideScreen);
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
            base.onDraw(gameTime);
            ASpriteBatch spriteBatch = screenManager.spriteBatch;
           
            if (m_currentBackground != null)
            {
                spriteBatch.begin();
                m_currentBackground.draw(gameTime, spriteBatch);
                spriteBatch.end();

            }

            if (m_showLayer)
            {
                spriteBatch.begin();
                if (screenState == EScreenState.Active)
                {
                    const float alpha = 0.91f;
                    spriteBatch.Draw(m_layerTexture, new Vector2(0, 0), Color.White * alpha);
                }
                else
                {
                    spriteBatch.Draw(m_layerTexture, new Vector2(0, 0), Color.White);
                }
                spriteBatch.end();

            }
        }



    }//ABackgroundScreen
}