function showUserDetails(userId) {
    console.log('showUserDetails called with userId:', userId);
    
    $.get(`/Users/GetUserDetails/${userId}`, function(data) {
        console.log('AJAX call successful, received data:', data);
        $('#modalContainer').html(data);
        console.log('Modal content injected into modalContainer');
        $('#userDetailsModal').modal('show');
        console.log('Bootstrap modal show triggered');
    }).fail(function(error) {
        console.error('AJAX call failed:', error);
    });
} 