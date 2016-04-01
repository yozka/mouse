using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Mouse.ScreenSystem;
using Mouse.GameProgramms.Map;
using Mouse.GameProgramms.GameObjects.Units;

namespace Mouse.Screens
{



     ///=====================================================================================
    ///
    /// <summary>
    /// Неподвижное фоновый экран, всегда рисуется независемо от ситуации
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AAccelerationScreen : AMenuScreen
    {
        private Texture2D       m_backgroundTexture;
        private Vector2         m_viewport;

        private AMap            m_map = null;
        private AActionAvatar   m_avatar = null;

        private Vector2         m_zeroCalibration = Vector2.Zero;

        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AAccelerationScreen()
            : base("Acceleration")
        {
            transitionOnTime = TimeSpan.FromSeconds(1.0);
            transitionOffTime = TimeSpan.FromSeconds(1.0);
        }
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
            return EScreenContent.Acceleration;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Заголовок экрана, используется в меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override string getCaption()
        {
            return "Acceleration";
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
        /// Загрузка картинок заднего фона игры
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onLoadContent(ContentManager content)
        {
            Viewport viewport = screenManager.GraphicsDevice.Viewport;
            m_viewport = new Vector2(viewport.Width, viewport.Height);
            m_backgroundTexture = content.Load<Texture2D>("Common/Background/AccelerationScreen");


            m_map = new AMap(m_viewport);



            m_avatar = new AActionAvatar();
            m_map.addObject(new AUnitMouse(m_avatar));

            m_map.loadContent(content);


            /*
            * создадим меню
            */
            AMenuEntry item = items.addItem("Calibration");
            item.selectEvent += calibrationMenu;
            items.addItemExitMenu("Back");


            base.onLoadContent(content);
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
            m_map.Dispose();
            items.clear();
            base.onUnloadContent();
        }
        ///--------------------------------------------------------------------------------------









         ///=====================================================================================
        ///
        /// <summary>
        /// Обновления фона экрана. 
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onUpdate(TimeSpan gameTime, bool otherScreenHasFocus,
                                    bool coveredByOtherScreen)
        {
            if (!coveredByOtherScreen && !otherScreenHasFocus)
            {
                m_map.updateObjects(gameTime);
            } 
            
            base.onUpdate(gameTime, otherScreenHasFocus, coveredByOtherScreen);
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
            m_avatar.moveAcceleration(input.acceleration);
            m_zeroCalibration = input.calibrationZeroZone();
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
            spriteBatch.Draw(m_backgroundTexture, new Vector2(0, 0), Color.White);
            spriteBatch.end();
            m_map.onDraw(gameTime, spriteBatch);
            m_map.onDrawObjects(gameTime, spriteBatch);

            base.onDraw(gameTime);
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// выбор пункта меню калибровка
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void calibrationMenu(AMenuScreen menu, AMenuEntry item)
        {
            screenManager.options.acceleration = m_zeroCalibration;
            m_avatar.setPositionCenter();
        }
        ///--------------------------------------------------------------------------------------




    }//ABackgroundScreen
}