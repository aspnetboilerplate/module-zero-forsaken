(function () {
    var controllerId = 'app.views.questions.index';
    angular.module('app').controller(controllerId, [
        'abp.services.app.question',
        function (questionService) {
            var vm = this;

            vm.questions = [];
            vm.totalQuestionCount = [];

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
        }
    ]);
})();