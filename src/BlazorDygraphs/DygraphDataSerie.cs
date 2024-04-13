namespace BlazorDygraphs;
public class DygraphDataSerie
{
    public string Label { get; set; }
    public string LabelX { get; set; }
    public string LabelY { get; set; }
    public List<DygraphDatapoint> Datapoints { get; set; } = new();
}
