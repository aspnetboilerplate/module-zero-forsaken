(function () {
    var controllerId = 'app.views.questions.index';
    angular.module('app').controller(controllerId, [
        'abp.services.app.question', '$modal',
        function (questionService, $modal) {
            var vm = this;

            vm.permissions = {
                canCreateQuestions: abp.auth.hasPermission("CanCreateQuestions")
            };

            vm.questions = [];
            vm.totalQuestionCount = [];

            vm.loadQuestions = function () {
                abp.ui.setBusy(
                    null,
                    questionService.getQuestions({
                        maxResultCount: 10,
                        skipCount: 0
                    }).success(function (data) {
                        vm.questions = data.items;
                        vm.totalQuestionCount = data.totalCount;
                    })
                );
            };

            vm.showNewQuestionDialog = function () {
                var modalInstance = $modal.open({
                    templateUrl: abp.appPath + 'App/Main/views/questions/createDialog.cshtml',
                    controller: 'app.views.questions.createDialog as vm',
                    size: 'md'
                });

                modalInstance.result.then(function () {
                    vm.loadQuestions();
                });
            };

            vm.loadQuestions();
        }
    ]);
})();