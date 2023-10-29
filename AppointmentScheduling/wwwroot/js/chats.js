// chats.js

// Get a reference to the chat icon element by its ID
const chatIcon = document.getElementById("chat-icon");

// Add a click event listener to the chat icon
chatIcon.addEventListener("click", function () {
    // Redirect the user to the chat page (replace "chat.html" with your actual chat page file name)
    window.location.href = "chat.html"; // Change "chat.html" to the actual URL of your chat page.
});


(function () {
    $(document).ready(function () {
        $(".more").on("click", function (e) {
            e.preventDefault();
        });
        $(".list li a").on("click", function (e) {
            e.preventDefault();
        });
        $(".change-tool a").on("click", function (e) {
            e.preventDefault();
        });
        $(".download").on("click", function (e) {
            e.preventDefault();
        });
    });
})();