using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Paragon.Sitecore.Extensions.FieldExtensions
{
    public static class FieldExtensions
    {

        /// <summary>
        /// Get the field for the item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string GetField(this Item item, string fieldName)
        {
            //Handle the possible null reference here.
            if (item.Fields[fieldName] == null)
            {
                return string.Empty;
            }
            return (string.IsNullOrWhiteSpace(item[fieldName])) ? string.Empty : item[fieldName];
        }
        
        // Methods
        public static bool GetBooleanField(this Item item, string fieldName)
        {
            Field field = item.Fields[fieldName];
            return ((field != null) && (field.Value == "1"));
        }

        public static DateTime GetDateField(this Item item, string fieldName)
        {
            DateField field = item.Fields[fieldName];
            DateTime now = DateTime.Now;
            if ((field != null) && (field.InnerField.Value != null))
            {
                now = field.DateTime;
            }
            return now;
        }

        public static HtmlField GetHtmlField(this Item item, string fieldName)
        {
            return item.Fields[fieldName];
        }

        public static LinkField GetLinkField(this Item item, string fieldName)
        {
            return item.Fields[fieldName];
        }

        public static MultilistField GetMultilistField(this Item item, string fieldName)
        {
            return item.Fields[fieldName];
        }

        public static Item[] GetMultilistFieldItems(this Item item, string fieldName)
        {
            Item[] items = null;
            MultilistField field = item.Fields[fieldName];
            if ((field != null) && (field.InnerField != null))
            {
                items = field.GetItems();
            }
            return items;
        }

        public static ReferenceField GetReferenceField(this Item item, string fieldName)
        {
            return item.Fields[fieldName];
        }

        public static Item GetReferenceFieldItem(this Item item, string fieldName)
        {
            Item targetItem = null;
            ReferenceField field = item.Fields[fieldName];
            if ((field != null) && (field.TargetItem != null))
            {
                targetItem = field.TargetItem;
            }
            return targetItem;
        }

        public static string GetStringField(this Item item, string fieldName)
        {
            string str = String.Empty;
            if (item != null)
            {
                Field field = item.Fields[fieldName];
                if (!((field == null) || String.IsNullOrEmpty(field.Value)))
                {
                    str = field.Value;
                }
            }
            return str;
        }


        public static bool IsFieldNullorEmptyorWhiteSpace(this Item item, String fieldName)
        {

            return String.IsNullOrEmpty(item[fieldName]) && String.IsNullOrWhiteSpace (item[fieldName]);
        }

        public static List<Item> TopNumofMultilistItems(this Item item, String fieldName, Int32 takeThese)
        {
            List<Item> fieldItems = new List<Item>(item.GetMultilistFieldItems(fieldName));

            if (fieldItems.Any())
            {
                return fieldItems.Count >= takeThese ? new List<Item>(fieldItems.Take(takeThese)) : new List<Item>(fieldItems);
            }

            return new List<Item>();
        }

        public static List<Item> GetLangVersionMultilistItems(this Item item, String fieldName)
        {
            List<Item> targetItems = new List<Item>();

            targetItems.AddRange(item.GetMultilistFieldItems(fieldName));

            if (targetItems.Any())
            {
                
                foreach (Item x in targetItems.Where (x => x.Language != Context.Language))
                {
                    targetItems.Remove (x);
                    targetItems.Add (x.Versions.GetLatestVersion (Context.Language));
                }
            }
            return targetItems;
        }


        public static XmlDocument GetFieldValueAsXmlDocument(this Item item, string fieldName, string xmlWrap = "data")
        {
            var xml = item[fieldName];

            if (String.IsNullOrWhiteSpace(xml))
            {
                return null;
            }


            var doc = new XmlDocument();

            if (!xml.StartsWith("<?xml"))
            {
                var declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null).OuterXml;
                doc.LoadXml(declaration + "<" + xmlWrap + ">" + xml + "</" + xmlWrap + ">");
            }
            else
            {
                doc.LoadXml(xml);
            }

            return doc;
        }

        public static Item GetFieldValueAsItem(this Item item, string fieldName)
        {
            var val = item.TryGetFieldValue(fieldName);

            if (String.IsNullOrWhiteSpace(fieldName))
                return null;

            return item.Database.GetItem(val);
        }

        public static string TryGetFieldValue(this Item item, string fieldName, string defaultValue = "")
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (String.IsNullOrWhiteSpace(fieldName))
                throw new ArgumentNullException("fieldName");

            var field = item.Fields[fieldName];

            if (field == null)
                return defaultValue;

            return field.Value;
        }
    }
}
