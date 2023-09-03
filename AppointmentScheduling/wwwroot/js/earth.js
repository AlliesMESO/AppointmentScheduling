function initEarth() {
    var earthDiv = document.getElementById('earth_div'); // HTML element to hold the Earth
    var options = {
        database: 'https://earth-api-utility-library.googlecode.com/svn/trunk/extensions/examples/advanced/ge-kml/kml/seeklogo.kmz',
    };
    google.earth.createInstance(earthDiv, initCallback, failureCallback, options);
}

function initCallback(earth) {
    // Customize the Earth view and add features
    earth.getWindow().setVisibility(true);
    // Add placemarks, layers, and other features here
}

function failureCallback(error) {
    console.error('Failed to initialize Google Earth:', error);
}
