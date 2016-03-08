angular.module("umbraco").controller('DashboardLastEditedController', ['$scope', '$http', '$timeout', '$q', function ($scope, $http, $timeout, $q) {

    // Set a flag telling the block is loading
    $scope.loading = true;

    // Set the items to an empty array
    $scope.items = [];

    function callbackSuccess(r) {
        $scope.loading = false;
        $scope.items = r.data;
    }

    function callbackError(res) {
        $scope.loading = false;
        $scope.error = {
            title: 'Hov! Der skete en fejl!',
            message: res.meta ? res.meta.error : ''
        };
    }








    // Get the items from the API
    var http = $http({
        method: 'GET',
        url: '/umbraco/backoffice/SkybrudDashboard/Dashboard/GetLastEditedData',
        params: {
            siteId: $scope.site.id
        }
    });

    // Show the loader for at least 200 ms
    var timer = $timeout(function () { }, 200);

    // Wait for both the AJAX call and the timeout
    $q.all([http, timer]).then(function (array) {
        callbackSuccess(array[0].data, array[0].status, array[0].headers, array[0].config);
    }, function (obj) {
        callbackError(obj.data, obj.status, obj.headers, obj.config);
    });











    //$http.get($scope.serviceurl + 'GetLastEditedData?siteId=' + $scope.dashboard.site.id).success(function (r) {

    //	$scope.items = r.data;
    //	$scope.loading = false;

    //});

}]);