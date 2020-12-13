mergeInto(LibraryManager.library, {

    CopyPasteReader: function(gObj, vName)
    {
        console.log('CopyPasteReader');

        var gameInstance = window.unityInstance;
        var gameObjectName = UTF8ToString(gObj);
        var voidName = UTF8ToString(vName);
        navigator.clipboard.readText().then(function(data) {
            gameInstance.SendMessage(gameObjectName, voidName, data);
        }, function(e) {
            gameInstance.SendMessage(gameObjectName, voidName, "");
        });
    }
});