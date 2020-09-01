let featureExtractor;

function CreateFeatureExtractor(Dotnet)
{
    featureExtractor = ml5.featureExtractor('MobileNet', ModelLoaded.bind(Dotnet));
}

function ModelLoaded() {
    this.invokeMethodAsync("ICModelLoad");
}
function AddImage(imageElementRef, label) {
    console.log(imageElementRef);
    featureExtractor.addImage(imageElementRef , label);
}

function Train(Dotnet,report) {
    featureExtractor.train((lossValue) =>
    {
        console.log(lossValue + "  ->>js");
        if (report)
            Dotnet.invokeMethodAsync("ICTrain", lossValue);
    });
}