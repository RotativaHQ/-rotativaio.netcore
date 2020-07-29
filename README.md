# RotativaIO.NetCore

This library let's you use rotativa.io to create PDF files using Razor templates without requiring to referenve Asp.Net Core libraries. This makes it suitable to be used in console applications or Azure Functions

This requires a valid [rotativa.io](https://rotativa.io) account. You can grab a (limited) [free account](https://www.rotativa.io/Register) or [subscribe a paid plan](https://www.rotativa.io/Subscriptions/New).

Usage is dead simple:

Install the nuget package

```bash
Install-package RotativaIO.NetCore
```

Create a new `PdfHelper` object passing your account's ApiKey and your chosen endpoint. use one of the helper's async methods to create PDF files.

```csharp

var template = "Hello @Model.Name";
var model = new TestModel { Name = "Giorgio" };
using (var pdfHelper = new PdfHelper(rotativaioKey, "https://eunorth.rotativahq.com"))
{ 

    var pdfBytes = await pdfHelper.GetPdfAsByteArray(
        template, 
        model, 
        new RotativaOptions 
        { 
            PageSize = Size.A5 
        });
    
    /// do something with it like, for example, send the PDF via email
    
}

```
You can find a code example in the demo Azure function project.
