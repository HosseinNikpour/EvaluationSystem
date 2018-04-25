var myApp = angular.module('persianDatePicker', []);

myApp.directive('persianDatePicker', function () {
    return {
        restrict: 'E',
        scope: {
            model: "="
        },
        template: '<span><input class="persianpicker form-control" type="text" style="width:100px;"/></span>',
        replace: true,
        link: function (scope, elem, attrs, ngModel) {
            var txt = elem.find(".persianpicker");
            txt.datepicker({
                isRTL: true,
                changeMonth: true,
                changeYear: true,
                dateFormat: "yy/mm/dd"
            });

            txt.on('change', function () {
                scope.$apply(function () {
                    try {
                        scope.model = moment(txt.val(), 'jYYYY/jMM/jDD');
                    } catch (e) {

                    }

                });
            });

            scope.$watch('model', function (newValue, oldValue) {
                if (newValue && moment.isMoment(newValue)) {
                    txt.datepicker("setDate", newValue.format('jYYYY/jMM/jDD'));
                }
                else {
                    txt.val(null);
                }
            });
        }
    }
});
