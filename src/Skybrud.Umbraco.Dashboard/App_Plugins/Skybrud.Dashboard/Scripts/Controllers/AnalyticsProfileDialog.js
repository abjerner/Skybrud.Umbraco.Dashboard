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
        $scope.updateList();
    };

    $scope.select = function(user, account, webProperty, profile) {

        var selection = {
            user: user.id,
            account: {
                id: account.id,
                name: account.name
            },
            webProperty: {
                id: webProperty.id,
                name: webProperty.name
            },
            profile: profile
        };

        $scope.submit(selection);

    };

    $scope.updateList();

});