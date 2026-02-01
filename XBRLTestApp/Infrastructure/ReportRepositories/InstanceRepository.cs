using System.Text;
using System.Xml;
using System.Xml.XPath;
using XBRLTestApp.Domain.Entities;
using XBRLTestApp.Domain.Interfaces;

namespace XBRLTestApp.Infrastructure.Repositories
{
	internal class InstanceRepository : IInstanceRepository
	{
		private readonly Dictionary<string, Context> _contexts = new();
		private readonly Dictionary<string, Unit> _units = new();
		private readonly Dictionary<string, Fact> _facts = new();
		private readonly Instance _instance;
		private readonly XPathDocument _document;
		private readonly XPathNavigator _documentNavigator;
		private readonly XmlNamespaceManager _xmlNamespaceManager;

		public InstanceRepository(string filePath)
		{
			_document = new XPathDocument(filePath);
			_documentNavigator = _document.CreateNavigator();
			_xmlNamespaceManager= GetXmlNamespaceManager(_documentNavigator.NameTable);
			ReadUnits(_documentNavigator, _xmlNamespaceManager);
			ReadContexts(_documentNavigator, _xmlNamespaceManager);
			ReadFacts(_documentNavigator, _xmlNamespaceManager);

			_instance = new Instance
			{
				Contexts = new(_contexts.Values.ToList()),
				Units = new(_units.Values.ToList()),
				Facts = new(_facts.Values.ToList())
			};
		}

		public Instance GetInstance()
		{
			return _instance;
		}

		public IReadOnlyDictionary<string, Context> GetIndexedContexts()
		{
			return _contexts;
		}
		public IReadOnlyDictionary<string, Unit> GetIndexedUnits()
		{
			return _units;
		}

		public IReadOnlyDictionary<string, Fact> GetIndexedFacts()
		{
			return _facts;
		}

		public string ExecuteXPathQuery(string request)
		{
			_documentNavigator.MoveToRoot();
			var iterator = _documentNavigator.Select(request, _xmlNamespaceManager);
			var sb = new StringBuilder();
			while (iterator.MoveNext())
			{
				sb.AppendLine(iterator.Current?.InnerXml!);
			}
			return sb.ToString();
		}

		private void ReadUnits(XPathNavigator navigator, XmlNamespaceManager namespaceManager)
		{
			navigator.MoveToRoot();

			if (navigator.MoveToFirstChild())
			{
				var iterator = navigator.Select($"//{MakePrefixFreeName("unit")}", namespaceManager);

				while (iterator.MoveNext())
				{
					var unit = new Unit();
					var current = iterator.Current!;

					unit.Id = current.GetAttribute("id", "");
					var measureIterator = current.Select(MakePrefixFreeName("measure"), namespaceManager);
					if (measureIterator.MoveNext())
					{
						unit.Measure = measureIterator.Current?.Value;
					}

					var unitDenominatorIterator = current.Select(MakePrefixFreeName("unitDenominator"), namespaceManager);
					if (unitDenominatorIterator.MoveNext())
					{
						unit.Denominator = unitDenominatorIterator.Current?.Value;
					}

					var unitNumeratorIterator = current.Select(MakePrefixFreeName("unitNumerator"), namespaceManager);
					if (unitNumeratorIterator.MoveNext())
					{
						unit.Numerator = unitNumeratorIterator.Current?.Value;
					}

					if (!string.IsNullOrEmpty(unit.Id) && !_units.ContainsKey(unit.Id))
					{
						_units.Add(unit.Id, unit);
					}
				}
			}
		}

		private void ReadContexts(XPathNavigator navigator, XmlNamespaceManager namespaceManager)
		{
			navigator.MoveToRoot();
			if (navigator.MoveToFirstChild())
			{
				var iterator = navigator.Select($"//{MakePrefixFreeName("context")}", namespaceManager);

				while (iterator.MoveNext())
				{
					var context = new Context();
					var current = iterator.Current!;

					context.Id = current.GetAttribute("id", "");

					var entityIterator = current.Select($"{MakePrefixFreeName("entity")}/{MakePrefixFreeName("identifier")}", namespaceManager);
					if (entityIterator.MoveNext())
					{
						context.EntityValue = entityIterator.Current?.Value;
						context.EntityScheme = entityIterator.Current?.GetAttribute("scheme", "");
					}

					var periodIterator = current.Select(MakePrefixFreeName("period"), namespaceManager);
					if (periodIterator.MoveNext())
					{
						var periodNavigator = periodIterator.Current!;
						context.PeriodStartDate = DateTime.TryParse(periodNavigator.SelectSingleNode(MakePrefixFreeName("startDate"), namespaceManager)?.Value, out var startDate) ? startDate : null!;
						context.PeriodEndDate = DateTime.TryParse(periodNavigator.SelectSingleNode(MakePrefixFreeName("endDate"), namespaceManager)?.Value, out var endDate) ? endDate : null!;
						context.PeriodInstant = DateTime.TryParse(periodNavigator.SelectSingleNode(MakePrefixFreeName("instant"), namespaceManager)?.Value, out var instantDate) ? instantDate : null!;
						context.PeriodForever = periodNavigator.SelectSingleNode(MakePrefixFreeName("forever"), namespaceManager)?.ValueAsBoolean ?? false;
					}

					var scenarios = current.Select(MakePrefixFreeName("scenario"), namespaceManager);
					if (scenarios.MoveNext())
					{
						context.Scenarios = ExtractScenarios(scenarios.Current!, namespaceManager);
					}

					if (!string.IsNullOrEmpty(context.Id) && !_units.ContainsKey(context.Id))
					{
						_contexts.Add(context.Id, context);
					}
				}
			}
		}

		private Scenarios ExtractScenarios(XPathNavigator navigator, XmlNamespaceManager namespaceManager)
		{
			var scenarios = new Scenarios();

			var childNodes = navigator.Select("*");
			while (childNodes.MoveNext())
			{
				var childNavigator = childNodes.Current;

				var scenario = new Scenario();
				var localName = childNavigator!.LocalName;

				scenario.DimensionType = localName;

				scenario.DimensionName = childNavigator.GetAttribute("dimension", "");

				if (localName == "explicitMember")
				{
					scenario.DimensionValue = childNavigator.Value?.Trim();

				}
				if (localName == "typedMember")
				{
					if (childNavigator.MoveToFirstChild())
					{
						scenario.DimensionCode = childNavigator.LocalName;
						scenario.DimensionValue = childNavigator.Value?.Trim();
					}
				}
				scenarios.Add(scenario);
			}

			return scenarios;
		}

		private void ReadFacts(XPathNavigator navigator, XmlNamespaceManager namespaceManager)
		{
			navigator.MoveToRoot();
			if (navigator.MoveToFirstChild())
			{
				var iterator = navigator.Select("//purcb-dic:*", namespaceManager);

				while (iterator.MoveNext())
				{
					var current = iterator.Current!;
					var fact = new Fact
					{
						Id = current.GetAttribute("id", ""),
						ContextRef = current.GetAttribute("contextRef", ""),
						UnitRef = current.GetAttribute("unitRef", ""),
						Decimals = int.TryParse(current.GetAttribute("decimals", ""), out var dec) ? dec : null!,
						Precision = int.TryParse(current.GetAttribute("precision", ""), out var pres) ? pres : null!,
						Value = current.Value
					};

					if(_contexts.TryGetValue(fact.ContextRef, out var context))
					{
						fact.Context = context;
					}

					if (_units.TryGetValue(fact.UnitRef, out var unit))
					{
						fact.Unit = unit;
					}

					if (!_facts.ContainsKey(fact.Id))
					{
						_facts.Add(fact.Id, fact);
					}
				}
			}
		}

		private XmlNamespaceManager GetXmlNamespaceManager(XmlNameTable nameTable)
		{
			var manager = new XmlNamespaceManager(nameTable);
			manager.AddNamespace("xbrli", "http://www.xbrl.org/2003/instance");
			manager.AddNamespace("purcb-dic", "http://www.cbr.ru/xbrl/nso/purcb/dic/purcb-dic");

			return manager;	
		}

		private string MakePrefixFreeName(string name) => $"*[local-name()='{name}']";


	}
}
