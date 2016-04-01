using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

using Mouse.Screens;
using Mouse.ScreenSystem;
using Mouse.GameProgramms.GameObjects;
using Mouse.GameProgramms.GameObjects.Units;
using Mouse.GameProgramms.Map;
using Mouse.App;
using Mouse.Particle;
using Mouse.GraphicsElement;


namespace Mouse.GameProgramms
{





    
    
     ///=====================================================================================
    ///
    /// <summary>
    /// Основная логика игры
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AGameLevel_1 : AScreen
    {
        private bool            m_gameOver = false; //признак того что игра закончилась
        private AMap            m_map = null; //Игровой уровень
        private AActionAvatar   m_avatar = null; //мы этой штукой управляем игровым объектом
        private int             m_score = 0; //Количество очков
        private int             m_countCheese = 0;//Количества сыра
        private int             m_countCats = 0; //Количество котов
        private AHeartHP        m_heartHP = null;
        private AParticleManager m_particle = null;//менеджер частиц
        private Song m_mediaSound = null;

        private AClearSrc       m_clearSrc = null;//спецэффект который скрывает экран
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор основной логики игры
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AGameLevel_1()
        {
            transitionOnTime = TimeSpan.FromSeconds(0.2);
            transitionOffTime = TimeSpan.FromSeconds(1.5);
            hidenState = EHidenState.Rotation;
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Заголовок окна, используется в меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override string getCaption()
        {
            return "Play game";
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Инциализация игрового экрана
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onInitialized()
        {
            m_particle = new AParticleManager(screenManager.particles);
            m_heartHP = new AHeartHP(new Vector2(500, 10), new AEffectHeartHP(m_particle));
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка игрового контента
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onLoadContent(ContentManager content)
        {
            m_mediaSound = content.Load<Song>("Common/Music/level_1");
            m_particle.loadContent(content);
            m_heartHP.loadContent(content);
            newGame();
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
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Выполнение игровой логики
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onUpdate(TimeSpan gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.onUpdate(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            if (!coveredByOtherScreen && !otherScreenHasFocus)
            {
                /*
                 * игровой цикл
                 */
                m_particle.update(gameTime);
                if (m_gameOver)
                {
                    gameOverClear(gameTime);
                }
                else
                {
                    m_map.updateObjects(gameTime);
                    if (!m_heartHP.isLive)
                    {
                        gameOver();
                    }
                }

                
            }
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Система ввода данных от пользователя
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onHandleInput(AInputHelper input)
        {
            if (input.isNewButtonPress(Buttons.Back))
            {
                gamePause();
            }
            base.onHandleInput(input);
            m_avatar.moveAcceleration(input.acceleration);
            m_map.onAcceleration(input.acceleration); //поменяем положение карты
            
            if (input.isTachpadDown())
            {
                m_avatar.moveTo(input.mousePosition);
            }


   
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка игровой ситуации
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onDraw(TimeSpan gameTime)
        {
            base.onDraw(gameTime);
            ASpriteBatch spriteBatch = screenManager.spriteBatch;
            //spriteBatch.begin();


            /* отрисовка поле для уровня
             */
            m_map.onDraw(gameTime, spriteBatch);


            /* отрисовка объектов на уровне
            */
            m_map.onDrawObjects(gameTime, spriteBatch);


            /* отрисуем частицы
            */
            m_particle.draw(gameTime, spriteBatch);


            /* финальная отрисовка поля
             */
            m_map.onAfterDraw(gameTime, spriteBatch);


            /* отрисовка информации по игроку, количество очков и жизней
             */
            m_heartHP.draw(gameTime, spriteBatch);


            /* отрисуем скрытие экрана, если оно есть
             * 
             */
            if (m_clearSrc != null)
            {
                m_clearSrc.draw(gameTime, spriteBatch);
            }

            Vector2 pos = new Vector2(20, 20);
            SpriteFont font = screenManager.spriteFonts.gameSpriteFont;


            spriteBatch.begin();
            spriteBatch.DrawString(font, "Score: " + m_score, pos, Color.Black, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
            spriteBatch.end();

            spriteBatch.begin();
            spriteBatch.DrawString(font, "Score: " + m_score, pos + new Vector2(2, 0), Color.White, 0, new Vector2(0, 0), 0.98f, SpriteEffects.None, 0);
            spriteBatch.end();



            //spriteBatch.End();
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// обработка сообщения, что мы находимся возле сыра!
        /// и даже взяли его
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void cheesEvent(AUnitObject unit, AObjCheese cheese)
        {
            if (unit.active && cheese.active)
            {
                cheese.remove();

                screenManager.vibration.vibGetCheese();
                m_countCheese++;
                m_score += m_countCheese * 100;

                AObjCheese che = new AObjCheese();
                che.loadContent(content);
                m_map.addObject(che);
                che.newPosition();
                while (m_map.isCollizion(che, unit))
                {
                    che.newPosition();
                }
                che.effect = new AEffectBoom(m_particle, TimeSpan.FromSeconds(2), Color.Yellow, Color.LightYellow); 

                //к юниту привяжем специфект
                unit.effect = new AEffectBoom(m_particle, TimeSpan.FromSeconds(0.5f), Color.Green, Color.GreenYellow); 

    

                /*
                 * добавим кота
                 */
                if (m_countCats < 10)
                {
                    nextLevelCats();
                }
            }

        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// обработка сообщения, что наткнулись на кота!
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void catsEvent(AUnitObject unit, AUnitCat cat)
        {
            if (unit.active && cat.active)
            {
                m_heartHP.lowHP();
                unit.suspendActive();
                //добавим эффект
                unit.effect = new AEffectHeartLife(m_particle, TimeSpan.FromSeconds(1));
                screenManager.vibration.vibCatAttacs();
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Следующий уровень, добавим котов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void nextLevelCats()
        {
            AGameHelper helper = AGameHelper.instance;
            AUnitObject obj = null;
            if (helper.random.Next(2) == 1)
            {
                obj = new AUnitCat(new AActionVertical());
            }
            else
            {
                obj = new AUnitCat(new AActionHorizontal());
            }
            obj.suspendActive(TimeSpan.FromSeconds(3));
            obj.loadContent(content);
            m_map.addObject(obj);
            m_countCats++;

            //добавим эффект
            obj.effect = new AEffectTrail(m_particle, TimeSpan.FromSeconds(2), Color.BlueViolet, Color.Blue);

            screenManager.vibration.vibNewCat();
            screenManager.sound.play(obj.sound_new);
    
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// новая игра
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void newGame()
        {
            m_clearSrc = null;
            m_gameOver = false;
            
            m_score = 0; //Количество очков
            m_countCheese = 0;//Количества сыра
            m_countCats = 0; //Количество котов
            m_heartHP.newGame();



            /*
             * создание карты
             */
            Viewport viewport = screenManager.GraphicsDevice.Viewport;
            m_map = new AMapGrass(new Vector2(viewport.Width, viewport.Height));



            /*
             * создание главгероя
             */
            AActionHeroMouse actHero = new AActionHeroMouse();
            actHero.cheesEvent += cheesEvent;//привязываем обработку на взятие сыра
            actHero.catsEvent += catsEvent;
            m_avatar = new AActionAvatar(actHero);
            m_map.addObject(new AUnitMouse(m_avatar));


 
            /*
             * создание одного сыра 
             */
            m_map.addObject(new AObjCheese());


            /*
             * загрузка объектов
             */
            m_map.loadContent(content);
            screenManager.Game.ResetElapsedTime();
            m_particle.clearParticle();
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Конец игры
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void gameOver()
        {
            m_gameOver = true;
            
            //создадим штуковину, которая очистит нам экран
            Viewport viewport = screenManager.GraphicsDevice.Viewport;
            m_clearSrc = new AClearSrc(new Vector2(viewport.Width, viewport.Height), m_avatar.lastPosition);
            m_clearSrc.loadContent(content);
        }
        ///--------------------------------------------------------------------------------------





        
         ///=====================================================================================
        ///
        /// <summary>
        /// Очистка экрана, и показываем заставку итоговых результатов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void gameOverClear(TimeSpan gameTime)
        {
            if (m_clearSrc == null || !m_clearSrc.updateTo(gameTime))
            {
                screenManager.addScreen(new AGameOverScreen(m_score));
                exitScreen();
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Пауза
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void gamePause()
        {
            /*
             * Создаем меню для паузы
            */
            APauseMenuScreen menu = new APauseMenuScreen(this);
            screenManager.addScreen(menu);
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
            screenManager.music.playRepeat(m_mediaSound);
        }
        ///--------------------------------------------------------------------------------------






    }//AGameProgramms
}