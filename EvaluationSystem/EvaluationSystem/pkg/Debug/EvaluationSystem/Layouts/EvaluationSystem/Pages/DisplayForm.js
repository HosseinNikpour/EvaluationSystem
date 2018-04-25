var app = angular.module('appDisplay', []);
app.controller('ctrlDisplay', function ($scope,$q, evalSrv) {
    var url = window.location.href;
    $scope.listname = getUrlVars()['ListName'];
    $scope.requestId = getUrlVars()['ID'];
    $scope.listFaName = listFaName;
    $scope.prvUrl = '/Lists/' + $scope.listname + '/AllItems.aspx';
    if ($scope.listname == "EvaluationSupplier" || $scope.listname == "EvalutaionProjectManager" || $scope.listname == "EvalutaionTechnicalInspector")
        $scope.isCompany = true;
    else $scope.isCompany = false;

    $scope.loading = true;
    $scope.comment = '';
    $q.all([evalSrv.getEvaluationContract($scope.requestId, $scope.listname), evalSrv.canApprove($scope.requestId,$scope.listname,$scope.isCompany)]).then(function (result) {

        $scope.evalContract = result[0].data.d.evalContract;
        $scope.histories = result[0].data.d.histories;
        $scope.canApprove = result[1].data.d;
     

        $scope.evalContract.details.sort(function (a, b) {
            var x = parseInt(a['order'].split("-")[0]),
                y = parseInt(a['order'].split("-")[1]),
                z = parseInt(b['order'].split("-")[0]),
                w = parseInt(b['order'].split("-")[1])
                if (x < z)
                    return -1;
                else if (x > z)
                    return 1;
                else {
                    if (y < w)
                        return -1;
                    else
                        return 1;
                }
        })

        for (var i = 0; i < $scope.evalContract.details.length; i++) {
            $scope.evalContract.details[i].order2 = $scope.evalContract.details[i].order.toString().replace('.', '-');


            var str = $scope.evalContract.details[i].order2;
            $scope.evalContract.details[i].rowIndex = parseInt(str.substring(str.indexOf('-') + 1, str.length));

            $scope.evalContract.details[i].groupIndex = parseInt(str.substring(0, str.indexOf('-')));

            $scope.evalContract.details[i].order2 = $scope.evalContract.details[i].rowIndex.toString() + '-' + $scope.evalContract.details[i].groupIndex.toString();


            //    //$scope.evalContract.details[i].rowIndex = str.substring(str.indexOf('-') + 1, str.length);
            //   // $scope.evalContract.details[i].org_weight = $scope.evalContract.details[i].weight;
            //}
        }
            for (var i = 0; i < $scope.histories.length; i++)
            {
                $scope.histories[i].HistoryDate = moment($scope.histories[i].HistoryDate).format('jYYYY/jMM/jDD');

            }

            groupIndexes();
            $scope.loading = false;
        }
        , function (d) {
            alert(d.data);
        });


    $scope.approve = function () {
        $scope.loading = true;
        evalSrv.approve($scope.evalContract.id, $scope.listname,$scope.comment,$scope.isCompany).then(function (d) {
            if (d.data.d == "") {
                alert('رسیدگی به ارزیابی با موفقیت انجام شد');
                $scope.loading = false;
                window.location.href = $scope.prvUrl;
            }
            else {
                console.log(d);
                alert(d);
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
            evalSrv.reject($scope.evalContract.id, $scope.listname, $scope.comment,$scope.isCompany).then(function (d) {
                if (d.data.d == "") {
                    alert('رسیدگی به ارزیابی با موفقیت انجام شد');

                    $scope.loading = false;
                    window.location.href = $scope.prvUrl;
                }
                else {
                    console.log(d);
                    alert(d);
                    $scope.loading = false;

                }
            }, function (d) {

                console.log(d.data.d);

            })
        }
    }

    $scope.close = function ()
    {
        window.location.href = $scope.prvUrl;
    }
    $scope.sumWeight = function (groupIndex) {
        var sum = 0;


        for (var i = 0; i < $scope.groupIndexes[groupIndex].indexes.length; i++) {
            var itm = $scope.groupIndexes[groupIndex].indexes[i];
            sum += itm.weight;
        }
        //for (var i = 0; i < $scope.evalContract.details.length; i++)
        //{
        //    var itm=$scope.evalContract.details[i];
        //    if (itm.groupIndex == groupIndex)
        //        sum += itm.weight;
        //}
        return sum;
    }
    $scope.totalScore = function (groupIndex) {
        var sum = 0;

        for (var i = 0; i < $scope.groupIndexes[groupIndex].indexes.length; i++) {
            var itm = $scope.groupIndexes[groupIndex].indexes[i];
            sum += itm.weight * itm.score;
        }
        //for (var i = 0; i < $scope.evalContract.details.length; i++) {
        //    var itm = $scope.evalContract.details[i];
        //    if (itm.groupIndex == groupIndex)
        //        sum += itm.weight*itm.score;
        //}
        return sum / 100;
    }
    function groupIndexes() {
        var groupData = [];
        var myArray = angular.copy($scope.evalContract.details);
        var groups = [];
        for (var i = 0; i < myArray.length; i++) {
            if (groups.indexOf(myArray[i].groupIndex) == -1) {
                groups.push(myArray[i].groupIndex);
                groupData.push({ groupIndex: myArray[i].groupIndex, groupName: myArray[i].criterion, indexes: myArray.filter(function (a) { return a.groupIndex == myArray[i].groupIndex }) })
            }
        }

        console.log(groupData);

        $scope.groupIndexes = groupData;
    }

    $scope.sumCol = function (key) {

        var sum = 0;
       
        if ($scope.groupIndexes && $scope.groupIndexes.length > 0)
            for (var i = 0; i < $scope.groupIndexes.length; i++)
                for (var j = 0; j < $scope.groupIndexes[i].indexes.length; j++) {
                    sum += $scope.groupIndexes[i].indexes[j][key];
                }


        //for (var i = 0; i < $scope.evalContract.details.length; i++)
        //{
        //    var itm=$scope.evalContract.details[i];
        //    if (itm.groupIndex == groupIndex)
        //        sum += itm.weight;
        //}
        return sum;
    }

    $scope.sumTotalCol = function () {
        var sum = 0;
        if ($scope.groupIndexes && $scope.groupIndexes.length > 0)
        for (var i = 0; i < $scope.groupIndexes.length; i++)
            for (var j = 0; j < $scope.groupIndexes[i].indexes.length; j++) {
                sum += $scope.groupIndexes[i].indexes[j].score * $scope.groupIndexes[i].indexes[j].weight;
            }
        return sum / 100;
    }
    
})