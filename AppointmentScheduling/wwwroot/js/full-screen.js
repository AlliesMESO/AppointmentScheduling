function openFullscreen() {
    const elem = document.documentElement; // the root element of your page

    if (elem.requestFullscreen) {
        elem.requestFullscreen();
    } else if (elem.mozRequestFullScreen) { /* Firefox */
        elem.mozRequestFullScreen();
    } else if (elem.webkitRequestFullscreen) { /* Chrome, Safari, and Opera */
        elem.webkitRequestFullscreen();
    } else if (elem.msRequestFullscreen) { /* IE/Edge */
        elem.msRequestFullscreen();
    }

}

// Call the openFullscreen function when a user clicks the "Go Fullscreen" button
document.getElementById('fullscreenButton').addEventListener('click', openFullscreen);