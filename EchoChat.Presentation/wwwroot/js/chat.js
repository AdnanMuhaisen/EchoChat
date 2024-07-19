﻿const chatHub = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/chatHub")
    .build();

const messageForm = document.getElementById("messageForm");
messageForm.addEventListener("submit", (event) => {
    console.log("Submitted");
    event.preventDefault();
    const formData = new FormData(messageForm);
    document.getElementById("messageInput").value = "";
    chatHub.send("SendMessageAsync", formData.get("chatId"), formData.get("receiverId"), formData.get("receiverName"), formData.get("message"));
});

window.onload = () => {
    const chatMessages = document.getElementById("chatMessages");
    chatMessages.scrollTop = chatMessages.scrollHeight;
}

chatHub.on("receiveMessage", (receiverName, messageText, sentAt) => {
    const chatMessages = document.getElementById("chatMessages");
    const messageToAppend = `
        <div class="w-100 bg-success rounded ms-1 mb-1" style="height:contain;--bs-bg-opacity: .5;">
            <span class="ps-1 w-100 d-block border-bottom" style="font-size:12px;">${receiverName}</span>
            <p class="ps-1 text-start mb-0">
                ${messageText}
            </p>
            <p class="text-end pe-2" style="font-size:12px;">${sentAt}</p>
        </div>`;
    chatMessages.insertAdjacentHTML("beforeend", messageToAppend);
    chatMessages.scrollTo({
        top: chatMessages.scrollHeight,
        behavior: 'smooth'
    });
});

chatHub.on("displayTheSentMessage", (messageText, sentAt) => {
    const chatMessages = document.getElementById("chatMessages");
    const messageToAppend = `
            <div class="w-100 bg-secondary rounded ms-1 mb-1" style="height:contain;--bs-bg-opacity: .5;">
                <span class="ps-1 w-100 d-block border-bottom" style="font-size:12px;">You</span>
                <p class="ps-1 text-start mb-0 pt-0 pb-0">
                    ${messageText}
                </p>
                <p class="text-end pe-2" style="font-size:12px;">${sentAt}</p>
            </div>`;
    chatMessages.insertAdjacentHTML("beforeend", messageToAppend);
    chatMessages.scrollTo({
        top: chatMessages.scrollHeight,
        behavior: 'smooth'
    });
});

let isTypingMessageDisplayed = false;
const messageInput = document.getElementById("messageInput");
messageInput.addEventListener("input", async () => {
    const receiverId = document.getElementById("receiverIdInput").value;
    if (messageInput.value.length > 0 && !isTypingMessageDisplayed) {
        const receiverConnected = await chatHub.invoke("DisplayTypingMessage", receiverId);
        if (receiverConnected) {
            isTypingMessageDisplayed = true;
        }
    } else if (messageInput.value.length === 0) {
        chatHub.send("HideTypingMessage", receiverId);
        isTypingMessageDisplayed = false;
    }
});

chatHub.on("displayTypingMessage", () => {
    const typingMessageSpan = document.getElementById("typingMessage");
    typingMessageSpan.removeAttribute("hidden");
});

chatHub.on("hideTypingMessage", () => {
    const typingMessageSpan = document.getElementById("typingMessage");
    typingMessageSpan.setAttribute("hidden", '');
});


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