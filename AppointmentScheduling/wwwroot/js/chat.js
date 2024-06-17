document.addEventListener('DOMContentLoaded', function () {
    // Initialize SignalR connection
    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    // Listen for the "MessageSent" event
    hubConnection.on("MessageSent", (user, message) => {
        // Handle the received message here
        console.log(`${user}: ${message}`);

        // Update the UI to show the received message
        const messageContainer = document.querySelector('#allMessages');
        const messageElement = document.createElement('li');
        messageElement.textContent = message;
        messageContainer.appendChild(messageElement);

        // Update the UI to show the recent message in the description part
        const descriptionElement = document.querySelector('.description');
        descriptionElement.innerText = message;
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
    function sendMessage(user, message, recipientId) {
        // Send the message to the hub
        hubConnection.invoke("SendMessage", user, message, recipientId).catch(err => {
            console.error(err.toString());
        });

        // Clear the textarea after sending the message
        document.querySelector('.textarea-container textarea').value = "";
    }

    // To send email messages to the hub
    function sendEmailMessage(user, message, recipientId) {
        // Send the email message to the hub

        hubConnection.invoke("SendEmailMessage", user, message, recipientId).catch(err => {
        //    debugger
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

        // Get the recipient user's UserId from your UI (you need to implement this part)
        const recipientId = getRecipientUserId();

        // Get the selected message type
        const regularMessageRadioButton = document.getElementById('regularMessage');
        const emailMessageRadioButton = document.getElementById('emailMessage');

        if (regularMessageRadioButton.checked) {
            // Call the sendMessage function for regular message
            sendMessage(user, message, recipientId);
        } else if (emailMessageRadioButton.checked) {
            // Call the sendEmailMessage function for email message
            sendEmailMessage(user, message, recipientId);
        } else {
            console.error("No message type selected.");
        }
    });

    // Add click event listeners to inbox items
    const inboxItems = document.querySelectorAll('.inbox-item');

    Array.from(inboxItems).forEach(function (inboxItem) {
        inboxItem.addEventListener('click', function () {
            // Remove 'active' class from all inbox items
            Array.from(inboxItems).forEach(function (item) {
                item.classList.remove('active');
            });

            // Add 'active' class to the clicked inbox item
            inboxItem.classList.add('active');

            // Retrieve data-recipient-id attribute and update UI
            const recipientId = inboxItem.getAttribute('data-recipient-id');
            console.log('Recipient ID:', recipientId);

                        // Update the recipientId in the UI

            updateRecipientId(recipientId);
        });
    });

    // Function to get the recipient user's UserId
    function getRecipientUserId() {
        const firstInboxItem = document.querySelector('.inbox-item');

        if (firstInboxItem) {
            const recipientId = firstInboxItem.getAttribute('data-recipient-id');

            if (recipientId !== null) {
                console.log("Recipient ID:", recipientId);
                return recipientId;
            }
        } else {
            console.error("No inbox item found.");
            return null;
        }
    }

    function updateRecipientId(recipientId)
    {
        console.log("Update Recipient ID:", recipientId);

        document.querySelector('.recipient-id').innerText = recipientId;
    }

    // Call the function to set the active inbox item and retrieve the recipient ID
    const recipientId = getRecipientUserId();
    if (recipientId !== null) {
        console.log("Recipient ID:", recipientId);
    } else {
        console.error("No active inbox item selected.")
    }
});
