angular.module('umbraco').controller('DashboardSitesController', function($scope, $http) {

    $scope.site = $scope.property.sites.length == 0 ? null : $scope.property.sites[0];

    angular.forEach($scope.property.sites, function(site) {
        if (site.id == $scope.property.defaultSite) {
            $scope.site = site;
        }
    });

    $scope.siteChanged = function () {
        $http.get('/umbraco/backoffice/SkybrudDashboard/Dashboard/SetDefaultSite?siteId=' + $scope.site.id);
    };

});