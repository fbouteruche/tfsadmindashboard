$(function () {
    if ($('#TpcList').val() != '') {
        var urlIdentityService = $('#identityservice').data('action');
        var urlIdentityService = urlIdentityService + '/' + $('#TpcList').val();
        $('#identityservice').load(urlIdentityService);
        var urlProjects = $('#projects').data('action');
        var urlProjects = urlProjects + '/' + $('#TpcList').val();
        $('#projects').load(urlProjects);
    }

    $('#TpcList').change(function () {
        $('#identityservice').html("");
        $('#projects').html("");
        if ($('#TpcList').val() != '') {
            var urlIdentityService = $('#identityservice').data('action');
            var urlIdentityService = urlIdentityService + '/' + $('#TpcList').val();
            $('#identityservice').load(urlIdentityService);
            var urlProjects = $('#projects').data('action');
            var urlProjects = urlProjects + '/' + $('#TpcList').val();
            $('#projects').load(urlProjects);
        }
    });
});
