angular
    .module('YorubaIntonationApp')
    .factory('YorubaFeedbackService', function ($http) {
        var service = {
            submitFeedback: submit
        };

        function submit(feedback) {
            return $http.post('api/yorubafeedback', feedback);
        }

        return service;
    })