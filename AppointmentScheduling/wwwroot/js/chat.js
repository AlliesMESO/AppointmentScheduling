
const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .configureLogging(SignalR.LogLevel.Information)
    .build();

hubconnection.on("ReceiveMessage", (user, message) => {
    //Handle the received message here
    console.log('${user}: ${message}');
});

hubConnection.start().then(() => {
    //Connection to the hub is established
    console.log("SignalR hub connection established.");
}).catch(err => {
    console.error(err.tostring());
});

//To send messages to the hub,we used a function like this one below:
function sendMessage(user, message) {
    hubConnection.invoke("SendMessage", user, message).catch(err => {
        console.error(err.toString());
    });
}