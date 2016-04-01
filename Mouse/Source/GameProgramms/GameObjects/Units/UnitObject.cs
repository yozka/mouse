using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Mouse.GameProgramms.GameObjects;
using Mouse.ScreenSystem;

namespace Mouse.GameProgramms.GameObjects.Units
{


    
     ///=====================================================================================
    ///
    /// <summary>
    /// Тип перемещения юнита
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public enum EUnitMove
    {
        None,
        Left,
        Right,
        Up,
        Down
    }
    ///--------------------------------------------------------------------------------------






     ///=====================================================================================
    ///
    /// <summary>
    /// Игровые юниты
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AUnitObject : AObject
    {

        ///--------------------------------------------------------------------------------------
        private const int c_unitWidth   = 100; //размер спрайта
        private const int c_unitHeight  = 90;
        private const int c_countStep   = 8;    //количество кадров в анимации

        private const int c_sizeWidth   = 50;   //размер игрового объекта
        private const int c_sizeHeight  = 50;   //размер игрового объекта
        private const int c_speedTime   = 1000; //время за которое проходит объект
        ///--------------------------------------------------------------------------------------

        private Texture2D   m_shadow; //тень юнита        
        private Texture2D   m_sprite; //спрайт для отрисовки
        
        private EUnitMove   m_move = EUnitMove.None;//тип движения
        private EUnitMove   m_moveNew = EUnitMove.None;//новый тип движения
        private TimeSpan    m_tileTime = TimeSpan.Zero;//Время, когда переключимся на новый вид

        private int         m_step = 0;//текущий кадр для отрисовки
        private int         m_stepSpeed = 0;//задержка для отрисовки кадра


        private Vector2     m_speedMove = new Vector2(); //вектор скорости перемещения пиксели/секунду

        private AActionAI   m_ai = null;//привязка на искуственный интелект

        private TimeSpan    m_flashTime = TimeSpan.Zero;//Время мигания юнита для задержки
        private bool        m_flash = false;//мерцание
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 1
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AUnitObject()
        {
            size = new Vector2(c_sizeWidth, c_sizeHeight);
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
        public AUnitObject(AActionAI actionAI)
        {
            ai = actionAI;
            size = new Vector2(c_sizeWidth, c_sizeHeight);
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Установка спрайта из ресурсов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Texture2D sprite
        {
            set
            {
                m_sprite = value;
            }
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// привязка искуственного интелекта к юниту
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AActionAI ai
        {
            set
            {
                m_ai = value;
            }
            get
            {
                return m_ai;
            }
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка контента для юнита, загружается тень
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onLoadContent(ContentManager content)
        {
            base.onLoadContent(content);
            m_shadow = content.Load<Texture2D>("Common/Units/shadow");
        }
        ///--------------------------------------------------------------------------------------


        



         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка тени
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onBeforeDraw(TimeSpan gameTime, ASpriteBatch spriteBatch)
        {
            spriteBatch.begin();
            spriteBatch.Draw(m_shadow, position - new Vector2(50, -5), Color.White);
            spriteBatch.end();
        }
        ///--------------------------------------------------------------------------------------





        
         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка юнита
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onDraw(TimeSpan gameTime, ASpriteBatch spriteBatch)
        {
  
            SpriteEffects effects = SpriteEffects.None;
            int sTop = 1;
            int sLeft = (c_unitWidth + 1) * (m_step + 1) + 1;
            switch (m_move)
            {
                case EUnitMove.None:
                    {
                        sTop = 183;
                        sLeft = 1;
                        break;
                    }
                case EUnitMove.Up:
                    {
                        sTop = 92; 
                        break;
                    }
                case EUnitMove.Down:
                    {
                        sTop = 183;
                        break;
                    }
                case EUnitMove.Left:
                    {
                        sTop = 1;
                        break;
                    }
                case EUnitMove.Right:
                    {
                        sTop = 1;
                        effects = SpriteEffects.FlipHorizontally;
                        break;
                    }
            }

            Vector2 size = new Vector2(c_unitWidth, c_unitHeight);


            
            /*
             * для теста, если не активный, то поставим полупрозрачность 
             */
            Color clrs = Color.White;
            if (!active && m_flash)
            {
                clrs = clrs * 0.1f;
            }

            spriteBatch.begin();
            spriteBatch.Draw(m_sprite, position - size / 2, new Rectangle(sLeft + 1, sTop + 1, c_unitWidth - 2, c_unitHeight - 2), clrs, 0, new Vector2(), 1, effects, 0);
            spriteBatch.end();
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// перемещение объекта
        /// задается вектор движения, вектор равняется 0, то стоим
        /// движемся до тех пор, пока не произойдет коллизия со стенами или другим объектом
        /// каждую коллизию обрабатывает AI
        /// 
        /// В качестве значения, передается вектор с количеством пикселей перемещения за одну секунду
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void moveStart(Vector2 shift)
        {
            if (shift.X == 0 && shift.Y == 0)
            {
                moveStop();
            }
            else
            {
                m_speedMove.X = shift.X / c_speedTime;
                m_speedMove.Y = shift.Y / c_speedTime;

                float ax = Math.Abs(shift.X);
                float ay = Math.Abs(shift.Y);
                EUnitMove msave = EUnitMove.None;

                if (shift.X > 0 && ax > ay)
                {
                    msave = EUnitMove.Right;
                }
                else
                    if (shift.X < 0 && ax > ay)
                    {
                        msave = EUnitMove.Left;
                    }
                    else
                        if (shift.Y > 0)
                        {
                            msave = EUnitMove.Down;
                        }
                        else
                        {
                            msave = EUnitMove.Up;
                        }

                if (m_move != msave && msave != m_moveNew)
                {
                    m_tileTime = TimeSpan.FromMilliseconds(200);//Время, когда нужно сменить тип движения
                }
                m_moveNew = msave;

            }
        }
        ///-------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Остановка движения юнита
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void moveStop()
        {
            m_speedMove = Vector2.Zero;
            if (m_moveNew != EUnitMove.None)
            {
                m_moveNew = EUnitMove.None;
                m_tileTime = TimeSpan.FromMilliseconds(200);
            }
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Проверка, можно или нет поставить новые координаты
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override bool onCheckPosition(Vector2 pos)
        {
            if (map == null)
            {
                return true;
            }
            Vector2 s = size / 2;
            Vector2 p1 = pos - s;
            Vector2 p2 = pos + s;
            return (p1.X < 0 || p1.Y < 0 || p2.X > map.size.X || p2.Y > map.size.Y) ? false : true;
        }
        ///--------------------------------------------------------------------------------------










         ///=====================================================================================
        ///
        /// <summary>
        /// Запускаем у объекта внутренний AI
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onUpdate(TimeSpan gameTime)
        {
            base.onUpdate(gameTime);
            /*
            * добавим анимацию
            *
            */
            m_stepSpeed += gameTime.Milliseconds;
            if (m_stepSpeed > 100)
            {
                m_stepSpeed = 0;
                m_step++;
                if (m_step >= c_countStep)
                {
                    m_step = 0;
                }
            }
            
            

            /*
             * отработка интелекта
             */
            if (m_ai != null)
            {
                m_ai.update(gameTime, this);
            }



            /*
             * обрабатываем активность
             * смотрим время и переводим его в активное состояние
             */
            if (!active || m_flash)
            {
                //при не активном режиме добавим мерцание
                m_flashTime += gameTime;
                if (m_flashTime.TotalMilliseconds > 200)
                {
                    m_flashTime = TimeSpan.Zero;
                    m_flash = !m_flash;
                }
            }



            /*
             * обрабатываем движения
             */
            if (m_speedMove != Vector2.Zero)
            {
                float fTime = gameTime.Milliseconds;
                Vector2 newPos = position + new Vector2(m_speedMove.X * fTime, m_speedMove.Y * fTime);
                if (onCheckPosition(newPos))
                {
                    position = newPos;
                }
                else
                {
                    //столкнулись с препятствием
                    if (m_ai != null)
                    {
                        m_ai.blockageMove(newPos, this);
                    }
                }
            }
            /* обрабатываем тип двжиения, смену
             * 
             */
            if (m_move != m_moveNew)
            {
                m_tileTime -= gameTime;
                if (m_tileTime.TotalMilliseconds < 0)
                {
                    m_move = m_moveNew;
                    m_tileTime = TimeSpan.Zero;
                }
            }
        }
        ///--------------------------------------------------------------------------------------




        






         ///=====================================================================================
        ///
        /// <summary>
        /// Возвращаем звук, который возникат при создании/появление юнита
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public virtual SoundEffect sound_new
        {
            get { return null; }
        }



    }//AUnitObject
}
