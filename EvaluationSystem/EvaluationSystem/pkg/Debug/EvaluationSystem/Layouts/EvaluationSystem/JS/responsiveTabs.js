 angular.module('responsive-tabs', [])
  .directive('responsiveTabs', responsiveTabs);

function responsiveTabs() {
  return {
    restrict: 'A',
    link: function(scope, element, attrs) {
      element.responsiveTabs({
        startCollapsed: false
      });

    }
  };
}