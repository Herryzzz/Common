using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System
{
    public static class UtilsIList
    {
        public static DataTable ToTable<T>(this List<T> entityList)
        {
            DataTable dt = new DataTable();

            //取类型T所有Propertie
            Type entityType = typeof(T);
            PropertyInfo[] entityProperties = entityType.GetProperties();
            Type colType = null;
            foreach (PropertyInfo propInfo in entityProperties)
            {

                if (propInfo.PropertyType.IsGenericType)
                {
                    colType = Nullable.GetUnderlyingType(propInfo.PropertyType);
                }
                else
                {
                    colType = propInfo.PropertyType;
                }

                if (colType.FullName.StartsWith("System"))
                {
                    dt.Columns.Add(propInfo.Name, colType);
                }
            }

            if (entityList != null && entityList.Count > 0)
            {
                foreach (T entity in entityList)
                {
                    DataRow newRow = dt.NewRow();
                    foreach (PropertyInfo propInfo in entityProperties)
                    {
                        if (dt.Columns.Contains(propInfo.Name))
                        {
                            object objValue = propInfo.GetValue(entity, null);
                            newRow[propInfo.Name] = objValue == null ? DBNull.Value : objValue;
                        }
                    }
                    dt.Rows.Add(newRow);
                }
            }

            return dt;
        }
    }
}
