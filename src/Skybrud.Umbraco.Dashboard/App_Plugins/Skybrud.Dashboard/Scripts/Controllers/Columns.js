angular.module('umbraco').controller('DashboardColumnsController', function($scope, $http) {

    if ($scope.block.columns.length == 1) {
        $scope.block.columns.push({
            view: '/App_Plugins/Skybrud.Dashboard/Views/Blocks/Dummy.html'
        });
    }

    if ($scope.block.columns.length <= 2) {
        $scope.block.columns.push({
            view: '/App_Plugins/Skybrud.Dashboard/Views/Blocks/Dummy.html'
        });
    }

});