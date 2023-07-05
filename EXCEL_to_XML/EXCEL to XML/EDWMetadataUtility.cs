using CsvHelper.Configuration;
using CsvHelper.Excel;
using CsvHelper;
using System.Globalization;
using System.Xml.Linq;
using System.Xml.XPath;

namespace EXCEL_to_XML;

public class EDWMetadataUtility
{
    public static readonly string TemplateName = "MinimumMetadataEntryTemplate";
    public static readonly string DesktopPath = $"C:/Users/cqb13/Desktop"; //trent
    public static readonly string WorkingFilePath = $"{DesktopPath}/xmldocs";

    public void Run(string xmlPath, string dataSourcePath, string outputFolderPath)
    {
        #region Config / setup

        var templateSheetNames = new List<string> { "EDW_CoreMetadata", "EDW_AttributesMetadata" };

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = header => header.Header.ToLower().Replace(" ", string.Empty), // Ignore casing and remove spaces
            ShouldSkipRecord = row => row.Record[0].StartsWith("Attribution Information")
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
            ? $"{DateTime.Now.Date.ToString("yyyy-MM-dd")}-{title}-output.xml"
            : $"{DateTime.Now.Date.ToString("yyyy-MM-dd")}-output.xml";

        tree.Save($"{outputFolderPath}/{outputFileName}");
        #endregion

        #endregion
    }

    public void HandleCoreData(XElement dataSet, List<CoreMetadataField> coreMetadataRecords)
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

    public void HandleAttributeData(XElement tree, List<AttributesMetadata> attributesMetadataRecords)
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

                //this is a udom
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
                //this is an edom

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