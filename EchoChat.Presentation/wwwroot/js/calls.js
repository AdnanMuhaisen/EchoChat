const callConnection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/callHub")
    .build();

let receiverId;
let comingCallType;
const userId = document.getElementById('user-id').value;
const userName = document.getElementById('user-name').value;

const onAudioCallClick = async (event) => {
    let target = event.target;
    if (target.tagName.toLowerCase() === 'i') {
        target = target.closest('button');
    }

    const elementIdAsArray = target.id.split('-');
    const receiverId = elementIdAsArray[2];

    await callConnection.send('CallUser', receiverId, 'audio');
};

const onVideoCallClick = async (event) => {
    let target = event.target;
    if (target.tagName.toLowerCase() === 'i') {
        target = target.closest('button');
    }

    const elementIdAsArray = target.id.split('-');
    const receiverId = elementIdAsArray[2];

    await callConnection.send('CallUser', receiverId, 'video');
};

const displayComingCallNotification = (display = true) => {
    const comingCallNotification = document.getElementById('coming-call-notification');
    if (display) {
        comingCallNotification.classList.add('d-flex');
    } else {
        comingCallNotification.classList.remove('d-flex');
    }

}

callConnection.on('comingCall', (senderId, senderName, callType) => {
    receiverId = senderId;
    comingCallType = callType;
    displayComingCallNotification();
    const callerNameElement = document.getElementById('caller-name');
    callerNameElement.innerText = senderName;
});

document.getElementById('answer-coming-call').addEventListener('click', async () => {
    displayComingCallNotification(false);
    if (comingCallType === 'audio') {
        const receiverName = document.getElementById('caller-name').innerText;
        await startAudioCall(receiverId, receiverName);
    } else if (comingCallType === 'video') {
        await startVideoCall(receiverId);
    }
});

document.getElementById('close-coming-call').addEventListener('click', async () => {
    displayComingCallNotification(false);
    await callConnection.send('RejectComingCall', receiverId);
});

callConnection.on('rejectedCall', () => {
    const rejectedCallMessage = document.getElementById('rejected-call-message');
    rejectedCallMessage.removeAttribute('hidden');
    setTimeout(() => {
        rejectedCallMessage.setAttribute('hidden', '');
    }, 3000);
});

let localStream = null;
let remoteStream = null;
let peerConnection = null;

const callContainerElement = document.getElementById('call-container');
const audioElement = document.getElementById('remote-audio');
const localVideoElement = document.getElementById('local-video-element');
const remoteVideoElement = document.getElementById('remote-video-element');

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

const fetchUserMedia = async (isVideoCall = false) => {
    try {
        let stream = await navigator.mediaDevices.getUserMedia({
            audio: true,
            video: isVideoCall
        });
        if (isVideoCall) {
            localVideoElement.srcObject = stream;
        }

        localStream = stream;
    } catch (error) {
        console.error(error);
    }
};

const createPeerConnection = async (isVideoCall = false) => {
    peerConnection = await new RTCPeerConnection(configuration);
    remoteStream = new MediaStream();
    if (isVideoCall) {
        remoteVideoElement.srcObject = remoteStream;
    } else {
        audioElement.srcObject = remoteStream;
    }

    peerConnection.onicecandidate = async (event) => {
        console.log("candidate: ", event.candidate);
        console.log("receiver id: ", receiverId);
        if (event.candidate) {
            callConnection.send("SendIceCandidate", JSON.stringify(event.candidate), receiverId);
        }
    };

    peerConnection.ontrack = (event) => {
        event.streams[0].getTracks().forEach(track => {
            remoteStream.addTrack(track, remoteStream);
        });
    };

    localStream.getTracks().forEach((track) => {
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

const displayAudioCallContainer = (name) => {
    const audioCallUserName = document.getElementById('audio-call-user-name');
    audioCallUserName.innerText = `Audio Call: ${name}`;
    const audioCallElement = document.getElementById('audio-call');
    audioCallElement.classList.add('d-flex');
};

const displayVideoCallContainer = (display = true) => {
    const videoCallContainer = document.getElementById('video-call');
    if (display) {
        videoCallContainer.classList.add('d-flex');
    } else {
        videoCallContainer.classList.remove('d-flex');
    }
};

const hideAudioCallContainer = () => {
    const audioCallElement = document.getElementById('audio-call');
    audioCallElement.classList.remove('d-flex');
    document.getElementById('audio-call-user-name').value = '';
};

const startAudioCall = async (targetReceiverId, receiverName) => {
    receiverId = targetReceiverId;
    await fetchUserMedia();
    await createPeerConnection();
    const offer = await peerConnection.createOffer();
    await peerConnection.setLocalDescription(offer);
    await callConnection.send("SendOffer", JSON.stringify(offer), userId, receiverId, userName, false);
    displayAudioCallContainer(receiverName);
};

const startVideoCall = async (targetReceiverId) => {
    receiverId = targetReceiverId;
    await fetchUserMedia(true);
    await createPeerConnection(true);
    const offer = await peerConnection.createOffer();
    await peerConnection.setLocalDescription(offer);
    await callConnection.send("SendOffer", JSON.stringify(offer), userId, receiverId, userName, true);
    displayVideoCallContainer();
};

callConnection.on("receiveIceCandidate", (candidate) => {
    console.log(`received candedate: ${candidate}`);
    peerConnection.addIceCandidate(new RTCIceCandidate(JSON.parse(candidate)));
});

callConnection.on("receiveOffer", async (offer, senderId, senderName, isVideoCall) => {
    try {
        // The following code will be executed on the receiver's side to generate
        // the answer. Here, when I get the sender's ID, this sender is actually the receiver
        // based on the client that will execute this handler.
        receiverId = senderId;
        await fetchUserMedia(isVideoCall);
        await createPeerConnection(isVideoCall);
        await peerConnection.setRemoteDescription(new RTCSessionDescription(JSON.parse(offer)));
        const answer = await peerConnection.createAnswer();
        await peerConnection.setLocalDescription(answer);
        await callConnection.send('SendAnswer', JSON.stringify(answer), senderId);
        if (isVideoCall) {
            displayVideoCallContainer();
        } else {
            displayAudioCallContainer(senderName);
        }
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
    hideAudioCallContainer();
    await callConnection.send('EndCall', receiverId, false);
});

document.getElementById('close-video-call').addEventListener('click', async () => {
    peerConnection.close();
    displayVideoCallContainer(false);
    await callConnection.send('EndCall', receiverId, true);
});

callConnection.on('callEnded', (isVideoCall) => {
    peerConnection.close();
    if (isVideoCall) {
        displayVideoCallContainer(false);
    } else {
        hideAudioCallContainer();
    }
});

callConnection.on('callingOfflineUser', () => {
    hideAudioCallContainer();
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