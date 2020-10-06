using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;

namespace AI.Play.Components
{
    public partial class ClassModifyView
    {
        int ColCount = 4;
        [Parameter]
        public string Index { get; set; }
        int i => int.Parse(Index);
        ImageClassContainer Class => ImageClassView.Classes[i];

        void RemoveImage(ImageRefs refs)
        {
            if (Class.Images.Contains(refs))
                Class.Images.Remove(refs);
            StateHasChanged();
        }
        async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            var imageFiles = e.GetMultipleFiles(1024);

            var format = "image/png";
            foreach (var imageFile in imageFiles)
            {
                var resizedImageFile = await imageFile.RequestImageFileAsync(format, 400, 400);
                var buffer = new byte[resizedImageFile.Size];
                await resizedImageFile.OpenReadStream().ReadAsync(buffer);
                var imageDataUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                Class.Images.Add(new ImageRefs() { Image = imageDataUrl });
            }

            StateHasChanged();
        }

    }
}
