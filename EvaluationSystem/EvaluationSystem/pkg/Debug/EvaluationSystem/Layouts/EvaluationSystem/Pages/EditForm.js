var app = angular.module('appEdit', []);
app.controller('ctrlEdit', function ($scope,evalSrv) {
    var url = window.location.href;
    $scope.loading = false;

    //todo:Go to serverSide
     
    //if (hasPermision!="")
    //{
    //    alert("شما دسترسی لازم برای باز کردن این فرم را ندارید");
    //    $scope.loading = true;
    //    return;
    //}
    $scope.listname = getUrlVars()['ListName'];
    $scope.listFaName = listFaName;
    if ($scope.listname == "EvaluationSupplier" || $scope.listname == "EvalutaionProjectManager" || $scope.listname == "EvalutaionTechnicalInspector")
        $scope.isCompany = true;
    else $scope.isCompany = false;
  
    $scope.requestId = getUrlVars()['ID'];
    $scope.prvUrl = '/Lists/' + $scope.listname + '/AllItems.aspx';

    evalSrv.getEvaluationContract($scope.requestId, $scope.listname).then(
        function (d) {
            $scope.evalContract =d.data.d.evalContract;
            $scope.histories =d.data.d.histories;
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
            }
            //    //$scope.evalContract.details[i].rowIndex = str.substring(str.indexOf('-') + 1, str.length);
            //   // $scope.evalContract.details[i].org_weight = $scope.evalContract.details[i].weight;
            //}
            for (var i = 0; i < $scope.histories.length; i++) {
                $scope.histories[i].HistoryDate = moment($scope.histories[i].HistoryDate).format('jYYYY/jMM/jDD');

            }
            groupIndexes();
            $scope.loading = false;

        }, function (d) {
            console.log(d);
        });


   
    $scope.save = function (isTemp) {
        $scope.loading = true;
        var items = [];
        for (var i = 0; i < $scope.groupIndexes.length; i++)
            for (var j = 0; j < $scope.groupIndexes[i].indexes.length; j++) {
                items.push($scope.groupIndexes[i].indexes[j]);
            }


        //var errorItems = items.filter(function (itm) {
        //    return (itm.isRelated == false)
        //});

        var fivemultiItems = items.filter(function (itm) {
            return (itm.score > 100 || itm.score < 0 || itm.score % 5 != 0)
        });

        //if (!isTemp && errorItems.length > 0) {
        //    alert('لطفا برای همه ستون ها وزن را وارد نمایید و یا تیک موضوعیت ندارد را بزنید');
        //    $scope.loading = false;
        //}
        //else
        if (fivemultiItems.length > 0) {
            alert('امتیاز هر شاخص باید عددی مضرب پنج در بازه بین 0 تا 100 باشد');
            $scope.loading = false;
        }
        else {
            $scope.evalContract.details = items;
            $scope.evalContract.totalScore = calcTotalScore();
            evalSrv.saveEvaluation($scope.evalContract, isTemp, $scope.listname, $scope.isCompany).then(function (d) {
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
            }, function (d) {

                console.log(d.data.d);
            })

        }
       
    }

    $scope.close = function ()
    {
        window.location.href = $scope.prvUrl;
    }

    $scope.notRelated = function (item)
    {

        if (item.isRelated)
        {
            item.weight = 0;
            item.score = 0;
        }
        var sumWeight = 0;
        for (var i = 0; i < $scope.groupIndexes.length; i++)
            for (var j = 0; j < $scope.groupIndexes[i].indexes.length; j++) {
                if ($scope.groupIndexes[i].indexes[j].isRelated == false)
                    sumWeight += $scope.groupIndexes[i].indexes[j].org_weight;


            }
    
        for (var i = 0; i < $scope.groupIndexes.length; i++)
            for (var j = 0; j < $scope.groupIndexes[i].indexes.length; j++) {

                if ($scope.groupIndexes[i].indexes[j].isRelated == false)
                    $scope.groupIndexes[i].indexes[j].weight = $scope.groupIndexes[i].indexes[j].org_weight / sumWeight * 100;

            }
       
        }
    

    $scope.sumWeight = function (groupIndex)
    {
        var sum=0;


        for (var i = 0; i < $scope.groupIndexes[groupIndex].indexes.length; i++)
        {
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
        return sum/100;
    }


    function calcTotalScore()
    {
        var sum = 0;
        for (var i = 0; i < $scope.evalContract.details.length; i++) {
            var itm = $scope.evalContract.details[i];
            
                sum += itm.weight * itm.score;
        }
        return sum/100;
    }

    function groupIndexes()
    {
        var groupData = [];
        var myArray = angular.copy($scope.evalContract.details);
        var groups = [];
        for (var i = 0; i < myArray.length; i++) {
            if (groups.indexOf(myArray[i].groupIndex)==-1) {
                groups.push(myArray[i].groupIndex);
                groupData.push({ groupIndex: myArray[i].groupIndex, groupName: myArray[i].criterion, indexes: myArray.filter(function (a) { return a.groupIndex == myArray[i].groupIndex }) })
            }
        }

       // console.log(groupData);

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
    //function groupIndexes() {

    //    var myArray = angular.copy($scope.evalContract.details);
    //    var groups = {};
    //    for (var i = 0; i < myArray.length; i++) {
    //        var groupName = myArray[i].groupIndex;
    //        if (!groups[groupName]) {
    //            groups[groupName] = [];
    //        }
    //        groups[groupName].push( {criterion:myArray[i].criterion ,  indexes:myArray[i]} );
    //    }
    //    myArray = [];
    //    for (var groupName in groups) {
    //        myArray.push({ group: groupName,criterion:groups[groupName].ceriterion,indexes: groups[groupName].indexes });
    //    }
    //    $scope.groupedIndexes = myArray;
    //}


    //function groupIndexes() {
    //    var myArray = angular.copy($scope.indexes);
    //    var groups = {};
    //    for (var i = 0; i < myArray.length; i++) {
    //        var groupName = myArray[i].criterionId;
    //        if (!groups[groupName]) {
    //            groups[groupName] = [];
    //        }
    //        groups[groupName].push({ id: myArray[i].id, title: myArray[i].title, criterion: myArray[i].criterion });
    //    }
    //    myArray = [];
    //    for (var groupName in groups) {
    //        myArray.push({ group: groupName, indexes: groups[groupName] });
    //    }
    //    $scope.groupedIndexes = myArray;
    //}
})