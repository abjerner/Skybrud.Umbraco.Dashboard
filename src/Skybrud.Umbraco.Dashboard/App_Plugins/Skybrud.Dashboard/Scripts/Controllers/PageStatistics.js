angular.module('umbraco').controller('DashboardPageStatisticsController', function($scope, $http, editorState, $element, $timeout) {

    $scope.loading = false;
    $scope.loaded = false;
    $scope.page = null;
    $scope.site = null;
    $scope.error = null;

    // Check whether the site has been published
    $scope.published = editorState.current.published;
    if (!editorState.current.published) return;

    // Hack: Listen for tab changes so we can lazy load the property editor
    angular.element(document.querySelectorAll('.nav-tabs li a')).bind('click', function () {
        $timeout(function() {
            if ($element.is(':visible')) {
                $scope.onVisible($element);
            }
        }, 20);
    });

    // Loads information about and blocks for the current site
    $scope.loadPage = function () {

        $scope.loading = true;

        $http.get('/umbraco/backoffice/SkybrudDashboard/Dashboard/GetPage?pageId=' + editorState.current.id).success(function (body) {

            $scope.loading = false;
            $scope.loaded = true;
            $scope.page = body.page;
            $scope.site = body.site;
            $scope.page.blocks = body.blocks;

        }).error(function (body) {

            $scope.loading = false;
            $scope.loaded = true;
            $scope.error = body;

        });
        
    };

    // Function called when the property editor enters a visible state (tab change)
    $scope.onVisible = function () {
        if ($scope.loading || $scope.loaded) return;
        $scope.init();
    };

    // Initialize the property editor
    $scope.init = function () {
        $scope.loadPage();
    };

    // Initialize the property editor if already visible (eg. if it is on the first tab)
    if ($element.is(':visible')) {
        scope.onVisible($element);
    }

});