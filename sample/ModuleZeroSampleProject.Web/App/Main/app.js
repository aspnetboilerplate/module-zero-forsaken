(function () {
    'use strict';

    var app = angular.module('app', [
        'ngAnimate',
        'ngSanitize',
        'ngMaterial',

        'ui.router',
        'ui.bootstrap',
        'ui.jq',

        'abp'
    ]);

    //Configuration for Angular UI routing.
    app.config([
        '$stateProvider', '$urlRouterProvider',
        function($stateProvider, $urlRouterProvider) {
            $urlRouterProvider.otherwise('/questions');
            $stateProvider
                .state('questions', {
                    url: '/questions',
                    templateUrl: abp.appPath + 'App/Main/views/questions/index.cshtml',
                    menu: 'Questions' //Matches to name of 'Questions' menu in ModuleZeroSampleProjectNavigationProvider
                })
                .state('questionDetail', {
                    url: '/questions/:id',
                    templateUrl: abp.appPath + 'App/Main/views/questions/detail.cshtml',
                    menu: 'Questions' //Matches to name of 'Questions' menu in ModuleZeroSampleProjectNavigationProvider
                })
                .state('people', {
                    url: '/people',
                    templateUrl: abp.appPath + 'App/Main/views/people/index.cshtml',
                    menu: 'People' //Matches to name of 'People' menu in ModuleZeroSampleProjectNavigationProvider
                });
        }
    ]);
})();