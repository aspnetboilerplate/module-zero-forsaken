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
            $urlRouterProvider.otherwise('/');
            $stateProvider
                .state('questions', {
                    url: '/',
                    templateUrl: '/App/Main/views/questions/index.cshtml',
                    menu: 'Questions' //Matches to name of 'Questions' menu in ModuleZeroSampleProjectNavigationProvider
                })
                .state('people', {
                    url: '/people',
                    templateUrl: '/App/Main/views/people/index.cshtml',
                    menu: 'People' //Matches to name of 'People' menu in ModuleZeroSampleProjectNavigationProvider
                });
        }
    ]);
})();