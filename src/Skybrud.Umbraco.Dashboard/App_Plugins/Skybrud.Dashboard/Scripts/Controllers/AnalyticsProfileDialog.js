angular.module("umbraco").controller("Dashboard.AnalyticsProfileDialog.Controller", function ($scope, $rootScope, $http) {

    $scope.loading = true;

    $scope.users = [];

    $scope.query = '';
    var query = '';

    $scope.updateList = function () {
        $scope.loading = true;
        $http.get('/umbraco/backoffice/SkybrudDashboard/Analytics/GetAccounts' + ($scope.query ? '?query=' + $scope.query : '')).success(function (res) {
            $scope.users = res;
            $scope.loading = false;
        });
    };

    $scope.queryChanged = function () {
        if ($scope.query == query) return;
        query = $scope.query;
        if ($scope.query) {
            $scope.updateList();
        } else {
            $scope.users = [];
        }
    };

    $scope.select = function(user, account, webProperty, profile) {

        var selection = {
            user: user.id,
            account: account.id,
            webProperty: webProperty.id,
            profile: profile
        };

        $scope.submit(selection);

    };

    $scope.updateList();

});