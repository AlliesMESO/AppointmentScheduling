

console.log("project.js is being accessed.");
$(document).ready(function () {
    // Show the modal when the '+' button is clicked
        // Function to show the modal when the '+' button is clicked

    $('#uploadbutton').click(function () {  // selector here
        $('#uploadModal').modal('show');

        // Add this line for testing
    //    alert("Button clicked!");
    });

    // Handle the upload button click
    $('#uploadProfilePicture').click(function () {

        // Perform the file upload using AJAX or your preferred method
        // In this example, we're using the FormData API and AJAX

        var formData = new FormData($('#profilePictureForm')[0]);

        $.ajax({
            url: '/User/UploadProfilePicture', // Replace with the URL to your server-side upload handler
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (data) {


                                           // Handle the success response, e.g., update the user's profile picture
                                         // and close the modal
                if (data.success) {
                    console.log("Image URL: " + data.imageUrl);

                    // Update the user's profile picture on the page
                    $('#profilePictureImage').attr('src', data.imageUrl);


                    //++++++++++++Redirect to the Profile page-------------
                    // Show the success message
                    // Display a success message
                    //document.getElementById("uploadMessage").innerText = "Profile picture uploaded."; //uncomment
                    //$('#uploadSuccessMessage').show(); // Display the success message
                    //setTimeout(function () {

                    //    $('#uploadSuccessMessage').fadeOut('fast'); // Hide the message after a few seconds
                    //}, 3000); // Adjust the time as needed

                    //--------------Redirect to the Profile page++++++++++++++++


                    //0000000000000000Show a success message (you can customize this part)000000000000000

                    alert("Profile picture uploaded successfully!");


                    // Redirect to the Profile page after a brief delay
                    setTimeout(function () {
                        window.location.replace('/User/Profile'); // Replace with the correct URL if needed
                    }, 1000); // Adjust the delay time as needed
                }


                
            },
            error: function () {
                //0000000000000000000000000000 Handle error here (e.g., display an error message)0-
                alert("An error occurred during the upload.");
            }
        });
    });
    $('#uploadbutton').click(function () {
        console.log("Upload button clicked!");
        $('#uploadModal').modal('show');
    });
});