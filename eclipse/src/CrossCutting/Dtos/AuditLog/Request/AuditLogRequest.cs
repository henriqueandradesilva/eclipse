namespace CrossCutting.Dtos.AuditLog.Request;

public class AuditLogRequest
{
    public string PropertyName { get; set; }

    public object ModifiedValue { get; set; }

    public object OriginalValue { get; set; }

    public override string ToString()
    {
        return $"Propriedade: {PropertyName}, De: {OriginalValue}, Para: {ModifiedValue}";
    }
}