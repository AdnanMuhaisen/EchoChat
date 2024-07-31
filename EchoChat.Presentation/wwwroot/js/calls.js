const callConnection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/callHub")
    .build();

let receiverId;
const userId = document.getElementById('user-id').value;
const userName = document.getElementById('user-name').value;

const onAudioCallClick = async (event) => {
    let target = event.target;
    if (target.tagName.toLowerCase() === 'i') {
        target = target.closest('button');
    }

    const elementIdAsArray = target.id.split('-');
    const receiverId = elementIdAsArray[2];
    const receiverName = elementIdAsArray[3];
    await startCall(receiverId, receiverName);
};

let localStream = null;
let remoteStream = null;
let peerConnection = null;

const callContainerElement = document.getElementById('call-container');
const audioElement = document.getElementById('remote-audio');

const configuration = {
    iceServers: [
        {
            urls: [
                'stun:stun.l.google.com:19302',
                'stun:stun1.l.google.com:19302'
            ]
        }
    ]
};

const fetchUserMedia = async () => {
    try {
        localStream = await navigator.mediaDevices.getUserMedia({ audio: true });
    } catch (error) {
        console.error(error);
    }
};

const createPeerConnection = async () => {
    peerConnection = await new RTCPeerConnection(configuration);
    remoteStream = new MediaStream();
    audioElement.srcObject = remoteStream;

    peerConnection.onicecandidate = async (event) => {
        console.log("candidate: ", event.candidate);
        console.log("receiver id: ", receiverId);
        if (event.candidate) {
            callConnection.send("SendIceCandidate", JSON.stringify(event.candidate), receiverId);
        }
    };

    peerConnection.ontrack = (event) => {
        event.streams[0].getTracks().forEach(track => {
            console.log('track from the remote user');
            remoteStream.addTrack(track, remoteStream);
        });
    };


    localStream.getTracks().forEach((track) => {
        console.log('add tracks to the local stream');
        peerConnection.addTrack(track, localStream);
    });

    peerConnection.onconnectionstatechange = (event) => {
        console.log("connection state changed: ", peerConnection.connectionState);
    };

    peerConnection.onsignalingstatechange = (e) => {
        console.log(e);
        console.log(peerConnection.signalingState);
    };
};

const displayAudioCallElement = (name) => {
    const audioCallUserName = document.getElementById('audio-call-user-name');
    audioCallUserName.innerText = `Audio Call: ${name}`;
    const audioCallElement = document.getElementById('audio-call');
    audioCallElement.classList.add('d-flex');
};

const hideAudioCallElement = () => {
    const audioCallElement = document.getElementById('audio-call');
    audioCallElement.classList.remove('d-flex');
    document.getElementById('audio-call-user-name').value = '';
};

const startCall = async (targetReceiverId, receiverName) => {
    receiverId = targetReceiverId;

    await fetchUserMedia();

    await createPeerConnection();

    const offer = await peerConnection.createOffer();

    await peerConnection.setLocalDescription(offer);

    console.log("send offer ", receiverId);

    await callConnection.send("SendOffer", JSON.stringify(offer), userId, receiverId, userName);

    displayAudioCallElement(receiverName);
}

callConnection.on("receiveIceCandidate", (candidate) => {
    console.log(`received candedate: ${candidate}`);
    peerConnection.addIceCandidate(new RTCIceCandidate(JSON.parse(candidate)));
});

callConnection.on("receiveOffer", async (offer, senderId, senderName) => {
    try {
        // The following code will be executed on the receiver's side to generate
        // the answer. Here, when I get the sender's ID, this sender is actually the receiver
        // based on the client that will execute this handler.
        receiverId = senderId;

        console.log("receiver id: ", receiverId);

        await fetchUserMedia();

        await createPeerConnection();

        await peerConnection.setRemoteDescription(new RTCSessionDescription(JSON.parse(offer)));

        const answer = await peerConnection.createAnswer();

        await peerConnection.setLocalDescription(answer);

        await callConnection.send('SendAnswer', JSON.stringify(answer), senderId);

        displayAudioCallElement(senderName);
    } catch (error) {
        console.error(error);
    }
});

callConnection.on("receiveAnswer", async (answer) => {
    try {
        console.log('received answer');
        await peerConnection.setRemoteDescription(new RTCSessionDescription(JSON.parse(answer)));
    } catch (error) {
        console.error(error);
    }
});

document.getElementById('close-audio-call').addEventListener('click', async () => {
    peerConnection.close();
    hideAudioCallElement();
    await callConnection.send('EndCall', receiverId);
});

callConnection.on('callEnded', () => {
    peerConnection.close();
    hideAudioCallElement();
});

callConnection.on('callingOfflineUser', () => {
    hideAudioCallElement();
    const offlineUserMessage = document.getElementById('offline-user-message');
    offlineUserMessage.removeAttribute('hidden');

    setTimeout(() => {
        offlineUserMessage.setAttribute('hidden', "");
    }, 2000);
});

callConnection
    .start()
    .then(
        () => console.log("call-hub: connected"),
        () => console.log("call-hub: failed to connect"));