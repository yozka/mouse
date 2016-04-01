using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mouse.ScreenSystem;


namespace Mouse.Screens
{



     ///=====================================================================================
    ///
    /// <summary>
    /// Базовый класс для экранов, которые содержат меню опций.Пользователь может
    /// Двигаться вверх и вниз для выбора записи, или Отмена, чтобы вернуться из экрана.
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public abstract class AMenuScreen : AScreen
    {
        public AMenuItems           items = null;
        protected AMenuEntry        selectedEntry = null; //выделенный пункт меню

        
        private const float         m_topTitle = 150; //позиция начала заголовка меню
        private const float         m_marginLogo = -80;//смещение лого в меню

        private const float         m_topBorder = 250;//верхняя позиция меню
        private const float         m_leftBorder = 300;//Левая позиция пунктов меню

        private string              m_menuTitle;
        private Texture2D           m_menuLogo;
        private Vector2             m_sizeLogo;

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
            return EScreenContent.Menu;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор интерфейса меню
        /// </summary>
        /// <param name="menuTitle">Название меню.</param>
        /// 
        ///--------------------------------------------------------------------------------------
        public AMenuScreen(string menuTitle)
        {
            items = new AMenuItems(this);
            m_menuTitle = menuTitle;
            transitionOnTime = TimeSpan.FromSeconds(0.45);
            transitionOffTime = TimeSpan.FromSeconds(0.3);
        }
        ///--------------------------------------------------------------------------------------





        
         ///=====================================================================================
        ///
        /// <summary>
        /// Цвет пункта 
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void ннн(string menuTitle)
        {

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
            m_menuLogo = content.Load<Texture2D>("Common/menuLogo");
            m_sizeLogo = new Vector2(m_menuLogo.Width, m_menuLogo.Height);
            items.loadContent(content);
        }
        ///--------------------------------------------------------------------------------------






  
      

         ///=====================================================================================
        ///
        /// <summary>
        /// Реагирования на пользовательский ввод, изменение выбранного элемента и его выбор
        //  или отмена в меню.
        /// </summary>
        /// <param name="input">Менеджер переферии для ввода информации.</param>
        /// 
        /// -------------------------------------------------------------------------------------
        public override void onHandleInput(AInputHelper input)
        {
            base.onHandleInput(input);
            
            // Обработка тачпада
            if (input.currentMouseState.X != input.lastMouseState.X ||
                input.currentMouseState.Y != input.lastMouseState.Y)
            {
                selectedEntry = items.getMenuEntryAt(input.currentMouseState);
            }


            if (input.isMouseLeftButton())
            {
                AMenuEntry item = items.getMenuEntryAt(input.lastMouseState);
                if (item != null)
                {
                    selectedEntry = item;
                    onSelectEntry(item);
                    screenManager.vibration.vibSelectMenu();
                    selectedEntry = null;//чтобы при выходе неподсвечивать выбранное меню
                }
            }


            /*
             * обработка меню через кнопки на телефоне
             */
            if (input.isMenuCancel())
            {
                onCancel();
            }
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Обработка когда пльзователь выбрал пункт меню
        /// </summary>
        /// <param name="entryIndex">Внутренний индекс выбранного пункта меню.</param>
        /// 
        ///--------------------------------------------------------------------------------------
        protected virtual void onSelectEntry(AMenuEntry item)
        {
            item.onSelect(this);
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Обработка когда пльзователь отменил пункт меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected virtual void onCancel()
        {
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
        protected virtual void updateMenuEntryLocations()
        {
            //делаем плавное перемещение меню, взависемости отнасколько показывается экран
            float transitionOffset = (float)Math.Pow(transitionPosition, 2);
            Vector2 position = new Vector2(0f, m_topBorder);

            /*
             * высчитаем отступы и высоту 
             */
            float fHeight = 0;
            foreach (AMenuEntry menuEntry in items.entries)
            {
                fHeight += menuEntry.getHeight(this);
            }
            Viewport viewport = screenManager.GraphicsDevice.Viewport;
            float fSpace = viewport.Height - fHeight - position.Y;
            float fSpaceHeight = fSpace / (items.count + 1);
            position.Y = m_topBorder + fSpace / 2;

            foreach (AMenuEntry menuEntry in items.entries)
            {

                // выставляем позицию по горизонтали
                position.X = m_leftBorder;

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
        /// Передаем метод обновления для всех вложенных меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onUpdate(TimeSpan gameTime, bool otherScreenHasFocus,
                                    bool coveredByOtherScreen)
        {
            base.onUpdate(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            if (screenState != EScreenState.Hidden)
            {
                items.update(gameTime, selectedEntry);
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
            base.onDraw(gameTime);
            // подсчитаем новые координаты пунктов меню
            updateMenuEntryLocations();

            GraphicsDevice graphics = screenManager.GraphicsDevice;
            ASpriteBatch spriteBatch = screenManager.spriteBatch;
            SpriteFont font = screenManager.spriteFonts.menuSpriteFont;


            // рисуем каждый пункт меню
            items.draw(gameTime, spriteBatch, selectedEntry);



            // отрисуем заголовок меню, он будет рисоватся сверху вниз, взависемости
            // от того показывается сейчас экран или нет
            // также здесь идет отрисовка картинка ЛОГО
            float transitionOffset = (float)Math.Pow(transitionPosition, 2);


            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2f, m_topTitle);
            Vector2 titleOrigin = font.MeasureString(m_menuTitle) / 2;
            const float titleScale = 0.9f;
            titlePosition.Y -= transitionOffset * 30;
            Vector2 logoPosition = new Vector2((graphics.Viewport.Width - m_sizeLogo.X) / 2f, titlePosition.Y - m_sizeLogo.Y - m_marginLogo);

            spriteBatch.begin();

            spriteBatch.Draw(m_menuLogo, logoPosition, Color.White);

            spriteBatch.DrawString(font, m_menuTitle, titlePosition, Color.White, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.end();
        }





    }//AMenuScreen
}