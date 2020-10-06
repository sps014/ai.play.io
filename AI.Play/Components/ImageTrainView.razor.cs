using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AI.Play.Components
{
    public partial class ImageTrainView
    {
        bool IsModelLoaded = false;
        static int imageAddedCount = 0;
        bool IsImageAddedBegin = false;
        bool trainingBegin = false;
        static double loss = 0;
        public static bool OnceTrained = false;
        DotNetObjectReference<ImageTrainView> reference;
        bool waitforLoad = false;

        protected override void OnInitialized()
        {
            reference = DotNetObjectReference.Create(this);
            OnLoad();
        }
        async void OnLoad()
        {
            await Runtime.InvokeVoidAsync("CreateFeatureExtractor", reference);
        }
        async Task AddImage(ElementReference image, string label)
        {
            await Runtime.InvokeVoidAsync("AddImage", image, label);
        }
        async Task Train(bool report = true)
        {
            if (!waitforLoad)
            {
                await ReloadModel();
                waitforLoad = true;
                return;
            }
            OnceTrained = false;

            //Add images to NN
            IsImageAddedBegin = true;
            StateHasChanged();
            await Task.Delay(1);
            imageAddedCount = 0;
            foreach (var cls in ImageClassView.Classes)
                foreach (var img in cls.Images)
                {
                    imageAddedCount++;
                    await AddImage(img.Reference, cls.ClassName);
                    StateHasChanged();
                    await Task.Delay(1);
                }
            IsImageAddedBegin = false;
            trainingBegin = true;
            StateHasChanged();
            await Task.Delay(1);
            await Runtime.InvokeVoidAsync("Train", reference, report);
        }

        async Task ReloadModel()
        {
            await Runtime.InvokeVoidAsync("ReloadModel", reference, ImageClassView.Classes.Count);
        }

        [JSInvokable("ICModelLoad")]
        public async void ModelLoaded()
        {
            IsModelLoaded = true;

            if (waitforLoad)
            {
                await Train();
                waitforLoad = false;
            }

            StateHasChanged();
        }
        [JSInvokable("ICTrain")]
        public async void OnTrain(string loss)
        {
            if (string.IsNullOrWhiteSpace(loss))
            {
                trainingBegin = false;
                OnceTrained = true;
                TrainingDone?.Invoke();
            }
            else
            {
                ImageTrainView.loss = double.Parse(loss);
            }
            //Console.WriteLine(loss);
            StateHasChanged();
            await Task.Delay(1);
        }
        public void Refresh()
        {
            StateHasChanged();
        }

        public delegate void TrainingDoneHandler();
        public event TrainingDoneHandler TrainingDone;
        int imagesCount => ImageClassView.Classes.Select(x => x.Images.Count).Sum();
    }
}
