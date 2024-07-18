const chatHub = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/chatHub")
    .build();


//chatHub.on("displayChatMessages", (receiverId, receiverName, messages) => {
//    console.log("updateTheDom", receiverId, receiverName);
//    const chatMessagesContainer = document.getElementById("chatMessagesContainer");
//    document.getElementById("receiverName").innerText = receiverName;
//    console.log(messages);
//});

//document.getElementById("messageForm").onsubmit = (event) => {
//    event.preventDefault();
//    const formData = new FormData(this);

//    console.log(formData);
//};



chatHub
    .start()
    .then(
        () => {
            console.log("connected");
        },
        () => {
            console.log("failed to connect");
        }
    );