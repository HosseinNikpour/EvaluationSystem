var app = angular.module('opApp', []);

app.filter('jalaliDate', function () {
    return function (inputDate, format) {
        var date = moment(inputDate);
        return date.format(format);
    }
});


app.controller('opCtrl', function ($scope, service, $q) {
   

    $scope.loading = true;
    $scope.listname = 'WeeklyConstructions';
    $scope.requestId = getUrlVars()['ID'];
    $scope.prvUrl = '/Lists/WeeklyConstructions/AllItems.aspx';

    $q.all([service.getWeeklyItem($scope.requestId), service.getWeeklyItemDetails($scope.requestId), service.canApprove($scope.requestId, 'WeeklyConstructions'), service.getHistories($scope.requestId, 'WeeklyConstructions')]).then(function (d) {

        $scope.weeklyItem = d[0].data.d.results[0];
        $scope.weeklyItem.Items = d[1].data.d.results;
        $scope.canApprove = d[2].data.d;
        $scope.histories = d[3].data.d.histories;
        for (var i = 0; i < $scope.histories.length; i++) {
            $scope.histories[i].HistoryDate = moment($scope.histories[i].HistoryDate).format('jYYYY/jMM/jDD');

        }
        $q.all([service.getOperationsOnFarmsFilterContract($scope.weeklyItem.ContractId), service.getWeeklyDetailPrevius($scope.weeklyItem.ContractId, moment($scope.weeklyItem.Period.StartDate).format('YYYY/MM/DD'))]).then(function (result) {


            $scope.contractOperationData = result[0].data.d.results;
            $scope.priviousDetails = result[1].data.d;

            $scope.weeklyItem.Items.map(function (itm) {
                itm.doneWork = itm.Constructed;
                var sum = 0;
                var result = $scope.priviousDetails.filter(function (f) {
                    if (itm.Measurement)
                        return (f.ExecutiveOperationId == itm.OperationId && f.SubExecutiveOperationId == itm.SubOperationId && f.Measure == itm.Measurement)
                    else
                        return (f.ExecutiveOperationId == itm.OperationId && f.SubExecutiveOperationId == itm.SubOperationId)
                });
                for (var i = 0; i < result.length; i++) {
                    sum += result[i].DoneWork;
                }
                itm.Constructed = sum;
                itm.CheckValue = getTotalWorkOperate(itm.SubOperationId, itm.OperationId, itm.Measurement)
            })

        });

        $scope.loading = false;
        //   $scope.operationData = d.operationData;
    },
    function (d) {
        console.log(d);
    });


    function getTotalWorkOperate(SubOperation, Operation, Measurement) {
        var filtered = $scope.contractOperationData.filter(function (itm) {
            if (itm.Measurement)
                return (itm.SubOperation.Id == SubOperation && itm.Operation.Id == Operation && itm.Measurement == Measurement)
            else
                return (itm.SubOperation.Id == SubOperation && itm.Operation.Id == Operation)

        });


        return filtered[0].CheckValue;
    }


    
    $scope.approve = function () {
        $scope.loading = true;
        if (!$scope.comment)
            $scope.comment = '';
        service.approve($scope.weeklyItem.Id, $scope.comment, $scope.listname).then(function (d) {
            if (d.data.d == "") {
                alert('رسیدگی به ارزیابی با موفقیت انجام شد');
                $scope.loading = false;
                window.location.href = $scope.prvUrl;
            }
            else {
                console.log(d);
                alert(d.data.d);
                $scope.loading = false;
            }

        }, function (d) {

            console.log(d);
            alert(d);
            $scope.loading = false;
        })
    }
    $scope.reject = function () {
        $scope.loading = true;
        if ($scope.comment == '') {
            alert('وارد نمودن توضیحات الزامی است.');
            $scope.loading = false;
        }
        else {
            service.reject($scope.weeklyItem.Id, $scope.comment, $scope.listname).then(function (d) {
                if (d.data.d == "") {
                    alert('رسیدگی به ارزیابی با موفقیت انجام شد');

                    $scope.loading = false;
                    window.location.href = $scope.prvUrl;
                }
                else {
                    console.log(d);
                    alert(d.data.d);
                    $scope.loading = false;

                }
            }, function (d) {

                console.log(d.data.d);

            })
        }
    }

    $scope.close = function () {
        window.location.href = $scope.prvUrl;
    }

}






)






