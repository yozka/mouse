using System;
using Microsoft.Devices;
using Microsoft.Xna.Framework;

namespace Mouse.ScreenSystem
{
    ///--------------------------------------------------------------------------------------






     ///=====================================================================================
    ///
    /// <summary>
    /// Система вибрации
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AVibration
    {
        ///--------------------------------------------------------------------------------------
        private bool m_vibration = false;
        private VibrateController vc = VibrateController.Default;


        private int[]       m_current = null; //текущий массив палитры вибраций
        private int         m_index = 0;//текущий индекс прослушивания в палитре
        private TimeSpan    m_timeNext = TimeSpan.Zero;


        /* описание партитуры вибрации в милесекундах
         * четные числа - вбирация
         * нечетные числа - ожидание
         * ------------------------------------------------- #       #       #
         */
        private readonly int[] m_vibSelectMenu  = new int[]{ 50, 100, 100 };
        private readonly int[] m_vibGetCheese   = new int[]{ 100, 100, 100 };
        private readonly int[] m_vibNewCat      = new int[]{ 50, 50, 50 };
        private readonly int[] m_vibCatAttacs   = new int[]{ 50, 100, 100, 100, 50 };
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Включенна выключена вибрация
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool enabled
        {
            get
            {
                return m_vibration;
            }
            set
            {
                m_vibration = value;
                if (!m_vibration)
                {
                    m_index = 0;
                    m_timeNext = TimeSpan.Zero;
                    m_current = null;
                }
            }
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Вибрация, всякая
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void update(TimeSpan gameTime)
        {
            if (m_vibration && m_current != null)
            {
                m_timeNext -= gameTime;
                if (m_timeNext.TotalMilliseconds < 0)
                {
                    //следующий сонг
                    int iLength = m_current.Length;
                    if (m_index < iLength)
                    {
                        //длительность звучания
                        int time = m_current[m_index];
                        vc.Start(TimeSpan.FromMilliseconds(time));
                        
                        //длительность паузы после звучания
                        m_index++;
                        if (m_index < iLength)
                        {
                            time += m_current[m_index];//пауза после звука
                            m_index++;//следующий сонг
                        }
                        m_timeNext = TimeSpan.FromMilliseconds(time);
                    }

                    //првоерка, если сонг проигран, то обнулим все
                    if (m_index >= iLength)
                    {
                        m_index = 0;
                        m_timeNext = TimeSpan.Zero;
                        m_current = null;
                    }
                }

                
            }

        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// выбор пункта меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void vibSelectMenu()
        {
            if (m_vibration)
            {
                m_index = 0;
                m_timeNext = TimeSpan.Zero;
                m_current = m_vibSelectMenu;
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Мыша взяла сыр
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void vibGetCheese()
        {
            if (m_vibration)
            {
                m_index = 0;
                m_timeNext = TimeSpan.Zero;
                m_current = m_vibGetCheese;
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Появился новый кот
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void vibNewCat()
        {
            if (m_vibration)
            {
                m_index = 0;
                m_timeNext = TimeSpan.Zero;
                m_current = m_vibNewCat;
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Уменьшились жизни, напали кошки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void vibCatAttacs()
        {
            if (m_vibration)
            {
                m_index = 0;
                m_timeNext = TimeSpan.Zero;
                m_current = m_vibCatAttacs;
            }
        }


    }
}
