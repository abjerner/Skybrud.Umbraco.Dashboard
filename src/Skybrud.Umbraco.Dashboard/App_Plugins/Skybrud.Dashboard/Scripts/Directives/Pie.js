angular.module("umbraco").directive('dashboardPie', ['$compile', '$templateCache', '$http', function ($compile, $templateCache, $http) {
    return {
        restrict: 'EA',
        replace: true,
        scope: {
            data: '=',
        },
        template: '<div class="pie"></div>',
        controller: ['$scope', '$element', function ($scope, $element) {

            // Declare some colors for the data model
            //var colors = ['#41495c', '#676d7d', '#8d929d', '#00ff00', '#ff00ff'];
            var colors = ['#35353d', '#676d7d', '#8d929d', '#00ff00', '#ff00ff'];

            var data = [];

            angular.forEach($scope.data, function (row, index) {
                data.push({
                    value: row.visits.percent.raw,
                    color: colors[index],
                    label: row.text
                });
            });

            var canvas = $('<canvas width="170" height="170"></canvas>').appendTo($element);

            // Get the 2D context of the canvas
            var ctx = canvas.get(0).getContext("2d");

            // Initialize the pie chart
            new Chart(ctx).Pie(data, {
                segmentStrokeWidth: 0,
                segmentShowStroke: false,
                animation: false,
                showTooltips: false
            });

        }]
    };
}]);