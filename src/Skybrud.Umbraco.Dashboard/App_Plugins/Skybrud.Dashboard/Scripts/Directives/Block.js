angular.module("umbraco").directive('dashboardBlock', ['$compile', '$templateCache', '$http', function ($compile, $templateCache, $http) {
    return {
        restrict: 'EA',
        link: function (scope, element, attrs) {
            if (!scope.block) return;
            $http.get(scope.block.view, { cache: $templateCache }).success(function (html) {
                element.replaceWith($compile(html)(scope));
            });
        }
    };
}]);