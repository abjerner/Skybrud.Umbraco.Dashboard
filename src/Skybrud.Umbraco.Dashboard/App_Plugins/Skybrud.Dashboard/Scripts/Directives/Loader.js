angular.module("umbraco").directive('dashboardLoader', function () {

    // Get the Umbraco version
    var v = Umbraco.Sys.ServerVariables.application.version.split('.');

    // Use the new Umbraco 7.4 loader :D
    if (v[0] == 7 && v[1] >= 4) {
        return {
            restrict: 'E',
            replace: true,
            template: '<div class="DashboardLoader"><umb-load-indicator /></div>'
        };
    }

    // Fallback to our custom loader
    return {
        restrict: 'E',
        replace: true,
        template: '<div class="DashboardLoader"><div class="spinner"><div class="spinner-container container1"><div class="circle1"></div><div class="circle2"></div><div class="circle3"></div><div class="circle4"></div></div><div class="spinner-container container2"><div class="circle1"></div><div class="circle2"></div><div class="circle3"></div><div class="circle4"></div></div><div class="spinner-container container3"><div class="circle1"></div><div class="circle2"></div><div class="circle3"></div><div class="circle4"></div></div></div></div>'
    };

});