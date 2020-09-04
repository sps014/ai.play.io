let featureExtractor;

function CreateFeatureExtractor(Dotnet)
{
    featureExtractor = ml5.featureExtractor('MobileNet', ModelLoaded.bind(Dotnet));
    featureExtractor = featureExtractor.classification();
}

function ModelLoaded() {
    this.invokeMethodAsync("ICModelLoad");
}
function AddImage(imageElementRef, label) {
    console.log(imageElementRef);
    featureExtractor.addImage(imageElementRef, label);
}

function Train(Dotnet,report) {
    featureExtractor.train((lossValue) =>
    {
        if (report)
            Dotnet.invokeMethodAsync("ICTrain", lossValue);
    });
}