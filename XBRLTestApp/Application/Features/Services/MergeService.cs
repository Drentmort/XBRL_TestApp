using System.Xml;
using XBRLTestApp.Application.Interfaces;
using XBRLTestApp.Domain.Interfaces;

namespace XBRLTestApp.Application.Features.Services
{
	internal class MergeService : IMergeService
	{
		private readonly IInstanceRepositoryPool _instanceRepositoryPool;

		public MergeService(IInstanceRepositoryPool instanceRepositoryPool)
		{
			_instanceRepositoryPool = instanceRepositoryPool;
		}

		public void Merge(string masterFile, string slaveFile, string outputFile)
		{
			var masterRepository = _instanceRepositoryPool.GetRepository(masterFile);
			var slaveRepository = _instanceRepositoryPool.GetRepository(slaveFile);

			var addedContextIds = slaveRepository.GetIndexedContexts().Keys.Where(x => !masterRepository.GetIndexedContexts().ContainsKey(x)).Select(x => x).ToList();
			var removedContextIds = masterRepository.GetIndexedContexts().Keys.Where(x => !slaveRepository.GetIndexedContexts().ContainsKey(x)).Select(x => x).ToList();
			var updatedContextIds = masterRepository.GetIndexedContexts().Keys.IntersectBy(slaveRepository.GetIndexedContexts().Keys, x => x).ToList();

			var addedUnitIds = slaveRepository.GetIndexedUnits().Keys.Where(x => !masterRepository.GetIndexedUnits().ContainsKey(x)).Select(x => x).ToList();
			var removedUnitIds = masterRepository.GetIndexedUnits().Keys.Where(x => !slaveRepository.GetIndexedUnits().ContainsKey(x)).Select(x => x).ToList();
			var updatedUnitIds = masterRepository.GetIndexedUnits().Keys.IntersectBy(slaveRepository.GetIndexedUnits().Keys, x => x).ToList();

			var addedFactIds = slaveRepository.GetIndexedFacts().Keys.Where(x => !masterRepository.GetIndexedFacts().ContainsKey(x)).Select(x => x).ToList();
			var removedFactIds = masterRepository.GetIndexedFacts().Keys.Where(x => !slaveRepository.GetIndexedFacts().ContainsKey(x)).Select(x => x).ToList();
			var updatedFactIds = masterRepository.GetIndexedFacts().Keys.IntersectBy(slaveRepository.GetIndexedFacts().Keys, x => x).ToList();

			var masterDoc = new XmlDocument();
			var slaveDoc = new XmlDocument();

			masterDoc.Load(masterFile);
			slaveDoc.Load(slaveFile);

			var nsManager = new XmlNamespaceManager(masterDoc.NameTable);
			nsManager.AddNamespace("xbrli", "http://www.xbrl.org/2003/instance");
			nsManager.AddNamespace("purcb-dic", "http://www.cbr.ru/xbrl/nso/purcb/dic/purcb-dic");

			// 1. Удаление
			RemoveElementsById(masterDoc, removedContextIds, "xbrli:context", nsManager);
			RemoveElementsById(masterDoc, removedUnitIds, "xbrli:unit", nsManager);
			RemoveElementsById(masterDoc, removedFactIds, "*", nsManager); // для фактов ищем любой элемент с таким id

			// 2. Добавление
			CopyElementsById(masterDoc, slaveDoc, addedContextIds, "xbrli:context",nsManager);
			CopyElementsById(masterDoc, slaveDoc, addedUnitIds, "xbrli:unit", nsManager);
			CopyElementsById(masterDoc, slaveDoc, addedFactIds, "*", nsManager);

			// 3. Обновление
			UpdateElementsById(masterDoc, slaveDoc, updatedContextIds, "xbrli:context", nsManager);
			UpdateElementsById(masterDoc, slaveDoc, updatedUnitIds, "xbrli:unit", nsManager);
			UpdateElementsById(masterDoc, slaveDoc, updatedFactIds, "*", nsManager);

			masterDoc.Save(outputFile);
		}

		private void RemoveElementsById(XmlDocument doc, List<string> ids, string elementName, XmlNamespaceManager nsManager)
		{
			foreach (var id in ids)
			{
				XmlNode node;
				if (elementName == "*")
				{
					node = doc.SelectSingleNode($"//*[@id='{id}']", nsManager);
				}
				else
				{
					node = doc.SelectSingleNode($"//{elementName}[@id='{id}']", nsManager);
				}

				node?.ParentNode?.RemoveChild(node);
			}
		}

		//private void CopyElementsById(XmlDocument masterDoc, XmlDocument slaveDoc, List<string> ids, string elementName, XmlNamespaceManager nsManager)
		//{
		//	foreach (var id in ids)
		//	{
		//		XmlNode slaveNode;
		//		if (elementName == "*")
		//		{
		//			slaveNode = slaveDoc.SelectSingleNode($"//*[@id='{id}']", nsManager);
		//		}
		//		else
		//		{
		//			slaveNode = slaveDoc.SelectSingleNode($"//{elementName}[@id='{id}']", nsManager);
		//		}

		//		if (slaveNode != null)
		//		{
		//			var importedNode = masterDoc.ImportNode(slaveNode, true);
		//			masterDoc.DocumentElement?.AppendChild(importedNode);
		//		}
		//	}
		//}

		private void CopyElementsById(XmlDocument masterDoc, XmlDocument slaveDoc, List<string> ids, string elementName, XmlNamespaceManager nsManager)
		{
			foreach (var id in ids)
			{
				XmlNode slaveNode;
				if (elementName == "*")
				{
					slaveNode = slaveDoc.SelectSingleNode($"//*[@id='{id}']", nsManager);
				}
				else
				{
					slaveNode = slaveDoc.SelectSingleNode($"//{elementName}[@id='{id}']", nsManager);
				}

				if (slaveNode != null)
				{
					var importedNode = masterDoc.ImportNode(slaveNode, true);

					XmlNodeList sameTypeNodes;
					if (elementName == "*")
					{
						var localName = slaveNode.LocalName;
						var namespaceUri = slaveNode.NamespaceURI;
						sameTypeNodes = masterDoc.SelectNodes($"//*[local-name()='{localName}' and namespace-uri()='{namespaceUri}']", nsManager);
					}
					else
					{
						sameTypeNodes = masterDoc.SelectNodes($"//{elementName}", nsManager);
					}

					if (sameTypeNodes != null && sameTypeNodes.Count > 0)
					{
						var lastNode = sameTypeNodes[sameTypeNodes.Count - 1];
						lastNode.ParentNode?.InsertAfter(importedNode, lastNode);
					}
					else
					{
						masterDoc.DocumentElement?.AppendChild(importedNode);
					}
				}
			}
		}

		private void UpdateElementsById(XmlDocument masterDoc, XmlDocument slaveDoc, List<string> ids, string elementName, XmlNamespaceManager nsManager)
		{
			foreach (var id in ids)
			{
				XmlNode masterNode, slaveNode;

				if (elementName == "*")
				{
					masterNode = masterDoc!.SelectSingleNode($"//*[@id='{id}']", nsManager)!;
					slaveNode = slaveDoc!.SelectSingleNode($"//*[@id='{id}']", nsManager)!;
				}
				else
				{
					masterNode = masterDoc.SelectSingleNode($"//{elementName}[@id='{id}']", nsManager)!;
					slaveNode = slaveDoc.SelectSingleNode($"//{elementName}[@id='{id}']", nsManager)!;
				}

				if (masterNode != null && slaveNode != null)
				{
					UpdateElement(masterNode, slaveNode, masterDoc);
				}
			}
		}

		private void UpdateElement(XmlNode masterNode, XmlNode slaveNode, XmlDocument masterDoc)
		{
			var masterAttrs = masterNode.Attributes;
			var slaveAttrs = slaveNode.Attributes;

			foreach (XmlAttribute slaveAttr in slaveAttrs)
			{
				var masterAttr = masterAttrs?[slaveAttr.Name];
				if (masterAttr != null)
				{
					masterAttr.Value = slaveAttr.Value;
				}
				else
				{
					var newAttr = masterDoc.CreateAttribute(slaveAttr.Name);
					newAttr.Value = slaveAttr.Value;
					masterAttrs?.Append(newAttr);
				}
			}

			masterNode.InnerXml = slaveNode.InnerXml;
		}

	}
}
