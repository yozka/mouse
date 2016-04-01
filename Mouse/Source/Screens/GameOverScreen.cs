using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Mouse.ScreenSystem;
using Mouse.GraphicsElement;

namespace Mouse.Screens
{



    ///=====================================================================================
    ///
    /// <summary>
    /// Заставка игры.
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AGameOverScreen : AMenuScreen
    {
        private Song m_mediaMenu = null;
        private ABackground m_back = null;


        private Texture2D m_gameOver;
        private Texture2D m_background;

        private int m_score = 0;
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Определяется тип, группа экрана.
        /// Также показывается что обрабатывает экран
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override EScreenContent getTypeContent()
        {
            return EScreenContent.GameOver;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Конец игры
        /// </summary>
        /// <param name="duration">Время показа лого заставки.</param>
        /// 
        ///--------------------------------------------------------------------------------------
        public AGameOverScreen(int score)
            : base("GameOver")
        {
            transitionOnTime = TimeSpan.FromSeconds(1.0f);
            transitionOffTime = TimeSpan.FromSeconds(1.0f);
            m_score = score;
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
            m_back.rotationDirect = 1.0f;

            items.itemBackgroundColor = Color.Black * 0.9f;
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
            base.onLoadContent(content);
            m_background = content.Load<Texture2D>("Common/Background/GameOverScreen");
            m_gameOver = content.Load<Texture2D>("Common/Background/gameOverString");
            m_back.loadContent(content, "Common/Background/tileGameOver");
            m_mediaMenu = content.Load<Song>("Common/Music/game_over");



            /* добавим пункты меню
            */
            items.addItemExitMenu("Back");
            items.addItemExitGame();
            items.loadContent(content);


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
            m_background.Dispose();
            m_gameOver.Dispose();
            base.onUnloadContent();
        }
        ///--------------------------------------------------------------------------------------




















         ///=====================================================================================
        ///
        /// <summary>
        /// При нажатии кнопки назад, закрываем экран
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
        /// активация фокуса у экрана
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onFocusActivation(AScreen focus)
        {
            screenManager.music.playRepeat(m_mediaMenu);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// расположить пункты меню по умолчанию, когда открываем или закрываем экран
        /// Все элементы меню выстроены в виде вертикального списка, по центру экрана.
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void updateMenuEntryLocations()
        {
            //делаем плавное перемещение меню, взависемости отнасколько показывается экран
            float transitionOffset = (float)Math.Pow(transitionPosition, 2);
            Vector2 position = new Vector2(0f, 300);

            /*
             * высчитаем отступы и высоту 
             */
   
            float fSpaceHeight = 50f;

            foreach (AMenuEntry menuEntry in items.entries)
            {

                // выставляем позицию по горизонтали
                position.X = 600;

                if (screenState == EScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                // новая позиция
                menuEntry.position = position;

                // следующая позиция для другого меню
                position.Y += menuEntry.getHeight(this) + fSpaceHeight;
            }
        }
        ///--------------------------------------------------------------------------------------










         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка всех пунктов меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onDraw(TimeSpan gameTime)
        {
            // подсчитаем новые координаты пунктов меню
            updateMenuEntryLocations();

            ASpriteBatch spriteBatch = screenManager.spriteBatch;
            SpriteFont font = screenManager.spriteFonts.menuSpriteFont;
            

            spriteBatch.begin();
            spriteBatch.Draw(m_background, Vector2.Zero, Color.White);
            spriteBatch.end();

            spriteBatch.begin();
            m_back.draw(gameTime, spriteBatch);
            spriteBatch.end();

            // рисуем каждый пункт меню

            items.draw(gameTime, spriteBatch, selectedEntry);

            spriteBatch.begin();
            spriteBatch.Draw(m_gameOver, new Vector2(50, 50), Color.White);
            spriteBatch.end();


            string sScore = string.Format("Score: {0}", m_score);
           

            spriteBatch.begin();
            spriteBatch.DrawString(font, sScore, new Vector2(100, 250), Color.Black, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
            spriteBatch.end();

            spriteBatch.begin();
            spriteBatch.DrawString(font, sScore, new Vector2(102, 250), Color.BlueViolet, 0, new Vector2(0, 0), 0.98f, SpriteEffects.None, 0);
            spriteBatch.end();

        }


    }//AGameOverScreen
}
