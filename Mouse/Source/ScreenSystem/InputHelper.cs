#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Controls;

using Mouse.App;
#endregion


namespace Mouse.ScreenSystem
{



     ///=====================================================================================
    ///
    /// <summary>
    /// Контроллер ввода пользовательской информации.
    /// Опрос тачпанелей и кнопок телефона
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AInputHelper
    {

        private GamePadState m_currentGamePadStates;
        private MouseState m_currentMouseState;
        private GamePadState m_lastGamePadStates;
        private MouseState m_lastMouseState;
        private TouchCollection m_touchState;
        private List<GestureSample> m_gestures = new List<GestureSample>();
        
        private Accelerometer m_accelerometer;
        private Vector3 m_avector; //считанные данные с акселерометра
        private Vector2 m_zeroZone = Vector2.Zero;//текущая нулевая зона для калибровки
        private Vector2 m_angle = new Vector2(1,1);//корректировка значения для расположение девайса
        private Vector2 m_accel = Vector2.Zero;//подсчитанные данные

        //фильтр средних значений на акселерометр
        private const int c_filterCount = 6;//количество данных в фильтре
        private Vector2[] m_filter;//новые подсчитанные данные
        private int m_filterID = 0;
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AInputHelper()
        {
            m_currentGamePadStates = new GamePadState();
            m_lastGamePadStates = new GamePadState();

            m_filter = new Vector2[c_filterCount];


            m_accelerometer = new Accelerometer();
            m_accelerometer.CurrentValueChanged += accelerometerChanged;
            m_accelerometer.Start();

            
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Чтение последнего состояния клавиатуры и геймпада.
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void update()
        {
            m_lastGamePadStates = m_currentGamePadStates;
            m_currentGamePadStates = GamePad.GetState(PlayerIndex.One);

            m_lastMouseState = m_currentMouseState;
            m_touchState = TouchPanel.GetState();
            if (m_touchState.Count > 0)
            {
                TouchLocation location = m_touchState[0];
                m_currentMouseState = new MouseState((int)location.Position.X, (int)location.Position.Y, 0, ButtonState.Pressed, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
            }
            else
            {
                m_currentMouseState = new MouseState(0, 0, 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
            }
            //m_currentMouseState = Mouse.GetState();

            m_gestures.Clear();
            while (TouchPanel.IsGestureAvailable)
            {
                m_gestures.Add(TouchPanel.ReadGesture());
            }
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Возврат текущего значения кнопок на телефоне.
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public GamePadState currentGamePadState
        {
            get { return m_currentGamePadStates; }
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Чтение прерыдущего значение кнопок на телефоне.
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public GamePadState lastGamePadState
        {
            get { return m_lastGamePadStates; }
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Чтение текущего значение тачпада и мышки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public MouseState currentMouseState
        {
            get { return m_currentMouseState; }
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Чтение предыдущего значения тачпада и мышки.
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public MouseState lastMouseState
        {
            get { return m_lastMouseState; }
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Отслеживание нажатие новой кнопки.
        /// </summary>
        /// 
        public bool isNewButtonPress(Buttons button)
        {
            return (m_currentGamePadStates.IsButtonDown(button) &&
                       m_lastGamePadStates.IsButtonUp(button));
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Проверка, польователь нажал хардвардную кнопку выхода из меню или нет.
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isMenuCancel()
        {
            return isNewButtonPress(Buttons.B) ||
                   isNewButtonPress(Buttons.Back);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Проверка, нажали ли левую кнопку мыщи или нжали на тачскрин
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isMouseLeftButton()
        {
            return m_currentMouseState.LeftButton == ButtonState.Released &&
                    m_lastMouseState.LeftButton == ButtonState.Pressed;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Проверка, нажали ли левую кнопку мыщи или нжали на тачскрин
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isTachpadDown()
        {
            return m_currentMouseState.X != m_lastMouseState.X ||
               m_currentMouseState.Y != m_lastMouseState.Y;
        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// Проверка, пользователь нажал паузу или нет во время игры.
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isPauseGame()
        {
            return isNewButtonPress(Buttons.Back) ||
                   isNewButtonPress(Buttons.Start);
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Текущая позиция мышки или тачпада
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Vector2 mousePosition
        {
            get { return new Vector2(m_currentMouseState.X, m_currentMouseState.Y); }
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Возврат смещения мышки.
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Vector2 mouseVelocity
        {
            get
            {
                return (
                           new Vector2(m_currentMouseState.X, m_currentMouseState.Y) -
                           new Vector2(m_lastMouseState.X, m_lastMouseState.Y)
                       );
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// обработка акселерометра
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void accelerometerChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            AccelerometerReading data = e.SensorReading;
            m_avector = new Vector3((float)data.Acceleration.X, (float)data.Acceleration.Y, (float)data.Acceleration.Z);

            /* высчитывание новой позиции
             */
            m_filter[m_filterID] = new Vector2(m_avector.Y, m_avector.X) * m_angle - m_zeroZone;
            m_filterID++;
            if (m_filterID >= c_filterCount)
            {
                m_filterID = 0;
            }

            calcFilter();
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// подсчет фильтра
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void calcFilter()
        {
            m_accel = m_filter[0];
            for (int i = 1; i < c_filterCount; i++)
            {
                m_accel += m_filter[i];
            }
            m_accel = m_accel / c_filterCount;

        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// Позиция акселерометра
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Vector2 acceleration
        {
            get
            {
                return m_accel;
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// начальная точка позицирования
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Vector2 zeroZone
        {
            get
            {
                return m_zeroZone;
            }
            set
            {
                m_zeroZone = value;
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// выдача текущей калибровки датчика
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Vector2 calibrationZeroZone()
        {

            return new Vector2(m_avector.Y, m_avector.X) * m_angle;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Обработка события на ориентации экрана
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void window_OrientationChanged(object sender, EventArgs e)
        {
            GameWindow window = sender as GameWindow;
            if (window != null)
            {
                orientationChanged(window.CurrentOrientation);
            }
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// устанавливаем корректирующие данные с учетом ориентации экрана
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void orientationChanged(DisplayOrientation orintat)
        {
            m_angle = new Vector2(-1, -1);
            if (orintat == DisplayOrientation.LandscapeRight)
            {
                m_angle = new Vector2(1, 1);
            }
        }




    }//AInputHelper
}