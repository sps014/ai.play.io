using Microsoft.AspNetCore.Components;
using System;

public class ImageClassContainer
{
    public string ClassName { get; set; }
    public ImageRefs[] Images { get; set; } = Array.Empty<ImageRefs>();
}
public class ImageRefs
{
    public string Image { get; set; }
    public ElementReference Reference { get; set; }
}