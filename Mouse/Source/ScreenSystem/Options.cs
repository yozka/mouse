using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mouse.ScreenSystem;
using Mouse.Screens;

namespace Mouse.ScreenSystem
{
    ///--------------------------------------------------------------------------------------






     ///=====================================================================================
    ///
    /// <summary>
    /// Настройки игры
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AOptions
    {

        ///--------------------------------------------------------------------------------------
        private bool            m_load      = false; //признак того что загружены настройки
        private AVibration      m_vibration   = null;
        private AMusic          m_music       = null;
        private ASound          m_sound       = null;
        private AInputHelper    m_input       = null;//указатель на штуку воода информации
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AOptions(AInputHelper input, AVibration vibration, AMusic music, ASound sound)
        {
            m_input = input;
            m_vibration = vibration;
            m_music = music;
            m_sound = sound;
        }
        ///--------------------------------------------------------------------------------------











         ///=====================================================================================
        ///
        /// <summary>
        /// Настройка включена или невключена вибрация
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool vibrationEnabled
        {
            get
            {
                load();
                return m_vibration.enabled;
            }
            set
            {
                m_vibration.enabled = value;
                IsolatedStorageSettings.ApplicationSettings["vibration"] = m_vibration.enabled;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Настройка включена или невключена фоновая музыка
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool musicEnabled
        {
            get
            {
                load();
                return m_music.enabled;
            }
            set
            {
                m_music.enabled = value;
                IsolatedStorageSettings.ApplicationSettings["music"] = m_music.enabled;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Настройка включена или невключена звуковые спецэффекты
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool soundEnabled
        {
            get
            {
                load();
                return m_sound.enabled;
            }
            set
            {
                m_sound.enabled = value;
                IsolatedStorageSettings.ApplicationSettings["sound"] = m_sound.enabled;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }
        ///--------------------------------------------------------------------------------------








        ///=====================================================================================
        ///
        /// <summary>
        /// Нулевая точка для акселерометра
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Vector2 acceleration
        {
            get
            {
                load();
                return m_input.zeroZone;
            }
            set
            {
                m_input.zeroZone = value;
                IsolatedStorageSettings.ApplicationSettings["acceleration"] = m_input.zeroZone;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }
        ///--------------------------------------------------------------------------------------




    



         ///=====================================================================================
        ///
        /// <summary>
        /// загрузка настроек
        /// и установка всех параметров
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void load()
        {
            if (m_load)
            {
                return;
            }
            m_load = true; 

            /* вибрация
             */
            m_vibration.enabled = true;
            if (IsolatedStorageSettings.ApplicationSettings.Contains("vibration") == true)
            {
                m_vibration.enabled = (bool)IsolatedStorageSettings.ApplicationSettings["vibration"];
            }

            /* фоновая музыка
             */
            m_music.setDefault();
            if (IsolatedStorageSettings.ApplicationSettings.Contains("music") == true)
            {
                m_music.enabled = (bool)IsolatedStorageSettings.ApplicationSettings["music"];
            }


            /* звуковые эффекты
             */
            m_sound.enabled = true;
            if (IsolatedStorageSettings.ApplicationSettings.Contains("sound") == true)
            {
                m_sound.enabled = (bool)IsolatedStorageSettings.ApplicationSettings["sound"];
            }


            /* нулевая точка акселерометра
            */
            m_input.zeroZone = Vector2.Zero;
            if (IsolatedStorageSettings.ApplicationSettings.Contains("acceleration") == true)
            {
                m_input.zeroZone = (Vector2)IsolatedStorageSettings.ApplicationSettings["acceleration"];
            }

        }
        ///--------------------------------------------------------------------------------------










    }//AOptions
}
