// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    const fullscreenButton = document.getElementById('fullscreenButton');
    const icon = fullscreenButton.querySelector('.glow');

    fullscreenButton.addEventListener('click', function () {
        if (!document.fullscreenElement) {
            document.documentElement.requestFullscreen().then(() => {
                fullscreenButton.classList.add('fullscreen-on');
                icon.style.color = 'green';
            }).catch(err => {
                console.error(`Error attempting to enable full-screen mode: ${err.message}`);
            });
        } else {
            document.exitFullscreen().then(() => {
                fullscreenButton.classList.remove('fullscreen-on');
            }).catch(err => {
                console.error(`Error attempting to exit full-screen mode: ${err.message}`);
            });
        }
    });
});

$(document).ready(function () {
    // Detect full-screen change
    $(document).on("fullscreenchange", function () {
        var fullscreenIcon = $("#fullscreenButton i");
        if (document.fullscreenElement) {
            // User is in full-screen mode, change the icon
            fullscreenIcon.removeClass("fa-user-astronaut").addClass("fa-tv-retro");
        } else {
            // User is not in full-screen mode, change the icon back
            fullscreenIcon.removeClass("fa-tv-retro").addClass("fa-user-astronaut");
        }
    });

    // Toggle full-screen mode when the button is clicked
    $("#fullscreenButton").click(function () {
        if (!document.fullscreenElement) {
            document.documentElement.requestFullscreen();
        } else {
            document.exitFullscreen();
        }
    });
});