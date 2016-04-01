using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mouse.ScreenSystem;

namespace Mouse.Particle
{
    ///--------------------------------------------------------------------------------------






     ///=====================================================================================
    ///
    /// <summary>
    /// Менеджер системы частиц
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AParticleManager
    {
        ///--------------------------------------------------------------------------------------
        private AParticleFactory    m_factory = null; //фабрика частиц
        private int                 m_managerID = -1; //глобальный индификатор менеджера (присваевает фабрика частиц)
        private bool                m_blocking = false;//блокирование частиц на манипуляции, при добавлении и удалении


        /*эффекты для частиц
         */
        private AParticleEffect     m_effectTrail = null;
        private AParticleEffect     m_effectHeartHP = null;
        private AParticleEffect     m_effectHeartLife = null;
        private AParticleEffect     m_effectBoom = null;
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AParticleManager(AParticleFactory factory)
        {
            m_factory = factory;
            m_managerID = factory.registrationManager(this);

            m_effectTrail = new AEffectTrail(this);
            m_effectHeartHP = new AEffectHeartHP(this);
            m_effectHeartLife = new AEffectHeartLife(this);
            m_effectBoom = new AEffectBoom(this);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка графики частиц
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void loadContent(ContentManager content)
        {
            //удалим все частицы, которые привязанны к данному менеджеру
            m_effectTrail.onLoadContent(content);
            m_effectHeartHP.onLoadContent(content);
            m_effectHeartLife.onLoadContent(content);
            m_effectBoom.onLoadContent(content);
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Создание новой частицы  в менеджере, менеджер привязан к экрану
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AParticle newParticle()
        {
            if (m_blocking)
            {
                throw new System.InvalidOperationException("Нельзя добавить новую частицу, когда менеджер частиц заблокирован отрисовкой или обработкой логики");
            }
            AParticle obj = m_factory.newParticle();
            obj.managerID = m_managerID;
            return obj;
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Удаление частицы на уровне менеджера частиц
        /// чтобы можно было безопасно удалить частицу.
        /// нужно удалять ее отложенно
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void removeParticle(AParticle obj)
        {
            obj.managerID = -1;
            if (!m_blocking)
            {
                m_factory.removeParticle(obj); 
            }
        }
        ///--------------------------------------------------------------------------------------












         ///=====================================================================================
        ///
        /// <summary>
        /// Удаление всех частиц
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void clearParticle()
        {
            bool next = true;
            while (next)
            {
                next = false;
                foreach (AParticle obj in m_factory.particles)
                {
                    int id = obj.managerID;
                    if (id == m_managerID)
                    {
                        next = true;
                        removeParticle(obj);
                        break;
                    }
                    if (id == -1)
                    {
                        next = true;
                        m_factory.removeParticle(obj);
                        break;
                    }

                }
            }
        }
        ///--------------------------------------------------------------------------------------










         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка частиц
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void draw(TimeSpan gameTime, ASpriteBatch spriteBatch)
        {
            m_blocking = true; //заблокировали список на ввод новых данных
            int iCount = 0;
            spriteBatch.begin();
            foreach (AParticle obj in m_factory.particles)
            {
                if (obj.managerID == m_managerID)
                {
                    //сбросим буфер отрисовки на экран,
                    iCount++;
                    if (iCount > 40)
                    {
                        spriteBatch.flush();
                        iCount = 0;
                    }
                    //
                    switch (obj.typeEffect)
                    {
                        case 1://effectTrail
                            {
                                m_effectTrail.onDrawParticle(gameTime, spriteBatch, obj);
                                continue;
                            }
                        case 2://effectHeartHP
                            {
                                m_effectHeartHP.onDrawParticle(gameTime, spriteBatch, obj);
                                continue;
                            }
                        case 3://effectHeartLife
                            {
                                m_effectHeartLife.onDrawParticle(gameTime, spriteBatch, obj);
                                continue;
                            }
                        case 4://effectBoom
                            {
                                m_effectBoom.onDrawParticle(gameTime, spriteBatch, obj);
                                continue;
                            }

                    }
                }
            }
            spriteBatch.end();
            m_blocking = false;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// обновление движения частиц, только для конкретного экрана
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void update(TimeSpan gameTime)
        {
            m_blocking = true;
            bool needRemove = false; //признак того что есть объекты для удаления
            
            m_effectTrail.onUpdate(gameTime);
            m_effectHeartHP.onUpdate(gameTime);
            m_effectHeartLife.onUpdate(gameTime);
            m_effectBoom.onUpdate(gameTime);
            foreach (AParticle obj in m_factory.particles)
            {
                int id = obj.managerID;
                if (id == m_managerID)
                {
                    switch (obj.typeEffect)
                    {
                        case 1://effectTrail
                            {
                                m_effectTrail.onUpdateParticle(gameTime, obj);
                                continue;
                            }
                        case 2://effectHeartHP
                            {
                                m_effectHeartHP.onUpdateParticle(gameTime, obj);
                                continue;
                            }
                        case 3://effectHeartLife
                            {
                                m_effectHeartLife.onUpdateParticle(gameTime, obj);
                                continue;
                            }
                        case 4://effectBoom
                            {
                                m_effectBoom.onUpdateParticle(gameTime, obj);
                                continue;
                            }
                    }
                }
                else
                    if (id == -1)
                    {
                       needRemove  = true;//требуется удалить объекты
                    }

            }
            
            /*
             * удаление свободно болтающихся объектов
             */
            while (needRemove)
            {
                needRemove = false;
                foreach (AParticle obj in m_factory.particles)
                {
                    if (obj.managerID == -1)
                    {
                        needRemove = true;
                        m_factory.removeParticle(obj);
                        break;
                    }
                }
            }
            m_blocking = false;
        }












    }
}
