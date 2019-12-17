using System;
using System.Collections.Generic;
using System.ComponentModel;
namespace Herryz.Common
{
	public class ModelCopier
	{
		public static void CopyCollection<T>(IEnumerable<T> from, ICollection<T> to)
		{
			if (from == null || to == null || to.IsReadOnly)
			{
				return;
			}
			to.Clear();
			foreach (T current in from)
			{
				to.Add(current);
			}
		}
		public static void CopyModel(object from, object to)
		{
			if (from == null || to == null)
			{
				return;
			}
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(from);
			PropertyDescriptorCollection properties2 = TypeDescriptor.GetProperties(to);
			foreach (PropertyDescriptor propertyDescriptor in properties)
			{
				PropertyDescriptor propertyDescriptor2 = properties2.Find(propertyDescriptor.Name, true);
				if (propertyDescriptor2 != null && !propertyDescriptor2.IsReadOnly)
				{
					bool flag = propertyDescriptor2.PropertyType.IsAssignableFrom(propertyDescriptor.PropertyType);
					bool flag2 = !flag && Nullable.GetUnderlyingType(propertyDescriptor.PropertyType) == propertyDescriptor2.PropertyType;
					if (flag || flag2)
					{
						object value = propertyDescriptor.GetValue(from);
						if (flag || (value != null && flag2))
						{
							propertyDescriptor2.SetValue(to, value);
						}
					}
				}
			}
		}
	}
}
