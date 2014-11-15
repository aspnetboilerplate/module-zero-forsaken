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
            vm.totalQuestionCount = 0;
            vm.sorting = 'CreationTime DESC';

            vm.sortingDirections = ['CreationTime DESC', 'VoteCount DESC', 'ViewCount DESC', 'AnswerCount DESC'];

            vm.loadQuestions = function() {
                abp.ui.setBusy(
                    null,
                    questionService.getQuestions({
                        maxResultCount: 10,
                        skipCount: 0,
                        sorting: vm.sorting
                    }).success(function(data) {
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

            vm.sort = function (sortingDirection) {
                vm.sorting = sortingDirection;
                vm.loadQuestions();
            };

            vm.loadQuestions();
        }
    ]);
})();