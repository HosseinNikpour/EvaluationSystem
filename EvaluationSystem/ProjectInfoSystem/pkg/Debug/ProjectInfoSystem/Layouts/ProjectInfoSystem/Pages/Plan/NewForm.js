
var app = angular.module('plnApp', []);

app.filter('jalaliDate', function () {
    return function (inputDate, format) {
        var date = moment(inputDate);
        return date.format(format);
    }
});


app.controller('plnCtrl', function ($scope, service, $q) {


    $scope.loading = true;
    $scope.prvUrl = '/Lists/WeeklyPlanConstructions/AllItems.aspx';
    $q.all([service.getPeriods(), service.getContracts()]).then(function (result) {

        $scope.periods = result[0].data.d.results;
        $scope.contracts = result[1].data.d.results;
        $scope.loading = false;
    })


    $scope.showOperations = function () {


        $scope.loading = true;

        service.isExitSameWeeklyPlanReport($scope.contract.Id, $scope.period.Id).then(function (result) {

            $scope.isExitSameReport = result.data.d;

            if ($scope.isExitSameReport) {
                alert('برای این پیمان قبلا در این دوره پلن هفتگی ثبت شده است.');

            }
            else {
                //  $scope.showTabelOperationOnContract = 'showTabelOperationOnContract';
                $q.all([service.getOperationsOnFarmsFilterContract($scope.contract.Id), service.getWeeklyDetailPrevius($scope.contract.Id, moment($scope.period.StartDate).format('YYYY/MM/DD'))]).then(function (a) {
                    $scope.tableData = a[0].data.d.results;
                    $scope.priviousDetails = a[1].data.d;
                    // calcTotal();


                    $scope.tableData.map(function (itm) {

                        var sum = 0;
                        var result = $scope.priviousDetails.filter(function (f) {
                            if (itm.Measurement)
                                return (f.ExecutiveOperationId == itm.Operation.Id && f.SubExecutiveOperationId == itm.SubOperation.Id && f.Measure == itm.Measurement)
                            else
                                return (f.ExecutiveOperationId == itm.Operation.Id && f.SubExecutiveOperationId == itm.SubOperation.Id)
                        });
                        for (var i = 0; i < result.length; i++) {
                            sum += result[i].DoneWork;
                        }
                        itm.Constructed = sum;

                    });




                })

            }
        });

        $scope.loading = false;
    }



    function ValidationSum() {
        var count = 0;
        for (var i = 0; i < $scope.OperationData.length ; i++) {
            value = $scope.OperationData[i].sum;
            if (value > $scope.OperationData[i].FinalVolume) {
                count++;
                false;
            } else {
                true;
            }
        }

        return count;
    }



    $scope.save = function (isTemp) {
        $scope.loading = true;





        var weeklyItem = {};

        weeklyItem.Id = 0,
        weeklyItem.Titel = "",
        weeklyItem.Contract = $scope.contract.Title,
        weeklyItem.ContractId = $scope.contract.Id,

        weeklyItem.Period = $scope.period.Id,

        weeklyItem.items = [];


        for (var i = 0; i < $scope.tableData.length ; i++) {
            if ($scope.tableData[i].hasError) {
                alert('لطفا موراد نشان داده را برطرف و مجددا ذخیره نمایید ');
                $scope.loading = false;
                return;
            }
            var WeeklyOperationItem = {}
            WeeklyOperationItem.Id = 0,
            WeeklyOperationItem.Titel = "",
            WeeklyOperationItem.DoneWork = $scope.tableData[i].doneWork,
            WeeklyOperationItem.ExecutiveOperation = $scope.tableData[i].Operation.Title,
            WeeklyOperationItem.ExecutiveOperationId = $scope.tableData[i].Operation.Id,
            WeeklyOperationItem.SubExecutiveOperation = $scope.tableData[i].SubOperation.Title,
            WeeklyOperationItem.SubExecutiveOperationId = $scope.tableData[i].SubOperation.Id,
            WeeklyOperationItem.Measure = $scope.tableData[i].Measurement,
            weeklyItem.items.push(WeeklyOperationItem);
        }



        service.savePlan(weeklyItem).then(function (d) {
            $scope.loading = false;
            if (d.data.d == "") {
                alert('اطلاعات با موفقیت ذخیره شد');
                $scope.loading = false;
                window.location = $scope.prvUrl;
            }
            else {
                alert("خطا در ذخیره سازی اطلاعات" + d.data.d);
                console.log(d.data.d);
                $scope.loading = false;
            }
        },
        function (d) {

            console.log(d);
        });
    }


    $scope.validateCell = function (item) {
        if ((item.Constructed + item.doneWork) > item.CheckValue) {
            item.hasError = true;
        }
        else { item.hasError = false; }
    }
    $scope.close = function () {
        window.location.href = $scope.prvUrl;
    }

});
