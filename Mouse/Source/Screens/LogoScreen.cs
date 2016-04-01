using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Mouse.ScreenSystem;

namespace Mouse.Screens
{



    ///=====================================================================================
    ///
    /// <summary>
    /// Заставка игры.
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class ALogoScreen : AScreen
    {
        private Rectangle m_destination;
        private TimeSpan m_duration;
        private Texture2D m_logoTexture;
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор заставки игры
        /// </summary>
        /// <param name="duration">Время показа лого заставки.</param>
        /// 
        ///--------------------------------------------------------------------------------------
        public ALogoScreen(TimeSpan duration)
        {
            m_duration = duration;
            transitionOffTime = TimeSpan.FromSeconds(2.0);
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка картинки логотипа на заставку
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onLoadContent(ContentManager content)
        {
            m_logoTexture = content.Load<Texture2D>("Common/Background/LogoScreen");


            Viewport viewport = screenManager.GraphicsDevice.Viewport;
            Vector2 logoSize = new Vector2(m_logoTexture.Width, m_logoTexture.Height);
            Vector2 logoPosition = screenManager.camera.ScreenCenter - logoSize / 2f;

            m_destination = new Rectangle((int)logoPosition.X, (int)logoPosition.Y, (int)logoSize.X, (int)logoSize.Y);

        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка картинки логотипа на заставку
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onUnloadContent()
        {
            m_logoTexture.Dispose();

        }
        ///--------------------------------------------------------------------------------------














        ///=====================================================================================
        ///
        /// <summary>
        /// Перехват нажатия кнопок
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onHandleInput(AInputHelper input)
        {
            base.onHandleInput(input);
            if (input.currentGamePadState.IsButtonDown(Buttons.A | Buttons.Start | Buttons.Back) ||
                input.currentMouseState.LeftButton == ButtonState.Pressed)
            {
                m_duration = TimeSpan.Zero;
            }
        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// Логика заставки, при истечении времени, сама закрывается
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onUpdate(TimeSpan gameTime, bool otherScreenHasFocus,
                                    bool coveredByOtherScreen)
        {
            m_duration -= gameTime;
            if (m_duration <= TimeSpan.Zero)
            {
                exitScreen();
            }
            base.onUpdate(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка заставки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onDraw(TimeSpan gameTime)
        {
            base.onDraw(gameTime);
            screenManager.GraphicsDevice.Clear(Color.White);

            screenManager.spriteBatch.begin();
            screenManager.spriteBatch.Draw(m_logoTexture, m_destination, Color.White);
            screenManager.spriteBatch.End();
        }





    }//ALogoScreen
}