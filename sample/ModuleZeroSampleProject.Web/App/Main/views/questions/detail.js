(function () {
    var controllerId = 'app.views.questions.detail';
    angular.module('app').controller(controllerId, [
        '$state', 'abp.services.app.question',
        function ($state, questionService) {
            var vm = this;

            vm.question = null;

            questionService.getQuestion({
                id: $state.params.id,
                incrementViewCount: true
            }).success(function (data) {
                vm.question = data.question;
            });
        }
    ]);
})();