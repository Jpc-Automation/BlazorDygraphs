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
using System.Text.Json;

namespace BlazorDygraphs;
public class DygraphComponent : ComponentBase, IDisposable
{
    [Parameter] public string? Id { get; set; }
    [Parameter] public string? CssClass { get; set; }
    [Parameter] public string? Style { get; set; }
    [Parameter] public string Title { get; set; } = "My Graph";
    [Parameter] public string LabelY { get; set; }
    [Parameter] public string LabelX { get; set; }
    [Parameter] public int RollPeriod { get; set; }
    [Parameter] public List<(double x, double y)> DataPoints { get; set; } = new();
    [Parameter] public Dictionary<string, object>? Options { get; set; }
    [Parameter] public int Width { get; set; } = 800;
    [Parameter] public int Height { get; set; } = 600;
    [Parameter] public int TitleHeight { get; set; } = 32;
    [Parameter] public double StrokeWidth { get; set; } = 1.5;
    [Parameter] public string? RangeSelectorStart { get; set; }
    [Parameter] public string? RangeSelectorEnd { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }


    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }



    [Inject] private IJSRuntime? JSRuntime { get; set; }

    private ElementReference _elementRef;
    private DotNetObjectReference<DygraphComponent> _ref;

    private long? _lastAnimationFrameId;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "id", Id);
        builder.AddAttribute(2, "class", CssClass);
        builder.AddMultipleAttributes(3, AdditionalAttributes);
        builder.AddAttribute(4, "style", $"width:{Width}px; height:{Height}px;{Style}");
        builder.AddElementReferenceCapture(5, __canvasElement => _elementRef = __canvasElement);

        builder.AddContent(5, ChildContent);

        builder.CloseElement();

    }

    protected override void OnInitialized()
    {
        if (string.IsNullOrEmpty(Id))
            Id = GenerateElementId();

        _ref = DotNetObjectReference.Create(this);
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
            var jsonOptions = JsonSerializer.Serialize(Options);

            await JSRuntime.InvokeVoidAsync(
                "InitializeDygraphChart",
                Id,
                Title,
                DataPoints.Select(dp => $"[{dp.x},{dp.y}]").ToArray(),
                7,
                "always",
                TitleHeight,
                LabelY,
                LabelX,
                StrokeWidth);
        }
    }

    private string GenerateElementId()
    {
        return string.Concat(Guid.NewGuid().ToString()
            .Where(c => !c.Equals('-'))).Remove(12)
            .Insert(0, "Z");
    }

    //[JSInvokable]
    //public void PlotDataPoints(string dataString)
    //{
    //    InvokeAsync(() =>
    //    {
    //        var dotnetCanvas = (ElementRef.Value as IJSRuntime).GetEnumerable<IJSInProcessObjectReference>() ?? Enumerable.Empty<IJSInProcessObjectReference>();
    //        foreach (var jsRef in dotnetCanvas)
    //        {
    //            try
    //            {
    //                jsRef.InvokeVoidAsync("plotDataPoints", dataString);
    //            }
    //            catch
    //            {
    //                // Ignore exceptions when trying to invoke the method since the canvas may no longer exist
    //            }
    //        }
    //    });
    //}

    [JSInvokable]
    public void UpdateGraph(List<(double x, double y)> newDataPoints)
    {
        DataPoints = newDataPoints;
        StateHasChanged();
    }

    private string[] DatapointsToString(List<(double x, double y)> datapoints)
    {
        var ff = datapoints.Select(dp => $"[{dp.x},{dp.y}]").ToArray();
        return ff;
    }

    public void Dispose()
    {
        _ref.Dispose();
    }
}
