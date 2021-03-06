﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Mouse.Collection;



namespace Mouse.Particle
{
    ///--------------------------------------------------------------------------------------






     ///=====================================================================================
    ///
    /// <summary>
    /// Фабрика всех частиц
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AParticleFactory
    {
        ///--------------------------------------------------------------------------------------
        private ACollectionConstLength<AParticle> m_particles = new ACollectionConstLength<AParticle>(2000);
        private List<string> m_handleManager = new List<string>();//зарегестрированные менеджеры частиц
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// возвратим глобальный массив частицц
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public ACollectionConstLength<AParticle> particles
        {
            get
            {
                return m_particles;
            }
        }
        ///--------------------------------------------------------------------------------------





        private int maxs = 0;

         ///=====================================================================================
        ///
        /// <summary>
        /// Создание новой частицы
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AParticle newParticle()
        {
            AParticle obj = m_particles.getNew();
            if (m_particles.count > maxs)
            {
                maxs = m_particles.count;
            }
            return obj;
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Удаление частицы на уровне фабрики
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void removeParticle(AParticle obj)
        {
            m_particles.remove(obj);
        }
        ///--------------------------------------------------------------------------------------



        





         ///=====================================================================================
        ///
        /// <summary>
        /// Регистрация менеджера в системе частиц
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int registrationManager(Object manager)
        {
            string sName = manager.ToString();
            int id = m_handleManager.BinarySearch(sName);
            if (id < 0)
            {
                id = m_handleManager.Count;
                m_handleManager.Add(sName);
            }
            return id;
        }


    }
}
