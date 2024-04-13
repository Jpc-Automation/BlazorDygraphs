// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

function InitializeDygraphChart(elementId, title, data, rollPeriod, legend, titleHeight, ylabel, xlabel, strokeWidth) {
    new Dygraph(
        document.getElementById(elementId),
        data, {
        rollPeriod: rollPeriod,
        legend: legend,
        title: title,
        titleHeight: titleHeight,
        ylabel: ylabel,
        xlabel: xlabel,
        strokeWidth: strokeWidth
    });

    return;
}
