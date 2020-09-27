using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

public class ImageClassContainer
{
    public string ClassName { get; set; }
    public List<ImageRefs> Images { get; set; } = new List<ImageRefs>();
}
public class ImageRefs
{
    public string Image { get; set; }
    public ElementReference Reference { get; set; }
}