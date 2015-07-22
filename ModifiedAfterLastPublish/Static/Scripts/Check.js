Type.registerNamespace("Alchemy.Plugins.CheckIfModifiedAfterPublish.Commands");

/**
 * Command for checking if an item is modified after last publish.
 */
Alchemy.Plugins.CheckIfModifiedAfterPublish.Commands.Check = function () {
    Type.enableInterface(this, "Alchemy.Plugins.CheckIfModifiedAfterPublish.Commands.Check");
    this.addInterface("Tridion.Cme.Command", ["Check"]);
};

/**
 * Whether or not the command is available.
 */
Alchemy.Plugins.CheckIfModifiedAfterPublish.Commands.Check.prototype.isAvailable = function (selection) {
    if (selection.getItems().length == 1) {
        var item = $models.getItem(selection.getItem(0));
        var itemType = item.getItemType();
        return itemType == "tcm:64";
    }
    else {
        return false;
    }
};

/**
 * Whether or not the command is enabled.
 */
Alchemy.Plugins.CheckIfModifiedAfterPublish.Commands.Check.prototype.isEnabled = function (selection) {
    var item = $models.getItem(selection.getItem(0));
    return item.isPublished();
};

/**
 * Executes the command.
 */
Alchemy.Plugins.CheckIfModifiedAfterPublish.Commands.Check.prototype._execute = function (selection) {
    var item = $models.getItem(selection.getItem(0));
    var itemId = item.getId();
    var encodedItemId = itemId.replace("tcm:", "");
    var progress = $messages.registerProgress("Checking if [" + item.getId() + "] is modified after the last time it was published...", null);
    var Service = Alchemy.Plugins.ModifiedAfterPublish.Api.Service.getReport(encodedItemId)
        .success(function (report) {
            if (!report.IsModifiedAfterPublish) {
                $messages.registerGoal(report.ReportText);
            }
            else {
                $messages.registerWarning(report.ReportText);
            }
        })
        .error(function (type, error) {
            $messages.registerError(error);
        })
        .complete(function () {
            progress.finish();
        });
};