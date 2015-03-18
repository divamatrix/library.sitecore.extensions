using System.Collections.Generic;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Paragon.Sitecore.Extensions.ItemExtensions
{
    public static class ItemAxesExtensions
    {
        /// <summary>
        /// Checks to see if the passed item is a child of the current item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="childIdStr"></param>
        /// <returns></returns>
        public static bool HasChild(this Item item, string childIdStr)
        {
            ID childId = new ID(childIdStr);
            return HasChild(item, childId);
        }

        /// <summary>
        /// Checks to see if the passed item is a child of the current item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="childId"></param>
        /// <returns></returns>
        public static bool HasChild(this Item item, ID childId)
        {
            foreach (Item child in item.Children)
            {
                if (child.ID == childId)
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Looks at the items decendants via recursion to see if the passed item is a descentant.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="descendantIdstr"></param>
        /// <returns></returns>
        public static bool HasDescendant(this Item item, string descendantIdstr)
        {
            ID descendantId = new ID(descendantIdstr);
            return HasDescendant(item, descendantId);
        }

        /// <summary>
        /// Looks at the items decendants via recursion to see if the passed item is a descentant.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="descendantId"></param>
        /// <returns></returns>
        public static bool HasDescendant(this Item item, ID descendantId)
        {
            foreach (Item child in item.Children)
            {
                if (child.ID == descendantId)
                    return true;
                HasDescendant(child, descendantId);
            }
            return false;
        }

        // Methods
        public static IEnumerable<Item> ChildrenDerivedFrom(this Item item, ID templateId)
        {
            ChildList children = item.GetChildren();
            List<Item> childrenDerivedFrom = new List<Item>();
            foreach (Item child in children)
            {
                if (child.IsDerived(templateId))
                {
                    childrenDerivedFrom.Add(child);
                }
            }
            return childrenDerivedFrom;
        }

        public static Item FindAncestorByBaseTemplateId(this Item current, ID baseTemplateId)
        {
            return current.FindAncestorByTemplateId(baseTemplateId);
        }

        public static Item FindAncestorByBaseTemplateId(this Item current, string baseTemplateId)
        {
            return current.FindAncestorByTemplateId(baseTemplateId);
        }

        public static Item FindAncestorByTemplateId(this Item current, ID templateId)
        {
            if (current == null)
            {
                return null;
            }
            return current.IsDerived(templateId) ? current : current.Parent.FindAncestorByTemplateId(templateId);
        }

        public static Item FindAncestorByTemplateId(this Item current, string templateId)
        {
            return current.FindAncestorByTemplateId(new ID(templateId));
        }

        public static Item FindOrCreateChildItem(this Item parent, string name, ID templateId)
        {
            Assert.IsNotNull(parent, "parent cannot be null.");
            Assert.IsNotNullOrEmpty(name, "name cannot be null or empty.");
            Assert.IsNotNull(templateId, "templateId cannot be null.");
            Item child = parent.Database.GetItem(parent.Paths.FullPath + "/" + name);
            if (child == null)
            {
                name = ItemUtil.ProposeValidItemName(name);
                child = parent.Add(name, new TemplateID(templateId));
            }
            return child;
        }

        public static string GetAncestorId(this Item current, string templateId)
        {
            if (current == null)
            {
                return string.Empty;
            }
            Item ancestor = current.FindAncestorByTemplateId(templateId);
            return ancestor == null ? string.Empty : ancestor.ID.ToString();
        }


       

    }
}
