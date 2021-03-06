﻿using System;
using Microsoft.Xna.Framework;
using Mouse.GameProgramms.Map;


namespace Mouse.GameProgramms.GameObjects.Units
{





     ///=====================================================================================
    ///
    /// <summary>
    /// Система аватара для управления игровым объектом
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AActionAvatar : AActionAI
    {

        private const float c_zoom = 900f; //подгонка значений для акселерометра
        private const float c_zero = 100f; //нулевая точка
        
        
        private Vector2 m_shift = Vector2.Zero;//перемещение
        private bool    m_newMove = false; //признак того что нужно установить данные
        private bool    m_newPositionCenter = false;//устанавливаем объект в центр карты


        private Vector2 m_movePosition = Vector2.Zero;
        private bool m_newPosition = false;//Двигать героя в данную позицию

        private Vector2 m_lastPosition = Vector2.Zero;//Последняя позиция последнего героя
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 1
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AActionAvatar()
            :
            base()
        {
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 2
        /// с привязкой обработчика интелекта
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AActionAvatar(AActionAI ai)
            :
            base(ai)
        {
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Обновление внутренней логики объекта
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onUpdate(TimeSpan gameTime, AUnitObject unit)
        {
            if (!unit.isPosition())
            {
                m_newPositionCenter = true;
            }

            if (m_newPositionCenter)
            {
                AMap map = unit.map;
                if (map != null)
                {
                    unit.position = map.size / 2 + new Vector2(0,unit.size.Y);

                    m_newPositionCenter = false;
                }
            }
            
            if (m_newMove || m_newPosition)
            {
                if (m_newPosition && m_movePosition != Vector2.Zero)
                {
                    //персонаж должен перейти на указанную позицию
                    Vector2 pos = unit.position;
                    Vector2 direct = m_movePosition - pos;
                    float dd = direct.Length();
                    direct.Normalize();
                    if (dd < 20f)
                    {
                        direct = Vector2.Zero;
                        m_movePosition = Vector2.Zero;
                        m_newPosition = false;
                    }
                    else
                    {
                        m_shift += direct * 300f;
                    }
                }

                unit.moveStart(m_shift);
                m_newMove = false;
                m_shift = Vector2.Zero;
            }

            m_lastPosition = unit.position;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Обработка столкновения с препятствием
        /// Останавливаем движение
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onBlockageMove(Vector2 newPosition, AUnitObject unit)
        {
            unit.moveStop();
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Обработка столкновения с препятствием
        /// Останавливаем движение
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void moveAcceleration(Vector2 shift)
        {
            m_shift = shift * c_zoom;

            if (m_shift.X > -c_zero && m_shift.X < c_zero &&
                m_shift.Y > -c_zero && m_shift.Y < c_zero)
            {
                m_shift.X = 0f;
                m_shift.Y = 0f;
            }            
            m_newMove = true;
        }
        ///--------------------------------------------------------------------------------------





        

         ///=====================================================================================
        ///
        /// <summary>
        /// Установка юнита в центр карты
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void setPositionCenter()
        {
            m_newPositionCenter = true;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Двигатся в указанную позицию
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void moveTo(Vector2 movePosition)
        {
            m_movePosition = movePosition;
            m_newPosition = true;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Стоп движению
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void moveStop()
        {
           // m_newPosition = false;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Последняя позиция героя
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Vector2 lastPosition
        {
            get
            {
                return m_lastPosition;
            }
        }




    }//AActionAvatar
}
