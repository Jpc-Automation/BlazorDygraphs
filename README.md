﻿## Give a Star! :star:
If you like or are using this project please give it a star. Thanks!

# Description

Simple lightweigt chart plotter for Blazor

## Documentation

add '<script src="_content/BlazorDygraphs/js/blazordygraph.js"></script>' to the index.html.

<!DOCTYPE html>
<html lang="en">

    <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <title>BlazorWasm</title>
        <base href="/" />
        <link rel="stylesheet" href="css/bootstrap/bootstrap.min.css" />
        <link rel="stylesheet" href="css/app.css" />
        <link rel="icon" type="image/png" href="favicon.png" />
        <link href="BlazorWasm.styles.css" rel="stylesheet" />
    </head>

    <body>

        <div id="app">
            <svg class="loading-progress">
                <circle r="40%" cx="50%" cy="50%" />
                <circle r="40%" cx="50%" cy="50%" />
            </svg>
            <div class="loading-progress-text"></div>
        </div>

        <div id="blazor-error-ui">
            An unhandled error has occurred.
            <a href="" class="reload">Reload</a>
            <a class="dismiss">🗙</a>
        </div>

        <script src="_framework/blazor.webassembly.js"></script>
        <script src="_content/BlazorDygraphs/js/blazordygraph.js"></script>
    </body>

</html>