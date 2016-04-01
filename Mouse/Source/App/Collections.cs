﻿using System;
using System.Collections;

namespace Mouse.Collection
{
    ///--------------------------------------------------------------------------------------






     ///=====================================================================================
    ///
    /// <summary>
    /// базовый класс элемента фиксированной коллекции
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AElementConstLength
    {
        ///----------------------------------------------------------------------------------
        private int m_index = -1;
        ///----------------------------------------------------------------------------------






         ///=================================================================================
        ///
        /// <summary>
        /// Используемый индекс в коллекции
        /// </summary>
        /// 
        ///----------------------------------------------------------------------------------
        internal int _index
        { 
            get 
            { 
                return m_index; 
            }
            set
            {
                m_index = value;
            }
        }
    }
    /// AElementConstLength
    ///--------------------------------------------------------------------------------------










     ///=====================================================================================
    ///
    /// <summary>
    /// базовый класс управления самой коллекцией
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class ACollectionConstLength<T> 
                            where T : AElementConstLength, new()
    {
        ///----------------------------------------------------------------------------------
        private T[] m_elements; //массив элементов
        private int m_count = 0;
        private int m_end = -1;
        ///----------------------------------------------------------------------------------







         ///=================================================================================
        ///
        /// <summary>
        /// Инициализация колекции
        /// </summary>
        /// <param name="length">Длинна коллекции</param>
        ///----------------------------------------------------------------------------------
        public ACollectionConstLength(int length)
        {
            m_elements = new T[length];
        }
        ///----------------------------------------------------------------------------------







         ///=================================================================================
        ///
        /// <summary>
        /// Количество задействованых элементов в коллекции
        /// </summary>
        ///----------------------------------------------------------------------------------
        public int count 
        { 
            get 
            { 
                return m_count; 
            } 
        }
        ///----------------------------------------------------------------------------------







         ///=================================================================================
        ///
        /// <summary>
        /// Доступ к элементу по индексу
        /// </summary>
        /// <param name="index">Индекс</param>
        /// <returns>Элемент</returns>
        ///----------------------------------------------------------------------------------
        public T this[int index]
        {
            get 
            { 
                return m_elements[index]; 
            }
        }
        ///----------------------------------------------------------------------------------







         ///=================================================================================
        ///
        /// <summary>
        /// Выделение места под новый элемент
        /// </summary>
        /// <returns>Новый элемент</returns>
        ///----------------------------------------------------------------------------------
        public virtual T getNew()
        {
            // если список полон, то увеличим базовый размер
            if (m_end == m_elements.Length - 1)
            {
                Array.Resize(ref m_elements, m_elements.Length + 1);
            }
            //
            m_end++;
            m_count++;
            // если не инициализирован элемент
            if (m_elements[m_end] == null)
            {
                m_elements[m_end] = new T();
                m_elements[m_end]._index = m_end;
            }
            //
            return m_elements[m_end];
        }
        ///----------------------------------------------------------------------------------






         ///=================================================================================
        ///
        /// <summary>
        /// Очистка списка
        /// </summary>
        ///----------------------------------------------------------------------------------
        public void clear()
        {
            m_end = -1;
            m_count = 0;
        }
        ///----------------------------------------------------------------------------------






         ///=================================================================================
        ///
        /// <summary>
        /// Удаление элемента по индексу
        /// </summary>
        /// <param name="index">Индекс</param>
        ///----------------------------------------------------------------------------------
        public void remove(int index)
        {
            // выход за пределы
            if (index < 0 || index > m_end)
                return;
            if (index < m_end)
            {
                // рокировка элементов
                T temp = m_elements[m_end];
                m_elements[m_end] = m_elements[index];
                m_elements[index] = temp;
                
                // рокировка индексов
                int i = m_elements[m_end]._index;
                m_elements[m_end]._index = m_elements[index]._index;
                m_elements[index]._index = i;
            }
            // уменьшаем список
            m_end--;
            m_count--;
        }
        ///----------------------------------------------------------------------------------






         ///=================================================================================
        ///
        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="e">Элемент</param>
        ///----------------------------------------------------------------------------------
        public void remove(T e)
        {
            // проверка на соответствие ссылок (защита от тупости)
            if (e != m_elements[e._index])
                return;
            remove(e._index);
        }
        ///----------------------------------------------------------------------------------





         ///=================================================================================
        ///
        /// <summary>
        /// Возвращаем итератор для forech
        /// </summary>
        ///----------------------------------------------------------------------------------
        public IEnumerator GetEnumerator()
        {
            return new AIterator(this);
        }
        ///----------------------------------------------------------------------------------






         ///=================================================================================
        ///
        /// <summary>
        /// Реализация итератора для обхода колеекции
        /// </summary>
        /// 
        ///----------------------------------------------------------------------------------
        public class AIterator : IEnumerator
        {
            ///------------------------------------------------------------------------------
            private ACollectionConstLength<T> m_parent = null;
            private int m_currentIndex = -1;
            ///------------------------------------------------------------------------------
            //
            public AIterator(ACollectionConstLength<T> parent)
            {
                m_parent = parent;
            }
            //
            public Object Current
            {
                get
                {
                    return m_parent[m_currentIndex];
                }
            }
            //
            public bool MoveNext()
            {
                m_currentIndex++;
                return m_currentIndex < m_parent.m_count ? true : false;
            }
            //
            public void Reset()
            {
                m_currentIndex = 0;
            }
        }
        /// AElementConstLength
        ///----------------------------------------------------------------------------------





    }
    //ACollectionConstLength
    ///--------------------------------------------------------------------------------------


}
