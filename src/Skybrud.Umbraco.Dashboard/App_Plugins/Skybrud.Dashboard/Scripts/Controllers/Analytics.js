angular.module("umbraco").controller("Analytics.Controller", ['$scope', '$http', '$timeout', '$q', function ($scope, $http, $timeout, $q) {

    $scope.blocks = [];
    $scope.loading = false;
    $scope.error = null;

    $scope.hasData1 = false;
    $scope.hasData2 = false;

    $scope.periods = $scope.block.periods;
    





    // Declare a callback for a successful response
    function callbackSuccess(r) {

        $scope.loading = false;

        $scope.linecharts = [r.data.linechart];
        $scope.linechart = r.data.linechart;
        $scope.blocks = r.data.blocks;

        $scope.hasData1 = $scope.linechart && $scope.linechart.hasData;
        $scope.hasData2 = false;
        angular.forEach($scope.blocks, function (block) {
            $scope.hasData2 = $scope.hasData2 || block.hasData;
        });

    }

    function callbackError(res, status) {
        $scope.error = {
            title: res.meta && res.meta.title ? res.meta.title : 'Hov! Der skete en fejl!',
            message: res.meta && res.meta.message ? res.meta.message : res.meta.error
        };
        $scope.loading = false;
    }





    $scope.selectTab = function(tab) {

        if ($scope.loading) return;

        $scope.loading = true;

        $scope.period = tab;

        if ($scope.page && $scope.page.id > 0) {

            // Make the call to the dashboard analytics service
            $http({
                method: 'GET',
                url: $scope.block.serviceurl + 'GetPageData',
                params: {
                    siteId: $scope.site.id,
                    pageId: $scope.page.id,
                    period: $scope.period.alias,
                    cache: true
                }
            }).success(callbackSuccess).error(callbackError);

        } else {

            // Make the call to the dashboard analytics service
            var http = $http({
                method: 'GET',
                url: $scope.block.serviceurl + 'GetSiteData',
                params: {
                    siteId: $scope.site.id,
                    period: $scope.period.alias,
                    cache: true
                }
            });

        }

        // Show the loader for at least 200 ms
        var timer = $timeout(function () { }, 200);

        // Wait for both the AJAX call and the timeout
        $q.all([http, timer]).then(function (array) {
            callbackSuccess(array[0].data, array[0].status, array[0].headers, array[0].config);
        }, function (obj) {
            callbackError(obj.data, obj.status, obj.headers, obj.config);
        });

    };

    $timeout(function () {
        $scope.selectTab($scope.periods[0]);
    }, 200);

}]);