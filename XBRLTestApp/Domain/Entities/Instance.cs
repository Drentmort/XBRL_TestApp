namespace XBRLTestApp.Domain.Entities
{
    /// <summary>
    /// Модель записи инстанса (файла отчета)
    /// </summary>
    /// <remarks>Таблица ReportInstances</remarks>

    public class Instance
    {

        /// <summary>Коллекция контекстов (context)</summary>
        /// <see>xbrli:context</see>
        public Contexts? Contexts { get; set; } = new Contexts();

        /// <summary>Коллекция единиц измерения (unit)</summary>
        /// <see>xbrli:unit</see>
        public Units? Units { get; set; } = new Units();

        /// <summary>Коллекция фактов (fact)</summary>
        public Facts? Facts { get; set; } = new Facts();

    }

}
