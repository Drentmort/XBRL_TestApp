using System.Collections.ObjectModel;

namespace XBRLTestApp.Domain.Entities
{

    /* XBRL definition
     * namespace: http://www.xbrl.org/2003/instance
     * file path: www.xbrl.org\2003\xbrl-instance-2003-12-31.xsd
     
    */

    public class Fact
    {
        /// <summary>Идентификатор фактов (значений отчета)</summary>
        /// <see>xbrli:unit/[@id]</see>
        public string? Id { get; set; }

        /// <summary>Ссылка на контекст</summary>
        /// <see>xbrli:unit/[@contextRef]</see>
        public string? ContextRef { get; set; }

        /// <summary>Используемые контекст</summary>
        public Context? Context { get; set; }

        /// <summary>Ссылка на единицу измерения</summary>
        /// <see>xbrli:unit/[@unitRef]</see>
        public string? UnitRef { get; set; }

        /// <summary>Используемые юнит</summary>
        public Unit? Unit { get; set; }

        /// <summary>Точность измерения</summary>
        /// <see>xbrli:unit/[@decimals]</see>
        public int? Decimals { get; set; }

        /// <summary>Точность значения</summary>
        /// <see>xbrli:unit/[@precision]</see>
        public  int? Precision { get; set; }

        /// <summary>Значение</summary>
        /// <see>xbrli:unit/*</see>
        public string? Value { get; set; }

    }

    public class Facts : Collection<Fact>
    {
		public Facts() { }
		public Facts(IList<Fact> facts) : base(facts) { }
	}
}
