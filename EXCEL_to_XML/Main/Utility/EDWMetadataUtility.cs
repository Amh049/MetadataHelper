using CsvHelper.Configuration;
using CsvHelper.Excel;
using CsvHelper;
using System.Globalization;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Form.Utility;

public class EDWMetadataUtility
{
    public string Run(string xmlPath, string dataSourcePath, string outputFolderPath)
    {
        #region Config / setup

        var templateSheetNames = new List<string> { "EDW_CoreMetadata", "EDW_AttributesMetadata" }; //This is reading in our Core Metadat Sheet and The Attribute Sheet

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = header => header.Header.ToLower().Replace(" ", string.Empty), // Ignore casing and remove spaces
            ShouldSkipRecord = row => row.Record[0].StartsWith("Attribution Information") //this is skipping the first row in our the attribute information sheet
        };

        #endregion

        #region Reading in template sheets 
        var coreMetadataRecords = new List<CoreMetadataField>();
        var attributesMetadataRecords = new List<AttributesMetadata>();

        foreach (var sheetName in templateSheetNames)
        {
            using var reader = new CsvReader(new ExcelParser(dataSourcePath, sheetName, config));

            if (sheetName == "EDW_CoreMetadata")
            {
                //read the core data from the template
                coreMetadataRecords = reader.GetRecords<CoreMetadataField>().ToList();
            }
            else if (sheetName == "EDW_AttributesMetadata")
            {
                //read the attributes data from the template
                attributesMetadataRecords = reader.GetRecords<AttributesMetadata>().ToList();
            }
        }
        #endregion

        #region Main Processing

        XElement tree = XElement.Load(xmlPath);

        HandleCoreData(tree, coreMetadataRecords);
        HandleAttributeData(tree, attributesMetadataRecords);

        #region Saving output

        CoreMetadataField title = coreMetadataRecords.FirstOrDefault(x => x.Field == "title");

        string outputFileName = title != null
            ? $"{DateTime.Now.Date.ToString("yyyy-MM-dd-hh")}-{title.Value}-output.xml"
            : $"{DateTime.Now.Date.ToString("yyyy-MM-dd-hh")}-output.xml";

        var outputFilePath = $"{outputFolderPath}/{outputFileName}";

        tree.Save(outputFilePath);

        #endregion

        #endregion

        return outputFilePath;
    }

    private void HandleCoreData(XElement dataSet, List<CoreMetadataField> coreMetadataRecords)
    {
        var xmlTemplateElements = dataSet.DescendantsAndSelf();

        foreach (var element in xmlTemplateElements)
        {
            string elementNameTrimmed = element.Name.ToString()
                .Replace("{", "")
                .Replace("}", "")
                .ToLower();

            var templateValue = coreMetadataRecords.FirstOrDefault(x => x.Field.ToLower() == elementNameTrimmed);

            if (templateValue != null && !string.IsNullOrEmpty(templateValue.Value))
            {
                element.Value = templateValue.Value;
            }
        }
    }

    private void HandleAttributeData(XElement tree, List<AttributesMetadata> attributesMetadataRecords)
    {
        var attributesDataGrouped = attributesMetadataRecords
            .GroupBy(x => x.AttributeLabel)
            .ToList();

        //loop over the attributes list that we grouped by label
        foreach (var attrDataGroup in attributesDataGrouped)
        {
            var attributeTemplate = tree.XPathSelectElements("//eainfo//detailed//attr").Last(); //find the <attr> in the template so we can add more below it

            if (attrDataGroup.Count() == 1)
            {
                var attrItem = attrDataGroup.First();

                //this is a udom (unrepresentable domain)
                var newAttributeTree = new XElement("attr",
                    new XElement("attrlabl", attrItem.AttributeLabel),
                    new XElement("attrdef", attrItem.AttributeDefinition),
                    new XElement("attrdefs", attrItem.AttributeDefinitionSource),
                    new XElement("attrdomv",
                        new XElement("udom", attrItem.ValueDefinition)));

                attributeTemplate!.AddAfterSelf(newAttributeTree);
            }
            else if (attrDataGroup.Count() > 1)
            {
                //this is an edom (Enumerated domain)

                var newAttributeTree = new XElement("attr",
                    new XElement("attrlabl", attrDataGroup.First().AttributeLabel),
                    new XElement("attrdef", attrDataGroup.First().AttributeDefinition),
                    new XElement("attrdefs", attrDataGroup.First().AttributeDefinitionSource),
                    new XElement("attrdomv"));

                //build a list of the AttributeValue
                foreach (var attrItem in attrDataGroup)
                {
                    var edom = new XElement("edom",
                        new XElement("edomv", attrItem.AttributeValue),
                        new XElement("edomvd", attrItem.ValueDefinition),
                        new XElement("edomvds", attrItem.ValueDefinitionSource));

                    newAttributeTree.Element("attrdomv")!.AddAfterSelf(edom);
                }

                attributeTemplate!.AddAfterSelf(newAttributeTree);
            }
        }

        //finally, remove the template <attr> because it will still be empty
        var attributeElement = tree.XPathSelectElements("//eainfo//detailed//attr").First();

        attributeElement.Remove();
    }
}