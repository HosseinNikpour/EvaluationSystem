var app = angular.module('App', ['persianDatePicker','angularjs-dropdown-multiselect']);
app.controller('adminCtrl', function ($scope, $q, evalSrv) {
    $scope.loading = true;

    $scope.secectTrnslationText = { dynamicButtonTextSuffix: ' تا', searchPlaceholder: 'جستجو ', buttonDefaultText: 'انتخاب',checkAll:'انتخاب همه' ,uncheckAll:'عدم انتخاب'};

    $scope.participantsSettings = {
        enableSearch: true, scrollable: true, showCheckAll: true, showUncheckAll: true,
        idProp: 'Id', template: '<span style="margin-right:5px; font-size:11px">{{option.Title}}</span>'
    };


    $scope.selectedContracts = [];
    $scope.selectedCompanies = [];
   $q.all([evalSrv.getEvaluationTypes(),evalSrv.getPeriods()]).then(function (d) {
       $scope.evaluationTypes = d[0].data.d.results;
       $scope.periods = d[1].data.d.results;
        $scope.evalType = $scope.evaluationTypes[0];
        if (!$scope.evalType.IsCompany) {
            setContracts();
            $scope.isCompany = false;
        }
        else {
            setCompanies();
            $scope.isCompany = true;
        }
        $scope.loading = false;
        
    })

   

    $scope.changeEvalType = function ()
    {

        if (!$scope.evalType.IsCompany) {
            setContracts();
            $scope.isCompany = false;
        }
        else {
            setCompanies();
            $scope.isCompany = true;
        }
        
    }


    function setContracts()
    {
        if ($scope.evalType) {
            evalSrv.getContracts($scope.evalType).then(function (d) {
                $scope.contracts = [];
                $scope.selectedContracts = [];
                $scope.contracts = d.data.d.results;

            })
        }

    }

    function setCompanies()
    {
        if ($scope.evalType) {
            evalSrv.getCompanies($scope.evalType).then(function (d) {
                $scope.companies = [];
                $scope.selectedCompanies = [];
                $scope.companies = d.data.d.results;

            })
        }

    }
    $scope.createItems = function ()
    {
        $scope.loading = true;
        $scope.contractIds = [];
        $scope.companyIds = [];
       if(!$scope.isCompany)
        for (var i = 0; i < $scope.selectedContracts.length; i++) {
            $scope.contractIds.push($scope.selectedContracts[i].id);
        }
        else
           for (var i = 0; i < $scope.selectedCompanies.length; i++) {
               $scope.companyIds.push($scope.selectedCompanies[i].id);
           }
        var fMonth='';
        var date= moment($scope.date);
        //var month = date.format('jMM');
        
        //switch (month)
        //{
        //    case ('01'):
        //        fMonth = 'فروردین';
        //        break;
        //    case ('02'):
        //        fMonth = 'اردیبهشت';
        //        break;
        //    case ('03'):
        //        fMonth ='خرداد';
        //        break;
        //    case ('04'):
        //        fMonth = 'تیر';
        //        break;
        //    case ('05'):
        //        fMonth = 'مرداد';
        //        break;
        //    case ('06'):
        //        fMonth = 'شهریور';
        //        break;
        //    case ('07'):
        //        fMonth = 'مهر';
        //        break;
        //    case ('08'):
        //        fMonth = 'آبان';
        //        break;
        //    case ('09'):
        //        fMonth = 'آذر';
        //        break;
        //    case ('10'):
        //        fMonth = 'دی';
        //        break;
        //    case ('11'):
        //        fMonth = 'بهمن';
        //        break;
        //    case ('12'):
        //        fMonth = 'اسفند';
        //        break;


        //}
        //var fDate = fMonth + " " + date.format('jYY');

        evalSrv.createEvaluationContracts($scope.evalType.Title, $scope.isCompany ? $scope.companyIds : $scope.contractIds, $scope.period.Id, $scope.isCompany).then(
            function (d) {
               
                if (d.data.d == "") {

                    $scope.loading = false;
                    alert("اطلاعات با موفقیت ذخیره شد");
                }
                else {
                    $scope.loading = false;
                    alert(d.data.d);
                }
            },
              function (d) {
                  alert('خطا در ذخیره سازی اطلاعات' + d);
                  $scope.loading = false;
              });
              
    }


    //$scope.$watch('selected', function (nowSelected) {
    //    // reset to nothing, could use `splice` to preserve non-angular references
    //    $scope.selectedContracts = [];

    //    if (!nowSelected) {
    //        // sometimes selected is null or undefined
    //        return;
    //    }

    //    // here's the magic
    //    angular.forEach(nowSelected, function (val) {
    //        $scope.selectedContracts.push(val.Id.toString());
    //    });
    //});
});
