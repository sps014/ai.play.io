let featureExtractor;

function CreateFeatureExtractor(Dotnet) {
    console.clear();
    featureExtractor = ml5.featureExtractor('MobileNet', ModelLoaded.bind(Dotnet));
    featureExtractor = featureExtractor.classification();
}

function ModelLoaded() {
    this.invokeMethodAsync("ICModelLoad");
}
function AddImage(imageElementRef, label) {
    featureExtractor.addImage(imageElementRef, label);
}

function Train(Dotnet, report) {
    featureExtractor.train((lossValue) => {
        if (report)
            Dotnet.invokeMethodAsync("ICTrain", lossValue);
    });
}
function Classify(Dotnet, input) {
    featureExtractor.classify(input, OnClassify.bind(Dotnet));
}
function OnClassify(err, results) {
    this.invokeMethodAsync("ICClassify", results);
}