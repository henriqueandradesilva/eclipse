using CrossCutting.Dtos.AuditLog.Request;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CrossCutting.Helpers;

public static class AuditLogHelper
{
    public static List<AuditLogRequest> GetListDifference<T>(T source, T destination)
    {
        if (source == null || destination == null)
            throw new ArgumentNullException("As entidades de origem e destino não podem ser nulas.");

        var listdifference = new List<AuditLogRequest>();

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (property.Name == "DateUpdated" || property.Name == "DateCreated")
                continue;

            if (!property.CanRead || !property.CanWrite)
                continue;

            var modifyValue = property.GetValue(source);
            var originalValue = property.GetValue(destination);

            if (!Equals(modifyValue, originalValue))
            {
                listdifference.Add(new AuditLogRequest
                {
                    PropertyName = property.Name,
                    ModifiedValue = modifyValue,
                    OriginalValue = originalValue
                });
            }
        }

        return listdifference;
    }
}