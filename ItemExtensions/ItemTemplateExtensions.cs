using System;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Data.Templates;

namespace Paragon.Sitecore.Extensions.ItemExtensions
{
    public static class ItemTemplateExtensions
    {
        public static bool IsDerived(this Item item, ID templateId)
        {
            if (templateId.IsNull)
            {
                return false;
            }
            TemplateItem templateItem = item.Database.Templates[templateId];
            return item.IsDerived(templateItem);
        }

        public static bool IsDerived(this Item item, TemplateItem templateToCompare)
        {
            if (item == null)
            {
                return false;
            }
            if (templateToCompare == null)
            {
                return false;
            }
            if (item.TemplateID != templateToCompare.ID)
            {
                TemplateItem itemTemplate = item.Database.Templates[item.TemplateID];
                if (itemTemplate == null)
                {
                    return false;
                }
                if (!(itemTemplate.ID == templateToCompare.ID))
                {
                    return TemplateIsDerived(itemTemplate, templateToCompare);
                }
            }
            return true;
        }

        public static bool IsDerived(this Item item, string templateName)
        {
            if (String.IsNullOrEmpty(templateName))
            {
                return false;
            }
            TemplateItem templateItem = item.Database.Templates[templateName];
            return item.IsDerived(templateItem);
        }


        
        
        /// <summary>
        /// Checks to see if the item has the passed template.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="baseTemplateNameOrId"></param>
        /// <returns></returns>
        public static bool HasBaseTemplate(this Item item, string baseTemplateNameOrId)
        {
            Template template = TemplateManager.GetTemplate(item);
            if (template != null)
            {
                if (template.ID.ToString().Equals(baseTemplateNameOrId, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                if (template.FullName.Equals(baseTemplateNameOrId, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                if (template.Name.Equals(baseTemplateNameOrId, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                foreach (Template template2 in template.GetBaseTemplates())
                {
                    if (template2.ID.ToString().Equals(baseTemplateNameOrId, StringComparison.OrdinalIgnoreCase))
                        return true;
                    if (template2.FullName.Equals(baseTemplateNameOrId, StringComparison.OrdinalIgnoreCase))
                        return true;
                    if (template2.Name.Equals(baseTemplateNameOrId, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get the Root Ancestor.
        /// </summary>
        /// <param name="item">Item</param>
        /// <param name="baseTemplate">string</param>
        /// <returns></returns>
        private static Item GetAncestorOrSelfByBaseTemplate(this Item item, string baseTemplate)
        {
            while (true)
            {
                if (item == null || item.HasBaseTemplate(baseTemplate))
                {
                    return item;
                }
                item = item.Parent;
            }
        }

        public static bool IsTemplate(this Item item)
        {
            return item.Database.Engines.TemplateEngine.IsTemplatePart(item);
        }

        private static bool TemplateIsDerived(TemplateItem template, TemplateItem templateToCompare)
        {
            bool result = false;
            foreach (TemplateItem baseTemplate in template.BaseTemplates)
            {
                if (baseTemplate.ID == templateToCompare.ID)
                {
                    return true;
                }
                result = TemplateIsDerived(baseTemplate, templateToCompare);
            }
            return result;
        }


    }
}
