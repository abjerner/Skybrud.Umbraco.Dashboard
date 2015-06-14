angular.module("umbraco").directive('dashboardChart', ['$timeout', function ($timeout) {
    return {
        restrict: 'EA',
        templateUrl: '/App_Plugins/Skybrud.Dashboard/Views/Blocks/Chart.html',
        controller: ['$scope', '$element', function ($scope, $element) {

            var chart = $('.graph', $element).text('');

            var canvas = $('<canvas width="' + (chart.width() - 150) + '" height="200"></canvas>').appendTo(chart);
            
            var visits = {
                label: $scope.block.datasets[1].label,
                fillColor: "rgba(65, 73, 92, 0.5)",
                strokeColor: "rgb(65, 73, 92)",
                pointColor: "rgb(65, 73, 92)",
                pointStrokeColor: "#fff",
                pointHighlightFill: "#fff",
                pointHighlightStroke: "rgb(65, 73, 92)",
                data: []
            };

            var pageviews = {
                label: $scope.block.datasets[0].label,
                fillColor: "rgba(141, 146, 157, 0.2)",
                strokeColor: "rgb(141, 146, 157)",
                pointColor: "rgb(141, 146, 157)",
                pointStrokeColor: "#fff",
                pointHighlightFill: "#fff",
                pointHighlightStroke: "rgb(141, 146, 157)",
                data: []
            };

            pageviews.strokeColor = 'rgba(141, 146, 157, 1)';
            pageviews.fillColor = 'rgba(141, 146, 157, 1)';

            visits.strokeColor = 'rgba(65, 73, 92, 1)';
            visits.fillColor = 'rgba(65, 73, 92, 1)';

            visits.strokeColor = '#35353d';
            visits.fillColor = '#35353d';

            var data = {
                labels: [],
                datasets: [pageviews, visits]
            };

            $.each($scope.block.items, function (i, row) {
                data.labels.push(row.label.value.text);
                pageviews.data.push(row.pageviews.value.raw + '');
                visits.data.push(row.visits.value.raw + '');
            });

            var ctx = canvas.get(0).getContext("2d");

            var c = new Chart(ctx).Line(data, {
                bezierCurve: false,
                scaleFontSize: 10,
                scaleFontColor: '#000',
                pointDotRadius: 0,
                showTooltips: true
            });

            var legend = c.generateLegend();

            $(legend).appendTo(chart);

            //$timeout(function() {
            //    alert((chart.width() - 150));
            //}, 500);

        }]
    };
}]);