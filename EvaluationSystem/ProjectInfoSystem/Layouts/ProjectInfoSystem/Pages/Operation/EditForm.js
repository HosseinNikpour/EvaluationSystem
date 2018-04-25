
var app = angular.module('opApp', []);
app.controller('opCtrl', function ($scope, service, $q) {
    $scope.loading = true;
    $scope.prvUrl = '/Lists/WeeklyConstructions/AllItems.aspx';
    //service.getFormtitle().then(function (d) {
    //    $scope.Title = d.data;
    //    console.log(d);
    //});
    
    $scope.listname = "WeeklyConstructions";
    $scope.requestId = getUrlVars()['ID'];
    
   
    $q.all([service.getWeeklyItem($scope.requestId),service.getWeeklyItemDetails($scope.requestId), service.canApprove($scope.requestId, 'WeeklyConstructions'), service.getHistories($scope.requestId, 'WeeklyConstructions')]).then(function (d) {
        $scope.weeklyItem = d[0].data.d.results[0];
        $scope.weeklyItem.Items = d[1].data.d.results;
        $scope.canApprove = d[2].data.d;
        $scope.histories = d[3].data.d.histories;
        for (var i = 0; i < $scope.histories.length; i++) {
            $scope.histories[i].HistoryDate = moment($scope.histories[i].HistoryDate).format('jYYYY/jMM/jDD');

        }
        $q.all([service.getOperationsOnFarmsFilterContract($scope.weeklyItem.ContractId),service.getWeeklyDetailPrevius( $scope.weeklyItem.ContractId, moment($scope.weeklyItem.Period.StartDate).format('YYYY/MM/DD'))]).then(function (result) {

       
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
                    itm.CheckValue=getTotalWorkOperate(itm.SubOperationId, itm.OperationId, itm.Measurement)
                })

            });
       

       
        $scope.loading = false;

    }, function (d){

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

   
    $scope.close = function () {
        window.location.href = $scope.prvUrl;
    }

    
    $scope.save = function (isTemp) {
        $scope.loading = true;
        $scope.saveItem = {};

        $scope.saveItem.Id = $scope.weeklyItem.Id,
        $scope.saveItem.Title = $scope.weeklyItem.Title,
        $scope.saveItem.Contract = $scope.weeklyItem.Contract.Title,
        $scope.saveItem.ContractId = $scope.weeklyItem.ContractId,
        $scope.saveItem.Period = $scope.weeklyItem.PeriodId,
        $scope.saveItem.Items = [];
        for (var i = 0; i < $scope.weeklyItem.Items.length ; i++) {
            if ($scope.weeklyItem.Items[i].hasError) {
                alert('لطفا موراد نشان داده را برطرف و مجددا ذخیره نمایید ');
                $scope.loading = false;
                return;
            }
        }
        $scope.saveItem.Items = $scope.weeklyItem.Items.map(function (itm) {
            var obj = {};
            obj.Id = itm.Id;
            obj.Title = itm.Title;
            obj.DoneWork = itm.doneWork;
            obj.ExecutiveOperation = itm.Operation.Title;
            obj.ExecutiveOperationId = itm.OperationId;
            obj.SubExecutiveOperation = itm.SubOperation.Title;
            obj.SubExecutiveOperationId = itm.SubOperationId;

            obj.Measure = itm.Measurement;


            return obj;
        });

        service.save($scope.saveItem, isTemp).then(function (d) {
            $scope.loading = true;
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
                     $scope.loading = false;
                 });

    }



    $scope.validateCell = function (item) {
        if (item.Constructed + item.doneWork > item.CheckValue) {
            item.hasError = true;
        }
        else { item.hasError = false; }
    }





    $scope.myVar = false;
    $scope.toggle = function () {
        $scope.myVar = !$scope.myVar;
    };



});
