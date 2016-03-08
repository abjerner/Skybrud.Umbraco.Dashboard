angular.module('umbraco').controller('DashboardSiteStatisticsController', function($scope, $http, editorState) {

    $scope.siteId = editorState.current.id;
    $scope.site = null;
    $scope.loading = true;
    $scope.error = null;

    // Check whether the site has been published
    $scope.published = editorState.current.published;
    if (!editorState.current.published) return;

    // Get dashboard blocks for the site
    $http.get('/umbraco/backoffice/SkybrudDashboard/Dashboard/GetSite?siteId=' + $scope.siteId).success(function (body) {
        $scope.loading = false;
        $scope.site = body;
    }).error(function(body) {
        $scope.loading = false;
        $scope.error = body;
    });

});