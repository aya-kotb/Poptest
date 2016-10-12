var Restart = {

	ReloadBrowser: function()
    {
        window.location.reload(true);
    }
};

mergeInto(LibraryManager.library, Restart);
