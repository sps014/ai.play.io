let featureExtractor;
let model;
function CreateFeatureExtractor(Dotnet) {
    console.clear();
    model = ml5.featureExtractor('MobileNet', ModelLoaded.bind(Dotnet));
    featureExtractor = model.classification();
}

function ModelLoaded() {
    this.invokeMethodAsync("ICModelLoad");
}
function ReloadModel(Dotnet,numClasses = 2) {
    model = ml5.featureExtractor('MobileNet', { numLabels: numClasses }, ModelLoaded.bind(Dotnet));
    featureExtractor = model.classification();
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
 function Save() {
    featureExtractor.save();
}