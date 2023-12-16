// Wait for the DOM content to be fully loaded
document.addEventListener('DOMContentLoaded', function () {
    // Initialize SignalR connection
    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub")
        .configureLogging(SignalR.LogLevel.Information)
        .build();

    // Handle regular messages
    hubConnection.on("ReceiveMessage", (user, message) => {
        // Handle the received message here
        console.log(`${user}: ${message}`);

        // Update the UI to show the actual username
        // Replace "USER_DISPLAY_ELEMENT" with the actual element where you want to display the username
        document.querySelector('.firstname').innerText = user;

        // Update the UI to show the received message in the description part
        // Replace "MESSAGE_DISPLAY_ELEMENT" with the actual element where you want to display the message
        const messageContainer = document.querySelector('#allMessages');
        const messageElement = document.createElement('li');
        messageElement.innerHTML = `<strong>${user}: </strong>${message}`;
        messageContainer.appendChild(messageElement);

        // Update the UI to show the recent message in the description part
        const descriptionElement = document.querySelector('.description');
        descriptionElement.innerText = `<strong>${user}: </strong>${message}`;
    });

    // Handle email messages
    hubConnection.on("ReceivedEmailMessage", (user, message) => {
        // Handle the received email message here
        console.log(`${user}: ${message}`);

        // Update the UI to show the received email message
        const messageContainer = document.querySelector('#allMessages');
        const messageElement = document.createElement('li');
        messageElement.innerHTML = `<strong>${user}: </strong>${message}`;
        messageContainer.appendChild(messageElement);

        // Update the UI to show the recent email message in the description part
        const descriptionElement = document.querySelector('.description');
        descriptionElement.innerHTML = `<strong>${user}: </strong>${message}`;
    });

    hubConnection.start().then(() => {
        // Connection to the hub is established
        console.log("SignalR hub connection established.");
    }).catch(err => {
        console.error(err.toString());
    });

    // To send messages to the hub, we use a function like this one below:
    function sendMessage(user, message) {
        // Send the message to the hub
        hubConnection.invoke("SendMessage", user, message).catch(err => {
            console.error(err.toString());
        });

        // Clear the textarea after sending the message
        document.querySelector('.textarea-container textarea').value = "";
    }

    // To send email messages to the hub
    function sendEmailMessage(user, message) {
        // Send the email message to the hub
        hubConnection.invoke("SendEmailMessage", user, message).catch(err => {
            console.error(err.toString());
        });

        // Clear the textarea after sending the message
        document.querySelector('.textarea-container textarea').value = "";
    }

    // Attach the sendMessage function to the click event of the "send" button
    document.querySelector('.send').addEventListener('click', function () {
        console.log('Button clicked');
        // Get the user and message from your UI elements
        const user = "user"; // Replace with the actual logic to get the user
        const message = document.querySelector('.textarea-container textarea').value;

        // Call the sendMessage function with user and message
        sendMessage(user, message);
    });

    // Attach the sendEmailMessage function to the click event of the "send email" button for email messages
    document.querySelector('.send-email').addEventListener('click', function () {
        console.log('Email Button clicked');
        // Get the user and email message from your UI elements
        const user = "user"; // Replace with the actual logic to get the user
        const emailMessage = document.querySelector('.email-textarea-container textarea').value;

        // Call the sendEmailMessage function with user and email message
        sendEmailMessage(user, emailMessage);
    });
});
