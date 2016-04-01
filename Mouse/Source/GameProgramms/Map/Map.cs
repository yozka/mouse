using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Mouse.GameProgramms.GameObjects;
using Mouse.ScreenSystem;

namespace Mouse.GameProgramms.Map
{
    ///--------------------------------------------------------------------------------------








     ///=====================================================================================
    ///
    /// <summary>
    /// Базовая карта с игровыми объектами
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AMap : IDisposable 
    {


        //список привязанных объектов на карте
        private List<AObject> m_objects = new List<AObject>();
        private List<AObject> m_objectsWaitAdd = new List<AObject>();//новые объекты, добовляются отложенно
        private List<AObject> m_objectsWaitDel = new List<AObject>();//Объекты которые ждут удаления из списка
        private Vector2 m_sizeMap = new Vector2();//размерность карты
        private bool m_blocking = false;//признак того что нужно добовлять объекты в отложенный массив
        ///--------------------------------------------------------------------------------------


        



         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор карты
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AMap(Vector2 sizeMap)
        {
            m_sizeMap = sizeMap;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Деструктор, высвобождение ресурсов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void Dispose()
        {
            while (m_objects.Count > 0)
            {
                m_objects[0].map = null;
            }


            while (m_objectsWaitAdd.Count > 0)
            {
                m_objectsWaitAdd[0].map = null;
            }

            while (m_objectsWaitDel.Count > 0)
            {
                m_objectsWaitDel[0].map = null;
            }
        }
        ///--------------------------------------------------------------------------------------




        


         ///=====================================================================================
        ///
        /// <summary>
        /// Добавление объекта на карту
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void addObject(AObject obj)
        {
            if (obj.map != this)
            {
                if (m_blocking)
                {
                    m_objectsWaitAdd.Add(obj);
                    m_objectsWaitDel.Remove(obj);
                }
                else
                {
                    m_objects.Add(obj);
                }
                obj.map = this;
            }
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Удаление объекта из карты
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void removeObject(AObject obj)
        {
            m_objectsWaitAdd.Remove(obj);
            if (m_blocking)
            {
                if (!m_objectsWaitDel.Contains(obj))
                {
                    m_objectsWaitDel.Add(obj);
                }
            }
            else
            {
                m_objects.Remove(obj);
                m_objectsWaitDel.Remove(obj);
            }
            if (obj != null && obj.map != null)
            {
                obj.map = null;
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка контента для карты
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void loadContent(ContentManager content)
        {
            foreach (AObject obj in m_objects)
            {
                obj.loadContent(content);
            }
            onLoadContent(content);
        }
        ///--------------------------------------------------------------------------------------






        
         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка контента карт потомков
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected virtual void onLoadContent(ContentManager content)
        {
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Возвратим список объектов, которые находятся в заданном регионе
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public List<AObject> getObjects(AObject obj)
        {
            return getObjects(obj.position - obj.size / 2, obj.size);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Возвратим список объектов, которые находятся в заданном регионе
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public List<AObject> getObjects(Vector2 leftTop, Vector2 size)
        {
            float fLeft     = leftTop.X;
            float fRight    = leftTop.X + size.X;
            float fTop      = leftTop.Y;
            float fBottom   = leftTop.Y + size.Y;

            List<AObject> list = new List<AObject>();
            foreach (AObject obj in m_objects)
            {
                Vector2 msize = obj.size;
                Vector2 mpos = obj.position - msize / 2;

                float fobjLeft      = mpos.X;
                float fobjRight     = fobjLeft + msize.X;
                float fobjTop       = mpos.Y;
                float fobjBottom    = fobjTop + msize.Y;

                if (fLeft < fobjRight && fRight > fobjLeft &&
                    fTop < fobjBottom && fBottom > fobjTop)
                {
                    list.Add(obj);
                }
            }



            return list;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Проверка на коллизию двух объектов на карте
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isCollizion(AObject obj1, AObject obj2)
        {
            if (obj1.map == this && obj2.map == this)
            {

                Vector2 msize1 = obj1.size;
                Vector2 mpos1 = obj1.position - msize1 / 2;
                float fL1 = mpos1.X;
                float fR1 = fL1 + msize1.X;
                float fT1 = mpos1.Y;
                float fB1 = fT1 + msize1.Y;

                Vector2 msize2 = obj2.size;
                Vector2 mpos2 = obj2.position - msize2 / 2;
                float fL2 = mpos2.X;
                float fR2 = fL2 + msize2.X;
                float fT2 = mpos2.Y;
                float fB2 = fT2 + msize2.Y;

                if (fL1 < fR2 && fR1 > fL2 &&
                    fT1 < fB2 && fB1 > fT2)
                {
                    return true;
                }

            }
            return false;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка поля уровня
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public virtual void onDraw(TimeSpan gameTime, ASpriteBatch spriteBatch)
        {


        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Финальная отрисовка уровня
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public virtual void onAfterDraw(TimeSpan gameTime, ASpriteBatch spriteBatch)
        {


        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка всех объектов которые на ней находятся
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public virtual void onDrawObjects( TimeSpan gameTime, ASpriteBatch spriteBatch)
        {
            m_blocking = true;
            /*
             * отрисовка объектов
             */
            foreach (AObject obj in m_objects)
            {
                if (obj.map == this)
                {
                    obj.onBeforeDraw(gameTime, spriteBatch);
                }
            }
            foreach (AObject obj in m_objects)
            {
                if (obj.map == this)
                {
                    obj.onDraw(gameTime, spriteBatch);
                }
            }
            foreach (AObject obj in m_objects)
            {
                if (obj.map == this)
                {
                    obj.onAfterDraw(gameTime, spriteBatch);
                }
            }

            /*
            foreach (AObject obj in m_objects)
            {
                obj.onBeforeDraw(gameTime, spriteBatch);
            }
            foreach (AObject obj in m_objects)
            {
                obj.onDraw(gameTime, spriteBatch);
            }
            foreach (AObject obj in m_objects)
            {
                obj.onAfterDraw(gameTime, spriteBatch);
            }
            */
            m_blocking = false;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// размер карты в пикселях
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Vector2 size
        {
            get { return m_sizeMap; }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// обработка AI объектов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void updateObjects(TimeSpan gameTime)
        {
            //добовляем отложенных объектов
            if (m_objectsWaitAdd.Count > 0)
            {
                m_objects.AddRange(m_objectsWaitAdd);
                m_objectsWaitAdd.Clear();
            }

            //удаляем отложенных объектов
            if (m_objectsWaitDel.Count > 0)
            {
                foreach (AObject obj in m_objectsWaitDel)
                {
                    m_objects.Remove(obj);
                }
                m_objectsWaitDel.Clear();
            }


            m_blocking = true;
            foreach (AObject obj in m_objects)
            {
                obj.onUpdate(gameTime);
            }
            m_blocking = false;


            /*
             * отсортируем объекты для нормальной отрисовки 
             */
            m_objects.Sort(compareObjects);
        }
        ///--------------------------------------------------------------------------------------



        private static int compareObjects(AObject obj1, AObject obj2)
        {
            Vector2 pos1 = obj1.position;
            Vector2 pos2 = obj2.position;
        
            return (int)pos1.Y >= (int)pos2.Y ? 1 : -1;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Обработка акселерометра для эффекта положения карты
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public virtual void onAcceleration(Vector2 value)
        {


        }





    }//ALevelMap
}
