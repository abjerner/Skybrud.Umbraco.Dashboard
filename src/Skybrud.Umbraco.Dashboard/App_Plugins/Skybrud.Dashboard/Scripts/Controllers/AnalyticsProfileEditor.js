angular.module("umbraco").controller("Dashboard.AnalyticsProfileEditor.Controller", function ($scope, dialogService) {

    var service = {
        openProfileDialog: function (options) {
            var d = dialogService.open({
                modalClass: 'dashboard-dialog',
                template: '/App_Plugins/Skybrud.Dashboard/Views/Dialogs/AnalyticsProfileDialog.html',
                show: true,
                animation: false,
                callback: function (value) {
                    if (options && options.callback) options.callback(value);
                },
                cancel: function () {
                    if (options && options.cancel) options.cancel();
                }
            });
            d.element[0].style.width = '1000px';
            d.element[0].style.marginLeft = '-500px';
            return d;
        }
    };

    $scope.open = function () {
        service.openProfileDialog({
            callback: function (profile) {
                $scope.model.value = profile;
            }
        });
    };

    $scope.reset = function () {
        $scope.model.value = null;
    };

});