using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Mouse.App;
using Mouse.ScreenSystem;


namespace Mouse.Particle
{
    ///--------------------------------------------------------------------------------------






     ///=====================================================================================
    ///
    /// <summary>
    /// Эффект для анимации исчезновение одной жизни у объекта
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AEffectBoom : AParticleEffect
    {




        ///=====================================================================================
        ///
        /// <summary>
        /// тип используемого эффекта
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override int type()
        {
            return 4;//AEffectBoom
        }
        ///--------------------------------------------------------------------------------------





        #region Блок 1, создание частиц
        ///--------------------------------------------------------------------------------------
        private TimeSpan m_lifeEffect = TimeSpan.Zero;//время жизни эффекта
        private Color m_baseColor1 = Color.Black;//Цвет частиц
        private Color m_baseColor2 = Color.Black;//Цвет частиц        
        private TimeSpan m_sleep = TimeSpan.Zero; //время между частицами
        ///--------------------------------------------------------------------------------------






    

        
         ///=====================================================================================
        ///
        /// <summary>
        /// constructor 2 этот эффект является отдельной штуковиной для эффекта
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AEffectBoom(AParticleManager manager, TimeSpan time, Color baseColor1, Color baseColor2)
            :
            base(manager)
        {
            m_lifeEffect = time;
            m_baseColor1 = baseColor1;
            m_baseColor2 = baseColor2;
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Обновление внутренней логики эффекта
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override bool onCreation(TimeSpan time, Vector2 position, Vector2 size)
        {
            m_lifeEffect -= time;
            if (m_lifeEffect.TotalMilliseconds < 0)
            {
                return false;
            }
            m_sleep += time;
            if (m_sleep.TotalMilliseconds < 300)
            {
                return true;
            }
            m_sleep = TimeSpan.Zero;


            /*
             * добовляем новые частицы
             */
            AGameHelper helper = AGameHelper.instance;

            int iCountPoint = helper.random.Next(30, 50); //Количество элементов, которые образуют круг
            for (int i = 0; i < iCountPoint; i++)
            {
                AParticle particle = newParticle();

                //положение центра частицы
                particle.position = position;
                particle.positionCenter = position;
                //время жизни прозрачность
                particle.lifeTime = TimeSpan.FromMilliseconds(helper.random.Next(500, 1000));
                particle.timeTransition = particle.lifeTime;
                particle.transition = 0; //значение смещения от 0..до..1
                particle.transparence = 1;//прозрачность уменьшается до 0

                //цвет
                particle.color = Color.Lerp(m_baseColor1, m_baseColor2, (float)helper.random.Next(1, 100) / 100.0f);

                //размеры
                particle.size = 0.1f;
                particle.sizeMax = (float)helper.random.Next(60, 140) / 100.0f; //размер ччастицы максимальный


                //угол поворота
                float angle = (360.0f / (float)iCountPoint) * (float)i;
                particle.rotationStyle = (float)(angle * Math.PI / 180.0f); 


                //размер луча (радиус)
                particle.rotation = (float)helper.random.Next(80, 100);

                //начало радиуса
                particle.styleView = (int)Math.Max(size.X, size.Y) / 4 + helper.random.Next(0, 20);

            }
            return true;
        }
        ///--------------------------------------------------------------------------------------
        #endregion






        #region Блок 2, управление частицами
        /*
         * **************************************************************************************
         *
         *  ниже идет реализация для присособленца
         *  запускается на уровне менеджера частиц
         */



        ///--------------------------------------------------------------------------------------
        private Texture2D m_sprite; //спрайт для отрисовки частицы
        private float m_transitionShow = 0.0f;//дельта увелечение размеров частицы при появлении
        private Vector2 m_sizeDiv2;
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// constructor 1 этот эффект является менеджером, присособленцем
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AEffectBoom(AParticleManager manager)
            :
            base(manager)
        {
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Виртуальный метод загрузки графического контента эффекта
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onLoadContent(ContentManager content)
        {
            m_sprite = content.Load<Texture2D>("Common/Particles/sun");
            m_sizeDiv2 = new Vector2(m_sprite.Width, m_sprite.Height) / 2;
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// выполнение эффекта для общей группы
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onUpdate(TimeSpan gameTime)
        {
            m_transitionShow = (float)(gameTime.TotalMilliseconds / 200.0f);


        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// выполнение эффекта для конкретного объекта
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onUpdateParticle(TimeSpan gameTime, AParticle particle)
        {
            particle.lifeTime -= gameTime;
            if (particle.lifeTime.TotalMilliseconds < 0)
            {
                //удалим частицу
                particle.transparence = 0;
                removeParticle(particle);
            }
            else
            {
                float transitionDelta = (float)(gameTime.TotalMilliseconds /
                                                    particle.timeTransition.TotalMilliseconds);
                particle.transition += transitionDelta;
                particle.transparence = 1 - MathHelper.Clamp(particle.transition, 0.0f, 1.0f);

                /* установим размер частицы*/
                if (particle.size < particle.sizeMax)
                {
                    particle.size += m_transitionShow;
                    particle.size = MathHelper.Clamp(particle.size, 0, particle.sizeMax);
                }

                float l = particle.styleView + (particle.transition * particle.rotation);//длина в кпикселях радиус
                float a = particle.rotationStyle;//угола радиуса
                float x = (float)Math.Sin(a) * l;
                float y = (float)Math.Cos(a) * l;
                particle.position = particle.positionCenter + new Vector2(x, y);
            }
        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка эффекта
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onDrawParticle(TimeSpan gameTime, ASpriteBatch spriteBatch, AParticle particle)
        {
            if (particle.transparence == 0.0f)
            {
                return;
            }
     
            
            // отрисовка частицы
            spriteBatch.Draw(m_sprite,
                                         particle.position, null,
                                         particle.color * particle.transparence,
                                         0, m_sizeDiv2, particle.size, 0, 0);
        }

        #endregion


    }
}
