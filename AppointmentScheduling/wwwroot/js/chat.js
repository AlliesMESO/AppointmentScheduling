// Initialize SignalR connection
const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .configureLogging(SignalR.LogLevel.Information)
    .build();

hubConnection.on("ReceiveMessage", (user, message) => {
    // Handle the received message here
    console.log(`${user}: ${message}`);

    // Implement logic to display the message in your UI
    // Update the UI to show the actual username
    // Replace "USER_DISPLAY_ELEMENT" with the actual element where you want to display the username
    document.querySelector('.firstname').innerText = user;

    // Update the UI to show the received message
    // Replace "MESSAGE_DISPLAY_ELEMENT" with the actual element where you want to display the message
    const messageContainer = document.querySelector('.message-container');
    const messageElement = document.createElement('div');
    messageElement.innerText = `${user}: ${message}`;
    messageContainer.appendChild(messageElement);
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

// Attach the sendMessage function to the click event of the "send" button
document.querySelector('.send').addEventListener('click', function () { 
        // Get the user and message from your UI elements
    const user = "user"; // Replace with the actual logic to get the user
    const message = document.querySelector('.textarea-container textarea').value;

    // Call the sendMessageToHub function with user and message
    // Call your sendMessage function here
    sendMessageToHub(user, message);
});
  