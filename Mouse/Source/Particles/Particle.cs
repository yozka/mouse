using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mouse.Collection;



namespace Mouse.Particle
{
    ///--------------------------------------------------------------------------------------






     ///=====================================================================================
    ///
    /// <summary>
    /// Одна частица
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AParticle : AElementConstLength
    {
        ///--------------------------------------------------------------------------------------
        
        /// <summary>
        /// привязанная частица к экрану
        /// </summary>
        public int managerID = -1;


        /// <summary>
        /// привязанная частица к эффекту логики своего выполнения
        /// </summary>
        public int typeEffect = 0;


        /// <summary>
        /// позиция частицы
        /// </summary>
        public Vector2 position;



        /// <summary>
        /// центр частицы
        /// </summary>
        public Vector2 positionCenter;

        

        /// <summary>
        /// цвет частицы
        /// </summary>
        public Color color;


     
        /// <summary>
        /// текущее время жизни частицы
        /// </summary>
        public TimeSpan lifeTime;



        /// <summary>
        /// время жизни частицы
        /// </summary>
        public TimeSpan timeTransition;



        /// <summary>
        /// размер частицы
        /// </summary>
        public float size;


        /// <summary>
        /// максимальный размер частицы
        /// </summary>
        public float sizeMax;


        /// <summary>
        /// прозрачность частицы
        /// </summary>
        public float transparence;




        /// <summary>
        /// переход из одного состояние в другое
        /// </summary>
        public float transition;



        /// <summary>
        /// поворот частицы
        /// </summary>
        public float rotation;



        /// <summary>
        /// сторона на которую осуществляется поворот
        /// некий стиль
        /// </summary>
        public float rotationStyle;





        /// <summary>
        /// стиль вида частицы
        /// некий стиль
        /// </summary>
        public int styleView;
        ///--------------------------------------------------------------------------------------



















    }
}
