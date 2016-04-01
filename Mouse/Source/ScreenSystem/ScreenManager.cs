using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Phone.Controls;

using Mouse.Particle;

namespace Mouse.ScreenSystem
{
    ///------------------------------------------------------------------------------------






     ///=====================================================================================
    ///
    /// <summary>
    /// Контекст отрисовки 2D изображения
    /// </summary>
    /// 
    /// -------------------------------------------------------------------------------------
    public class ASpriteBatch : SpriteBatch
    {
        public ASpriteBatch(GraphicsDevice graphicsDevice)
            :
            base(graphicsDevice)
        {
        }
        public void begin()
        {
            Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
            //Begin();
        }
        public void end()
        {
            End();
        }
        public void flush()
        {
            End();
            Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
        }


    }
    ///------------------------------------------------------------------------------------






     ///=====================================================================================
    ///
    /// <summary>
    /// Это менеджер кранов, управляет стеком экранов, отрисовкой, переходы между экранами
    /// Передает фокус экранов
    /// </summary>
    /// 
    /// -------------------------------------------------------------------------------------
    public class AScreenManager : DrawableGameComponent
    {
        /// <summary>
        /// Контекст для отрисовки спрайтов
        /// </summary>
        public ASpriteBatch spriteBatch;

        
        /// <summary>
        /// Камера для управление рендерингом экранов.
        /// </summary>
        public ACamera2D        camera;


        /// <summary>
        /// Контейнер шрифтов.
        /// </summary>
        public ASpriteFonts     spriteFonts;



        /// <summary>
        /// Контейнер настроек
        /// </summary>
        public AOptions         options;


        /// <summary>
        /// Система вибрации
        /// </summary>
        public AVibration       vibration;


        /// <summary>
        /// Система фоновой музыки
        /// </summary>
        public AMusic           music;


        /// <summary>
        /// Система звуковых эффектов
        /// </summary>
        public ASound           sound;



        
        /// <summary>
        /// фабрика частиц
        /// </summary>
        public AParticleFactory particles;


        /*
         * внутренние ресурсы менеджера
         */
        private ContentManager          m_contentManager = null;
        private Texture2D               m_blankTexture;
        private AInputHelper            m_input = new AInputHelper();
        private bool                    m_isInitialized;

        private List<AScreen>           m_screens = new List<AScreen>();
        private List<AScreen>           m_screensToUpdate = new List<AScreen>();

        private List<RenderTarget2D>    m_transitions = new List<RenderTarget2D>();

        private AScreen                 m_focusScreen = null;//текущий экран который находится в фокусе
        ///--------------------------------------------------------------------------------------
       
        /// FPS
        private int m_frameRate = 0;
        private int m_frameCounter = 0;
        private TimeSpan m_elapsedTime = TimeSpan.Zero;
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор менеджера экранов.
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AScreenManager(Game game)
            : base(game)
        {
            //инциализация пользовательских жестов
            TouchPanel.EnabledGestures = GestureType.None;
            m_contentManager = game.Content;


            /* подцепим обработчк слежения за расположением экрана
             */
            game.Window.OrientationChanged += new EventHandler<EventArgs>(m_input.window_OrientationChanged);
            m_input.orientationChanged(game.Window.CurrentOrientation);
        }
        ///--------------------------------------------------------------------------------------










         ///=====================================================================================
        ///
        /// <summary>
        /// Возвращаем тип текущего контента экрана.
        /// Эта информация понадобится, чтобы экран с бакгроундом мог отследить,
        /// какой контент сейчас рисуется для принития решения - рисовать или не рисовать бекграунд
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public EScreenContent focusContent
        {
            get 
            { 
                return m_focusScreen != null ? m_focusScreen.getTypeContent() : EScreenContent.None; 
            }
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Инциализация менеджера экранов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void Initialize()
        {
            vibration = new AVibration();
            music = new AMusic();
            sound = new ASound();
            spriteFonts = new ASpriteFonts(m_contentManager);
            options = new AOptions(m_input, vibration, music, sound);
            particles = new AParticleFactory();

            base.Initialize();
            m_isInitialized = true;
            foreach (AScreen screen in m_screens)
            {
                screen.initialized();
            }
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Освобождение памяти под буфер перехода между экранами
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void resetTargets()
        {
            m_transitions.Clear();
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка графического контента
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void LoadContent()
        {
            options.load();//загрузка настроек


            spriteBatch = new ASpriteBatch(GraphicsDevice);
            m_blankTexture = m_contentManager.Load<Texture2D>("Common/blank");
            camera = new ACamera2D(GraphicsDevice);


            /* загрузка отдлеьно, каждый экран
             * зедсь нужно последовательно загружать, сначало сплешь экран. 
             * потом в методе Update все остальное
             */
            foreach (AScreen screen in m_screens)
            {
                screen.initialized();
                screen.loadContent();
            }
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Выгрузка графического контента игры
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void UnloadContent()
        {
            // выгрузим у каждого экрана
            foreach (AScreen screen in m_screens)
            {
                screen.unloadContent();
            }
            m_blankTexture.Dispose();
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Обработка логики обновления экранов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void Update(GameTime snapshotGameTime)
        {
            TimeSpan gameTime = snapshotGameTime.ElapsedGameTime;
            
            //FPS
            m_elapsedTime += gameTime;
            if (m_elapsedTime.TotalSeconds > 1)
            {
                m_elapsedTime = TimeSpan.Zero;
                m_frameRate = m_frameCounter;
                m_frameCounter = 0;
            }
            //

            
            
            if (!Game.IsActive)
            {
                //если игра не активная, то нах ничего не делаем, чтобы незлоупортреблять ресурсами
                Game.SuppressDraw();
                return;
            }

            // опрашиваем тачпад и кнопки
            m_input.update();

            // обновляем данные в камере
            camera.update();

            // обрабатываем вибрацию
            vibration.update(gameTime);

            // Сделать копию экрана списка мастера, чтобы избежать путаницы, 
            // если процесс обновления одного экрана добавляет или удаляет другие экраны.
            m_screensToUpdate.Clear();
            foreach (AScreen screen in m_screens)
            {
                m_screensToUpdate.Add(screen);
            }


            bool otherScreenHasFocus = !Game.IsActive; //фокус имеется на другом экране
            bool coveredByOtherScreen = false;//признак того что еще монитор устройства не захвачен другим экраном
                                                //true - экран захвачен и его нужно скрыть

            // пробежимся по всем экранам, которые ждут обновления логики
            while (m_screensToUpdate.Count > 0)
            {
                // Вытащим самый верхний экран в стеке
                AScreen screen = m_screensToUpdate[m_screensToUpdate.Count - 1];
                m_screensToUpdate.RemoveAt(m_screensToUpdate.Count - 1);

                // Обновим логику в экране.
                screen.onUpdate(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.screenState == EScreenState.TransitionOn ||
                    screen.screenState == EScreenState.Active)
                {
                    // Если это первый экран в стеке, 
                    // то пердать ему фокус
                    if (!otherScreenHasFocus)
                    {
                        if (m_focusScreen != screen)
                        {
                            //передача фокуса
                            if (m_focusScreen != null)
                            {
                                m_focusScreen.onFocusDeactivation(screen);
                            }
                            screen.onFocusActivation(m_focusScreen);
                            m_focusScreen = screen;
                        }
                        
                        screen.onHandleInput(m_input);
                        otherScreenHasFocus = true; //следующие экраны получат информацию, что фокус занят
                    }

                    // если этот экран не является всплывающим экраном,
                    // то всем другим экранам скажем чтобы они скрылись из монитора
                    if (!screen.isPopup)
                        coveredByOtherScreen = true; //переведм в режим, чтобы последующие экраны скрылись
                }
            }


        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка экранов.
        /// В буфере рисуем экраны, которым необходимы спецэффкты перехода.
        /// Далее отрисовываем верхний экран
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void Draw(GameTime snapshotGameTime)
        {
            TimeSpan gameTime = snapshotGameTime.ElapsedGameTime;

            /*
             * подготавливаем буфер для отрисовки экранов с анимацие переходов
             */
            int transitionCount = 0;
            foreach (AScreen screen in m_screens)
            {
                if (screen.screenState == EScreenState.TransitionOn ||
                    screen.screenState == EScreenState.TransitionOff)
                {
                    ++transitionCount;
                    if (m_transitions.Count < transitionCount)
                    {
                        //подготавливаем буфер для отрисовки анимации перехода
                        PresentationParameters _pp = GraphicsDevice.PresentationParameters;
                        m_transitions.Add(new RenderTarget2D(GraphicsDevice, _pp.BackBufferWidth, _pp.BackBufferHeight,
                                                            false,
                                                            SurfaceFormat.Color, DepthFormat.Depth24Stencil8, _pp.MultiSampleCount,
                                                            RenderTargetUsage.DiscardContents));
                    }
                    GraphicsDevice.SetRenderTarget(m_transitions[transitionCount - 1]);
                    GraphicsDevice.Clear(Color.Transparent);
                    screen.onDraw(gameTime);
                    GraphicsDevice.SetRenderTarget(null);
                }
            }


            /*
             * отрисовка непосредственно экранов
             */
            GraphicsDevice.Clear(Color.SteelBlue);
            transitionCount = 0;
            foreach (AScreen screen in m_screens)
            {
                if (screen.screenState == EScreenState.Hidden)
                    continue;

                /*
                 * отрисовка экранов с эффектом перехода
                 */
                if (screen.screenState == EScreenState.TransitionOn ||
                    screen.screenState == EScreenState.TransitionOff)
                {
                    if (screen.screenState == EScreenState.TransitionOff && screen.hidenState == EHidenState.Rotation)
                    {
                        /*
                         * переход ввиде ротации по кругу
                         */
                        Vector2 _translate = camera.ScreenCenter +
                                             (new Vector2(64, camera.ScreenHeight - 64) - camera.ScreenCenter) *
                                             screen.transitionPosition * screen.transitionPosition;
                        float rotation = -4f * screen.transitionPosition * screen.transitionPosition;
                        float scale = screen.transitionAlpha * screen.transitionAlpha;
                        spriteBatch.begin();
                        spriteBatch.Draw(m_transitions[transitionCount],
                                          _translate, null,
                                          Color.White * screen.transitionAlpha,
                                          rotation, camera.ScreenCenter, scale, 0, 0);
                        spriteBatch.End();
                    }
                    else
                    {
                        /*
                         * переход с альфом каналом
                         */
                        spriteBatch.begin();
                        spriteBatch.Draw(m_transitions[transitionCount], Vector2.Zero, Color.White * screen.transitionAlpha);
                        spriteBatch.End();

                    }
                    ++transitionCount;
                }
                else
                {
                    //простая отрисовка, без перехода
                    screen.onDraw(gameTime);
                }
            }


            //FPS
            /*
            m_frameCounter++;
            string fps = string.Format("fps: {0}", m_frameRate);
            spriteBatch.begin();
            spriteBatch.DrawString(spriteFonts.gameSpriteFont, fps, new Vector2(33, 50), Color.Black);
            spriteBatch.End();
             */
        
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Добавление нового экрана в менеджер экранов
        /// </summary>
        /// <param name="screen">Добовляемый экран.</param>
        /// 
        /// -------------------------------------------------------------------------------------
        /// 
        public bool addScreen(AScreen screen)
        {
            if (screen == null)
            {
                return false;
            }
            
            /*
             * проверяем, есть ли данный экран или нет.
             * нужно исключить ситуацию, когда находятся два одинаковых экрана в менеджере
             */
            foreach (AScreen scr in m_screens)
            {
                if (scr == screen)
                {
                    return false; //такой экран ужо есть
                }
            }


            screen.screenManager = this;
            screen.isExiting = false;

            // если мы уже инцеализировались, то подгружаем контент
            if (m_isInitialized)
            {
                screen.initialized();
                screen.loadContent();
            }

            m_screens.Add(screen);

            // Указываем какие жесты мы будем отлавливать на тачскрине
            TouchPanel.EnabledGestures = screen.enabledGestures;
            screen.firstRun = false;
            return true;
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Немедленное удаление экрана, из пула менеджера. 
        /// Как правило, для выхода нужно исопльзовать GameScreen.ExitScreen 
        /// вместо вызова этого метода, 
        /// так на экране может осуществлятся постепенный переход на исчезновения экрана
        /// после того, экран будет полностью удален
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void removeScreen(AScreen screen)
        {
            // Если нашь девайс инцелизирован, то выгружаем непосредсвенно контент при выходе
            if (m_isInitialized)
            {
                screen.unloadContent();
            }

            m_screens.Remove(screen);
            m_screensToUpdate.Remove(screen);

            if (m_focusScreen == screen)
            {
                screen.onFocusDeactivation(null);
                m_focusScreen = null;
            }

            //если это не последний экран, то передаем в панель управления жестами другому экрану
            if (m_screens.Count > 0)
            {
                TouchPanel.EnabledGestures = m_screens[m_screens.Count - 1].enabledGestures;
            }
        }
        ///--------------------------------------------------------------------------------------









         ///=====================================================================================
        ///
        /// <summary>
        /// Возвратим массив экранов для обработки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AScreen[] getScreens()
        {
            return m_screens.ToArray();
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовываем затенение экрана, для спецэффектов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void fadeBackBufferToBlack(float alpha)
        {
            Viewport viewport = GraphicsDevice.Viewport;
            spriteBatch.begin();
            spriteBatch.Draw(m_blankTexture,
                              new Rectangle(0, 0, viewport.Width, viewport.Height),
                              Color.Black * alpha);
            spriteBatch.End();
        }


    }//AScreenManager
}