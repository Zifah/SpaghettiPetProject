angular
    .module('YorubaIntonationApp')
    .factory('YorubaIntonationService', function ($http) {
        var service = {
            getGreeting: getGreeting,
            processParagraph: processParagraph,
            notifyCompleted: notifyCompleted,
            getRecentlyProcessed: getRecentlyProcessed
        };

        function getGreeting() {
            return "Hello world";
        }

        function processParagraph(word) {
            return $http.post('api/yoruba/paragraph/derivatives', { content: word });
        }

        function notifyCompleted(id, finalForm) {
            return $http.put('api/yoruba/paragraph/derivatives/?noblockui', { id: id, output: finalForm });
        }

        function getRecentlyProcessed() {
            return $http.get('/api/yoruba/paragraph/derivatives');
        }

        return service;
    })