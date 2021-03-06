using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.XPath;
using System.Xml;
using DevExpress.Data.Filtering;
using Aurum.Interface.Win.Controllers;

namespace Aurum.Interface.Win.Filters
{
    /// <summary>
    /// Фильтр табличных данных
    /// </summary>
    public partial class QueryGridFilterBase : UserControl
    {
        /// <summary>
        /// Ошибка
        /// </summary>
        protected static readonly string errorMessage = "Ошибка!";

        /// <summary>
        /// Поиск значений, не равных null
        /// </summary>
        protected static readonly string notNullValue = "<непустое значение>";

        /// <summary>
        /// Поиск значений, равных null
        /// </summary>
        protected static readonly string nullValue = "<пустое значение>";

        /// <summary>
        /// Значение
        /// </summary>
        protected FilterValue value;

        /// <summary>
        /// Конкретный тип данных
        /// </summary>
        private Type type = null;

        /// <summary>
        /// Конструктор
        /// </summary>
        public QueryGridFilterBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Событие на изменение типа данных
        /// </summary>
        protected event EventHandler dataTypeChanged;

        /// <summary>
        /// Описание установленного фильтра
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string Description
        {
            get { return null; }
        }

        /// <summary>
        /// Конкретный тип данных
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Type Type
        {
            get { return type; }
            set
            {
                type = value;

                if (dataTypeChanged != null)
                {
                    dataTypeChanged(this, new System.EventArgs());
                }
            }
        }

        /// <summary>
        /// Значение фильтра
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual FilterValue Value
        {
            get { return value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Колонка
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual FilterColumn FilterColumn
        {
            get;
            set;
        }

        /// <summary>
        /// Очистка полей ввода
        /// </summary>
        public virtual void ClearExternalValues()
        {
            throw new Exception("Method is not implemented!");
        }

        /// <summary>
        /// Очистка сохраненных значений
        /// </summary>
        public virtual void ClearInternalValues()
        {
            throw new Exception("Method is not implemented!");
        }

        /// <summary>
        /// Создание глубокой копии объекта фильтра.
        /// Каждый наследник обязан перегрузить метод,
        /// в котором нужно создать экземпляр собственного класса фильтра
        /// с глубокой копией всех данных фильтра.
        /// </summary>
        /// <returns> Глубокая копия объекта </returns>
        public virtual QueryGridFilterBase Clone()
        {
            throw new Exception("Clone() is not implemented in : " + this.GetType().Name);
        }

        /// <summary>
        /// Запоминание введенных значений во внутренних переменных
        /// </summary>
        public virtual void ExternalValuesToInternalValues()
        {
            throw new Exception("ExternalValuesToInternalValues() is not implemented in : " + this.GetType().Name);
        }

        /// <summary>
        /// Получить выражение фильтра
        /// </summary>
        /// <returns>Выражение фильтра</returns>
        public virtual CriteriaOperator GetExpression()
        {
            return null;
        }

        /// <summary>
        /// Сброс полей ввода и восстановление сохраненных значений
        /// </summary>
        public virtual void InternalValuesToExternalValues()
        {
            throw new Exception("Method is not implemented!");
        }

        /// <summary>
        /// Инициализация данных в клонируемом объекте данными текущего объекта.
        /// Наследники, имеющие свои член-данные, должны перегрузить метод и инициализировать эти данные в этом методе.
        /// </summary>
        /// <param name="clone"> Объект для инициализации. </param>
        protected virtual void InitClone(QueryGridFilterBase clone)
        {
            if (clone == null)
            {
                return;
            }

            clone.Value = (FilterValue)Value.Clone();
            clone.Type = type;
        }

        /// <summary>
        /// Логические операторы
        /// </summary>
        protected class Operators
        {
            /// <summary>
            /// Равно
            /// </summary>
            public static Operators Equal = new Operators("=", false, false, BinaryOperatorType.Equal);

            /// <summary>
            /// Больше
            /// </summary>
            public static Operators Greater = new Operators(">", false, false, BinaryOperatorType.Greater);

            /// <summary>
            /// Больше-равно
            /// </summary>
            public static Operators GreaterEqual = new Operators("≥", false, false, BinaryOperatorType.GreaterOrEqual);

            /// <summary>
            /// Меньше
            /// </summary>
            public static Operators Less = new Operators("<", false, false, BinaryOperatorType.Less);

            /// <summary>
            /// Меньше-равно
            /// </summary>
            public static Operators LessEqual = new Operators("≤", false, false, BinaryOperatorType.LessOrEqual);

            /// <summary>
            /// Не равно
            /// </summary>
            public static Operators NotEqual = new Operators("≠", false, false, BinaryOperatorType.NotEqual);

            /// <summary>
            /// Принадлежит
            /// </summary>
            public static Operators In = new Operators("∈", false, false, null);

            private bool binary;
            private BinaryOperatorType[] op;
            private bool or;
            private string text;

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="s">Текстовое представление оператора</param>
            /// <param name="binary">Бинарный или унарный оператор</param>
            /// <param name="useOR">OR или AND для бинарного оператора</param>
            /// <param name="o">Операторы</param>
            private Operators(string s, bool binary, bool useOR, params BinaryOperatorType[] o)
            {
                text = s;
                op = o;
                this.binary = binary;
                or = useOR;
            }

            /// <summary>
            /// Бинарный или унарный
            /// </summary>
            public bool Binary
            {
                get
                {
                    return binary;
                }
            }

            /// <summary>
            /// OR или AND для бинарного оператора
            /// </summary>
            public bool Or
            {
                get
                {
                    return or;
                }
            }

            /// <summary>
            /// Текстовое представление
            /// </summary>
            public string Text
            {
                get
                {
                    return text;
                }
            }

            /// <summary>
            /// "Перевернуть" оператор, как если бы он читался справа налево
            /// </summary>
            /// <param name="o"></param>
            /// <returns></returns>
            public static Operators Invert(Operators o)
            {
                if (o == Less) return Greater;
                if (o == Greater) return Less;
                if (o == LessEqual) return GreaterEqual;
                if (o == GreaterEqual) return GreaterEqual;
                return o;
            }

            /// <summary>
            /// Операторы
            /// </summary>
            /// <returns></returns>
            public BinaryOperatorType[] GetOperators()
            {
                return op;
            }

            /// <summary>
            /// Преобразование в строку
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return text;
            }
        }
    }

    /// <summary>
    /// Значение фильтра
    /// </summary>
    public abstract class FilterValue : ICloneable
    {
        #region ICloneable Members

        /// <summary>
        /// Создание копии
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        #endregion ICloneable Members
    }
}