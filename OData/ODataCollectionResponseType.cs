using System.Text.Json.Serialization;

namespace OData;

public sealed class ODataCollectionResponseType<T>
{
    [JsonPropertyName("@odata.context")]
    public string Context { get; set; }
    [JsonPropertyName("@odata.count")]
    public string Count { get; set; }
    public T[] Value { get; set; } = default!;

    private ODataCollectionResponseType() { }
}