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
    var item = $models.getItem(selection.getItem(0));
    var itemType = item.getItemType();
    return itemType == "tcm:64";
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
    var Service = Alchemy.Plugins.CheckIfModifiedAfterPublish.Api.Service.getReport(encodedItemId)
        .success(function (report) {
            var ok = report.split("|")[0] == "OK";
            if (ok) {
                $messages.registerGoal(report.split("|")[1]);
            }
            else {
                $messages.registerWarning(report.split("|")[1]);
            }
        })
        .error(function (type, error) {
            $messages.registerError("FAIL! ARGH!");
        })
        .complete(function () {
            progress.finish();
        });
};