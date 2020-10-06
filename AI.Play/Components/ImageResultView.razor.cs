using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
namespace AI.Play.Components
{
    public partial class ImageResultView
    {
        DotNetObjectReference<ImageResultView> DotNetObject;
        string imageToClassify = null;
        ElementReference imageRef;
        string[] colors = new string[] { "success", "danger", "primary", "warning", "dark" };
        ClassResult[] results;

        protected override void OnInitialized()
        {
            DotNetObject = DotNetObjectReference.Create(this);
        }
        async Task Classify()
        {
            await Runtime.InvokeVoidAsync("Classify", DotNetObject, imageRef);
        }


        static string GetLabel(string label)
        {
            if (label.Length > 6)
            {
                return label.Substring(0, 6) + "...";
            }
            else
            {
                return label;
            }
        }
        [JSInvokable("ICClassify")]
        public void ICClassify(ClassResult[] results)
        {
            if (results == null)
                return;

            this.results = results.OrderByDescending(x => x.confidence).ToArray();
            StateHasChanged();
        }
        public void Refresh()
        {
            StateHasChanged();
        }

        public string GetColor(int i)
        {
            return colors[i % colors.Length];
        }
        public async void Save()
        {
            await Runtime.InvokeVoidAsync("Save");
        }
        async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            var imageFiles = e.GetMultipleFiles();

            var format = "image/png";
            foreach (var imageFile in imageFiles)
            {
                var resizedImageFile = await imageFile.RequestImageFileAsync(format, 300, 300);
                var buffer = new byte[resizedImageFile.Size];
                await resizedImageFile.OpenReadStream().ReadAsync(buffer);
                imageToClassify = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
            }

            StateHasChanged();

            if (imageToClassify != null)
                await Classify();
        }

        public class ClassResult
        {
            public string label { get; set; }
            public double confidence { get; set; }
        }
 
    }
}
