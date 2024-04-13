/*
 <script src="https://cdnjs.cloudflare.com/ajax/libs/dygraph/2.1.0/dygraph-combined.min.js"></script>
< link rel = "stylesheet" href = "https://cdnjs.cloudflare.com/ajax/libs/dygraph/2.1.0/dygraph.min.css" />
https://rstudio.github.io/dygraphs/gallery-range-selector.html
https://dygraphs.com/data.html#datatable
https://dygraphs.com/gallery/#g/synchronize
*/

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace BlazorDygraphs;
public class DygraphComponent : ComponentBase, IAsyncDisposable
{
    [Parameter] public string? Id { get; set; }
    [Parameter] public string? CssClass { get; set; }
    [Parameter] public string? Style { get; set; }
    [Parameter] public string Title { get; set; } = "My Graph";

    [Parameter] public string? LabelY { get; set; }
    [Parameter] public string? LabelX { get; set; }
    [Parameter] public List<DygraphDataSerie> Series { get; set; } = new();
    [Parameter] public Dictionary<string, object>? Options { get; set; }
    [Parameter] public int Width { get; set; } = 800;
    [Parameter] public int Height { get; set; } = 600;
    [Parameter] public int TitleHeight { get; set; } = 32;
    [Parameter] public double StrokeWidth { get; set; } = 1.5;

    [Parameter] public bool ShowRangeSelector { get; set; }
    [Parameter] public int RangeSelectorHeight { get; set; }
    [Parameter] public string? RangeSelectorPlotStrokeColor { get; set; }
    [Parameter] public string? RangeSelectorPlotFillColor { get; set; }
    [Parameter] public string? RangeSelectorStart { get; set; }
    [Parameter] public string? RangeSelectorEnd { get; set; }

    [Parameter] public double? ValueRangeMin { get; set; }
    [Parameter] public double? ValueRangeMax { get; set; }

    [Parameter] public bool Fractions { get; set; }
    [Parameter] public bool ErrorBars { get; set; }
    [Parameter] public bool ShowRoller { get; set; }
    [Parameter] public int RollPeriod { get; set; }
    [Parameter] public bool CustomBars { get; set; }
    [Parameter] public LegendTypes Legend { get; set; } = LegendTypes.Always;

    [Parameter] public bool AnimatedZooms { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    [Inject] private IJSRuntime? JSRuntime { get; set; }

    //private ElementReference _elementRef;
    //private DotNetObjectReference<DygraphComponent> _ref;
    //private long? _lastAnimationFrameId;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "id", Id);
        builder.AddAttribute(2, "class", CssClass);
        builder.AddMultipleAttributes(3, AdditionalAttributes);
        builder.AddAttribute(4, "style", $"width:{Width}px; height:{Height}px;{Style}");
        //builder.AddElementReferenceCapture(5, __canvasElement => _elementRef = __canvasElement);

        //builder.AddContent(5, ChildContent);

        builder.CloseElement();

    }

    protected override void OnInitialized()
    {
        if (string.IsNullOrEmpty(Id))
            Id = GenerateElementId();

        //_ref = DotNetObjectReference.Create(this);
        base.OnInitialized();
    }

    //protected override async Task OnAfterRenderAsync(bool firstRender)
    //{
    //    if (firstRender)
    //    {
    //        var optionsObj = Options == null ? "{}" : JsonSerializer.Serialize(Options);
    //        await JSRuntime.InvokeVoidAsync(
    //            "window.dygraphCallbacks",
    //            DotNetObjectReference.Create(_ref),
    //            _elementRef,
    //            Title,
    //            optionsObj);
    //    }

    //    if (!_lastAnimationFrameId.HasValue && DataPoints.Count > 0)
    //    {
    //        _lastAnimationFrameId = AnimationLoop();
    //    }
    //}

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Interop();
            //List<int[]> itemsss = new();
            //itemsss.Add([1, 10, 100]);
            //itemsss.Add([2, 20, 80]);
            //itemsss.Add([3, 50, 60]);
            //itemsss.Add([4, 70, 80]);

            //var desw = itemsss.ToArray();
            //var dsx = GetDataFromSeries(Series);

            //string[] labels = ["aa", "bb", "cc"];

            //await JSRuntime.InvokeVoidAsync(
            //    "InitializeDygraphChart",
            //    Id,
            //    Title,
            //    GetLabelsFromSeries(Series),
            //    GetDataFromSeries(Series),
            //    Legend.ToString().ToLower(),
            //    TitleHeight,
            //    LabelY,
            //    LabelX,
            //    StrokeWidth,
            //    Fractions,
            //    ErrorBars,
            //    ShowRoller,
            //    RollPeriod,
            //    RangeMin,
            //    RangeMax,
            //    AnimatedZooms
            //    );
        }
    }

    [JSInvokable]
    public ValueTask UpdateGraph(List<DygraphDataSerie> series)
    {
        Series = series;
        //StateHasChanged();

        return JSRuntime.InvokeVoidAsync(
        "UpdateDygraphChart",
        Id,
        GetDataFromSeries(Series));
    }

    private ValueTask Interop()
    {
        return JSRuntime.InvokeVoidAsync(
                "InitializeDygraphChart",
                Id,
                Title,
                GetLabelsFromSeries(Series),
                GetDataFromSeries(Series),
                Legend.ToString().ToLower(),
                TitleHeight,
                LabelY,
                LabelX,
                StrokeWidth,
                Fractions,
                ErrorBars,
                ShowRoller,
                RollPeriod,
                ValueRangeMin,
                ValueRangeMax,
                AnimatedZooms
                );

    }

    private string GenerateElementId()
    {
        return string.Concat(Guid.NewGuid().ToString()
            .Where(c => !c.Equals('-'))).Remove(12)
            .Insert(0, "Z");
    }

    private object[][] GetDataFromSeries(List<DygraphDataSerie> series)
    {
        var seriesCount = series.Count;
        var datapointCount = series.First().Datapoints.Count;

        List<object[]> items = new();

        for (var i = 0; i < datapointCount; i++)
        {
            List<object> serieItems = new();

            serieItems.Add(series[0].Datapoints[i].ValueX);

            for (int i2 = 0; i2 < seriesCount; i2++)
            {
                serieItems.Add(series[i2].Datapoints[i].ValueY);
            }

            items.Add(serieItems.ToArray());
        }

        return items.ToArray();
    }

    private string[] GetLabelsFromSeries(List<DygraphDataSerie> series)
    {
        List<string> items = new List<string>();

        var serie2 = series.First();
        items.Add(serie2.LabelX);

        foreach (var serie in series)
        {
            items.Add(serie.Label);
        }

        return items.ToArray();
    }


    public void Dispose()
    {
        JSRuntime.InvokeVoidAsync("DisposeChart", Id);
        //_ref.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await JSRuntime.InvokeVoidAsync("DisposeChart", Id);
    }
}
